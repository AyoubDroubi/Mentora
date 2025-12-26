using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities
{
    public class AiStep
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Checkpoints { get; set; } = new();
        public List<AiResource> Resources { get; set; } = new();
    }
}