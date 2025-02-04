﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Areas.Drugs.Models;

public partial class ClinicSysContext : DbContext
{
    public ClinicSysContext(DbContextOptions<ClinicSysContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PharmacyHealthInformation> PharmacyHealthInformation { get; set; }

    public virtual DbSet<PharmacyTClinicalUseDetails> PharmacyTClinicalUseDetails { get; set; }

    public virtual DbSet<PharmacyTClinicalUseList> PharmacyTClinicalUseList { get; set; }

    public virtual DbSet<PharmacyTMedicinesList> PharmacyTMedicinesList { get; set; }

    public virtual DbSet<PharmacyTSideEffectDetails> PharmacyTSideEffectDetails { get; set; }

    public virtual DbSet<PharmacyTSideEffectList> PharmacyTSideEffectList { get; set; }

    public virtual DbSet<PharmacyTTypeDetails> PharmacyTTypeDetails { get; set; }

    public virtual DbSet<PharmacyTTypeList> PharmacyTTypeList { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PharmacyHealthInformation>(entity =>
        {
            entity.HasOne(d => d.FIdDrugNavigation).WithMany(p => p.PharmacyHealthInformation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pharmacy_HealthInformation_Pharmacy_tMedicinesList");
        });

        modelBuilder.Entity<PharmacyTClinicalUseDetails>(entity =>
        {
            entity.HasOne(d => d.FIdClicicalUseNavigation).WithMany(p => p.PharmacyTClinicalUseDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_藥師系統_t適應證明細表_藥師系統_t適應症總表");

            entity.HasOne(d => d.FIdDrugNavigation).WithMany(p => p.PharmacyTClinicalUseDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_藥師系統_t適應證明細表_藥師系統_t藥品基本資料");
        });

        modelBuilder.Entity<PharmacyTClinicalUseList>(entity =>
        {
            entity.HasKey(e => e.FIdClinicalUse).HasName("PK_藥師系統_t適應症總表");
        });

        modelBuilder.Entity<PharmacyTMedicinesList>(entity =>
        {
            entity.HasKey(e => e.FIdDrug).HasName("PK_藥師系統_t藥品基本資料");

            entity.Property(e => e.FDosage).IsFixedLength();
            entity.Property(e => e.FPregnancyCategory).IsFixedLength();
        });

        modelBuilder.Entity<PharmacyTSideEffectDetails>(entity =>
        {
            entity.HasOne(d => d.FIdDrugNavigation).WithMany(p => p.PharmacyTSideEffectDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_藥師系統_t副作用明細表_藥師系統_t藥品基本資料");

            entity.HasOne(d => d.FIdSideEffectNavigation).WithMany(p => p.PharmacyTSideEffectDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_藥師系統_t副作用明細表_藥師系統_t副作用總表");
        });

        modelBuilder.Entity<PharmacyTSideEffectList>(entity =>
        {
            entity.HasKey(e => e.FIdSideEffect).HasName("PK_藥師系統_t副作用總表");
        });

        modelBuilder.Entity<PharmacyTTypeDetails>(entity =>
        {
            entity.HasOne(d => d.FIdDrugNavigation).WithMany(p => p.PharmacyTTypeDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pharmacy_tTypeDetails_Pharmacy_tMedicinesList");

            entity.HasOne(d => d.FIdTpyeNavigation).WithMany(p => p.PharmacyTTypeDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pharmacy_tTypeDetails_Pharmacy_tTpyeList");
        });

        modelBuilder.Entity<PharmacyTTypeList>(entity =>
        {
            entity.HasKey(e => e.FIdType).HasName("PK_藥師系統_t劑型總表");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}