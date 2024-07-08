// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Org.BouncyCastle.Utilities.Encoders;
using Starshine.DataEncryption;

namespace Starshine.Admin.Core;

public class CryptogramUtil
{
    public static readonly string CryptoType = StarshineApp.GetConfig<string>("Cryptogram:CryptoType"); // 加密类型
    public static readonly string PublicKey = StarshineApp.GetConfig<string>("Cryptogram:PublicKey"); // 公钥
    public static readonly string PrivateKey = StarshineApp.GetConfig<string>("Cryptogram:PrivateKey"); // 私钥

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string Encrypt(string plainText)
    {
        if (CryptoType == CryptogramEnum.MD5.ToString())
        {
            return MD5Encryption.Encrypt(plainText);
        }
        else if (CryptoType == CryptogramEnum.SM2.ToString())
        {
            return SM2Encrypt(plainText);
        }
        return plainText;
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="cipherText"></param>
    /// <returns></returns>
    public static string Decrypt(string cipherText)
    {
        if (CryptoType == CryptogramEnum.SM2.ToString())
        {
            return SM2Decrypt(cipherText);
        }
        return cipherText;
    }

    /// <summary>
    /// SM2加密
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string SM2Encrypt(string plainText)
    {
        byte[] sourceData = Encoding.Default.GetBytes(plainText);
        return SM2Util.Encrypt(Hex.Decode(PublicKey), sourceData);
    }

    /// <summary>
    /// SM2解密
    /// </summary>
    /// <param name="cipherText"></param>
    /// <returns></returns>
    public static string SM2Decrypt(string cipherText)
    {
        return Encoding.Default.GetString(SM2Util.Decrypt(Hex.Decode(PrivateKey), Hex.Decode(cipherText)));
    }

    /// <summary>
    /// SM4加密（ECB）
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string SM4EncryptECB(string plainText)
    {
        var sm4 = new SM4Util();
        return sm4.Encrypt_ECB(plainText);
    }

    /// <summary>
    /// SM4解密（ECB）
    /// </summary>
    /// <param name="cipherText"></param>
    /// <returns></returns>
    public static string SM4DecryptECB(string cipherText)
    {
        var sm4 = new SM4Util();
        return sm4.Decrypt_ECB(cipherText);
    }

    /// <summary>
    /// SM4加密（CBC）
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string SM4EncryptCBC(string plainText)
    {
        var sm4 = new SM4Util();
        return sm4.Encrypt_CBC(plainText);
    }

    /// <summary>
    /// SM4解密（CBC）
    /// </summary>
    /// <param name="cipherText"></param>
    /// <returns></returns>
    public static string SM4DecryptCBC(string cipherText)
    {
        var sm4 = new SM4Util();
        return sm4.Decrypt_CBC(cipherText);
    }
}