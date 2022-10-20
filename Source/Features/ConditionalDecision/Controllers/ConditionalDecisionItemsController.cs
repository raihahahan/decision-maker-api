using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DecisionMakerApi.Features.ConditionalDecision.Models;

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
        public async Task<ActionResult<IEnumerable<ConditionalDecisionItem>>> GetConditionalDecisionItems()
        {
          if (_context.ConditionalDecisionItems == null)
          {
              return NotFound();
          }
            return await _context.ConditionalDecisionItems.Include(ti => ti.Choices).Include(ti => ti.Conditions.Include).Include(ti => ti.Conditions.Exclude).ToListAsync();
        }

        // GET: api/ConditionalDecisionItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConditionalDecisionItem>> GetConditionalDecisionItem(long id)
        {
          if (_context.ConditionalDecisionItems == null)
          {
              return NotFound();
          }
            var conditionalDecisionItems = await _context.ConditionalDecisionItems.Include(ti => ti.Choices).Include(ti => ti.Conditions.Include).Include(ti => ti.Conditions.Exclude).ToListAsync();

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

        private bool ConditionalDecisionItemExists(long id)
        {
            return (_context.ConditionalDecisionItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
