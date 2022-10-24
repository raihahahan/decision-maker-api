using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DecisionMakerApi.Features.WeightedDecision.Models;
using DecisionMakerApi.Features.WeightedDecision.Domains;
using DecisionMakerApi.Common.Domains;
using DecisionMakerApi.Common.Services;

namespace DecisionMakerApi.Source.Features.WeightedDecision.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeightedDecisionItemsController : ControllerBase
    {
        private readonly WeightedDecisionContext _context;

        public WeightedDecisionItemsController(WeightedDecisionContext context)
        {
            _context = context;
        }

        // GET: api/WeightedDecisionItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeightedDecisionItem>>> GetWeightedDecisionItems(string? sortorder, int? pageNumber)
        {
      
            if (_context.WeightedDecisionItems == null)
            {
                return NotFound();
            }
            int pageSize = 3;
          
            var decisions =  _context.WeightedDecisionItems
                                .Include(ti => ti.Choices)
                                .Include(ti => ti.CriteriaList);
    
            switch (sortorder)
            {
                case "name_desc":
                    return await PaginatedList<WeightedDecisionItem>
                            .CreateAsync(decisions
                                            .OrderByDescending(s => s.Name), pageNumber ?? 1, pageSize);
                case "Date":
                    return await PaginatedList<WeightedDecisionItem>
                            .CreateAsync(decisions
                                            .OrderBy(s => s.CreatedAt), pageNumber ?? 1, pageSize);
                case "date_desc":
                    return await PaginatedList<WeightedDecisionItem>
                            .CreateAsync(decisions
                                            .OrderByDescending(s => s.CreatedAt), pageNumber ?? 1, pageSize);
                default:
                    return await PaginatedList<WeightedDecisionItem>
                            .CreateAsync(decisions
                                            .OrderBy(s => s.Name), pageNumber ?? 1, pageSize);               
            }
        }

        // GET: api/WeightedDecisionItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WeightedDecisionItem>> GetWeightedDecisionItem(long id)
        {
          if (_context.WeightedDecisionItems == null)
          {
              return NotFound();
          }
            var weightedDecisionItems = await _context.WeightedDecisionItems.Include(ti => ti.Choices).Include(ti => ti.CriteriaList).ToListAsync();
            
            var weightedDecisionItem = weightedDecisionItems.Find(i => i.Id == id);
            
            if (weightedDecisionItem == null)
            {
                return NotFound();
            }

            return weightedDecisionItem;
        }

        // POST: api/WeightedDecisionItems/5/decide
        [HttpPost("{id}/decide")]
        public async Task<ActionResult<List<WeightedResult>>> GetMakeWeightedDecision(long id, [FromBody] List<WeightedInput> _weightedInput)
        {

            var weightedDecisionItem = await this.GetWeightedDecisionItem(id);

            if (weightedDecisionItem == null || weightedDecisionItem.Value == null) return NotFound();
            
            List<Choice> choiceList = weightedDecisionItem.Value.Choices;
            if (choiceList.Count == 0) return NotFound();  

            List<WeightedResult> FinalList = _weightedInput.Select((w, index) => {
                double totalWeight = 0;
                w._CriteriaInput.ForEach(c => {
                    totalWeight += c.value * c.Weight;
                });
                return new WeightedResult(index, w.ChoiceId, totalWeight, w.ChoiceName);
            }).OrderByDescending(o => o.TotalWeight).ToList();

            return FinalList;
        }

        // PUT: api/WeightedDecisionItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeightedDecisionItem(long id, WeightedDecisionItem weightedDecisionItem)
        {
            if (id != weightedDecisionItem.Id)
            {
                return BadRequest();
            }
            weightedDecisionItem.UpdatedAt = new DateTime();
            _context.Entry(weightedDecisionItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeightedDecisionItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/WeightedDecisionItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WeightedDecisionItem>> PostWeightedDecisionItem(WeightedDecisionItem weightedDecisionItem)
        {
          if (_context.WeightedDecisionItems == null)
          {
              return Problem("Entity set 'WeightedDecisionContext.WeightedDecisionItems'  is null.");
          }
            _context.WeightedDecisionItems.Add(weightedDecisionItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWeightedDecisionItem), new { id = weightedDecisionItem.Id }, weightedDecisionItem);
        }

        // DELETE: api/WeightedDecisionItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeightedDecisionItem(long id)
        {
            if (_context.WeightedDecisionItems == null)
            {
                return NotFound();
            }
            var weightedDecisionItem = await _context.WeightedDecisionItems.FindAsync(id);
            if (weightedDecisionItem == null)
            {
                return NotFound();
            }

            _context.WeightedDecisionItems.Remove(weightedDecisionItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WeightedDecisionItemExists(long id)
        {
            return (_context.WeightedDecisionItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
