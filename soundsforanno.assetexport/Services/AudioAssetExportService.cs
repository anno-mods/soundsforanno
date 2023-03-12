using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using SoundsForAnno.Assetexport.Interfaces;
using SoundsForAnno.Serializable;

namespace SoundsForAnno.Assetexport.Services
{
    public class AudioAssetExportService : XmlExporterService, IAudioAssetExportService
    {
        static readonly string audio_template = "res/audio.xml";

        IGuidMappingService _guidMappingService;
        ILogger<AudioAssetExportService> _logger;

        public AudioAssetExportService(
            IAutoGuidingService autoGuiding,
            ILogger<AudioAssetExportService> logger,
            IGuidMappingService guidMapping) 
            : base(autoGuiding)
        {
            _guidMappingService = guidMapping;
            _logger = logger;
            ConfigureTemplate(audio_template);
        }

        public override void AddAsset(MultiLanguageEvent ml_event) {
            var audio_asset = _doc.ImportNode(_template, true);
            var dur_lang_array = _doc.ImportNode(SerializeToXmlElement(ml_event), true);
            audio_asset.SelectSingleNode("/Values/Audio").AppendChild(dur_lang_array);

            var guid = _autoGuiding.GiveGuid().ToString();
            audio_asset.SelectSingleNode("/Values/Standard/GUID").InnerText = guid;
            audio_asset.SelectSingleNode("/Values/Standard/Name").InnerText = ml_event.Name;
            audio_asset.SelectSingleNode("/Values/WwiseStandard/WwiseID").InnerText = ml_event.Id;
            _guidMappingService.AddMapping(ml_event.Id, guid);
            _logger.LogInformation($"Name: {ml_event.Name}\nGUID:{guid}\nId;{ml_event.Id}");
            _doc.DocumentElement.AppendChild(audio_asset);
        }
    }
    
}
