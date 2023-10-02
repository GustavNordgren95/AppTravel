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
    public class csUser : ISeed<csUser>
    {
        [Key]
        public Guid UserId { get; set; }
        public bool Seeded { get; set; } = true;

        [Required]
        public string Username { get; set; }

        public List<csComment> csComments { get; set; } = new List<csComment>();

        public csUser()
        {
        }

        public csUser Seed(csSeedGenerator rnd)
        {
            var us = new csUser
            {
                UserId = Guid.NewGuid(),
                Username = rnd.FullName,
                Seeded = true
            };
            return us;
        }
    }


}
