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

        public override string ToString()
        {
            return string.Format("{0}: {1}{2}", TaskName, condition.ToString(), action.ToString());
        }
    }
}
