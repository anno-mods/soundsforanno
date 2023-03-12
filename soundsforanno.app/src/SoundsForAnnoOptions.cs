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

        public String? OutputFilename { get; set; }
    }
}
