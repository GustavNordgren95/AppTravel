using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbModels;

namespace Models
{
    public class csComment : ISeed<csComment>
    {
        [Key]
        public Guid CommentId { get; set; }
        public bool Seeded { get; set; } = true;
        public string Comment { get; set; }

        [ForeignKey("AttractionId")]
        public Guid? AttractionId { get; set; }
        public csAttraction csAttraction { get; set; }

        [ForeignKey("UserId")]
        public Guid? UserId { get; set; }
        public csUser csUser { get; set; }



        public csComment()
        {
        }

        public csComment Seed(csSeedGenerator sgen)
        {
            var co = new csComment
            {
                CommentId = Guid.NewGuid(),
                Comment = sgen.CommentText,
                Seeded = true
            };
            return co;
        }
    }
}
