using System;
using System.Collections.Generic;

namespace RoleManagementApi.Models;

public partial class AppRole
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string? RoleDescription { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<LeftMenu> LeftMenus { get; set; } = new List<LeftMenu>();
}
