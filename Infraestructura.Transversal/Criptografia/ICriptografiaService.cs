using System;

namespace Sipecom.FactElec.Pymes.Infraestructura.Transversal.Criptografia
{
    public interface ICriptografiaService
    {
        string Encriptar(string data);

        string Encriptar(string data, string semilla);

        string Desencriptar(string dataEncriptada);

        string Desencriptar(string dataEncriptada, string semilla);

        string EncriptarClave(string clave);

        string EncriptarClave(string clave, string semilla);
    }
}
