using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities
{
    public class AiSkill
    {
        public string Name { get; set; } = string.Empty;
        public string TargetLevel { get; set; } = "Beginner";
    }
}