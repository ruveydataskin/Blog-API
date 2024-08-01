using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlogAPI.Models
{
	public class CommentLike
	{
        [Required]
        public long? CommentId { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [JsonIgnore]
        [ForeignKey(nameof(CommentId))]
        public Comment? Comment { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser? ApplicationUser { get; set; }
    }
}