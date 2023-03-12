using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundsForAnno.Assetexport.Interfaces
{
    public interface IGuidMappingService
    {
        void AddMapping(String wwise_id, String asset_guid);
        String GetGuid(String wwise_id);
    }
}
