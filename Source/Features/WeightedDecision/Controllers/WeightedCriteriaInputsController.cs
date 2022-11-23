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
    public class WeightedCriteriaInputsController : ControllerBase
    {
        private readonly WeightedDecisionContext _context;

        public WeightedCriteriaInputsController(WeightedDecisionContext context)
        {
            _context = context;
        }

        // GET: api/WeightedCriteriaInputs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CriteriaInput>>> GetCriteriaInputs()
        {
          if (_context.CriteriaInputs == null)
          {
              return NotFound();
          }
            return await _context.CriteriaInputs.ToListAsync();
        }

        // GET: api/WeightedCriteriaInputs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CriteriaInput>> GetCriteriaInput(long id)
        {
          if (_context.CriteriaInputs == null)
          {
              return NotFound();
          }
            var criteriaInput = await _context.CriteriaInputs.FindAsync(id);

            if (criteriaInput == null)
            {
                return NotFound();
            }

            return criteriaInput;
        }

        // PUT: api/WeightedCriteriaInputs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCriteriaInput(long id, CriteriaInput criteriaInput)
        {
            if (id != criteriaInput.Id)
            {
                return BadRequest();
            }

            _context.Entry(criteriaInput).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CriteriaInputExists(id))
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

        [HttpPut("{id}/editName")]
         public async Task<IActionResult> PutCriteriaInputName(long id, CriteriaInput criteriaInput)
        {
            if (id != criteriaInput.CriteriaId)
            {
                return BadRequest();
            }

            var criteriaInputs = await _context.CriteriaInputs.ToListAsync();

            var toEdit = criteriaInputs.Where(i => i.CriteriaId == id);

            foreach (var item in toEdit)
            {
                item.Name = criteriaInput.Name;
                item.Weight = criteriaInput.Weight;
                // _context.Entry(item).State = EntityState.Modified;                
            }

            _context.CriteriaInputs.UpdateRange(toEdit);

            // _context.Entry(criteriaInput).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CriteriaInputExists(id))
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

        // POST: api/WeightedCriteriaInputs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CriteriaInput>> PostCriteriaInput(CriteriaInput criteriaInput)
        {
          if (_context.CriteriaInputs == null)
          {
              return Problem("Entity set 'WeightedDecisionContext.CriteriaInputs'  is null.");
          }
            _context.CriteriaInputs.Add(criteriaInput);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCriteriaInput", new { id = criteriaInput.Id }, criteriaInput);
        }

        // DELETE: api/WeightedCriteriaInputs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCriteriaInput(long id)
        {
            if (_context.CriteriaInputs == null)
            {
                return NotFound();
            }
            var criteriaInput = await _context.CriteriaInputs.ToListAsync();
            var toDelete = criteriaInput.Where(i => i.CriteriaId == id);
            if (toDelete == null)
            {
                return NotFound();
            }

            _context.CriteriaInputs.RemoveRange(toDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CriteriaInputExists(long id)
        {
            return (_context.CriteriaInputs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
