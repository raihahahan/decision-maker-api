using DecisionMakerApi.Common.Domains;
using DecisionMakerApi.Features.ConditionalDecision.Domains;

namespace DecisionMakerApi.Features.ConditionalDecision.Models;
public class ConditionalDecisionItem : Decision
{
    private Condition _conditions = new Condition();
    public Condition Conditions 
    { 
        get { return _conditions; }  
        set { _conditions = value; } 
    }
}