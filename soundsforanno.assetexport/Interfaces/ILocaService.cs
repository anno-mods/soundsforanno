using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SoundsForAnno.Assetexport.Interfaces
{
    public interface ILocaService
    {
        void AddText(String guid, String text);
        XmlDocument GetResult();
    }
}
