using DietTrackerBlazorServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietTrackerBlazorServer.Model
{
    public class FoodDataPoint
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int FoodTypeId { get; set; }
        public FoodType FoodType { get; set; }
        public DateTime Date { get; set; }

    }
}
