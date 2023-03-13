using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundsForAnno.App
{
    [Verb("gen")]
    public class SoundsForAnnoOptions
    {
        [Option('f', "files", Required = true)]
        public IEnumerable<string>? SoundBankDocs { get; set; }

        [Option('s', "soundbanks", Required = false)]
        public IEnumerable<string>? SoundBanks { get; set; }

        [Option('a', "audiotexts", Required = false, Default = false)]
        public bool GenerateAudioTexts { get; set; }

        [Option('g', "start-guid", Required = true)]
        public int StartGuid { get; set; }

        [Option('o', "audio-output", Required = false, Default = "audio.xml")]
        public String? OutputFilename { get; set; }

        [Option('t', "text-output", Required = false, Default = "audiotexts.xml")]
        public String? AudioTextFilename { get; set; }
    }
}
