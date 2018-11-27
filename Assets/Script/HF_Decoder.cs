
using System;
using System.Collections;
using System.Collections.Generic;

using System.Security.Cryptography;

/// <summary>
/// 解码调整
/// </summary>
public class HF_Decoder
{
    //一定不能出现大于等于0xDF的数,否则输出解码会超过12位(10进制)
    public enum MsgCode
    {
        None = 0,
        TouBiBiLi = 0xA0,
        ChangDiLeiXing = 0xA1,
        DaMaTianShu = 0xA2,
        XiTongShiJian = 0xA3,
        XianShiDaMaXinXi = 0xA4
    }
    public static MsgCode GetMsgPlainType(byte[] msgPlain)
    {
        if (msgPlain[0] == (byte)MsgCode.TouBiBiLi)
            return MsgCode.TouBiBiLi;

        if (msgPlain[0] == (byte)MsgCode.ChangDiLeiXing)
            return MsgCode.ChangDiLeiXing;
        if (msgPlain[0] == (byte)MsgCode.DaMaTianShu)
            return MsgCode.DaMaTianShu;
        if (msgPlain[0] == (byte)MsgCode.XiTongShiJian)
            return MsgCode.XiTongShiJian;
        if (msgPlain[0] == (byte)MsgCode.XianShiDaMaXinXi)
            return MsgCode.XianShiDaMaXinXi;
        return MsgCode.None;
    }

    /// <summary>
    /// 投币比例 ,转换为消息
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static byte[] TouBiBiLi_ToMsgPlain(int num, uint tableNum, uint tagNum)
    {

        byte[] numBytes = BitConverter.GetBytes((ushort)num);
        byte[] msg = new byte[] { ((byte)MsgCode.TouBiBiLi), numBytes[0], numBytes[1], (byte)tagNum, (byte)(tableNum ^ tagNum) };//补位

        return msg;
    }

    /// <summary>
    /// 消息->投币比例
    /// </summary>
    /// <param name="plain"></param>
    /// <returns></returns>
    public static int TouBiBiLi_FromMsgPlain(byte[] plain, uint tableNum, uint tagNum, out bool isVerifySucess)
    {
        if (plain[3] == (byte)tagNum && plain[4] == (byte)(tableNum ^ tagNum))
            isVerifySucess = true;
        else
            isVerifySucess = false;

        return (int)(BitConverter.ToUInt16(new byte[] { plain[1], plain[2] }, 0));
    }

    

    /// <summary>
    /// 场地类型 -> 消息
    /// </summary>
    /// <param name="type">0:小,1:中,2:大</param>
    /// <returns></returns>
    public static byte[] ChangeDiLeiXing_ToMsgPlain(int type, uint tableNum, uint tagNum)
    {

        byte[] msg = new byte[] { ((byte)MsgCode.ChangDiLeiXing), (byte)type, (byte)tableNum, (byte)tagNum, (byte)(tableNum ^ tagNum) };//补位

        return msg;
    }

    /// <summary>
    /// 消息->场地类型
    /// </summary>
    /// <param name="plain"></param>
    /// <returns></returns>
    public static int ChangeDiLeiXing_FromMsgPlain(byte[] plain, uint tableNum, uint tagNum, out bool isVerifySucess)
    {
        if (plain[2]== (byte)tableNum && plain[3] == (byte)tagNum && plain[4] == (byte)(tableNum ^ tagNum))
            isVerifySucess = true;
        else
            isVerifySucess = false;

        return plain[1];
    }
     

    /// <summary>
    /// 打码天数 -> 消息
    /// </summary>
    /// <param name="type">1~13(+1)</param>
    /// <returns></returns>
    public static byte[] DaMaTianShu_ToMsgPlain(int day, uint tableNum, uint tagNum)
    {

        byte[] msg = new byte[] { ((byte)MsgCode.DaMaTianShu), (byte)day, (byte)tableNum, (byte)tagNum, (byte)(tableNum ^ tagNum) };//补位

        return msg;
    }

    /// <summary>
    /// 消息->打码天数
    /// </summary>
    /// <param name="plain"></param>
    /// <returns></returns>
    public static int DaMaTianShu_FromMsgPlain(byte[] plain, uint tableNum, uint tagNum, out bool isVerifySucess)
    {
        if (plain[2] == (byte)tableNum && plain[3] == (byte)tagNum && plain[4] == (byte)(tableNum ^ tagNum))
            isVerifySucess = true;
        else
            isVerifySucess = false;

        return plain[1];
    }


    /// <summary>
    /// 显示打码信息 -> 消息
    /// </summary>
    /// <param name="type">0:false,1:true</param>
    /// <returns></returns>
    public static byte[] XianShiDaMaXinXi_ToMsgPlain(bool isView, uint tableNum, uint tagNum)
    {

        byte[] msg = new byte[] { ((byte)MsgCode.XianShiDaMaXinXi), (byte)(isView ? 1 : 0), (byte)tableNum, (byte)tagNum, (byte)(tableNum ^ tagNum) };//补位

        return msg;
    }

    /// <summary>
    /// 消息->显示打码信息
    /// </summary>
    /// <param name="plain"></param>
    /// <returns></returns>
    public static bool XianShiDaMaXinXi_FromMsgPlain(byte[] plain, uint tableNum, uint tagNum, out bool isVerifySucess)
    {
        if (plain[2] == (byte)tableNum && plain[3] == (byte)tagNum && plain[4] == (byte)(tableNum ^ tagNum))
            isVerifySucess = true;
        else
            isVerifySucess = false;

        return plain[1] == 1 ? true : false;
    }


    /// <summary>
    /// 系统时间 -> 消息
    /// </summary>
    /// <param name="type">0:false,1:true</param>
    /// <returns></returns>
    public static byte[] XiTongShiJian_ToMsgPlain(uint year, uint month, uint day, uint hour, uint minit, uint tableNum, uint tagNum)
    {
        //7,4,5,5,6
        uint data = (year << 25) | (month << 21) | (day << 16) | (hour << 11) | (minit << 5);
        byte[] dataBytes = BitConverter.GetBytes(data);
        byte[] msg = new byte[] { ((byte)MsgCode.XiTongShiJian), dataBytes[0], dataBytes[1], dataBytes[2], dataBytes[3] };

        return msg;
    }

