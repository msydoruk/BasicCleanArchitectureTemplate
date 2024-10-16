using BasicCleanArchitectureTemplate.Infrastructure.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BasicCleanArchitectureTemplate.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventDataModel>()
                .HasOne(e => e.RecurrenceSetting) 
                .WithOne(r => r.Event)
                .HasForeignKey<RecurrenceSettingDataModel>(r => r.EventId);

            modelBuilder.Entity<RecurrenceSettingDataModel>()
                .Property(e => e.DayOfWeeks)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v), 
                    v => JsonConvert.DeserializeObject<List<DayOfWeek>>(v));
        }

        public DbSet<EventDataModel> Events { get; set; }

        public DbSet<RecurrenceSettingDataModel> RecurrenceSetting { get; set; }
    }
}