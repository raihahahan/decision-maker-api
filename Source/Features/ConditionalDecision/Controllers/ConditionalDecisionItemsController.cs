using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DecisionMakerApi.Features.ConditionalDecision.Models;
using DecisionMakerApi.Common.Domains;
using DecisionMakerApi.Features.ConditionalDecision.Domains;
using DecisionMakerApi.Common.Services;

namespace DecisionMakerApi.Source.Features.ConditionalDecision.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConditionalDecisionItemsController : ControllerBase
    {
        private readonly ConditionalDecisionContext _context;

        public ConditionalDecisionItemsController(ConditionalDecisionContext context)
        {
            _context = context;
        }

        // GET: api/ConditionalDecisionItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConditionalDecisionItem>>> GetConditionalDecisionItems(string? sortorder, int? pageNumber)
        {
          if (_context.ConditionalDecisionItems == null)
          {
              return NotFound();
          }
          int pageSize = 3;
          var decisions =  _context.ConditionalDecisionItems
                        .Include(ti => ti.Choices)
                        .Include(ti => ti.Conditions)
                        .ThenInclude(thenTi => thenTi.Include)
                        .Include(ti => ti.Conditions)
                        .ThenInclude(thenTi => thenTi.Exclude);

          switch (sortorder)
            {
                case "name_desc":
                    return await PaginatedList<ConditionalDecisionItem>
                            .CreateAsync(decisions
                                            .OrderByDescending(s => s.Name), pageNumber ?? 1, pageSize);
                case "Date":
                    return await PaginatedList<ConditionalDecisionItem>
                            .CreateAsync(decisions
                                            .OrderBy(s => s.CreatedAt), pageNumber ?? 1, pageSize);
                case "date_desc":
                    return await PaginatedList<ConditionalDecisionItem>
                            .CreateAsync(decisions
                                            .OrderByDescending(s => s.CreatedAt), pageNumber ?? 1, pageSize);
                default:
                    return await PaginatedList<ConditionalDecisionItem>
                            .CreateAsync(decisions
                                            .OrderBy(s => s.Name), pageNumber ?? 1, pageSize);               
            }
        }

        // GET: api/ConditionalDecisionItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConditionalDecisionItem>> GetConditionalDecisionItem(long id)
        {
          if (_context.ConditionalDecisionItems == null)
          {
              return NotFound();
          }
            var conditionalDecisionItems = await _context.ConditionalDecisionItems.Include(ti => ti.Choices).Include(ti => ti.Conditions).ToListAsync();

            var conditionalDecisionItem = conditionalDecisionItems.Find(i => i.Id == id);

            if (conditionalDecisionItem == null)
            {
                return NotFound();
            }

            return conditionalDecisionItem;
        }

        // PUT: api/ConditionalDecisionItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConditionalDecisionItem(long id, ConditionalDecisionItem conditionalDecisionItem)
        {
            if (id != conditionalDecisionItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(conditionalDecisionItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConditionalDecisionItemExists(id))
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

        // POST: api/ConditionalDecisionItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ConditionalDecisionItem>> PostConditionalDecisionItem(ConditionalDecisionItem conditionalDecisionItem)
        {
          if (_context.ConditionalDecisionItems == null)
          {
              return Problem("Entity set 'ConditionalDecisionContext.ConditionalDecisionItems'  is null.");
          }
            _context.ConditionalDecisionItems.Add(conditionalDecisionItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConditionalDecisionItem), new { id = conditionalDecisionItem.Id }, conditionalDecisionItem);
        }

        // DELETE: api/ConditionalDecisionItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConditionalDecisionItem(long id)
        {
            if (_context.ConditionalDecisionItems == null)
            {
                return NotFound();
            }
            var conditionalDecisionItem = await _context.ConditionalDecisionItems.FindAsync(id);
            if (conditionalDecisionItem == null)
            {
                return NotFound();
            }

            _context.ConditionalDecisionItems.Remove(conditionalDecisionItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/ConditionalDecisionItems/5/decide
        [HttpPost("{id}/decide")]
        public async Task<ActionResult<List<WeightedResult>>> GetMakeConditionalDecision(long id, [FromBody] List<ConditionalInput> conditionalInput)
        {
            var conditionalDecisionItem = await this.GetConditionalDecisionItem(id);

            if (conditionalDecisionItem == null || conditionalDecisionItem.Value == null || conditionalInput == null) return NotFound();

            Dictionary<long, int> PointDict = new Dictionary<long, int>();

            conditionalDecisionItem.Value.Choices.ForEach(choice => {
                PointDict.Add(choice.Id, 0);
            });

            conditionalInput.ForEach(c => {
                if (c.Value)
                {
                    c.Include.ForEach(inc => PointDict[inc.ChoiceId]++);
                    c.Exclude.ForEach(exc => PointDict[exc.ChoiceId]--);
                }
            });

            List<WeightedResult> FinalResult = new List<WeightedResult>();
            int idIndex = 0;
            foreach(KeyValuePair<long, int> entry in PointDict) 
            {
                Choice? EntryChoice = conditionalDecisionItem.Value.Choices.Find(choice => choice.Id == entry.Key);
                if (EntryChoice == null) return NotFound();
                FinalResult.Add(new WeightedResult(idIndex, EntryChoice.Id, entry.Value, EntryChoice.Name));
            }
            FinalResult = FinalResult.OrderByDescending(o => o.TotalWeight).ToList();

            return FinalResult;
        }

        private bool ConditionalDecisionItemExists(long id)
        {
            return (_context.ConditionalDecisionItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
