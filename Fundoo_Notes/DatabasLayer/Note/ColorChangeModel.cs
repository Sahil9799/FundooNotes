using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DatabasLayer.Note
{
    public class ColorChangeModel
    {
        [Required]
        public string Colour { get; set; }
    }
}
