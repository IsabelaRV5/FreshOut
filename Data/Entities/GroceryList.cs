using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Data.Entities
{
    public class GroceryList
    {
        public int GroceryListId { get; set; }
        public required string Name { get; set; }
        public int UserId { get; set; }
        public required User User { get; set; }
        public List<GroceryItem> Items { get; set; } = new();
    }
}
