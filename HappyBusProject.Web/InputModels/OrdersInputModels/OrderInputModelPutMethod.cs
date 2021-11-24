using System;
using System.ComponentModel.DataAnnotations;

namespace HappyBusProject.InputModels
{
    /// <summary>
    /// JUST FOR ORDERS PUT METHOD!!!
    /// </summary>
    public class OrderInputModelPutMethod
    {
        [Required]
        public string FullName { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public int OrderSeatsNum { get; set; }
        public DateTime DesiredDepartureTime { get; set; }
        [Required]
        public OrderType OrderType { get; set; }
    }
}
