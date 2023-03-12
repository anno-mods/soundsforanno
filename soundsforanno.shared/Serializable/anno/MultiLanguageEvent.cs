using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using System.Xml.Serialization;

namespace SoundsForAnno.Serializable
{
    public enum Language { 
        eng, ger, fra
    }

    [XmlRoot(ElementName ="DurationLanguageArray")]
    public class MultiLanguageEvent
    {
        [XmlIgnore]
        public string Id { get; set; }
        [XmlIgnore]
        public string Name { get; set; }

        [XmlElement("German")]
        public Duration DurationGer { get; set; }
        [XmlElement("English")]
        public Duration DurationEng { get; set; }
        [XmlElement("French")]
        public Duration DurationFr { get; set; }

        public void ApplyDuration(Language lang, Duration d)
        {
            switch (lang)
            { 
                case Language.eng:
                    DurationEng = d;
                    break;
                case Language.ger:
                    DurationGer = d;
                    break;
                case Language.fra:
                    DurationFr = d;
                    break;
                default: throw new ArgumentException("fuck");
            }
        }
    }
}
