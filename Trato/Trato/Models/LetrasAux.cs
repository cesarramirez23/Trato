using System;
using System.Text;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
namespace Trato
{
    public static class NombresAux
    {
        #region PERFIL NORMAL PACIENTE
        public const string v_log="log";
        public const string v_membre="membre"; 
        public const string v_letra="letra"; 
        public const string v_folio="folio"; 
        public const string v_perfGen= "perfGen";
        public const string v_perMed= "perfMed";
        public const string v_validar="validar";
        /// <summary>
        /// cuando los doctores solo tenian una especialidad
        /// </summary>
        public const string v_redmedica="medicos";
        /// <summary>
        /// los medicos pueden tener mas especialidades
        /// </summary>
        public const string v_redmedica2="medicos2";
        public const string v_serviciosmedicos = "servicios";
        public const string v_serviciosgenereales = "generales";
        public const string v_token="token";
        public const string v_citas = "citas";
        public const string v_Nota = "notaMed";
        /// <summary>
        /// Cita de notificacion
        /// </summary>
        public const string v_citaNot = "Notif";
        public const string v_filtro = "filtro";
        public const string v_filCita = "filtroCita";
        #endregion
        public const string BASE_URL = "https://tratoespecial.com/";

        #region ENCRYPT
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private const string initVector = "pemgail9uzpgzl88";
        // This constant is used to determine the keysize of the encryption algorithm
        private const int keysize = 256;
        //Encrypt
        public static string EncryptString(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }
        #endregion
    }
}
