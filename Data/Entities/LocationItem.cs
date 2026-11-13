using Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class LocationItem : FoodItem
    {
        public int LocationListId { get; set; }
        public required LocationList Location { get; set; }
        public DateOnly ExpirationDate { get; set; }

        public string BackgroundColor => CheckColors(ExpirationDate);

        /// <summary>
        /// Checks expiration date and assign color 
        /// for background color of Textblock component
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string CheckColors(DateOnly date)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            int difference = date.DayNumber - today.DayNumber;

            // Before today (Expired)
            if (difference < 0)
            {
                return ColorEnums.DarkRed.ToString();
            }

            // 0-2 days (Very close to expiration)
            else if (difference > -1 && difference < 3)
            {
                return ColorEnums.Red.ToString();
            }

            // 3-5 days (Close to expiration)
            else if (difference > 2 && difference < 6)
            {
                return ColorEnums.Gold.ToString();
            }

            // 6-8 days (Good condition)
            else if (difference > 5 && difference < 9)
            {
                return ColorEnums.Moccasin.ToString();
            }

            // 9-14 days (Very good condition)
            else if (difference > 8 && difference < 15)
            {
                return ColorEnums.LightGreen.ToString();
            }
            else if (difference > 14 && difference < 31)
            {
                return ColorEnums.Lime.ToString();
            }

            // 31+ days (Fresh)
            else
            {
                return ColorEnums.Green.ToString();
            }
        }
    }
}