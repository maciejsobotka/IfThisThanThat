<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITTT_Final
{
    [Serializable()]        // umożliwia serializacje klasy
    public class Task : System.Data.Entity.DropCreateDatabaseIfModelChanges<TaskDbContext>
    {
        public int Id { get; set; }
        public string TaskName { get; set; }

        public ITTTCondition condition;
        public ITTTAction action;

        public Task()
        {
            Tasks = new List<Task>();
        }

        public List<Task> Tasks { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}{2}", TaskName, condition.ToString(), action.ToString());
        }
    }
}
=======
﻿using System;
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
>>>>>>> 12b55fc8d846e5da9dd2a966c1e98e74c3407a21
