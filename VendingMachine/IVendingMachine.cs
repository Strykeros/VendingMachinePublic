namespace VendingMachine
{
    public interface IVendingMachine
    {
        string Manufacturer { get; }

        bool HasProducts { get; }

        Money Amount { get; }

        Product[] Products { get; }

        /// <summary>
        /// in case of valid coins returns 0.00.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        Money InsertCoin(Money amount);

        Money ReturnMoney();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <param name="count"></param>
        /// <returns>if product add was successful</returns>
        bool AddProduct(string name, Money price, int count);

        bool UpdateProduct(int productNumber, string name, Money? price, int amount);
    }
}
