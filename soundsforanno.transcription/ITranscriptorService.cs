using SoundsForAnno.Serializable;

namespace SoundsForAnno.Transcription
{
    public interface ITranscriptorService
    {
        void AddSoundbanks(String bnk_file);
        Task ProcessAsync();
        Dictionary<String, TextGroup> GetResult();
    }
}
