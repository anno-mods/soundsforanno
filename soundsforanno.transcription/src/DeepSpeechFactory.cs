using DeepSpeechClient;
using DeepSpeechClient.Interfaces;
using Microsoft.Extensions.Logging;
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

        ILogger<DeepSpeechFactory> _logger; 

        public DeepSpeechFactory(ILogger<DeepSpeechFactory> logger) 
        { 
            _logger = logger;
            _clients = new Dictionary<Language, IDeepSpeech>();

            TryConfigure(Language.eng, "lib/english");
            TryConfigure(Language.ger, "lib/german");
            TryConfigure(Language.fra, "lib/french");
        }

        private void TryConfigure(Language lang, String file_base)
        {
            var base_model_name = $"{file_base}.pbmm";
            if (!File.Exists(base_model_name))
            {
                _logger.LogInformation($"No language model was found for {lang} under {base_model_name}, thus no deep speech transcription could be configured for the language");
                return;
            }
            var client = new DeepSpeech(base_model_name);

            var scorer_name = $"{file_base}.scorer";
            if (File.Exists(scorer_name))
                client.EnableExternalScorer(scorer_name);
            else
                _logger.LogInformation($"No scorer was found for {lang} under {scorer_name}. Transcription results might be not be optimal.");

            _clients.Add(lang, client);
        }

        public IDeepSpeech Get(Language lang) => _clients[lang];
    }
}
