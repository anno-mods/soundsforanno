using SoundsForAnno.Serializable;
using System.Collections.Generic;
using System.Xml;

namespace SoundsForAnno.Assetexport.Interfaces
{
    public interface IExportService
    {
        XmlDocument GetResult(); 
    }

    public interface ITextAssetExportService : IExportService
    {
        void ConfigureTexts(Dictionary<string, TextGroup> texts);
        void AddAssets(IEnumerable<MultiLanguageEvent> events);
    }

    public interface IAudioAssetExportService : IExportService
    {
        void AddAssets(IEnumerable<MultiLanguageEvent> events);
    }
}
