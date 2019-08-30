using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CustomIdentityCore2.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Extensions.Internal;

namespace CustomIdentityCore2.Data
{
    public class CustomRoleStore : IRoleStore<Role>
    {

        private bool _dispose;
        private CustomIdentityCoreDbContext _dbcontext;
        public CustomRoleStore(CustomIdentityCoreDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        #region " --- Role --- " 
        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            _dbcontext.Role.Add(role);
            await _dbcontext.SaveChangesAsync(cancellationToken);
            return await Task.FromResult(IdentityResult.Success);

        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            _dbcontext.Update(role);
            await _dbcontext.SaveChangesAsync(cancellationToken);
            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            _dbcontext.Remove(role);
            await _dbcontext.SaveChangesAsync(cancellationToken);
            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return await Task.FromResult(role.RoleId.ToString());
        }

        public async Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return await Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));

            }
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken = default(CancellationToken))
        {
            //not added in role table
            throw new System.NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult((object)null);
        }

        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            if (roleId == null)
            {
                throw new ArgumentNullException(nameof(roleId));
            }

            if (int.TryParse(roleId, out int id))
            {
                return await _dbcontext.Role.FindAsync(id);
            }
            else
            {
                return await Task.FromResult((Role)null);
            }
        }

        public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            if (normalizedRoleName == null)
            {
                throw new ArgumentNullException(nameof(normalizedRoleName));
            }

            return await _dbcontext.Role
                .AsAsyncEnumerable()
                .SingleOrDefault(r => r.Name.Equals(normalizedRoleName, StringComparison.OrdinalIgnoreCase),
                    cancellationToken);
        }
        #endregion

        #region " --- Dispose --- "

        public void Dispose() => _dispose = true;

        private void ThrowIfDisposed()
        {
            if (_dispose)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        #endregion
    }



}