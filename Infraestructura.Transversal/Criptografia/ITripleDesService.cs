using System;

namespace Sipecom.FactElec.Pymes.Infraestructura.Transversal.Criptografia
{
    public interface ITripleDesService : IDisposable
    {
        byte[] Key { get; set; }

        byte[] Encrypt(byte[] data);

        byte[] EncryptText(string data);

        byte[] Decrypt(byte[] data);

        string DecryptText(byte[] data);
    }
}
