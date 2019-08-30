using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CustomIdentityCore2.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;

namespace CustomIdentityCore2.Data
{
    public class CustomUserStore : IUserEmailStore<User>, IUserRoleStore<User>, IUserPasswordStore<User>
    {
        private CustomIdentityCoreDbContext _dbcontext;
        private bool _disposed;

        public CustomUserStore(CustomIdentityCoreDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        #region " User "
        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return await Task.FromResult(user.UserId.ToString());

            //return await Task.FromResult(user.UserId.ToString());
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return await Task.FromResult(user.UserName);
            //return await Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.UserName = userName;
            return Task.CompletedTask;

            //throw new System.NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            ////don't have a normalizedName in the user class...
            throw new System.NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
        {
            ////don't have a normalizedName in the user class...
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult((object)null);
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException();
            _dbcontext.Add(user);
            await _dbcontext.SaveChangesAsync(cancellationToken);
            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException();
            _dbcontext.Update(user);
            await _dbcontext.SaveChangesAsync(cancellationToken);
            return await Task.FromResult(IdentityResult.Success);
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null) throw new ArgumentNullException();
            _dbcontext.Remove(user);

            var returnValue = await _dbcontext.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(returnValue == 1 ? IdentityResult.Success : IdentityResult.Failed());
        }

        #endregion

        #region " UserRole "

        public Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var role = _dbcontext.Role.Where(item => item.Name.Equals(roleName)).FirstOrDefault();
            if (role != null)
            {
                UserRole assignment = new UserRole() { RoleId = role.RoleId, UserId = user.UserId };
                _dbcontext.UserRole.Add(assignment);
                _dbcontext.SaveChanges();
            }
            return Task.FromResult((User)null);
        }

        public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            var role = _dbcontext.Role.Where(r => r.Name.Equals(roleName)).FirstOrDefault();
            if (role != null)
            {
                var assignments = _dbcontext.UserRole.Where(item =>
                    item.UserId.Equals(user.UserId) && item.RoleId.Equals(role.RoleId));
                _dbcontext.UserRole.RemoveRange(assignments.ToArray());
                _dbcontext.SaveChanges();
            }

            return Task.FromResult<User>(null);
        }

        public Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            var assignments = _dbcontext.UserRole.Include(record => record.Role)
                .Where(item => item.UserId.Equals(user.UserId));
            List<string> roles = new List<string>();
            foreach (var record in assignments)
            {
                roles.Add(record.Role.Name);
            }

            return Task.FromResult<IList<string>>(roles);
        }

        public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            bool inRole = false;
            var role = _dbcontext.Role.Where(r => r.Name.Equals(roleName)).FirstOrDefault();
            if (role != null)
            {
                var assignment = _dbcontext.UserRole.Where(item =>
                    item.UserId.Equals(user.UserId) && item.RoleId.Equals(role.RoleId)).FirstOrDefault();

                inRole = assignment != null;
            }

            return Task.FromResult<bool>(inRole);
        }

        public Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken =default(CancellationToken))
        {
            IList<User> users = new List<User>();
            var role = _dbcontext.Role.Where(item => item.RoleId.Equals(roleName)).FirstOrDefault();
            if (role != null)
            {
                var assignments = _dbcontext.UserRole.Where(item => item.RoleId.Equals(role.RoleId));
                foreach (var record in assignments)
                {
                    users.Add(record.User);
                }
            }

            return Task.FromResult<IList<User>>(users);
        }
        #endregion

        #region " Email "

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            if (int.TryParse(userId, out int id))
            {
                return await _dbcontext.User.FindAsync(id);
            }
            else
            {
                return await Task.FromResult((User)null);
            }
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            if (normalizedUserName == null)
            {
                throw new ArgumentNullException(nameof(normalizedUserName));
            }
            return await _dbcontext.User
                .AsAsyncEnumerable()
                .SingleOrDefault(p => p.Email.Equals(normalizedUserName, StringComparison.OrdinalIgnoreCase), cancellationToken);
        }


        public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.Email = email;
            return Task.CompletedTask;

            //throw new NotImplementedException();
        }

        public async Task<string> GetEmailAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return await Task.FromResult(user.Email);
            // throw new NotImplementedException();
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken =default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user==null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(true);
            //throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user==null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult<object>(null);

            //throw new NotImplementedException();

        }


        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            //not add in the user class
            ThrowIfDisposed();
            if (normalizedEmail == null)
            {
                throw new ArgumentNullException(nameof(normalizedEmail));
            }
            return await _dbcontext.User
                .AsAsyncEnumerable()
                .SingleOrDefault(p => p.Email.Equals(normalizedEmail, StringComparison.OrdinalIgnoreCase), cancellationToken);

            //throw new NotImplementedException();
        }

        public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user==null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.UserName);
            //throw new NotImplementedException();
        }


        public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Email = normalizedEmail;
            return Task.CompletedTask;

        }
        #endregion

        #region"  Password "
        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new AbandonedMutexException(nameof(user));
            }

            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new AppDomainUnloadedException(nameof(user));
            }

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken=default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.PasswordHash != null);
        }
        #endregion  #region"  Dispose "

        #region " Dispose "
        /// <summary>
        /// Throws if this class has been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
        public void Dispose() => _disposed = true;

        #endregion
    }
}