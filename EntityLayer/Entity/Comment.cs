using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entity
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string ProductPlatformID { get; set; }
        public string CommentText { get; set; }
        public string? ProductLink { get; set; }
        public string? Prediction { get; set; }
    }
}
