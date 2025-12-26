using Mentora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entitie
{
    public class AiCareerPlanResponse
    {
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public List<AiStep> Steps { get; set; } = new();
        public List<AiSkill> Skills { get; set; } = new();
    }
}