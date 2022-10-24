namespace DecisionMakerApi.Common.Domains;

public abstract class Decision
{
    private List<Choice> _choices = new List<Choice>();
    private DateTime updatedAt = new DateTime();
    private DateTime createdAt = new DateTime();

    public long Id { get; set; }
    public string Name { get; set; } = null!;

    public DateTime CreatedAt 
    { 
        get { return createdAt; }
        set { createdAt = value; }
    }

    public DateTime UpdatedAt
    {
        get { return updatedAt; }
        set { updatedAt = value; }
    }

    public List<Choice> Choices
    {
        get { return _choices; }
        set { _choices = value; }
    }
}