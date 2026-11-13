using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Data.Entities
{
    public class LocationList
    {
        public int LocationListId { get; set; }
        public required string Name { get; set; }
        public int UserId { get; set; }
        public required User User { get; set; }
        public List<LocationItem> Items { get; set; } = new();
    }
}