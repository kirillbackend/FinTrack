﻿using FinTrack.Services.Dtos;

namespace FinTrack.Services.Contracts
{
    public interface ICurrencyService
    {
        Task<CurrencyDto> GetCurrencyById(Guid id);
        Task<IEnumerable<CurrencyDto>> GetCurrencies();
        Task AddCurrency(CurrencyDto currencyDto);
        Task Delete(Guid id);
        Task<CurrencyDto> Update(CurrencyDto currencyDto);
        Task<decimal> ConvertCurrency(string to, string from, string amount);
        Task ProduceAsync(string message);
        Task<string> StartAsync();
    }
}
