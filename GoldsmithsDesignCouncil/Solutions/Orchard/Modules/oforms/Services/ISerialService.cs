using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orchard;

namespace oforms.Services
{
    public interface ISerialService : IDependency {
        void SaveSerialToFile(string value);
        bool ValidateSerial();
        string ReadSerialFromFile();
    }
}
