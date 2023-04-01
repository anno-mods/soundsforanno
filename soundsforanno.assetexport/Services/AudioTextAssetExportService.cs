using Microsoft.Extensions.Logging;
using SoundsForAnno.Assetexport;
using SoundsForAnno.Assetexport.Interfaces;
using SoundsForAnno.Serializable;
using System.Collections.Generic;

namespace SoundsForAnno.Assetexport.Services
{
    public class AudioTextAssetExportService : XmlAssetExporterService, ITextAssetExportService
    {
        static readonly string audio_template = "res/audiotext.xml";

        private Dictionary<string, TextGroup> _transcriptions;
        IGuidMappingService _guidMappingService;
        ILocaFactory _locaFactory; 

        ILogger<AudioTextAssetExportService> _logger;

        public AudioTextAssetExportService(
            IAutoGuidingService autoGuiding,
            ILogger<AudioTextAssetExportService> logger,
            IGuidMappingService guidMapping, 
            ILocaFactory locaFactory) 
            : base(autoGuiding)
        {
            ConfigureTemplate(audio_template);
            _transcriptions = new();
            _guidMappingService = guidMapping;
            _logger = logger;
            _locaFactory = locaFactory;
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
            var name = $"AT_{ml_event.Name}_{ml_event.Id}";
            asset.SelectSingleNode("/Values/Standard/Name").InnerText = name;
            _logger.LogDebug($"[Adding AudioText Asset] Name: {name} | GUID: {guid} | Id: {ml_event.Id}");
            _doc.DocumentElement.AppendChild(asset);

            //add transcriptions to locatexts
            if(transcription.French != null)
                _locaFactory.Get(Language.fra).AddText(guid, transcription.French.Text);
            if (transcription.English != null)
                _locaFactory.Get(Language.eng).AddText(guid, transcription.English.Text);
            if (transcription.German != null)
                _locaFactory.Get(Language.ger).AddText(guid, transcription.German.Text);
        }
    }
}
