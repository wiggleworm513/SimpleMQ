using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EventServer.Data.Entities
{
    public partial class EventQueueContext : DbContext
    {
        public EventQueueContext()
        {
        }

        public EventQueueContext(DbContextOptions<EventQueueContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<ProcessEvent> ProcessEvent { get; set; }
        public virtual DbSet<QueueStatus> QueueStatus { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.EventId)
                    .HasColumnName("EventID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.EventDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EventPayload)
                    .IsRequired()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EventSender)
                    .IsRequired()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.EventTopic)
                    .IsRequired()
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<ProcessEvent>(entity =>
            {
                entity.Property(e => e.ProcessEventId)
                    .HasColumnName("ProcessEventID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.ProcessDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ProcessMessage)
                    .IsRequired()
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<QueueStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.Property(e => e.StatusId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.StatusDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.StatusMessage)
                    .IsRequired()
                    .HasDefaultValueSql("('')");
            });
        }
    }
}
