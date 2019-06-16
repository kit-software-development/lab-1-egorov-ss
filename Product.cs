using System;

namespace Events
{
    /// <summary>
    /// Класс должен описывать представление о товаре. 
    /// В рамках лабораторной работы должен являться 
    /// источником события
    /// </summary>
    class Product
    {

        #region Variables
        /// <summary>
        /// Наименование
        /// </summary>
        private string name;
        /// <summary>
        /// Стоимость
        /// </summary>
        private decimal price;

        #endregion

        #region Properties

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                string oldName = name;
                name = value;
                /* 
                 * TODO #4 Инициировать уведомление об 
                 * изменении наименования
                 */
                OnNameChanged?.Invoke(this, new ProductEventArgs(oldName));
            }
        }
        /// <summary>
        /// Стоимость
        /// </summary>
        public decimal Price
        {
            get { return price; }
            set
            {
                decimal oldPrice = price;
                price = value;
                /*
                 * TODO #5 Инициировать уведомление об 
                 * изменении стоимости
                 */
                OnPriceChanged?.Invoke(this, new ProductEventArgs(oldPrice));
            }
        }

        #endregion

        #region Events

        /* 
         * TODO #3 Добавить определение событий
         */
        public static event EventHandler<ProductEventArgs> OnNameChanged;
        public static event EventHandler<ProductEventArgs> OnPriceChanged;

        #endregion

        public Product(string name, decimal price)
        {
            Name  = name;
            Price = price;
        }

    }
}
