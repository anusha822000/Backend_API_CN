using System;
using System.Collections.Generic;

namespace RoleManagementApi.Models;

public partial class LeftMenu
{
    public Guid MenuId { get; set; }

    public string MenuName { get; set; } = null!;

    public int MenuLevel { get; set; }

    public Guid? ParentMenuId { get; set; }

    public string? NavigationLink { get; set; }

    public bool IsActive { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<LeftMenu> InverseParentMenu { get; set; } = new List<LeftMenu>();

    public virtual LeftMenu? ParentMenu { get; set; }

    public virtual AppRole Role { get; set; } = null!;
}
