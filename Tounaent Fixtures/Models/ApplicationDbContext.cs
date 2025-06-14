﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Tounaent_Fixtures.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Gender> Gender { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<TblTournament> TblTournament { get; set; }

    public virtual DbSet<TblTournamentUserReg> TblTournamentUserRegs { get; set; }
    public virtual DbSet<TblCategory> TblCategory { get; set; }

    public virtual DbSet<TblWeightCategory> TblWeightCategory { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:tournamentfixtures.database.windows.net,1433;Initial Catalog=TournamentFixutres;Persist Security Info=False;User ID=adminnew;Password=TestPassword@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblWeightCategory>(entity =>
        {
            entity.HasKey(e => e.WeightCatId);
            entity.Property(e => e.WeightCatId).HasColumnName("Weight_Cat_id");
            entity.Property(e => e.WeightCatName).HasColumnName("Weight_Cat_Name");
            entity.Property(e => e.CatId).HasColumnName("Cat_id");
            // Map others similarly if needed
        });
        modelBuilder.Entity<TblCategory>(entity =>
        {
            entity.HasKey(e => e.CatId);

            entity.ToTable("Tbl_Category"); // match your table name exactly

            entity.Property(e => e.CatId).HasColumnName("Cat_id");
            entity.Property(e => e.CategoryName).HasColumnName("Category_Name");
            entity.Property(e => e.GenId).HasColumnName("Gen_id");
            entity.Property(e => e.IsActive).HasColumnName("IsActive");
            entity.Property(e => e.AddedDt).HasColumnName("Added_dt");
            entity.Property(e => e.AddedBy).HasColumnName("Added_by");
            entity.Property(e => e.ModifyDt).HasColumnName("Modify_dt");
            entity.Property(e => e.ModifyBy).HasColumnName("Modify_by");
        });
        modelBuilder.Entity<TblTournamentUserReg>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Tbl_Tournament_User_Reg");

            entity.Property(e => e.AddedBy)
                .HasMaxLength(50)
                .HasColumnName("Added_by");
            entity.Property(e => e.AddedDt)
                .HasColumnType("datetime")
                .HasColumnName("Added_dt");
            entity.Property(e => e.AdharNumb).HasMaxLength(500);
            entity.Property(e => e.CatId).HasColumnName("Cat_id");
            entity.Property(e => e.ClubName).HasMaxLength(500);
            entity.Property(e => e.DistictId).HasColumnName("Distict_id");
            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FatherName)
                .HasMaxLength(150)
                .HasColumnName("Father_Name");
            entity.Property(e => e.MobileNo).HasMaxLength(150);
            entity.Property(e => e.ModifyBy)
                .HasMaxLength(50)
                .HasColumnName("Modify_by");
            entity.Property(e => e.ModifyDt)
                .HasColumnType("datetime")
                .HasColumnName("Modify_dt");
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.TrId).HasColumnName("Tr_id");
            entity.Property(e => e.TrUserId)
                .ValueGeneratedOnAdd()
                .HasColumnName("Tr_User_id");
            entity.Property(e => e.UserId).HasColumnName("User_id");
            entity.Property(e => e.WeightCatId).HasColumnName("Weight_Cat_id");
        });
        modelBuilder.Entity<TblTournament>(entity =>
        {
            entity.ToTable("Tbl_Tournament");

            entity.HasKey(e => e.TournamentId);
            entity.Property(e => e.TournamentId).ValueGeneratedOnAdd()
            .HasColumnName("Tournament_Id");
            entity.Property(e => e.FromDt)
              .HasColumnName("From_dt");
            entity.Property(e => e.AddedBy)
              .HasColumnName("Added_by");
            entity.Property(e => e.AddedDt)
              .HasColumnName("Added_dt");
            entity.Property(e => e.ModifyBy)
              .HasColumnName("Modify_by");
            entity.Property(e => e.ModifyDt)
              .HasColumnName("Modify_dt");
            entity.Property(e => e.ToDt)
              .HasColumnName("To_dt");
        });
        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.GenderId).HasName("PK__Gender__4E24E9F701341065");

            entity.ToTable("Gender");

            entity.Property(e => e.AddedBy)
                .HasMaxLength(50)
                .HasColumnName("Added_by");
            entity.Property(e => e.AddedDt)
                .HasColumnType("datetime")
                .HasColumnName("Added_dt");
            entity.Property(e => e.GenderName).HasMaxLength(50);
            entity.Property(e => e.ModifyBy)
                .HasMaxLength(50)
                .HasColumnName("Modify_by");
            entity.Property(e => e.ModifyDt)
                .HasColumnType("datetime")
                .HasColumnName("Modify_dt");
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("PK__Registra__6EF58810C0A6896F");

            entity.ToTable("Registration");

            entity.Property(e => e.Aadhaar).HasMaxLength(20);
            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Height).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.PinCode).HasMaxLength(10);
            entity.Property(e => e.Weight).HasMaxLength(20);

            entity.HasOne(d => d.Gender).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.GenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__Gende__5FB337D6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
