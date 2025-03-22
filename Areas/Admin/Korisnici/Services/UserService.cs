using DodjelaStanovaZG.Areas.Admin.Korisnici.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace DodjelaStanovaZG.Areas.Admin.Korisnici.Services
{
    public interface IUserService
    {
        Task<TableData<UserDto>> GetUsersAsync(UserManager<IdentityUser> userManager, string searchText, string filterRole, TableState state, CancellationToken cancellationToken);
        Task<bool> DeleteUserAsync(UserManager<IdentityUser> userManager, string userId);
    }

    public class UserService : IUserService
    {
        public async Task<TableData<UserDto>> GetUsersAsync(
            UserManager<IdentityUser> userManager,
            string searchText,
            string filterRole,
            TableState state,
            CancellationToken cancellationToken)
        {
            // Krenemo od svih korisnika
            var query = userManager.Users.AsQueryable();

            // Filtriranje po searchText (UserName ili Email)
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(u => u.UserName != null && u.Email != null && (u.UserName.Contains(searchText) || u.Email.Contains(searchText)));
            }

            // Filtriranje po roli, ako nije "All"
            if (!string.IsNullOrWhiteSpace(filterRole) && filterRole != RoleNames.All)
            {
                // Dohvat korisnika koji su u traženoj roli
                var usersInRole = await userManager.GetUsersInRoleAsync(filterRole);
                var userIds = usersInRole.Select(u => u.Id).ToList();
                // Ograničimo query samo na korisnike iz tog ID skupa
                query = query.Where(u => userIds.Contains(u.Id));
            }

            // Ukupan broj zapisa (za pager)
            int totalItems = await query.CountAsync(cancellationToken);

            // Paginacija i dohvat
            var userEntities = await query
                .OrderBy(u => u.UserName)
                .Skip(state.Page * state.PageSize)
                .Take(state.PageSize)
                .ToListAsync(cancellationToken);

            // Mapiranje u DTO
            var users = new List<UserDto>();
            foreach (var user in userEntities)
            {
                var roles = await userManager.GetRolesAsync(user);
                users.Add(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = string.Join(", ", roles)
                });
            }

            return new TableData<UserDto>
            {
                Items = users,
                TotalItems = totalItems
            };
        }

        public async Task<bool> DeleteUserAsync(UserManager<IdentityUser> userManager, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            return false;
        }
    }
}
