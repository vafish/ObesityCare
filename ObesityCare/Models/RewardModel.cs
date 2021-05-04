using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ObesityCare.Models
{
    public partial class RewardModel : DbContext
    {
        public RewardModel()
            : base("name=RewardModel1")
        {
        }

        public virtual DbSet<Reward> Rewards { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
