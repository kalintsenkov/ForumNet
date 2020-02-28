﻿namespace ForumNet.Web.ViewModels.Posts
{
    using System.ComponentModel.DataAnnotations;

    public class PostsInputViewModel
    {
        [Required]
        [MaxLength(130, ErrorMessage = "The {0} must be max {1} characters long.")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Post Type")]
        public string PostType { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = "Url")]
        public string ImageOrVideoUrl { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string Tags { get; set; }
    }
}