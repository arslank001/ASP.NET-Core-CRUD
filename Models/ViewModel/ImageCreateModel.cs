using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDCoreApplication.Models.ViewModel
{
    public class ImageCreateModel
    {
        public int ImageID { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please choose Image/File")]
        [Display(Name = "Choose Image")]
        public IFormFile ImagePath { get; set; }
    }
}
