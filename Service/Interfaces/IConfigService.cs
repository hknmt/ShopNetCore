using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interfaces
{
    public interface IConfigService
    {
        string GetValueByName(string ConfigName);
    }
}
