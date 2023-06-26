using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindJobs.Domain.Services
{
    public interface IEncryption
    {
        string Encrypt(string clearText, string key);
        string Decrypt(string cipherText, string key);

    }
}
