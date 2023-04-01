
using CommandLine;
using Microsoft.Extensions.Logging;
using SoundsForAnno.Assetexport.Interfaces;
using SoundsForAnno.Serializable;
using SoundsForAnno.Transcription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SoundsForAnno.App
{
    public interface ISoundsForAnnoService
    {
        Task RunAsync(SoundsForAnnoOptions o);
    }

    public class SoundsForAnnoService : ISoundsForAnnoService
    {
        IAutoGuidingService _autoGuidingService;
        IMultiLanguageMapService _multiLanguageMapService;
        IAudioAssetExportService _audioAssetExportService;
        ITextAssetExportService _textAssetExportService;
        ITranscriptorService _transcriptorService;
        ILogger<SoundsForAnnoService> _logger; 

        public SoundsForAnnoService(
            IAutoGuidingService autoGuidingService,
            IMultiLanguageMapService multiLanguageMapService,
            IAudioAssetExportService audioExportService,
            ITextAssetExportService audioTextExportService,
            ITranscriptorService transcriptorService,
            ILogger<SoundsForAnnoService> logger
        ) 
        {
            _autoGuidingService = autoGuidingService;
            _multiLanguageMapService = multiLanguageMapService;
            _audioAssetExportService = audioExportService;
            _textAssetExportService = audioTextExportService;
            _transcriptorService = transcriptorService;
            _logger = logger; 
        }

        public async Task RunAsync(SoundsForAnnoOptions o)
        {
            _autoGuidingService.ConfigureStartGuid(o.StartGuid);

            if (o.SoundBankDocs is null || o.SoundBankDocs.Count() == 0)
            {
                _logger.LogError("No jsons to soundbanks were provided.");
                return; 
            }

            foreach (String s in o.SoundBankDocs)
            {
                var bnk = JsonSerializer.Deserialize<WWISEJsonObject>(File.ReadAllText(s));
                _multiLanguageMapService.AddSoundbankInfo(bnk);
            }

            var events = _multiLanguageMapService.GetEvents();
            _audioAssetExportService.AddAssets(events);
            var generated_audioassets = _audioAssetExportService.GetResult();
            generated_audioassets.Save(o.OutputFilename ?? "out.xml");
            
            if (o.SoundBanks is null || o.SoundBanks.Count() == 0 || !o.GenerateAudioTexts)
            {
                _logger.LogError("No soundbank files were provided. Cannot transcribe and generate audio texts from nothing.");
                return;
            }

            foreach (String bnk in o.SoundBanks)
                _transcriptorService.AddSoundbanks(bnk);
            await _transcriptorService.ProcessAsync(); 

            _textAssetExportService.ConfigureTexts(_transcriptorService.GetResult());
            _textAssetExportService.AddAssets(events);

            var generated_textassets = _textAssetExportService.GetResult();
            generated_textassets.Save("audiotexts.xml");
        }
    }
}
