using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITTT_Final
{
    [Serializable()]        // umożliwia serializacje klasy
    public class Task 
    {
        public int Id { get; set; }
        public string TaskName { get; set; }

        public ITTTCondition condition;
        public ITTTAction action;

        public void CheckTask(Form1 form)
        {
            string fileName = "";
            string msg = "";

            if (condition.CheckCondition(ref fileName, ref msg, form))
                {
                    form.UpdateInfoBox("Warunek został spełniony");
                    Logs.Info("Warunek został spełniony");
                    action.ExecuteAction(fileName, msg);
                    form.UpdateInfoBox("Wykonano akcje");
                    Logs.Info("Wykonano akcje");
                }
                else
                {
                    form.UpdateInfoBox("Warunek nie został spełniony");
                    Logs.Info("Warunek nie został spełniony");
                }
        }
        public override string ToString()
        {
            return string.Format("{0}: {1}{2}", TaskName, condition.ToString(), action.ToString());
        }
    }
}
