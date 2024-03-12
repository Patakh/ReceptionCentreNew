using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace ReceptionCentreNew
{
    public class CRPassword
    {
        private static string keyCrypt = "electron$_key^";
        /// /// Шифрует строку value
        /// 
        /// Строка которую необходимо зашифровать
        /// Ключ шифрования
        public static string Encrypt(string str)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(str), keyCrypt));
        }

        /// /// Расшифровывает данные из строки value
        /// 
        /// Зашифрованая строка
        /// Ключ шифрования
        /// Возвращает null, если прочесть данные не удалось
        [DebuggerNonUserCode]
        public static string Decrypt(string str)
        {
            string Result;
            try
            {
                CryptoStream Cs = InternalDecrypt(Convert.FromBase64String(str), keyCrypt);
                StreamReader Sr = new(Cs);

                Result = Sr.ReadToEnd();

                Cs.Close();
                Cs.Dispose();

                Sr.Close();
                Sr.Dispose();
            }
            catch (CryptographicException)
            {
                return null;
            }

            return Result;
        }

        private static byte[] Encrypt(byte[] key, string value)
        {
            SymmetricAlgorithm Sa = Rijndael.Create();
            ICryptoTransform Ct = Sa.CreateEncryptor(new PasswordDeriveBytes(value, null).GetBytes(16), new byte[16]);

            MemoryStream Ms = new MemoryStream();
            CryptoStream Cs = new CryptoStream(Ms, Ct, CryptoStreamMode.Write);

            Cs.Write(key, 0, key.Length);
            Cs.FlushFinalBlock();

            byte[] Result = Ms.ToArray();

            Ms.Close();
            Ms.Dispose();

            Cs.Close();
            Cs.Dispose();

            Ct.Dispose();

            return Result;
        }

        private static CryptoStream InternalDecrypt(byte[] key, string value)
        {
            SymmetricAlgorithm sa = Rijndael.Create();
            PasswordDeriveBytes passwordDeriveBytes = new(value, key);
            ICryptoTransform ct = sa.CreateEncryptor(key, passwordDeriveBytes.GetBytes(16));

            MemoryStream ms = new(key);
            return new CryptoStream(ms, ct, CryptoStreamMode.Read);
        }

        #region Jitsi
        public static byte[] Jitsi_encode(String text)
        {
            byte[] arr = Encoding.Default.GetBytes(text);
            byte[] keyarr = Encoding.Default.GetBytes(ConfigurationManager.AppSettings["CryptKey_Call"]);
            byte[] result = new byte[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                result[i] = (byte)(arr[i] ^ keyarr[i % keyarr.Length]);
            }
            return result;
        }
        #endregion
    }
}