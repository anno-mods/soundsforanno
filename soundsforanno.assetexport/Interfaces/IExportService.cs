using SoundsForAnno.Serializable;
using System.Collections.Generic;
using System.Xml;

namespace SoundsForAnno.Assetexport.Interfaces
{
    public interface IExportService
    {
        void AddAssets(IEnumerable<MultiLanguageEvent> events);
        XmlDocument GetResult(); 
    }

    public interface ITextAssetExportService : IExportService
    {
        void ConfigureTexts(Dictionary<string, TextGroup> texts);
    }

    public interface IAudioAssetExportService : IExportService
    { }
}
