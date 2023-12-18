
using conversor.Data.Interfaces;
using conversor.Entities;
using conversor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conversor.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class PlanController : ControllerBase
{
    private readonly IPlanService _context;

    public PlanController(IPlanService context)
    {
        _context = context;
    }

    [Route("all")]
    [Authorize(Roles = "admin")]
    [HttpGet]
    public IActionResult GetAll()
    {
        var plans = _context.GetPlans();
        return Ok(plans);
    }

    [HttpGet("{planId}")]
    public ActionResult<Plan> GetPlan(int planId)
    {
        if (_context.GetPlanId(planId) == null)
        {
            return NotFound(new
            {
                error = "The plan ID was not found"
            });
        }
        
        var plan = _context.GetPlanId(planId);
        return Ok(plan);
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public ActionResult<Plan> PostPlan(Plan planForCreationDto)
    {
        if (_context.GetPlanName(planForCreationDto.Name) != null)
        {
            return Conflict(new
            {
                error = "Plan name already exists"
            });
        }
        
        if (planForCreationDto.Limit <= -1)
        {
            return Conflict(new
            {
                error = "Plan limit must be higher than -1"
            });
        }
        
        if (planForCreationDto.Price <= 0)
        {
            return Conflict(new
            {
                error = "Plan price must be higher than 0"
            });
        }

        var plan = _context.CreatePlan(planForCreationDto);
        return Ok(plan);
    }

    [HttpPut]
    [Authorize(Roles = "admin")]
    public ActionResult<Plan> PutPlan(Plan planForUpdateDto)
    {
        if (_context.GetPlanId(planForUpdateDto.Id) == null)
        {
            return NotFound(new
            {
                error = "Plan id not found"
            });
        }

        if (_context.GetPlanName(planForUpdateDto.Name) != null)
        {
            return Conflict(new
            {
                error = "Plan name already exists"
            });
        }
        
        if (planForUpdateDto.Limit <= -1)
        {
            return Conflict(new
            {
                error = "Plan limit must be greater than  -1"
            });
        }
        
        if (planForUpdateDto.Price <= 0)
        {
            return Conflict(new
            {
                error = "Plan price must be greater than 0"
            });
        }

        _context.UpdatePlan(planForUpdateDto);
        return Ok("Plan updated successfully");
    }

    [HttpDelete("{planId}")]
    [Authorize(Roles = "admin")]
    public ActionResult<Plan> DeletePlan(int planId)
    {
        if (_context.GetPlanId(planId) == null)
        {
            return NotFound(new
            {
                error = "Plan id not found"
            });
        }

        _context.DeletePlan(planId);
        return Ok("Plan deleted successfully");
    }
}