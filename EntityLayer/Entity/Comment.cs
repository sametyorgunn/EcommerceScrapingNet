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
        [LoadColumn(0)]
        public string CommentText { get; set; }
        [LoadColumn(1)]
        public bool Label { get; set; }
    }
}
