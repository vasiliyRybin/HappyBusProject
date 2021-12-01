#nullable disable

using HappyBusProject.Interfaces;

namespace HappyBusProject
{
    public partial class RouteStop : IBaseEntity
    {
        public int PointId { get; set; }
        public string Name { get; set; }
        public int RouteLengthKM { get; set; }
        public string RouteDirection { get; set; }
    }
}
