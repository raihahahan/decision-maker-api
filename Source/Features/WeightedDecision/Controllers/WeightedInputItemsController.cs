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
    public class WeightedInputItemsController : ControllerBase
    {
        private readonly WeightedDecisionContext _context;

        public WeightedInputItemsController(WeightedDecisionContext context)
        {
            _context = context;
        }

        // GET: api/WeightedInputItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeightedInputItem>>> GetWeightedInputItems()
        {
            if (_context.WeightedInputItems == null)
            {
                return NotFound();
            }
            return await _context.WeightedInputItems
                .Include(i => i.WeightedInputs)
                .ThenInclude(j => j.CriteriaInput)
                .ToListAsync();
        }

        // GET: api/WeightedInputItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WeightedInputItem>> GetWeightedInputItem(long id)
        {
            if (_context.WeightedInputItems == null)
            {
                return NotFound();
            }
            var weightedInputItems = await _context.WeightedInputItems
                                                .Include(i => i.WeightedInputs)
                                                .ThenInclude(i => i.CriteriaInput)
                                                .ToListAsync();

            var weightedInputItem = weightedInputItems.Find(i => i.WeightedItemId == id);

            if (weightedInputItem == null)
            {
                return NotFound();
            }

            return weightedInputItem;
        }

        // PUT: api/WeightedInputItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeightedInputItem(long id, WeightedInputItem weightedInputItem)
        {
            if (id != weightedInputItem.WeightedItemId)
            {
                return BadRequest();
            }

            _context.Entry(weightedInputItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeightedInputItemExists(id))
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

        // POST: api/WeightedInputItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WeightedInputItem>> PostWeightedInputItem(WeightedInputItem weightedInputItem)
        {
            if (_context.WeightedInputItems == null)
            {
                return Problem("Entity set 'WeightedDecisionContext.WeightedInputItems'  is null.");
            }
            
            _context.WeightedInputItems.Add(weightedInputItem);
            foreach (var item in weightedInputItem.WeightedInputs)
            {
                item.ForeignId = weightedInputItem.Id;
                _context.WeightedInputs.Add(item);
                foreach (var _item in item.CriteriaInput)
                {
                    _item.InputId = item.Id;
                    _context.CriteriaInputs.Add(_item);
                }
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWeightedInputItem", new { id = weightedInputItem.Id }, weightedInputItem);
        }

        // DELETE: api/WeightedInputItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeightedInputItem(long id)
        {
            if (_context.WeightedInputItems == null)
            {
                return NotFound();
            }
            var weightedInputItem = await _context.WeightedInputItems.FindAsync(id);
            if (weightedInputItem == null)
            {
                return NotFound();
            }

            _context.WeightedInputItems.Remove(weightedInputItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WeightedInputItemExists(long id)
        {
            return (_context.WeightedInputItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
