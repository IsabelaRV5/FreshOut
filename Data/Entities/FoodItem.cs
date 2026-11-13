using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Data.Entities
{
    public abstract class FoodItem
    {
        public int FoodItemId { get; set; }
        public required string Name { get; set; }
        public int UserId { get; set; }
        public required User User { get; set; }
        public required string Type { get; set; }
        public int Quantity { get; set; }
    }
}