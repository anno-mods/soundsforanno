using DeepSpeechClient;
using DeepSpeechClient.Interfaces;
using NAudio.Wave;
using SoundsForAnno.Serializable;

namespace SoundsForAnno.Transcription
{
    public class DeepSpeechAccess : IDisposable 
    {
        IDeepSpeech _sttClient;

        public DeepSpeechAccess() 
        {
            _sttClient = new DeepSpeech("lib/english.pbmm");
            _sttClient.EnableExternalScorer("lib/english.scorer");
        }

        public String SpeechToText(String wave_filename, Language lang = Language.eng)
        {
            var waveBuffer = new WaveBuffer(File.ReadAllBytes(wave_filename));
            String speechResult;
            using (var waveInfo = new WaveFileReader(wave_filename))
            {
                speechResult = _sttClient.SpeechToText(waveBuffer.ShortBuffer,
                    Convert.ToUInt32(waveBuffer.MaxSize / 2));
            }
            waveBuffer.Clear();
            return speechResult;
        }

        public void Dispose()
        {
            _sttClient.Dispose();
        }
    }
}
