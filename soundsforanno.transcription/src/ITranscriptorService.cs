using SoundsForAnno.Serializable;

namespace SoundsForAnno.Transcription
{
    public interface ITranscriptorService
    {
        void AddSoundbanks(string bnk_file);
        Task ProcessAsync();
        Dictionary<string, TextGroup> GetResult();
    }
}
