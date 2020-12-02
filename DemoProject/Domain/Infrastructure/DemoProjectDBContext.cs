using DemoProject.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoProject.Domain.Infrastructure
{
    public class DemoProjectDBContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<CalendarDay> CalendarDays { get; set; }

        public DbSet<AMReservation> AMReservations { get; set; }

        public DbSet<PMReservation> PMReservations { get; set; }

        public DemoProjectDBContext(DbContextOptions<DemoProjectDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>(ConfigureRoom);
            modelBuilder.Entity<Reservation>(ConfigureReservation);
            modelBuilder.Entity<CalendarDay>(ConfigureCalendarDay);
            modelBuilder.Entity<AMReservation>(ConfigureAMReservation);
            modelBuilder.Entity<PMReservation>(ConfigurePMReservation);
        }

        private void ConfigureRoom(EntityTypeBuilder<Room> builder)
        {
            builder.OwnsOne(room => room.Owner);
        }

        private void ConfigureReservation(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasOne(reservation => reservation.Room)
                .WithMany(room => room.Reservations)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureCalendarDay(EntityTypeBuilder<CalendarDay> builder)
        {
            builder.HasMany(calendarDay => calendarDay.Reservations)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureAMReservation(EntityTypeBuilder<AMReservation> builder)
        {
            builder.HasOne(amReservation => amReservation.CalendarDay)
                .WithMany(calendarDay => calendarDay.AMFreeRooms)
                .HasForeignKey(amReservation => amReservation.CalendarDayId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(amReservation => amReservation.Room)
                .WithMany(room => room.AMFreeCalendarDays)
                .HasForeignKey(amReservation => amReservation.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasKey(amReservation => new { amReservation.CalendarDayId, amReservation.RoomId });
        }

        private void ConfigurePMReservation(EntityTypeBuilder<PMReservation> builder)
        {
            builder.HasOne(pmReservation => pmReservation.CalendarDay)
                .WithMany(calendarDay => calendarDay.PMFreeRooms)
                .HasForeignKey(pmReservation => pmReservation.CalendarDayId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pmReservation => pmReservation.Room)
                .WithMany(calendarDay => calendarDay.PMFreeCalendarDays)
                .HasForeignKey(pmReservation => pmReservation.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasKey(pmReservation => new { pmReservation.CalendarDayId, pmReservation.RoomId });
        }
    }
}
