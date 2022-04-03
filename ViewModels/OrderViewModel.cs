using DutchTreat.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        [Required]
        [MinLength(4, ErrorMessage = "OrderNumber must be at least 4 chars in length!!!")]
        public string OrderNumber { get; set; }
        public ICollection<OrderItemViewModel> Items { get; set; }
    }
}
