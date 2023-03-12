using NAudio.Wave;
using SoundsForAnno.Assetexport.Interfaces;
using System.Collections.Generic;

namespace SoundsForAnno.Assetexport.Services
{
    public class GuidMappingService : IGuidMappingService
    {
        Dictionary<string, string> map; 

        public GuidMappingService() {
            map = new(); 
        }

        public void AddMapping(string wwise_id, string asset_guid)
        {
            map.Add(wwise_id, asset_guid);
        }

        public string GetGuid(string wwise_id)
        {
            return map.GetValueOrDefault(wwise_id);
        }
    }
}
