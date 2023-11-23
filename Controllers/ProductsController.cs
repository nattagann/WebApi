using CShapTestWebApi.Entities;
using CShapTestWebApi.Models;
using CShapTestWebApi.Services;
using CShapTestWebApi.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CShapTestWebApi.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get all products.
        /// </summary>
        [HttpGet]
        public ActionResult<ApiResponse<List<ProductModel>>> Get()
        {
            var products = _productService.GetAllProducts()
                .Select(p => new ProductModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price
                }).ToList();

            return Ok(new ApiResponse<List<ProductModel>>
            {
                Status = (int)HttpStatusCode.OK,
                Message = HttpStatusHelper.GetStatusMessage(HttpStatusCode.OK),
                Data = products
            });
        }

        /// <summary>
        /// Get a specific product by ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The requested product.</returns>
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<ProductModel>> Get(int id)
        {
            var product = _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound(new ApiResponse<ProductModel>
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Message = HttpStatusHelper.GetStatusMessage(HttpStatusCode.NotFound),
                });
            }

            return Ok(new ApiResponse<ProductModel>
            {
                Status = (int)HttpStatusCode.OK,
                Message = HttpStatusHelper.GetStatusMessage(HttpStatusCode.OK),
                Data = product
            });
        }

        /// <summary>
        /// Create a new product.
        /// </summary>
        /// <remarks>
        /// Creates a new product with the provided details.
        /// </remarks>
        /// <param name="newProduct">The details of the new product.</param>
        /// <returns>The newly created product.</returns>
        /// <response code="200">Returns the newly created product.</response>
        /// <response code="400">If the request is invalid or the product creation fails.</response>
        [HttpPost]
        public ActionResult<ProductModel> Post([FromBody] CreateProductModel newProduct)
        {
            int lastProductId = _productService.AddProduct(newProduct);
            ProductModel createdProduct = _productService.GetProductById(lastProductId);

            return Ok(new ApiResponse<ProductModel>
            {
                Status = (int)HttpStatusCode.OK,
                Message = HttpStatusHelper.GetStatusMessage(HttpStatusCode.OK),
                Data = createdProduct
            });
        }

        /// <summary>
        /// Update an existing product.
        /// </summary>
        /// <remarks>
        /// Updates an existing product with the provided details.
        /// </remarks>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="updatedProduct">The updated details of the product.</param>
        /// <returns>No content.</returns>
        /// <response code="204">No Content - Product successfully updated.</response>
        /// <response code="400">Bad Request - If the request is invalid.</response>
        /// <response code="404">Not Found - If the specified product is not found.</response>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateProductModel updatedProduct)
        {
            try
            {
                _productService.UpdateProduct(id, updatedProduct);
                // 204 No Content
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new ApiResponse<ProductModel>
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Message = ex.Message,
                });
            }
        }

        /// <summary>
        /// Delete a specific product by ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">No Content - Product successfully deleted.</response>
        /// <response code="404">Not Found - If the specified product is not found.</response>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _productService.DeleteProduct(id);
                // 204 No Content
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                // 404 Not Found
                return NotFound(new ApiResponse<ProductModel>
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Message = ex.Message,
                });
            }
        }
    }
}
