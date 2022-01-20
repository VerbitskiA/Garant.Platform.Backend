using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Garant.Platform.Models.Mailing.Output;

namespace Garant.Platform.Base.Abstraction
{
    /// <summary>
    /// Абстракция общего сервиса.
    /// </summary>
    public interface ICommonService
    {
        /// <summary>
        /// Метод создает код. Кол-во цифр зависит от переданного типа.
        /// </summary>
        /// <param name="data">Телефон или почта.</param>
        /// <returns>Флаг успеха.</returns>
        Task<MailngOutput> GenerateAcceptCodeAsync(string data);

        /// <summary>
        /// Метод хэширует пароль аналогично как это делает Identity.
        /// </summary>
        /// <param name="password">Исходный пароль без хэша.</param>
        /// <returns>Хэш пароля.</returns>
        Task<string> HashPasswordAsync(string password);

        /// <summary>
        /// Метод вычислит кол-во месяцев между датами.
        /// </summary>
        /// <param name="start">Дата начала.</param>
        /// <param name="end">Текущая дата.</param>
        /// <returns>Кол-во месяцев округленное до целого.</returns>
        Task<double> GetSubtractMonthAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Метод соединит в строку из массива строк разделяя запятой.
        /// </summary>
        /// <param name="arr">Исходный массив строк.</param>
        /// <returns>Строка со значениями массива.</returns>
        Task<string> JoinArrayWithDelimeterAsync(string[] arr);

        /// <summary>
        /// Метод преобразует рубли в копейки.
        /// </summary>
        /// <param name="amount">Сумма в руб.</param>
        /// <returns>Сумма в копейках.</returns>
        Task<double> ConvertRubToPennyAsync(double amount);

        /// <summary>
        /// Метод хэширует строку по SHA-256.
        /// </summary>
        /// <param name="str">Исходная строка с параметрами, которые конкантенированы.</param>
        /// <returns>Измененная строку по SHA-256.</returns>
        Task<string> HashSha256Async(Dictionary<string, object> hashValues);
    }
}
