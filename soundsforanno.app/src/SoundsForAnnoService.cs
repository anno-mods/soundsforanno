
using CommandLine;
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

        public SoundsForAnnoService(
            IAutoGuidingService autoGuidingService,
            IMultiLanguageMapService multiLanguageMapService,
            IAudioAssetExportService audioExportService,
            ITextAssetExportService audioTextExportService,
            ITranscriptorService transcriptorService
        ) 
        {
            _autoGuidingService = autoGuidingService;
            _multiLanguageMapService = multiLanguageMapService;
            _audioAssetExportService = audioExportService;
            _textAssetExportService = audioTextExportService;
            _transcriptorService = transcriptorService;
        }

        public async Task RunAsync(SoundsForAnnoOptions o)
        {
            _autoGuidingService.ConfigureStartGuid(o.StartGuid);

            foreach (String s in o.SoundBankDocs)
            {
                var bnk = JsonSerializer.Deserialize<WWISEJsonObject>(File.ReadAllText(s));
                _multiLanguageMapService.AddSoundbankInfo(bnk);
            }
            var events = _multiLanguageMapService.GetEvents();
            _audioAssetExportService.AddAssets(events);
            var generated_audioassets = _audioAssetExportService.GetResult();
            generated_audioassets.Save(o.OutputFilename ?? "out.xml");

            if (!o.GenerateAudioTexts || o.SoundBanks.Count() == 0)
                return;

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
