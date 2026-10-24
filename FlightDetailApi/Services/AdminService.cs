using FlightDetailApi.Data;
using FlightDetailApi.Models;
using FlightDetailApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlightDetailApi.Services
{
    // Service class responsible for handling admin authentication and registration
    public class AdminService
    {
        private readonly ApplicationDbContext _dbContext;

        public AdminService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Authenticates an admin user based on username and password.
        /// </summary>
        /// <param name="username">The username of the admin.</param>
        /// <param name="password">The password of the admin.</param>
        /// <returns>Returns the admin model if authentication is successful; otherwise, null.</returns>
        public async Task<AdminModel> LoginAsync(string username, string password)
        {
            try
            {
                // Find the admin by username
                var admin = await _dbContext.Admin.Where(x=>x.Username == username).FirstOrDefaultAsync();

                // Return null if the admin does not exist or the password is incorrect
                if (admin == null || !VerifyPassword(password, admin.Password))
                    return null;

                return admin;
            }
            catch (Exception ex)
            {
                // Handle exceptions that occur during login
                throw new ApplicationException("An error occurred during login.", ex);
            }
        }

        /// <summary>
        /// Registers a new admin if the username is not already taken.
        /// </summary>
        /// <param name="admin">The admin model containing registration details.</param>
        /// <returns>Returns true if registration is successful; otherwise, false.</returns>
        public async Task<bool> RegisterAsync(AdminModel admin)
        {
            if (admin == null)
                throw new ArgumentNullException(nameof(admin));

            try
            {
                // Check if an admin with the same username already exists
                var existingAdmin = await _dbContext.Admin.Where(a => a.Username == admin.Username).FirstOrDefaultAsync();
                if (existingAdmin != null)
                    return false;

                // Hash the password before storing it
                admin.Password = HashPassword(admin.Password);
                
                // Add new admin to the database
                await _dbContext.Admin.AddAsync(admin);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Handle exceptions that occur during registration
                throw new ApplicationException("An error occurred during registration.", ex);
            }
        }

        /// <summary>
        /// Verifies if the input password matches the stored password.
        /// </summary>
        /// <param name="inputPassword">The password provided by the user.</param>
        /// <param name="storedPasswordHash">The stored password hash.</param>
        /// <returns>Returns true if the passwords match; otherwise, false.</returns>
        private bool VerifyPassword(string inputPassword, string storedPasswordHash)
        {
            // TODO: Implement proper password hashing and verification mechanism
            return inputPassword == storedPasswordHash; 
        }

        /// <summary>
        /// Hashes the password before storing it in the database.
        /// </summary>
        /// <param name="password">The plain text password.</param>
        /// <returns>Returns the hashed password.</returns>
        private string HashPassword(string password)
        {
            // TODO: Implement proper password hashing
            return password; 
        }
    }
}
