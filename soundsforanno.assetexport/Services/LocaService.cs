using SoundsForAnno.Assetexport.Interfaces;
using SoundsForAnno.Assetexport.Services;
using SoundsForAnno.Serializable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundsForAnno.Assetexport.Services
{
    public class LocaService : XmlExporterService, ILocaService
    {
        static readonly string text_template = "res/text.xml";

        public LocaService()
        {
            ConfigureTemplate(text_template);
        }

        public void AddText(string guid, string locatext)
        {
            var text = _doc.ImportNode(_template, true);
            text.SelectSingleNode("/Text").InnerText = locatext;
            text.SelectSingleNode("/GUID").InnerText = guid;
            _doc.DocumentElement.AppendChild(text);
        }
    }
}
