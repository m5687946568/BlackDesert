﻿using System;
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
        string settingmark;

        public Form1()
        {
            InitializeComponent();
            try
            {
                NowDateTime(ref tyear, ref tmonth, ref tday, ref thour, ref tminute, ref tweek);
                GetBossNextTime(tyear, tmonth, tday, thour, tminute, tweek);
            }
            catch
            {
                MessageBox.Show("Error!","錯誤",MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
                Environment.Exit(Environment.ExitCode);
            }
            GC.Collect();
        }

        public void GetBossNextTime(string todayyear, string todaymonth, string todayday, string todayhour, string todayminute, string todayweek)
        {
            int BTS = 0, BWS = 0;
            int tHM = Convert.ToInt16(thour + tminute);
            DateTime tYMD = Convert.ToDateTime(tyear + "-" + tmonth + "-" + tday);
            label1.Text = "更新時間：" + tyear + "／" + tmonth + "／" + tday + "　" + thour + "：" + tminute + "　" + GetWeekCht(tweek);

            //出現時間
            int[] allbosstime = { 0015, 0200, 1100, 1500, 1900, 2330 };
            string[,] bosstimecht = new string[,]
            {
                    { "00", "02", "11", "15", "19", "23" },
                    { "15", "00", "00", "00", "00", "30" }
            };

            //世界王名字及圖片
            string b0 = null;
            string b1 = World_Boss_Next_Time.Properties.Settings.Default.BossName1;
            string b2 = World_Boss_Next_Time.Properties.Settings.Default.BossName2;
            string b3 = World_Boss_Next_Time.Properties.Settings.Default.BossName3;
            string b4 = World_Boss_Next_Time.Properties.Settings.Default.BossName4;
            string b5 = World_Boss_Next_Time.Properties.Settings.Default.BossName5;
            string b6 = World_Boss_Next_Time.Properties.Settings.Default.BossName6;
            string b7 = World_Boss_Next_Time.Properties.Settings.Default.BossName7;
            string b8 = World_Boss_Next_Time.Properties.Settings.Default.BossName8;
            string b9 = World_Boss_Next_Time.Properties.Settings.Default.BossName9;
            BossFilter(ref b1, ref b2, ref b3, ref b4, ref b5, ref b6, ref b7, ref b8, ref b9);

            //各時段出現種類名稱及圖片 array[出現時段,星期,王1王2]
            //BOSS:1克價卡,2庫屯,3羅裴勒,4卡嵐達,5奧平,6木拉卡,7肯恩特,8貝爾,9卡莫斯
            string[,,] bossname = new string[,,]
                {
                    //星期:日,一,二,三,四,五,六
                    { { b0, b0 }, { b6, b7 }, { b0, b0 }, { b0, b0 }, { b0, b0 }, { b0, b0 }, { b9, b0 } },//0015
                    { { b3, b4 }, { b1, b2 }, { b3, b0 }, { b4, b0 }, { b2, b0 }, { b1, b3 }, { b1, b2 } },//0200
                    { { b1, b3 }, { b4, b0 }, { b1, b2 }, { b1, b3 }, { b2, b4 }, { b2, b5 }, { b2, b4 } },//1100
                    { { b8, b0 }, { b1, b2 }, { b3, b4 }, { b9, b0 }, { b1, b3 }, { b3, b4 }, { b5, b0 } },//1500
                    { { b2, b4 }, { b9, b0 }, { b8, b0 }, { b1, b2 }, { b3, b0 }, { b1, b2 }, { b1, b3 } },//1900
                    { { b5, b0 }, { b1, b2 }, { b1, b3 }, { b3, b4 }, { b2, b4 }, { b6, b7 }, { b0, b0 } }//2330
            };
            BossStage(ref tYMD, ref BTS, ref BWS);
            BossStageAddChk(ref tYMD, ref BTS, ref BWS);
            BossListView(tYMD, BTS, BWS);
            ViewBossData(tYMD, bosstimecht[0, BTS], bosstimecht[1, BTS], BWS, bossname[BTS, BWS, 0], bossname[BTS, BWS, 1]);
            GC.Collect();


            void BossListView(DateTime lvDate, int lvBTS, int lvBWS)
            {
                listView1.Items.Clear();
                //第一項
                BossStageLess(ref lvDate, ref lvBTS, ref lvBWS);
                var lvi = new ListViewItem(lvDate.ToString("yyyy/MM/dd"));
                string st1 = bosstimecht[0, lvBTS] + ":" + bosstimecht[1, lvBTS];
                string st2 = bossname[lvBTS, lvBWS, 0] + " " + bossname[lvBTS, lvBWS, 1];
                lvi.SubItems.Add(st1.Trim());
                lvi.SubItems.Add(st2.Trim());
                listView1.Items.Add(lvi);

                //第二項
                BossStageAdd(ref lvDate, ref lvBTS, ref lvBWS);
                lvi = new ListViewItem(lvDate.ToString("yyyy/MM/dd"));
                st1 = bosstimecht[0, lvBTS] + ":" + bosstimecht[1, lvBTS];
                st2 = bossname[lvBTS, lvBWS, 0] + " " + bossname[lvBTS, lvBWS, 1];
                lvi.BackColor = Color.YellowGreen;
                lvi.SubItems.Add(st1.Trim());
                lvi.SubItems.Add(st2.Trim());
                listView1.Items.Add(lvi);
                listView1.Items[1].Selected = true;

                //後n項
                for (int n = 1; n <= 4; n++)
                {
                    BossStageAdd(ref lvDate, ref lvBTS, ref lvBWS);
                    lvi = new ListViewItem(lvDate.ToString("yyyy/MM/dd"));
                    st1 = bosstimecht[0, lvBTS] + ":" + bosstimecht[1, lvBTS];
                    st2 = bossname[lvBTS, lvBWS, 0] + " " + bossname[lvBTS, lvBWS, 1];
                    lvi.SubItems.Add(st1.Trim());
                    lvi.SubItems.Add(st2.Trim());
                    listView1.Items.Add(lvi);
                }
            }

            void ViewBossData(DateTime TodayDate, string bth, string btm, int week, string bn1, string bn2)
            {
                if (bn1 == null & bn2 == null)
                {
                    settingmark = "1";
                    groupBox1.Text = null;
                }
                else
                {
                    settingmark = "0";
                    string startstring = World_Boss_Next_Time.Properties.Settings.Default.StartString;
                    string st1 = startstring + bth + btm + " " + bn1 + " " + bn2;
                    groupBox1.Text = "下次世界王時間：" + TodayDate.ToString("yyyy/MM/dd") + "　" + bth + "：" + btm + "　" + GetWeekCht(week.ToString());
                    textBox1.Text = st1.Trim();
                }
            }

            void BossStage(ref DateTime TodayDate, ref int BossTimeStage, ref int BossWeekStage)
            {
                //取得日期及時段
                int TC = allbosstime.Count() - 1;
                for (int t=1; t <= TC; t++)
                {
                    if (tHM <= allbosstime[t] && tHM > allbosstime[t - 1]) { BossTimeStage = t; }
                }
                if (tHM <= allbosstime[0]) { BossTimeStage = 0; }
                if (tHM > allbosstime[TC]) { TodayDate = TodayDate.AddDays(1); BossTimeStage = 0; }

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

            void BossStageAddChk(ref DateTime TodayDate, ref int BossTimeStage, ref int BossWeekStage)
            {
                int TC = allbosstime.Count() - 1;
                int BNTC = (bossname.Length) / 2;
                for(int temp=1;temp<=BNTC;temp++)
                {
                    if (bossname[BossTimeStage, BossWeekStage, 0] == null && bossname[BossTimeStage, BossWeekStage, 1] == null)
                    {
                        if (BossTimeStage >= TC)
                        {
                            TodayDate = TodayDate.AddDays(1);
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
                        else
                        {
                            BossTimeStage += 1;
                        }
                    }
                    if (bossname[BossTimeStage, BossWeekStage, 0] == null && bossname[BossTimeStage, BossWeekStage, 1] != null)
                    {
                        bossname[BossTimeStage, BossWeekStage, 0] = bossname[BossTimeStage, BossWeekStage, 1];
                        bossname[BossTimeStage, BossWeekStage, 1] = null;
                    }
                }
            }

            void BossStageLessChk(ref DateTime TodayDate, ref int BossTimeStage, ref int BossWeekStage)
            {
                int TC = allbosstime.Count() - 1;
                int BNTC = (bossname.Length) / 2;
                for (int temp = 1; temp <= BNTC; temp++)
                {
                    if (bossname[BossTimeStage, BossWeekStage, 0] == null && bossname[BossTimeStage, BossWeekStage, 1] == null)
                    {
                        if (BossTimeStage == 0)
                        {
                            TodayDate = TodayDate.AddDays(-1);
                            BossTimeStage = TC;
                            if (BossWeekStage == 0)
                            {
                                BossWeekStage = 6;
                            }
                            else
                            {
                                BossWeekStage -= 1;
                            }
                        }
                        else
                        {
                            BossTimeStage -= 1;
                        }
                    }
                    if (bossname[BossTimeStage, BossWeekStage, 0] == null && bossname[BossTimeStage, BossWeekStage, 1] != null)
                    {
                        bossname[BossTimeStage, BossWeekStage, 0] = bossname[BossTimeStage, BossWeekStage, 1];
                        bossname[BossTimeStage, BossWeekStage, 1] = null;
                    }
                }
            }

            void BossStageAdd(ref DateTime TodayDate, ref int BossTimeStage, ref int BossWeekStage)
            {
                int TC = allbosstime.Count() - 1;
                if (BossTimeStage >= TC)
                {
                    TodayDate = TodayDate.AddDays(1);
                    BossTimeStage = 0;
                    if (BossWeekStage < 6)
                    {
                        BossWeekStage += 1;
                    }
                    else
                    {
                        BossWeekStage = 0;
                    }
                    BossStageAddChk(ref TodayDate, ref BossTimeStage, ref BossWeekStage);
                }
                else
                {
                    BossTimeStage += 1;
                    BossStageAddChk(ref TodayDate, ref BossTimeStage, ref BossWeekStage);
                }
            }

            void BossStageLess(ref DateTime TodayDate, ref int BossTimeStage, ref int BossWeekStage)
            {
                int TC = allbosstime.Count() - 1;
                if (BossTimeStage == 0)
                {
                    TodayDate = TodayDate.AddDays(-1);
                    BossTimeStage = TC;
                    if (BossWeekStage == 0)
                    {
                        BossWeekStage = 6;
                    }
                    else
                    {
                        BossWeekStage -= 1;
                    }
                    BossStageLessChk(ref TodayDate, ref BossTimeStage, ref BossWeekStage);
                }
                else
                {
                    BossTimeStage -= 1;
                    BossStageLessChk(ref TodayDate, ref BossTimeStage, ref BossWeekStage);
                }
            }

            void BossFilter(ref string boss1, ref string boss2, ref string boss3, ref string boss4, ref string boss5, ref string boss6, ref string boss7, ref string boss8, ref string boss9)
            {

                if (b1chkbox.Checked == false) { b1 = null; }
                if (b2chkbox.Checked == false) { b2 = null; }
                if (b3chkbox.Checked == false) { b3 = null; }
                if (b4chkbox.Checked == false) { b4 = null; }
                if (b5chkbox.Checked == false) { b5 = null; }
                if (b6chkbox.Checked == false) { b6 = null; }
                if (b7chkbox.Checked == false) { b7 = null; }
                if (b8chkbox.Checked == false) { b8 = null; }
                if (b9chkbox.Checked == false) { b9 = null; }
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

        public void ItemViewChgLine()
        {
            string startstring = World_Boss_Next_Time.Properties.Settings.Default.StartString;
            if (checkBox1.Checked == true)
            {
                checkBox1.Text = "換行";
                var ivs = listView1.SelectedItems;
                if(listView1.SelectedItems.Count == 1)
                {
                    textBox1.Text = null;
                    string[] sivsHHMM = ivs[0].SubItems[1].Text.Split(':');
                    textBox1.Text += startstring + sivsHHMM[0] + sivsHHMM[1] + " " + ivs[0].SubItems[2].Text;
                }
                if (listView1.SelectedItems.Count > 1)
                {
                    textBox1.Text = null;
                    string[] sivsHHMM = ivs[0].SubItems[1].Text.Split(':');
                    textBox1.Text += startstring + sivsHHMM[0] + sivsHHMM[1] + " " + ivs[0].SubItems[2].Text;
                    for (int t = 1; ivs.Count - 2 >= t; t++)
                    {
                        string[] ivsHHMM = ivs[t].SubItems[1].Text.Split(':');
                        textBox1.Text += "\r\n" + startstring + ivsHHMM[0] + ivsHHMM[1] + " " + ivs[t].SubItems[2].Text;
                    }
                    string[] eivsHHMM = ivs[ivs.Count - 1].SubItems[1].Text.Split(':');
                    textBox1.Text += "\r\n" + startstring + eivsHHMM[0] + eivsHHMM[1] + " " + ivs[ivs.Count - 1].SubItems[2].Text;
                }
            }
            else
            {
                checkBox1.Text = "不換行";
                var ivs = listView1.SelectedItems;
                if (listView1.SelectedItems.Count == 1)
                {
                    textBox1.Text = null;
                    string[] sivsHHMM = ivs[0].SubItems[1].Text.Split(':');
                    textBox1.Text += startstring + sivsHHMM[0] + sivsHHMM[1] + " " + ivs[0].SubItems[2].Text;
                }
                if (listView1.SelectedItems.Count > 1)
                {
                    textBox1.Text = null;
                    string[] sivsHHMM = ivs[0].SubItems[1].Text.Split(':');
                    textBox1.Text += startstring + sivsHHMM[0] + sivsHHMM[1] + " " + ivs[0].SubItems[2].Text;
                    for (int t = 1; ivs.Count - 2 >= t; t++)
                    {
                        string[] ivsHHMM = ivs[t].SubItems[1].Text.Split(':');
                        textBox1.Text += " " + startstring + ivsHHMM[0] + ivsHHMM[1] + " " + ivs[t].SubItems[2].Text;
                    }
                    string[] eivsHHMM = ivs[ivs.Count - 1].SubItems[1].Text.Split(':');
                    textBox1.Text += " " + startstring + eivsHHMM[0] + eivsHHMM[1] + " " + ivs[ivs.Count - 1].SubItems[2].Text;
                    
                }
                GC.Collect();
            }
        }

        private void TextBox1Click(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void Button1Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, textBox1.Text);
        }

        private void Button2Click(object sender, EventArgs e)
        {
            NowDateTime(ref tyear, ref tmonth, ref tday, ref thour, ref tminute, ref tweek);
            GetBossNextTime(tyear, tmonth, tday, thour, tminute, tweek);
        }

        private void ItemViewSelectionChg(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ItemViewChgLine();
        }

        private void ItemViewVisibleChg(object sender, EventArgs e)
        {
            listView1.SelectedItems.Clear();
            listView1.Items[1].Selected = true;
        }

        private void CheckBox1CkhChg(object sender, EventArgs e)
        {
            ItemViewChgLine();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (panel1.Visible == true)
            {
                World_Boss_Next_Time.Properties.Settings.Default.BossName1 = textBoxPS1.Text;
                World_Boss_Next_Time.Properties.Settings.Default.BossName2 = textBoxPS2.Text;
                World_Boss_Next_Time.Properties.Settings.Default.BossName3 = textBoxPS3.Text;
                World_Boss_Next_Time.Properties.Settings.Default.BossName4 = textBoxPS4.Text;
                World_Boss_Next_Time.Properties.Settings.Default.BossName5 = textBoxPS5.Text;
                World_Boss_Next_Time.Properties.Settings.Default.BossName6 = textBoxPS6.Text;
                World_Boss_Next_Time.Properties.Settings.Default.BossName7 = textBoxPS7.Text;
                World_Boss_Next_Time.Properties.Settings.Default.BossName8 = textBoxPS8.Text;
                World_Boss_Next_Time.Properties.Settings.Default.BossName9 = textBoxPS9.Text;
                World_Boss_Next_Time.Properties.Settings.Default.Save();
                GetBossNextTime(tyear, tmonth, tday, thour, tminute, tweek);
                if (settingmark == "0")
                {
                    panel1.Visible = false;
                    listView1.Visible = true;
                    textBox1.Visible = true;
                    checkBox1.Visible = true;
                    button1.Visible = true;
                }
                else
                {
                    MessageBox.Show("至少選擇一個項目!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            else
            {
                textBoxPS1.Text = World_Boss_Next_Time.Properties.Settings.Default.BossName1;
                textBoxPS2.Text = World_Boss_Next_Time.Properties.Settings.Default.BossName2;
                textBoxPS3.Text = World_Boss_Next_Time.Properties.Settings.Default.BossName3;
                textBoxPS4.Text = World_Boss_Next_Time.Properties.Settings.Default.BossName4;
                textBoxPS5.Text = World_Boss_Next_Time.Properties.Settings.Default.BossName5;
                textBoxPS6.Text = World_Boss_Next_Time.Properties.Settings.Default.BossName6;
                textBoxPS7.Text = World_Boss_Next_Time.Properties.Settings.Default.BossName7;
                textBoxPS8.Text = World_Boss_Next_Time.Properties.Settings.Default.BossName8;
                textBoxPS9.Text = World_Boss_Next_Time.Properties.Settings.Default.BossName9;
                listView1.Visible = false;
                textBox1.Visible = false;
                checkBox1.Visible = false;
                button1.Visible = false;
                panel1.Visible = true;
            }
        }

        private void settingbutton1(object sender, EventArgs e)
        {
            b1chkbox.Checked = true;
            b2chkbox.Checked = true;
            b3chkbox.Checked = true;
            b4chkbox.Checked = true;
            b5chkbox.Checked = true;
            b6chkbox.Checked = true;
            b7chkbox.Checked = true;
            b8chkbox.Checked = true;
            b9chkbox.Checked = true;
        }

        private void settingbutton2(object sender, EventArgs e)
        {
            b1chkbox.Checked = false;
            b2chkbox.Checked = false;
            b3chkbox.Checked = false;
            b4chkbox.Checked = false;
            b5chkbox.Checked = false;
            b6chkbox.Checked = false;
            b7chkbox.Checked = false;
            b8chkbox.Checked = false;
            b9chkbox.Checked = false;
        }

        private void settingbutton3(object sender, EventArgs e)
        {
            textBoxPS1.Text = "克價卡";
            textBoxPS2.Text = "庫屯";
            textBoxPS3.Text = "羅裴勒";
            textBoxPS4.Text = "卡嵐達";
            textBoxPS5.Text = "奧平";
            textBoxPS6.Text = "木拉卡";
            textBoxPS7.Text = "肯恩特";
            textBoxPS8.Text = "貝爾";
            textBoxPS9.Text = "卡莫斯";
        }
    }
}
