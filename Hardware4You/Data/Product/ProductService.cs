using Hardware4You.Models;
using Microsoft.EntityFrameworkCore;

namespace Hardware4You.Data
{
    public class ProductService : IProductService
    {
        private ProductContext _context;

        public ProductService(ProductContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetProducts()
        {
            try
            {
                return _context.Products.ToList();
            }
            catch
            {
                throw;
            }
        }

        public void DeleteProduct(long id)
        {
            try
            {
                Product product = _context.Products.Find(id);
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public void InsertProduct(Product product)
        {
            try
            {
                //TODO code nicer
                product.Id = _context.Products.Count() + 1;

                _context.Products.Add(product);
                _context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public Product SingleProduct(long id)
        {
            throw new NotImplementedException();
        }

        public void UpdateProduct(long id, Product product)
        {
            try
            {
                var local = _context.Set<Product>().Local.FirstOrDefault(entry => entry.Id.Equals(product.Id));

                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }
                _context.Entry(product).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}
