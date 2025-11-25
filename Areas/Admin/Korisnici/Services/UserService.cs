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

    public class UserService(ILogger<UserService> logger) : IUserService
    {
        public async Task<TableData<UserDto>> GetUsersAsync(
            UserManager<IdentityUser> userManager,
            string searchText,
            string filterRole,
            TableState state,
            CancellationToken cancellationToken)
        {
            logger.LogDebug("Fetching users (search: {SearchText}, role: {Role})", searchText, filterRole);
            var query = userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(u => u.UserName != null && u.Email != null && (u.UserName.Contains(searchText) || u.Email.Contains(searchText)));
            }

            if (!string.IsNullOrWhiteSpace(filterRole) && filterRole != RoleNames.All)
            {
                var usersInRole = await userManager.GetUsersInRoleAsync(filterRole);
                var userIds = usersInRole.Select(u => u.Id).ToList();
                query = query.Where(u => userIds.Contains(u.Id));
            }

            int totalItems = await query.CountAsync(cancellationToken);
            logger.LogDebug("Total users after filtering: {Count}", totalItems);

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
                var lockoutEnd = await userManager.GetLockoutEndDateAsync(user);
                bool isLocked = lockoutEnd.HasValue && lockoutEnd > DateTimeOffset.UtcNow;

                users.Add(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = string.Join(", ", roles),
                    IsLocked = isLocked
                });
            }

            logger.LogDebug("Returning {Count} users", users.Count);
            return new TableData<UserDto>
            {
                Items = users,
                TotalItems = totalItems
            };
        }

        public async Task<bool> DeleteUserAsync(UserManager<IdentityUser> userManager, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                logger.LogWarning("User {UserId} not found for deletion", userId);
                return false;
            }

            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
                logger.LogInformation("Deleted user {UserId}", userId);
            else
                logger.LogError("Failed to delete user {UserId}", userId);

            return result.Succeeded;
        }
    }
}