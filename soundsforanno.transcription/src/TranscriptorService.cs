﻿using SoundsForAnno.Serializable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using DeepSpeechClient.Interfaces;

namespace SoundsForAnno.Transcription
{
    public class TranscriptorService : ITranscriptorService
    {
        private List<Soundbank> _loaded_banks;
        private Dictionary<string, TextGroup> _existing_texts;
        private Dictionary<string, string> _idMapping;

        private List<string> _bnk_files;

        private ILogger<IDeepSpeech> _logger;
        private IDeepSpeechFactory _deepSpeechFactory;

        bool is_finished = false;
        bool has_started = false;

        public TranscriptorService(ILogger<IDeepSpeech> logger, IDeepSpeechFactory deepSpeechFactory)
        {
            _loaded_banks = new List<Soundbank>();
            _bnk_files = new List<string>();
            _existing_texts = new();
            _idMapping = new();
            _logger = logger;
            _deepSpeechFactory = deepSpeechFactory;
        }

        public void AddSoundbanks(string bnk_file)
        {
            if (has_started)
                throw new InvalidOperationException("The processing has already started!");
            var jsonobject = JsonSerializer.Deserialize<WWISEJsonObject>(File.ReadAllText(Path.ChangeExtension(bnk_file, ".json")));

            foreach (var bnk in jsonobject.SoundBanksInfo.SoundBanks)
            {
                _loaded_banks.Add(bnk);
                _bnk_files.Add(bnk_file);

                foreach (var ev in bnk.IncludedEvents)
                {
                    var ids = ev.IncludedMemoryFiles?.Select(x => x.Id) ?? Enumerable.Empty<string>();
                    foreach (var id in ids)
                    {
                        _idMapping.Add(id, ev.Id);
                    }
                }
            }
        }

        public async Task ProcessAsync()
        {
            has_started = true;
            _logger.LogInformation("Started transcribing audio files... This may take a while depending on your computer");
            foreach (var bnk in _bnk_files)
            {
                await ProcessSingleBank(bnk);
            }
            _logger.LogInformation("transcription finished!");
            is_finished = true;
        }

        public Dictionary<string, TextGroup> GetResult()
        {
            if (!is_finished)
                throw new InvalidOperationException("await processing first!!!!");
            return _existing_texts;
        }

        private async Task ProcessSingleBank(string bnk_file)
        {
            await AudioTools.ExtractBankAsync(bnk_file);
            var directory = GetDirectoryOfExtractedBank(bnk_file);
            var wems = Directory.EnumerateFiles(directory, "*.wem").ToArray();

            foreach (var wem in wems)
            {
                var wave = Path.ChangeExtension(wem, ".wav");
                await AudioTools.ConvertWemToWavAsync(wem, wave);
                await ProcessWavAsync(wave);
            }
        }

        private async Task ProcessWavAsync(string wav)
        {
            var id = Path.GetFileNameWithoutExtension(wav);
            var text_language = GetLanguageOf(id);
            var textgroup = GetTextGroup(id);
            var tmp_wav = Path.GetTempFileName();
            await AudioTools.ReencodeWavAsync(wav, tmp_wav);
            File.Move(tmp_wav, wav, true);
            _logger.LogDebug($"Running STT: {id}...");
            var deepSpeech = _deepSpeechFactory.Get(text_language);
            var text = await Task.Run(() => deepSpeech.SpeechToText(wav));
            _logger.LogDebug($"STT finished: {id} | {text}");
            textgroup.ApplyText(text_language, text);
        }

        private Language GetLanguageOf(string sound_id)
        {
            var soundbank = _loaded_banks.Where(
                x => x.IncludedEvents.Any(
                    x => x.IncludedMemoryFiles.Any(y => y.Id == sound_id)
                ))
                .FirstOrDefault();
            if (soundbank is null)
                throw new InvalidOperationException($"{sound_id} does not exist in the loaded soundbanks");
            return soundbank.GetLanguageCode();
        }

        private TextGroup GetTextGroup(string id)
        {
            var mapped_id = GetEventId(id);
            var text = _existing_texts.GetValueOrDefault(mapped_id);
            if (text is null)
            {
                text = new TextGroup();
                _existing_texts.Add(mapped_id, text);
            }
            return text;
        }

        private string GetDirectoryOfExtractedBank(string bnk_file)
        {
            var fullpath = Path.GetFullPath(bnk_file);
            return Path.Combine(Path.GetDirectoryName(fullpath), Path.GetFileNameWithoutExtension(bnk_file));
        }

        private string GetEventId(string mem_file_id)
        {
            return _idMapping.GetValueOrDefault(mem_file_id);
        }
    }
}
