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
using System.Windows.Forms.DataVisualization.Charting;

namespace Numerolog
{
    public partial class Form1 : Form
    {
        #region "VARS"

        private int cF,cI,cO, cD,cM,cY;
        private int[] kCH =  new int[6];
        private int[] kG = new int[6];
        private int[] FO = new int[6];
        private int[] UB = new int[6];
        private int[] FNZ = new int[6];
        private int[] kB ;
        private int[] sB;

        private string[] UBS = new string[6];
        private string[] SY = new string[6];

        private int[] rodW = { 2, 3, 6, 12, 14, 15, 17, 18, 20, 21, 22 };
        private int[] rodM = { 1, 4, 5, 7, 8, 9, 10, 11, 13, 16, 19 };
        private int[] dayActiv = new int[24];
        private int[] sumA = new int[3] { 0, 0, 0 };

        private int[] IMUNBlue;
        private int[] IMUNRed;

        private int[] IMLine;
        private int[] IMBarsPlus;
        private int[] IMBarsMinus;


        private int KExtS;
        private int[] kIntS;


        #endregion
        #region "load_save"

        public Form1()
        {
            InitializeComponent();
        }
        private void cmdSavePicture_Click(object sender, EventArgs e)
        {
            //cmdSavePicture.Visible = false;
            //cmdCalc1.Visible = false;
            //txtLOG.Visible = false;
         
            try
            {
                int height = cmdCalc1.Top + tabSOTI.Height;
                int pos = 0;
                if (chkDayly.Checked)
                    height += tabDayly.Height;
                if(chkIM.Checked)
                    height += tabIM.Height;
                if (chkIMUN.Checked)
                    height += tabIMMUNITY.Height;


                string fn = Path.Combine(Util.SavePath, txtF.Text +"_"+txtI.Text +"_" + txtO.Text + "_" + DateTime.Now.ToString("yyyyMMddHHmmss")+ ".png");
                Bitmap bmp = new Bitmap(tabParams.Width, height);

                using (Graphics G = Graphics.FromImage(bmp))
                {
                    G.Clear(Color.White);
                }

                pos = 0;
                tabParams.DrawToBitmap(bmp, new Rectangle(0, pos,
                            tabParams.Width,  cmdCalc1.Top));

                pos += cmdCalc1.Top;

                mainTab.SelectedTab = tabSOTI;
                tabSOTI.DrawToBitmap(bmp, new Rectangle(0,  pos,
                            tabParams.Width, tabSOTI.Height));
                
                pos += tabSOTI.Height;

                if (chkDayly.Checked)
                {
                    
                    mainTab.SelectedTab = tabDayly;
                    tabDayly.DrawToBitmap(bmp, new Rectangle(0, pos,
                              tabParams.Width, tabDayly.Height));
                    pos += tabDayly.Height;
                }

                if (chkIM.Checked)
                {
                    mainTab.SelectedTab = tabIM;
                    tabIM.DrawToBitmap(bmp, new Rectangle(0, pos,
                              tabParams.Width, tabIMMUNITY.Height));
                    pos += tabIM.Height;
                }

                if (chkIMUN.Checked)
                {
                    mainTab.SelectedTab = tabIMMUNITY;
                    tabIMMUNITY.DrawToBitmap(bmp, new Rectangle(0, pos,
                              tabParams.Width, tabIMMUNITY.Height));
                    pos += tabIMMUNITY.Height;
                }


               
                bmp.Save(fn, ImageFormat.Png);

                mainTab.SelectedTab = tabParams;

                if (Util.OpenAfterSave)
                    System.Diagnostics.Process.Start(fn);
                else
                    MessageBox.Show("Сохранено в файл: " + fn);
            }
            catch
            {
                // handle exceptions here.

               
            }

            //cmdSavePicture.Visible = true;
            //cmdCalc1.Visible = true;
            //txtLOG.Visible = true;
        }


        private bool InLoad = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            Util.ReadCFG();
            InLoad = true;
            txtSaveFolder.Text = Util.SavePath;
            chkOpenSavedFile.Checked = Util.OpenAfterSave;
            chkDayly.Checked = Util.SaveDayly;
            chkIM.Checked = Util.SaveIM;
            chkIMUN.Checked = Util.SaveImmun;
            dtpProgFrom.Value = DateTime.Today;
            dtpProgTo.Value = DateTime.Today.AddDays(5);

            InLoad = false;
        }

        private void chkOpenSavedFile_CheckedChanged(object sender, EventArgs e)
        {
            if (InLoad) return;
            Util.SaveDayly = chkDayly.Checked;
            Util.SaveImmun = chkIMUN.Checked;
            Util.SaveIM = chkIM.Checked;
            Util.OpenAfterSave = chkOpenSavedFile.Checked;
            Util.SaveCFG();
        }

