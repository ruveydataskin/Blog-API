using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = "";

        [Required]
        public string Content { get; set; } = "";

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public DateTime? DateUpdated { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

        public List<Comment>? Comments { get; set; }
        public List<PostTag>? PostTags { get; set; }
        public List<PostLike>? PostLikes { get; set; }
        public List<UsersPost>? UserPost { get; set; }

        public int LikeCount { get; set; }

        public int CommentCount { get; set; }

    }
}
