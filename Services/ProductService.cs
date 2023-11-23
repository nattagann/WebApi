using CShapTestWebApi.Entities;
using CShapTestWebApi.Models;

namespace CShapTestWebApi.Services
{
    public class ProductService
    {
        private readonly List<Product> _products;
        private int _lastProductId;

        public ProductService(List<Product> initialProducts)
        {
            _products = initialProducts ?? new List<Product>();
            _lastProductId = _products.Count;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _products;
        }

        public ProductModel GetProductById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return null;
            }

            var productModel = new ProductModel
            {
                // Copy properties from Product to ProductModel
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
            };

            return productModel;
        }

        public int AddProduct(CreateProductModel newProduct)
        {
            _lastProductId++;
            Product product = new Product
            {
                Id = _lastProductId,
                Name = newProduct.Name,
                Description = newProduct.Description,
                Price = newProduct.Price
            };
            _products.Add(product);
            return _lastProductId;
        }

        public void UpdateProduct(int id, UpdateProductModel updatedProduct)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == id);

            if (existingProduct != null)
            {
                existingProduct.Name = updatedProduct.Name;
                existingProduct.Description = updatedProduct.Description;
                existingProduct.Price = updatedProduct.Price;
            }
            else
            {
                throw new ArgumentException("Product not found");
            }
        }

        public void DeleteProduct(int id)
        {
            var productToRemove = _products.FirstOrDefault(p => p.Id == id);

            if (productToRemove != null)
            {
                _products.Remove(productToRemove);
            }
            else
            {
                throw new ArgumentException("Product not found");
            }
        }
    }
}
