using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SoundsForAnno.Serializable
{
    public class RootPaths
    {
        public String ProjectRoot { get; set; }
        public String SourceFilesRoot { get; set; }
        public String SoundBanksRoot { get; set; }
        public String ExternalSourcesInputFile { get; set; }
        public String ExternalSourcesOutputRoot { get; set; }
    }
}
