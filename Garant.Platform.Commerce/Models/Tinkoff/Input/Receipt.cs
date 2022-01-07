using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Garant.Platform.Core.Attributes;

namespace Garant.Platform.Commerce.Models.Tinkoff.Input
{
    /// <summary>
    /// Класс описывает массив данных чека.
    /// </summary>
    public class Receipt
    {
        /// <summary>
        /// Электронная почта покупателя.
        /// </summary>
        [Required]
        [ConditionalValidation(nameof(Phone), true)]
        [MaxLength(64)]
        public string Email { get; set; }

        /// <summary>
        /// Телефон покупателя.
        /// </summary>
        [Required]
        [ConditionalValidation(nameof(Email), true)]
        [MaxLength(64)]
        public string Phone { get; set; }

        /// <summary>
        /// Электронная почта продавца.
        /// </summary>
        [MaxLength(64)]
        public string EmailCompany { get; set; } = "gobizy@mail.ru";

        /// <summary>
        /// Система налогообложения.
        /// </summary>
        [Required]
        public string Taxation { get; set; }

        /// <summary>
        /// Массив позиций чека с информацией о товарах.
        /// </summary>
        public List<Item> Items { get; set; }
    }

    /// <summary>
    /// Класс описывает структуру позиции чека.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Наименование товара.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Цена за единицу товара в копейках.
        /// </summary>
        [Required]
        [MaxLength(10)]
        public double Price { get; set; }

        /// <summary>
        /// Количество или вес товара.
        /// </summary>
        [Required]
        [MaxLength(8)]
        public double Quantity { get; set; } = 1;

        /// <summary>
        /// Стоимость товара в копейках.
        /// </summary>
        [Required]
        [MaxLength(10)]
        public double Amount { get; set; }

        /// <summary>
        /// Признак способа расчета. Если не передано, то по дефолту ставится full_payment.
        /// </summary>
        public string PaymentMethod { get; set; } = "full_payment";

        /// <summary>
        /// Признак предмета расчета. По дефолту платеж.
        /// </summary>
        public string PaymentObject { get; set; } = "payment";

        /// <summary>
        /// Ставка НДС. По дефолту без НДС.
        /// </summary>
        [Required]
        public string Tax { get; set; } = "none";
    }
}
