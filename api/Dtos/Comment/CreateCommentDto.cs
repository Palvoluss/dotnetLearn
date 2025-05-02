using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comment
{
    public class CreateCommentDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Title must be between 3 and 100 characters"), MaxLength(100, ErrorMessage = "Title must be between 3 and 100 characters")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(500, ErrorMessage = "Content must be less than 500 characters")]
        public string Content { get; set; } = string.Empty;

    }
}