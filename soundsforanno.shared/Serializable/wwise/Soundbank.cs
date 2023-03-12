using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SoundsForAnno.Serializable
{
    public class Soundbank
    {
        public String Id { get; set; }
        public String GUID { get; set; }
        public String Language { get; set; }
        public String ObjectPath { get; set; }
        public String ShortName { get; set; }
        public String Path { get; set; }
        public Event[] IncludedEvents { get; set; }
    }

    public static class SoundbankExtension
    {
        public static Language GetLanguageCode(this Soundbank bank)
        {
            switch (bank.Language)
            {
                case "de_de":
                    return Language.ger;
                case "fr_fr":
                    return Language.fra;
                default:
                    return Language.eng;
            }
        }
    }
}
