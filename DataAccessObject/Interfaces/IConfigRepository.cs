using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessObject.Interfaces
{
    public interface IConfigRepository
    {
        string GetValueByName(string Name);
    }
}
