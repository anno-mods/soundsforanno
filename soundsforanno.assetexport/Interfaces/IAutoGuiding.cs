using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundsForAnno.Assetexport.Interfaces
{
    public interface IAutoGuidingService
    {
        int GiveGuid();
        void ConfigureStartGuid(int guid);
    }
}
