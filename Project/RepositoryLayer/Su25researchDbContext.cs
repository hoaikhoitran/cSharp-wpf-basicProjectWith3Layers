using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace RepositoryLayer;

public partial class Su25researchDbContext : DbContext
{
    public Su25researchDbContext()
    {
    }

    public Su25researchDbContext(DbContextOptions<Su25researchDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ResearchProject> ResearchProjects { get; set; }

    public virtual DbSet<Researcher> Researchers { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(GetConnectionString());
    }

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
        var strConn = config["ConnectionStrings:DefaultConnection"];

        return strConn;
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ResearchProject>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__Research__761ABED0BE767124");

            entity.ToTable("ResearchProject");

            entity.Property(e => e.ProjectId)
                .ValueGeneratedNever()
                .HasColumnName("ProjectID");
            entity.Property(e => e.Budget).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LeadResearcherId).HasColumnName("LeadResearcherID");
            entity.Property(e => e.ProjectTitle).HasMaxLength(200);
            entity.Property(e => e.ResearchField).HasMaxLength(100);

            entity.HasOne(d => d.LeadResearcher).WithMany(p => p.ResearchProjects)
                .HasForeignKey(d => d.LeadResearcherId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ResearchP__LeadR__3E52440B");
        });

        modelBuilder.Entity<Researcher>(entity =>
        {
            entity.HasKey(e => e.ResearcherId).HasName("PK__Research__7CC06F054DC839C0");

            entity.ToTable("Researcher");

            entity.HasIndex(e => e.Email, "UQ__Research__A9D105349A20F225").IsUnique();

            entity.Property(e => e.ResearcherId)
                .ValueGeneratedNever()
                .HasColumnName("ResearcherID");
            entity.Property(e => e.Affiliation).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Expertise).HasMaxLength(200);
            entity.Property(e => e.FullName).HasMaxLength(100);
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserAcco__1788CCACDC0F8D41");

            entity.ToTable("UserAccount");

            entity.HasIndex(e => e.Email, "UQ__UserAcco__A9D10534D3A95FF2").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(60);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
