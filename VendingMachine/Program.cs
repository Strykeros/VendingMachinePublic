using System;
using System.Net.WebSockets;
using System.Text;

namespace VendingMachine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StartVendingMachine();
        }

        private static void StartVendingMachine()
        {
            VendingMachine machine = new VendingMachine();
            ProductManager productManager = new ProductManager(machine);
            productManager.AddProducts();

            while (true)
            {
                Console.WriteLine("============== Vending Machine ======================");
                Console.WriteLine();
                productManager.ListProducts();
                productManager.SelectProduct();  
            }
        }
    }
}
