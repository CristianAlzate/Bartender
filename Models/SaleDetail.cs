using System.ComponentModel.DataAnnotations;

namespace BartenderApp.Models
{
    public class SaleDetail
    {
        [Key]
        public int Id { get; set; }
        public int IdSale { get; set; }
        public string? Name { get; set; }
        public int Price { get; set; }
        public int CostPrice { get; set; }
        public int Quantity {  get; set; }
    }
}
