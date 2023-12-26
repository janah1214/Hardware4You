using Hardware4You.Models;
using MudBlazor;
using Syncfusion.Blazor.Popups;

namespace Hardware4You.Data.ShoppingCart
{
    public class CartService
    {
        public List<Product> SelectedItems { get; set; } = new();

        public void AddProductToCart(long productId, List<Product> products)
        {
            var product = products.First(p => p.Id == productId);
            SelectedItems.Add(product);
        }

        public void BuyProductInCart(Product product)
        {
            product.Quantity -= 1;
        }
    }
}
