using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace CloudPlatform.Code
{
    public class cls_EncryptionUntie
    {
        /// <summary>
        /// AES加密字符串
        /// </summary>
        /// <param name="Text">原始字符串</param>
        /// <returns>加密后字符串</returns>
        public string Encryption(string Text)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(Text);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// AES解密字符串
        /// </summary>
        /// <param name="Text">加密后字符串</param>
        /// <returns>解密后字符串</returns>
        public string Decryption(string Text)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            byte[] toEncryptArray = Convert.FromBase64String(Text);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);

        }

        /// <summary>
        /// DEC 加密过程
        /// </summary>
        /// <param name="pToEncrypt">被加密的字符串</param>
        /// <returns>返回被加密的字符串</returns>
        public string DEC_Encrypt(string pToEncrypt, string Key = "abcdEFgh")
        {
            //访问数据加密标准(DES)算法的加密服务提供程序 (CSP) 版本的包装对象
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = Encoding.ASCII.GetBytes(Key);　//建立加密对象的密钥和偏移量
            des.IV = Encoding.ASCII.GetBytes(Key);　 //原文使用ASCIIEncoding.ASCII方法的GetBytes方法

            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);//把字符串放到byte数组中

            MemoryStream ms = new MemoryStream();//创建其支持存储区为内存的流　
            //定义将数据流链接到加密转换的流
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //上面已经完成了把加密后的结果放到内存中去

            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        /// <summary>
        /// DEC 解密过程
        /// </summary>
        /// <param name="pToDecrypt">被解密的字符串</param>
        /// <returns>返回被解密的字符串</returns>
        public string DEC_Decrypt(string pToDecrypt, string Key = "abcdEFgh")
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = Encoding.ASCII.GetBytes(Key);　//建立加密对象的密钥和偏移量，此值重要，不能修改
            des.IV = Encoding.ASCII.GetBytes(Key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            //建立StringBuild对象，createDecrypt使用的是流对象，必须把解密后的文本变成流对象
            StringBuilder ret = new StringBuilder();

            return Encoding.Default.GetString(ms.ToArray());
        }
    }
}