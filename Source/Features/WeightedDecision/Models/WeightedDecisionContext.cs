using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using DecisionMakerApi.Common.Domains;
using DecisionMakerApi.Features.WeightedDecision.Domains;

namespace DecisionMakerApi.Features.WeightedDecision.Models;

public class WeightedDecisionContext : DbContext
{
    public WeightedDecisionContext(DbContextOptions<WeightedDecisionContext> options) : base(options) { }

    public DbSet<WeightedDecisionItem> WeightedDecisionItems { get; set; } = null!;
    public DbSet<Choice> Choices { get; set; } = null!;
    public DbSet<Criteria> Criterias { get; set; } = null!;
    public DbSet<WeightedInputItem> WeightedInputItems { get; set; } = null!;
    public DbSet<WeightedInput> WeightedInputs { get; set; } = null!;
    public DbSet<CriteriaInput> CriteriaInputs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeightedDecisionItem>()
            .HasMany(d => d.Choices)
            .WithOne()
            .HasForeignKey(i => i.DecisionId);

        modelBuilder.Entity<WeightedDecisionItem>()
            .HasMany(d => d.CriteriaList)
            .WithOne()
            .HasForeignKey(i => i.DecisionId);

        modelBuilder.Entity<WeightedInputItem>()
            .HasMany(i => i.WeightedInputs)
            .WithOne()
            .HasForeignKey(d => d.ForeignId);

        modelBuilder.Entity<WeightedInput>()
            .HasMany(i => i.CriteriaInput)
            .WithOne()
            .HasForeignKey(i => i.InputId);
    }
}
