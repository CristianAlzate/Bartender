
using System.ComponentModel.DataAnnotations;

namespace BartenderApp.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? DateSale { get; set; }
        public int Amount { get; set; }
        public int Pay { get; set; }
        public bool Cancelled { get; set; }
    }
}
