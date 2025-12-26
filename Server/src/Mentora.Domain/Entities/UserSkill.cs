using CleanBackend.Domain.Common;
using CleanBackend.Domain.Entities.Auth;
using Mentora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanBackend.Domain.Entities
{
    public class UserSkill : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public int SkillId { get; set; }
        public Skill Skill { get; set; } = null!;
        public SkillLevel CurrentLevel { get; set; } = SkillLevel.Beginner;
    }
}