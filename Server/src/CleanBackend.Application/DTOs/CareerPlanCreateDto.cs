using System.Collections.Generic;

namespace Mentora.App.DTOs
{
    public class CareerPlanCreateDto
    {
        public string Title { get; set; }
        public string CurrentLevel { get; set; }
        public string TargetLevel { get; set; }
        public string Description { get; set; } // هذا الوصف من ماريا
        public List<string> Steps { get; set; } = new List<string>();
    }
}