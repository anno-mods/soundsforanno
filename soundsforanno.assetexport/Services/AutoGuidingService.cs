using SoundsForAnno.Assetexport.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoundsForAnno.Assetexport.Services
{
    public class AutoGuidingService : IAutoGuidingService
    {
        int Offset = 0;
        int StartGuid = 0;

        public AutoGuidingService() { }

        public int GiveGuid() {
            Offset++;
            return StartGuid + Offset -1;
        }

        public void ConfigureStartGuid(int start_guid) => StartGuid = start_guid;
    }
}
