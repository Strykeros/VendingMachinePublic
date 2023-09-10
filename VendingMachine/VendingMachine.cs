using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VendingMachine
{
    public class VendingMachine : IVendingMachine
    {
        public Money _totalAmount;
        public string Manufacturer { get; }

        public bool HasProducts { get; set; }

        public Money Amount { get; set; }

        public Product[] Products { get; set; }
        public List<Product> _productList = new List<Product>();
        public Product _selectedProduct;

        public VendingMachine()
        {
            
        }
        
        public bool AddProduct(string name, Money price, int count)
        {
            _productList.Add(new Product { Name = name, Price = price, Available = count });
            Products = _productList.ToArray();
            return true;
        }

        public Money InsertCoin(Money amount)
        {
            Money money = amount;
            Amount = money;
            _totalAmount.Cents += money.Cents;
            _totalAmount.Euros += money.Euros;
            if(_totalAmount.Cents > 99)
            {
                _totalAmount.Euros += 1;
                _totalAmount.Cents = 0;
            }
            return money;
        }

        public Money ReturnMoney()
        {
            int priceInCents = (_selectedProduct.Price.Euros * 100) + _selectedProduct.Price.Cents;
            int moneyInCents = (_totalAmount.Euros * 100) + _totalAmount.Cents;
            int returnableMoneyInCents = moneyInCents - priceInCents;
            Money returnableMoney = new Money();

            if (returnableMoneyInCents > 0)
            {
                returnableMoney.Euros = returnableMoneyInCents / 100;
                returnableMoney.Cents = returnableMoneyInCents % 100;
            }

            _totalAmount.Euros -= returnableMoney.Euros;
            _totalAmount.Cents -= returnableMoney.Cents;

            if (_totalAmount.Cents > 99)
            {
                _totalAmount.Euros += 1;
                _totalAmount.Cents = 0;
            }

            return returnableMoney;
        }

        public bool UpdateProduct(int productNumber, string name, Money? price, int amount)
        {
            Products[productNumber].Name = name;
            Money newPrice = Products[productNumber].Price;
            newPrice.Euros = price.Value.Euros;
            newPrice.Cents = price.Value.Cents;
            Products[productNumber].Price = newPrice;
            Products[productNumber].Available = amount;

            if (Products[productNumber].Available == 0)
            {
                _productList.RemoveAt(productNumber);
                Products = _productList.ToArray();
            }

            return true;
        }

        public Money CheckUserInput()
        {
            string coin = "";
            bool waitingCorrectInput = true;
            string[] allowedCoins = { "0.10", "0.20", "0.50", "1.00", "2.00", "1.0", "2.0" };

            while (waitingCorrectInput)
            {
                string input = Console.ReadLine();

                for (int i = 0; i < allowedCoins.Length; i++)
                {
                    if (input.Contains(allowedCoins[i]))
                    {
                        coin = allowedCoins[i];
                        waitingCorrectInput = false;
                        break;
                    }

                    if (i == allowedCoins.Length - 1)
                        Console.WriteLine("Enter correct input");
                }
            }

            Money outputMoney = FormatToMoney(coin);

            return outputMoney;
        }

        public Money FormatToMoney(string coins)
        {
             Money money = new Money();
            string[] coinArray = coins.Split('.');
            money.Euros += int.Parse(coinArray[0]);
            money.Cents += int.Parse(coinArray[1]);
            return money;
        }

        public string EndTransaction(int productIndex)
        {
            int availProduct = _selectedProduct.Available - 1;
            UpdateProduct(productIndex, _selectedProduct.Name, _selectedProduct.Price, availProduct);
            Money returnedMoney = ReturnMoney();
            string outputStr = "";

            if (returnedMoney.Euros != 0 || returnedMoney.Cents != 0)
            {
                outputStr += $"Do not forget to take cashback!: {returnedMoney.Euros}.{returnedMoney.Cents}";
                Console.WriteLine(outputStr);
            }

            outputStr += "\nEnjoy!";
            Console.WriteLine("Enjoy!");
            return outputStr;
        }
    }
}
