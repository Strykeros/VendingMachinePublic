using FluentAssertions;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Tests
{
    [TestClass]
    public class ProductManagerTests
    {
        private ProductManager _manager;
        private VendingMachine _machine;

        [TestInitialize]
        public void Setup()
        {

            _machine = new VendingMachine();
            _manager = new ProductManager(_machine);
        }

        [TestMethod]
        public void AddProducts_Products_ProductsAddedToArray()
        {
            bool productsAdded = _manager.AddProducts();
            productsAdded.Should().BeTrue();
        }

        [TestMethod]
        public void ListProducts_Products_ProductsPrinted()
        {
            _manager.AddProducts();

            string expectedOutput = "1. snickers, Price - €1.0, 20 left.\n2. m&m's, Price - €2.50, 1 left.\n3. coke, Price - €2.0, 10 left.\n4. fanta, Price - €1.80, 10 left.\n";

            TextWriter originalOutput = Console.Out;
            using (StringWriter stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);
                string result = _manager.ListProducts();
                Assert.AreEqual(expectedOutput, result);
            }
            Console.SetOut(originalOutput);
        }

        [TestMethod]
        public void GetMoney_SelectedProduct_AmountForProduct()
        {
            _manager.AddProducts();
            _machine._totalAmount.Euros = 1;
            _machine._totalAmount.Cents = 80;
            double money = _manager.GetMoney('3', true);

            money.Should().Be(1.80);
        }

        [TestMethod]
        public void StopProductManager_StopCall_VendingMachineStopped()
        {
            int exit = _manager.StopProductManager(true);
            exit.Should().Be(0);
        }
    }
}
