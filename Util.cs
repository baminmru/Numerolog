using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace Numerolog
{
    public static class Util
    {
        public const string ALPHA = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        public const string SOGL  = " БВГД  ЖЗ ЙКЛМН ПРСТ ФХЦЧШЩЪ Ь   ";
        public const string DIGS = "0123456789";
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
                   // WriteLine(U.Substring(i, 1) + " -> " + MOD9((idx+1)).ToString());
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


        public static int[] STRING2INTS(string S)
        {
            int i;
            List<int> values = new List<int>();
            string U = S.ToUpper();
            for (i = 0; i < U.Length; i++)
            {
                int idx = ALPHA.IndexOf(U.Substring(i, 1));

                if (idx >= 0)
                {
                    //WriteLine(U.Substring(i, 1) + " -> " + MOD9((idx + 1)).ToString());
                    values.Add(MOD9(idx + 1));
                }
                else
                {
                    WriteLine("Неверный символ! (" + U.Substring(i, 1) + ") -> 0");
                }
            }

            return values.ToArray();
        }

        public static int[] SCODE_RU(string S)
        {
            List<int> values = new List<int>();
            int i;
            string U = S.ToUpper();
            for (i = 0; i < U.Length; i++)
            {
                int idx = SOGL.IndexOf(U.Substring(i, 1));
                
                if (idx >= 0)
                {
                    //WriteLine(U.Substring(i, 1) + " -> " + MOD9((idx + 1)).ToString());
                    values.Add( MOD9(idx + 1));
                }
                else
                {
                    idx = ALPHA.IndexOf(U.Substring(i, 1));
                    if(idx<0) 
                        WriteLine("Неверный символ! (" + U.Substring(i, 1) + ") -> 0");
                    // else пропускаем гласные
                }
            }
            return values.ToArray();
        }


        public static int[] DATE2INTS(string S)
        {
            List<int> values = new List<int>();
            int i;
            int cnt = 0;
            string U = S.ToUpper();
            for (i = 0; i < U.Length; i++)
            {
                int idx = DIGS.IndexOf(U.Substring(i, 1));

                if (idx >= 0)
                {
                    //WriteLine(U.Substring(i, 1) + " -> " + idx .ToString());
                    values.Add(idx );
                }
                else
                {
                    if (idx < 0)
                        WriteLine("Неверный символ! (" + U.Substring(i, 1) + ")");
                    
                }
            }
            return values.ToArray();
        }

        public static int[] REVERS2INTS(string S)
        {
            List<int> values = new List<int>();
            int i;
            int cnt = 0;
            string U = S.ToUpper();
            for (i = 0; i < U.Length; i++)
            {
                int idx = DIGS.IndexOf(U.Substring(i, 1));

                if (idx >= 0)
                {
                    if (idx != 0 && idx != 9)
                        idx = 9 - idx;
                   // WriteLine(U.Substring(i, 1) + " -> " + idx.ToString());
                    values.Add(idx);
                }
                else
                {
                    if (idx < 0)
                        WriteLine("Неверный символ! (" + U.Substring(i, 1) + ")");

                }
            }
            return values.ToArray();
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
            if (v == 0) return 22;
            return ((v - 1) % 22) + 1;
        }


        public static int MOD9(int v)
        {
            if (v == 0) return 9;
            return ((v - 1) % 9) + 1;
        }

        public static int MOD90(int v)
        {
            return ((v ) % 10) ;
        }

        public static string SavePath="";
        public static bool OpenAfterSave;
        public static bool SaveDayly;
        public static bool SaveImmun;
        public static bool SaveIM;

        public static void SaveCFG()
        {
            

            XmlDocument xDoc = new XmlDocument();

            xDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?><config/>");
            XmlNode n = xDoc.LastChild;
            XmlAttribute attr;

            if (SavePath != "")
            {
                if (Directory.Exists(SavePath))
                {
                    attr = xDoc.CreateAttribute("path");
                    attr.Value = SavePath;
                    n.Attributes.Append(attr);
                }
            }
            else
            {
                

                SavePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Numerolog";
                if (!Directory.Exists(SavePath))
                {
                    Directory.CreateDirectory(SavePath);
                }
                if (Directory.Exists(SavePath))
                {
                    attr = xDoc.CreateAttribute("path");
                    attr.Value = SavePath;
                    n.Attributes.Append(attr);
                }
            }

            
            attr = xDoc.CreateAttribute("OpenAfterSave");
            attr.Value = OpenAfterSave.ToString();
            n.Attributes.Append(attr);


            attr = xDoc.CreateAttribute("SaveDayly");
            attr.Value = SaveDayly.ToString();
            n.Attributes.Append(attr);

            attr = xDoc.CreateAttribute("SaveImmun");
            attr.Value = SaveImmun.ToString();
            n.Attributes.Append(attr);

            attr = xDoc.CreateAttribute("SaveIM");
            attr.Value = SaveIM.ToString();
            n.Attributes.Append(attr);


            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            try
            {
                xDoc.Save(path + @"\Numerolog_Config.xml");
            }
            catch (Exception ex)
            {
               
            }

        }

        public static void ReadCFG()
        {
            
            try
            {
                XmlDocument xDoc = new XmlDocument();

                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                xDoc.Load(path + @"\Numerolog_Config.xml");

                XmlNode n = xDoc.LastChild;
                XmlAttribute attr;

                try
                {
                    attr = n.Attributes["path"];
                    if (Directory.Exists(attr.Value))
                    {
                        SavePath = attr.Value;
                    }
                    else
                    {
                        SavePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Interview";
                        if (!Directory.Exists(SavePath))
                        {
                            Directory.CreateDirectory(SavePath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                }

                try
                {
                    attr = n.Attributes["OpenAfterSave"];
                    if( attr.Value.ToLower()=="true")
                        OpenAfterSave =true;
                    else
                        OpenAfterSave = false;
                }
                catch (Exception ex)
                {
                    OpenAfterSave = false;
                }

                try
                {
                    attr = n.Attributes["SaveDayly"];
                    if (attr.Value.ToLower() == "true")
                        SaveDayly = true;
                    else
                        SaveDayly = false;
                }
                catch (Exception ex)
                {
                    SaveDayly = false;
                }

                try
                {
                    attr = n.Attributes["SaveImmun"];
                    if (attr.Value.ToLower() == "true")
                        SaveImmun = true;
                    else
                        SaveImmun = false;
                }
                catch (Exception ex)
                {
                    SaveImmun = false;
                }


                try
                {
                    attr = n.Attributes["SaveIM"];
                    if (attr.Value.ToLower() == "true")
                        SaveIM = true;
                    else
                        SaveIM = false;
                }
                catch (Exception ex)
                {
                    SaveIM = false;
                }



            }
            catch (Exception ex)
            {
               
            }

        }

    }
}
