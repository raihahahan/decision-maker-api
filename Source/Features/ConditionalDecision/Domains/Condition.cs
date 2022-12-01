namespace DecisionMakerApi.Features.ConditionalDecision.Domains;

public class Condition 
{
    private List<IncludeItem> _include = new List<IncludeItem>();
    private List<ExcludeItem> _exclude = new List<ExcludeItem>();

    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public List<IncludeItem> Include 
    {
        get { return _include; }
        set { _include = value; }
    }
    public List<ExcludeItem> Exclude
    {
        get { return _exclude; }
        set { _exclude = value; }
    }

    public long DecisionId { get; set; }
}