        private void cmdSaveFolder_Click(object sender, EventArgs e)
        {
            fbd.ShowNewFolderButton = true;
            //fbd.RootFolder = Util.SavePath;
            fbd.Description = "Папка для сохранения результата";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtSaveFolder.Text = fbd.SelectedPath;
                Util.SavePath = txtSaveFolder.Text;
                Util.SaveCFG();
            }
        }

        #endregion
        #region "Соты"

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
                MessageBox.Show("В ФИО есть символы, которые не входят в текущий алфавит", "Внимание!");
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
                UB[i] = Util.MOD22(Util.CODE_RU(UBS[i]));
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
            kCH[1] = Util.MOD22(Math.Abs(cD - cM));
            kCH[2] = Util.MOD22(Math.Abs(cD - cY));
            kCH[3] = Util.MOD22(Math.Abs(kCH[1] - kCH[2]));
            kCH[4] = Util.MOD22(Math.Abs(cM - cY));
            kCH[5] = Util.MOD22(kCH[1]+kCH[2]+kCH[3]+kCH[4]);


            kG[1] = Util.MOD22(Math.Abs(cF - cI));
            kG[2] = Util.MOD22(Math.Abs(cF - cO));
            kG[3] = Util.MOD22(Math.Abs(kG[1] - kG[2]));
            kG[4] = Util.MOD22(Math.Abs(cI - cO));
            kG[5] = Util.MOD22(kG[1] + kG[2] + kG[3] + kG[4]);

            for(int j=1;j<=5;j++)
                FO[j] = Util.MOD22(kCH[j] + kG[j]);


            FNZ[1] = Util.MOD22(Math.Abs(FO[1] - FO[2]));
            FNZ[2] = Util.MOD22(Math.Abs(FO[4] - FO[5]));


            for (i = 1; i <= 5; i++)
            {
                SY[i] = "";
                int from = 0;
                int to = 0;

                for (int j = 0; j < 5; j++)
                {

                    from = (j * 5 * kG[1] + (i - 1) * kG[1] + 1);
                    to = ((j * 5 + 1) * kG[1] + (i - 1) * kG[1] - 1 + 1);
                    if (from == 1) from = 0;

                    if (j * 5 * kG[1] + (i - 1) * kG[1] <= 110)
                    {
                        SY[i] = from.ToString() + " - " + to.ToString() + "\r\n" + SY[i];
                    }
                    else
                    {
                        if (i != 3)
                            SY[i] = "\r\n" + SY[i];
                    }

                }
            }

            DaylyCalc();

            IMMUNITYCalc("ЗДОРОВЬЕ");

            CalcOutput();
        }

        private void numImmFrom_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                loadChartIMUNITY();
            }
            catch { }
            
        }

        private void numImmTo_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                loadChartIMUNITY();
            }
            catch { }
        }

        private void CalcOutput()
        {
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

        private void cmdCalcWord_Click(object sender, EventArgs e)
        {
            string[] words = txtWORD.Text.ToUpper().Split(' ');


            
            
            string sOut = "";
            int sSum = 0;
            foreach (string w in words)
            {
                if (w != "")
                {
                    int[] sTemp = Util.STRING2INTS(w);
                    for (int l = 0; l < w.Length; l++)
                    {
                        sOut += w.Substring(l, 1) + " ";
                    }

                    sOut += "\r\n";

                    for (int l = 0; l < sTemp.Length; l++)
                    {

                        sOut += sTemp[l].ToString() + " ";
                        sSum += sTemp[l];
                    }

                    sOut += "\r\n\r\n";
                }
                
            }
            sOut += "СУММА = " + Util.MOD22(sSum).ToString();


            sOut += "\r\n\r\n";

            sOut += WordCalc(txtWORD.Text, txtF.Text + txtI.Text + txtO.Text);

            if (sOut != "")
                txtWordOut.Text = (sOut);





           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dateCritFrom.Value >= dateCritTo.Value) return;
            DateTime d = dateCritFrom.Value;
            int eqCnt=0;
            int[] kCH = new int[6];
            StringBuilder sb = new StringBuilder();

            while (d <= dateCritTo.Value)
            {
               

                int cD = Util.MOD22(d.Day);
                int cM = d.Month;
                int cc = Util.NUM2CODE(d.Year.ToString());
                int cY = Util.MOD22(cc);
                
                kCH[1] = Util.MOD22(Math.Abs(cD - cM));
                kCH[2] = Util.MOD22(Math.Abs(cD - cY));
                kCH[3] = Util.MOD22(Math.Abs(kCH[1] - kCH[2]));
                kCH[4] = Util.MOD22(Math.Abs(cM - cY));
                kCH[5] = Util.MOD22(kCH[1] + kCH[2] + kCH[3] + kCH[4]);

                System.Diagnostics.Debug.Print(d.ToString("dd.MM.yyyy") + " " + kCH[1].ToString() + " " + kCH[1].ToString() + " " + kCH[2].ToString() + " " + kCH[3].ToString() + " " + kCH[4].ToString() + " " + kCH[5].ToString());
                

                for (int i=1;i<23;i++)
                {
                    eqCnt = 0;
                    for (int j =1; j <= 5; j++)
                    {
                        if (i == kCH[j])
                        {
                            eqCnt++;
                        }
                    }
                    if (eqCnt >= 3)
                    {
                        sb.AppendLine(d.ToString("dd.MM.yyyy") + " -> (" + eqCnt.ToString() + ")  [ " + kCH[1].ToString() + " " + kCH[2].ToString() + " " + kCH[3].ToString() + " " + kCH[4].ToString() + " " + kCH[5].ToString() + "]");
                        break;
                    }
                }

                

                d = d.AddDays(1);
            }

            txtCritLog.Text = sb.ToString();


        }




        #endregion

        #region "Суточная активность"

        private void DaylyCalc()
        {
            // код болезни
            Util.WriteLine("-------------- код болезни ---------------");
            kB = Util.DATE2INTS(bDate.Value.ToString("ddMMyyyy"));
            int[] tempKB = Util.SCODE_RU(txtF.Text + txtI.Text + txtO.Text);
            sumA[0] = 0;
            sumA[1] = 0;
            sumA[2] = 0;
            string sOut = "";
            for (int l = 0; l < kB.Length; l++)
            {
                sOut += kB[l].ToString() + " ";
            }
            Util.WriteLine(sOut);
            sOut = "";
            for (int l = 0; l < tempKB.Length; l++)
            {
                sOut += tempKB[l].ToString() + " ";
                kB[l % 8] += tempKB[l];
                if (l % 8 == 7)
                {
                    Util.WriteLine(sOut);
                    sOut = "";
                }
            }
            if (sOut != "")
                Util.WriteLine(sOut);
            sOut = "";
            int sumKB = 0;
            Util.WriteLine("----------------");
            for (int l = 0; l < kB.Length; l++)
            {
                sumKB += Util.MOD9(kB[l]);
                sOut += Util.MOD9(kB[l]).ToString() + " ";
            }
            Util.WriteLine(sOut + " КБ=" + sumKB.ToString());
            sOut = "";



            for (int l = 0; l < kB.Length; l++)
            {
                dayActiv[l] = Util.MOD9(kB[l]);
                sumA[0] += dayActiv[l];
                dayActiv[l + 8] = Util.MOD9(kB[l] + 1);
                sumA[1] += dayActiv[l + 8];
                dayActiv[l + 16] = Util.MOD9(kB[l] + 2);
                sumA[2] += dayActiv[l + 16];
            }

            for (int l = 0; l < 8; l++)
            {
                sOut += dayActiv[l].ToString() + " ";
            }

            Util.WriteLine(sOut + " =" + sumA[0].ToString() + " БНЧ1=" + ((int)(100.0 / 72 * sumA[0])).ToString());
            sOut = "";
            for (int l = 8; l < 16; l++)
            {
                sOut += dayActiv[l].ToString() + " ";
            }

            Util.WriteLine(sOut + " =" + sumA[1].ToString() + " БНЧ2=" + ((int)(100.0 / 72 * sumA[1])).ToString());
            sOut = "";
            for (int l = 16; l < 24; l++)
            {
                sOut += dayActiv[l].ToString() + " ";
            }

            Util.WriteLine(sOut + " =" + sumA[2].ToString() + " БНЧ3=" + ((int)(100.0 / 72 * sumA[2])).ToString());


            int BNCH = (int)((((int)(100.0 / 72 * sumA[0])) + ((int)(100.0 / 72 * sumA[1])) + ((int)(100.0 / 72 * sumA[2]))) / 3.0);
            Util.WriteLine("БНЧ0=" + BNCH.ToString());

            int KV = (int)(0.7 * BNCH);
            Util.WriteLine("КВ=" + KV.ToString());



            // вывод
            loadChartH();

            txtBNCH.Text = BNCH.ToString();
            txtKV.Text = KV.ToString();
            txtKB.Text = sumKB.ToString();
        }

        private void cmdProg_Click(object sender, EventArgs e)
        {
            Util.ClearLog();
          
            int bDay = bDate.Value.Day;
            cD = Util.MOD22(bDate.Value.Day);
            cM = bDate.Value.Month;
            int cc = Util.NUM2CODE(bDate.Value.Year.ToString());
            cY = Util.MOD22(cc);
           
            int V1, V2, V3, V4, V5, V6, V7;
            int K;
            int pD,pM,pY;

            bool pMode;

            if(bDay < 14 || bDay > 22)
            {
                pMode = false;
         
                V6 = 0;
                V7 = 0;

                V1 = Util.MOD22(Math.Abs(cD - cM));
                V2 = Util.MOD22(Math.Abs(cD - cY));
                V3 = Util.MOD22(Math.Abs(V1 - V2));
                V4 = Util.MOD22(Math.Abs(cM - cY));
                V5 = Util.MOD22(V1 + V2 + V3 + V4);
                K = V1;
            }
            else
            {
                pMode = true;
                V1 = cD;
                V2 = Util.MOD22(Math.Abs(cD - cM));
                V3 = Util.MOD22(Math.Abs(cD - cY));
                V4 = Util.MOD22(Math.Abs(V2 - V3));
                V5 = Util.MOD22(Math.Abs(cM - cY));
                V6 = Util.MOD22( V2 + V3 + V4 + V5);
                V7 = Util.MOD22( V1 + V2 + V3 + V4 + V5);
                K = V2;
            }

            List<int> lV = new List<int>();
            lV.Add(V1);
            lV.Add(V2);
            lV.Add(V3);
            lV.Add(V4);
            lV.Add(V5);
            if (pMode)
            {
                lV.Add(V6);
                lV.Add(V7);
            }

            int pR = 0;
            if(chkProgFIO.Checked)
                pR = Util.MOD22(Util.CODE_RU(txtF.Text + txtI.Text + txtO.Text));
            else
                pR = Util.MOD22(Util.CODE_RU(txtI.Text));

            DateTime ProgDate;
            DateTime yearBerthday;
            ProgDate = dtpProgFrom.Value;


            while(ProgDate <= dtpProgTo.Value)
            {
               

                yearBerthday = new DateTime(ProgDate.Year, bDate.Value.Month, bDate.Value.Day);

                if(ProgDate >= yearBerthday)
                {
                    pY = ProgDate.Year;
                }
                else
                {
                    pY = ProgDate.Year - 1;
                }


                // расчет на день
                pM = ProgDate.Month;
                pD = ProgDate.Day + bDate.Value.Day;
                int days = DateTime.DaysInMonth(ProgDate.Year, ProgDate.Month);
                if (pD > days)
                {
                    pD -= days;
                }

                if (pD > days)
                {
                    pD -= days;
                    pM +=1;
                }

                /////////////////////////////////////////

                DateTime pgDate = new DateTime(pY, pM, pD);
                             

                kB = Util.DATE2INTS(pgDate.ToString("ddMMyyyy"));
                
                
               
                int pQ = 0;
                Util.WriteLine("--------------------------------------------------------");
                for (int l = 0; l < kB.Length; l++)
                {
                    pQ += kB[l];
                }
                pQ = Util.MOD22(pQ);
              
                int pF;
                int Dt, Mt, Yt, At;
                if (pD > 22) 
                    Dt = pD - 22;
                else
                    Dt = pD;
                Mt = pM;
                cc = Util.NUM2CODE(pY.ToString());
                Yt = Util.MOD22(cc);

                pF = Util.MOD22(Dt + Mt + Yt); 

                decimal v =Math.Ceiling( (decimal) (pY - bDate.Value.Year ) / K);

                int sig ;


                if( ((int) v % 2) == 0)
                {
                    sig = -1;
                }
                else
                {
                    sig = 1;
                }
                
                int pT = Util.MOD22( Math.Abs( pQ + pR   * sig));

                int pL = Util.MOD22(Math.Abs(pF + pR * sig));

               

                string DayType = "+";

                if(lV.Contains(pQ) || lV.Contains(pF))
                {
                    DayType = "-";
                }else if (lV.Contains(pT) || lV.Contains(pL))
                {
                    DayType = "+-";
                }

                Util.WriteLine("Day: " + ProgDate.ToString("dd/MM/yyyy") + " -> PG Day: " + pgDate.ToString("dd/MM/yyyy") + " =  " + DayType);

                if (pMode)
                {
                    Util.WriteLine("[ " + pQ.ToString()
                    + ", " + pF.ToString()
                    + ", " + pT.ToString()
                    + ", " + pL.ToString()
                    + "] -> [ " + V1.ToString()
                    + ", " + V2.ToString()
                    + ", " + V3.ToString()
                    + ", " + V4.ToString()
                    + ", " + V5.ToString()
                    + ", " + V6.ToString()
                    + ", " + V7.ToString()
                    + "]"
                    );
                }
                else
                {
                    Util.WriteLine("[ " + pQ.ToString()
                    + ", " + pF.ToString()
                    + ", " + pT.ToString()
                    + ", " + pL.ToString()
                    + "] -> [ " + V1.ToString()
                    + ", " + V2.ToString()
                    + ", " + V3.ToString()
                    + ", " + V4.ToString()
                    + ", " + V5.ToString()
                    + "]"
                    );
                }

                txtProgOut.Text = Util.Log;


                //////////////////////////////////////////


                ProgDate = ProgDate.AddDays(1);
            }


        }

        private void loadChartH()
        {
                
                double valAvg = 0.0;
                
                int ii;
                
                   
                chartH.Titles.Clear();
                chartH.Titles.Add("Суточная активность");

                // Set chart title font
                chartH.Titles[0].Font = new Font("Times New Roman", 14, FontStyle.Bold);

                // Set chart title color
                chartH.Titles[0].ForeColor = Color.Blue;

                // Set border title color
                // chartH.Titles[0].BorderColor = Color.Black

                // Set background title color
                chartH.Titles[0].BackColor = Color.White;

                // Set Title Alignment
                chartH.Titles[0].Alignment = System.Drawing.ContentAlignment.MiddleCenter;

                // Set Title Alignment
                chartH.Titles[0].ToolTip = chartH.Titles[0].Text;


                chartH.Series.Clear();

                int[] shiftedActiv = new int[24];

                for (ii = 0; ii <= 23; ii++)
                {
                    int idx = (int)((ii + numBHour.Value) % 24);
                    shiftedActiv[ii] = dayActiv[idx];
                    valAvg += dayActiv[ii];
                }
                valAvg /= 24;


            string seriesName;

            chartH.ChartAreas[0].AxisX.Interval = 1; 
            chartH.ChartAreas[0].AxisX.MajorGrid.Interval = 1;
            chartH.ChartAreas[0].AxisY.Interval = 1;
            chartH.ChartAreas[0].AxisY.MajorGrid.Interval = 1;

            seriesName = "Значения";
            chartH.Series.Add(seriesName);
            chartH.Series[seriesName].ChartType = SeriesChartType.Line;
            chartH.Series[seriesName].BorderWidth = 2;
            chartH.Series[seriesName].Label = "#VAL{#,##0}";

            seriesName = "Среднее = " + valAvg.ToString("#0.00");
            chartH.Series.Add(seriesName);
            chartH.Series[seriesName].ChartType = SeriesChartType.Line;
            chartH.Series[seriesName].BorderWidth = 2;
            //chartH.Series[seriesName].Label = "#VAL{#,##0.00}";

            //seriesName = "A+ предыдущая неделя";
            //chartH.Series.Add(seriesName);
            //chartH.Series[seriesName].ChartType = SeriesChartType.Line;
            //chartH.Series[seriesName].BorderWidth = 2;
            //chartH.Series[seriesName].Label = "#VAL{#,##0}";


            //seriesName = "Экономия 10%";
            //chartH.Series.Add(seriesName);
            //chartH.Series[seriesName].ChartType = SeriesChartType.Line;
            //chartH.Series[seriesName].BorderWidth = 2;
            //chartH.Series[seriesName].Label = "#VAL{#,##0}";


            //seriesName = "Экономия 23%";
            //chartH.Series.Add(seriesName);
            //chartH.Series[seriesName].ChartType = SeriesChartType.Line;
            //chartH.Series[seriesName].BorderWidth = 2;
            //chartH.Series[seriesName].Label = "#VAL{#,##0}";

        

            
            for (ii = 0; ii <= 23; ii++)
            {
                seriesName = "Значения";
                chartH.Series[seriesName].Points.AddXY(ii+1, shiftedActiv[ii]);

                seriesName = "Среднее = " + valAvg.ToString("#0.00");
                chartH.Series[seriesName].Points.AddXY(ii+1 , valAvg);

                //seriesName = "A+ предыдущая неделя";
                //chartH.Series[seriesName].Points.AddXY(dt.Rows(ii)("P_DATE"), valPrev);

                //seriesName = "Экономия 10%";
                //chartH.Series[seriesName].Points.AddXY(dt.Rows(ii)("P_DATE"), valPrev * 0.9);

                //seriesName = "Экономия 23%";
                //chartH.Series[seriesName].Points.AddXY(dt.Rows(ii)("P_DATE"), valPrev * 0.77);
            }

                   
                
            
        }

        #endregion

        #region "Иммунитет"

        private void loadChartIM()
        {
            double valAvg = 0.0;

            int ii;

            IMBarsMinus = new int[111];
            IMBarsPlus = new int[111];
            IMLine = new int[10];
            bool IsMinus = true;

            IMBarsMinus[0] = 10;
            IMBarsPlus[0] = 0;


            for (int i = 0; i < 110; i++)
            {
                if(i >0 && i % kG[1] == 0)
                {
                    IsMinus = !IsMinus;
                }
                if (IsMinus)
                {
                    IMBarsMinus[i+1] = 10;
                    IMBarsPlus[i+1] = 0;
                }
                else
                {
                    IMBarsMinus[i+1] = 0;
                    IMBarsPlus[i+1] = 10;
                }

            }
            

        
           
            for (int l = 0; l < 10; l++)
            {
                IMLine[l] = Util.MOD90(kIntS[l % 8] );
            }
            

            chartIM.Titles.Clear();
            chartIM.Titles.Add("Иммунитет");

            // Set chart title font
            chartIM.Titles[0].Font = new Font("Times New Roman", 14, FontStyle.Bold);

            // Set chart title color
            chartIM.Titles[0].ForeColor = Color.Blue;

            // Set border title color
            // chartIM.Titles[0].BorderColor = Color.Black

            // Set background title color
            chartIM.Titles[0].BackColor = Color.White;

            // Set Title Alignment
            chartIM.Titles[0].Alignment = System.Drawing.ContentAlignment.MiddleCenter;

            // Set Title Alignment
            chartIM.Titles[0].ToolTip = chartIM.Titles[0].Text;


            chartIM.Series.Clear();


            valAvg = 0.0;

            for (ii = 0; ii < 8; ii++)
            {

                valAvg += kIntS[ii];
            }
            valAvg /= 8.0;


            string seriesName;
            DateTime d;

            chartIM.ChartAreas[0].AxisX.Interval = 10;
            chartIM.ChartAreas[0].AxisX.Minimum = 0;

            chartIM.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            chartIM.ChartAreas[0].AxisX.MajorGrid.Interval = 10;
            chartIM.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Black;
            

            //chartIM.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            //chartIM.ChartAreas[0].AxisX.MinorGrid.Interval = 1;
            //chartIM.ChartAreas[0].AxisX.MinorGrid.LineColor = Color.Black;
            //chartIM.ChartAreas[0].AxisX.MinorGrid.LineDashStyle = ChartDashStyle.Dot;

            chartIM.ChartAreas[0].AxisY.Interval = 1;
            chartIM.ChartAreas[0].AxisY.MajorGrid.Interval = 1;
            chartIM.ChartAreas[0].AxisY.Minimum = 0;
            chartIM.ChartAreas[0].AxisY.Maximum = 10;

            seriesName = "Иммунитет";
            chartIM.Series.Add(seriesName);
            chartIM.Series[seriesName].ChartType = SeriesChartType.Line;
            chartIM.Series[seriesName].BorderWidth = 3;
            chartIM.Series[seriesName].Color = Color.Blue;
            chartIM.Series[seriesName].Label = "#VAL{#,##0}";

            seriesName = "Среднее = " + valAvg.ToString("#0.00");
            chartIM.Series.Add(seriesName);
            chartIM.Series[seriesName].ChartType = SeriesChartType.Line;
            chartIM.Series[seriesName].Color = Color.Brown;
            chartIM.Series[seriesName].BorderWidth = 3;

            seriesName = "Минусовая сота";
            chartIM.Series.Add(seriesName);
            chartIM.Series[seriesName].ChartType = SeriesChartType.Column;
            chartIM.Series[seriesName].BorderWidth = 8;
            chartIM.Series[seriesName].Color = Color.FromArgb(128, 128, 0, 0);
            

            seriesName = "Плюсовая сота";
            chartIM.Series.Add(seriesName);
            chartIM.Series[seriesName].ChartType = SeriesChartType.Column;
            chartIM.Series[seriesName].BorderWidth = 8;
            chartIM.Series[seriesName].Color = Color.FromArgb(128, 0, 128, 0);


         



            

            for (ii = 0; ii < 110; ii++)
            {
                seriesName = "Минусовая сота";
                chartIM.Series[seriesName].Points.AddXY(ii, IMBarsMinus[ii]);
                
                seriesName = "Плюсовая сота";
                chartIM.Series[seriesName].Points.AddXY(ii, IMBarsPlus[ii]);

                seriesName = "Среднее = " + valAvg.ToString("#0.00");
                chartIM.Series[seriesName].Points.AddXY(ii, valAvg);
            }


            seriesName = "Иммунитет";
            chartIM.Series[seriesName].Points.AddXY(0, 0);

            for (ii = 0; ii < 10; ii++)
            {
                chartIM.Series[seriesName].Points.AddXY((ii+1)*10 + IMLine[ii], IMLine[ii]);
            }

        }


        #endregion


        #region "Напряженность иммунитета"

        private void IMMUNITYCalc(string KEYWORD)
        {
            // код болезни
            Util.WriteLine("-------------- сила болезни ---------------");
            sB = Util.DATE2INTS(bDate.Value.ToString("ddMMyyyy"));

            int[] tempSB = Util.STRING2INTS(KEYWORD);
            
            string sOut = "";
            for (int l = 0; l < sB.Length; l++)
            {
                sOut += sB[l].ToString() + " ";
            }
            Util.WriteLine(sOut);
            sOut = "";
            for (int l = 0; l < tempSB.Length; l++)
            {
                sOut += tempSB[l].ToString() + " ";
                sB[l % 8] += tempSB[l];
                if (l % 8 == 7)
                {
                    Util.WriteLine(sOut);
                    sOut = "";
                }
            }
            if (sOut != "")
                Util.WriteLine(sOut);

            sOut = "";
            int sumSB = 0;
            Util.WriteLine("----------------");
            for (int l = 0; l < sB.Length; l++)
            {
                sB[l] = Util.MOD9(sB[l]);
                sumSB += sB[l];

                sOut += Util.MOD9(sB[l]).ToString() + " ";
            }
            Util.WriteLine(sOut + " СБ=" + sumSB.ToString());

            sOut = "";
            Util.WriteLine("ВТЗ=" + ((sumSB * 100.0 / 72.0)).ToString("#00.00"));

            KExtS = (sB[0] * 10 + sB[1]) * (sB[2] * 10 + sB[3]) * (sB[4] * 1000 + sB[5] * 100 + sB[6] * 10 + sB[7]);
            Util.WriteLine("Код Внешней Среды=" + KExtS.ToString());
            while(KExtS.ToString().Length < 8)
            {
                KExtS *= 10;
            }
            kIntS = Util.REVERS2INTS(KExtS.ToString());

            sOut = "";
            for (int l = 0; l < kIntS.Length; l++)
            {
                sOut += kIntS[l].ToString() ;
            }
            Util.WriteLine("Код Внутренней Среды=" + sOut);

            int sumKIntS=0;
            for (int l = 0; l < kIntS.Length; l++)
            {
                sumKIntS += kIntS[l];
            }
            Util.WriteLine("Коэф. Внутренней Среды=" + sumKIntS.ToString());


            Util.WriteLine("ИС=" + ((sumKIntS * 100.0 / 72.0)).ToString("#00.00"));

            int [] BNum = Util.DATE2INTS(bDate.Value.ToString("ddMMyyyy"));

            int sumBNum = 0;
            for (int l = 0; l < BNum.Length; l++)
            {
                sumBNum += BNum[l];
            }
            Util.WriteLine("ЧР=" + sumBNum.ToString());
            Util.WriteLine("ЭС [приведено к 100%] =" + ((sumBNum * 100.0 / 72.0)).ToString("#00.00"));

            txtSB.Text = sumSB.ToString();
            txtVTZ.Text =((sumSB * 100.0 / 72.0)).ToString("#00.00");
            txtIS.Text = ((sumKIntS * 100.0 / 72.0)).ToString("#00.00");
            txtEnS.Text = ((sumBNum * 100.0 / 72.0)).ToString("#00.00");
            double pchv =(((sumSB * 100.0 / 72.0) + (sumKIntS * 100.0 / 72.0) + (sumBNum * 100.0 / 72.0)) / 3);
            pchv = pchv * 100.0 / 80.0;
            Util.WriteLine("ПЧВ [приведено к 100%]=" + pchv.ToString("#00.00"));
            txtPVCH.Text = pchv.ToString("#00.00") ;

            loadChartIMUNITY();

            loadChartIM();

        }


        private void loadChartIMUNITY()
        {
            double valAvg = 0.0;

            int ii;


            IMUNBlue = new int[8 * 12+1];
            IMUNRed = new int[8 * 12+1];
            

            for (int m = 0; m < 12; m++)
            {
                for (int l = 0; l < 8; l++)
                {
                    IMUNBlue[m * 8 + l] = Util.MOD90(kIntS[l] + m);
                    IMUNRed[m * 8 + l ] = Util.MOD90(sB[l]);

                    // Util.WriteLine("Год= " + (m * 8 + l).ToString() + " И= " + IMUNBlue[m * 8 + l].ToString() + " Б= " + IMUNRed[m * 8 + l].ToString());
                }
            }

            chartIMUN.Titles.Clear();
            chartIMUN.Titles.Add("Напряженность иммунитета");

            // Set chart title font
            chartIMUN.Titles[0].Font = new Font("Times New Roman", 14, FontStyle.Bold);

            // Set chart title color
            chartIMUN.Titles[0].ForeColor = Color.Blue;

            // Set border title color
            // chartIMUN.Titles[0].BorderColor = Color.Black

            // Set background title color
            chartIMUN.Titles[0].BackColor = Color.White;

            // Set Title Alignment
            chartIMUN.Titles[0].Alignment = System.Drawing.ContentAlignment.MiddleCenter;

            // Set Title Alignment
            chartIMUN.Titles[0].ToolTip = chartIMUN.Titles[0].Text;

            
            chartIMUN.Series.Clear();

         

            for (ii = 0; ii < 8; ii++)
            {
             
                valAvg += sB[ii];
            }
            valAvg /= 8;


            string seriesName;
            DateTime d ;

            LabelStyle ls = new LabelStyle();
            ls.Format = "yyyy";
            chartIMUN.ChartAreas[0].AxisX.Interval = 12;
            chartIMUN.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;

            chartIMUN.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            chartIMUN.ChartAreas[0].AxisX.MajorGrid.Interval = 12;
            chartIMUN.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Black;
            chartIMUN.ChartAreas[0].AxisX.MinorGrid.IntervalType = DateTimeIntervalType.Months;

            chartIMUN.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            chartIMUN.ChartAreas[0].AxisX.MinorGrid.Interval = 1;
            chartIMUN.ChartAreas[0].AxisX.MinorGrid.LineColor = Color.Black;
            chartIMUN.ChartAreas[0].AxisX.MinorGrid.LineDashStyle = ChartDashStyle.Dot;
            chartIMUN.ChartAreas[0].AxisX.MinorGrid.IntervalType = DateTimeIntervalType.Months;



            chartIMUN.ChartAreas[0].AxisX.LabelStyle = ls;

            chartIMUN.ChartAreas[0].AxisY.Interval = 1;
            chartIMUN.ChartAreas[0].AxisY.MajorGrid.Interval = 1;

            seriesName = "Болезнь";
            chartIMUN.Series.Add(seriesName);
            chartIMUN.Series[seriesName].ChartType = SeriesChartType.Line;
            chartIMUN.Series[seriesName].BorderWidth = 2;
            chartIMUN.Series[seriesName].Color=Color.Red;
            chartIMUN.Series[seriesName].Label = "#VAL{#,##0}";

            seriesName = "Иммунитет";
            chartIMUN.Series.Add(seriesName);
            chartIMUN.Series[seriesName].ChartType = SeriesChartType.Line;
            chartIMUN.Series[seriesName].BorderWidth = 2;
            chartIMUN.Series[seriesName].Color = Color.Blue;
            chartIMUN.Series[seriesName].Label = "#VAL{#,##0}";

            
            seriesName = "Среднее = " + valAvg.ToString("#0.00");
            chartIMUN.Series.Add(seriesName);
            chartIMUN.Series[seriesName].ChartType = SeriesChartType.Line;
            chartIMUN.Series[seriesName].Color = Color.Green;
            chartIMUN.Series[seriesName].BorderWidth = 2;




            

            for (ii =(int) numImmFrom.Value ; ii < (int)numImmTo.Value && ii < 8*12; ii++)
            {
                d = bDate.Value;
               // d=d.AddYears(ii+1);
                d = d.AddYears(ii );
                seriesName = "Болезнь";
                chartIMUN.Series[seriesName].Points.AddXY(d , IMUNRed[ii]);

                seriesName = "Иммунитет";
                chartIMUN.Series[seriesName].Points.AddXY(d, IMUNBlue[ii]);


                seriesName = "Среднее = " + valAvg.ToString("#0.00");
                chartIMUN.Series[seriesName].Points.AddXY(d, valAvg);

               
                
            }
        }


        #endregion

        #region "word+fio"
        private string  WordCalc(string KEYWORD, string FIO)
        {

            string K = KEYWORD.Replace(" ", "");
            int[] tempSB = Util.STRING2INTS(K);

            string sOut =  "";
            int wSz = tempSB.Length;
            string data = (K + FIO).Replace(" ", "");
            int Sz = data.Length;
            int hSz = (int)Math.Ceiling((decimal)Sz / (decimal)wSz);
            int[,] m = new int[wSz, hSz];

            int t = 0;

            for(int h=0; h <hSz; h++)
            {
                for (int w = 0; w < wSz; w++)
                {
                    if (t < Sz)
                    {
                        m[w, h] = Util.CODE_RU(data.Substring(t, 1));
                        t++;
                    }
                    else
                    {
                        m[w, h] = 0;
                    }
                }
            }


            int[] r = new int[wSz];
            int s;

            // складываем по столбцам
            for (int w = 0; w < wSz; w++)
            {
                s = 0;
                for (int h = 0; h < hSz; h++)
                {
                    s += m[w, h];

                  
                }

                while(s > 9)
                {
                    s = Util.NUM2CODE(s.ToString());
                }

                r[w] = s;
            }

            s = 0;
            // складываем итоговую строку
            for (int w = 0; w < wSz; w++)
            {
                s+=r[w];
            }

            s = Util.MOD22(s);

            string lStr = "";
            t = 0;

            for (int h = 0; h < hSz; h++)
            {
                for (int w = 0; w < wSz; w++)
                {
                    if (t < Sz)
                    {
                        lStr += data.Substring(t, 1) + " ";
                        t++;
                    }

                    
                    sOut += m[w, h].ToString() + " ";
                }
                lStr += "\r\n";
                sOut += "\r\n" ;
            }

            sOut += "\r\n";
            for (int w = 0; w < wSz; w++)
            {
                sOut += r[w].ToString() + " ";
            }
            sOut += "\r\n\r\n";

            sOut += "СУММА "+ s.ToString();


            return lStr+"\r\n"+sOut;


        }

        #endregion

    }
}

