using SoundsForAnno.Serializable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SoundsForAnno.Serializable
{
    [XmlRoot("LocaText")]
    public class TextGroup
    {
        [XmlElement("English")]
        public LocaText? English { get; set; }
        [XmlElement("German")]
        public LocaText? German { get; set; }
        [XmlElement("French")]
        public LocaText? French { get; set; }

        public TextGroup() { }

        public void ApplyText(Language lang, String Text)
        {
            switch (lang)
            {
                case Language.ger:
                    German = new LocaText(Text);
                    break;
                case Language.fra:
                    French = new LocaText(Text);
                    break;
                default: 
                    English = new LocaText(Text);
                    break;
            }
        }
    }

    public class LocaText
    {
        [XmlElement("Text")]
        public String Text { get; set; }
        public LocaText(String s)
        {
            Text = s; 
        }
        public LocaText() { }
    }
}