    /// <summary>
    /// 消息->系统时间
    /// </summary>
    /// <param name="plain"></param>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="day"></param>
    /// <param name="hour"></param>
    /// <param name="minit"></param>
    public static void XiTongShiJian_FromMsgPlain(byte[] plain, out uint year, out uint month, out uint day, out uint hour, out uint minit)
    {
        byte[] dataBytes = new byte[] { plain[1], plain[2], plain[3], plain[4] };
        uint data = BitConverter.ToUInt32(dataBytes, 0);
        year = data >> 25;
        month = data << 7 >> 28;
        day = data << 11 >> 27;
        hour = data << 16 >> 27;
        minit = data << 21 >> 26;

    }


    /// <summary>
    /// 用 机台号 和 解码特征码 加密 明文
    /// </summary>
    /// <param name="plain"></param>
    /// <param name="tableNum"></param>
    /// <param name="tagNum"></param>
    /// <returns></returns>
    public static byte[] Encrypt_Msg(byte[] data, uint tableNum, uint tagNum)
    {
        byte[] keyBytes = BitConverter.GetBytes(tableNum ^ tagNum);
        byte keyByte = 0;
        for (int i = 0; i != keyBytes.Length; ++i)
            keyByte ^= keyBytes[i];


        for (int j = 0; j != 2; ++j)
            for (int i = 1; i != data.Length; ++i)
            {
                data[i] ^= keyByte;
                keyByte = data[i];
            }

        return data;
    }

    /// <summary>
    /// 解密信息 (需要机台号+特征码)
    /// </summary>
    /// <param name="data"></param>
    /// <param name="tableNum"></param>
    /// <param name="tagNum"></param>
    /// <returns></returns>
    public static byte[] Decrypt_Msg(byte[] data, uint tableNum, uint tagNum)
    {
        byte[] keyBytes = BitConverter.GetBytes(tableNum ^ tagNum);
        byte keyByte = 0;
        for (int i = 0; i != keyBytes.Length; ++i)
            keyByte ^= keyBytes[i];

        byte nextData;
        //int j = 0;
        for (int j = 0; j != 2; ++j)
            for (int i = data.Length - 1; i != 0; --i)
            {
                if (j == 0 && i == 1)//第一趟最后使用data最后一个数据
                    nextData = data[data.Length - 1];
                else if (j == 1 && i == 1)//第二趟最好需要用到keybyte
                    nextData = keyByte;
                else
                    nextData = data[i - 1];
                //data[i] ^= keyByte;
                data[i] ^= nextData;
                //nextData = i == 0 ? data[data.Length - 1] : data[i - 2];

            }

        return data;
    }

    /// <summary>
    /// 消息 -> 10进制码
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static ulong MsgToDecimalCode(byte[] msg)
    {
        byte[] ba = new byte[8];
        for (int i = 0; i != msg.Length; ++i)
            ba[i] = msg[msg.Length - i - 1];

        return BitConverter.ToUInt64(ba, 0);
    }

    /// <summary>
    /// 10进制码->消息
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static byte[] MsgFromDecimalCode(ulong code)
    {
        byte[] ba = BitConverter.GetBytes(code);
        byte[] outVal = new byte[5];
        //Array.Copy(ba, outVal, 5);
        for (int i = 0; i != 5; ++i)
            outVal[i] = ba[4 - i];
        return outVal;
    }

    /// <summary>
    /// 生成特征码
    /// </summary>
    /// <param name="tableIdx">台号</param>
    /// <param name="lineIdx">线号</param>
    /// <param name="formulaCode">公式码</param>
    /// <param name="gameIdx">游戏索引</param>
    /// <remarks>
    /// 注意:
    ///     1.为兼容以前版本,当判断 (formulaCode==0xffffffff && gameIdx==0xffffffff)时,则忽略formulaCode,gameIdx两个参数
    /// 不足:
    ///     1.不一定有8位,需要外部补完
    /// </remarks>
    /// <returns></returns>
    public static uint GenerateTagCode(uint tableIdx, uint lineIdx, uint formulaCode, uint gameIdx)
    {
        uint cipher0;
        if (formulaCode == 0xffffffff && gameIdx == 0xffffffff)//为兼容以前版本
        {
            cipher0 = (tableIdx ^ lineIdx) & 0x3FFFFFF;//屏蔽两位
        }
        else
        {
            cipher0 = (tableIdx ^ lineIdx ^ formulaCode ^ gameIdx) & 0x3FFFFFF;//屏蔽两位
        }


        uint cipher = (cipher0 ^ ((uint)new Random().Next(0, 7143) * 9394));//保证生成数范围足够大(10进制八位)

        //if (cipher > 99999999)
        //    return GenerateTagCode(tableIdx, lineIdx);

        return cipher;
    }


    /// <summary>
    /// 验证特征码是否正确
    /// </summary>
    /// <param name="tagCode">待验证特征码</param>
    /// <param name="tableIdx">台号</param>
    /// <param name="lineIdx">线号</param>
    /// <param name="formulaCode">公式码</param>
    /// <param name="gameIdx">游戏索引</param>
    /// <remarks>
    ///  注意:
    ///     1.为兼容以前版本,当判断 (formulaCode==0xffffffff && gameIdx==0xffffffff)时,则忽略formulaCode,gameIdx两个参数
    /// </remarks>
    /// <returns></returns>
    public static bool IsValidTagCode(uint tagCode, uint tableIdx, uint lineIdx, uint formulaCode, uint gameIdx)
    {
        uint rndNum;
        if (formulaCode == 0xffffffff && gameIdx == 0xffffffff)//为兼容以前版本
        {
            rndNum = tagCode ^ ((tableIdx ^ lineIdx) & 0x3FFFFFF);
        }
        else
        {
            rndNum = tagCode ^ ((tableIdx ^ lineIdx ^ formulaCode ^ gameIdx) & 0x3FFFFFF);
        }

        if ((rndNum) % 9394 == 0)
            return true;
        else
            return false;
    }
}


