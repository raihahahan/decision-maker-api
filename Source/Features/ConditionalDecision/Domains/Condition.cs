namespace DecisionMakerApi.Features.ConditionalDecision.Domains;

public class Condition 
{
    private List<string> _include = new List<string>();
    private List<string> _exclude = new List<string>();

    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public List<string> Include 
    {
        get { return _include; }
        set { _include = value; }
    }
    public List<string> Exclude
    {
        get { return _exclude; }
        set { _exclude = value; }
    }
}