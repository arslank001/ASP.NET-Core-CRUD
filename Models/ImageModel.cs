using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDCoreApplication.Models
{
    public class ImageModel
    {
        [Key]
        public int ImageID { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
    }
}
