using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualAcademy.Models
{
    public class PostSummary
    {
        public string BlogName { get; set; }
        public string PostTitle { get; set; }
        public DateOnly PublishedOn { get; set; }
    }
}
