using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITTT_Final
{
    public class ITTTDbContext : DbContext
    {
        public ITTTDbContext()
            : base("ITTTDataBase")
        {
            // Użyj klasy TaskDbInitializer do zainicjalizowania bazy danych
            Database.SetInitializer<ITTTDbContext>(new ITTTDbInitializer());
        }

        public DbSet<Task> Task { get; set; }
        public DbSet<ITTTCondition> Condition { get; set; }
        public DbSet<ITTTAction> Action { get; set; }

        public void ClearDb()
        {
            var task = Task;
            var cond = Condition;
            var act = Action;
            foreach (var t in task)
            {
                Task.Remove(t);
            }

            foreach (var c in cond)
            {
                Condition.Remove(c);
            }

            foreach (var a in act)
            {
                Action.Remove(a);
            }
            SaveChanges();
        }
    }
}
