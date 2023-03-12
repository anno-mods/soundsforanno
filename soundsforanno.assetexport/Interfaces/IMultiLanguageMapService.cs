using SoundsForAnno.Serializable;
using System.Collections.Generic;

namespace SoundsForAnno.Assetexport.Interfaces
{
    public interface IMultiLanguageMapService
    {
        void AddSoundbankInfo(WWISEJsonObject soundbank);
        IEnumerable<MultiLanguageEvent> GetEvents();
    }
}
