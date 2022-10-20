using DecisionMakerApi.Common.Domains;
using DecisionMakerApi.Features.WeightedDecision.Domains;

namespace DecisionMakerApi.Features.WeightedDecision.Models;
public class WeightedDecisionItem : Decision
{
    private List<Criteria> _criteriaList = new List<Criteria>();

    public List<Criteria> CriteriaList
    {
        get { return _criteriaList; }
        set { _criteriaList = value; }
    }
}