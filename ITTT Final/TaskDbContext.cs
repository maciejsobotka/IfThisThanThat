using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITTT_Final
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext()
            : base("BaseTask")
        {
            // Użyj klasy TaskDbInitializer do zainicjalizowania bazy danych
            Database.SetInitializer<TaskDbContext>(new TaskDbInitializer());
        }

        public DbSet<Task> Task { get; set; }
        public DbSet<ITTTCondition> Condition { get; set; }
        public DbSet<ITTTAction> Action { get; set; }
    }
}
