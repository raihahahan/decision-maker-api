using DecisionMakerApi.Features.ConditionalDecision.Domains;

public class ConditionalInputItem
{
    public long Id { get; set; }
    public List<ConditionalInput> ConditionalInputs { get; set; } = null!;
    public long ConditionalItemId { get; set; }
}