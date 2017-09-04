using System;
using System.Collections.Generic;
using System.Text;
using Service.Interfaces;
using DataAccessObject.Interfaces;

namespace Service.Implements
{
    public class ConfigService : IConfigService
    {
        private readonly IConfigRepository _configRepository;

        public ConfigService(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        public string GetValueByName(string ConfigName)
        {
            return _configRepository.GetValueByName(ConfigName);
        }
    }
}