/// <summary>
/// 打码报账 相关
/// </summary>
class HF_CodePrint
{
    public static uint EncryptUint4Bit(uint ui)
    {
        uint outVal = ui;
        for (int i = 0; i != 8; ++i)
            outVal = (outVal >> 4) ^ ui;
        return outVal & 0xF;
    }
 
    /// <summary>
    /// 游戏数据 -> 报账码(可逆)
    /// </summary>
    /// <param name="gainTotal"></param>
    /// <param name="gainCurrent"></param>
    /// <param name="tableIdx"></param>
    /// <param name="numPrint"></param>
    /// <param name="codeComfirm"></param>
    /// <param name="gainAdjustIdx">
    /// 6bit数据
    /// (0-20)盈利调整索引
    /// "保持现状","放水一千","放水二千","放水三千","放水四千","放水五千","放水八千","放水一万","放水两万","放水五万","放水十万" ,"抽水一千","抽水二千","抽水三千","抽水四千","抽水五千","抽水八千","抽水一万","抽水两万","抽水五万","抽水十万" </param>
    /// (30-36)加难设置六档难度
    /// 30:不加难 ， 31-36：六档难度
    /// <remarks>
    /// 注意：
    /// 1.输出uint只有26bit有效(打码只有8位十进制数),高6bit必须为0
    /// 2.【弊端】输入抽放水和难度设置不能同时打
    /// </remarks>
    /// <returns></returns>
    public static uint DataToPrintCode(int gainTotal, int gainCurrent, uint tableIdx, uint numPrint, uint codeComfirm, uint gainAdjustIdx)
    {
        if (gainAdjustIdx > 63)//如果输入值大于63，则会在VerifyPrintCode中返回错误的gainAdjustIdx，导致错误操作
            return 0;

        uint outVal = 0;
        outVal |= EncryptUint4Bit((uint)gainTotal) << 16;
        outVal |= EncryptUint4Bit((uint)gainCurrent) << 12;
        outVal |= EncryptUint4Bit(tableIdx) << 8;
        outVal |= EncryptUint4Bit(numPrint) << 4;
        outVal |= EncryptUint4Bit(codeComfirm);
        outVal |= gainAdjustIdx << 20; 
        

        //混淆
        outVal = (outVal << 13) & 0x3FFFFFF | (outVal >> 13); //13位前后互换

        uint mask = 0x2000000;
        uint vec = 0;
        uint[] masks = new uint[26];
        uint[] deMasks = new uint[26];
        for (int i = 0; i != 26; ++i)
        {
            masks[i] = mask >> (i + 1);
            deMasks[i] = masks[i] ^ 0xffffffff;
        }

        for (int j = 0; j != 2; ++j)
            for (int i = 0; i != 26; ++i)
            {
                uint a1 = (vec ^ outVal) & masks[i];
                uint a2 = outVal & deMasks[i];
                vec = a1 >> 1;
                outVal = a1 | a2;
            }

        return outVal;
    }


   
    /// <summary>
    /// 验证报账码,并返回gainAdjustIdx
    /// </summary>
    /// <param name="printCodeEnc"></param>
    /// <param name="gainTotal"></param>
    /// <param name="gainCurrent"></param>
    /// <param name="tableIdx"></param>
    /// <param name="numPrint"></param>
    /// <param name="codeComfirm"></param>
    /// <param name="gainAdjustIdx">抽放水系数(0-20)</param>
    /// <param name="gainRatioMulti">游戏加难设置(1-6)</param>
    /// <returns>printCode正确</returns>
    public static bool VerifyPrintCode(uint printCodeEnc, int gainTotal, int gainCurrent, uint tableIdx, uint numPrint, uint codeComfirm
        , ref uint gainAdjustIdx
        , ref uint gainRatioMulti)
    {
        //解密printCode
        uint mask = 0x2000000;
        uint vec = 0;
        uint[] masks = new uint[26];
        uint[] deMasks = new uint[26];
        for (int i = 0; i != 26; ++i)
        {
            masks[i] = mask >> (i + 1);
            deMasks[i] = masks[i] ^ 0xffffffff;
        }

        uint decryptPrintCode = printCodeEnc;
        int bitStart = 25;
        for (int j = 1; j != -1; --j)
        {
            for (int i = bitStart; i != 0; --i)
            {
                vec = decryptPrintCode & masks[i - 1];//取得最后袷

                uint a1 = ((vec >> 1) ^ decryptPrintCode) & masks[i];
                uint a2 = decryptPrintCode & deMasks[i];
                decryptPrintCode = a1 | a2;
            }
            //处理最后一个
            vec = j != 0 ? decryptPrintCode & masks[bitStart] : 0;
            uint b1 = ((vec >> 1) ^ decryptPrintCode) & masks[0];
            uint b2 = decryptPrintCode & deMasks[0];
            decryptPrintCode = b1 | b2;
        }
        decryptPrintCode = (decryptPrintCode << 13) & 0x3FFFFFF | (decryptPrintCode >> 13); //13位前后互换
        uint outVal = decryptPrintCode >> 20;
        if (outVal >= 0 && outVal <= 20)
            gainAdjustIdx = outVal;
        else if (outVal >= 30 && outVal <= 36)//根据函数DataToPrintCode中的值规定
            gainRatioMulti = outVal % 30;

        uint calcPrintCode = 0;
        calcPrintCode |= EncryptUint4Bit((uint)gainTotal) << 16;
        calcPrintCode |= EncryptUint4Bit((uint)gainCurrent) << 12;
        calcPrintCode |= EncryptUint4Bit(tableIdx) << 8;
        calcPrintCode |= EncryptUint4Bit(numPrint) << 4;
        calcPrintCode |= EncryptUint4Bit(codeComfirm);

        return (decryptPrintCode & 0xFFFFF) == calcPrintCode;
    }

