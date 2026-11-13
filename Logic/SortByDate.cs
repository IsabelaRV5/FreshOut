using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities;

namespace Logic
{
    /// <summary>
    /// this class is the logic for Sort LocationItems by Expiration Date
    /// It handles the sorting of LocationItems by Expiration Date
    /// </summary>
    /// <remarks>
    /// author: Xena Tat
    /// date: 14.04.2026
    /// </remarks>
    public class SortByDate : ISortMethod<LocationItem>
    {
        ///<summary>
        /// <c>SortAscending</c>
        /// Sorts food items list from oldest to newest
        /// 
        /// </summary>
        /// <param name="list"></param> Input list
        /// <returns> Returns sorted list</returns>
        public List<LocationItem> SortAscending(List<LocationItem> list)
        {
            return list.OrderBy(x => x.ExpirationDate).ToList();
        }

        ///<summary>
        /// <c>SortDescending</c>
        /// Sorts food items list from newest to oldest
        /// 
        /// </summary>
        /// <param name="list"></param> Input list
        /// <returns> Returns sorted list</returns>
        public List<LocationItem> SortDescending(List<LocationItem> list)
        {
            return list.OrderByDescending(x => x.ExpirationDate).ToList();
        }
    }
}
