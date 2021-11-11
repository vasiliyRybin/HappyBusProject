using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HappyBusProject.HappyBusProject.DataLayer.InputModels
{
    /// <summary>
    /// Order types is:
    /// 1 is for MobileApp
    /// 2 is for Site
    /// </summary>
    public enum OrderType
    {
        MobileApp = 1,
        Site
    }

    public class OrderInputModel
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string StartPoint { get; set; }
        [Required]
        public string EndPoint { get; set; }
        [Required]
        public int OrderSeatsNum { get; set; }
        [Required]
        public DateTime DesiredDepartureTime { get; set; }
        [Required]
        public OrderType OrderType { get; set; }
    }
}
