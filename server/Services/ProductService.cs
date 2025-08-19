using Server.DTOs;
using Server.Models;
using Server.Repositories;

namespace Server.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(MapToProductResponseDto);
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product != null ? MapToProductResponseDto(product) : null;
        }

        public async Task<ProductResponseDto?> GetProductByCodeAsync(string code)
        {
            var product = await _productRepository.GetByCodeAsync(code);
            return product != null ? MapToProductResponseDto(product) : null;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetProductsByCategoryAsync(string category)
        {
            var products = await _productRepository.GetByCategoryAsync(category);
            return products.Select(MapToProductResponseDto);
        }

        public async Task<IEnumerable<ProductResponseDto>> GetProductsByLocationAsync(string location)
        {
            var products = await _productRepository.GetAllAsync();
            var filtered = products.Where(p => p.Location.Contains(location, StringComparison.OrdinalIgnoreCase));
            return filtered.Select(MapToProductResponseDto);
        }

        public async Task<ProductResponseDto> CreateProductAsync(ProductCreateDto productDto)
        {
            var product = new Product
            {
                Code = productDto.Code,
                Name = productDto.Name,
                Category = productDto.Category,
                Quantity = productDto.Quantity,
                Location = productDto.Location,
                RegistrationDate = DateTime.UtcNow
            };

            var createdProduct = await _productRepository.CreateAsync(product);
            return MapToProductResponseDto(createdProduct);
        }

        public async Task<ProductResponseDto?> UpdateProductAsync(int id, ProductUpdateDto productDto)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                return null;

            if (!string.IsNullOrEmpty(productDto.Code))
                existingProduct.Code = productDto.Code;
            if (!string.IsNullOrEmpty(productDto.Name))
                existingProduct.Name = productDto.Name;
            if (!string.IsNullOrEmpty(productDto.Category))
                existingProduct.Category = productDto.Category;
            if (productDto.Quantity.HasValue)
                existingProduct.Quantity = productDto.Quantity.Value;
            if (!string.IsNullOrEmpty(productDto.Location))
                existingProduct.Location = productDto.Location;

            var updatedProduct = await _productRepository.UpdateAsync(id, existingProduct);
            return updatedProduct != null ? MapToProductResponseDto(updatedProduct) : null;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _productRepository.DeleteAsync(id);
        }

        private static ProductResponseDto MapToProductResponseDto(Product product)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                Category = product.Category,
                Quantity = product.Quantity,
                Location = product.Location,
                RegistrationDate = product.RegistrationDate
            };
        }
    }
}
