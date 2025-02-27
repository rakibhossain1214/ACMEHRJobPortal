using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HRJobPortal.Data
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? ContactInfo { get; set; } // Nullable
        public string? Address { get; set; } // Nullable

        [Required]
        public string PasswordHash { get; set; }

        public ICollection<Education> Educations { get; set; } = new List<Education>();
        public ICollection<UploadResume> UploadResumes { get; set; } = new List<UploadResume>();
        public ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
    }

    public class Education
    {
        [Key]
        public int EducationId { get; set; }

        [Required]
        public string Degree { get; set; }

        [Required]
        public string University { get; set; }

        public float CGPA { get; set; }

        public int YearOfCompletion { get; set; }

        public int UserId { get; set; } // Foreign Key
        public User User { get; set; } // Navigation property
    }

    public class UploadResume
    {
        [Key]
        public int ResumeId { get; set; }

        [Required]
        public string FilePath { get; set; } // Store path or use Blob Storage

        public DateTime UploadedDate { get; set; } = DateTime.Now;

        public int UserId { get; set; } // Foreign Key
        public User User { get; set; } // Navigation property
    }

    public class Job
    {
        [Key]
        public int JobId { get; set; }

        [Required]
        public string JobTitle { get; set; }

        public string Description { get; set; }

        public string Requirements { get; set; }

        public DateTime Deadline { get; set; }

        public ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();

        // Computed property to check if job is active
        [NotMapped]
        public bool IsActive => Deadline > DateTime.Now;
    }

    public class JobApplication
    {
        [Key]
        public int ApplicationId { get; set; }

        public int UserId { get; set; } // Foreign Key
        public User User { get; set; } // Navigation property

        public int JobId { get; set; } // Foreign Key
        public Job Job { get; set; } // Navigation property

        public DateTime AppliedDate { get; set; } = DateTime.Now;
    }
} 