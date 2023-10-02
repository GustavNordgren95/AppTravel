using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using System.Linq;

using Configuration;
using Models;


namespace DbModels
{
    public enum AttractionCategory
    {
        Restaurant,
        WaterRide,
        Tour,
        Museum,
        Sport,
        Park,
        Monument
    }

    public class csAttraction : ISeed<csAttraction>
    {
        [Key]
        public Guid AttractionId { get; set; }
        public bool Seeded { get; set; } = true;

        [Required]
        public AttractionCategory Category { get; set; }

        [Required]
        public string strCategory
        {
            get => Category.ToString();
            set { }
        }

        public string AttractionName { get; set; }
        public string Description { get; set; }

        public csAddress csAddress { get; set; }
        public List<csComment> csComments { get; set; } = new List<csComment>();

        public csAttraction()
        {
        }

        public csAttraction Seed(csSeedGenerator rnd)
        {
            var att = new csAttraction
            {
                AttractionId = Guid.NewGuid(),
                AttractionName = rnd.AttractionName,
                Description = rnd.Description,
                Category = rnd.FromEnum<AttractionCategory>(),
                Seeded = true
            };
            return att;
        }
    }
}

