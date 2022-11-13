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

        public RandomDecisionItemsController(RandomDecisionContext context)
        {
            _context = context;
        }

        // GET: api/RandomDecisionItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RandomDecisionItem>>> GetRandomDecisionItems(string? sortorder, int? pageNumber)
        {
          if (_context.RandomDecisionItems == null)
          {
              return NotFound();
          }

        int pageSize = 3;

        var decisions = _context.RandomDecisionItems
                    .Include(ti => ti.Choices);

          switch (sortorder)
            {
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
                default:
                    return await PaginatedList<RandomDecisionItem>
                            .CreateAsync(decisions
                                            .OrderBy(s => s.Name), pageNumber ?? 1, pageSize); 
            }
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
            List<WeightedResult> w = ls.Select((item, index) => {
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

            var randomDecisionItems =  await _context.RandomDecisionItems.Include(ti => ti.Choices).ToListAsync();

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
            randomDecisionItem.UpdatedAt = new DateTime();
            _context.Entry(randomDecisionItem).Property(i => i.UpdatedAt).IsModified = true;
            _context.Entry(randomDecisionItem).State = EntityState.Modified;

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
