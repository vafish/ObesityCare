using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ObesityCare.Models
{
    public partial class ClaimModel : DbContext
    {
        public ClaimModel()
            : base("name=ClaimModel")
        {
        }

        public virtual DbSet<Claim> Claim { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Claim>()
                .Property(e => e.Item)
                .IsFixedLength();

            modelBuilder.Entity<Claim>()
                .Property(e => e.UserId)
                .IsFixedLength();
        }
    }
}
