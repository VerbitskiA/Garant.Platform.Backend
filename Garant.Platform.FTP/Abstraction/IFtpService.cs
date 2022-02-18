using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// Метод скачает файл с сервера по FTP.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <returns>Файл для скачивания фронтом.</returns>
        Task<FileContentResult> DownloadFileAsync(string fileName);
    }
}
