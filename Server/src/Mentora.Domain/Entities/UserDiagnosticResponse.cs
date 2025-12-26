using Mentora.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities
{
    public class UserDiagnosticResponse : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Major { get; set; } = string.Empty;
        public string RawResponseJson { get; set; } = "{}";
    }
}