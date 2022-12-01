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
    public class ConditionalIncludeController : ControllerBase
    {
        private readonly ConditionalDecisionContext _context;

        public ConditionalIncludeController(ConditionalDecisionContext context)
        {
            _context = context;
        }

        // GET: api/ConditionalInclude
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncludeItem>>> GetInclude()
        {
          if (_context.Include == null)
          {
              return NotFound();
          }
            return await _context.Include.ToListAsync();
        }

        // GET: api/ConditionalInclude/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IncludeItem>> GetIncludeItem(long id)
        {
          if (_context.Include == null)
          {
              return NotFound();
          }
            var includeItem = await _context.Include.FindAsync(id);

            if (includeItem == null)
            {
                return NotFound();
            }

            return includeItem;
        }

        // PUT: api/ConditionalInclude/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncludeItem(long id, IncludeItem includeItem)
        {
            if (id != includeItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(includeItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncludeItemExists(id))
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

        // POST: api/ConditionalInclude
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IncludeItem>> PostIncludeItem(IncludeItem includeItem)
        {
          if (_context.Include == null)
          {
              return Problem("Entity set 'ConditionalDecisionContext.Include'  is null.");
          }
            _context.Include.Add(includeItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIncludeItem", new { id = includeItem.Id }, includeItem);
        }

        // DELETE: api/ConditionalInclude/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncludeItem(long id)
        {
            if (_context.Include == null)
            {
                return NotFound();
            }
            var includeItem = await _context.Include.FindAsync(id);
            if (includeItem == null)
            {
                return NotFound();
            }

            _context.Include.Remove(includeItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IncludeItemExists(long id)
        {
            return (_context.Include?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
