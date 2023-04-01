using SoundsForAnno.Assetexport.Interfaces;
using SoundsForAnno.Serializable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundsForAnno.Assetexport.Services
{
    public class LocaFactory : ILocaFactory
    {
        private Dictionary<Language, ILocaService> _services;

        public LocaFactory() {
            _services = new();
            _services.Add(Language.eng, new LocaService());
            _services.Add(Language.ger, new LocaService());
            _services.Add(Language.fra, new LocaService());
        }

        public ILocaService Get(Language lang) => _services[lang];

    }
}
