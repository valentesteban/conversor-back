using conversor.Entities;
using conversor.Models;

namespace conversor.Data.Interfaces;

public interface IExchangeService
{
    public List<CoinExchange> GetExchanges();
    public List<CoinExchange> GetExchangesFromUser(int userId, int limit);
    public CoinExchange CreateExchange(ExchangeForCreationDTO exchangeForCreationDto);
    public List<CoinExchange> GetExchangesByPage(int page);
    public int GetTotalExchanges();
}