using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Threading;


namespace ITTT_Final
{
    public partial class Form1 : Form
    {
        private Serialization s;
        private List<Task> list;
        private int taskNumber;
        private DateTime time;
        ITTTDbContext db;

        public Form1()
        {         
            InitializeComponent();
            s = new Serialization(this);
            list = new List<Task>();
            db = new ITTTDbContext();
            taskNumber = 0;
            List<ITTTCondition> condList = new List<ITTTCondition>();
            List<ITTTAction> actList = new List<ITTTAction>();
            var task = db.Task;
            var cond = db.Condition;
            var act = db.Action;

            if(task!=null){
                foreach (var c in cond)
                {
                    condList.Add(c);
                    taskNumber++;
                }
                taskNumber=0;
                foreach (var a in act)
                {
                    actList.Add(a);
                    taskNumber++;    
                }
                taskNumber=0;
                foreach (var t in task)
                {
                    t.condition = condList[taskNumber];
                    t.action = actList[taskNumber];
                    list.Add(t);
                    listBox1.Items.Add(t.ToString());
                    taskNumber++;
                }
            }
            UpdateInfoBox("Start programu");
            Logs.Info("Start programu");
        }

        public void UpdateInfoBox(string nfo)
        {
            time = DateTime.Now;
            listBox2.Items.Add('[' + time.ToLongTimeString() + ' ' + time.ToShortDateString() + "] " + nfo);
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
            taskNumber++;
            if (tabControl1.SelectedIndex == 0)
            {
                tmp.condition = new ITTTConditionPicture();
                tmp.condition.Url = "http://www." + textBox1.Text + "/";
                tmp.condition.Text = textBox2.Text;
                tmp.condition.Id = taskNumber;
            }
            if (tabControl1.SelectedIndex == 1)
            {
                tmp.condition = new ITTTConditionWeather();
                tmp.condition.Url = textBox5.Text;
                tmp.condition.Text = numericUpDown1.Text;
                tmp.condition.Id = taskNumber;
            }
            if (tabControl2.SelectedIndex == 0)
            {
                tmp.action = new ITTTActionSendMail();
                tmp.action.Address = textBox3.Text;
                tmp.action.Id = taskNumber;
            }
            if (tabControl2.SelectedIndex == 1)
            {
                tmp.action = new ITTTActionShowWindow();
                tmp.action.Address = "";
                tmp.action.Id = taskNumber;
            }
            tmp.TaskName = textBox4.Text;
            tmp.Id = taskNumber;
            listBox1.Items.Add(tmp.ToString());
            list.Add(tmp);
            db.Condition.Add(tmp.condition);
            db.Action.Add(tmp.action);
            db.Task.Add(tmp);
            db.SaveChanges();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Task tmp = new Task();
            string fileName = "";
            string msg = "";

            for (int i = 0; i < taskNumber; i++)
            {
                tmp = list[i];
                if (tmp.condition.CheckCondition(ref fileName, ref msg, this))
                {
                    UpdateInfoBox("Warunek został spełniony");
                    Logs.Info("Warunek został spełniony");
                    tmp.action.ExecuteAction(fileName, msg);
                    UpdateInfoBox("Wykonano akcje");
                    Logs.Info("Wykonano akcje");
                }
                else
                {
                    UpdateInfoBox("Warunek nie został spełniony");
                    Logs.Info("Warunek nie został spełniony");
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            list.Clear();
            taskNumber = 0;
            db.ClearDb();     
            
            UpdateInfoBox("Wyczyszczono listę zadań");
            Logs.Info("Wyczyszczono listę zadań");
        }
        private void button4_Click(object sender, EventArgs e)
        {
            s.Serialize(list);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            list.Clear();
            db.ClearDb(); 
            list = s.DeSerialize();
            taskNumber = list.Count;
            for (int i = 0; i < taskNumber; i++)
            {
                listBox1.Items.Add(list[i].ToString());
                db.Condition.Add(list[i].condition);
                db.Action.Add(list[i].action);
                db.Task.Add(list[i]);
                db.SaveChanges();
            }
         }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            UpdateInfoBox("Zakończenie programu");
            Logs.Info("Zakończenie programu");
        }
    }
}
