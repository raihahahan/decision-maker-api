using DecisionMakerApi.Common.Domains;

namespace DecisionMakerApi.Features.WeightedDecision.Models;
public class WeightedDecisionItem : Decision
{
    public double Weight { get; set; }
}