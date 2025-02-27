using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HRJobPortal.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            // Look for existing data
            if (context.Users.Any() || context.Jobs.Any())
            {
                return; // DB has been seeded
            }

            // Add sample users
            var user1 = new User
            {
                Name = "John Doe",
                ContactInfo = "john.doe@example.com",
                Address = "123 Main St, Anytown, USA"
            };

            var user2 = new User
            {
                Name = "Jane Smith",
                ContactInfo = "jane.smith@example.com",
                Address = "456 Oak Ave, Somewhere, USA"
            };

            context.Users.AddRange(user1, user2);
            await context.SaveChangesAsync();

            // Add education records
            var education1 = new Education
            {
                Degree = "Bachelor of Science in Computer Science",
                University = "Tech University",
                CGPA = 3.8f,
                YearOfCompletion = 2020,
                UserId = user1.UserId
            };

            var education2 = new Education
            {
                Degree = "Master of Business Administration",
                University = "Business School",
                CGPA = 3.9f,
                YearOfCompletion = 2022,
                UserId = user2.UserId
            };

            context.Educations.AddRange(education1, education2);
            await context.SaveChangesAsync();

            // Add jobs
            var job1 = new Job
            {
                JobTitle = "Software Developer",
                Description = "We are looking for a skilled software developer to join our team. The ideal candidate will have experience with C# and ASP.NET Core.",
                Requirements = "- 2+ years of experience with C# and .NET\n- Experience with web development\n- Bachelor's degree in Computer Science or related field",
                Deadline = DateTime.Now.AddDays(30)
            };

            var job2 = new Job
            {
                JobTitle = "Project Manager",
                Description = "We need an experienced project manager to lead our development team. The candidate should have strong leadership skills and technical background.",
                Requirements = "- 5+ years of project management experience\n- PMP certification\n- Experience with Agile methodologies",
                Deadline = DateTime.Now.AddDays(15)
            };

            var job3 = new Job
            {
                JobTitle = "UI/UX Designer",
                Description = "Join our creative team as a UI/UX designer. You will be responsible for creating beautiful and intuitive user interfaces for our web applications.",
                Requirements = "- 3+ years of UI/UX design experience\n- Proficiency with design tools like Figma or Adobe XD\n- Portfolio of previous work",
                Deadline = DateTime.Now.AddDays(-5) // Expired job
            };

            context.Jobs.AddRange(job1, job2, job3);
            await context.SaveChangesAsync();

            // Add job applications
            var application1 = new JobApplication
            {
                UserId = user1.UserId,
                JobId = job2.JobId,
                AppliedDate = DateTime.Now.AddDays(-2)
            };

            context.JobApplications.Add(application1);
            await context.SaveChangesAsync();
        }
    }
} 