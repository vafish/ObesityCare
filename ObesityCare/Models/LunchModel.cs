using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ObesityCare.Models
{
    public partial class LunchModel : DbContext
    {
        public LunchModel()
            : base("name=LunchModel")
        {
        }

        public virtual DbSet<Lunch> Lunch { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lunch>()
                .Property(e => e.UserId)
                .IsFixedLength();
        }
    }
}
