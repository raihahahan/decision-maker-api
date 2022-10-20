using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DecisionMakerApi.Features.WeightedDecision.Models;

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
        public async Task<ActionResult<IEnumerable<WeightedDecisionItem>>> GetWeightedDecisionItems()
        {
          if (_context.WeightedDecisionItems == null)
          {
              return NotFound();
          }
            return await _context.WeightedDecisionItems.ToListAsync();
        }

        // GET: api/WeightedDecisionItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WeightedDecisionItem>> GetWeightedDecisionItem(long id)
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

            return weightedDecisionItem;
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

            return CreatedAtAction("GetWeightedDecisionItem", new { id = weightedDecisionItem.Id }, weightedDecisionItem);
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
