using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace World_Boss_Next_Time
{
    public partial class Form1 : Form
    {
        string tyear = "0000";
        string tmonth = "00";
        string tday = "00";
        string thour = "00";
        string tminute = "00";
        string tweek = "0";
        
        public Form1()
        {
            InitializeComponent();
            NowDateTime(ref tyear, ref tmonth, ref tday, ref thour, ref tminute, ref tweek);
            GetBossNextTime(tyear, tmonth, tday, thour, tminute, tweek);
            GC.Collect();
        }

        public void GetBossNextTime(string todayyear, string todaymonth, string todayday, string todayhour, string todayminute, string todayweek)
        {
            //TodayDateTime TDT = new TodayDateTime();
            int tHM = Convert.ToInt16(thour + tminute);
            label1.Text = "現在時間：" + tyear + "／" + tmonth + "／" + tday + "　" + thour + "：" + tminute + "　" + GetWeekCht(tweek);


            //出現時間
            int[] allbosstime = { 0200, 1100, 1500, 1900, 2330 };
            string[,] bosstimecht = new string[,]
            {
                    { "02", "11", "15", "19", "23" },
                    { "00", "00", "00", "00", "30" }
            };

            //世界王名字及圖片
            Bitmap p0 = null; string b0 = null;
            Bitmap p1 = new Bitmap(World_Boss_Next_Time.Properties.Resources.b1); string b1 = "《○克價卡》";
            Bitmap p2 = new Bitmap(World_Boss_Next_Time.Properties.Resources.b2); string b2 = "《●庫屯》";
            Bitmap p3 = new Bitmap(World_Boss_Next_Time.Properties.Resources.b3); string b3 = "《★羅裴勒》";
            Bitmap p4 = new Bitmap(World_Boss_Next_Time.Properties.Resources.b4); string b4 = "《☆卡嵐達》";
            Bitmap p5 = new Bitmap(World_Boss_Next_Time.Properties.Resources.b5); string b5 = "《█奧平》";
            Bitmap p6 = new Bitmap(World_Boss_Next_Time.Properties.Resources.b6); string b6 = "《▲木拉卡》";
            Bitmap p7 = new Bitmap(World_Boss_Next_Time.Properties.Resources.b7); string b7 = "《▼肯恩特》";

            //各時段出現種類名稱及圖片 array[出現時段,星期,王1王2]
            string[,,] bossname = new string[,,]
                {
                    { { b3, b4 }, { b1, b2 }, { b3, b0 }, { b4, b0 }, { b2, b0 }, { b1, b3 }, { b1, b2 } },
                    { { b1, b3 }, { b4, b0 }, { b1, b2 }, { b1, b3 }, { b2, b0 }, { b5, b0 }, { b2, b4 } },
                    { { b2, b0 }, { b1, b2 }, { b3, b0 }, { b4, b0 }, { b1, b3 }, { b3, b4 }, { b5, b0 } },
                    { { b3, b4 }, { b4, b0 }, { b2, b0 }, { b1, b2 }, { b3, b0 }, { b1, b2 }, { b1, b3 } },
                    { { b5, b0 }, { b1, b2 }, { b1, b3 }, { b3, b4 }, { b2, b4 }, { b6, b7 }, { b0, b0 } }
            };
            Bitmap[,,] bossphoto = new Bitmap[,,]
                {
                    { { p3, p4 }, { p1, p2 }, { p3, p0 }, { p4, p0 }, { p2, p0 }, { p1, p3 }, { p1, p2 } },
                    { { p1, p3 }, { p4, p0 }, { p1, p2 }, { p1, p3 }, { p2, p0 }, { p5, p0 }, { p2, p4 } },
                    { { p2, p0 }, { p1, p2 }, { p3, p0 }, { p4, p0 }, { p1, p3 }, { p3, p4 }, { p5, p0 } },
                    { { p3, p4 }, { p4, p0 }, { p2, p0 }, { p1, p2 }, { p3, p0 }, { p1, p2 }, { p1, p3 } },
                    { { p5, p0 }, { p1, p2 }, { p1, p3 }, { p3, p4 }, { p2, p4 }, { p6, p7 }, { p0, p0 } }
            };

            int BTS = 0, BWS = 0;
            BossStage(ref BTS, ref BWS);
            BossStageChk(ref BTS, ref BWS);
            BossListView(BTS, BWS);
            ViewBossData(bosstimecht[0, BTS], bosstimecht[1, BTS], BWS, bossname[BTS, BWS, 0], bossname[BTS, BWS, 1], bossphoto[BTS, BWS, 0], bossphoto[BTS, BWS, 1]);

            void ViewBossData(string bth, string btm, int week, string bn1, string bn2, Bitmap bp1, Bitmap bp2)
            {
                textBox1.Text = "€" + bth + btm + " " + bn1 + bn2;
                groupBox1.Text = "下次世界王時間：" + tyear + "／" + tmonth + "／" + tday + "　" + bth + "：" + btm + "　" + GetWeekCht(week.ToString());
                label2.Text = bn1;
                label3.Text = bn2;
                pictureBox1.Image = bp1;
                pictureBox2.Image = bp2;
            }

            void BossStage(ref int BossTimeStage, ref int BossWeekStage)
            {
                //取得時段
                int TC = allbosstime.Count() - 1;
                for (int t=1; t <= TC; t++)
                {
                    if (tHM <= allbosstime[t] && tHM > allbosstime[t - 1]) { BossTimeStage = t; }
                }
                if (tHM <= allbosstime[0]) { BossTimeStage = 0; }
                if (tHM > allbosstime[TC]) { BossTimeStage = 0; }

                //取得星期
                int weeknumber = Convert.ToInt16(tweek);
                if (tHM <= allbosstime[TC]) { BossWeekStage = weeknumber; }
                if (tHM > allbosstime[TC])
                {
                    if (weeknumber >= 6)
                    {
                        BossWeekStage = 0;
                    }
                    else
                    {
                        BossWeekStage = weeknumber + 1;
                    }
                }
            }

            void BossStageChk(ref int BossTimeStage, ref int BossWeekStage)
            {
                int TC = allbosstime.Count() - 1;
                if (bossname[BTS, BWS, 0] == null && bossname[BTS, BWS, 1] == null)
                {
                    if (BossTimeStage <= TC) { BossTimeStage += 1; }
                    if (BossTimeStage > TC)
                    {
                        BossTimeStage = 0;
                        if (BossWeekStage >= 6)
                        {
                            BossWeekStage = 0;
                        }
                        else
                        {
                            BossWeekStage += 1;
                        }
                    }
                    
                }
            }

            void BossStageAdd(ref DateTime listviewdate,ref int BossTimeStage, ref int BossWeekStage)
            {
                int TC = allbosstime.Count() - 1;
                if (BossTimeStage>=TC)
                {
                    listviewdate = listviewdate.AddDays(1);
                    BossTimeStage = 0;
                    BossWeekStage += 1;
                }
                else
                {
                    BossTimeStage += 1;
                }
            }

            void BossListView(int lvBTS,int lvBWS)
            {
                listView1.Items.Clear();
                string tYMD = tyear + "-" + tmonth + "-" + tday;
                DateTime dt = Convert.ToDateTime(tYMD);

                for (int t = 1; t<= 6; t++)
                {
                    var lvi = new ListViewItem(dt.ToString("yyyy/MM/dd"));
                    lvi.SubItems.Add(bosstimecht[0, lvBTS] + ":" + bosstimecht[1, lvBTS]);
                    lvi.SubItems.Add(bossname[lvBTS, lvBWS, 0] + bossname[lvBTS, lvBWS, 1]);

                    listView1.Items.Add(lvi);
                    BossStageAdd(ref dt, ref lvBTS, ref lvBWS);
                }
            }

        }
        
        public void NowDateTime(ref string tyear,ref string tmonth,ref string tday,ref string thour,ref string tminute,ref string tweek)
        {
            tyear = DateTime.Now.ToString("yyyy");
            tmonth = DateTime.Now.ToString("MM");
            tday = DateTime.Now.ToString("dd");
            thour = DateTime.Now.ToString("HH");
            tminute = DateTime.Now.ToString("mm");
            tweek = DateTime.Now.DayOfWeek.ToString("d");
        }

        public string GetWeekCht(string week)
        {
            switch (week)
            {
                case "0":
                    return "星期日";
                case "1":
                    return "星期一";
                case "2":
                    return "星期二";
                case "3":
                    return "星期三";
                case "4":
                    return "星期四";
                case "5":
                    return "星期五";
                case "6":
                    return "星期六";
                default:
                    return "";
            }
        }

        private void TextBox1Click(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
            GC.Collect();
        }

        private void Button1Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, textBox1.Text);
            GC.Collect();
        }

        private void Button2Click(object sender, EventArgs e)
        {
            NowDateTime(ref tyear, ref tmonth, ref tday, ref thour, ref tminute, ref tweek);
            GetBossNextTime(tyear, tmonth, tday, thour, tminute, tweek);
            GC.Collect();
        }

        private void Button3Click(object sender, EventArgs e)
        {
            if (listView1.Visible == true)
            {
                listView1.Items[0].Focused = true;
                listView1.Items[0].Selected = true;
                listView1.Visible = false;
            }
            else
            {
                listView1.Items[0].Focused = true;
                listView1.Items[0].Selected = true;
                listView1.Visible = true;
            }
            GC.Collect();
        }

        private void ItemViewSelectionChg(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            var ivs = listView1.SelectedItems;
            if (listView1.SelectedItems.Count > 0)
            {
                textBox1.Text = null;
                for (int t = 0; ivs.Count-1  > t; t++)
                {
                    string[] ivsHHMM = ivs[t].SubItems[1].Text.Split(':');
                    textBox1.Text += "€" + ivsHHMM[0] + ivsHHMM[1] + " " + ivs[t].SubItems[2].Text + "\r\n";
                }
                int et = ivs.Count - 1;
                string[] eivsHHMM = ivs[et].SubItems[1].Text.Split(':');
                textBox1.Text += "€" + eivsHHMM[0] + eivsHHMM[1] + " " + ivs[et].SubItems[2].Text;
            }
            GC.Collect();
        }

    }
}
