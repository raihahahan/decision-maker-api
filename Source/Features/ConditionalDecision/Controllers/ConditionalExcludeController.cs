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
    public class ConditionalExcludeController : ControllerBase
    {
        private readonly ConditionalDecisionContext _context;

        public ConditionalExcludeController(ConditionalDecisionContext context)
        {
            _context = context;
        }

        // GET: api/ConditionalExclude
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExcludeItem>>> GetExclude()
        {
          if (_context.Exclude == null)
          {
              return NotFound();
          }
            return await _context.Exclude.ToListAsync();
        }

        // GET: api/ConditionalExclude/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExcludeItem>> GetExcludeItem(long id)
        {
          if (_context.Exclude == null)
          {
              return NotFound();
          }
            var excludeItem = await _context.Exclude.FindAsync(id);

            if (excludeItem == null)
            {
                return NotFound();
            }

            return excludeItem;
        }

        // PUT: api/ConditionalExclude/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExcludeItem(long id, ExcludeItem excludeItem)
        {
            if (id != excludeItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(excludeItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExcludeItemExists(id))
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

        // POST: api/ConditionalExclude
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExcludeItem>> PostExcludeItem(ExcludeItem excludeItem)
        {
          if (_context.Exclude == null)
          {
              return Problem("Entity set 'ConditionalDecisionContext.Exclude'  is null.");
          }
            _context.Exclude.Add(excludeItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExcludeItem", new { id = excludeItem.Id }, excludeItem);
        }

        // DELETE: api/ConditionalExclude/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExcludeItem(long id)
        {
            if (_context.Exclude == null)
            {
                return NotFound();
            }
            var excludeItem = await _context.Exclude.FindAsync(id);
            if (excludeItem == null)
            {
                return NotFound();
            }

            _context.Exclude.Remove(excludeItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExcludeItemExists(long id)
        {
            return (_context.Exclude?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
