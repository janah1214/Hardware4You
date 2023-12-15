using Hardware4You.Models;
using MudBlazor;
using Syncfusion.Blazor.Popups;

namespace Hardware4You.Data
{
    public class CartService
    {
        public List<Product> SelectedItems { get; set; } = new();

        public void AddProductToCart(long productId, List<Product> products)
        {
            var product = products.First(p => p.Id == productId);

            // TODO: Wenn anz pro kategorie haben möchte
            //if (SelectedItems.Contains(product) is false)
            //{
                SelectedItems.Add(product);
            //}
        }
    }
}
