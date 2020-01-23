using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Numerolog
{
    public static class Util
    {
        public const string ALPHA = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        public static string Log="";

        public static void WriteLine(string msg)
        {
            Log += msg + "\r\n";
        }

        public static void ClearLog()
        {
            Log = "";
        }

        public static int CODE_RU(string S)
        {
            int i;
            int cnt = 0;
            string U = S.ToUpper();
            for (i = 0; i < U.Length; i++)
            {
                int idx = ALPHA.IndexOf(U.Substring(i, 1));

                if (idx >= 0)
                {
                    WriteLine(U.Substring(i, 1) + " -> " + MOD9((idx+1)).ToString());
                    cnt += MOD9(idx + 1);
                }
                else
                {
                    WriteLine("Неверный символ! (" + U.Substring(i, 1) + ") -> 0");
                }
            }

            WriteLine(S + " -> " + cnt.ToString());
            return cnt;
        }


        public static int NUM2CODE(string S)
        {
            int i;
            int cnt = 0;
            char[] U = S.ToUpper().ToCharArray();
            for (i = 0; i < U.Length; i++)
            {
                int idx = (int)(U[i]-'0');
                cnt += (idx );
                
            }
            return cnt;
        }


        public static int MOD22(int v)
        {
            return ((v - 1) % 22) + 1;
        }


        public static int MOD9(int v)
        {
            return ((v - 1) % 9) + 1;
        }
    }
}
