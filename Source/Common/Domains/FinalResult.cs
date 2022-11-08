namespace DecisionMakerApi.Common.Domains;

public class FinalResult 
{
    private List<WeightedResult> _weightedResults = new List<WeightedResult>();
    private string _decisionName = "";

    public FinalResult(List<WeightedResult> weightedResults, string decisionName) 
    {
        this._weightedResults = weightedResults;
        this._decisionName = decisionName;
    }

    public List<WeightedResult> WeightedResults
    {
        get { return this._weightedResults; }
        set { this._weightedResults = value; }
    }

    public string DecisionName
    {
        get { return this._decisionName; }
        set { this._decisionName = value; }
    }

}