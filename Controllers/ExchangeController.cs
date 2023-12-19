using conversor.Data.Interfaces;
using conversor.Entities;
using conversor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conversor.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ExchangeController : ControllerBase
{
    private readonly IExchangeService _exchangeContext;
    private readonly IPlanService _planService;
    private readonly ICoinService _coinService;
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public ExchangeController(IExchangeService exchangeContext, IAuthService authService, IPlanService planService, IUserService userService, ICoinService coinService)
    {
        _exchangeContext = exchangeContext;
        _authService = authService;
        _userService = userService;
        _coinService = coinService;
        _planService = planService;
    }

    [Route("all")]
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_exchangeContext.GetExchanges());
    }

    [HttpGet("{userId}")]
    public ActionResult GetExchanges(int userId, int limit)
    {
        if (_authService.getCurrentUser() == null)
        {
            return Unauthorized(new { error = "You are not logged in" });
        }

        if (!_authService.isSameUserRequest(userId))
        {
            return Unauthorized(new { error = "You are not authorized to see this user's exchanges" });
        }
        
        if (_userService.GetUser(userId) == null)
        {
            return NotFound(new
            {
                error = "User id not found"
            });
        }

        var exchanges = _exchangeContext.GetExchangesFromUser(userId, limit);
        return Ok(exchanges);
    }

    [HttpPost]
    public ActionResult<CoinExchange> PostExchange(ExchangeForCreationDTO exchangeForCreationDto)
    {
        if (_authService.getCurrentUser() == null)
        {
            return Unauthorized(new { error = "You are not logged in" });
        }
        
        if (!_authService.isSameUserRequest(exchangeForCreationDto.UserId))
        {
            return Unauthorized(new { error = "You are not authorized to post exchanges for another user" });
        }
        
        if (_userService.GetUser(exchangeForCreationDto.UserId) == null)
        {
            return NotFound(new
            {
                error = "User id not found"
            });
        }
        
        var user = _userService.GetUser(exchangeForCreationDto.UserId)!;
        
        if (_coinService.GetCoinId(exchangeForCreationDto.ToCoinId) == null)
        {
            return NotFound(new
            {
                error = "To Coin id not found"
            });
        }
        
        if (_coinService.GetCoinId(exchangeForCreationDto.FromCoinId) == null)
        {
            return NotFound(new
            {
                error = "From Coin id not found"
            });
        }
        
        if (exchangeForCreationDto.Amount <= 0)
        {
            return BadRequest(new
            {
                error = "Amount must be greater than 0"
            });
        }
        
        if (_planService.GetPlanId(user.PlanId) == null)
        {
            return BadRequest(new
            {
                error = "User must have an active Plan"
            });
        }
        
        var planLimit = _planService.GetPlanId(user.PlanId)!.Limit;
        var userExchanges = _exchangeContext.GetExchangesFromUser(exchangeForCreationDto.UserId, 0).Count;
        
        if (userExchanges >= planLimit && planLimit != -1)
        {
            return BadRequest(new
            {
                error = "User has reached the limit of exchanges"
            });
        }

        var exchanges = _exchangeContext.CreateExchange(exchangeForCreationDto);
        return Ok(exchanges);
    }

    [Authorize(Roles = "admin")]
    [HttpGet("page/{page}")]
    public ActionResult<CoinExchange> GeExchangesByPage(int page)
    {
        if (page <= 0)
        {
            return BadRequest(new
            {
                error = "Page must be greater than 0"
            });
        }

        var exchanges = _exchangeContext.GetExchangesByPage(page);
        return Ok(exchanges);
    }

    [Authorize(Roles = "admin")]
    [HttpGet("count")]
    public ActionResult<CoinExchange> GetTotalExchanges()
    {
        var count = _exchangeContext.GetTotalExchanges();
        return Ok("Total exchanges available: " + count);
    }
}