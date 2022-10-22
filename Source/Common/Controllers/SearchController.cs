using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DecisionMakerApi.Features.ConditionalDecision.Models;
using DecisionMakerApi.Features.RandomDecision.Models;
using DecisionMakerApi.Features.WeightedDecision.Models;
using DecisionMakerApi.Common.Domains;

namespace DecisionMakerApi.Common.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GlobalSearchController : ControllerBase
{
    private readonly RandomDecisionContext _randomCtx;
    private readonly WeightedDecisionContext _weightedCtx;
    private readonly ConditionalDecisionContext _conditionalCtx;

    public GlobalSearchController(RandomDecisionContext randomContext, WeightedDecisionContext weightedContext, ConditionalDecisionContext conditionalContext)
    {
        _randomCtx = randomContext;
        _weightedCtx = weightedContext;
        _conditionalCtx = conditionalContext;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Decision>> GlobalSearch(string searchString)
    {
        var randomDecisions = from r in _randomCtx.RandomDecisionItems select r;
        var weightedDecisions = from w in _weightedCtx.WeightedDecisionItems select w;
        var conditionalDecisions = from c in _conditionalCtx.ConditionalDecisionItems select c;
        
        if (randomDecisions == null || weightedDecisions == null || conditionalDecisions == null) return NotFound();
        
        if (!String.IsNullOrEmpty(searchString))
        {
            searchString = searchString.ToLower();

            randomDecisions = randomDecisions.Where(r => r.Name.ToLower()!.Contains(searchString));
            weightedDecisions = weightedDecisions.Where(w => w.Name.ToLower()!.Contains(searchString));
            conditionalDecisions = conditionalDecisions.Where(c => c.Name.ToLower()!.Contains(searchString));
        }
        var allDecisions = new List<Decision>(randomDecisions.Count() + weightedDecisions.Count() + conditionalDecisions.Count());

        allDecisions.AddRange(randomDecisions.Include(r => r.Choices));
        allDecisions.AddRange(weightedDecisions.Include(w => w.Choices));
        allDecisions.AddRange(conditionalDecisions.Include(c => c.Choices));

        return allDecisions;
    }
}
