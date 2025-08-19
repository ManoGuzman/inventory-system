using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Server.Repositories;
using Server.Models;
using Server.DTOs;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductResponseDto>>>> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();
            var result = products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Category = p.Category,
                Quantity = p.Quantity,
                Location = p.Location,
                RegistrationDate = p.RegistrationDate
            });

            return Ok(new ApiResponse<IEnumerable<ProductResponseDto>>
            {
                Success = true,
                Data = result,
                Message = "Products retrieved successfully"
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductResponseDto>>> GetProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound(new ApiResponse<ProductResponseDto>
                {
                    Success = false,
                    Message = "Product not found"
                });

            var result = new ProductResponseDto
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                Category = product.Category,
                Quantity = product.Quantity,
                Location = product.Location,
                RegistrationDate = product.RegistrationDate
            };

            return Ok(new ApiResponse<ProductResponseDto>
            {
                Success = true,
                Data = result,
                Message = "Product retrieved successfully"
            });
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<ApiResponse<ProductResponseDto>>> GetProductByCode(string code)
        {
            var product = await _productRepository.GetByCodeAsync(code);
            if (product == null)
                return NotFound(new ApiResponse<ProductResponseDto>
                {
                    Success = false,
                    Message = "Product not found"
                });

            var result = new ProductResponseDto
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                Category = product.Category,
                Quantity = product.Quantity,
                Location = product.Location,
                RegistrationDate = product.RegistrationDate
            };

            return Ok(new ApiResponse<ProductResponseDto>
            {
                Success = true,
                Data = result,
                Message = "Product retrieved successfully"
            });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProductResponseDto>>> CreateProduct(ProductCreateDto productDto)
        {
            // Check if code already exists
            if (await _productRepository.ExistsByCodeAsync(productDto.Code))
            {
                return BadRequest(new ApiResponse<ProductResponseDto>
                {
                    Success = false,
                    Message = "A product with this code already exists"
                });
            }

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

            var result = new ProductResponseDto
            {
                Id = createdProduct.Id,
                Code = createdProduct.Code,
                Name = createdProduct.Name,
                Category = createdProduct.Category,
                Quantity = createdProduct.Quantity,
                Location = createdProduct.Location,
                RegistrationDate = createdProduct.RegistrationDate
            };

            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id },
                new ApiResponse<ProductResponseDto>
                {
                    Success = true,
                    Data = result,
                    Message = "Product created successfully"
                });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ProductResponseDto>>> UpdateProduct(int id, ProductUpdateDto productDto)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                return NotFound(new ApiResponse<ProductResponseDto>
                {
                    Success = false,
                    Message = "Product not found"
                });

            // Check if code already exists for another product
            if (!string.IsNullOrEmpty(productDto.Code) &&
                await _productRepository.ExistsByCodeAsync(productDto.Code, id))
            {
                return BadRequest(new ApiResponse<ProductResponseDto>
                {
                    Success = false,
                    Message = "A product with this code already exists"
                });
            }

            // Update only the fields that are provided
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

            var result = new ProductResponseDto
            {
                Id = updatedProduct!.Id,
                Code = updatedProduct.Code,
                Name = updatedProduct.Name,
                Category = updatedProduct.Category,
                Quantity = updatedProduct.Quantity,
                Location = updatedProduct.Location,
                RegistrationDate = updatedProduct.RegistrationDate
            };

            return Ok(new ApiResponse<ProductResponseDto>
            {
                Success = true,
                Data = result,
                Message = "Product updated successfully"
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteProduct(int id)
        {
            var deleted = await _productRepository.DeleteAsync(id);
            if (!deleted)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Product not found"
                });

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Product deleted successfully"
            });
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductResponseDto>>>> GetProductsByCategory(string category)
        {
            var products = await _productRepository.GetByCategoryAsync(category);
            var result = products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Category = p.Category,
                Quantity = p.Quantity,
                Location = p.Location,
                RegistrationDate = p.RegistrationDate
            });

            return Ok(new ApiResponse<IEnumerable<ProductResponseDto>>
            {
                Success = true,
                Data = result,
                Message = "Products retrieved successfully"
            });
        }

        [HttpGet("location/{location}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductResponseDto>>>> GetProductsByLocation(string location)
        {
            var products = await _productRepository.GetByLocationAsync(location);
            var result = products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Category = p.Category,
                Quantity = p.Quantity,
                Location = p.Location,
                RegistrationDate = p.RegistrationDate
            });

            return Ok(new ApiResponse<IEnumerable<ProductResponseDto>>
            {
                Success = true,
                Data = result,
                Message = "Products retrieved successfully"
            });
        }

        [HttpGet("categories")]
        public async Task<ActionResult<ApiResponse<IEnumerable<string>>>> GetCategories()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return Ok(new ApiResponse<IEnumerable<string>>
            {
                Success = true,
                Data = categories,
                Message = "Categories retrieved successfully"
            });
        }
    }
}
