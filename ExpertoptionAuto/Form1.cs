using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using System.Threading;

namespace ExpertoptionAuto
{
    public partial class Form1 : Form
    {

        //Send money: //*[@id='app']/div/div/div[5]/div/div/div/div/div/div[4]/div/div/div[2]/div[1]/div[2]/span[2]/input
        //Click down: //*[@id='app']/div/div/div[5]/div/div/div/div/div/div[4]/div/div/div[2]/div[3]/span[1]
        //Click up:   //*[@id='app']/div/div/div[5]/div/div/div/div/div/div[4]/div/div/div[2]/div[3]/span[2]
        //Get win-lose ($)  //*[@id='app']/div/div/div[1]/div/div/div[2]/div[2]/div/div/div[3]/div[1]/div[2]/div[2]/span
        string xPathMoney = "//*[@id=\"app\"]/div/div/div[2]/div/div/div/div/div/div[4]/div[2]/div/div[2]/div[1]/div[2]/span[2]/input";
        string xPathClickDown = "//*[@id=\"app\"]/div/div/div[2]/div/div/div/div/div/div[4]/div[2]/div/div[2]/div[3]/span[1]";
        string xPathClickUp = "//*[@id=\"app\"]/div/div/div[2]/div/div/div/div/div/div[4]/div[2]/div/div[2]/div[3]/span[2]";
        string xPathCheckComplete = "//*[@id=\"app\"]/div/div/span[1]/div/div/div/div[2]/div[2]/div/div/div[3]/div[1]/div[1]";

        string xPathGetMoney = "//*[@id=\"app\"]/div/div/span[1]/div/div/div/div[2]/div[2]/div/div/div[3]/div[1]/div[2]/div[2]/span";
        string xPathClose = "//*[@id=\"app\"]/div/div/span[1]/div/div/div/div[1]/span";
        string keyResult = "Trade result";
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        private IWebElement myelement;
        private IWebDriver driver;
        private bool nochange = false;
        private bool isStop = false;
        private bool isPlay = false;
        Queue<string> lValue = new Queue<string>();
        Queue<string> lTimeDelay = new Queue<string>();
        Queue<string> lDown = new Queue<string>();
        Queue<string> lDuplic = new Queue<string>();

        string xPathClick = null;


