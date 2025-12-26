using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.DTOs
{
    public class CareerPlanUpdateDto
    {
        public string Title { get; set; }
        public string CurrentLevel { get; set; }
        public string TargetLevel { get; set; }
        public string Description { get; set; }
        public List<string> Steps { get; set; } = new List<string>();
        public bool IsCompleted { get; set; } = false;
    }
}