using System.Linq;

namespace SportsStore.Models
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext context;

        public ProductRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Product> Products => context.Products;

        public Product Delete(int productId)
        {
            var product = context.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                var entry = context.Products.Remove(product);
                context.SaveChanges();
            }
            return product;
        }

        public void SaveProduct(Product product)
        {
            if (product.ProductID == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                Product dbEntry = context.Products.FirstOrDefault(p => p.ProductID == product.ProductID);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                }
            }
            context.SaveChanges();
        }
    }
}