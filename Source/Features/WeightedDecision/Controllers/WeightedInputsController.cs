using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DecisionMakerApi.Features.WeightedDecision.Domains;
using DecisionMakerApi.Features.WeightedDecision.Models;

namespace DecisionMakerApi.Source.Features.WeightedDecision.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeightedInputsController : ControllerBase
    {
        private readonly WeightedDecisionContext _context;

        public WeightedInputsController(WeightedDecisionContext context)
        {
            _context = context;
        }

        // GET: api/WeightedInputs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeightedInput>>> GetWeightedInputs()
        {
            if (_context.WeightedInputs == null)
            {
                return NotFound();
            }
            return await _context.WeightedInputs
                    .Include(i => i.CriteriaInput)
                    .ToListAsync();
        }

        // GET: api/WeightedInputs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WeightedInput>> GetWeightedInput(long id)
        {
            if (_context.WeightedInputs == null)
            {
                return NotFound();
            }
            var weightedInput = await _context.WeightedInputs.FindAsync(id);

            if (weightedInput == null)
            {
                return NotFound();
            }

            return weightedInput;
        }

        // PUT: api/WeightedInputs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeightedInput(long id, WeightedInput weightedInput)
        {
            if (id != weightedInput.Id)
            {
                return BadRequest();
            }

            _context.Entry(weightedInput).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeightedInputExists(id))
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

        // POST: api/WeightedInputs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WeightedInput>> PostWeightedInput(WeightedInput weightedInput)
        {
            if (_context.WeightedInputs == null)
            {
                return Problem("Entity set 'WeightedDecisionContext.WeightedInputs'  is null.");
            }
            _context.WeightedInputs.Add(weightedInput);
            foreach (var item in weightedInput.CriteriaInput)
            {   
                _context.CriteriaInputs.Add(item);
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWeightedInput", new { id = weightedInput.Id }, weightedInput);
        }

        // DELETE: api/WeightedInputs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeightedInput(long id)
        {
            if (_context.WeightedInputs == null)
            {
                return NotFound();
            }
            var weightedInput = await _context.WeightedInputs.ToListAsync();
            var toDelete = weightedInput.Find(i => i.ChoiceId == id);
            if (toDelete == null)
            {
                return NotFound();
            }

            _context.WeightedInputs.Remove(toDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WeightedInputExists(long id)
        {
            return (_context.WeightedInputs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
