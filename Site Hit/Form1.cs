using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace Site_Hit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void proxyList()
        {
            try
            {
                listBox_Proxy.Items.Clear();

                Uri url = new Uri("http://proxy-list.org/english/index.php?p=1");
                WebClient client = new WebClient() { Encoding = Encoding.UTF8 };
                string html = client.DownloadString(url);

                HtmlAgilityPack.HtmlDocument dokuman = new HtmlAgilityPack.HtmlDocument();
                dokuman.LoadHtml(html);
                HtmlNodeCollection XPath = dokuman.DocumentNode.SelectNodes("//*[@id='proxy-table']/div[2]/div");
                foreach (var veri in XPath)
                {
                    richTextBox1.Text = veri.InnerHtml;
                }

                HtmlAgilityPack.HtmlDocument dokuman2 = new HtmlAgilityPack.HtmlDocument();
                dokuman2.LoadHtml(richTextBox1.Text);
                HtmlNodeCollection XPath2 = dokuman2.DocumentNode.SelectNodes("//li[@class='proxy']");
                foreach (var veri2 in XPath2)
                {
                    listBox_Proxy.Items.Add(veri2.InnerText);
                }
            }
            catch { }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        int limit = 0;
        int sınır = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            limit = limit + 1;

            if (sınır == int.Parse(textBox4.Text))
            {
                button2.Enabled = false;
                button1.Enabled = true;
                timer1.Enabled = false;
                limit = 0;
                sınır = 0;
                hit = 0;
                MessageBox.Show("İşlem tamamlandı","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                if (limit == 1)
                {
                    proxyList();
                    webBrowser1.Navigate(textBox1.Text);
                }

                if (limit == 3)
                {
                    Random rnd = new Random();
                    listBox_Proxy.SelectedIndex = rnd.Next(0, listBox_Proxy.Items.Count);
                    Proxy.ProxyAyarla(listBox_Proxy.Text);
                    webBrowser1.Navigate(textBox1.Text);
                }

                if (limit == 4 + int.Parse(textBox2.Text))
                {
                    sınır = sınır + 1;
                    limit = 0;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;
            timer1.Enabled = true;
            textBox3.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button1.Enabled = true;
            timer1.Enabled = false;
        }

        int hit = 0;
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            hit = hit + 1;
            textBox3.Text = hit.ToString();

            int kalan = int.Parse(textBox4.Text) - int.Parse(textBox3.Text);
            textBox5.Text = kalan.ToString();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}
