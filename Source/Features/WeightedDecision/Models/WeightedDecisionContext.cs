using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis; 

namespace DecisionMakerApi.Features.WeightedDecision.Models;

public class WeightedDecisionContext : DbContext 
{ 
    public WeightedDecisionContext(DbContextOptions<WeightedDecisionContext> options) : base(options) { } 
    public DbSet<WeightedDecisionItem> WeightedDecisionItems { get; set; } = null!; 
} 
