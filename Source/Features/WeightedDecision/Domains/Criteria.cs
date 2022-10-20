namespace DecisionMakerApi.Features.WeightedDecision.Domains;

public class Criteria 
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public double Weight { get; set; }
}