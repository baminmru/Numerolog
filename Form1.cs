using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace Numerolog
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region "VARS"

        private int cF,cI,cO, cD,cM,cY;
        private int[] kCH =  new int[6];
        private int[] kG = new int[6];
        private int[] FO = new int[6];
        private int[] UB = new int[6];
        private int[] FNZ = new int[6];

        private void cmdSavePicture_Click(object sender, EventArgs e)
        {
            cmdSavePicture.Visible = false;
            cmdCalc1.Visible = false;
            txtLOG.Visible = false;
            
            try
            {
                string fn = Path.Combine(Path.GetTempPath(), "Соты_" + DateTime.Now.ToString("yyyyMMddHHmmss")+ ".png");
                Bitmap bmp = new Bitmap(tabPage2.Width, tabPage2.Height +  cmdCalc1.Top);

                //using (Graphics G = Graphics.FromImage(bmp))
                //{
                //    G.Clear(Color.Black);
                //}

                tabPage1.DrawToBitmap(bmp, new Rectangle(0, 0,
                            tabPage2.Width,  cmdCalc1.Top));

                tabPage2.DrawToBitmap(bmp, new Rectangle(0,  cmdCalc1.Top,
                            tabPage2.Width, tabPage2.Height));

                bmp.Save(fn, ImageFormat.Png);
           
                //MessageBox.Show("Сохранено в файл: " + fn);
                System.Diagnostics.Process.Start(fn);
            }
            catch
            {
                // handle exceptions here.

               
            }

            cmdSavePicture.Visible = true;
            cmdCalc1.Visible = true;
            txtLOG.Visible = true;
        }

        private string[] UBS = new string[6];
        private string[] SY = new string[6];

        private int[] rodW = { 2, 3, 6, 12, 14, 15, 17, 18, 20, 21, 22 };
        private int[] rodM = { 1, 4, 5, 7, 8, 9, 10, 11, 13, 16, 19};


        #endregion


        private void cmdCalc1_Click(object sender, EventArgs e)
        {
            Util.ClearLog();
            cF = Util.MOD22(Util.CODE_RU(txtF.Text));
            Util.WriteLine("=" + cF.ToString());
            Util.WriteLine("------------------------");
            cI = Util.MOD22(Util.CODE_RU(txtI.Text));
            Util.WriteLine("=" + cI.ToString());
            Util.WriteLine("------------------------");
            cO = Util.MOD22(Util.CODE_RU(txtO.Text));
            Util.WriteLine("=" + cO.ToString());
            Util.WriteLine("------------------------");

            vF.Text = cF.ToString();
            vI.Text = cI.ToString();
            vO.Text = cO.ToString();

            string temp =( txtF.Text + txtI.Text + txtO.Text).ToUpper();
            string FIO = "";

            int i;
            for (i = 0; i < temp.Length; i++)
            {
                if (Util.ALPHA.Contains(temp.Substring(i, 1))){
                    FIO += temp.Substring(i, 1);
                }
            }
            if (temp.Length != FIO.Length)
            {
                MessageBox.Show("В ФИО есть символы, которые не входят в теекущий алфавит", "Внимание!");
            }
            

            for (i = 1; i <= 5; i++)
            {
                UBS[i] = "";
            }

            for ( i = 0; i < FIO.Length; i++)
            {
                int idx = (i % 5) + 1;
                UBS[idx] += FIO.Substring(i, 1);
            }

            FNZ[3] = 0;

            for (i = 1; i <= 5; i++)
            {
                Util.WriteLine("УБ" + i.ToString()+"->" + UBS[i]);
            }
            Util.WriteLine("------------------------");
            for (i = 1; i <= 5; i++) {
                Util.WriteLine("УБ" + i.ToString());
                UB[i] = Util.MOD9(Util.CODE_RU(UBS[i]));
                Util.WriteLine("УБ" + i.ToString() +"="+ UB[i].ToString());
                Util.WriteLine("------------------------");
                FNZ[3]+=UB[i];
            }

            FNZ[3] = Util.MOD22(FNZ[3]);

            if (rodM.Contains(UB[4]))
            {
                txtMW.Text = "Муж.";
            }
            else
            {
                txtMW.Text = "Жен.";
            }

            sMW.Text = txtMW.Text;

            cD = Util.MOD22(bDate.Value.Day);
            cM = bDate.Value.Month;
            int cc = Util.NUM2CODE(bDate.Value.Year.ToString());
            cY = Util.MOD22(cc);
            kCH[1] = Math.Abs(cD - cM);
            kCH[2] = Math.Abs(cD - cY);
            kCH[3] = Math.Abs(kCH[1] - kCH[2]);
            kCH[4] = Math.Abs(cM - cY);
            kCH[5] = Util.MOD22(kCH[1]+kCH[2]+kCH[3]+kCH[4]);


            kG[1] = Util.MOD22(Math.Abs(cF - cI));
            kG[2] = Util.MOD22(Math.Abs(cF - cO));
            kG[3] = Math.Abs(kG[1] - kG[2]);
            kG[4] = Util.MOD22(Math.Abs(cI - cO));
            kG[5] = Util.MOD22(kG[1] + kG[2] + kG[3] + kG[4]);

            for(int j=1;j<=5;j++)
                FO[j] = Util.MOD22(kCH[j] + kG[j]);


            FNZ[1] = Math.Abs(FO[1] - FO[2]);
            FNZ[2] = Math.Abs(FO[4] - FO[5]);


            for (i = 1; i <= 5; i++)
            {
                SY[i] = "";

                for (int j = 0; j < 5; j++)
                {
                    if (j * 5 * kG[1] + (i - 1) * kG[1] < 110)
                        SY[i] = (j * 5 * kG[1] + (i - 1) * kG[1]).ToString() + " - " + ((j * 5 + 1) * kG[1] + (i - 1) * kG[1] - 1).ToString() + "\r\n" + SY[i];
                    else
                    {
                        if (i != 3)
                            SY[i] = "\r\n" + SY[i];
                    }

                }
            }


            txtD.Text = cD.ToString();
            txtM.Text = cM.ToString();
            txtY.Text = cY.ToString();


            

            txtKCH1.Text = kCH[1].ToString();
            txtKCH2.Text = kCH[2].ToString();
            txtKCH3.Text = kCH[3].ToString();
            txtKCH4.Text = kCH[4].ToString();
            txtKCH5.Text = kCH[5].ToString();


            txtKG1.Text = kG[1].ToString();
            txtKG2.Text = kG[2].ToString();
            txtKG3.Text = kG[3].ToString();
            txtKG4.Text = kG[4].ToString();
            txtKG5.Text = kG[5].ToString();


            txtFO1.Text = FO[1].ToString();
            txtFO2.Text = FO[2].ToString();
            txtFO3.Text = FO[3].ToString();
            txtFO4.Text = FO[4].ToString();
            txtFO5.Text = FO[5].ToString();

            txtUB1.Text = UB[1].ToString();
            txtUB2.Text = UB[2].ToString();
            txtUB3.Text = UB[3].ToString();
            txtUB4.Text = UB[4].ToString();
            txtUB5.Text = UB[5].ToString();

            txtFNZ1.Text = FNZ[1].ToString();
            txtFNZ2.Text = FNZ[2].ToString();
            txtFNZ3.Text = FNZ[3].ToString();

            // соты


            sCH1.Text = kCH[1].ToString();
            sCH2.Text = kCH[2].ToString();
            sCH3.Text = kCH[3].ToString();
            sCH4.Text = kCH[4].ToString();
            sCH5.Text = kCH[5].ToString();


            sG1.Text = kG[1].ToString();
            sG2.Text = kG[2].ToString();
            sG3.Text = kG[3].ToString();
            sG4.Text = kG[4].ToString();
            sG5.Text = kG[5].ToString();


            sFO1.Text = FO[1].ToString();
            sFO2.Text = FO[2].ToString();
            sFO3.Text = FO[3].ToString();
            sFO4.Text = FO[4].ToString();
            sFO5.Text = FO[5].ToString();

            sUB1.Text = UB[1].ToString();
            sUB2.Text = UB[2].ToString();
            sUB3.Text = UB[3].ToString();
            sUB4.Text = UB[4].ToString();
            sUB5.Text = UB[5].ToString();

            sFNZ1.Text = FNZ[1].ToString();
            sFNZ2.Text = FNZ[2].ToString();
            sFNZ3.Text = FNZ[3].ToString();

            sY1.Text = SY[1];
            sY2.Text = SY[2];
            sY3.Text = SY[3];
            sY4.Text = SY[4];
            sY5.Text = SY[5];

            txtLOG.Text = Util.Log;

        }
    }
}
