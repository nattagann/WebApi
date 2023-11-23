using System.ComponentModel.DataAnnotations;

namespace CShapTestWebApi.Models
{
    public class UpdateProductModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }
    }
}
