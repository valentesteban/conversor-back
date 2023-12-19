using conversor.Data.Interfaces;
using conversor.Entities;
using conversor.Models;

namespace conversor.Data.Implementations;

public class CoinService : ICoinService
{
    private readonly ConversorContext _context;

    public CoinService(ConversorContext context)
    {
        _context = context;
    }

    public List<Coin> GetCoins()
    {
        return _context.Coin.ToList();
    }

    public Coin? GetCoinId(int id)
    {
        return _context.Coin.FirstOrDefault((coin) => coin.Id == id);
    }

    public Coin? GetCoinCode(string code)
    {
        return _context.Coin.FirstOrDefault((coin) => coin.Code.ToLower() == code.ToLower());
    }

    public Coin CreateCoin(CoinForCreationDTO coinForCreationDto)
    {
        Coin coin = new()
        {
            Name = coinForCreationDto.Name,
            Value = coinForCreationDto.Value,
            Code = coinForCreationDto.Code,
        };

        var newCoin = _context.Coin.Add(coin);
        _context.SaveChanges();

        return newCoin.Entity;
    }

    public void UpdateCoin(CoinForUpdateDTO coinForUpdateDto)
    {
        var toChange = GetCoinId(coinForUpdateDto.CoinId)!;

        toChange.Name = coinForUpdateDto.Name;
        toChange.Value = coinForUpdateDto.Value;
        toChange.Code = coinForUpdateDto.Code;

        _context.Coin.Update(toChange);
        _context.SaveChanges();
    }

    public void DeleteCoin(int coinId)
    {
        var toRemove = GetCoinId(coinId)!;

        _context.Coin.Remove(toRemove);
        _context.SaveChanges();
    }
    
    public List<Coin> GetCoinsByPage(int page)
    {
        return _context.Coin.Skip((page - 1) * 10).Take(10).ToList();
    }

    public int GetTotalCoins()
    {
        return _context.Coin.Count();
    }
}