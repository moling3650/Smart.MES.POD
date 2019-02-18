using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.IO.Ports;
using System.Configuration;
using System.Media;
using System.Text.RegularExpressions;
using System.Data.OleDb;

namespace LEMES_POD.Voice
{
    public class voice
    {
        static SoundPlayer player = new SoundPlayer();

        /// <summary>
        /// 通过类型，播放声音(线程等待声音播放完)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sleep">需要等待多少毫秒播放声音</param>
        public void ShowVoice(string type, int sleep)
        {
            player.SoundLocation = System.Windows.Forms.Application.StartupPath + "\\Music\\" + type;
            player.LoadAsync();
            player.PlayLooping();
            System.Threading.Thread.Sleep(sleep);
            player.Stop();
        }
        /// <summary>
        /// 通过类型，播放声音(异步，线程不会阻塞)
        /// </summary>
        /// <param name="type"></param>
        public static void ShowVoice(string type)
        {
            player.SoundLocation = System.Windows.Forms.Application.StartupPath + "\\Music\\" + type;
            player.Play();
        }

        /// <summary>
        /// 使用正则表达式验证字符串
        /// </summary>
        /// <param name="reg">验证规则</param>
        /// <param name="value">需要验证的值</param>
        /// <returns></returns>
        public bool CheckString(string reg, string value)
        {
            Regex oReg = new Regex(reg);
            MatchCollection oCollection = oReg.Matches(value);
            if (oCollection.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