    /// <summary>
    ///[作废] 游戏数据 -> 报账码
    /// </summary>
    /// <param name="gainTotal"></param>
    /// <param name="gainCurrent"></param>
    /// <param name="tableIdx"></param>
    /// <param name="numPrint"></param>
    /// <param name="codeComfirm"></param>
    /// <returns></returns>
    //public static uint DataToPrintCode(int gainTotal,int gainCurrent, uint tableIdx,uint numPrint, uint codeComfirm)
    //{
    //    List<byte> dataCollect = new List<byte>();
    //    byte[] byteBuf = BitConverter.GetBytes(gainTotal);
    //    foreach (byte b in byteBuf)
    //        dataCollect.Add(b);

    //    byteBuf = BitConverter.GetBytes(gainCurrent);
    //    foreach (byte b in byteBuf)
    //        dataCollect.Add(b);
    //    byteBuf = BitConverter.GetBytes(tableIdx);
    //    foreach (byte b in byteBuf)
    //        dataCollect.Add(b);

    //    byteBuf = BitConverter.GetBytes(numPrint);
    //    foreach (byte b in byteBuf)
    //        dataCollect.Add(b);

    //    byteBuf = BitConverter.GetBytes(codeComfirm);
    //    foreach (byte b in byteBuf)
    //        dataCollect.Add(b);

    //    MD5 md5 = new MD5CryptoServiceProvider();
    //    byte[] md5data = md5.ComputeHash(dataCollect.ToArray());//计算data字节数组的哈希值 
    //    md5.Clear();

    //    for (int i = 0; i != 4; ++i)
    //        md5data[i+12] = (byte)(md5data[i] ^ md5data[i + 4] ^ md5data[i + 8] ^ md5data[i + 12]);

    //    uint outCode = BitConverter.ToUInt32(md5data, 12);//取最后12位

    //    outCode %= 100000000;//得出8位码
    //    return outCode;
    //}

    /// <summary>
    /// 生成四位验证码
    /// </summary>
    /// <param name="gainTotal">总盈利数</param>
    /// <param name="gainCurrent">当期盈利数</param>
    /// <param name="lineIdx">线号</param>
    /// <param name="tableIdx">台号</param>
    /// <param name="numPrint">打码次数</param>
    /// <param name="formulaCode">公式码</param>
    /// <param name="gameIdx">游戏索引</param>
    /// <remarks>
    /// 注意:
    ///     1.为兼容以前版本,当判断 (formulaCode==0xffffffff && gameIdx==0xffffffff)时,则忽略formulaCode,gameIdx两个参数
    /// </remarks>
    /// <returns></returns>
    public static uint GenerateComfirmCode(int gainTotal, int gainCurrent, int lineIdx, uint tableIdx, uint numPrint, uint formulaCode, uint gameIdx)
    {
        int[] code;
        if (formulaCode == 0xffffffff && gameIdx == 0xffffffff)//为兼容以前版本
        {
            code = new int[] { gainTotal, gainCurrent, lineIdx, (int)tableIdx, (int)numPrint };
        }
        else
        {
            code = new int[] { gainTotal, gainCurrent, lineIdx, (int)tableIdx, (int)numPrint, (int)formulaCode, (int)gameIdx };
        }


        List<byte> dataCollect = new List<byte>();

        foreach (int c in code)
        {
            byte[] byteBuf = BitConverter.GetBytes(c);//将int值转为byte数组(1 int = 4 byte)
            foreach (byte b in byteBuf)
                dataCollect.Add(b);
        }

        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] md5data = md5.ComputeHash(dataCollect.ToArray());//计算data字节数组的哈希值 
        md5.Clear();

        for (int i = 0; i != 4; ++i)
            md5data[i + 12] = (byte)(md5data[i] ^ md5data[i + 4] ^ md5data[i + 8] ^ md5data[i + 12]);

        uint outCode = BitConverter.ToUInt32(md5data, 12);//取得最后12位 
        outCode %= 10000;
        return outCode;
    }

}

public static class GlobalMembersDaMa
{
    //C++ TO C# CONVERTER WARNING: The following #include directive was ignored:
    //#include "rand.h"


    public static uint g_iXianHao = 286;

    public static void tea_encipher(UInt32[] plain, UInt32[] key, UInt32[] crypt)
    {
        UInt32 left = plain[0];
        UInt32 right = plain[1];
        UInt32 a = key[0];
        UInt32 b = key[1];
        UInt32 c = key[2];
        UInt32 d = key[3];
        UInt32 n = 32;
        UInt32 sum = 0;
        UInt32 delta = (uint)0x9e3779b9;

        UInt32 temp_shu = g_iXianHao;

        a = key[0] + temp_shu;
        b = key[1] + temp_shu;
        c = key[2] + temp_shu;
        d = key[3] + temp_shu;


        while (n-- > 0)
        {
            sum += delta;
            left += ((right << 4) + a)^(right + sum)^((right >> 5) + b);
            right += ((left << 4) + c)^(left + sum)^((left >> 5) + d);
        }

        crypt[0] = left;
        crypt[1] = right;
    }

    public static void tea_encipher2(UInt32[] plain, UInt32[] key, UInt32[] crypt)
    {
        UInt32 left = plain[0];
        UInt32 right = plain[1];
        UInt32 a = key[0];
        UInt32 b = key[1];
        UInt32 c = key[2];
        UInt32 d = key[3];
        UInt32 n = 32;
        UInt32 sum = 0;
        UInt32 delta = (uint)0x9e3779b9;

        UInt32 temp_shu = g_iXianHao;

        a = key[0] + temp_shu;
        b = key[1] + temp_shu;
        c = key[2] + temp_shu;
        d = key[3] + temp_shu;


        while (n-- > 0)
        {
            sum += delta;
            left += ((right << 3) + a) ^ (right + sum) ^ ((right >> 4) + b);
            right += ((left << 3) + c) ^ (left + sum) ^ ((left >> 4) + d);
        }

        crypt[0] = left;
        crypt[1] = right;
    }


