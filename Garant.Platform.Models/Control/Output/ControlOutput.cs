using System;

namespace Garant.Platform.Models.Control.Output
{
    /// <summary>
    /// Класс выходной модели контролов.
    /// </summary>
    public class ControlOutput
    {
        /// <summary>
        /// Тип контрола.
        /// </summary>
        public string ControlType { get; set; }

        /// <summary>
        /// Название контрола.
        /// </summary>
        public string ControlName { get; set; }

        /// <summary>
        /// Код Guid.
        /// </summary>
        public Guid ValueId { get; set; }

        /// <summary>
        /// Значение.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Позиция в списке.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Выбрано ли по дефолту.
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
