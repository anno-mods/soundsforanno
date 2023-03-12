using Microsoft.Extensions.Logging;
using SoundsForAnno.Assetexport;
using SoundsForAnno.Assetexport.Interfaces;
using SoundsForAnno.Serializable;
using System.Collections.Generic;

namespace SoundsForAnno.Assetexport.Services
{
    public class AudioTextAssetExportService : XmlExporterService, ITextAssetExportService
    {
        static readonly string audio_template = "res/audiotext.xml";

        private Dictionary<string, TextGroup> _transcriptions;
        IGuidMappingService _guidMappingService;

        ILogger<AudioTextAssetExportService> _logger;

        public AudioTextAssetExportService(
            IAutoGuidingService autoGuiding,
            ILogger<AudioTextAssetExportService> logger,
            IGuidMappingService guidMapping) 
            : base(autoGuiding)
        {
            ConfigureTemplate(audio_template);
            _transcriptions = new();
            _guidMappingService = guidMapping;
            _logger = logger;
        }

        public void ConfigureTexts(Dictionary<string, TextGroup> texts) => _transcriptions = texts;

        public override void AddAsset(MultiLanguageEvent ml_event)
        {
            var asset = _doc.ImportNode(_template, true);
            var transcription = _transcriptions.GetValueOrDefault(ml_event.Id);
            if(transcription is null)
                return;
            var linked_guid = _guidMappingService.GetGuid(ml_event.Id);
            var loca_text = _doc.ImportNode(SerializeToXmlElement(transcription), true);

            asset.SelectSingleNode("/Values/AudioText/AudioAsset").InnerText = linked_guid;
            asset.SelectSingleNode("/Values/Text").AppendChild(loca_text);
            var guid = _autoGuiding.GiveGuid().ToString();
            asset.SelectSingleNode("/Values/Standard/GUID").InnerText = guid;
            asset.SelectSingleNode("/Values/Standard/Name").InnerText = $"AT_{ml_event.Name}_{ml_event.Id}";
            _logger.LogInformation($"Audio Text Exported: {ml_event.Name}\nGUID:{guid}\nId;{ml_event.Id}");
            _doc.DocumentElement.AppendChild(asset);
        }
    }
}
