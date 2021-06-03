using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel;

namespace TestApp
{
    class TestContext : DbContext
    {

        public TestContext()
            :base("DbConnection")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<TestContext>());
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Order> Orders { get; set; }

       
        //disable cascade deletion
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.
                Entity<Unit>().
                HasMany(u => u.Employees).
                WithRequired(e => e.Unit).
                HasForeignKey(s => s.UnitId).
                WillCascadeOnDelete(false);

            modelBuilder.
                Entity<Employee>().
                HasMany(e => e.Subordinated).
                WithOptional(u => u.Head).
                HasForeignKey(s => s.HeadId).
                WillCascadeOnDelete(false);

            modelBuilder.
                Entity<Employee>().
                HasMany(e => e.Orders).
                WithRequired(o => o.Author).
                HasForeignKey(s => s.AuthorId).
                WillCascadeOnDelete(false);



        }

    }
}
