using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities;

namespace Logic
{
    /// <summary>
    /// this class is the logic for Sort Fooditems by Name
    /// It handles the sorting of FoodItems by Name
    /// </summary>
    /// <remarks>
    /// author: Xena Tat
    /// date: 14.04.2026
    /// </remarks>
    public class SortByName : ISortMethod<FoodItem>
    {
        /// <summary>
        /// <c>SortAscending</c>
        /// Sorts FoodItems list by Name in Ascending order
        /// 
        /// </summary>
        /// <param name="list"></param> Input list
        /// <returns> Returns sorted list</returns>
        public List<FoodItem> SortAscending(List<FoodItem> list)
        {
            return list.OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// <c>SortDescending</c>
        /// Sorts FoodItems list by Quantity in Descending order
        /// 
        /// </summary>
        /// <param name="list"></param> Input list
        /// <returns> Returns sorted list</returns>
        public List<FoodItem> SortDescending(List<FoodItem> list)
        {
            return list.OrderByDescending(x => x.Name).ToList();
        }
    }
}
