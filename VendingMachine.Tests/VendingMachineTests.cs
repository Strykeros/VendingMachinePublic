using FluentAssertions;

namespace VendingMachine.Tests
{
    [TestClass]
    public class VendingMachineTests
    {
        private VendingMachine _machine;

        [TestInitialize]
        public void Setup()
        {
            _machine = new VendingMachine();
        }

        [TestMethod] 
        public void AddProduct_Product_ProductAddedToMachine()
        {
            Money money = new Money();
            money.Euros = 2;
            money.Cents = 50;
            _machine.AddProduct("Snickers", money, 20);

            _machine.Products.Length.Should().Be(1);
        }

        [TestMethod]
        public void InsertCoin_Money_InsertedMoney()
        {
            Money money = new Money();
            money.Euros = 0;
            money.Cents = 50;

            _machine.InsertCoin(money);
            _machine.Amount.Euros.Should().Be(0);
            _machine.Amount.Cents.Should().Be(50);
        }

        [TestMethod]
        public void ReturnMoney_Money_MoneyIsReturnedToUser()
        {
            ProductManager productManager = new ProductManager(_machine);
            productManager.AddProducts();
            Money money = new Money();
            Money returnableMoney = new Money();
            money.Euros = 3;
            money.Cents = 0;

            _machine._selectedProduct = _machine.Products[1];

            _machine.InsertCoin(money);
            returnableMoney = _machine.ReturnMoney();

            returnableMoney.Euros.Should().Be(0);
            returnableMoney.Cents.Should().Be(50);
        }

        [TestMethod]
        public void UpdateProduct_Product_ProductListUpdated()
        {
            Money price = new Money();
            price.Euros = 2;
            price.Cents = 0;
            _machine.AddProduct("Coke", price, 30);

            Money newPrice = new Money();
            newPrice.Euros = 3;
            newPrice.Cents = 50;

            _machine.UpdateProduct(0, "Coke", newPrice, 40);

            _machine.Products[0].Name.Should().Be("Coke");
            _machine.Products[0].Price.Euros.Should().Be(newPrice.Euros);
            _machine.Products[0].Price.Cents.Should().Be(newPrice.Cents);
            _machine.Products[0].Available.Should().Be(40);
        }

        [TestMethod]
        public void CheckUserInput_ValidInput_ReturnsMoney()
        {
            string input = "1.00" + Environment.NewLine;
            Money expectedMoney = new Money { Euros = 1, Cents = 0 };

            TextReader originalInput = Console.In;
            StringReader stringReader = new StringReader(input);
            Console.SetIn(stringReader);

            Money result = _machine.CheckUserInput();

            Assert.AreEqual(expectedMoney.Euros, result.Euros);
            Assert.AreEqual(expectedMoney.Cents, result.Cents);

            Console.SetIn(originalInput);
        }

        [TestMethod]
        public void FormatToMoney_MoneyString_ReturnsMoneyStruct()
        {
            string inputMoney = "1.50";
            Money formatedMoney = _machine.FormatToMoney(inputMoney);

            formatedMoney.Euros.Should().Be(1);
            formatedMoney.Cents.Should().Be(50);
        }

        [TestMethod]
        public void EndTransaction_ProductIndex_UpdatesProductAndReturnsCashback()
        {
            ProductManager productManager = new ProductManager(_machine);
            productManager.AddProducts();
            Money money = new Money();
            money.Euros = 2;
            money.Cents = 0;

            string transactionEnd = _machine.EndTransaction(3);
            string expectedResult = "\nEnjoy!";

            transactionEnd.Should().Be(expectedResult);
        }
    }
}
