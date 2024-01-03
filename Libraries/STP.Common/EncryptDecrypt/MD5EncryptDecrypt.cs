using System;
using System.Security.Cryptography;
using System.Text;

namespace STP.Common.EncryptDecrypt
{
    public class MD5EncryptDecrypt
    {
        const string sKey = "wUoLiB82K4We2";

        public MD5EncryptDecrypt()
        {

        }

        #region Encrypt Details
        /// <summary>
        /// For Encrypt Password
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public static string EncryptDetails(string detail)
        {
            try
            {
                string sEncrypted;
                byte[] aPwdhash, aBuff;

                TripleDESCryptoServiceProvider oDes;
                MD5CryptoServiceProvider oHashmd5;

                oHashmd5 = new MD5CryptoServiceProvider();
                oDes = new TripleDESCryptoServiceProvider();

                aBuff = ASCIIEncoding.ASCII.GetBytes(detail);

                // Genarate Password Hash
                aPwdhash = oHashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(sKey));
                oHashmd5 = null;

                // The key is the secret password hash
                oDes.Key = aPwdhash;

                // CBC, CFB
                oDes.Mode = CipherMode.ECB;

                // Encrypted Password
                sEncrypted = Convert.ToBase64String(oDes.CreateEncryptor().TransformFinalBlock(aBuff, 0, aBuff.Length));

                return sEncrypted;
            }

            catch (System.Exception)
            {
                return "";
            }
        }
        #endregion

        #region for Decrypt Details
        /// <summary>
        /// Decrypt Pasword
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        public static string DecryptDetails(string details)
        {
            try
            {
                string sDecrypted;
                byte[] aPwdhash, aBuff;
                TripleDESCryptoServiceProvider oDes;
                MD5CryptoServiceProvider oHashmd5;

                oHashmd5 = new MD5CryptoServiceProvider();
                oDes = new TripleDESCryptoServiceProvider();

                // Genarate Password Hash
                aPwdhash = oHashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(sKey));
                oHashmd5 = null;

                // The key is the secret password hash
                oDes.Key = aPwdhash;

                // CBC, CFB
                oDes.Mode = CipherMode.ECB;
                aBuff = Convert.FromBase64String(details);

                // Decrypt
                sDecrypted = ASCIIEncoding.ASCII.GetString(oDes.CreateDecryptor().TransformFinalBlock(aBuff, 0, aBuff.Length));

                return sDecrypted;
            }
            
            catch (System.Exception)
            {
                return "";
            }
        }
        #endregion
    }
}
