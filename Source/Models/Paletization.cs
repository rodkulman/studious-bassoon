namespace Rodkulman.MilkMafia.Models
{
    public class Paletization
    {
        #region TableMapping
            public int ProductId { get; set; }
            public int BoxQuantity { get; set; }
            public int BoxLayerQuantity { get; set; }
            public int LayerPalletQuantity { get; set; }
        #endregion

        #region Relations
            public Product Product { get; set; }
        #endregion
    }
}