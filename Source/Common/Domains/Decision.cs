namespace DecisionMakerApi.Common.Domains;

public abstract class Decision
{
    private List<Choice> _choices = new List<Choice>();

    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public List<Choice> Choices
    {
        get { return _choices; }
        set { _choices = value; }
    }
}