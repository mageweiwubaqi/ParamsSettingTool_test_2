using System;
using System.Collections.Generic;
using System.Text;
using ITL.Public;
using ITL.Framework;
using System.Security.Cryptography;
using System.IO;

namespace ITL.ParamsSettingTool
{
    public static class KeyMacOperate
    {
        public const string TRANSPORT_KEY = "C9EEDBDACDFAC1FA"; //传输密钥，“深圳旺龙”的Hex码
        public const string DEFAULT_SYSTEM_ENCRY_PSD = "0E2DC5B5B504F3DD";  //8个0默认密码进行DES加密后的密文
        public const string DEFAULT_SYSTEM_PSD = "00000000";  //8个0默认密码

        public static string GetEncryKey(string psd)
        {
            string hexPsd = StrUtils.ASCIIStrToHexStr(psd);
            return TripleDESIntf.EncryHexStr_DESCS(hexPsd, TRANSPORT_KEY);
        }

        public static string GetDecryKey()
        {
            string strDecryHexStr_DESCS = TripleDESIntf.DecryHexStr_DESCS(AppEnv.Singleton.SystemPsd, TRANSPORT_KEY);



            return strDecryHexStr_DESCS;
        }

        //public static string GetMac(string hexContent)
        //{
        //    string hexKey = KeyMacOperate.GetDecryKey();
        //    string hexIv = "0000000000000000";
        //    string hexPadding = "8000000000000000";
        //    int hexLen = hexContent.Length;
        //    String afterCoverStr = hexContent + hexPadding.Substring(0, 16 - hexLen % 16);
        //    int afterCoverLen = afterCoverStr.Length;
        //    int mabLen = afterCoverLen / 16;
        //    String[] mab = new String[mabLen];

        //    for (int i = 0; i < mabLen; i++)
        //    {
        //        mab[i] = afterCoverStr.Substring(i * 16, 16);
        //    }
        //    string leftKey = hexKey.Substring(0, 16);

        //    EcbCipherMode ecbCipherMode = new EcbCipherMode(hex2Bytes(hexIv));
        //    DesCipher desCipherLeft = new DesCipher(hex2Bytes(leftKey), ecbCipherMode, null);
        //    TripleDesCipher desCipher3 = new TripleDesCipher(hex2Bytes(hexKey), ecbCipherMode, null);
        //    String I = xor(hexIv, mab[0]);
        //    String O = string.Empty;
        //    for (int i = 1; i < mabLen; i++)
        //    {
        //        byte[] ob = new byte[8];
        //        desCipherLeft.EncryptBlock(hex2Bytes(I), 0, I.Length, ob, 0);
        //        O = bytes2Hex(ob);
        //        I = xor(mab[i], O);
        //    }
        //    return bytes2Hex(desCipher3.Encrypt(hex2Bytes(I)));
        //}

        //private static string xor(string s1, string s2)
        //{
        //    byte[] s1Bytes = hex2Bytes(s1);
        //    byte[] s2Bytes = hex2Bytes(s2);
        //    List<byte> xorList = new List<byte>();
        //    for (int i = 0; i < s1Bytes.Length; i++)
        //    {
        //        xorList.Add(Convert.ToByte(s1Bytes[i] ^ s2Bytes[i]));
        //    }
        //    return bytes2Hex(xorList.ToArray());
        //}

        //private static String bytes2Hex(byte[] data)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    for (int i = 1; i <= data.Length; i++)
        //    {
        //        sb.Append(string.Format("{0:X2}", data[i - 1]));

        //    }
        //    return sb.ToString();
        //}


        //private static byte[] hex2Bytes(string mHex) // 返回十六进制代表的字符串 
        //{
        //    mHex = mHex.Replace(" ", "");
        //    if (mHex.Length <= 0) return null;
        //    byte[] vBytes = new byte[mHex.Length / 2];
        //    for (int i = 0; i < mHex.Length; i += 2)
        //        if (!byte.TryParse(mHex.Substring(i, 2), System.Globalization.NumberStyles.HexNumber, null, out vBytes[i / 2]))
        //            vBytes[i / 2] = 0;
        //    return vBytes;
        //}

