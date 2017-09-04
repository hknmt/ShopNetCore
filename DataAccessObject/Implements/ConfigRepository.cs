using System;
using System.Collections.Generic;
using System.Text;
using BusinessObject;
using DataAccessObject.Interfaces;
using System.Linq;

namespace DataAccessObject.Implements
{
    public class ConfigRepository : IConfigRepository
    {
        private readonly BanHangDbContext _context;

        public ConfigRepository(BanHangDbContext context)
        {
            _context = context;
        }

        public string GetValueByName(string Name)
        {
            var configName = _context.ConfigShop.Where(x => x.ConfigName == Name).FirstOrDefault();

            if(configName != null)
                return configName.ConfigValue;

            return string.Empty;
        }
    }
}
