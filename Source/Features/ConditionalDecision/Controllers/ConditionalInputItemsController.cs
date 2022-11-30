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
    public class ConditionalInputItemsController : ControllerBase
    {
        private readonly ConditionalDecisionContext _context;

        public ConditionalInputItemsController(ConditionalDecisionContext context)
        {
            _context = context;
        }

        // GET: api/ConditionalInputItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConditionalInputItem>>> GetConditionalInputItems()
        {
          if (_context.ConditionalInputItems == null)
          {
              return NotFound();
          }
          var conditionalInputItems = await _context.ConditionalInputItems
            .Include(i => i.ConditionalInputs)
            .ThenInclude(i => i.Include)
            .Include(i => i.ConditionalInputs)
            .ThenInclude(i => i.Exclude)
            .ToListAsync();

            return conditionalInputItems;
        }

        // GET: api/ConditionalInputItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConditionalInputItem>> GetConditionalInputItem(long id)
        {
          if (_context.ConditionalInputItems == null)
          {
              return NotFound();
          }
          var conditionalInputItems = await _context.ConditionalInputItems
            .Include(i => i.ConditionalInputs)
            .ThenInclude(i => i.Include)
            .Include(i => i.ConditionalInputs)
            .ThenInclude(i => i.Exclude)
            .ToListAsync();

            var conditionalInputItem = conditionalInputItems.Find(i => i.ConditionalItemId == id);

            if (conditionalInputItem == null)
            {
                return NotFound();
            }

            return conditionalInputItem;
        }

        // PUT: api/ConditionalInputItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConditionalInputItem(long id, ConditionalInputItem conditionalInputItem)
        {
            if (id != conditionalInputItem.ConditionalItemId)
            {
                return BadRequest();
            }

            _context.Entry(conditionalInputItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConditionalInputItemExists(id))
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

        // POST: api/ConditionalInputItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ConditionalInputItem>> PostConditionalInputItem(ConditionalInputItem conditionalInputItem)
        {
          if (_context.ConditionalInputItems == null)
          {
              return Problem("Entity set 'ConditionalDecisionContext.ConditionalInputItems'  is null.");
          }
            _context.ConditionalInputItems.Add(conditionalInputItem);
            foreach (var item in conditionalInputItem.ConditionalInputs)
            {
                item.ForeignId = conditionalInputItem.Id;
                _context.ConditionalInputs.Add(item);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConditionalInputItem", new { id = conditionalInputItem.Id }, conditionalInputItem);
        }

        // DELETE: api/ConditionalInputItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConditionalInputItem(long id)
        {
            if (_context.ConditionalInputItems == null)
            {
                return NotFound();
            }
            var conditionalInputItem = await _context.ConditionalInputItems.FindAsync(id);
            if (conditionalInputItem == null)
            {
                return NotFound();
            }

            _context.ConditionalInputItems.Remove(conditionalInputItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConditionalInputItemExists(long id)
        {
            return (_context.ConditionalInputItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
