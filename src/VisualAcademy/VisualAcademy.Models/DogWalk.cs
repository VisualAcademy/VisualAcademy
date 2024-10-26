using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualAcademy.Models
{
    public class DogWalk
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public Terrain Terrain { get; set; }
        public List<DateOnly> DaysVisited { get; private set; } = new();
        public Pub? ClosestPub { get; set; }
    }

    public enum Terrain
    {
        Forest,
        River,
        Hills,
        Village,
        Park,
        Beach,
    }
}
