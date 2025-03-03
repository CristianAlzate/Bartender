
using System.ComponentModel.DataAnnotations;

namespace BartenderApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Price { get; set; }
        public int CostPrice { get; set; }
        public string? SearchName { get; set; }
    }   
}
