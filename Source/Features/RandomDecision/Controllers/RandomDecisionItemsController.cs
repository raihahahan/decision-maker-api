using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DecisionMakerApi.Features.RandomDecision.Models;

namespace DecisionMakerApi.Source.Feautures.RandomDecision.Controllers
{
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
        public async Task<ActionResult<IEnumerable<RandomDecisionItem>>> GetRandomDecisionItems()
        {
          if (_context.RandomDecisionItems == null)
          {
              return NotFound();
          }
            return await _context.RandomDecisionItems.ToListAsync();
        }

        // GET: api/RandomDecisionItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RandomDecisionItem>> GetRandomDecisionItem(long id)
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

            return randomDecisionItem;
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
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRandomDecisionItem", new { id = randomDecisionItem.Id }, randomDecisionItem);
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