        #region Support Selenium
        private void OpenBrowser()
        {
            FirefoxProfileManager profilemanager = new FirefoxProfileManager();
            FirefoxProfile profile = (profilemanager.GetProfile("123"));//pathsToProfiles[0]);
            driver = new FirefoxDriver(new FirefoxBinary(@"FF\firefox.exe"), profile);
        }
        private bool isExistXpath(By _by, out string value)
        {
            value = "";
            try
            {
                myelement = driver.FindElement(_by);
                value = myelement.Text;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void Setinput(By _by, string _key)
        {
            //wait.Until(ExpectedConditions.ElementExists(By.CssSelector("input[type='text']"))); 
            while (true)
            {
                try
                {
                    myelement = driver.FindElement(_by);
                    break;
                }
                catch (Exception ex)
                {
                    Thread.Sleep(1000);
                }
            }
            while (myelement.Enabled == false || myelement.Displayed == false)
            {
                Thread.Sleep(1000);
            }
            try
            {
                //myelement.Click();
                //myelement.Clear();
                //Thread.Sleep(500);
                myelement.SendKeys(OpenQA.Selenium.Keys.Control + "a");
                //return;
                myelement.SendKeys(OpenQA.Selenium.Keys.Delete);

                myelement.SendKeys(_key);
                Thread.Sleep(1000);
            }
            catch (NoSuchElementException)
            {
            }
        }
        private string GetValue(By _by)
        {
            while (true)
            {
                try
                {
                    myelement = driver.FindElement(_by);
                    string value = myelement.Text;
                    if (!string.IsNullOrEmpty(value))
                    {
                        return value;
                    }
                    else
                        continue;
                }
                catch
                {
                    Thread.Sleep(1000);
                }
            }

        }
        private void ClickElement(By _by)
        {
            while (true)
            {
                try
                {
                    myelement = driver.FindElement(_by);
                    break;
                }
                catch (Exception ex)
                {
                    Thread.Sleep(1000);
                }
            }
            while (myelement.Enabled == false || myelement.Displayed == false)
            {
                Thread.Sleep(1000);
            }
            try
            {
                myelement.Click();
                //myelement.Clear();
                //myelement.SendKeys(_key);
                Thread.Sleep(1000);
            }
            catch (NoSuchElementException)
            {
            }
        }
        #endregion

        #region Support
        private void Delay(int time)
        {
            for (int i = 0; i < time; i++)
            {
                lbStatus.Text = $"Delay: {time - i}";
                if (isStop)
                {
                    lbStatus.Text = "Stop";
                    return;
                }
                Thread.Sleep(1000);
            }
        }
        private void Delay2(int time)
        {
            for (int i = 0; i < time; i++)
            {
                //driver.PageSource.Contains("123");
                lbStatus.Text = $"Delay: {time - i}";
                if (i >= 9)
                {
                    string valueReal = "";
                    bool Completed = isExistXpath(By.XPath(xPathCheckComplete), out valueReal);
                    if (Completed)
                        return;
                }
                Thread.Sleep(1000);
            }
        }
        private void GetValue()
        {
            lValue.Clear();
            lValue.Enqueue(txtValue2.Text);
            lValue.Enqueue(txtValue3.Text);
            lValue.Enqueue(txtValue4.Text);
            lValue.Enqueue(txtValue5.Text);
            lValue.Enqueue(txtValue6.Text);
            lValue.Enqueue(txtValue7.Text);
            lValue.Enqueue(txtValue8.Text);
            lValue.Enqueue(txtValue9.Text);
            lValue.Enqueue(txtValue10.Text);
            lValue.Enqueue(txtValue11.Text);
            lValue.Enqueue(txtValue12.Text);

            lDuplic.Enqueue(txtValue10_2.Text);
            lDuplic.Enqueue(txtValue11_2.Text);
            lDuplic.Enqueue(txtValue12_2.Text);

            lTimeDelay.Clear();
            lTimeDelay.Enqueue(txtDelay2.Text);
            lTimeDelay.Enqueue(txtDelay3.Text);
            lTimeDelay.Enqueue(txtDelay4.Text);
            lTimeDelay.Enqueue(txtDelay5.Text);
            lTimeDelay.Enqueue(txtDelay6.Text);
            lTimeDelay.Enqueue(txtDelay7.Text);
            lTimeDelay.Enqueue(txtDelay8.Text);
            lTimeDelay.Enqueue(txtDelay9.Text);
            lTimeDelay.Enqueue(txtDelay10.Text);
            lTimeDelay.Enqueue(txtDelay11.Text);
            lTimeDelay.Enqueue(txtDelay12.Text);


            lDown.Clear();
            lDown.Enqueue(DownUp(cboxDown2.Checked));
            lDown.Enqueue(DownUp(cboxDown3.Checked));
            lDown.Enqueue(DownUp(cboxDown4.Checked));
            lDown.Enqueue(DownUp(cboxDown5.Checked));
            lDown.Enqueue(DownUp(cboxDown6.Checked));
            lDown.Enqueue(DownUp(cboxDown7.Checked));
            lDown.Enqueue(DownUp(cboxDown8.Checked));
            lDown.Enqueue(DownUp(cboxDown9.Checked));
            lDown.Enqueue(DownUp(cboxDown10.Checked));
            lDown.Enqueue(DownUp(cboxDown11.Checked));
            lDown.Enqueue(DownUp(cboxDown12.Checked));



        }
        private string DownUp(bool checkbox) { return checkbox ? xPathClickDown : xPathClickUp; }

        #endregion
        private bool Put(string value)
        {
            lbStatus.Text = "Send Money";
            //Setinput(By.XPath("//input[@class='dropdown-toggle mousetrap dropdown-title ng-pristine ng-valid ng-not-empty ng-touched']"),value);
            Setinput(By.XPath(xPathMoney), value);
            //string time = GetValue(By.XPath("//*[@id=\"eur-graphs\"]/div/div/aside[2]/div[1]/dl[2]/dd/div/button/span"));
            Delay(1);
            lbStatus.Text = "Click";
            ClickElement(By.XPath(xPathClick));
            //if (cboxDown.Checked)
            //    ClickElement(By.XPath(xPathClickDown));
            //else
            //    ClickElement(By.XPath(xPathClickUp));
            lbStatus.Text = "Delay";
            Delay2(100);
            lbStatus.Text = "Get Money";
            //2 = usd
            string win = GetValue(By.XPath(xPathGetMoney)).Replace(",",string.Empty).Remove(0,1);
            double dWin = Convert.ToDouble(win);
            if (dWin == Convert.ToDouble(value))
            {
                nochange = true;
                ClickElement(By.XPath(xPathClose));

                return true;
            }
            if (dWin != 0)
            {
                txtPlay.AppendText($"Win: + {win}$" + Environment.NewLine);
                ClickElement(By.XPath(xPathClose));

                return true;
            }
            txtPlay.AppendText($"Lose: - {value}$" + Environment.NewLine);
            ClickElement(By.XPath(xPathClose));
            //Delay(1);
            return false;

        }
        private bool Put2(string value1, int count)
        {
            lbStatus.Text = "Send Money";
            //Setinput(By.XPath("//input[@class='dropdown-toggle mousetrap dropdown-title ng-pristine ng-valid ng-not-empty ng-touched']"),value);
            Setinput(By.XPath(xPathMoney), value1);
            //string time = GetValue(By.XPath("//*[@id=\"eur-graphs\"]/div/div/aside[2]/div[1]/dl[2]/dd/div/button/span"));
            Delay(1);
            for (int i = 0; i < count; i++)
            {
                //Delay(2);
                lbStatus.Text = "Click";
                ClickElement(By.XPath(xPathClick));
                Thread.Sleep(500);

            }
            //lbStatus.Text = "Click";
            //ClickElement(By.XPath(xPathClick));

            //Delay(1);

            //Setinput(By.XPath(xPathMoney), value2);
            //string time = GetValue(By.XPath("//*[@id=\"eur-graphs\"]/div/div/aside[2]/div[1]/dl[2]/dd/div/button/span"));
            //Delay(1);
            //lbStatus.Text = "Click";
            //ClickElement(By.XPath(xPathClick));

            lbStatus.Text = "Delay";
            Delay2(100);
            lbStatus.Text = "Get Money";
            //2 = usd
            string win = GetValue(By.XPath(xPathGetMoney)).Remove(0, 1);
            double dWin = Convert.ToDouble(win);
            if (dWin == Convert.ToDouble(value1)*count)
            {
                nochange = true;
                ClickElement(By.XPath(xPathClose));

                return true;
            }
            if (dWin != 0)
            {
                txtPlay.AppendText($"Win: + {win}$" + Environment.NewLine);
                ClickElement(By.XPath(xPathClose));

                return true;
            }
            txtPlay.AppendText($"Lose: - {value1}*{count}$" + Environment.NewLine);
            ClickElement(By.XPath(xPathClose));

            return false;

        }

        private void btnOpenBrowser_Click(object sender, EventArgs e)
        {
            OpenBrowser();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            driver.Navigate().GoToUrl("https://app.expertoption.com/");
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (lValue.Count == 0)
            {
                MessageBox.Show("Chưa add Value");
                return;
            }
            string value = txtValue1.Text;
            string time = txtDelay1.Text;
            xPathClick = DownUp(cboxDown1.Checked);
            isStop = false;
            isPlay = true;
            new Thread(() =>
            {
                while (!isStop)
                {
                    bool win = false;
                    if (lValue.Count <= 2)
                    {
                        int dup = Convert.ToInt32(lDuplic.Dequeue());
                        win = Put2(txtValue10.Text, dup);
                        //lValue.Clear();
                    }
                    else
                        win = Put(value);
                    if (!win)
                    {
                        if (lValue.Count == 0)
                        {
                            lbStatus.Text = "Stop";
                            return;
                        }
                        value = lValue.Dequeue();
                        time = lTimeDelay.Dequeue();
                        xPathClick = lDown.Dequeue();
                        while (string.IsNullOrEmpty(value))
                        {
                            if (lValue.Count == 0)
                            {
                                lbStatus.Text = "Stop";
                                return;
                            }
                            value = lValue.Dequeue();

                        }

                    }
                    else if (win && nochange)
                    {
                        nochange = false;
                    }
                    else
                    {
                        value = txtValue1.Text;
                        time = txtDelay1.Text;
                        xPathClick = DownUp(cboxDown1.Checked);
                        GetValue();
                    }
                    Delay(Convert.ToInt32(time));
                }
                btnPlay.Enabled = true;
            }).Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GetValue();
            MessageBox.Show("Lưu thành công");
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            isStop = true;
            lbStatus.Text = "Stop";
            if (isPlay)
                btnPlay.Enabled = false;
            isPlay = false;
        }
    }
}
