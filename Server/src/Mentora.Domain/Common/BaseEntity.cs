using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Common;

public abstract class BaseEntity
{
    // توحيد النوع ليكون Guid
    public Guid Id { get; set; } = Guid.NewGuid();

    // حقول التدقيق (Audit Fields) حسب الطلب في الـ Checklist
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // لدعم الـ Soft Delete إذا احتجناه لاحقاً
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
}