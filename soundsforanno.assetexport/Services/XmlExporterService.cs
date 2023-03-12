using SoundsForAnno.Assetexport.Interfaces;
using SoundsForAnno.Assetexport;
using SoundsForAnno.Serializable;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace SoundsForAnno.Assetexport.Services
{
    public abstract class XmlExporterService
    {
        protected XmlDocument _doc;
        protected XmlNode _template;

        protected IAutoGuidingService _autoGuiding;

        public XmlExporterService(IAutoGuidingService autoGuiding)
        {
            _doc = new XmlDocument();
            var root = _doc.CreateElement("Assets");
            _doc.AppendChild(root);

            _autoGuiding = autoGuiding;
        }

        public void AddAssets(IEnumerable<MultiLanguageEvent> events)
        {
            foreach (var x in events)
            {
                AddAsset(x);
            }
        }

        protected void ConfigureTemplate(String template_filename)
        {
            var template_doc = new XmlDocument();
            template_doc.Load(template_filename);
            _template = template_doc.DocumentElement;
        }

        public XmlDocument GetResult() => _doc;

        public abstract void AddAsset(MultiLanguageEvent e);

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
