using DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class csAddress : ISeed<csAddress>
    {
        [Key]
        public Guid AddressId { get; set; }
        public bool Seeded { get; set; } = true;

        public string AddressName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        [ForeignKey("AttractionId")]
        public Guid AttractionId { get; set; }
        public csAttraction csAttraction { get; set; }

        public csAddress()
        {
        }


        public csAddress Seed(csSeedGenerator sgen)
        {
            var al = new csAddress
            {
                AddressId = Guid.NewGuid(),
                AddressName = sgen.StreetAddress(Country),
                City = sgen.City(Country),
                Country = sgen.Country,
                Seeded = true
            };
            return al;
        }
    }
}
