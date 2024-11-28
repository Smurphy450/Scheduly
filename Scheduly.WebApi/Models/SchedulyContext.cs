using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Scheduly.WebApi.Models;

public partial class SchedulyContext : DbContext
{
    public SchedulyContext()
    {
    }

    public SchedulyContext(DbContextOptions<SchedulyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Absence> Absences { get; set; }

    public virtual DbSet<AbsenceType> AbsenceTypes { get; set; }

    public virtual DbSet<AdminSetting> AdminSettings { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Premise> Premises { get; set; }

    public virtual DbSet<PremiseCategory> PremiseCategories { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<ResourceCategory> ResourceCategories { get; set; }

    public virtual DbSet<TimeRegistration> TimeRegistrations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<ZipCode> ZipCodes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=Scheduly;TrustServerCertificate=True;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Absence>(entity =>
        {
            entity.HasKey(e => e.AbsenceId).HasName("PK_dbo_Absence$AbsenceID");

            entity.ToTable("Absence");

            entity.Property(e => e.AbsenceId).HasColumnName("AbsenceID");
            entity.Property(e => e.AbsenceTypeId).HasColumnName("AbsenceTypeID");
            entity.Property(e => e.End)
                .HasPrecision(2)
                .HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Start)
                .HasPrecision(2)
                .HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.AbsenceType).WithMany(p => p.Absences)
                .HasForeignKey(d => d.AbsenceTypeId)
                .HasConstraintName("FK_dbo_AbsenceType$Absence_AbsenceTypeID");

            entity.HasOne(d => d.User).WithMany(p => p.Absences)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo_User$Absence_UserID");
        });

        modelBuilder.Entity<AbsenceType>(entity =>
        {
            entity.HasKey(e => e.AbsenceTypeId).HasName("PK_dbo_AbsenceType$AbsenceTypeID");

            entity.ToTable("AbsenceType");

            entity.Property(e => e.AbsenceTypeId).HasColumnName("AbsenceTypeID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.WageFactor).HasColumnType("decimal(9, 4)");
        });

        modelBuilder.Entity<AdminSetting>(entity =>
        {
            entity.HasKey(e => e.SettingsId).HasName("PK_dbo_AdminSettings$SettingsID");

            entity.Property(e => e.SettingsId).HasColumnName("SettingsID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingsId).HasName("PK_dbo_Bookings$BookingsID");

            entity.Property(e => e.BookingsId).HasColumnName("BookingsID");
            entity.Property(e => e.End)
                .HasPrecision(2)
                .HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.PremiseId).HasColumnName("PremiseID");
            entity.Property(e => e.ResourceId).HasColumnName("ResourceID");
            entity.Property(e => e.Start)
                .HasPrecision(2)
                .HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Premise).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.PremiseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_dbo_Premises$Bookings_PremiseID");

            entity.HasOne(d => d.Resource).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ResourceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_dbo_Resources$Bookings_ResourceID");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo_User$Bookings_UserID");
        });

        modelBuilder.Entity<Premise>(entity =>
        {
            entity.HasKey(e => e.PremiseId).HasName("PK_dbo_Premises$PremiseID");

            entity.Property(e => e.PremiseId).HasColumnName("PremiseID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PremiseCategoryId).HasColumnName("PremiseCategoryID");
            entity.Property(e => e.Size).HasMaxLength(10);

            entity.HasOne(d => d.PremiseCategory).WithMany(p => p.Premises)
                .HasForeignKey(d => d.PremiseCategoryId)
                .HasConstraintName("FK_dbo_PremiseCategory$Premises_PremiseCategoryID");
        });

        modelBuilder.Entity<PremiseCategory>(entity =>
        {
            entity.HasKey(e => e.PremiseCategoryId).HasName("PK_dbo_PremiseCategory$PremiseCategoryID");

            entity.ToTable("PremiseCategory");

            entity.Property(e => e.PremiseCategoryId).HasColumnName("PremiseCategoryID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PK_dbo_Profiles$ProfileID");

            entity.Property(e => e.ProfileId).HasColumnName("ProfileID");
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(16);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Profiles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_dbo_User$Profiles_UserID");

            entity.HasOne(d => d.ZipCodeNavigation).WithMany(p => p.Profiles)
                .HasForeignKey(d => d.ZipCode)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_dbo_ZipCodes$Profiles_ZipCode");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.ResourceId).HasName("PK_dbo_Resources$ResourceID");

            entity.Property(e => e.ResourceId).HasColumnName("ResourceID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Category).WithMany(p => p.Resources)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_dbo_ResourceCategory$Resources_CategoryID");
        });

        modelBuilder.Entity<ResourceCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK_dbo_ResourceCategory$CategoryID");

            entity.ToTable("ResourceCategory");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TimeRegistration>(entity =>
        {
            entity.HasKey(e => e.TimeId).HasName("PK_dbo_TimeRegistration$TimeID");

            entity.ToTable("TimeRegistration");

            entity.Property(e => e.TimeId).HasColumnName("TimeID");
            entity.Property(e => e.End)
                .HasPrecision(2)
                .HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.Start)
                .HasPrecision(2)
                .HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.TimeRegistrations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo_TimeRegistration$Bookings_UserID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_dbo_User$UserID");

            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<ZipCode>(entity =>
        {
            entity.HasKey(e => e.ZipCode1).HasName("PK_dbo_ZipCodes$ZipCode");

            entity.Property(e => e.ZipCode1)
                .ValueGeneratedNever()
                .HasColumnName("ZipCode");
            entity.Property(e => e.City).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
