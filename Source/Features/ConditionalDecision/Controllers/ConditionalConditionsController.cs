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
    public class ConditionalConditionsController : ControllerBase
    {
        private readonly ConditionalDecisionContext _context;

        public ConditionalConditionsController(ConditionalDecisionContext context)
        {
            _context = context;
        }

        // GET: api/ConditionalConditions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Condition>>> GetConditions()
        {
          if (_context.Conditions == null)
          {
              return NotFound();
          }
            return await _context.Conditions.ToListAsync();
        }

        // GET: api/ConditionalConditions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Condition>> GetCondition(long id)
        {
          if (_context.Conditions == null)
          {
              return NotFound();
          }
            var condition = await _context.Conditions.FindAsync(id);

            if (condition == null)
            {
                return NotFound();
            }

            return condition;
        }

        // PUT: api/ConditionalConditions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCondition(long id, Condition condition)
        {
            if (id != condition.Id)
            {
                return BadRequest();
            }

            _context.Entry(condition).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConditionExists(id))
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

        // POST: api/ConditionalConditions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Condition>> PostCondition(Condition condition)
        {
          if (_context.Conditions == null)
          {
              return Problem("Entity set 'ConditionalDecisionContext.Conditions'  is null.");
          }
            _context.Conditions.Add(condition);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCondition", new { id = condition.Id }, condition);
        }

        // DELETE: api/ConditionalConditions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCondition(long id)
        {
            if (_context.Conditions == null)
            {
                return NotFound();
            }
            var condition = await _context.Conditions.FindAsync(id);
            if (condition == null)
            {
                return NotFound();
            }

            _context.Conditions.Remove(condition);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConditionExists(long id)
        {
            return (_context.Conditions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
