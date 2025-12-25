using System;
using System.Collections.Generic;

namespace Mentora.App.DTOs
{
    public class CareerPlanViewDto
    {
        // تم التغيير من int إلى Guid
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string CurrentLevel { get; set; }
        public string TargetLevel { get; set; }
        public string Description { get; set; }

        // سنرجع النصوص فقط للواجهة لتبسيط الأمور حالياً
        public List<string> Steps { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}