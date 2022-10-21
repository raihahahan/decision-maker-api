namespace DecisionMakerApi.Features.WeightedDecision.Domains;

public class WeightedInput
{
    private List<CriteriaInput> _criteriaInput = new List<CriteriaInput>();
    public long Id { get; set; }
    public long ChoiceId { get; set; }
    public string ChoiceName { get; set; } = null!;
    public List<CriteriaInput> _CriteriaInput
    {
        get { return _criteriaInput; }
        set{ _criteriaInput = value; }
    }
}