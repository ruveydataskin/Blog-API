using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Models
{
    public class Comment
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = "";

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public long? CommentId { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        public string? UserId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post? Post { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser? ApplicationUser { get; set; }

        [ForeignKey(nameof(CommentId))]
        public Comment? ParentComment { get; set; }

        public List<Comment>? Replies { get; set; }

        public List<CommentLike>? CommentLikes { get; set; }

        public int LikeCount { get; set; }

        public int CommentCount { get; set; }
    }
}