    public static void tea_decrypt(UInt32[] crypt, UInt32[] key, UInt32[] plain)
    {
        UInt32 y = crypt[0];
        UInt32 z = crypt[1];
        UInt32 sum = 0xC6EF3720, i;			/* set up */
        UInt32 delta = 0x9e3779b9;										/* a key schedule constant */
        UInt32 a = key[0];
        UInt32 b = key[1];
        UInt32 c = key[2];
        UInt32 d = key[3];			/* cache key */
        UInt32 temp_shu = g_iXianHao;

        a = key[0] + temp_shu;
        b = key[1] + temp_shu;
        c = key[2] + temp_shu;
        d = key[3] + temp_shu;

        /* basic cycle start */
        for (i = 0; i < 32; i++)
        {
            z -= ((y << 4) + c) ^ (y + sum) ^ ((y >> 5) + d);
            y -= ((z << 4) + a) ^ (z + sum) ^ ((z >> 5) + b);
            sum -= delta;
        }

        /* end cycle */

        plain[0] = y;
        plain[1] = z;
    } 

    public static uint U8ArrayToU32(ushort[] src, byte pos)
    {
        uint temp;
        uint temp1;
        uint temp2;
        temp = 0x00000000;
        temp1 = 0x00000000;
        temp2 = 0x00000000;
        temp |= (src[pos + 3]);
        temp = temp << 24;
        temp1 |= (src[pos + 2]);
        temp1 = temp1 << 16;
        temp |= temp1;
        temp2 |= (src[pos + 1]);
        temp2 = temp2 << 8;
        temp |= temp2;
        temp |= (src[pos]);
        return temp;
    }


    //
    //
    //计数带控选择码 根据输入的四个数计算出4位十进制校验码并返回 
    //参数：
    //U32 QuanBuZong_LR = 0;//全部总利润 0~8位十进制
    //U32 BenCi_ShiJi_LR = 0;//本次实际利润 0~8位十进制
    //U32 FenJi_HM = 0;//分机号码 8位十进制
    //U32 BaoMa_CS = 0;//报码次数 0~5位十进制
    //U8* jym    一个4个元素的U8类型数组
    //
    //返回值：
    //根据输入的四个数计算出4位十进制校验码并返回
    //
    //
    public static UInt32 JiaoYan_JYM_encipher(UInt32 QuanBuZong_LR, UInt32 BenCi_ShiJi_LR, UInt32 FenJi_HM, UInt32 BaoMa_CS, UInt16[] jym)
    {
        //UInt32[] key = { 1231595628, 1236212315, 3161231583, 1136939275 };
        //UInt32[] key = { 3256494628, 1436254228, 4165748483, 1236439275 };
        //UInt32[] key = { 3158695628, 1236259228, 3165758883, 1136939275 };

        UInt32[] key = {3256494985, 1436254594, 4165748379, 1236439473};


        UInt32[] plain = new UInt32[2];
        UInt32[] crypt = new UInt32[2];
        UInt16 i = new UInt16();
        UInt32 u32_temp = new UInt32();
        UInt32 u32_cpy = new UInt32();


        u32_temp = 0;
        plain[0] = QuanBuZong_LR; plain[1] = BenCi_ShiJi_LR;
        tea_encipher(plain, key, crypt);
        u32_temp += crypt[0];
        u32_temp += crypt[1];

        plain[0] = FenJi_HM; ;
        plain[1] = BaoMa_CS;
        tea_encipher(plain, key, crypt);
        u32_temp += crypt[0];
        u32_temp += crypt[1];

        u32_cpy = u32_temp;
        for (i = 4; i > 0; i--)
        {
            jym[i - 1] = (ushort)(u32_temp % 10);
            u32_temp = u32_temp / 10;
        }

        return 0;
    }

    public static UInt16 JiaoYan_JYM_decrypt(UInt32 QuanBuZong_LR, UInt32 BenCi_ShiJi_LR, UInt32 FenJi_HM, UInt32 BaoMa_CS, UInt16[] jym) //打码器调用
    {
        //UInt32[] key = { 1231595628, 1236212315, 3161231583, 1136939275 };
        //UInt32[] key = {3256494628, 1436254228, 4165748483, 1236439275};
        //UInt32[] key = { 3158695628, 1236259228, 3165758883, 1136939275 };

        UInt32[] key = {3256494985, 1436254594, 4165748379, 1236439473};


        UInt32[] plain = new UInt32[2];
        UInt32[] crypt = new UInt32[2];
        UInt16 i = new UInt16();
        UInt32 u32_temp = new UInt32();

        UInt16[] jym_temp = new UInt16[8];


        u32_temp = 0;
        plain[0] = QuanBuZong_LR;
        plain[1] = BenCi_ShiJi_LR;
        tea_encipher(plain, key, crypt);
        u32_temp += crypt[0];
        u32_temp += crypt[1];

        plain[0] = FenJi_HM;
        plain[1] = BaoMa_CS;
        tea_encipher(plain, key, crypt);
        u32_temp += crypt[0];
        u32_temp += crypt[1];

        for (i = 4; i > 0; i--)
        {
            jym_temp[i - 1] = (ushort)(u32_temp % 10);
            u32_temp = u32_temp / 10;
        }

        for (i = 0; i < 4; i++)
        {
            if (jym_temp[i] != jym[i])
            {
                return 0;
            }
        }

        return 1;

    }


