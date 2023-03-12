using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SoundsForAnno.Serializable
{
    public class SoundbanksInfo
    {
        public String Platform { get; set; }
        public String BasePlatform { get; set; }
        public String SchemaVersion { get; set; }
        public String SoundbankVersion { get; set; }
        public RootPaths RootPaths { get; set; }
        public Soundbank[] SoundBanks { get; set; }
    }
}
