using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Data.Entities
{
    public class GroceryItem : FoodItem
    {
        public int GroceryListId { get; set; }
        public required GroceryList GroceryList { get; set; }

        public bool IsChecked { get; set; } = false;

        public string CheckColor => Check(IsChecked);

        public string TextDecor => SetTextDecor();

        /// <summary>
        /// Sets text decoration value according to check status
        /// </summary>
        /// <returns>value for text decoration</returns>
        public string SetTextDecor()
        {
            if (this.IsChecked)
            {
                return "Strikethrough";
            }
            return "";
        }

        /// <summary>
        /// Sets check status colors
        /// </summary>
        /// <returns>Color according to check status</returns>
        public static string Check(bool isChecked)
        {
            if (isChecked)
            {
                return "#45fc03";
            }
            return "#faecb1";
        }
    }
}