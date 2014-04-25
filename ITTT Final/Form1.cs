using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



using System.Security.Permissions;


namespace ITTT_Final
{
    public partial class Form1 : Form
    {
        private Serialization s;
        private List<Task> list;
        private int taskNumber;
        public Form1()
        {         
            InitializeComponent();
            s = new Serialization(this);
            list = new List<Task>();
            taskNumber = 0;
            Logs.Info("Start programu");
        }
        public void UpdateInfoLabel(string nfo)
        {
            label14.Text = nfo;
            Application.DoEvents();
        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Task tmp = new Task();
            if (tabControl1.SelectedIndex == 0)
            {
                tmp.condition = new ITTTConditionPicture();
                tmp.condition.Url = "http://www." + textBox1.Text + "/";
                tmp.condition.Text = textBox2.Text;
            }
            if (tabControl1.SelectedIndex == 1)
            {
                tmp.condition = new ITTTConditionPicture();
                tmp.condition.Url = textBox5.Text;
                tmp.condition.Text = numericUpDown1.Text;
            }
            if (tabControl2.SelectedIndex == 0)
            {
                tmp.action = new ITTTActionSendMail();
                tmp.action.Address = textBox3.Text;
            }
            if (tabControl2.SelectedIndex == 1)
            {
                tmp.action = new ITTTActionShowWindow();
                tmp.action.Address = "";
            }
            tmp.TaskName = textBox4.Text;
            listBox1.Items.Add(tmp.ToString());
            list.Add(tmp);
            taskNumber++;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Net_Functions net = new Net_Functions();
            Task tmp = new Task();
            string fileName = "pic.jpg";
            for (int i = 0; i < taskNumber; i++)
            {
                tmp = list[i];
                if (net.Is_Url(tmp.condition.Url))
                {
                    string html = net.GetPageHtml(tmp.condition.Url);
                    if (tmp.condition.CheckCondition(html, fileName))
                    {
                        tmp.action.ExecuteAction(fileName, tmp.condition.Url, tmp.condition.Text);
                        Logs.Info("Wykonano akcje");
                        UpdateInfoLabel("Wykonano akcje");
                    }
                    else
                    {
                        UpdateInfoLabel("Nie znaleziono obrazka z podanym tekstem");
                        Logs.Error("Nie znaleziono obrazka z podanym tekstem");
                    }
                }
                else
                {
                    UpdateInfoLabel("Podany Url nie istnieje");
                    Logs.Error("Podany Url nie istnieje");
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            list.Clear();
            taskNumber = 0;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            s.Serialize(list);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            list.Clear();
            list = s.DeSerialize();
            taskNumber = list.Count;
            for (int i = 0; i < taskNumber; i++)
                listBox1.Items.Add(list[i].ToString());
         }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Logs.Info("Zakończenie programu");
        }
    }
}
