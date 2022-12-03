using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DecisionMakerApi.Features.RandomDecision.Models;
using DecisionMakerApi.Common.Domains;
using DecisionMakerApi.Services.Pagination;
using Microsoft.AspNetCore.Cors;

namespace DecisionMakerApi.Source.Feautures.RandomDecision.Controllers
{
    [EnableCors("CORS_spec")]
    [Route("api/[controller]")]
    [ApiController]
    public class RandomDecisionItemsController : ControllerBase
    {
        private readonly RandomDecisionContext _context;
        private readonly ILogger _logger;
        private const int pageSize = 5;
        public RandomDecisionItemsController(RandomDecisionContext context, ILogger<RandomDecisionItemsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/RandomDecisionItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RandomDecisionItem>>> GetRandomDecisionItems(string? sortorder, int? pageNumber, string? q)
        {
            if (_context.RandomDecisionItems == null)
            {
                return NotFound();
            }
            
            var decisions = _context.RandomDecisionItems.Include(i => i.Choices);

            if (!String.IsNullOrEmpty(q) && q.Trim() != "") {
                decisions = decisions
                        .Where(i => i.Name
                                    .Trim()
                                    .ToLower()
                                    .Contains(q
                                                .Trim()
                                                .ToLower()))
                        .Include(ti => ti.Choices);
            } 

            switch (sortorder)
            {
                case "name":
                    return await PaginatedList<RandomDecisionItem>
                                    .CreateAsync(decisions
                                                    .OrderBy(s => s.Name), pageNumber ?? 1, pageSize);
                case "name_desc":
                    return await PaginatedList<RandomDecisionItem>
                                    .CreateAsync(decisions
                                                    .OrderByDescending(s => s.Name), pageNumber ?? 1, pageSize);
                case "Date":
                    return await PaginatedList<RandomDecisionItem>
                                    .CreateAsync(decisions
                                                    .OrderBy(s => s.CreatedAt), pageNumber ?? 1, pageSize);
                case "date_desc":
                    return await PaginatedList<RandomDecisionItem>
                                    .CreateAsync(decisions
                                                    .OrderByDescending(s => s.CreatedAt), pageNumber ?? 1, pageSize);
                case "updated":
                    return await PaginatedList<RandomDecisionItem>
                                    .CreateAsync(decisions
                                                    .OrderBy(s => s.UpdatedAt), pageNumber ?? 1, pageSize);
                case "updated_desc":
                    return await PaginatedList<RandomDecisionItem>
                                    .CreateAsync(decisions
                                                    .OrderByDescending(s => s.UpdatedAt), pageNumber ?? 1, pageSize);
                default:
                    return await PaginatedList<RandomDecisionItem>
                            .CreateAsync(decisions
                                            .OrderByDescending(s => s.UpdatedAt), pageNumber ?? 1, pageSize);
            }
        }

        [HttpGet("totalPages")]
        public async Task<ActionResult<int>> GetTotalPages() 
        {
             if (_context.RandomDecisionItems == null)
            {
                return NotFound();
            }

            int pageSize = 5;

            var decisions = await _context.RandomDecisionItems
                        .Include(ti => ti.Choices)
                        .ToListAsync();

            var pagination = new PaginatedList<RandomDecisionItem>(decisions, decisions.Count(), 1, pageSize);

            return pagination.TotalPages;
        }

        // GET: api/RandomDecisionItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RandomDecisionItem>> GetRandomDecisionItem(long id)
        {
            if (_context.RandomDecisionItems == null)
            {
                return NotFound();
            }
            var randomDecisionItems = await _context.RandomDecisionItems.Include(ti => ti.Choices).ToListAsync();

            var randomDecisionItem = randomDecisionItems.Find(i => i.Id == id);

            if (randomDecisionItem == null)
            {
                return NotFound();
            }

            return randomDecisionItem;
        }



        private ActionResult<FinalResult> Decide(RandomDecisionItem randomDecisionItem)
        {
            if (randomDecisionItem == null)
            {
                return NotFound();
            }

            List<Choice> ls = randomDecisionItem.Choices;
            if (ls.Count == 0) return NotFound();
            List<WeightedResult> w = ls.Select((item, index) =>
            {
                var rand = new Random();
                double weight = rand.NextDouble();
                return new WeightedResult(index, item.Id, weight, item.Name);
            })
            .OrderBy(s => s.TotalWeight)
            .ToList();

            return new FinalResult(w, randomDecisionItem.Name);
        }

        [HttpPost("decide")]
        public ActionResult<FinalResult> PostMakeRandomDecision(RandomDecisionItem randomDecisionItem)
        {
            return Decide(randomDecisionItem);
        }


        // POST: api/RandomDecisionItem/5/decide
        [HttpPost("{id}/decide")]
        public async Task<ActionResult<FinalResult>> PostMakeRandomDecision(long id)
        {
            if (_context.RandomDecisionItems == null)
            {
                return NotFound();
            }

            var randomDecisionItems = await _context.RandomDecisionItems.Include(ti => ti.Choices).ToListAsync();

            var randomDecisionItem = randomDecisionItems.Find(i => i.Id == id);

            if (randomDecisionItem == null) return NotFound();

            return Decide(randomDecisionItem);
        }


        // PUT: api/RandomDecisionItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRandomDecisionItem(long id, RandomDecisionItem randomDecisionItem)
        {
            if (id != randomDecisionItem.Id)
            {
                return BadRequest();
            }

            /*
                1. get current existing decision item
                2. for each choice in existing decision item, delete the choice if it does not exist in the new decision item.
                3. for each choice in new decision item, add the choice if it does not currently exists.
                4. else, update the choice.
            */

            var existingRandomDecisionItem = _context.RandomDecisionItems
                                                .Include(i => i.Choices)
                                                .FirstOrDefault(i => i.Id == id);
            
            if (existingRandomDecisionItem == null)
            {
                return NotFound();
            }
            else
            {
                // update non-nested fields
                _context.Entry(existingRandomDecisionItem).CurrentValues.SetValues(randomDecisionItem);

                 // add new choices and update edited choices
                foreach (var choice in randomDecisionItem.Choices)
                {
                    var existingChoice = existingRandomDecisionItem.Choices
                                            .FirstOrDefault(c => c.Id == choice.Id);

                    if (existingChoice == null || choice.Id == 0)
                    {
                        existingRandomDecisionItem.Choices.Add(choice);
                    }
                    else
                    {
                        _context.Entry(existingChoice).CurrentValues.SetValues(choice);
                    }
                }

                // delete choices in existing decision item that no longer exists
                foreach (var choice in existingRandomDecisionItem.Choices)
                {
                    var checkIfDeletedChoice = randomDecisionItem.Choices
                        .FirstOrDefault(c => c.Id == choice.Id);
                    
                    if (checkIfDeletedChoice == null)
                    {
                        _context.Choices.Remove(choice);
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RandomDecisionItemExists(id))
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

        // POST: api/RandomDecisionItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RandomDecisionItem>> PostRandomDecisionItem(RandomDecisionItem randomDecisionItem)
        {
            if (_context.RandomDecisionItems == null)
            {
                return Problem("Entity set 'RandomDecisionContext.RandomDecisionItems'  is null.");
            }
            _context.RandomDecisionItems.Add(randomDecisionItem);
            foreach (var item in randomDecisionItem.Choices)
            {
                item.DecisionId = randomDecisionItem.Id;
                _context.Choices.Add(item);
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRandomDecisionItem), new { id = randomDecisionItem.Id }, randomDecisionItem);
        }

        // DELETE: api/RandomDecisionItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRandomDecisionItem(long id)
        {
            if (_context.RandomDecisionItems == null)
            {
                return NotFound();
            }
            var randomDecisionItem = await _context.RandomDecisionItems.FindAsync(id);
            if (randomDecisionItem == null)
            {
                return NotFound();
            }

            _context.RandomDecisionItems.Remove(randomDecisionItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RandomDecisionItemExists(long id)
        {
            return (_context.RandomDecisionItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