    //参数要求：
    //U8 Num ;//带控选择码类型
    //“0为保持现状”
    //“1为放水一千”
    //“2为放水二千”
    //“3为放水三千”
    //“4为放水四千”
    //“5为放水五千”
    //“6为放水八千”
    //“7为放水一万”
    //“8为放水二万”
    //“9为放水五万”
    //“10为放水十万”
    //“11为抽水一千”
    //“12为抽水二千”
    //“13为抽水三千”
    //“14为抽水四千”
    //“15为抽水五千”
    //“16为抽水八千”
    //“17为抽水一万”
    //“18为抽水二万”
    //“19为抽水五万”
    //“20为抽水十万”

    //U32 QuanBuZong_LR = 0;//全部总利润 0~8位十进制
    //U32 BenCi_ShiJi_LR = 0;//本次实际利润 0~8位十进制
    //U32 FenJi_HM = 0;//分机号码 8位十进制
    //U32 BaoMa_CS = 0;//报码次数 0~5位十进制
    //U8* jym    校验码，一个4个元素的U8类型数组
    //U8 Num ;//带控选择码类型
    //U8 Data[8];//用于存放带控选择码

    // 返回值中，Data[6] * 10 + Data[7] == 带控选择码类型；
    public static void JiSuan_DKXZM_encipher(UInt32 QuanBuZong_LR, UInt32 BenCi_ShiJi_LR, UInt32 FenJi_HM, UInt32 BaoMa_CS, ref UInt16[] jym, UInt16 Num, UInt16[] Data) //计算带控选择码 计算出的码存放到数组Data //打码器调用
    {
	    //UInt32[] key = {2123153998, 1231576893, 1958121253, 2232812125};

        //UInt32[] key = {3952673498, 3264876843, 2458154343, 2234815423};
        //UInt32[] key = {2952673998, 3169876893, 1958156383, 2232815923};
        UInt32[] key = {2793423498, 3465202843, 1702134343, 2234834981}; //res->


        UInt32[] plain = new UInt32[2];
        UInt32[] crypt = new UInt32[2];
        UInt16 i = new UInt16();
        UInt16 j = new UInt16();
        UInt32 temp = 0;
        UInt32 jym_temp = new UInt32();

        for (i = 0; i < 8; i++)
        {
            Data[i] = 0;
        }


        plain[0] = QuanBuZong_LR;
        plain[1] = BenCi_ShiJi_LR;
        tea_encipher(plain, key, crypt);
        temp += crypt[0];
        temp += crypt[1];

        plain[0] = FenJi_HM;
        plain[1] = BaoMa_CS;
        tea_encipher(plain, key, crypt);
        temp += crypt[0];
        temp += crypt[1];

        jym_temp = U8ArrayToU32(jym, 0);
        plain[0] = jym_temp;
        plain[1] = jym_temp;
        tea_encipher(plain, key, crypt);
        temp += crypt[0];
        temp += crypt[1];

        temp = temp / 100;

        for (i = 0, j = 7; i < 8; i++, j--)
        {
            Data[j] = (ushort)(temp % 10);
            temp = temp / 10;
        }

        Data[6] = (ushort)(Num / 10);
        Data[7] = (ushort)(Num % 10);
    }

    // 报账时使用
    // 返回值：0：输入错误，1：输入正确
    public static string JiSuan_DKXZM_decrypt(UInt32 QuanBuZong_LR, UInt32 BenCi_ShiJi_LR, UInt32 FenJi_HM, UInt32 BaoMa_CS, UInt16[] jym, UInt32[] key) //计算带控选择码 计算出的码存放到数组Data 
    {
        //UInt32[] key = { 2952673998,3169876893, 1958156383,2232815923 };
        //UInt32[] key = { 2123153998, 1231576893, 1958121253, 2232812125 };
       // UInt32[] key = {3952673498, 3264876843, 2458154343, 2234815423};
        //UInt32[] key = {2793423498, 3465202843, 1702134343, 2234834981};


        UInt32[] plain = new UInt32[2];
        UInt32[] crypt = new UInt32[2];
        UInt32 i = new UInt32();
        UInt32 j = new UInt32();
        UInt32 temp = 0;
        UInt32 jym_temp = new UInt32();
        UInt32[] Data = new UInt32[8];



        plain[0] = QuanBuZong_LR;
        plain[1] = BenCi_ShiJi_LR;
        tea_encipher(plain, key, crypt);
        temp += crypt[0];
        temp += crypt[1];

        //plain[0] = (uint)GameMain.Singleton.BSSetting.Dat_IdTable.Val;
        plain[0] = FenJi_HM;
        plain[1] = BaoMa_CS;
        tea_encipher(plain, key, crypt);
        temp += crypt[0];
        temp += crypt[1];

        jym_temp = U8ArrayToU32(jym, 0);
        plain[0] = jym_temp;
        plain[1] = jym_temp;
        tea_encipher(plain, key, crypt);
        temp += crypt[0];
        temp += crypt[1];

        temp = temp / 100;

        for (i = 0, j = 7; i < 8; i++, j--)
        {
            Data[j] = (ushort)(temp % 10);
            temp = temp / 10;
        }

        //for (i = 0; i < 6; i++)
        //{
        //    if (Data[i] != pData[i])
        //    {
        //        break;
        //    }
        //}

        //if (i < 6)
        //{
        //    return 0;
        //}

        //Num = (uint)(pData[6] * 10 + pData[7]);
        string result = Data[0].ToString()+ 
            Data[1].ToString() + 
            Data[2].ToString() + 
            Data[3].ToString() + 
            Data[4].ToString() +
            Data[5].ToString() +
            Data[6].ToString() +
            Data[7].ToString();
        
        return result;
    }



    //计算本次调整码 根据传过来的前三个参数计算出调整码并放到Data数组内 

