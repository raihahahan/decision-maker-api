using DecisionMakerApi.Features.WeightedDecision.Domains;

public class WeightedInputItem
{
    public long Id { get; set; }
    public long WeightedItemId { get; set; }
    public List<WeightedInput> WeightedInputs { get; set; } = null!;
}