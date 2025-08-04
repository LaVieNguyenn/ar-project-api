using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARClothingAPI.Common.DTOs
{
    public class PlanCreateDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int DurationMonths { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercent { get; set; }
    }
}
