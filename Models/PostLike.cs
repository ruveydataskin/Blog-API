using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Models
{
    public class PostLike
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [ForeignKey(nameof(PostId))]
        public Post? Post { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
