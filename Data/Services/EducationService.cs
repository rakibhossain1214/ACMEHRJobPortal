using Microsoft.EntityFrameworkCore;
using HRJobPortal.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRJobPortal.Data.Services
{
    public class EducationService
    {
        private readonly ApplicationDbContext _context;

        public EducationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Education>> GetEducationsByUserIdAsync(int userId)
        {
            return await _context.Educations
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }

        public async Task<Education> GetEducationByIdAsync(int id)
        {
            return await _context.Educations.FindAsync(id);
        }

        public async Task<Education> AddEducationAsync(Education education)
        {
            _context.Educations.Add(education);
            await _context.SaveChangesAsync();
            return education;
        }

        public async Task<Education> UpdateEducationAsync(Education education)
        {
            _context.Entry(education).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return education;
        }

        public async Task DeleteEducationAsync(int id)
        {
            var education = await _context.Educations.FindAsync(id);
            if (education != null)
            {
                _context.Educations.Remove(education);
                await _context.SaveChangesAsync();
            }
        }
    }
} 