using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace NVBarcode.Serial
{
    public class SerialManager
    {
        public enum Enters
        {
            Enter16,
            Enter32,
        };

        public Enters Enter;

        /// <summary>
        /// 将2进制转换为指定字母的索引表
        /// </summary>
        Hashtable BitList = new Hashtable();


        public SerialManager(Enters enter,string fills)
        {
            if (enter == Enters.Enter16)
            {
                FillEnter16ToBitList(fills);
                this.Enter = enter;
            }
            else if (enter == Enters.Enter32)
            {
                FillBitList(fills);
                this.Enter = enter;
            }
        }

        private void FillBitList(string fills)
        {
            BitList.Add("00000", fills[0]);
            BitList.Add("00001", fills[1]);
            BitList.Add("00010", fills[2]);
            BitList.Add("00011",fills[3]);
            BitList.Add("00100",fills[4]);
            BitList.Add("00101",fills[5]);
            BitList.Add("00110",fills[6]);
            BitList.Add("00111",fills[7]);
            BitList.Add("01000",fills[8]);
            BitList.Add("01001",fills[9]);
            BitList.Add("01010",fills[10]);
            BitList.Add("01011",fills[11]);
            BitList.Add("01100",fills[12]);
            BitList.Add("01101",fills[13]);
            BitList.Add("01110",fills[14]);
            BitList.Add("01111",fills[15]);
            BitList.Add("10000",fills[16]);
            BitList.Add("10001",fills[17]);
            BitList.Add("10010",fills[18]);
            BitList.Add("10011",fills[19]);
            BitList.Add("10100",fills[20]);
            BitList.Add("10101",fills[21]);
            BitList.Add("10110",fills[22]);
            BitList.Add("10111",fills[23]);
            BitList.Add("11000",fills[24]);
            BitList.Add("11001",fills[25]);
            BitList.Add("11010",fills[26]);
            BitList.Add("11011",fills[27]);
            BitList.Add("11100",fills[28]);
            BitList.Add("11101",fills[29]);
            BitList.Add("11110",fills[30]);
            BitList.Add("11111",fills[31]);
        }
        private void FillEnter16ToBitList(string fills)
        {
            BitList.Add("0000", fills[0]);
            BitList.Add("0001", fills[1]);
            BitList.Add("0010", fills[2]);
            BitList.Add("0011", fills[3]);
            BitList.Add("0100", fills[4]);
            BitList.Add("0101", fills[5]);
            BitList.Add("0110", fills[6]);
            BitList.Add("0111", fills[7]);
            BitList.Add("1000", fills[8]);
            BitList.Add("1001", fills[9]);
            BitList.Add("1010", fills[10]);
            BitList.Add("1011", fills[11]);
            BitList.Add("1100", fills[12]);
            BitList.Add("1101", fills[13]);
            BitList.Add("1110", fills[14]);
            BitList.Add("1111", fills[15]);
        }

        /// <summary>
        /// 将10进制转为任意字符集的进制
        /// </summary>
        /// <param name="enter"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public string TransDecimal(int decimalNum,int enter,string[] strs)
        {
            return null;
        }


        /// <summary>
        /// 10进制转2进制
        /// </summary>
        /// <returns></returns>
        public string TransDecimalToBit(int n)
        {
            string str = "";
            if (n == 0)
                return "0";
            while (n > 0)
            {
                str = (n % 2).ToString() + str ;
                n /= 2;
            }
            return str;
        }

        /// <summary>
        /// 获取2进制数的2进制数组形式，用于计算16进制或32进制
        /// </summary>
        public string[] GetBitsByBit(string bit)
        {
            int count = 0;
            if (this.Enter == Enters.Enter16)
            {
                count = 4;
            }
            else if (this.Enter == Enters.Enter32)
            {
                count = 5;
            }

            string DescBits=GetDescStr(bit);  //把这个字符串倒过来
            int i = DescBits.Length / count;  //获取循环次数,至少是1次
            if (DescBits.Length % count > 0)
                i += 1;
            string[] groups=new string[i]; //生成一个字符串数组，之后返回这个数组
            for (int j = 0; j < i; j++)
            {
                string str = "";
                for (int n = 0; n < count; n++)
                {
                    if (j * count + n >= DescBits.Length)  //这种情况是数组越界,不足位补0的情况
                    {
                        str = "0"+str;
                    }
                    else
                    {
                        str = DescBits[j * count + n].ToString() + str;
                    }
                }
                groups[i - j-1] = str;   //放进数组的时候，要倒过来放
            }

            return groups;
        }

        //获取一个字符串的倒序
        private string GetDescStr(string str)
        {
            string DescStr = "";
            for (int i = str.Length - 1; i > -1; i--)
            {
                DescStr += str[i];
            }
            return DescStr;
        }


        /// <summary>
        /// 将10进制
        /// </summary>
        /// <param name="decimalStr"></param>
        /// <param name="enter"></param>
        /// <returns></returns>
        public string GetEnterString(int dec)
        {
            string BitStr = TransDecimalToBit(dec);
            string[] bits = GetBitsByBit(BitStr);

            string ents = "";
            foreach (string s in bits)
            {
                ents += BitList[s];
            }
            return ents;
        }
    }
}
