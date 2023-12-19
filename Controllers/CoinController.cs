using conversor.Data.Interfaces;
using conversor.Entities;
using conversor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace conversor.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class CoinController : ControllerBase
{
    private readonly ICoinService _context;

    public CoinController(ICoinService context)
    {
        _context = context;
    }

    [Route("all")]
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_context.GetCoins());
    }

    [HttpGet("id/{coinId}")]
    public ActionResult<Coin> GetCoin(int coinId)
    {
        if (_context.GetCoinId(coinId) == null)
        {
            return NotFound(new
            {
                error = "Coin id not found"
            });
        }

        var coin = _context.GetCoinId(coinId);
        return Ok(coin);
    }

    [HttpGet("code/{code}")]
    public ActionResult<Coin> GetCoinCode(string code)
    {
        if (_context.GetCoinCode(code) == null)
        {
            return NotFound(new
            {
                error = "Coin code not found"
            });
        }

        var coin = _context.GetCoinCode(code);
        return Ok(coin);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public ActionResult<Coin> PostCoin(CoinForCreationDTO coinForCreationDto)
    {
        if (_context.GetCoinCode(coinForCreationDto.Code) != null)
        {
            return Conflict(new
            {
                error = "Coin code already exists"
            });
        }
        
        if (coinForCreationDto.Value <= 0)
        {
            return Conflict(new
            {
                error = "Coin value must be greater than 0"
            });
        }
        
        if (coinForCreationDto.Code.Length < 3)
        {
            return Conflict(new
            {
                error = "Code code must be greater than 3 characters"
            });
        }
        
        if (coinForCreationDto.Code.Length > 5)
        {
            return Conflict(new
            {
                error = "Coin code must be less than 5 characters"
            });
        }
        
        if (coinForCreationDto.Name.Length < 3)
        {
            return Conflict(new
            {
                error = "Coin name must be higher than 3 characters"
            });
        }
        
        var coin = _context.CreateCoin(coinForCreationDto);
        return Ok(coin);
    }

    [HttpPut]
    [Authorize(Roles = "admin")]
    public ActionResult<Coin> PutCoin(CoinForUpdateDTO coinForUpdateDto)
    {
        if (_context.GetCoinId(coinForUpdateDto.CoinId) == null)
        {
            return NotFound(new
            {
                error = "Coin id not found"
            });
        }

        _context.UpdateCoin(coinForUpdateDto);
        return Ok("Coin updated successfully");
    }

    [HttpDelete("{coinId}")]
    [Authorize(Roles = "admin")]
    public ActionResult<Coin> DeleteCoin(int coinId)
    {
        if (_context.GetCoinId(coinId) == null)
        {
            return NotFound(new
            {
                error = "Coin id not found"
            });
        }

        _context.DeleteCoin(coinId);
        return Ok("Coin deleted successfully");
    }
    
    [HttpGet("page/{page}")]
    public ActionResult<Coin> GetCoinsByPage(int page)
    {
        if (page <= 0)
        {
            return BadRequest(new
            {
                error = "Page must be greater than 0"
            });
        }
        
        if (page != (int)page)
        {
            return BadRequest(new
            {
                error = "Page must be an integer"
            });
        }
        
        var coins = _context.GetCoinsByPage(page);
        return Ok(coins);
    }

    [HttpGet("count")]
    public ActionResult<Coin> GetTotalCoins()
    {
        int count = _context.GetTotalCoins();
        return Ok("Total coins available: " + count);
    }
}