    // U32 System_Time;//系统时间
    // U8 Data[12];//用于存放计算出来的调整码
    public static void JiSuan_TZM_encipher(UInt32 JMTZM, UInt16 TZM_State_1, UInt16 TZM_State_2, UInt32 System_Time, UInt16[] Data) // 打码器使用
    {
        UInt32 temp = new UInt32();
        UInt32 temp1 = new UInt32();
        UInt32 temp2 = new UInt32();
        UInt32 temp3 = new UInt32();
        UInt32 temp4 = new UInt32();
        //UInt32[] key = { 3293217502, 3508680038, 3225292888, 255898098 };
       // UInt32[] key = {3294247502, 3548680038, 3245242444, 245848094};
        //UInt32[] key = {1683217502, 3168680038, 3216892888, 251688098};
        UInt32[] key = {3293402834, 3429452038, 3942042444, 1793948094};

        UInt32[] plain = new UInt32[2];
        UInt32[] crypt = new UInt32[2];

        UInt16[] data1 = new UInt16[12];

        UInt16 i = new UInt16();
        UInt16 j = new UInt16();


        plain[0] = JMTZM;
        plain[1] = (uint)2404765961;
        tea_encipher(plain, key, crypt);
        if (TZM_State_1 == 0 || TZM_State_1 == 1 || TZM_State_1 == 2 || TZM_State_1 == 4)
        {
            temp1 = TZM_State_2;
            temp1 = temp1 << 27;

            temp2 = TZM_State_1;
            temp2 = temp2 << 21;

            temp3 = temp1 | temp2;

            temp3 = temp3 & 0xF9EF9F3F; // 11111001 11101111 10011111 00111111
            temp4 = JMTZM & 0x61060C0;  // 00000110 00010000 01100000 11000000

            temp = temp3 | temp4;

            temp = temp ^ crypt[0] ^ crypt[1];
        }
        else
        {
            // 年
            temp = System_Time / 100000000;
            temp = temp << 27;

            // 月
            System_Time = System_Time % 100000000;
            temp1 = System_Time / 1000000;
            temp1 = temp1 << 21;
            temp = temp | temp1;

            // 日
            System_Time = System_Time % 1000000;
            temp1 = System_Time / 10000;
            temp1 = temp1 << 15;
            temp = temp | temp1;

            // 时
            System_Time = System_Time % 10000;
            temp1 = System_Time / 100;
            temp1 = temp1 << 8;
            temp = temp | temp1;

            // 分
            System_Time = System_Time % 100;
            temp1 = System_Time;
            temp = temp | temp1;

            temp = temp & 0xF9EF9F3F;   // 11111001 11101111 10011111 00111111
            temp4 = JMTZM & 0x61060C0;  // 00000110 00010000 01100000 11000000

            temp = temp | temp4;

            temp = temp ^ crypt[0] ^ crypt[1];
        }


        for (i = 0; i < 12; i++)
        {
            Data[i] = 0;
        }

        for (i = 2, j = 11; i < 12; i++, j--)
        {
            Data[j] = (ushort)(temp % 10);
            temp = temp / 10;
        }

        	Data[0] = TZM_State_1;
    }

    //根据调整码，提取出相关信息 

    // U32 System_Time;//系统时间
    // U8 Data[12];//存放着计算出来的调整码

    // 返回值：0：输入错误，1：输入正确
    public static UInt16 JiSuan_TZM_decrypt(UInt32 JMTZM, ulong[] Data, ref UInt32 State_1, ref UInt32 State_2, ref UInt32 year, ref UInt32 month, ref UInt32 date, ref UInt32 hour, ref UInt32 min)
    {
        UInt32 temp = new UInt32();
        UInt32 temp1 = new UInt32();
        UInt32 temp2 = new UInt32();
        UInt32 temp3 = new UInt32();
        UInt32 temp4 = new UInt32();
        //	U32 key[4] = {3293217502, 3508680038, 3225292888, 255898098};
        //UInt32[] key = { 1683217502, 3168680038, 3216892888, 251688098 };

        //UInt32[] key = { 1683217502, 3168680038, 3216892888, 251688098 };

        UInt32[] key = {3293402834, 3429452038, 3942042444, 1793948094};

        UInt32[] plain = new UInt32[2];
        UInt32[] crypt = new UInt32[2];

        UInt32 TZM_State_1 = new UInt32();
        UInt32 TZM_State_2 = new UInt32();
        UInt32 System_Time = new UInt32();

        UInt16 i = new UInt16();
        UInt16 j = new UInt16();

        temp = 0;
        for (i = 2; i < 12; i++)
        {
            temp = (uint)(temp * 10 + Data[i]);
        }

        plain[0] = JMTZM;
        plain[1] = 2404765961;
        tea_encipher(plain, key, crypt);

        temp = temp ^ crypt[1] ^ crypt[0];


        temp1 = temp & (int)0x61060C0;
        temp2 = JMTZM & (int)0x61060C0;
        if (temp1 != temp2)
        {
            return 0;
        }

        temp = temp & 0xF9EF9F3F; // 11111001 11101111 10011111 00111111

//         TZM_State_2 = temp;
//         TZM_State_2 = TZM_State_2 & 0x1F00;
//         TZM_State_2 = TZM_State_2 >> 8;
//         State_2 = TZM_State_2;
// 
//         TZM_State_1 = temp;
//         TZM_State_1 = TZM_State_1 & 0x3F;
//         //TZM_State_1 = TZM_State_1 >> 15;
//         State_1 = TZM_State_1;

        if (Data[0] == 0 || Data[0] == 1 || Data[0] == 2 || Data[0] == 4 || Data[0] == 5)
        {
            TZM_State_2 = temp;
            TZM_State_2 = TZM_State_2 >> 27;
            State_2 = TZM_State_2;

            TZM_State_1 = temp;
            TZM_State_1 = TZM_State_1 & 0x1E00000;
            TZM_State_1 = TZM_State_1 >> 21;
            State_1 = TZM_State_1;
        }
        else
        {
            State_1 = 3;

            year = temp;
            year = (year) >> 27;

            month = temp;
            month = (month) & 0x1E00000;
            month = (month) >> 21;

            date = temp;
            date = (date) & 0xF8000;
            date = (date) >> 15;

            hour = temp;
            hour = (hour) & 0x1F00;
            hour = (hour) >> 8;

            min = temp;
            min = (min) & 0x3F;
        }

        return 1;
    }


