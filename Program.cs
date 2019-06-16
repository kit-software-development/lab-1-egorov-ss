using System;
using System.IO;

namespace Events
{
    class Program
    {
        internal Product Product
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
                throw new System.NotImplementedException();
            }
        }

        static void Main(string[] args)
        {
            var product = new Product("Some product name", 0);

            /* 
             * TODO #6 Назначить обработчики событий в текущем контексте 
             */
            Product.OnNameChanged  += OnProductNameChanged;
            Product.OnPriceChanged += OnProductPriceChanged;

            /*
             * TODO #7 Выполнить с экземпляром класса Product действия, 
             * приводящие к возникновению описанных Вами событий
             */
             product.Name  = "Plumbus";
             product.Price = 911;

             product.Name  = "Baldezh";
             product.Price = 1729;
             product.Price = 2019;
        }

        /* 
         * TODO #8 Добавить определение обработчиков событий 
         */
        static void OnProductNameChanged(object sender, ProductEventArgs e)
        {
            var product = sender as Product;
            Console.WriteLine(
                  $"The product \"{e.OldName}\" "
                + $"is now called \"{product.Name}\"."
            );
        }
        static void OnProductPriceChanged(object sender, ProductEventArgs e)
        {
            var product = sender as Product;
            Console.WriteLine(
                  $"The \"{product.Name}\" product's price has changed "
                + $"from {e.OldPrice}$ to {product.Price}$."
            );
        }

    }
}
