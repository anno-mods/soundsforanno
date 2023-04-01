using DeepSpeechClient;
using DeepSpeechClient.Interfaces;
using SoundsForAnno.Serializable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundsForAnno.Transcription
{
    public class DeepSpeechFactory : IDeepSpeechFactory
    {
        private Dictionary<Language, IDeepSpeech> _clients; 

        public DeepSpeechFactory() { 
            _clients = new Dictionary<Language, IDeepSpeech>();

            TryConfigure(Language.eng, "lib/english");
            TryConfigure(Language.ger, "lib/german");
            TryConfigure(Language.ger, "lib/french");
        }

        private void TryConfigure(Language lang, String file_base)
        {
            var base_model_name = $"{file_base}.pbmm";
            if (!File.Exists(base_model_name))
                return;
            var client = new DeepSpeech(base_model_name);

            var scorer_name = $"{file_base}.scorer";
            if (File.Exists(scorer_name))
                client.EnableExternalScorer(scorer_name);

            _clients.Add(lang, client);

        }

        public IDeepSpeech Get(Language lang) => _clients[lang];
    }
}
