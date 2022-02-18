using System;
using System.IO;
using System.Net;
using System.Net.FtpClient;
using System.Threading.Tasks;
using Garant.Platform.Core.Data;
using Garant.Platform.Core.Logger;
using Garant.Platform.FTP.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Garant.Platform.FTP.Service
{
    /// <summary>
    /// Сервис FTP.
    /// </summary>
    public sealed class FtpService : IFtpService
    {
        private readonly IConfiguration _configuration;
        private readonly PostgreDbContext _postgreDbContext;

        public FtpService(IConfiguration configuration, PostgreDbContext postgreDbContext)
        {
            _configuration = configuration;
            _postgreDbContext = postgreDbContext;
        }

        /// <summary>
        /// Метод загрузит файлы по FTP на сервер.
        /// </summary>
        /// <param name="files">Файлы для отправки.</param>
        public async Task UploadFilesFtpAsync(IFormFileCollection files)
        {
            try
            {
                if (files.Count > 0)
                {
                    var host = _configuration.GetSection("FtpSettings:Host").Value;
                    var login = _configuration.GetSection("FtpSettings:Login").Value;
                    var password = _configuration.GetSection("FtpSettings:Password").Value;
                    var ftp = new FtpClient
                    {
                        Host = host,
                        Credentials = new NetworkCredential(login, password)
                    };

                    ftp.Connect();

                    // Закачать файлы на сервер в папку фронта (изображения).
                    foreach (var file in files)
                    {
                        // Если файлы изображений или видео.
                        if (file.FileName.EndsWith(".png")
                            || file.FileName.EndsWith(".jpg")
                            || file.FileName.EndsWith(".jpeg")
                            || file.FileName.EndsWith(".svg"))
                        {
                            ftp.SetWorkingDirectory("/images");
                        }

                        // Если документы.
                        else if (file.FileName.EndsWith(".docx")
                                 || file.FileName.EndsWith(".xlsx")
                                 || file.FileName.EndsWith(".pdf")
                                 || file.FileName.EndsWith(".pptx")
                                 || file.FileName.EndsWith(".doc")
                                 || file.FileName.EndsWith(".xls"))
                        {
                            ftp.SetWorkingDirectory("/docs");
                        }

                        await using var remote = ftp.OpenWrite(file.FileName, FtpDataType.Binary);
                        await file.CopyToAsync(remote);
                    }

                    ftp.Disconnect();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }

        /// <summary>
        /// Метод скачает файл с сервера по FTP.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <returns>Файл для скачивания фронтом.</returns>
        public async Task<FileContentResult> DownloadFileAsync(string fileName)
        {
            try
            {
                var host = _configuration.GetSection("FtpSettings:Host").Value;
                var login = _configuration.GetSection("FtpSettings:Login").Value;
                var password = _configuration.GetSection("FtpSettings:Password").Value;
                var ftp = new FtpClient
                {
                    Host = host,
                    Credentials = new NetworkCredential(login, password)
                };

                ftp.Connect();
                ftp.SetWorkingDirectory("/docs");
                await using var readStream = ftp.OpenRead(fileName, FtpDataType.Binary);
                
                var byteData = await GetByteArrayAsync(readStream);
                var result = new FileContentResult(byteData, "application/octet-stream");
                ftp.Disconnect();

                return result;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                var logger = new Logger(_postgreDbContext, e.GetType().FullName, e.Message, e.StackTrace);
                await logger.LogError();
                throw;
            }
        }
        
        /// <summary>
        /// Метод получит массив байт из потока.
        /// </summary>
        /// <param name="input">Поток.</param>
        /// <returns>Масив байт.</returns>
        private async Task<byte[]> GetByteArrayAsync(Stream input)
        {
            var buffer = new byte[16*1024];
            await using MemoryStream ms = new MemoryStream();
            var read = 0;
                
            while ((read = await input.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }
                
            return ms.ToArray();
        }
    }
}