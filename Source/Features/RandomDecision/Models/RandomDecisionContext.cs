using Microsoft.EntityFrameworkCore;
using DecisionMakerApi.Common.Domains;
using System.Diagnostics.CodeAnalysis; 

namespace DecisionMakerApi.Features.RandomDecision.Models;

public class RandomDecisionContext : DbContext 
{ 
    public RandomDecisionContext(DbContextOptions<RandomDecisionContext> options) : base(options) { } 
    public DbSet<RandomDecisionItem> RandomDecisionItems { get; set; } = null!; 
    public DbSet<Choice> Choices { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RandomDecisionItem>()
            .HasMany(d => d.Choices)
            .WithOne();
    }

} 
