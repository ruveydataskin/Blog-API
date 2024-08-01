using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = "";

        public List<Post>? Posts { get; set; }
        public List<SubCategory>? SubCategories { get; set; }
    }
}
