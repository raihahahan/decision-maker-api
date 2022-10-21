namespace DecisionMakerApi.Common.Domains;

public class Choice
{
    private long id;
    private string _Name;
    public Choice(long Id, string Name) 
    {
        id = Id;
        _Name = Name;
    }
    public long Id 
    { 
        get { return id; }
        set { id = value; }
    }
    public string Name 
    {
        get { return _Name; }
        set{ _Name = value; }
    }
}