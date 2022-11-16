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
    public class WeightedCriteriasController : ControllerBase
    {
        private readonly WeightedDecisionContext _context;

        public WeightedCriteriasController(WeightedDecisionContext context)
        {
            _context = context;
        }

        // GET: api/WeightedCriterias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Criteria>>> GetCriterias()
        {
          if (_context.Criterias == null)
          {
              return NotFound();
          }
            return await _context.Criterias.ToListAsync();
        }

        // GET: api/WeightedCriterias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Criteria>> GetCriteria(long id)
        {
          if (_context.Criterias == null)
          {
              return NotFound();
          }
            var criteria = await _context.Criterias.FindAsync(id);

            if (criteria == null)
            {
                return NotFound();
            }

            return criteria;
        }

        // PUT: api/WeightedCriterias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCriteria(long id, Criteria criteria)
        {
            if (id != criteria.Id)
            {
                return BadRequest();
            }

            _context.Entry(criteria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CriteriaExists(id))
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

        // POST: api/WeightedCriterias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Criteria>> PostCriteria(Criteria criteria)
        {
          if (_context.Criterias == null)
          {
              return Problem("Entity set 'WeightedDecisionContext.Criterias'  is null.");
          }
            _context.Criterias.Add(criteria);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCriteria", new { id = criteria.Id }, criteria);
        }

        // DELETE: api/WeightedCriterias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCriteria(long id)
        {
            if (_context.Criterias == null)
            {
                return NotFound();
            }
            var criteria = await _context.Criterias.FindAsync(id);
            if (criteria == null)
            {
                return NotFound();
            }

            _context.Criterias.Remove(criteria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CriteriaExists(long id)
        {
            return (_context.Criterias?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
