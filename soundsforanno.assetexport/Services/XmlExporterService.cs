using SoundsForAnno.Assetexport.Interfaces;
using SoundsForAnno.Assetexport;
using SoundsForAnno.Serializable;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace SoundsForAnno.Assetexport.Services
{
    public abstract class XmlAssetExporterService : XmlExporterService
    {
        protected IAutoGuidingService _autoGuiding;
        public XmlAssetExporterService(IAutoGuidingService autoGuiding) : base()
        {
            _autoGuiding = autoGuiding;
        }

        public void AddAssets(IEnumerable<MultiLanguageEvent> events)
        {
            foreach (var x in events)
            {
                AddAsset(x);
            }
        }
        public abstract void AddAsset(MultiLanguageEvent e);

    }
    public abstract class XmlExporterService
    {
        protected XmlDocument _doc;
        protected XmlNode _template;

        public XmlExporterService()
        {
            _doc = new XmlDocument();
            var root = _doc.CreateElement("Assets");
            _doc.AppendChild(root);
        }

        protected void ConfigureTemplate(String template_filename)
        {
            var loadFrom = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, template_filename);
            var template_doc = new XmlDocument();
            template_doc.Load(loadFrom);
            _template = template_doc.DocumentElement;
        }

        public XmlDocument GetResult() => _doc;

        public XmlNode SerializeToXmlElement(object o)
        {
            XmlDocument tmpdoc = new XmlDocument();
            using (XmlWriter writer = tmpdoc.CreateNavigator().AppendChild())
            {
                var serializer = new XmlSerializer(o.GetType());
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                serializer.Serialize(writer, o, ns);
            }
            return tmpdoc.DocumentElement;
        }
    }
}
