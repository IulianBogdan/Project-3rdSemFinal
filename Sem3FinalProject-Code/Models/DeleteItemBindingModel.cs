using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sem3FinalProject_Code.Models
{
    public class DeleteItemBindingModel
    {
        [Required]
        public string ProductNumber { get; set; }
    }
}