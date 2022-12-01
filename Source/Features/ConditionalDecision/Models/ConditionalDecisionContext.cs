using Microsoft.EntityFrameworkCore;
using DecisionMakerApi.Common.Domains;
using System.Diagnostics.CodeAnalysis; 
using DecisionMakerApi.Features.ConditionalDecision.Domains;
using DecisionMakerApi.Features.ConditionalDecision.Models;

namespace DecisionMakerApi.Features.ConditionalDecision.Models;

public class ConditionalDecisionContext : DbContext 
{ 
    public ConditionalDecisionContext(DbContextOptions<ConditionalDecisionContext> options) : base(options) { } 
    public DbSet<ConditionalDecisionItem> ConditionalDecisionItems { get; set; } = null!; 

    public DbSet<Condition> Conditions { get; set; } = null!;
    public DbSet<IncludeItem> Include { get; set; } = null!;
    public DbSet<ExcludeItem> Exclude { get; set; } = null!;

    public DbSet<Choice> Choices { get; set; } = null!;

    public DbSet<ConditionalInputItem> ConditionalInputItems { get; set; } = null!;
    public DbSet<ConditionalInput> ConditionalInputs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    {
        modelBuilder.Entity<ConditionalDecisionItem>()
            .HasMany(d => d.Choices)
            .WithOne()
            .HasForeignKey(i => i.DecisionId);

        modelBuilder.Entity<ConditionalDecisionItem>()
            .HasMany(d => d.Conditions)
            .WithOne()
            .HasForeignKey(i => i.DecisionId);

        modelBuilder.Entity<Condition>()
            .HasMany(d => d.Include)
            .WithOne()
            .HasForeignKey(i => i.ConditionId);
        
        modelBuilder.Entity<Condition>()
            .HasMany(d => d.Exclude)
            .WithOne()
            .HasForeignKey(i => i.ConditionId);

        // Input

        modelBuilder.Entity<ConditionalInputItem>()
            .HasMany(d => d.ConditionalInputs)
            .WithOne()
            .HasForeignKey(i => i.ForeignId);

        modelBuilder.Entity<ConditionalInput>()
            .HasMany(d => d.Include)
            .WithOne()
            .HasForeignKey(i => i.ConditionIdInclude);
        
        modelBuilder.Entity<ConditionalInput>()
            .HasMany(d => d.Exclude)
            .WithOne()
            .HasForeignKey(i => i.ConditionIdExclude);
    }
} 
