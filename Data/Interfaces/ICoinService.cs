using conversor.Entities;
using conversor.Models;

namespace conversor.Data.Interfaces;

public interface ICoinService
{
    public List<Coin> GetCoins();
    public Coin? GetCoinId(int id);
    public Coin? GetCoinCode(string code);
    public Coin CreateCoin(CoinForCreationDTO currencyForCreationDto);
    public void UpdateCoin(CoinForUpdateDTO currencyForUpdateDto);
    public void DeleteCoin(int currencyId);
    public List<Coin> GetCoinsByPage(int page);
    public int GetTotalCoins();
}