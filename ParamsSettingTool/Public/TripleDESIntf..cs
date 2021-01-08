using System.Runtime.InteropServices;

namespace ITL.Public
{
    public static class TripleDESIntf
    {
        public const string TRIDES_DLL = @"TripleDES.dll";

        //DES加密，输入输出全为实际数据，原数据长度不是8字节整数倍时，将自动后补#$00
        [DllImport(TRIDES_DLL, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern string EncryStr_DESCS(string Str, string Key);

        //DES解密，输入输出全为实际数据，原数据长度不是8字节整数倍时，将自动后补#$00
        [DllImport(TRIDES_DLL, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern string DecryStr_DESCS(string Str, string Key);

        //3DES加密，输入输出全为实际数据，原数据长度不是8字节整数倍时，将自动后补#$00
        [DllImport(TRIDES_DLL, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern string EncryStr_3DESCS(string Str, string Key);

        //3DES解密，输入输出全为实际数据，原数据长度不是8字节整数倍时，将自动后补#$00
        [DllImport(TRIDES_DLL, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern string DecryStr_3DESCS(string Str, string Key);

        //DES加密，输入输出全为16进制串，原数据长度不是16字节整数倍时，将自动后补0
        [DllImport(TRIDES_DLL, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern string EncryHexStr_DESCS(string Str, string Key);

        //DES解密，输入输出全为16进制串，原数据长度不是16字节整数倍时，将自动后补0
        [DllImport(TRIDES_DLL, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern string DecryHexStr_DESCS(string Str, string Key);

        //3DES加密，输入输出全为16进制串，原数据长度不是16字节整数倍时，将自动后补0
        [DllImport(TRIDES_DLL, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern string EncryHexStr_3DESCS(string Str, string Key);

        //3DES解密，输入输出全为16进制串，原数据长度不是16字节整数倍时，将自动后补0
        [DllImport(TRIDES_DLL, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern string DecryHexStr_3DESCS(string Str, string Key);
    }
}
