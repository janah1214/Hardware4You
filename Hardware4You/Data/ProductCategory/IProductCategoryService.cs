using Hardware4You.Models;

namespace Hardware4You.Data
{
    interface IProductCategoryService
    {
        IEnumerable<ProductCategory> GetProductCategories();
        void InsertProductCategory(ProductCategory productCategory);
        void UpdateProductCategory(long id, ProductCategory productCategory);
        ProductCategory SingleProductCategory(long id);
        void DeleteProductCategory(long id);
    }
}
