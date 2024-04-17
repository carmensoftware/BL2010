using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Security;
using System.Security.Cryptography;
using System.Text;

public partial class UserDefinedFunctions
{
    //[Serializable]
    //[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native)]
    //public string EnDeCryptor 
    //{
    //    EnCrypt = "En",
    //    DeCrypt = "De"
    //}
    /// <summary>
    /// This function is used to encrypt or decrypt the string by using MD5
    /// CryptoService and Triple DES CryptoService
    /// </summary>
    /// <param name="OriginalString"></param>
    /// <param name="Encrypt"></param>
    /// <returns></returns>
    [Microsoft.SqlServer.Server.SqlFunction]
    public static string EnDecryptString(string OriginalString, string Encrypt)
    {
        string Key                          = null;
        string EncryptString                = null;
        string DecryptString                = null;
        byte[] HashKey                      = null;
        byte[] buff                         = null;
        MD5CryptoServiceProvider HashMD5    = null;
        TripleDESCryptoServiceProvider Des3 = null;

        if (OriginalString != null || OriginalString.Length != 0)
        {
            HashMD5     = new MD5CryptoServiceProvider();
            Key         = "BinEnPassCoder";
            HashKey     = HashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(Key));
            Des3        = new TripleDESCryptoServiceProvider();
            Des3.Key    = HashKey;
            Des3.Mode   = CipherMode.ECB;

            if (Encrypt == "En")
            {
                //CONVERT TO ARRAY BYTE
                buff = ASCIIEncoding.ASCII.GetBytes(OriginalString);

                //ENCRYPT
                EncryptString       = Convert.ToBase64String(Des3.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length));
                string renString    = BinToHex(EncryptString);

                return renString;
            }
            else
            {
                string sTmp     = HexToStr(OriginalString);
                buff            = Convert.FromBase64String(sTmp);
                DecryptString   = ASCIIEncoding.ASCII.GetString(Des3.CreateDecryptor().TransformFinalBlock(buff, 0, buff.Length));

                return DecryptString;
            }
        }
        else
        {
            return OriginalString;
        }
    }

    public static string BinToHex(string StrVal)
    {
        string StrRetVal    = "";
        int nLength         = StrVal.Length;

        for (int i = 0; i < nLength; i++)
        {
            string StrTmp   = Uri.HexEscape(StrVal[i]);
            StrRetVal       += StrTmp.Substring(1);
        }
        return StrRetVal;
    }

    public static string HexToStr(string StrHex)
    {
        string StrRet   = "";
        int nCount      = StrHex.Length / 2;
        int iIdx        = 0;

        for (int i = 0; i < nCount; i++)
        {
            iIdx            = 0;
            string strTemp  = "%" + StrHex.Substring(i * 2, 2);
            StrRet          += Uri.HexUnescape(strTemp, ref iIdx);
        }
        return StrRet;
    }
}
