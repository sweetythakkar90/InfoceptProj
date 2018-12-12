using IP.JobsAPI.Models;
using IP.JobsAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
namespace IP.JobsAPI.Services
{
    public class GlobalServiceMethods : IGlobalRepository
    {
        private ExceptionLogService eLogService;
        public GlobalServiceMethods()
        {
            eLogService = new ExceptionLogService();
        }
        public string CipherText(string objText, string convertTool)
        {
            string passPhrase = "Rajeev Biswas";
            string saltValue = "Vipin Gera";
            string initVector = "Infocept Pty Ltd.";
            string cipherText;
            if (convertTool == "E")
            {
                int keySize = 256;
                int passwordIterations = 03;
                string hashAlgorithm = "MD5";
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(objText);

                PasswordDeriveBytes password = new PasswordDeriveBytes
                (
                    passPhrase,
                    saltValueBytes,
                    hashAlgorithm,
                    passwordIterations
                );
                byte[] keyBytes = password.GetBytes(keySize / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor
                (
                keyBytes,
                initVectorBytes
                );
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream
                (
                    memoryStream,
                    encryptor,
                    CryptoStreamMode.Write
                );
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] cipherTextBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                cipherText = Convert.ToBase64String(cipherTextBytes);
                cipherText = HttpUtility.UrlEncode(cipherText);
            }
            else
            {
                int keySize = 256;
                int passwordIterations = 03;
                string hashAlgorithm = "MD5";
                try
                {
                    cipherText = HttpUtility.UrlDecode(objText);
                    byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                    byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
                    byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
                    PasswordDeriveBytes password = new PasswordDeriveBytes
                    (
                    passPhrase,
                    saltValueBytes,
                    hashAlgorithm,
                    passwordIterations
                    );
                    byte[] keyBytes = password.GetBytes(keySize / 8);
                    RijndaelManaged symmetricKey = new RijndaelManaged();
                    symmetricKey.Mode = CipherMode.CBC;
                    ICryptoTransform decryptor = symmetricKey.CreateDecryptor
                    (
                    keyBytes,
                    initVectorBytes
                    );
                    MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
                    CryptoStream cryptoStream = new CryptoStream
                    (
                    memoryStream,
                    decryptor,
                    CryptoStreamMode.Read
                    );
                    byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                    int decryptedByteCount = cryptoStream.Read
                    (
                    plainTextBytes,
                    0,
                    plainTextBytes.Length
                    );
                    memoryStream.Close();
                    cryptoStream.Close();
                    cipherText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                }
                catch (Exception ex)
                {
                    cipherText = "";
                }
            }
            return cipherText;
        }

        public void LogData(Exception ex)
        {
            ExceptionLog log = new ExceptionLog();
            log.UserID = 1;
            log.ApplicationName = "PMS";
            log.MachineName = Environment.MachineName;  // HttpContext.Current.Server.MachineName;
            log.ExceptionClassName = new StackTrace(ex).GetFrame(0).GetMethod().DeclaringType.Name.ToString();
            log.ExceptionMethodName = new StackTrace(ex).GetFrame(0).GetMethod().Name;

            log.ExceptionMessage = ex.Message;
            log.ExceptionStackTrace = ex.StackTrace;
           log.ServerName = Environment.MachineName;
            log.ExceptionType = "E";
            log.Url = "";
            log.ExceptionLoggingTime = DateTime.Now;

            eLogService.InsertExceptionLogDetailsAsync(log);
        }

    }
}
