using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlogAPI.Models
{
	public class UsersPost
	{
        public string? UsersId { get; set; }

        public int PostId { get; set; }

        [ForeignKey(nameof(UsersId))]
        public ApplicationUser? User { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(PostId))]
        public Post? Post { get; set; }
    }
}

