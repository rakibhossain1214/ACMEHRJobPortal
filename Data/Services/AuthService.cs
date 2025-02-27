using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;

namespace HRJobPortal.Data.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        // In-memory store for authenticated users (in a real app, you might use a distributed cache)
        private static readonly ConcurrentDictionary<string, int> _authenticatedUsers = new ConcurrentDictionary<string, int>();
        private static readonly ConcurrentDictionary<int, string> _userNames = new ConcurrentDictionary<int, string>();
        
        // Current user for this instance
        private int? _currentUserId;
        private string _currentUserName;

        public AuthService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            
            // Try to get user from connection ID
            var connectionId = _httpContextAccessor.HttpContext?.Connection.Id;
            if (connectionId != null && _authenticatedUsers.TryGetValue(connectionId, out var userId))
            {
                _currentUserId = userId;
                _userNames.TryGetValue(userId, out _currentUserName);
            }
        }

        public async Task<User> RegisterUserAsync(string name, string email, string password)
        {
            // Check if user already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                return null; // User already exists
            }

            // Create new user
            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = HashPassword(password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Store user in memory
            var connectionId = _httpContextAccessor.HttpContext?.Connection.Id;
            if (connectionId != null)
            {
                _authenticatedUsers[connectionId] = user.UserId;
                _userNames[user.UserId] = user.Name;
                _currentUserId = user.UserId;
                _currentUserName = user.Name;
            }

            return user;
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return null; // User not found
            }

            // Verify password
            if (VerifyPassword(password, user.PasswordHash))
            {
                // Store user in memory
                var connectionId = _httpContextAccessor.HttpContext?.Connection.Id;
                if (connectionId != null)
                {
                    _authenticatedUsers[connectionId] = user.UserId;
                    _userNames[user.UserId] = user.Name;
                    _currentUserId = user.UserId;
                    _currentUserName = user.Name;
                }
                return user;
            }

            return null; // Invalid password
        }

        public void Logout()
        {
            var connectionId = _httpContextAccessor.HttpContext?.Connection.Id;
            if (connectionId != null && _authenticatedUsers.TryRemove(connectionId, out _))
            {
                _currentUserId = null;
                _currentUserName = null;
            }
        }

        public bool IsAuthenticated()
        {
            return _currentUserId.HasValue;
        }

        public int GetCurrentUserId()
        {
            return _currentUserId ?? 0;
        }

        public string GetCurrentUserName()
        {
            return _currentUserName ?? string.Empty;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
} 