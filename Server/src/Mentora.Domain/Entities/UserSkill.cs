using Mentora.Domain.Common;
using Mentora.Domain.Entities.Auth;
using Mentora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities
{
    public class UserSkill : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid SkillId { get; set; }
        public Skill Skill { get; set; } = null!;
        public SkillLevel CurrentLevel { get; set; } = SkillLevel.Beginner;
    }
}