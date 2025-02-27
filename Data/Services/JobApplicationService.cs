using Microsoft.EntityFrameworkCore;
using HRJobPortal.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRJobPortal.Data.Services
{
    public class JobApplicationService
    {
        private readonly ApplicationDbContext _context;

        public JobApplicationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<JobApplication>> GetApplicationsByUserIdAsync(int userId)
        {
            return await _context.JobApplications
                .Include(ja => ja.Job)
                .Where(ja => ja.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> HasUserAppliedToJobAsync(int userId, int jobId)
        {
            return await _context.JobApplications
                .AnyAsync(ja => ja.UserId == userId && ja.JobId == jobId);
        }

        public async Task<JobApplication> ApplyToJobAsync(int userId, int jobId)
        {
            // Check if user has already applied
            bool alreadyApplied = await HasUserAppliedToJobAsync(userId, jobId);
            if (alreadyApplied)
            {
                return null; // User already applied
            }

            var application = new JobApplication
            {
                UserId = userId,
                JobId = jobId,
                AppliedDate = System.DateTime.Now
            };

            _context.JobApplications.Add(application);
            await _context.SaveChangesAsync();
            return application;
        }

        public async Task<bool> WithdrawApplicationAsync(int applicationId)
        {
            var application = await _context.JobApplications.FindAsync(applicationId);
            
            if (application == null)
            {
                return false;
            }
            
            _context.JobApplications.Remove(application);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<JobApplication>> GetApplicationsByJobIdAsync(int jobId)
        {
            return await _context.JobApplications
                .Include(ja => ja.User)
                .Where(ja => ja.JobId == jobId)
                .ToListAsync();
        }
    }
} 