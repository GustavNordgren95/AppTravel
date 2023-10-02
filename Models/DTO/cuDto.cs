using System;
using System.Collections.Generic;
using System.Linq;
using DbModels;

namespace Models.DTO
{
    public class csAttractionCUdto
    {
        public Guid AttractionId { get; set; }
        public bool Seeded { get; set; } = true;
        public string AttractionName { get; set; }
        public string Description { get; set; }
        public AttractionCategory Category { get; set; }
        public Guid? AddressId { get; set; }
        public List<Guid> CommentIds { get; set; }

        // Property for displaying comments in string format
        public string DisplayComments { get; set; }

        public csAttractionCUdto()
        {
        }

        public csAttractionCUdto(csAttraction model)
        {
            this.AttractionId = model.AttractionId;
            this.Seeded = model.Seeded;
            this.AttractionName = model.AttractionName;
            this.Description = model.Description;
            this.Category = model.Category;
            this.AddressId = model?.csAddress?.AddressId;
            this.CommentIds = model.csComments?.Select(comment => comment.CommentId).ToList();

            // Format comments as a string
            this.DisplayComments = model.csComments?.Any() == true
                ? string.Join(", ", model.csComments.Select(comment => comment.Comment))
                : "No comments available";
        }
    }
}



