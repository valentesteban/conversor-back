using conversor.Data.Interfaces;
using conversor.Entities;
using conversor.Models;
using Microsoft.EntityFrameworkCore;

namespace conversor.Data.Implementations;

public class ExchangeService : IExchangeService
{
    private readonly ConversorContext _context;

    public ExchangeService(ConversorContext context)
    {
        _context = context;
    }

    public List<CoinExchange> GetExchanges()
    {
        return _context.CoinExchange
            .Include((exchange) => exchange.FromCoin)
            .Include((exchange) => exchange.ToCoin)
            .ToList();
    }

    public List<CoinExchange> GetExchangesFromUser(int userId, int limit)
    {
        if (limit == 0)
        {
            return _context.CoinExchange.Where((exchange) => exchange.UserId == userId)
                .Include((exchange) => exchange.FromCoin)
                .Include((exchange) => exchange.ToCoin).ToList();
        }

        return _context.CoinExchange.Where((exchange) => exchange.UserId == userId)
            .Include((exchange) => exchange.FromCoin)
            .Include((exchange) => exchange.ToCoin).Take(limit).ToList();
    }

    public CoinExchange CreateExchange(ExchangeForCreationDTO exchangeForCreationDto)
    {
        var user = _context.Users.FirstOrDefault((user) => user.Id == exchangeForCreationDto.UserId)!;
        var temporalUserCurrencyList =
            _context.CoinExchange.Where((exchange) => exchange.UserId == user.Id).ToList();

        var fromCoin =
            _context.CoinExchange.FirstOrDefault((exchange) => exchange.Id == exchangeForCreationDto.FromCurrencyId)!;
        var toCoin =
            _context.CoinExchange.FirstOrDefault((exchange) => exchange.Id == exchangeForCreationDto.ToCurrencyId)!;

        CoinExchange exchange = new()
        {
            Amount = exchangeForCreationDto.Amount,
            Date = DateTime.Now,
            FromCoin = fromCoin,
            ToCoin = toCoin
        };

        temporalUserCurrencyList.Add(exchange);
        user.Exchanges = temporalUserCurrencyList;
        _context.SaveChanges();

        return exchange;
    }

    public List<CoinExchange> GeExchangesByPage(int page)
    {
        return _context.CoinExchange
            .Include((exchange) => exchange.FromCoin)
            .Include((exchange) => exchange.ToCoin)
            .Skip((page - 1) * 10)
            .Take(10)
            .ToList();
    }
    
    public int GetTotalExchanges()
    {
        return _context.CoinExchange.Count();
    }
}