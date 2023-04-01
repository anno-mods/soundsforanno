using DeepSpeechClient.Interfaces;
using SoundsForAnno.Serializable;
using SoundsForAnno.Transcription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundsForAnno.Transcription
{
    public interface IDeepSpeechFactory
    {
        IDeepSpeech Get(Language lang);
    }
}
