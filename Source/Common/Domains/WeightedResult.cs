namespace DecisionMakerApi.Common.Domains;

public class WeightedResult
{
    private long id;
    private long choiceId;
    private double totalWeight;
    private string name;

    public WeightedResult(long id, long choiceId, double totalWeight, string name)
    {
        this.id = id;
        this.choiceId = choiceId;
        this.totalWeight = totalWeight;
        this.name = name;
    }

    public long Id 
    {
        get { return id; }
        set { id = value; }
    }
    public long ChoiceId
    {
        get { return choiceId; }
        set { choiceId = value; }
    }
    public double TotalWeight
    {
        get { return totalWeight; }
        set { totalWeight = value; }
    }
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
}