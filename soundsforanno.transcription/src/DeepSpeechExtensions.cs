using DeepSpeechClient;
using DeepSpeechClient.Interfaces;
using NAudio.Wave;
using SoundsForAnno.Serializable;

namespace SoundsForAnno.Transcription
{
    public static class IDeepSpeechExtensions
    {
        public static String SpeechToText(this IDeepSpeech deepSpeech, String wave_filename) {

            var waveBuffer = new WaveBuffer(File.ReadAllBytes(wave_filename));
            String speechResult;
            using (var waveInfo = new WaveFileReader(wave_filename))
            {
                speechResult = deepSpeech.SpeechToText(waveBuffer.ShortBuffer,
                    Convert.ToUInt32(waveBuffer.MaxSize / 2));
            }
            waveBuffer.Clear();
            return speechResult;
        }
    }
}
