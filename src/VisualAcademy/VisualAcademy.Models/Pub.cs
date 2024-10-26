using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualAcademy.Models
{
    public class Pub
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string[] Beers { get; set; }
        public List<DateOnly> DaysVisited { get; private set; } = new();
    }
}
