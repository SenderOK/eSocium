using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eSocium.Domain.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace eSocium.Domain.Concrete
{
    public class SurveysContext : DbContext
    {
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Respondent> Respondents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Question>()
                        .HasRequired(q => q.Survey)
                        .WithMany(p => p.Questions)
                        .WillCascadeOnDelete();
            modelBuilder.Entity<Answer>()
                        .HasRequired(a => a.Question)
                        .WithMany(q => q.Answers)
                        .WillCascadeOnDelete();
        }
    }
}
