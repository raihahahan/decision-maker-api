using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DecisionMakerApi.Common.Domains;
using DecisionMakerApi.Features.WeightedDecision.Models;

namespace DecisionMakerApi.Source.Features.WeightedDecision.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeightedChoicesController : ControllerBase
    {
        private readonly WeightedDecisionContext _context;

        public WeightedChoicesController(WeightedDecisionContext context)
        {
            _context = context;
        }

        // GET: api/WeightedChoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Choice>>> GetChoices()
        {
          if (_context.Choices == null)
          {
              return NotFound();
          }
            return await _context.Choices.ToListAsync();
        }

        // GET: api/WeightedChoices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Choice>> GetChoice(long id)
        {
          if (_context.Choices == null)
          {
              return NotFound();
          }
            var choice = await _context.Choices.FindAsync(id);

            if (choice == null)
            {
                return NotFound();
            }

            return choice;
        }

        // PUT: api/WeightedChoices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChoice(long id, Choice choice)
        {
            if (id != choice.Id)
            {
                return BadRequest();
            }

            _context.Entry(choice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChoiceExists(id))
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

        // POST: api/WeightedChoices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Choice>> PostChoice(Choice choice)
        {
          if (_context.Choices == null)
          {
              return Problem("Entity set 'WeightedDecisionContext.Choices'  is null.");
          }
            _context.Choices.Add(choice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChoice", new { id = choice.Id }, choice);
        }

        // DELETE: api/WeightedChoices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChoice(long id)
        {
            if (_context.Choices == null)
            {
                return NotFound();
            }
            var choice = await _context.Choices.FindAsync(id);
            if (choice == null)
            {
                return NotFound();
            }

            _context.Choices.Remove(choice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChoiceExists(long id)
        {
            return (_context.Choices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
