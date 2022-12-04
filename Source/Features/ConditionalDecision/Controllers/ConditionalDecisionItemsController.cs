using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DecisionMakerApi.Features.ConditionalDecision.Models;
using DecisionMakerApi.Common.Domains;
using DecisionMakerApi.Features.ConditionalDecision.Domains;
using DecisionMakerApi.Services.Pagination;

namespace DecisionMakerApi.Source.Features.ConditionalDecision.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConditionalDecisionItemsController : ControllerBase
    {
        private readonly ConditionalDecisionContext _context;
        private const int pageSize = 5;

        public ConditionalDecisionItemsController(ConditionalDecisionContext context)
        {
            _context = context;
        }

        // GET: api/ConditionalDecisionItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConditionalDecisionItem>>> GetConditionalDecisionItems(string? sortorder, int? pageNumber, string? q)
        {
          if (_context.ConditionalDecisionItems == null)
          {
              return NotFound();
          }

          var decisions =  _context.ConditionalDecisionItems
                        .Include(ti => ti.Choices)
                        .Include(ti => ti.Conditions)
                        .ThenInclude(thenTi => thenTi.Include)
                        .Include(ti => ti.Conditions)
                        .ThenInclude(thenTi => thenTi.Exclude);

        if (!String.IsNullOrEmpty(q) && q.Trim() != "") {
                decisions = decisions
                        .Where(i => i.Name
                                    .Trim()
                                    .ToLower()
                                    .Contains(q
                                                .Trim()
                                                .ToLower()))
                        .Include(ti => ti.Choices)
                        .Include(ti => ti.Conditions)
                        .ThenInclude(thenTi => thenTi.Include)
                        .Include(ti => ti.Conditions)
                        .ThenInclude(thenTi => thenTi.Exclude);
            } 

          switch (sortorder)
            {
                case "name":
                    return await PaginatedList<ConditionalDecisionItem>
                            .CreateAsync(decisions
                                            .OrderBy(s => s.Name), pageNumber ?? 1, pageSize);
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
                case "updated":
                    return await PaginatedList<ConditionalDecisionItem>
                                    .CreateAsync(decisions
                                                    .OrderBy(s => s.UpdatedAt), pageNumber ?? 1, pageSize);
                case "updated_desc":
                    return await PaginatedList<ConditionalDecisionItem>
                                    .CreateAsync(decisions
                                                    .OrderByDescending(s => s.UpdatedAt), pageNumber ?? 1, pageSize);
                default:
                    return await PaginatedList<ConditionalDecisionItem>
                            .CreateAsync(decisions
                                            .OrderBy(s => s.Name), pageNumber ?? 1, pageSize);               
            }
        }

        [HttpGet("totalPages")]
        public async Task<ActionResult<int>> GetTotalPages() 
        {
             if (_context.ConditionalDecisionItems == null)
            {
                return NotFound();
            }

            int pageSize = 5;

            var decisions = await _context.ConditionalDecisionItems
                        .Include(ti => ti.Choices)
                        .ToListAsync();

            var pagination = new PaginatedList<ConditionalDecisionItem>(decisions, decisions.Count(), 1, pageSize);

            return pagination.TotalPages;
        }

        // GET: api/ConditionalDecisionItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConditionalDecisionItem>> GetConditionalDecisionItem(long id)
        {
          if (_context.ConditionalDecisionItems == null)
          {
              return NotFound();
          }
            var conditionalDecisionItems = await _context.ConditionalDecisionItems
                .Include(ti => ti.Choices)
                .Include(ti => ti.Conditions)
                .ThenInclude(ti => ti.Include)
                .Include(ti => ti.Conditions)
                .ThenInclude(ti => ti.Exclude)
                .ToListAsync();

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

            foreach (var item in conditionalDecisionItem.Choices)
            {
                _context.Choices.Add(item);
            }

            foreach (var item in conditionalDecisionItem.Conditions)
            {
                _context.Conditions.Add(item);
                foreach (var _include in item.Include)
                {
                    if (_include.Type.Equals("include"))
                    {
                        _context.Include.Add(_include);
                    }
                }
                foreach (var _exclude in item.Exclude)
                {
                    if (_exclude.Type.Equals("exclude"))
                    {
                        _context.Exclude.Add(_exclude);
                    }
                }
            }
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
        public async Task<ActionResult<FinalResult>> GetMakeConditionalDecision(long id, [FromBody] List<ConditionalInput> conditionalInput)
        {
            var conditionalDecisionItem = await this.GetConditionalDecisionItem(id);

            if (conditionalDecisionItem == null || conditionalDecisionItem.Value == null || conditionalInput == null) return NotFound();

            Dictionary<string, int> PointDict = new Dictionary<string, int>();

            conditionalDecisionItem.Value.Choices.ForEach(choice => {
                if (choice?.RefId != null) {
                    PointDict.Add(choice.RefId, 0);
                }
                
            });

            conditionalInput.ForEach(c => {
                if (c.Value)
                {
                    c.Include.ForEach(inc => {
                        if (PointDict.ContainsKey(inc.ChoiceId)) PointDict[inc.ChoiceId]++;
                    });
                    c.Exclude.ForEach(exc => {
                        if (PointDict.ContainsKey(exc.ChoiceId)) PointDict[exc.ChoiceId]--;
                    });
                }
            });

            List<WeightedResult> _FinalResult = new List<WeightedResult>();
            int idIndex = 0;
            foreach(KeyValuePair<string, int> entry in PointDict) 
            {
                Choice? EntryChoice = conditionalDecisionItem.Value.Choices.Find(choice => choice.RefId != null ? choice.RefId.Equals(entry.Key) : false);
                if (EntryChoice == null) return NotFound();
                _FinalResult.Add(new WeightedResult(idIndex, EntryChoice.Id, entry.Value, EntryChoice.Name));
                idIndex++;
            }
            _FinalResult = _FinalResult.OrderByDescending(o => o.TotalWeight).ToList();

            return new FinalResult(_FinalResult, conditionalDecisionItem.Value.Name);
        }

        private bool ConditionalDecisionItemExists(long id)
        {
            return (_context.ConditionalDecisionItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
