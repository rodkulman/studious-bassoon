namespace Rodkulman.MilkMafia.Models
{
    public class ProductQuantity
    {
        #region TableMapping
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public double Price { get; set; }
        #endregion        

        #region Relations
            public Product Product { get; set; }
        #endregion
    }
}