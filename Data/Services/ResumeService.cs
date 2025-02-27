using Microsoft.EntityFrameworkCore;
using HRJobPortal.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRJobPortal.Data.Services
{
    public class ResumeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ResumeService(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<List<UploadResume>> GetResumesByUserIdAsync(int userId)
        {
            return await _context.Resumes
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<UploadResume> GetResumeByIdAsync(int id)
        {
            return await _context.Resumes.FindAsync(id);
        }

        public async Task<UploadResume> UploadResumeAsync(IBrowserFile file, int userId)
        {
            // Create uploads directory if it doesn't exist
            string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate unique filename
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.Name;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.OpenReadStream(maxAllowedSize: 10485760).CopyToAsync(fileStream);
            }

            // Create resume record
            var resume = new UploadResume
            {
                FilePath = "/uploads/" + uniqueFileName,
                UploadedDate = DateTime.Now,
                UserId = userId
            };

            _context.Resumes.Add(resume);
            await _context.SaveChangesAsync();
            return resume;
        }

        public async Task DeleteResumeAsync(int id)
        {
            var resume = await _context.Resumes.FindAsync(id);
            if (resume != null)
            {
                // Delete physical file
                string filePath = Path.Combine(_environment.WebRootPath, resume.FilePath.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Remove from database
                _context.Resumes.Remove(resume);
                await _context.SaveChangesAsync();
            }
        }
    }
} 