using DecisionMakerApi.Common.Domains;
using DecisionMakerApi.Features.ConditionalDecision.Domains;

namespace DecisionMakerApi.Features.ConditionalDecision.Models;
public class ConditionalDecisionItem : Decision
{
    private List<Condition> _conditions = new List<Condition>();
    public List<Condition> Conditions 
    { 
        get { return _conditions; }  
        set { _conditions = value; } 
    }
}