        /// <summary>  
        /// MAC计算所要采用的CBC DES算法实现加密  
        /// </summary>  
        /// <param name="key">Key数据</param>  
        /// <param name="data">原数据</param>  
        /// <returns>返回加密后结果</returns>  
        public static byte[] HCDES_Encrypt(byte[] key, byte[] data)
        {
            try
            {
                //创建一个DES算法的加密类  
                DESCryptoServiceProvider MyServiceProvider = new DESCryptoServiceProvider();
                MyServiceProvider.Mode = CipherMode.CBC;
                MyServiceProvider.Padding = PaddingMode.None;
                //从DES算法的加密类对象的CreateEncryptor方法,创建一个加密转换接口对象  
                //第一个参数的含义是：对称算法的机密密钥(长度为64位,也就是8个字节)  
                // 可以人工输入,也可以随机生成方法是：MyServiceProvider.GenerateKey();  
                //第二个参数的含义是：对称算法的初始化向量(长度为64位,也就是8个字节)  
                // 可以人工输入,也可以随机生成方法是：MyServiceProvider.GenerateIV()  
                //创建加密对象  
                ICryptoTransform MyTransform = MyServiceProvider.CreateEncryptor(key, new byte[8]);
                //CryptoStream对象的作用是将数据流连接到加密转换的流  
                using (MemoryStream ms = new MemoryStream())
                {
                    CryptoStream MyCryptoStream = new CryptoStream(ms, MyTransform, CryptoStreamMode.Write);
                    //将字节数组中的数据写入到加密流中  
                    MyCryptoStream.Write(data, 0, data.Length);
                    //MyCryptoStream关闭之前ms.Length 为8， 关闭之后为16  
                    MyCryptoStream.FlushFinalBlock();
                    MyCryptoStream.Close();
                    byte[] bTmp = ms.ToArray();
                    ms.Close();
                    return bTmp;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>  
        /// MAC计算所要采用的CBC DES算法实现解密  
        /// </summary>  
        /// <param name="key">Key数据</param>  
        /// <param name="data">加密后数据</param>  
        /// <returns>返回解密结果</returns>  
        public static byte[] HCDES_Decrypt(byte[] key, byte[] data)
        {
            try
            {
                //创建一个DES算法的加密类  
                DESCryptoServiceProvider MyServiceProvider = new DESCryptoServiceProvider();
                MyServiceProvider.Mode = CipherMode.CBC;
                MyServiceProvider.Padding = PaddingMode.None;
                //从DES算法的加密类对象的CreateEncryptor方法,创建一个加密转换接口对象  
                //第一个参数的含义是：对称算法的机密密钥(长度为64位,也就是8个字节)  
                // 可以人工输入,也可以随机生成方法是：MyServiceProvider.GenerateKey();  
                //第二个参数的含义是：对称算法的初始化向量(长度为64位,也就是8个字节)  
                // 可以人工输入,也可以随机生成方法是：MyServiceProvider.GenerateIV()  
                //创建解密对象  
                ICryptoTransform MyTransform = MyServiceProvider.CreateDecryptor(key, new byte[8]);
                //CryptoStream对象的作用是将数据流连接到加密转换的流  
                using (MemoryStream ms = new MemoryStream())
                {
                    CryptoStream MyCryptoStream = new CryptoStream(ms, MyTransform, CryptoStreamMode.Write);
                    //将字节数组中的数据写入到解密流中  
                    MyCryptoStream.Write(data, 0, data.Length);
                    // MyCryptoStream关闭之前ms.Length 为8， 关闭之后为16  
                    MyCryptoStream.FlushFinalBlock();
                    MyCryptoStream.Close();
                    byte[] bTmp = ms.ToArray();
                    ms.Close();
                    return bTmp;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>  
        /// MAC计算 (ANSI-X9.9-MAC)  
        /// </summary>  
        /// <param name="data">数据</param>  
        /// <returns>返回该数据MAC值</returns>  
        public static byte[] GetMAC(byte[] data, byte[] key)
        {
            try
            {
                int iGroup = 0;
                byte[] bKey = key;
                byte[] bIV = new byte[8];
                byte[] bTmpBuf1 = new byte[8];
                byte[] bTmpBuf2 = new byte[8];
                // init  
                Array.Copy(bIV, bTmpBuf1, 8);
                if ((data.Length % 8 == 0))
                {
                    iGroup = data.Length / 8;
                }
                else
                {
                    iGroup = data.Length / 8 + 1;
                }
                int i = 0;
                int j = 0;
                for (i = 0; i < iGroup; i++)
                {
                    Array.Copy(data, 8 * i, bTmpBuf2, 0, 8);
                    for (j = 0; j < 8; j++)
                    {
                        bTmpBuf1[j] = (byte)(bTmpBuf1[j] ^ bTmpBuf2[j]);
                    }
                    bTmpBuf2 = HCDES_Encrypt(bKey, bTmpBuf1);
                    Array.Copy(bTmpBuf2, bTmpBuf1, 8);
                }
                return bTmpBuf2;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetMacEx(string data)
        {
            try
            {
                string hexPadding = "8000000000000000";
                int padLength = 16 - data.Length % 16;
                byte[] dataByte = StrUtils.HexStrToBytes(data + StrUtils.CopySubStr(hexPadding, 0, padLength));
                byte[] keyByte = StrUtils.HexStrToBytes(GetDecryKey());
                byte[] macRes = GetMAC(dataByte, keyByte);

                return StrUtils.CopySubStr(StrUtils.BytesToHexStr(macRes), 0, 8);

            }
            catch(Exception ex)
            {
                return "12A254AF";
                RunLog.Log(ex.ToString());


            }
        }


        //{0xC9, 0xEE, 0xDB, 0xDA, 0xCD, 0xFA, 0xC1, 0xFA}
        public static string GetMacEx(string data, string key)
        {
            string hexPadding = "8000000000000000";
            int padLength = 16 - data.Length % 16;
            byte[] dataByte = StrUtils.HexStrToBytes(data + StrUtils.CopySubStr(hexPadding, 0, padLength));
            byte[] keyByte = StrUtils.HexStrToBytes(key);
            byte[] macRes = GetMAC(dataByte, keyByte);
            return StrUtils.CopySubStr(StrUtils.BytesToHexStr(macRes), 0, 8);
        }
    }
}
