using System.Data.Entity;
using HelpDeskApp.Data.Entities;

namespace HelpDeskApp.Data
{
    public class HelpDeskContext : DbContext
    {
        //Konstruktor - Datenbankname:
        public HelpDeskContext()
            : base("name=HelpDeskDb")
        { 
        }

        //Tabellen:
        public DbSet<TicketEntity> Tickets { get; set; }
        public DbSet<AttachmentEntity> Attachments { get; set; }
        public DbSet<LogEntity> LogEntries { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<TemplateEntity> Templates { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Beziehung Ticket zu Attachments
            modelBuilder.Entity<TicketEntity>()
                .HasMany(t => t.Attachments)
                .WithRequired(a => a.Ticket)
                .HasForeignKey(a => a.TicketId);
        }
    }
}
