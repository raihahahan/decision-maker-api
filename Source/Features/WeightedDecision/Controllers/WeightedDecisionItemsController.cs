using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DecisionMakerApi.Features.WeightedDecision.Models;
using DecisionMakerApi.Features.WeightedDecision.Domains;
using DecisionMakerApi.Common.Domains;
using DecisionMakerApi.Services.Pagination;

namespace DecisionMakerApi.Source.Features.WeightedDecision.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeightedDecisionItemsController : ControllerBase
    {
        private readonly WeightedDecisionContext _context;
        private readonly ILogger _logger;
        private const int pageSize = 5;
        public WeightedDecisionItemsController(WeightedDecisionContext context, ILogger<WeightedDecisionItemsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/WeightedDecisionItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeightedDecisionItem>>> GetWeightedDecisionItems(string? sortorder, int? pageNumber, string? q)
        {

            if (_context.WeightedDecisionItems == null)
            {
                return NotFound();
            }

            var decisions = _context.WeightedDecisionItems
                                .Include(ti => ti.Choices)
                                .Include(ti => ti.CriteriaList);

            if (!String.IsNullOrEmpty(q) && q.Trim() != "") {
                decisions = decisions
                        .Where(i => i.Name
                                    .Trim()
                                    .ToLower()
                                    .Contains(q
                                                .Trim()
                                                .ToLower()))
                        .Include(ti => ti.Choices)
                        .Include(ti => ti.CriteriaList);
            } 

            switch (sortorder)
            {
                case "name":
                    return await PaginatedList<WeightedDecisionItem>
                            .CreateAsync(decisions
                                            .OrderBy(s => s.Name), pageNumber ?? 1, pageSize);
                case "name_desc":
                    return await PaginatedList<WeightedDecisionItem>
                            .CreateAsync(decisions
                                            .OrderByDescending(s => s.Name), pageNumber ?? 1, pageSize);
                case "Date":
                    return await PaginatedList<WeightedDecisionItem>
                            .CreateAsync(decisions
                                            .OrderBy(s => s.CreatedAt), pageNumber ?? 1, pageSize);
                case "date_desc":
                    return await PaginatedList<WeightedDecisionItem>
                            .CreateAsync(decisions
                                            .OrderByDescending(s => s.CreatedAt), pageNumber ?? 1, pageSize);
                case "updated":
                    return await PaginatedList<WeightedDecisionItem>
                            .CreateAsync(decisions
                                            .OrderBy(s => s.UpdatedAt), pageNumber ?? 1, pageSize);
                case "updated_desc":
                    return await PaginatedList<WeightedDecisionItem>
                            .CreateAsync(decisions
                                            .OrderByDescending(s => s.UpdatedAt), pageNumber ?? 1, pageSize);
                default:
                    return await PaginatedList<WeightedDecisionItem>
                            .CreateAsync(decisions
                                            .OrderByDescending(s => s.UpdatedAt), pageNumber ?? 1, pageSize);
            }
        }

        [HttpGet("totalPages")]
        public async Task<ActionResult<int>> GetTotalPages() 
        {
             if (_context.WeightedDecisionItems == null)
            {
                return NotFound();
            }

            int pageSize = 5;

            var decisions = await _context.WeightedDecisionItems
                        .Include(ti => ti.Choices)
                        .ToListAsync();

            var pagination = new PaginatedList<WeightedDecisionItem>(decisions, decisions.Count(), 1, pageSize);

            return pagination.TotalPages;
        }

        // GET: api/WeightedDecisionItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WeightedDecisionItem>> GetWeightedDecisionItem(long id)
        {
            if (_context.WeightedDecisionItems == null)
            {
                return NotFound();
            }
            var weightedDecisionItems = await _context.WeightedDecisionItems.Include(ti => ti.Choices).Include(ti => ti.CriteriaList).ToListAsync();

            var weightedDecisionItem = weightedDecisionItems.Find(i => i.Id == id);

            if (weightedDecisionItem == null)
            {
                return NotFound();
            }

            return weightedDecisionItem;
        }

        // POST: api/WeightedDecisionItems/5/decide
        [HttpPost("{id}/decide")]
        public async Task<ActionResult<FinalResult>> GetMakeWeightedDecision(long id, [FromBody] List<WeightedInput> _weightedInput)
        {

            var weightedDecisionItem = await this.GetWeightedDecisionItem(id);

            if (weightedDecisionItem == null || weightedDecisionItem.Value == null) return NotFound();

            List<Choice> choiceList = weightedDecisionItem.Value.Choices;
            if (choiceList.Count == 0) return NotFound();

            List<WeightedResult> FinalList = _weightedInput.Select((w, index) =>
            {
                double totalWeight = 0;
                w.CriteriaInput.ForEach(c =>
                {
                    totalWeight += c.value * c.Weight;
                });
                return new WeightedResult(index, w.ChoiceId, totalWeight, w.ChoiceName);
            }).OrderByDescending(o => o.TotalWeight).ToList();

            return new FinalResult(FinalList, weightedDecisionItem.Value.Name);
        }

        // PUT: api/WeightedDecisionItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeightedDecisionItem(long id, WeightedDecisionItem weightedDecisionItem)
        {
            if (id != weightedDecisionItem.Id)
            {
                _logger.LogError("0: id != weightedDecisionItem.Id");
                return BadRequest();
            }

            var existingWeightedDecisionItem = _context.WeightedDecisionItems
                .Include(ti => ti.Choices)
                .Include(ti => ti.CriteriaList)
                .FirstOrDefault(i => i.Id == id);

            if (existingWeightedDecisionItem == null)
            {
                return NotFound();
            }
            else 
            {   
                 // update non-nested fields
                _context.Entry(existingWeightedDecisionItem).CurrentValues.SetValues(weightedDecisionItem);

                // HANDLE NESTED FIELDS

                // CHOICES

                // add new choices and update edited choices
                foreach (var choice in weightedDecisionItem.Choices)
                {
                    var existingChoice = existingWeightedDecisionItem.Choices
                                            .FirstOrDefault(c => c.Id == choice.Id);

                    if (existingChoice == null || choice.Id == 0)
                    {
                        _logger.LogInformation("ADD CHOICE");
                        choice.DecisionId = id;
                        _context.Choices.Add(choice);
                        _context.SaveChanges();

                        var newChoice = choice;
                        
                        var criteriaInputToAdd = new List<CriteriaInput>();
                        
                        var criteriasRef = _context.Criterias.Where(i => i.DecisionId == id).ToList();

                        foreach (var item in criteriasRef)
                        {
                            var newCriteriaInput = new CriteriaInput
                            {
                                Name = item.Name,
                                Weight = item.Weight,
                                DecisionId = -1,
                                value = 20,
                                CriteriaId = item.Id
                            };
                            
                            criteriaInputToAdd.Add(newCriteriaInput);
                        }
                        

                        _logger.LogInformation("LENTHH: {length}", criteriaInputToAdd.Count);

                        var newWeightedInput = new WeightedInput
                            {
                                ChoiceId = newChoice.Id,
                                ChoiceName = newChoice.Name,
                                ForeignId = id,
                                CriteriaInput = criteriaInputToAdd
                                
                            };


                        _context.WeightedInputs.Add(newWeightedInput);
                        _context.SaveChanges();

                    }
                    else
                    {
                        _logger.LogInformation("UPDATE CHOICE {name}", choice.Name);
                        _context.Entry(existingChoice).CurrentValues.SetValues(choice);
                        var weightedInput = _context.WeightedInputs.Where(i => i.ChoiceId == choice.Id).FirstOrDefault();
                        
                        if (weightedInput != null) 
                        {
                            weightedInput.ChoiceName = choice.Name;
                            _context.Entry(weightedInput).State = EntityState.Modified;
                        }
                    }
                }

                // delete choices in existing decision item that no longer exists
                foreach (var choice in existingWeightedDecisionItem.Choices)
                {
                    var checkIfDeletedChoice = weightedDecisionItem.Choices
                        .FirstOrDefault(c => c.Id == choice.Id);
                    
                    if (checkIfDeletedChoice == null)
                    {
                        _logger.LogInformation("DELETE CHOICE {name}", choice.Name);
                        _context.Choices.Remove(choice);
                        var weightedInput = await _context.WeightedInputs.ToListAsync();
                        var toDelete = weightedInput.Find(i => i.ChoiceId == choice.Id);
                        if (toDelete == null)
                        {
                            return NotFound();
                        }
                        _context.WeightedInputs.Remove(toDelete);
                    }
                }
            }

            // CRITERIA LIST

            // add new criteria and update edited criteria and criteriaInput
            foreach (var criteria in weightedDecisionItem.CriteriaList)
            {
                var existingCriteria = existingWeightedDecisionItem.CriteriaList
                                        .FirstOrDefault(c => c.Id == criteria.Id);
                if (existingCriteria == null || criteria.Id == 0)
                {
                    // add criteria
                   
                    var weightedInputItems = _context.WeightedInputItems.Include(i => i.WeightedInputs).FirstOrDefault(i => i.WeightedItemId == id);

                    if (weightedInputItems == null || weightedInputItems.WeightedInputs == null)
                    {
                        _logger.LogError("3: weightedInputItems == null");
                        return BadRequest();
                    }
                    criteria.DecisionId = id;
                    _context.Criterias.Add(criteria);
                    _context.SaveChanges();

                    var postedCriteria = criteria;

                    foreach (var item in weightedInputItems.WeightedInputs)
                    {
                        var newCriteriaInput = new CriteriaInput
                        {
                            Name = criteria.Name,
                            Weight = criteria.Weight,
                            DecisionId = -1,
                            CriteriaId = postedCriteria.Id,
                            InputId = item.Id,
                            value = 20
                        };
                        _context.CriteriaInputs.Add(newCriteriaInput);
                    }               
                }
                else
                {
                    _context.Entry(existingCriteria).CurrentValues.SetValues(criteria);

                    // Update Weighted Input
                    var criteriaInputs = await _context.CriteriaInputs.ToListAsync();

                    var editedCriteriaInput = new CriteriaInput
                    {
                        CriteriaId = criteria.Id,
                        Name = criteria.Name,
                        Weight = criteria.Weight,
                        DecisionId = criteria.DecisionId,
                        value = 23,                        
                    };

                    var toEdit = criteriaInputs.Where(i => i.CriteriaId == criteria.Id);

                    foreach (var item in toEdit)
                    {
                        item.Name = editedCriteriaInput.Name;
                        item.Weight =  editedCriteriaInput.Weight;      
                    }

                    _context.CriteriaInputs.UpdateRange(toEdit);
                }
                _context.SaveChanges();
            }

            // delete criteria in existing decision item that no longer exists

            foreach (var criteria in existingWeightedDecisionItem.CriteriaList)
            {
                var checkIfDeletedCriteria = weightedDecisionItem.CriteriaList
                    .FirstOrDefault(c => c.Id == criteria.Id);
                
                if (checkIfDeletedCriteria == null)
                {
                    _context.Criterias.Remove(criteria);
                    var criteriaInput = await _context.CriteriaInputs.ToListAsync();
                    var toDelete = criteriaInput.Where(i => i.CriteriaId == criteria.Id);
                    if (toDelete == null)
                    {
                        return NotFound();
                    }

                    _context.CriteriaInputs.RemoveRange(toDelete);
                }
            }

            // _context.Entry(weightedDecisionItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeightedDecisionItemExists(id))
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


        // POST: api/WeightedDecisionItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WeightedDecisionItem>> PostWeightedDecisionItem(WeightedDecisionItem weightedDecisionItem)
        {
            if (_context.WeightedDecisionItems == null)
            {
                return Problem("Entity set 'WeightedDecisionContext.WeightedDecisionItems'  is null.");
            }
            _context.WeightedDecisionItems.Add(weightedDecisionItem);
            foreach (var item in weightedDecisionItem.Choices)
            {
                item.DecisionId = weightedDecisionItem.Id;
                _context.Choices.Add(item);
            }
            foreach (var item in weightedDecisionItem.CriteriaList)
            {
                item.DecisionId = weightedDecisionItem.Id;
                _context.Criterias.Add(item);
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWeightedDecisionItem), new { id = weightedDecisionItem.Id }, weightedDecisionItem);
        }

        // DELETE: api/WeightedDecisionItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeightedDecisionItem(long id)
        {
            if (_context.WeightedDecisionItems == null)
            {
                return NotFound();
            }
            var weightedDecisionItem = await _context.WeightedDecisionItems.FindAsync(id);
            if (weightedDecisionItem == null)
            {
                return NotFound();
            }

            _context.WeightedDecisionItems.Remove(weightedDecisionItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WeightedDecisionItemExists(long id)
        {
            return (_context.WeightedDecisionItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
