using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Garant.Platform.FTP.Abstraction
{
    /// <summary>
    /// Абстракция FTP сервиса.
    /// </summary>
    public interface IFtpService
    {
        /// <summary>
        /// Метод загрузит файлы по FTP на сервер.
        /// </summary>
        /// <param name="files">Файлы для отправки.</param>
        Task UploadFilesFtpAsync(IFormFileCollection files);
    }
}
