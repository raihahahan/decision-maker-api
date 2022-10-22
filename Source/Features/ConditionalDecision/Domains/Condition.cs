namespace DecisionMakerApi.Features.ConditionalDecision.Domains;

public class Condition 
{
    public class InnerItem
    {
        // Id refers to Choices primary key
        public long Id { get; set; }
        public long ChoiceId { get; set; }
    }

    private List<InnerItem> _include = new List<InnerItem>();
    private List<InnerItem> _exclude = new List<InnerItem>();

    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public List<InnerItem> Include 
    {
        get { return _include; }
        set { _include = value; }
    }
    public List<InnerItem> Exclude
    {
        get { return _exclude; }
        set { _exclude = value; }
    }
}