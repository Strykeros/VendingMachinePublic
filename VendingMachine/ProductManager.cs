using System;
using System.Text;

namespace VendingMachine
{
    public class ProductManager
    {
        public VendingMachine _vendingMachine;
        public ProductManager(VendingMachine vendingMachine)
        {
            _vendingMachine = vendingMachine;

        }

        public bool AddProducts()
        {
            string[] names = { "snickers", "m&m's", "coke", "fanta" };
            bool productsAdded = false;
            Money[] prices = new Money[]
            {
                new Money { Euros = 1, Cents = 0 },
                new Money { Euros = 2, Cents = 50 },
                new Money { Euros = 2, Cents = 0 },
                new Money { Euros = 1, Cents = 80 }
            };
            int[] amounts = { 20, 1, 10, 10 };

            for (int i = 0; i < names.Length; i++)
            {
                _vendingMachine.AddProduct(names[i], prices[i], amounts[i]);
            }

            productsAdded = true;
            return productsAdded;
        }

        public string ListProducts()
        {
            Console.OutputEncoding = Encoding.UTF8;
            int no = 1;
            StringBuilder products = new StringBuilder();

            for (int i = 0; i < _vendingMachine.Products.Length; i++)
            {
                products.Append($"{no}. {_vendingMachine.Products[i].Name}, Price - \u20AC{_vendingMachine.Products[i].Price.Euros}.{_vendingMachine.Products[i].Price.Cents}, {_vendingMachine.Products[i].Available} left.\n");
                no++;
            }

            Console.WriteLine(products.ToString());
            return products.ToString();
        }


        public string SelectProduct()
        {
            bool productIsSelected = false;
            string keyOutp = "";

            while (!productIsSelected)
            {
                Console.WriteLine($"Select a product from 1 to {_vendingMachine.Products.Length} (to turn off the vending machine, press 0):");
                ConsoleKeyInfo key = Console.ReadKey();
                char pressedKey = key.KeyChar;
                keyOutp = pressedKey.ToString();

                if (int.Parse(pressedKey.ToString()) == 0)
                    StopProductManager(false);

                GetMoney(pressedKey, false);
                productIsSelected = true;
            }

            return keyOutp;
        }

        public double GetMoney(char selectedProductChar, bool test)
        {
            int productNumber = int.Parse(selectedProductChar.ToString());
            int productIndex = productNumber == 1 ? 0 : productNumber - 1;
            _vendingMachine._selectedProduct = _vendingMachine.Products[productIndex];

            string combinedPriceStr = _vendingMachine._selectedProduct.Price.Euros.ToString() + "." + _vendingMachine._selectedProduct.Price.Cents.ToString();
            double totalProductPrice = double.Parse(combinedPriceStr);

            string insertedMoneyStr = _vendingMachine._totalAmount.Euros.ToString() + "." + _vendingMachine._totalAmount.Cents.ToString();
            double insertedMoney = double.Parse(insertedMoneyStr);
            string combinedMoneyStr;

            while (insertedMoney < totalProductPrice && !test)
            {
                Console.WriteLine();
                Console.WriteLine("Insert coin:");
                _vendingMachine.InsertCoin(_vendingMachine.CheckUserInput());
                combinedMoneyStr = _vendingMachine._totalAmount.Euros.ToString() + "." + _vendingMachine._totalAmount.Cents.ToString();
                insertedMoney = double.Parse(combinedMoneyStr);
            }

            _vendingMachine.EndTransaction(productIndex);
            return insertedMoney;
        }

        public int StopProductManager(bool test)
        {
            Console.WriteLine();
            Console.WriteLine("Bye!");

            if (test)
                return 0;
            else
                Environment.Exit(0);
            return 0;
        }
    }
}
