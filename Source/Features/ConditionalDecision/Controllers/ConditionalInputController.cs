using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DecisionMakerApi.Features.ConditionalDecision.Domains;
using DecisionMakerApi.Features.ConditionalDecision.Models;

namespace DecisionMakerApi.Source.Features.ConditionalDecision.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConditionalInputController : ControllerBase
    {
        private readonly ConditionalDecisionContext _context;

        public ConditionalInputController(ConditionalDecisionContext context)
        {
            _context = context;
        }

        // GET: api/ConditionalInput
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConditionalInput>>> GetConditionalInputs()
        {
          if (_context.ConditionalInputs == null)
          {
              return NotFound();
          }
            return await _context.ConditionalInputs.ToListAsync();
        }

        // GET: api/ConditionalInput/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConditionalInput>> GetConditionalInput(long id)
        {
          if (_context.ConditionalInputs == null)
          {
              return NotFound();
          }
            var conditionalInput = await _context.ConditionalInputs.FindAsync(id);

            if (conditionalInput == null)
            {
                return NotFound();
            }

            return conditionalInput;
        }

        // PUT: api/ConditionalInput/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConditionalInput(long id, ConditionalInput conditionalInput)
        {
            if (id != conditionalInput.Id)
            {
                return BadRequest();
            }

            _context.Entry(conditionalInput).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConditionalInputExists(id))
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

        // POST: api/ConditionalInput
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ConditionalInput>> PostConditionalInput(ConditionalInput conditionalInput)
        {
          if (_context.ConditionalInputs == null)
          {
              return Problem("Entity set 'ConditionalDecisionContext.ConditionalInputs'  is null.");
          }
            _context.ConditionalInputs.Add(conditionalInput);
         
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConditionalInput", new { id = conditionalInput.Id }, conditionalInput);
        }

        // DELETE: api/ConditionalInput/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConditionalInput(long id)
        {
            if (_context.ConditionalInputs == null)
            {
                return NotFound();
            }
            var conditionalInput = await _context.ConditionalInputs.FindAsync(id);
            if (conditionalInput == null)
            {
                return NotFound();
            }

            _context.ConditionalInputs.Remove(conditionalInput);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConditionalInputExists(long id)
        {
            return (_context.ConditionalInputs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
