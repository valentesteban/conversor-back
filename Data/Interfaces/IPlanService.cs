using conversor.Entities;

namespace conversor.Data.Interfaces;

public interface IPlanService
{
    public List<Plan> GetPlans();
    
    public Plan? GetPlanId(int id);
    public Plan? GetPlanName(string name);
    public Plan CreatePlan(Plan planForCreationDto);
    public void UpdatePlan(Plan planForUpdateDto);
    public void DeletePlan(int planId);
}