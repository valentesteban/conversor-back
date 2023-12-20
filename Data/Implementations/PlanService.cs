using conversor.Data.Interfaces;
using conversor.Entities;

namespace conversor.Data.Implementations;

public class PlanService : IPlanService
{
    private readonly ConversorContext _context;

    public PlanService(ConversorContext context)
    {
        _context = context;
    }

    public List<Plan> GetPlans()
    {
        return _context.Plan.ToList();
    }

    public Plan? GetPlanId(int id)
    {
        return _context.Plan.FirstOrDefault((plan) => plan.Id == id);
    }

    public Plan? GetPlanName(string name)
    {
        return _context.Plan.FirstOrDefault((plan) => plan.Name.ToLower() == name.ToLower());
    }

    public Plan CreatePlan(Plan planForCreationDto)
    {
        Plan plan = new()
        {
            Name = planForCreationDto.Name,
            Limit = planForCreationDto.Limit
        };

        var planCreated = _context.Plan.Add(plan);
        _context.SaveChanges();

        return planCreated.Entity;
    }

    public void UpdatePlan(Plan planForUpdateDto)
    {
        var plan = GetPlanId(planForUpdateDto.Id)!;

        plan.Name = planForUpdateDto.Name;
        plan.Limit = planForUpdateDto.Limit;
        plan.Price = planForUpdateDto.Price;

        _context.Plan.Update(plan);
        _context.SaveChanges();
    }

    public void DeletePlan(int planId)
    {
        var plan = GetPlanId(planId)!;

        _context.Plan.Remove(plan);
        _context.SaveChanges();
    }
}