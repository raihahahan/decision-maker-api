using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis; 

namespace DecisionMakerApi.Features.RandomDecision.Models;

public class RandomDecisionContext : DbContext 
{ 
    public RandomDecisionContext(DbContextOptions<RandomDecisionContext> options) : base(options) { } 
    public DbSet<RandomDecisionItem> RandomDecisionItems { get; set; } = null!; 
} 
