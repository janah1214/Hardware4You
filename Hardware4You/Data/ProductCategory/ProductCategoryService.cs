using Hardware4You.Models;
using Microsoft.EntityFrameworkCore;

namespace Hardware4You.Data
{
    public class ProductCategoryService : IProductCategoryService
    {
        private ProductCategoryContext _context;

        public ProductCategoryService(ProductCategoryContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductCategory> GetProductCategories()
        {
            try
            {
                return _context.ProductCategories.ToList();
            }
            catch
            {
                throw;
            }
        }

        public void DeleteProductCategory(long id)
        {
            try
            {
                ProductCategory productCategory = _context.ProductCategories.Find(id);
                _context.ProductCategories.Remove(productCategory);
                _context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void InsertProductCategory(ProductCategory productCategory)
        {
            try
            {
                productCategory.Id = _context.ProductCategories.Count() + 1;

                _context.ProductCategories.Add(productCategory);
                _context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public ProductCategory SingleProductCategory(long id)
        {
            throw new NotImplementedException();
        }

        public void UpdateProductCategory(long id, ProductCategory productCategories)
        {
            try
            {
                var local = _context.Set<ProductCategory>().Local.FirstOrDefault(entry => entry.Id.Equals(productCategories.Id));

                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }
                _context.Entry(productCategories).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}
