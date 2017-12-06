using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sem3FinalProject_Code.Models
{
    /// <summary>
    /// Item binding model for add, update and get
    /// </summary>
    public class ItemBindingModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ProductNumber { get; set; }
        [Required]
        public string ItemTypeName { get; set; }
        [Required]
        public IDictionary<string, string> Properties { get; set; }
    }
}