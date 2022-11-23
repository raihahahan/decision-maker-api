using DecisionMakerApi.Features.WeightedDecision.Domains;

public class CriteriaInput : Criteria
{
    public double value { get; set; }
    public long InputId { get; set; }
    public long CriteriaId { get; set; }
}