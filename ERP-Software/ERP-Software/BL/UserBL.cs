using ERP_Software.DL;
using ERP_Software.Models;
using ERP_Software.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Software.BL
{
    internal class UserBL
    {
        public static string CreateAdminUser(string username, string email, int roleId, string createdBy)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
                return "Username and Email are required.";

            string plainPassword = GenerateRandomPassword(); 
            string hashedPassword = PasswordHasher.HashPassword(plainPassword);

            User user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = hashedPassword,
                RoleID = roleId,
                CreatedBy = createdBy
            };

            UserDL.InsertUser(user);

            string roleName = UserRolesBL.GetRoleNameById(roleId);
            EmailHelper.SendCredentialsEmail(email, username, plainPassword, roleName);

            return "Admin user created and credentials sent via email.";
        }
        private static string GenerateRandomPassword()
        {
            return Guid.NewGuid().ToString().Substring(0, 8); // 8-char random string
        }
        public static List<User> GetAllUsers()
        {
            return UserDL.GetAllUsers();
        }

        public static string DeleteUser(int userId)
        {
            UserDL.DeleteUser(userId);
            return "User deleted successfully.";
        }

        public static string UpdatePassword(int userId, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                return "Password cannot be empty.";

            string hashed = PasswordHasher.HashPassword(newPassword);
            UserDL.UpdatePassword(userId, hashed);
            return "Password updated successfully.";
        }
        public static User AuthenticateUser(string username, string password)
        {
            var user = UserDL.GetUserByUsername(username);
            if (user != null && PasswordHasher.VerifyPassword(password, user.PasswordHash))
            {
                return user; // valid login
            }
            return null;
        }

    }
}
