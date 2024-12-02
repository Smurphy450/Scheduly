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

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Premise> Premises { get; set; }

    public virtual DbSet<PremiseCategory> PremiseCategories { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<ResourceCategory> ResourceCategories { get; set; }

    public virtual DbSet<SchedulyLogging> SchedulyLoggings { get; set; }

    public virtual DbSet<TimeRegistration> TimeRegistrations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<ZipCode> ZipCodes { get; set; }

    // TODO: If Database connection doesn't work, delete '\\SQLEXPRESS'
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

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK_dbo_Notifications$NotificationID");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.Message).HasMaxLength(255);
            entity.Property(e => e.Sms).HasColumnName("SMS");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo_User$Notifications_UserID");
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

        modelBuilder.Entity<SchedulyLogging>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK_dbo_SchedulyLogging$LogID");

            entity.ToTable("SchedulyLogging");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.Action).HasMaxLength(20);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.SchedulyLoggings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo_User$SchedulyLogging_UserID");
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

    public void SeedZipCodes()
    {
        if (!ZipCodes.Any())
        {
            var zipCodes = new List<ZipCode>
            {
                new ZipCode { ZipCode1 = 1000, City = "København K" },
                new ZipCode { ZipCode1 = 1050, City = "København K" },
                new ZipCode { ZipCode1 = 1100, City = "København K" },
                new ZipCode { ZipCode1 = 1150, City = "København K" },
                new ZipCode { ZipCode1 = 1200, City = "København K" },
                new ZipCode { ZipCode1 = 1250, City = "København K" },
                new ZipCode { ZipCode1 = 1300, City = "København K" },
                new ZipCode { ZipCode1 = 1350, City = "København K" },
                new ZipCode { ZipCode1 = 1400, City = "København K" },
                new ZipCode { ZipCode1 = 1450, City = "København K" },
                new ZipCode { ZipCode1 = 1500, City = "København V" },
                new ZipCode { ZipCode1 = 1550, City = "København V" },
                new ZipCode { ZipCode1 = 1600, City = "København V" },
                new ZipCode { ZipCode1 = 1650, City = "København V" },
                new ZipCode { ZipCode1 = 1700, City = "København V" },
                new ZipCode { ZipCode1 = 1750, City = "København V" },
                new ZipCode { ZipCode1 = 1800, City = "Frederiksberg C" },
                new ZipCode { ZipCode1 = 1850, City = "Frederiksberg C" },
                new ZipCode { ZipCode1 = 1900, City = "Frederiksberg C" },
                new ZipCode { ZipCode1 = 1950, City = "Frederiksberg C" },
                new ZipCode { ZipCode1 = 2000, City = "Frederiksberg" },
                new ZipCode { ZipCode1 = 2100, City = "København Ø" },
                new ZipCode { ZipCode1 = 2150, City = "Nordhavn" },
                new ZipCode { ZipCode1 = 2200, City = "København N" },
                new ZipCode { ZipCode1 = 2300, City = "København S" },
                new ZipCode { ZipCode1 = 2400, City = "København NV" },
                new ZipCode { ZipCode1 = 2450, City = "København SV" },
                new ZipCode { ZipCode1 = 2500, City = "Valby" },
                new ZipCode { ZipCode1 = 2600, City = "Glostrup" },
                new ZipCode { ZipCode1 = 2605, City = "Brøndby" },
                new ZipCode { ZipCode1 = 2610, City = "Rødovre" },
                new ZipCode { ZipCode1 = 2620, City = "Albertslund" },
                new ZipCode { ZipCode1 = 2625, City = "Vallensbæk" },
                new ZipCode { ZipCode1 = 2630, City = "Taastrup" },
                new ZipCode { ZipCode1 = 2635, City = "Ishøj" },
                new ZipCode { ZipCode1 = 2640, City = "Hedehusene" },
                new ZipCode { ZipCode1 = 2650, City = "Hvidovre" },
                new ZipCode { ZipCode1 = 2660, City = "Brøndby Strand" },
                new ZipCode { ZipCode1 = 2665, City = "Vallensbæk Strand" },
                new ZipCode { ZipCode1 = 2670, City = "Greve" },
                new ZipCode { ZipCode1 = 2680, City = "Solrød Strand" },
                new ZipCode { ZipCode1 = 2690, City = "Karlslunde" },
                new ZipCode { ZipCode1 = 2700, City = "Brønshøj" },
                new ZipCode { ZipCode1 = 2720, City = "Vanløse" },
                new ZipCode { ZipCode1 = 2730, City = "Herlev" },
                new ZipCode { ZipCode1 = 2740, City = "Skovlunde" },
                new ZipCode { ZipCode1 = 2750, City = "Ballerup" },
                new ZipCode { ZipCode1 = 2760, City = "Måløv" },
                new ZipCode { ZipCode1 = 2765, City = "Smørum" },
                new ZipCode { ZipCode1 = 2770, City = "Kastrup" },
                new ZipCode { ZipCode1 = 2791, City = "Dragør" },
                new ZipCode { ZipCode1 = 2800, City = "Kongens Lyngby" },
                new ZipCode { ZipCode1 = 2820, City = "Gentofte" },
                new ZipCode { ZipCode1 = 2830, City = "Virum" },
                new ZipCode { ZipCode1 = 2840, City = "Holte" },
                new ZipCode { ZipCode1 = 2850, City = "Nærum" },
                new ZipCode { ZipCode1 = 2860, City = "Søborg" },
                new ZipCode { ZipCode1 = 2870, City = "Dyssegård" },
                new ZipCode { ZipCode1 = 2880, City = "Bagsværd" },
                new ZipCode { ZipCode1 = 2900, City = "Hellerup" },
                new ZipCode { ZipCode1 = 2920, City = "Charlottenlund" },
                new ZipCode { ZipCode1 = 2930, City = "Klampenborg" },
                new ZipCode { ZipCode1 = 2942, City = "Skodsborg" },
                new ZipCode { ZipCode1 = 2950, City = "Vedbæk" },
                new ZipCode { ZipCode1 = 2960, City = "Rungsted Kyst" },
                new ZipCode { ZipCode1 = 2970, City = "Hørsholm" },
                new ZipCode { ZipCode1 = 2980, City = "Kokkedal" },
                new ZipCode { ZipCode1 = 2990, City = "Nivå" },
                new ZipCode { ZipCode1 = 3000, City = "Helsingør" },
                new ZipCode { ZipCode1 = 3050, City = "Humlebæk" },
                new ZipCode { ZipCode1 = 3060, City = "Espergærde" },
                new ZipCode { ZipCode1 = 3070, City = "Snekkersten" },
                new ZipCode { ZipCode1 = 3080, City = "Tikøb" },
                new ZipCode { ZipCode1 = 3100, City = "Hornbæk" },
                new ZipCode { ZipCode1 = 3120, City = "Dronningmølle" },
                new ZipCode { ZipCode1 = 3140, City = "Ålsgårde" },
                new ZipCode { ZipCode1 = 3150, City = "Hellebæk" },
                new ZipCode { ZipCode1 = 3200, City = "Helsinge" },
                new ZipCode { ZipCode1 = 3210, City = "Vejby" },
                new ZipCode { ZipCode1 = 3220, City = "Tisvildeleje" },
                new ZipCode { ZipCode1 = 3230, City = "Græsted" },
                new ZipCode { ZipCode1 = 3250, City = "Gilleleje" },
                new ZipCode { ZipCode1 = 3300, City = "Frederiksværk" },
                new ZipCode { ZipCode1 = 3310, City = "Ølsted" },
                new ZipCode { ZipCode1 = 3320, City = "Skævinge" },
                new ZipCode { ZipCode1 = 3330, City = "Gørløse" },
                new ZipCode { ZipCode1 = 3360, City = "Liseleje" },
                new ZipCode { ZipCode1 = 3370, City = "Melby" },
                new ZipCode { ZipCode1 = 3390, City = "Hundested" },
                new ZipCode { ZipCode1 = 3400, City = "Hillerød" },
                new ZipCode { ZipCode1 = 3450, City = "Allerød" },
                new ZipCode { ZipCode1 = 3460, City = "Birkerød" },
                new ZipCode { ZipCode1 = 3480, City = "Fredensborg" },
                new ZipCode { ZipCode1 = 3490, City = "Kvistgård" },
                new ZipCode { ZipCode1 = 3500, City = "Værløse" },
                new ZipCode { ZipCode1 = 3520, City = "Farum" },
                new ZipCode { ZipCode1 = 3540, City = "Lynge" },
                new ZipCode { ZipCode1 = 3550, City = "Slangerup" },
                new ZipCode { ZipCode1 = 3600, City = "Frederikssund" },
                new ZipCode { ZipCode1 = 3630, City = "Jægerspris" },
                new ZipCode { ZipCode1 = 3650, City = "Ølstykke" },
                new ZipCode { ZipCode1 = 3660, City = "Stenløse" },
                new ZipCode { ZipCode1 = 3670, City = "Veksø Sjælland" },
                new ZipCode { ZipCode1 = 3700, City = "Rønne" },
                new ZipCode { ZipCode1 = 3720, City = "Aakirkeby" },
                new ZipCode { ZipCode1 = 3730, City = "Nexø" },
                new ZipCode { ZipCode1 = 3740, City = "Svaneke" },
                new ZipCode { ZipCode1 = 3751, City = "Østermarie" },
                new ZipCode { ZipCode1 = 3760, City = "Gudhjem" },
                new ZipCode { ZipCode1 = 3770, City = "Allinge" },
                new ZipCode { ZipCode1 = 3782, City = "Klemensker" },
                new ZipCode { ZipCode1 = 3790, City = "Hasle" },
                new ZipCode { ZipCode1 = 4000, City = "Roskilde" },
                new ZipCode { ZipCode1 = 4040, City = "Jyllinge" },
                new ZipCode { ZipCode1 = 4050, City = "Skibby" },
                new ZipCode { ZipCode1 = 4060, City = "Kirke Såby" },
                new ZipCode { ZipCode1 = 4070, City = "Kirke Hyllinge" },
                new ZipCode { ZipCode1 = 4100, City = "Ringsted" },
                new ZipCode { ZipCode1 = 4130, City = "Viby Sjælland" },
                new ZipCode { ZipCode1 = 4140, City = "Borup" },
                new ZipCode { ZipCode1 = 4160, City = "Herlufmagle" },
                new ZipCode { ZipCode1 = 4171, City = "Glumsø" },
                new ZipCode { ZipCode1 = 4173, City = "Fjenneslev" },
                new ZipCode { ZipCode1 = 4174, City = "Jystrup Midtsj" },
                new ZipCode { ZipCode1 = 4180, City = "Sorø" },
                new ZipCode { ZipCode1 = 4190, City = "Munke Bjergby" },
                new ZipCode { ZipCode1 = 4200, City = "Slagelse" },
                new ZipCode { ZipCode1 = 4220, City = "Korsør" },
                new ZipCode { ZipCode1 = 4230, City = "Skælskør" },
                new ZipCode { ZipCode1 = 4241, City = "Vemmelev" },
                new ZipCode { ZipCode1 = 4242, City = "Boeslunde" },
                new ZipCode { ZipCode1 = 4243, City = "Rude" },
                new ZipCode { ZipCode1 = 4250, City = "Fuglebjerg" },
                new ZipCode { ZipCode1 = 4261, City = "Dalmose" },
                new ZipCode { ZipCode1 = 4262, City = "Sandved" },
                new ZipCode { ZipCode1 = 4270, City = "Høng" },
                new ZipCode { ZipCode1 = 4281, City = "Gørlev" },
                new ZipCode { ZipCode1 = 4291, City = "Ruds Vedby" },
                new ZipCode { ZipCode1 = 4293, City = "Dianalund" },
                new ZipCode { ZipCode1 = 4295, City = "Stenlille" },
                new ZipCode { ZipCode1 = 4296, City = "Nyrup" },
                new ZipCode { ZipCode1 = 4300, City = "Holbæk" },
                new ZipCode { ZipCode1 = 4320, City = "Lejre" },
                new ZipCode { ZipCode1 = 4330, City = "Hvalsø" },
                new ZipCode { ZipCode1 = 4340, City = "Tølløse" },
                new ZipCode { ZipCode1 = 4350, City = "Ugerløse" },
                new ZipCode { ZipCode1 = 4360, City = "Kirke Eskilstrup" },
                new ZipCode { ZipCode1 = 4370, City = "Store Merløse" },
                new ZipCode { ZipCode1 = 4390, City = "Vipperød" },
                new ZipCode { ZipCode1 = 4400, City = "Kalundborg" },
                new ZipCode { ZipCode1 = 4420, City = "Regstrup" },
                new ZipCode { ZipCode1 = 4440, City = "Mørkøv" },
                new ZipCode { ZipCode1 = 4450, City = "Jyderup" },
                new ZipCode { ZipCode1 = 4460, City = "Snertinge" },
                new ZipCode { ZipCode1 = 4470, City = "Svebølle" },
                new ZipCode { ZipCode1 = 4480, City = "Store Fuglede" },
                new ZipCode { ZipCode1 = 4490, City = "Jerslev Sjælland" },
                new ZipCode { ZipCode1 = 4500, City = "Nykøbing Sjælland" },
                new ZipCode { ZipCode1 = 4520, City = "Svinninge" },
                new ZipCode { ZipCode1 = 4532, City = "Gislinge" },
                new ZipCode { ZipCode1 = 4534, City = "Hørve" },
                new ZipCode { ZipCode1 = 4540, City = "Fårevejle" },
                new ZipCode { ZipCode1 = 4550, City = "Asnæs" },
                new ZipCode { ZipCode1 = 4560, City = "Vig" },
                new ZipCode { ZipCode1 = 4571, City = "Grevinge" },
                new ZipCode { ZipCode1 = 4572, City = "Nørre Asmindrup" },
                new ZipCode { ZipCode1 = 4573, City = "Højby" },
                new ZipCode { ZipCode1 = 4581, City = "Rørvig" },
                new ZipCode { ZipCode1 = 4583, City = "Sjællands Odde" },
                new ZipCode { ZipCode1 = 4591, City = "Føllenslev" },
                new ZipCode { ZipCode1 = 4592, City = "Sejerø" },
                new ZipCode { ZipCode1 = 4593, City = "Eskebjerg" },
                new ZipCode { ZipCode1 = 4600, City = "Køge" },
                new ZipCode { ZipCode1 = 4621, City = "Gadstrup" },
                new ZipCode { ZipCode1 = 4622, City = "Havdrup" },
                new ZipCode { ZipCode1 = 4623, City = "Lille Skensved" },
                new ZipCode { ZipCode1 = 4632, City = "Bjæverskov" },
                new ZipCode { ZipCode1 = 4640, City = "Faxe" },
                new ZipCode { ZipCode1 = 4652, City = "Hårlev" },
                new ZipCode { ZipCode1 = 4653, City = "Karise" },
                new ZipCode { ZipCode1 = 4654, City = "Faxe Ladeplads" },
                new ZipCode { ZipCode1 = 4660, City = "Store Heddinge" },
                new ZipCode { ZipCode1 = 4671, City = "Strøby" },
                new ZipCode { ZipCode1 = 4672, City = "Klippinge" },
                new ZipCode { ZipCode1 = 4673, City = "Rødvig Stevns" },
                new ZipCode { ZipCode1 = 4681, City = "Herfølge" },
                new ZipCode { ZipCode1 = 4682, City = "Tureby" },
                new ZipCode { ZipCode1 = 4683, City = "Rønnede" },
                new ZipCode { ZipCode1 = 4684, City = "Holmegaard" },
                new ZipCode { ZipCode1 = 4690, City = "Haslev" },
                new ZipCode { ZipCode1 = 4700, City = "Næstved" },
                new ZipCode { ZipCode1 = 4733, City = "Tappernøje" },
                new ZipCode { ZipCode1 = 4735, City = "Mern" },
                new ZipCode { ZipCode1 = 4736, City = "Karrebæksminde" },
                new ZipCode { ZipCode1 = 4750, City = "Lundby" },
                new ZipCode { ZipCode1 = 4760, City = "Vordingborg" },
                new ZipCode { ZipCode1 = 4771, City = "Kalvehave" },
                new ZipCode { ZipCode1 = 4772, City = "Langebæk" },
                new ZipCode { ZipCode1 = 4773, City = "Stensved" },
                new ZipCode { ZipCode1 = 4780, City = "Stege" },
                new ZipCode { ZipCode1 = 4791, City = "Borre" },
                new ZipCode { ZipCode1 = 4792, City = "Askeby" },
                new ZipCode { ZipCode1 = 4793, City = "Bogø By" },
                new ZipCode { ZipCode1 = 4800, City = "Nykøbing Falster" },
                new ZipCode { ZipCode1 = 4840, City = "Nørre Alslev" },
                new ZipCode { ZipCode1 = 4850, City = "Stubbekøbing" },
                new ZipCode { ZipCode1 = 4862, City = "Guldborg" },
                new ZipCode { ZipCode1 = 4863, City = "Eskilstrup" },
                new ZipCode { ZipCode1 = 4871, City = "Horbelev" },
                new ZipCode { ZipCode1 = 4872, City = "Idestrup" },
                new ZipCode { ZipCode1 = 4873, City = "Væggerløse" },
                new ZipCode { ZipCode1 = 4874, City = "Gedser" },
                new ZipCode { ZipCode1 = 4880, City = "Nysted" },
                new ZipCode { ZipCode1 = 4891, City = "Toreby L" },
                new ZipCode { ZipCode1 = 4892, City = "Kettinge" },
                new ZipCode { ZipCode1 = 4894, City = "Øster Ulslev" },
                new ZipCode { ZipCode1 = 4895, City = "Errindlev" },
                new ZipCode { ZipCode1 = 4900, City = "Nakskov" },
                new ZipCode { ZipCode1 = 4912, City = "Harpelunde" },
                new ZipCode { ZipCode1 = 4913, City = "Horslunde" },
                new ZipCode { ZipCode1 = 4920, City = "Søllested" },
                new ZipCode { ZipCode1 = 4930, City = "Maribo" },
                new ZipCode { ZipCode1 = 4941, City = "Bandholm" },
                new ZipCode { ZipCode1 = 4943, City = "Torrig L" },
                new ZipCode { ZipCode1 = 4944, City = "Fejø" },
                new ZipCode { ZipCode1 = 4951, City = "Nørreballe" },
                new ZipCode { ZipCode1 = 4952, City = "Stokkemarke" },
                new ZipCode { ZipCode1 = 4953, City = "Vesterborg" },
                new ZipCode { ZipCode1 = 4960, City = "Holeby" },
                new ZipCode { ZipCode1 = 4970, City = "Rødby" },
                new ZipCode { ZipCode1 = 4983, City = "Dannemare" },
                new ZipCode { ZipCode1 = 4990, City = "Sakskøbing" },
                new ZipCode { ZipCode1 = 5000, City = "Odense C" },
                new ZipCode { ZipCode1 = 5200, City = "Odense V" },
                new ZipCode { ZipCode1 = 5210, City = "Odense NV" },
                new ZipCode { ZipCode1 = 5220, City = "Odense SØ" },
                new ZipCode { ZipCode1 = 5230, City = "Odense M" },
                new ZipCode { ZipCode1 = 5240, City = "Odense NØ" },
                new ZipCode { ZipCode1 = 5250, City = "Odense SV" },
                new ZipCode { ZipCode1 = 5260, City = "Odense S" },
                new ZipCode { ZipCode1 = 5270, City = "Odense N" },
                new ZipCode { ZipCode1 = 5290, City = "Marslev" },
                new ZipCode { ZipCode1 = 5300, City = "Kerteminde" },
                new ZipCode { ZipCode1 = 5320, City = "Agedrup" },
                new ZipCode { ZipCode1 = 5330, City = "Munkebo" },
                new ZipCode { ZipCode1 = 5350, City = "Rynkeby" },
                new ZipCode { ZipCode1 = 5370, City = "Mesinge" },
                new ZipCode { ZipCode1 = 5380, City = "Dalby" },
                new ZipCode { ZipCode1 = 5390, City = "Martofte" },
                new ZipCode { ZipCode1 = 5400, City = "Bogense" },
                new ZipCode { ZipCode1 = 5450, City = "Otterup" },
                new ZipCode { ZipCode1 = 5462, City = "Morud" },
                new ZipCode { ZipCode1 = 5463, City = "Harndrup" },
                new ZipCode { ZipCode1 = 5464, City = "Brenderup Fyn" },
                new ZipCode { ZipCode1 = 5466, City = "Asperup" },
                new ZipCode { ZipCode1 = 5471, City = "Søndersø" },
                new ZipCode { ZipCode1 = 5474, City = "Veflinge" },
                new ZipCode { ZipCode1 = 5485, City = "Skamby" },
                new ZipCode { ZipCode1 = 5491, City = "Blommenslyst" },
                new ZipCode { ZipCode1 = 5492, City = "Vissenbjerg" },
                new ZipCode { ZipCode1 = 5500, City = "Middelfart" },
                new ZipCode { ZipCode1 = 5540, City = "Ullerslev" },
                new ZipCode { ZipCode1 = 5550, City = "Langeskov" },
                new ZipCode { ZipCode1 = 5560, City = "Aarup" },
                new ZipCode { ZipCode1 = 5580, City = "Nørre Aaby" },
                new ZipCode { ZipCode1 = 5591, City = "Gelsted" },
                new ZipCode { ZipCode1 = 5600, City = "Faaborg" },
                new ZipCode { ZipCode1 = 5610, City = "Assens" },
                new ZipCode { ZipCode1 = 5620, City = "Glamsbjerg" },
                new ZipCode { ZipCode1 = 5631, City = "Ebberup" },
                new ZipCode { ZipCode1 = 5642, City = "Millinge" },
                new ZipCode { ZipCode1 = 5672, City = "Broby" },
                new ZipCode { ZipCode1 = 5683, City = "Haarby" },
                new ZipCode { ZipCode1 = 5690, City = "Tommerup" },
                new ZipCode { ZipCode1 = 5700, City = "Svendborg" },
                new ZipCode { ZipCode1 = 5750, City = "Ringe" },
                new ZipCode { ZipCode1 = 5762, City = "Vester Skerninge" },
                new ZipCode { ZipCode1 = 5771, City = "Stenstrup" },
                new ZipCode { ZipCode1 = 5772, City = "Kværndrup" },
                new ZipCode { ZipCode1 = 5792, City = "Årslev" },
                new ZipCode { ZipCode1 = 5800, City = "Nyborg" },
                new ZipCode { ZipCode1 = 5853, City = "Ørbæk" },
                new ZipCode { ZipCode1 = 5854, City = "Gislev" },
                new ZipCode { ZipCode1 = 5856, City = "Ryslinge" },
                new ZipCode { ZipCode1 = 5863, City = "Ferritslev Fyn" },
                new ZipCode { ZipCode1 = 5871, City = "Frørup" },
                new ZipCode { ZipCode1 = 5874, City = "Hesselager" },
                new ZipCode { ZipCode1 = 5881, City = "Skårup Fyn" },
                new ZipCode { ZipCode1 = 5882, City = "Vejstrup" },
                new ZipCode { ZipCode1 = 5883, City = "Oure" },
                new ZipCode { ZipCode1 = 5884, City = "Gudme" },
                new ZipCode { ZipCode1 = 5892, City = "Gudbjerg Sydfyn" },
                new ZipCode { ZipCode1 = 5900, City = "Rudkøbing" },
                new ZipCode { ZipCode1 = 5932, City = "Humble" },
                new ZipCode { ZipCode1 = 5935, City = "Bagenkop" },
                new ZipCode { ZipCode1 = 5953, City = "Tranekær" },
                new ZipCode { ZipCode1 = 5960, City = "Marstal" },
                new ZipCode { ZipCode1 = 5970, City = "Ærøskøbing" },
                new ZipCode { ZipCode1 = 5985, City = "Søby Ærø" },
                new ZipCode { ZipCode1 = 6000, City = "Kolding" },
                new ZipCode { ZipCode1 = 6040, City = "Egtved" },
                new ZipCode { ZipCode1 = 6051, City = "Almind" },
                new ZipCode { ZipCode1 = 6052, City = "Viuf" },
                new ZipCode { ZipCode1 = 6064, City = "Jordrup" },
                new ZipCode { ZipCode1 = 6070, City = "Christiansfeld" },
                new ZipCode { ZipCode1 = 6091, City = "Bjert" },
                new ZipCode { ZipCode1 = 6092, City = "Sønder Stenderup" },
                new ZipCode { ZipCode1 = 6093, City = "Sjølund" },
                new ZipCode { ZipCode1 = 6094, City = "Hejls" },
                new ZipCode { ZipCode1 = 6100, City = "Haderslev" },
                new ZipCode { ZipCode1 = 6200, City = "Aabenraa" },
                new ZipCode { ZipCode1 = 6230, City = "Rødekro" },
                new ZipCode { ZipCode1 = 6240, City = "Løgumkloster" },
                new ZipCode { ZipCode1 = 6261, City = "Bredebro" },
                new ZipCode { ZipCode1 = 6270, City = "Tønder" },
                new ZipCode { ZipCode1 = 6280, City = "Højer" },
                new ZipCode { ZipCode1 = 6300, City = "Gråsten" },
                new ZipCode { ZipCode1 = 6310, City = "Broager" },
                new ZipCode { ZipCode1 = 6320, City = "Egernsund" },
                new ZipCode { ZipCode1 = 6330, City = "Padborg" },
                new ZipCode { ZipCode1 = 6340, City = "Kruså" },
                new ZipCode { ZipCode1 = 6360, City = "Tinglev" },
                new ZipCode { ZipCode1 = 6372, City = "Bylderup-Bov" },
                new ZipCode { ZipCode1 = 6392, City = "Bolderslev" },
                new ZipCode { ZipCode1 = 6400, City = "Sønderborg" },
                new ZipCode { ZipCode1 = 6430, City = "Nordborg" },
                new ZipCode { ZipCode1 = 6440, City = "Augustenborg" },
                new ZipCode { ZipCode1 = 6470, City = "Sydals" },
                new ZipCode { ZipCode1 = 6500, City = "Vojens" },
                new ZipCode { ZipCode1 = 6510, City = "Gram" },
                new ZipCode { ZipCode1 = 6520, City = "Toftlund" },
                new ZipCode { ZipCode1 = 6534, City = "Agerskov" },
                new ZipCode { ZipCode1 = 6535, City = "Branderup J" },
                new ZipCode { ZipCode1 = 6541, City = "Bevtoft" },
                new ZipCode { ZipCode1 = 6560, City = "Sommersted" },
                new ZipCode { ZipCode1 = 6580, City = "Vamdrup" },
                new ZipCode { ZipCode1 = 6600, City = "Vejen" },
                new ZipCode { ZipCode1 = 6621, City = "Gesten" },
                new ZipCode { ZipCode1 = 6622, City = "Bække" },
                new ZipCode { ZipCode1 = 6623, City = "Vorbasse" },
                new ZipCode { ZipCode1 = 6630, City = "Rødding" },
                new ZipCode { ZipCode1 = 6640, City = "Lunderskov" },
                new ZipCode { ZipCode1 = 6650, City = "Brørup" },
                new ZipCode { ZipCode1 = 6660, City = "Lintrup" },
                new ZipCode { ZipCode1 = 6670, City = "Holsted" },
                new ZipCode { ZipCode1 = 6682, City = "Hovborg" },
                new ZipCode { ZipCode1 = 6683, City = "Føvling" },
                new ZipCode { ZipCode1 = 6690, City = "Gørding" },
                new ZipCode { ZipCode1 = 6700, City = "Esbjerg" },
                new ZipCode { ZipCode1 = 6705, City = "Esbjerg Ø" },
                new ZipCode { ZipCode1 = 6710, City = "Esbjerg V" },
                new ZipCode { ZipCode1 = 6715, City = "Esbjerg N" },
                new ZipCode { ZipCode1 = 6720, City = "Fanø" },
                new ZipCode { ZipCode1 = 6731, City = "Tjæreborg" },
                new ZipCode { ZipCode1 = 6740, City = "Bramming" },
                new ZipCode { ZipCode1 = 6752, City = "Glejbjerg" },
                new ZipCode { ZipCode1 = 6753, City = "Agerbæk" },
                new ZipCode { ZipCode1 = 6760, City = "Ribe" },
                new ZipCode { ZipCode1 = 6771, City = "Gredstedbro" },
                new ZipCode { ZipCode1 = 6780, City = "Skærbæk" },
                new ZipCode { ZipCode1 = 6792, City = "Rømø" },
                new ZipCode { ZipCode1 = 6800, City = "Varde" },
                new ZipCode { ZipCode1 = 6818, City = "Årre" },
                new ZipCode { ZipCode1 = 6823, City = "Ansager" },
                new ZipCode { ZipCode1 = 6830, City = "Nørre Nebel" },
                new ZipCode { ZipCode1 = 6840, City = "Oksbøl" },
                new ZipCode { ZipCode1 = 6851, City = "Janderup Vestj" },
                new ZipCode { ZipCode1 = 6852, City = "Billum" },
                new ZipCode { ZipCode1 = 6853, City = "Vejers Strand" },
                new ZipCode { ZipCode1 = 6854, City = "Henne" },
                new ZipCode { ZipCode1 = 6855, City = "Outrup" },
                new ZipCode { ZipCode1 = 6857, City = "Blåvand" },
                new ZipCode { ZipCode1 = 6862, City = "Tistrup" },
                new ZipCode { ZipCode1 = 6870, City = "Ølgod" },
                new ZipCode { ZipCode1 = 6880, City = "Tarm" },
                new ZipCode { ZipCode1 = 6893, City = "Hemmet" },
                new ZipCode { ZipCode1 = 6900, City = "Skjern" },
                new ZipCode { ZipCode1 = 6920, City = "Videbæk" },
                new ZipCode { ZipCode1 = 6933, City = "Kibæk" },
                new ZipCode { ZipCode1 = 6940, City = "Lem St" },
                new ZipCode { ZipCode1 = 6950, City = "Ringkøbing" },
                new ZipCode { ZipCode1 = 6960, City = "Hvide Sande" },
                new ZipCode { ZipCode1 = 6971, City = "Spjald" },
                new ZipCode { ZipCode1 = 6973, City = "Ørnhøj" },
                new ZipCode { ZipCode1 = 6980, City = "Tim" },
                new ZipCode { ZipCode1 = 6990, City = "Ulfborg" },
                new ZipCode { ZipCode1 = 7000, City = "Fredericia" },
                new ZipCode { ZipCode1 = 7080, City = "Børkop" },
                new ZipCode { ZipCode1 = 7100, City = "Vejle" },
                new ZipCode { ZipCode1 = 7120, City = "Vejle Øst" },
                new ZipCode { ZipCode1 = 7130, City = "Juelsminde" },
                new ZipCode { ZipCode1 = 7140, City = "Stouby" },
                new ZipCode { ZipCode1 = 7150, City = "Barrit" },
                new ZipCode { ZipCode1 = 7160, City = "Tørring" },
                new ZipCode { ZipCode1 = 7171, City = "Uldum" },
                new ZipCode { ZipCode1 = 7173, City = "Vonge" },
                new ZipCode { ZipCode1 = 7182, City = "Bredsten" },
                new ZipCode { ZipCode1 = 7183, City = "Randbøl" },
                new ZipCode { ZipCode1 = 7184, City = "Vandel" },
                new ZipCode { ZipCode1 = 7190, City = "Billund" },
                new ZipCode { ZipCode1 = 7200, City = "Grindsted" },
                new ZipCode { ZipCode1 = 7250, City = "Hejnsvig" },
                new ZipCode { ZipCode1 = 7260, City = "Sønder Omme" },
                new ZipCode { ZipCode1 = 7270, City = "Stakroge" },
                new ZipCode { ZipCode1 = 7280, City = "Sønder Felding" },
                new ZipCode { ZipCode1 = 7300, City = "Jelling" },
                new ZipCode { ZipCode1 = 7321, City = "Gadbjerg" },
                new ZipCode { ZipCode1 = 7323, City = "Give" },
                new ZipCode { ZipCode1 = 7330, City = "Brande" },
                new ZipCode { ZipCode1 = 7361, City = "Ejstrupholm" },
                new ZipCode { ZipCode1 = 7362, City = "Hampen" },
                new ZipCode { ZipCode1 = 7400, City = "Herning" },
                new ZipCode { ZipCode1 = 7430, City = "Ikast" },
                new ZipCode { ZipCode1 = 7441, City = "Bording" },
                new ZipCode { ZipCode1 = 7442, City = "Engesvang" },
                new ZipCode { ZipCode1 = 7451, City = "Sunds" },
                new ZipCode { ZipCode1 = 7470, City = "Karup J" },
                new ZipCode { ZipCode1 = 7480, City = "Vildbjerg" },
                new ZipCode { ZipCode1 = 7490, City = "Aulum" },
                new ZipCode { ZipCode1 = 7500, City = "Holstebro" },
                new ZipCode { ZipCode1 = 7540, City = "Haderup" },
                new ZipCode { ZipCode1 = 7550, City = "Sørvad" },
                new ZipCode { ZipCode1 = 7560, City = "Hjerm" },
                new ZipCode { ZipCode1 = 7570, City = "Vemb" },
                new ZipCode { ZipCode1 = 7600, City = "Struer" },
                new ZipCode { ZipCode1 = 7620, City = "Lemvig" },
                new ZipCode { ZipCode1 = 7650, City = "Bøvlingbjerg" },
                new ZipCode { ZipCode1 = 7660, City = "Bækmarksbro" },
                new ZipCode { ZipCode1 = 7673, City = "Harboøre" },
                new ZipCode { ZipCode1 = 7680, City = "Thyborøn" },
                new ZipCode { ZipCode1 = 7700, City = "Thisted" },
                new ZipCode { ZipCode1 = 7730, City = "Hanstholm" },
                new ZipCode { ZipCode1 = 7741, City = "Frøstrup" },
                new ZipCode { ZipCode1 = 7742, City = "Vesløs" },
                new ZipCode { ZipCode1 = 7752, City = "Snedsted" },
                new ZipCode { ZipCode1 = 7755, City = "Bedsted Thy" },
                new ZipCode { ZipCode1 = 7760, City = "Hurup Thy" },
                new ZipCode { ZipCode1 = 7770, City = "Vestervig" },
                new ZipCode { ZipCode1 = 7790, City = "Thyholm" },
                new ZipCode { ZipCode1 = 7800, City = "Skive" },
                new ZipCode { ZipCode1 = 7830, City = "Vinderup" },
                new ZipCode { ZipCode1 = 7840, City = "Højslev" },
                new ZipCode { ZipCode1 = 7850, City = "Stoholm Jyll" },
                new ZipCode { ZipCode1 = 7860, City = "Spøttrup" },
                new ZipCode { ZipCode1 = 7870, City = "Roslev" },
                new ZipCode { ZipCode1 = 7884, City = "Fur" },
                new ZipCode { ZipCode1 = 7900, City = "Nykøbing M" },
                new ZipCode { ZipCode1 = 7950, City = "Erslev" },
                new ZipCode { ZipCode1 = 7960, City = "Karby" },
                new ZipCode { ZipCode1 = 7970, City = "Redsted M" },
                new ZipCode { ZipCode1 = 7980, City = "Vils" },
                new ZipCode { ZipCode1 = 7990, City = "Øster Assels" },
                new ZipCode { ZipCode1 = 8000, City = "Aarhus C" },
                new ZipCode { ZipCode1 = 8200, City = "Aarhus N" },
                new ZipCode { ZipCode1 = 8210, City = "Aarhus V" },
                new ZipCode { ZipCode1 = 8220, City = "Brabrand" },
                new ZipCode { ZipCode1 = 8230, City = "Åbyhøj" },
                new ZipCode { ZipCode1 = 8240, City = "Risskov" },
                new ZipCode { ZipCode1 = 8250, City = "Egå" },
                new ZipCode { ZipCode1 = 8260, City = "Viby J" },
                new ZipCode { ZipCode1 = 8270, City = "Højbjerg" },
                new ZipCode { ZipCode1 = 8300, City = "Odder" },
                new ZipCode { ZipCode1 = 8305, City = "Samsø" },
                new ZipCode { ZipCode1 = 8310, City = "Tranbjerg J" },
                new ZipCode { ZipCode1 = 8320, City = "Mårslet" },
                new ZipCode { ZipCode1 = 8330, City = "Beder" },
                new ZipCode { ZipCode1 = 8340, City = "Malling" },
                new ZipCode { ZipCode1 = 8350, City = "Hundslund" },
                new ZipCode { ZipCode1 = 8355, City = "Solbjerg" },
                new ZipCode { ZipCode1 = 8361, City = "Hasselager" },
                new ZipCode { ZipCode1 = 8362, City = "Hørning" },
                new ZipCode { ZipCode1 = 8370, City = "Hadsten" },
                new ZipCode { ZipCode1 = 8380, City = "Trige" },
                new ZipCode { ZipCode1 = 8381, City = "Tilst" },
                new ZipCode { ZipCode1 = 8382, City = "Hinnerup" },
                new ZipCode { ZipCode1 = 8400, City = "Ebeltoft" },
                new ZipCode { ZipCode1 = 8410, City = "Rønde" },
                new ZipCode { ZipCode1 = 8420, City = "Knebel" },
                new ZipCode { ZipCode1 = 8444, City = "Balle" },
                new ZipCode { ZipCode1 = 8450, City = "Hammel" },
                new ZipCode { ZipCode1 = 8462, City = "Harlev J" },
                new ZipCode { ZipCode1 = 8464, City = "Galten" },
                new ZipCode { ZipCode1 = 8471, City = "Sabro" },
                new ZipCode { ZipCode1 = 8472, City = "Sporup" },
                new ZipCode { ZipCode1 = 8500, City = "Grenaa" },
                new ZipCode { ZipCode1 = 8520, City = "Lystrup" },
                new ZipCode { ZipCode1 = 8530, City = "Hjortshøj" },
                new ZipCode { ZipCode1 = 8541, City = "Skødstrup" },
                new ZipCode { ZipCode1 = 8543, City = "Hornslet" },
                new ZipCode { ZipCode1 = 8544, City = "Mørke" },
                new ZipCode { ZipCode1 = 8550, City = "Ryomgård" },
                new ZipCode { ZipCode1 = 8560, City = "Kolind" },
                new ZipCode { ZipCode1 = 8570, City = "Trustrup" },
                new ZipCode { ZipCode1 = 8581, City = "Nimtofte" },
                new ZipCode { ZipCode1 = 8585, City = "Glesborg" },
                new ZipCode { ZipCode1 = 8586, City = "Ørum Djurs" },
                new ZipCode { ZipCode1 = 8592, City = "Anholt" },
                new ZipCode { ZipCode1 = 8600, City = "Silkeborg" },
                new ZipCode { ZipCode1 = 8620, City = "Kjellerup" },
                new ZipCode { ZipCode1 = 8632, City = "Lemming" },
                new ZipCode { ZipCode1 = 8641, City = "Sorring" },
                new ZipCode { ZipCode1 = 8643, City = "Ans By" },
                new ZipCode { ZipCode1 = 8653, City = "Them" },
                new ZipCode { ZipCode1 = 8654, City = "Bryrup" },
                new ZipCode { ZipCode1 = 8660, City = "Skanderborg" },
                new ZipCode { ZipCode1 = 8670, City = "Låsby" },
                new ZipCode { ZipCode1 = 8680, City = "Ry" },
                new ZipCode { ZipCode1 = 8700, City = "Horsens" },
                new ZipCode { ZipCode1 = 8721, City = "Daugård" },
                new ZipCode { ZipCode1 = 8722, City = "Hedensted" },
                new ZipCode { ZipCode1 = 8723, City = "Løsning" },
                new ZipCode { ZipCode1 = 8732, City = "Hovedgård" },
                new ZipCode { ZipCode1 = 8740, City = "Brædstrup" },
                new ZipCode { ZipCode1 = 8751, City = "Gedved" },
                new ZipCode { ZipCode1 = 8752, City = "Østbirk" },
                new ZipCode { ZipCode1 = 8762, City = "Flemming" },
                new ZipCode { ZipCode1 = 8763, City = "Rask Mølle" },
                new ZipCode { ZipCode1 = 8765, City = "Klovborg" },
                new ZipCode { ZipCode1 = 8766, City = "Nørre Snede" },
                new ZipCode { ZipCode1 = 8781, City = "Stenderup" },
                new ZipCode { ZipCode1 = 8783, City = "Hornsyld" },
                new ZipCode { ZipCode1 = 8800, City = "Viborg" },
                new ZipCode { ZipCode1 = 8830, City = "Tjele" },
                new ZipCode { ZipCode1 = 8831, City = "Løgstrup" },
                new ZipCode { ZipCode1 = 8832, City = "Skals" },
                new ZipCode { ZipCode1 = 8840, City = "Rødkærsbro" },
                new ZipCode { ZipCode1 = 8850, City = "Bjerringbro" },
                new ZipCode { ZipCode1 = 8860, City = "Ulstrup" },
                new ZipCode { ZipCode1 = 8870, City = "Langå" },
                new ZipCode { ZipCode1 = 8881, City = "Thorsø" },
                new ZipCode { ZipCode1 = 8882, City = "Fårvang" },
                new ZipCode { ZipCode1 = 8883, City = "Gjern" },
                new ZipCode { ZipCode1 = 8900, City = "Randers C" },
                new ZipCode { ZipCode1 = 8920, City = "Randers NV" },
                new ZipCode { ZipCode1 = 8930, City = "Randers NØ" },
                new ZipCode { ZipCode1 = 8940, City = "Randers SV" },
                new ZipCode { ZipCode1 = 8960, City = "Randers SØ" },
                new ZipCode { ZipCode1 = 8961, City = "Allingåbro" },
                new ZipCode { ZipCode1 = 8963, City = "Auning" },
                new ZipCode { ZipCode1 = 8970, City = "Havndal" },
                new ZipCode { ZipCode1 = 8981, City = "Spentrup" },
                new ZipCode { ZipCode1 = 8983, City = "Gjerlev J" },
                new ZipCode { ZipCode1 = 8990, City = "Fårup" },
                new ZipCode { ZipCode1 = 9000, City = "Aalborg" },
                new ZipCode { ZipCode1 = 9200, City = "Aalborg SV" },
                new ZipCode { ZipCode1 = 9210, City = "Aalborg SØ" },
                new ZipCode { ZipCode1 = 9220, City = "Aalborg Øst" },
                new ZipCode { ZipCode1 = 9230, City = "Svenstrup J" },
                new ZipCode { ZipCode1 = 9240, City = "Nibe" },
                new ZipCode { ZipCode1 = 9260, City = "Gistrup" },
                new ZipCode { ZipCode1 = 9270, City = "Klarup" },
                new ZipCode { ZipCode1 = 9280, City = "Storvorde" },
                new ZipCode { ZipCode1 = 9293, City = "Kongerslev" },
                new ZipCode { ZipCode1 = 9300, City = "Sæby" },
                new ZipCode { ZipCode1 = 9310, City = "Vodskov" },
                new ZipCode { ZipCode1 = 9320, City = "Hjallerup" },
                new ZipCode { ZipCode1 = 9330, City = "Dronninglund" },
                new ZipCode { ZipCode1 = 9340, City = "Asaa" },
                new ZipCode { ZipCode1 = 9352, City = "Dybvad" },
                new ZipCode { ZipCode1 = 9362, City = "Gandrup" },
                new ZipCode { ZipCode1 = 9370, City = "Hals" },
                new ZipCode { ZipCode1 = 9380, City = "Vestbjerg" },
                new ZipCode { ZipCode1 = 9381, City = "Sulsted" },
                new ZipCode { ZipCode1 = 9382, City = "Tylstrup" },
                new ZipCode { ZipCode1 = 9400, City = "Nørresundby" },
                new ZipCode { ZipCode1 = 9430, City = "Vadum" },
                new ZipCode { ZipCode1 = 9440, City = "Aabybro" },
                new ZipCode { ZipCode1 = 9460, City = "Brovst" },
                new ZipCode { ZipCode1 = 9480, City = "Løkken" },
                new ZipCode { ZipCode1 = 9490, City = "Pandrup" },
                new ZipCode { ZipCode1 = 9492, City = "Blokhus" },
                new ZipCode { ZipCode1 = 9493, City = "Saltum" },
                new ZipCode { ZipCode1 = 9500, City = "Hobro" },
                new ZipCode { ZipCode1 = 9510, City = "Arden" },
                new ZipCode { ZipCode1 = 9520, City = "Skørping" },
                new ZipCode { ZipCode1 = 9530, City = "Støvring" },
                new ZipCode { ZipCode1 = 9541, City = "Suldrup" },
                new ZipCode { ZipCode1 = 9550, City = "Mariager" },
                new ZipCode { ZipCode1 = 9560, City = "Hadsund" },
                new ZipCode { ZipCode1 = 9574, City = "Bælum" },
                new ZipCode { ZipCode1 = 9575, City = "Terndrup" },
                new ZipCode { ZipCode1 = 9600, City = "Aars" },
                new ZipCode { ZipCode1 = 9610, City = "Nørager" },
                new ZipCode { ZipCode1 = 9620, City = "Aalestrup" },
                new ZipCode { ZipCode1 = 9631, City = "Gedsted" },
                new ZipCode { ZipCode1 = 9632, City = "Møldrup" },
                new ZipCode { ZipCode1 = 9640, City = "Farsø" },
                new ZipCode { ZipCode1 = 9670, City = "Løgstør" },
                new ZipCode { ZipCode1 = 9681, City = "Ranum" },
                new ZipCode { ZipCode1 = 9690, City = "Fjerritslev" },
                new ZipCode { ZipCode1 = 9700, City = "Brønderslev" },
                new ZipCode { ZipCode1 = 9740, City = "Jerslev J" },
                new ZipCode { ZipCode1 = 9750, City = "Østervrå" },
                new ZipCode { ZipCode1 = 9760, City = "Vrå" },
                new ZipCode { ZipCode1 = 9800, City = "Hjørring" },
                new ZipCode { ZipCode1 = 9830, City = "Tårs" },
                new ZipCode { ZipCode1 = 9850, City = "Hirtshals" },
                new ZipCode { ZipCode1 = 9870, City = "Sindal" },
                new ZipCode { ZipCode1 = 9881, City = "Bindslev" },
                new ZipCode { ZipCode1 = 9900, City = "Frederikshavn" },
                new ZipCode { ZipCode1 = 9940, City = "Læsø" },
                new ZipCode { ZipCode1 = 9970, City = "Strandby" },
                new ZipCode { ZipCode1 = 9981, City = "Jerup" },
                new ZipCode { ZipCode1 = 9982, City = "Ålbæk" },
                new ZipCode { ZipCode1 = 9990, City = "Skagen" },
            };
            ZipCodes.AddRange(zipCodes);
            SaveChanges();
        }
    }

    public void SeedAdminSettings()
    {
        if (!AdminSettings.Any())
        {
            var adminSetting = new List<AdminSetting>
            {
                new AdminSetting { Name = "Må skifte Adresse", Enabled = true },
                new AdminSetting { Name = "Må skifte Email", Enabled = true },
                new AdminSetting { Name = "Må skifte Tlf", Enabled = true },
                new AdminSetting { Name = "Må skifte Kode", Enabled = true },
                new AdminSetting { Name = "Må skifte Fornavn", Enabled = true },
                new AdminSetting { Name = "Må skifte Efternavn", Enabled = true },
                new AdminSetting { Name = "Må skifte Username", Enabled = true },
            };
            AdminSettings.AddRange(adminSetting);
            SaveChanges();
        }
    }
}
