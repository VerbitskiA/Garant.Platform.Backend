using System.ComponentModel.DataAnnotations;

namespace Garant.Platform.Messaging.Models.Chat.Output
{
    /// <summary>
    /// Класс выходной модели для диалога.
    /// </summary>
    public class DialogOutput
    {
        /// <summary>
        /// Id диалога.
        /// </summary>
        public long DialogId { get; set; }

        /// <summary>
        /// Название диалога (фамилия и имя с кем ведется переписка).
        /// </summary>
        public string DialogName { get; set; }

        /// <summary>
        /// Последнее сообщение в кратком виде для отображения в сокращенном виде диалога.
        /// </summary>
        [MaxLength(40)]
        public string LastMessage { get; set; }

        /// <summary>
        /// Имя собеседника.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия собеседника.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Фото предмета обсуждения.
        /// </summary>
        public string ChatItemIcon { get; set; }

        /// <summary>
        /// Вычисляемое время для диалогов.
        /// </summary>
        public string CalcTime { get; set; }

        /// <summary>
        /// Вычисляемая дата для диалогов.
        /// </summary>
        public string CalcShortDate { get; set; }

        /// <summary>
        /// Полная дата.
        /// </summary>
        public string Created { get; set; }

        /// <summary>
        /// Id пользователя, который есть в диалоге.
        /// </summary>
        //[JsonPropertyName("Id")]
        public string UserId { get; set; }

        /// <summary>
        /// Тип диалога на основании типа отображается изображение франшизы или бизнеса в диалоге.
        /// </summary>
        public string DialogType { get; set; }

        /// <summary>
        /// Имя + фамилия.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Путь к изображению предмета диалога.
        /// </summary>
        public string Url { get; set; }
    }
}
