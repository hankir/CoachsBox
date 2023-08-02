using System.Collections.Generic;
using CoachsBox.Core.Primitives;

namespace CoachsBox.Coaching.Services
{
  /// <summary>
  /// Сервис поиска адреса по названию места проведения.
  /// Например, при вводе Школа 49, выдаст адрес Труда 18, г. Ижевск.
  /// Плюс список всех других адресов совпадающих с условием.
  /// TODO: Попробовать https://dadata.ru/suggestions/usage/address
  /// </summary>
  /// <remarks>Сервис интеграции с поискавиками.</remarks>
  public interface ILocationService
  {
    IReadOnlyList<Address> FindByName(string name);
  }
}
