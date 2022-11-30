namespace DecisionMakerApi.Features.ConditionalDecision.Domains;

public class ConditionalInput : Condition
{
    public Boolean Value { get; set; }
    public long ForeignId { get; set; }
}