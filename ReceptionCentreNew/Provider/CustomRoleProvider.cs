using Microsoft.AspNetCore.Identity;
using ReceptionCentreNew.Data.Context.App;
using ReceptionCentreNew.Models;

namespace ReceptionCentreNew.Providers;
public class CustomRoleProvider : IUserRoleStore<ApplicationUser>
{
#warning Исправить
    public string[] GetRolesForUser(string name)
    {
        string[] role = { };
        using (ReceptionCentreContext _db = new())
        {
            try
            {
                // Получаем пользователя
                SprEmployees employee = (from u in _db.SprEmployees
                                         where u.EmployeesLogin == name
                                         select u).FirstOrDefault();
                if (employee != null)
                {
                    // получаем роль
                    string[] userRole = _db.SprEmployeesRoleJoin
                        .Where(UR => UR.SprEmployeesId == employee.Id)
                        .Join(_db.SprEmployeesRole, S => S.SprEmployeesRoleId, SS => SS.Id, (S, SS) => SS)
                        .Select(s => s.RoleName)
                        .ToArray();

                    if (userRole != null)
                    {
                        role = userRole;
                    }
                }
            }
            catch
            {
                role = [];
            }
        }
        return role;
    }

    public bool IsUserInRole(string username, string roleName)
    {
        bool outputResult = false;
        // Находим пользователя
        using (ReceptionCentreContext _db = new())
        {
            try
            {
                // Получаем пользователя
                SprEmployees employee = (from u in _db.SprEmployees
                                         where u.EmployeesLogin == username
                                         select u).FirstOrDefault();
                if (employee != null)
                {
                    // получаем роль
                    string[] userRole = _db.SprEmployeesRoleJoin

                        .Where(UR => UR.SprEmployeesId == employee.Id)
                        .Join(_db.SprEmployeesRole, S => S.SprEmployeesRoleId, SS => SS.Id, (S, SS) => SS)
                        .Select(s => s.RoleName).ToArray();

                    foreach (string role in userRole)
                        //сравниваем
                        if (userRole != null && role == roleName)
                        {
                            return outputResult = true;
                        }
                }
            }
            catch
            {
                outputResult = false;
            }
        }
        return outputResult;
    }

    public Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}