using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entity
{
    public class Comment
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string CommentText { get; set; }
        public string? ProductLink { get; set; }
    }
}
