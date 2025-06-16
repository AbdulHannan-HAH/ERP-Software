using System;
using System.Collections.Generic;
using ERP_Software.DL;
using ERP_Software.Models;

namespace ERP_Software.BL
{
    class UserRolesBL
    {
        public static string AddRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return "❌ Role name cannot be empty.";

            if (UserRolesDL.RoleExists(roleName))
                return "❌ Role already exists.";

            UserRolesDL.InsertRole(roleName);
            return "✅ Role added successfully.";
        }

        public static List<Roles> GetRoles()
        {
            return UserRolesDL.GetAllRoles();
        }

        public static string UpdateRole(int roleId, string newRoleName)
        {
            if (roleId <= 0)
                return "❌ Invalid Role ID.";

            if (string.IsNullOrWhiteSpace(newRoleName))
                return "❌ New role name cannot be empty.";

            if (UserRolesDL.RoleExists(newRoleName))
                return "❌ This role name already exists.";

            UserRolesDL.UpdateRole(roleId, newRoleName);
            return "✅ Role updated successfully.";
        }

        public static string DeleteRole(int roleId)
        {
            if (roleId <= 0)
                return "❌ Invalid Role ID.";

            UserRolesDL.DeleteRole(roleId);
            return "✅ Role deleted successfully.";
        }
        public static string GetRoleNameById(int roleId)
        {
            var roles = UserRolesDL.GetAllRoles();
            var role = roles.FirstOrDefault(r => r.RoleID == roleId);
            return role?.RoleName ?? "Unknown";
        }

    }
}
