using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BlogAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public long IdNumber { get; set; }

        [StringLength(500)]
        public string? FirstName { get; set; }

        [StringLength(500)]
        public string? LastName { get; set; }

        [Required]
        [StringLength(700)]
        public string Address { get; set; } = "";

        [Required]
        public bool Gender { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public DateTime RegisterDate { get; set; }

        [NotMapped]
        public string? Password { get; set; }

        [NotMapped]
        [Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }

        public List<UsersPost>? UserPosts { get; set; }
        public List<CommentLike>? CommentLikes { get; set; }
        public List<PostLike>? PostLikes { get; set; }
    }
}
