using System;
using System.Collections.Generic;
using System.Globalization;
using SoundsForAnno.Assetexport.Interfaces;
using SoundsForAnno.Exceptions;
using SoundsForAnno.Serializable;

namespace SoundsForAnno.Assetexport.Services
{
    public class MultiLanguageMapService : IMultiLanguageMapService
    {
        private List<MultiLanguageEvent> _events;
        private bool is_initialized = false;

        public MultiLanguageMapService() {
            _events = new List<MultiLanguageEvent>();
        }

        public void AddSoundbankInfo(WWISEJsonObject jsonobject)
        {
            if (!is_initialized)
            {
                Initialize(jsonobject);
                is_initialized = true;
                return;
            }
            Map(jsonobject);
        }

        public IEnumerable<MultiLanguageEvent> GetEvents() => _events;

        private void Initialize(WWISEJsonObject jsonobject)
        {
            if (jsonobject.SoundBanksInfo.SoundBanks.Length == 0)
                throw new ArgumentException("object does not contain any sound banks");

            foreach (Soundbank bank in jsonobject.SoundBanksInfo.SoundBanks)
                InitBank(bank);
        }

        private void Map(WWISEJsonObject jsonobject)
        {
            if (jsonobject.SoundBanksInfo.SoundBanks.Length == 0)
                throw new ArgumentException("object does not contain any sound banks");
            foreach (Soundbank bank in jsonobject.SoundBanksInfo.SoundBanks)
                MapBank(bank);
        }

        private void InitBank(Soundbank bank)
        {
            foreach (Event e in bank.IncludedEvents)
            {
                MultiLanguageEvent ml_event = new MultiLanguageEvent();
                ml_event.Id = e.Id;
                ml_event.Name = e.Name;
                var lang = bank.GetLanguageCode();
                ApplyToMlEvent(ml_event, lang, e);
                _events.Add(ml_event);
            }
        }

        private void MapBank(Soundbank bank)
        {
            foreach (Event e in bank.IncludedEvents)
            {
                MultiLanguageEvent ml_event = _events.Find(x => x.Id.Equals(e.Id));
                if (ml_event is null)
                    throw new FilesNotSimilarException();
                var lang = bank.GetLanguageCode();
                ApplyToMlEvent(ml_event, lang, e);
            }
        }

        private void ApplyToMlEvent(MultiLanguageEvent ml_event, Language lang, Event e)
        {

            Duration duration = CreateDurationFrom(e.DurationMin, e.DurationMax);
            ml_event.ApplyDuration(lang, duration);
        }

        private Duration CreateDurationFrom(String duration_min, String duration_max)
        {

            float parsed_min = float.Parse(duration_min, CultureInfo.InvariantCulture);
            float parsed_max = float.Parse(duration_max, CultureInfo.InvariantCulture);

            return new Duration {
                DurationMaximum = Math.Round(parsed_max * 1000).ToString(),
                DurationMinimum = Math.Round(parsed_min * 1000).ToString()
            };
        }
    }
}
