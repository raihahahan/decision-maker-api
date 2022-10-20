using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis; 

namespace DecisionMakerApi.Features.ConditionalDecision.Models;

public class ConditionalDecisionContext : DbContext 
{ 
    public ConditionalDecisionContext(DbContextOptions<ConditionalDecisionContext> options) : base(options) { } 
    public DbSet<ConditionalDecisionItem> ConditionalDecisionItems { get; set; } = null!; 
} 
