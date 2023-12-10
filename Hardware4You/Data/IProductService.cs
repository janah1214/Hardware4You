using Hardware4You.Data;

namespace Hardware4You.Data
{
    interface IProductService
    {
        IEnumerable<Product> GetProducts();
        void InsertProduct(Product product);
        void UpdateProduct(long id, Product product);
        Product SingleProduct(long id);
        void DeleteProduct(long id);
    }
}