    public static UInt16 JiSuan_TZM_decrypt2(UInt32 JMTZM, ulong[] Data, ref UInt32 State_1, ref UInt32 State_2, ref UInt32 year, ref UInt32 month, ref UInt32 date, ref UInt32 hour, ref UInt32 min)
{
        UInt32 temp = new UInt32();
        UInt32 temp1 = new UInt32();
        UInt32 temp2 = new UInt32();
        UInt32 temp3 = new UInt32();
        UInt32 temp4 = new UInt32();
        //	U32 key[4] = {3293217502, 3508680038, 3225292888, 255898098};
        //UInt32[] key = { 1683217502, 3168680038, 3216892888, 251688098 };

        //UInt32[] key = { 1683217502, 3168680038, 3216892888, 251688098 };

        UInt32[] key = {3293402834, 3429452038, 3942042444, 1793948094};
        UInt32[] plain = new UInt32[2];
        UInt32[] crypt = new UInt32[2];

        UInt32 TZM_State_1 = new UInt32();
        UInt32 TZM_State_2 = new UInt32();
        UInt32 System_Time = new UInt32();

        UInt16 i = new UInt16();
        UInt16 j = new UInt16();

	temp = 0;
	for(i = 0; i < 8; i++)
	{
        temp = (uint)(temp * 10 + Data[i]);
	}

	plain[0] = JMTZM;
	plain[1] = 2404765961;
	tea_encipher2(plain, key, crypt);
	crypt[0] = crypt[ 0 ]%10000000;
	crypt[1] = crypt[ 1 ]%10000000;

	temp = temp ^ crypt[1] ^ crypt[0];


	temp1 = temp & 0x61060C0;
	temp2 = JMTZM & 0x61060C0;
	if(temp1 != temp2)
	{
		return 0;
	}

	temp = temp & 0x1EF9F3F; // 11111001 11101111 10011111 00111111

	TZM_State_2 = temp;
	TZM_State_2 = TZM_State_2 & 0x1F00;
	TZM_State_2 = TZM_State_2 >> 8;
	State_2 = TZM_State_2;

	TZM_State_1 = temp;
	TZM_State_1 = TZM_State_1 & 0x3F;
	//TZM_State_1 = TZM_State_1 >> 15;
	State_1 = TZM_State_1;


	return 1;
}

    // 获取解码特征码
    // 输入参数：FenJi_HM--分机号
    // 输出参数：U8 JieMa_TeZheng_M[8]
    public static void get_JMTZM(UInt32 FenJi_HM, UInt16[] JieMa_TeZheng_M)
    {
        //UInt32[]  key = {3809741235, 4128049392, 1733281440, 3905716589};
	    //UInt32[]  key = {3123451235, 3128012345, 1234581880, 2123456589};
        UInt32[] key = {3809741010, 4128049456, 1733281782, 3905716334};

        UInt32[] plain = new UInt32[2];
        UInt32[] crypt = new UInt32[2];
        UInt16 i = new UInt16();
        UInt16 j = new UInt16();
        UInt32 temp1 = 0;
        UInt32 temp2 = 0;
        UInt16[] temp4 = new UInt16[4];
        UInt16[] data = new UInt16[8];


        for (i = 0; i < 4; i++)
        {
            temp4[i] = (ushort)(new Random().Next(0, 10000) % 256);
        }
        temp2 = U8ArrayToU32(temp4, 0);
        temp2 = temp2 % 10000;

        key[0] = temp2;

        plain[0] = FenJi_HM;
        plain[1] = FenJi_HM << 2;
        tea_encipher(plain, key, crypt);
        temp1 = crypt[0] + crypt[1];
        //temp1 = temp1 & 0x4FFFFFF;


        for (i = 0, j = 7; i < 4; i++, j--)
        {
            JieMa_TeZheng_M[j] = (ushort)(temp1 % 10);
            temp1 = temp1 / 10;
        }

        for (i = 4, j = 3; i < 8; i++, j--)
        {
            JieMa_TeZheng_M[j] = (ushort)(temp2 % 10);
            temp2 = temp2 / 10;
        }
    }


    //
    //参数要求：
    //U32 JiTai_HM;//机台号码 8位十进制
    //U8 JieMa_TeZheng_M[8] ;//解码特征码 8位十进制
    //返回值：
    //校验成功为 1
    //校验失败为 0
    //
    // 校验机台号码和解码校验码是否正确
    public static UInt16 JiaoYan_JTHM_JMTZM(UInt32 FenJi_HM, UInt16[] JieMa_TeZheng_M) // 打码器调用
    {
        //UInt32[] key = { 3123451235, 3128012345, 1234581880, 2123456589 };
        //UInt32[]  key = {3809741235, 4128049392, 1733281440, 3905716589};
        UInt32[] key = {3809741010, 4128049456, 1733281782, 3905716334};


        UInt32[] plain = new UInt32[2];
        UInt32[] crypt = new UInt32[2];
        UInt16 i = new UInt16();
        UInt16 j = new UInt16();
        UInt32 temp1 = 0;
        UInt32 temp2 = 0;
        UInt16[] temp3 = new UInt16[4];
        UInt32 temp4 = new UInt32();
        UInt32 temp = new UInt32();
        UInt16[] data = new UInt16[8];

        temp1 = 0;
        for (i = 0; i < 4; i++)
        {
            temp1 = temp1 * 10 + JieMa_TeZheng_M[i];
        }

        key[0] = temp1;

        plain[0] = FenJi_HM;
        plain[1] = FenJi_HM << 2;
        tea_encipher(plain, key, crypt);
        temp2 = crypt[0] + crypt[1];

        for (i = 4; i > 0; i--)
        {
            data[i - 1] = (ushort)(temp2 % 10);
            temp2 = temp2 / 10;
        }

        for (i = 0, j = 4; i < 4 && j < 8; i++, j++)
        {
            if (data[i] != JieMa_TeZheng_M[j])
            {
                return 0;
            }
        }

        return 1;

    }
}
