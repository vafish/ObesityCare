using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ObesityCare.Models
{
    public partial class StarModel : DbContext
    {
        public StarModel()
            : base("name=StarModel")
        {
        }

        public virtual DbSet<Star> Star { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Star>()
                .Property(e => e.Amount);
                
        }
    }
}
