using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RoleManagementApi.Models;

namespace RoleManagementApi.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ✅ Existing DbSets
        public virtual DbSet<AppRole> AppRoles { get; set; }
        public virtual DbSet<Facility> Facilities { get; set; }
        public virtual DbSet<LeftMenu> LeftMenus { get; set; }
        public virtual DbSet<Organisation> Organisations { get; set; }

        // ✅ Newly added DbSets
        public virtual DbSet<Link> Links { get; set; }
        public virtual DbSet<LinkCheck> LinkChecks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, move it out of source code. You can use the Name= syntax to read it from configuration.
            => optionsBuilder.UseSqlServer("Server=SSSLBG4NB862\\SQLEXPRESS01;Database=Pharmacy_Management;Trusted_Connection=True;TrustServerCertificate=True");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // --- AppRole ---
            modelBuilder.Entity<AppRole>(entity =>
            {
                entity.HasKey(e => e.RoleId).HasName("PK__AppRoles__8AFACE1A913EE4FF");

                entity.HasIndex(e => e.RoleName, "UQ__AppRoles__8A2B616043F5EF1A").IsUnique();

                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.RoleDescription).HasMaxLength(255);
                entity.Property(e => e.RoleName).HasMaxLength(100);
            });

            // --- Facility ---
            modelBuilder.Entity<Facility>(entity =>
            {
                entity.HasKey(e => e.FacilityId).HasName("PK__facility__B2E8EAAE6C0B66D7");

                entity.ToTable("facility");

                entity.Property(e => e.FacilityId).HasColumnName("facility_id");
                entity.Property(e => e.FacilityExtra)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("facility_extra");
                entity.Property(e => e.FacilityName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("facility_name");
                entity.Property(e => e.FacilityUnitNumber).HasColumnName("facility_unit_number");
                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("image");
                entity.Property(e => e.IsSingleEntity).HasColumnName("is_single_entity");
                entity.Property(e => e.OrganisationId).HasColumnName("organisation_id");
                entity.Property(e => e.ParentFacilityId).HasColumnName("parent_facility_id");
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("status");
                entity.Property(e => e.UnitNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("unit_number");

                entity.HasOne(d => d.Organisation).WithMany(p => p.Facilities)
                    .HasForeignKey(d => d.OrganisationId)
                    .HasConstraintName("fk_facility_org");

                entity.HasOne(d => d.ParentFacility).WithMany(p => p.InverseParentFacility)
                    .HasForeignKey(d => d.ParentFacilityId)
                    .HasConstraintName("fk_facility_parent");
            });

            // --- LeftMenu ---
            modelBuilder.Entity<LeftMenu>(entity =>
            {
                entity.HasKey(e => e.MenuId);

                entity.ToTable("LeftMenu");

                entity.Property(e => e.MenuId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.MenuName).HasMaxLength(150);
                entity.Property(e => e.NavigationLink).HasMaxLength(500);

                entity.HasOne(d => d.ParentMenu).WithMany(p => p.InverseParentMenu)
                    .HasForeignKey(d => d.ParentMenuId)
                    .HasConstraintName("FK_LeftMenu_Parent");

                entity.HasOne(d => d.Role).WithMany(p => p.LeftMenus)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LeftMenu_Role");
            });

            // --- Organisation ---
            modelBuilder.Entity<Organisation>(entity =>
            {
                entity.HasKey(e => e.OrganisationId).HasName("PK__organisa__6CB3A4F2638CF2A1");

                entity.ToTable("organisation");

                entity.Property(e => e.OrganisationId).HasColumnName("organisation_id");
                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("city");
                entity.Property(e => e.Logo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("logo");
                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");
                entity.Property(e => e.State)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("state");
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("status");
            });

            // --- Links ---
            modelBuilder.Entity<Link>(entity =>
            {
                entity.HasKey(e => e.LinkId).HasName("PK__Links__2D12213550F4F230");

                entity.Property(e => e.ContentType).HasMaxLength(100);
                entity.Property(e => e.FileName).HasMaxLength(255);
                entity.Property(e => e.FilePath).HasMaxLength(500);
                entity.Property(e => e.LastCheckedAt).HasColumnType("datetime");
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("Unknown");
                entity.Property(e => e.Type).HasMaxLength(50);
                entity.Property(e => e.Url).HasMaxLength(500);
            });

            // --- LinkChecks ---
            modelBuilder.Entity<LinkCheck>(entity =>
            {
                entity.HasKey(e => e.LinkCheckId).HasName("PK__LinkChec__571DF602FC56F107");

                entity.Property(e => e.CheckedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Message).HasMaxLength(500);
                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.Link).WithMany(p => p.LinkChecks)
                    .HasForeignKey(d => d.LinkId)
                    .HasConstraintName("FK_LinkChecks_Links");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
