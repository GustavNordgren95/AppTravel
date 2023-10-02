using System.Runtime.InteropServices;
using System.Security.Cryptography;
using DbContext;
using DbModels;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO;

namespace Services;

public class csAttractionService : IAttractionService
{
    public async Task<int> Seed()
    {
        var sg = new csSeedGenerator();

        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            var _users = new List<csUser>();
            for (int i = 0; i < 50; i++)
            {
                var usr = new csUser().Seed(sg);
                _users.Add(usr);
            }

            // Add the users to the database
            db.Users.AddRange(_users);
            await db.SaveChangesAsync();

            var _attractions = new List<csAttraction>();
            var _comments = new List<csComment>();

            for (int i = 0; i < 1000; i++)
            {
                var mg = new csAttraction().Seed(sg);

                var al = new csAddress().Seed(sg);
                mg.csAddress = al; // Set the single Address

                _attractions.Add(mg);

                var numberOfComments = sg.Next(0, 21); // Randomly generate 0 to 20 comments
                for (int j = 0; j < numberOfComments; j++)
                {
                    var comment = new csComment().Seed(sg);
                    comment.AttractionId = mg.AttractionId;
                    comment.csUser = sg.FromList(_users); // Assign a random user to the comment
                    _comments.Add(comment);
                }
            }

            // Add attractions and comments to the database
            db.Attractions.AddRange(_attractions);
            db.Comments.AddRange(_comments);
            await db.SaveChangesAsync();

            int cnt = await db.Attractions.CountAsync();
            return cnt;
        }
    }

    public async Task<int> RemoveSeed()
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            // Remove seeded comments
            db.Comments.RemoveRange(db.Comments.Where(comment => comment.Seeded));

            // Remove seeded attractions
            db.Attractions.RemoveRange(db.Attractions.Where(attraction => attraction.Seeded));

            // Remove seeded users
            db.Users.RemoveRange(db.Users.Where(user => user.Seeded));

            await db.SaveChangesAsync();

            int _count = await db.Attractions.CountAsync();
            return _count;
        }
    }

    public async Task csAttractionCUdto_To_Attraction_Navigation(csMainDbContext db,
        csAttractionCUdto _src, csAttraction _dst)
    {
        var id = _src.AddressId; // Assuming this property holds the Address ID to be updated

        var _address = await db.Addresses.FirstOrDefaultAsync(a => a.AddressId == id);

        if (_address == null)
            throw new ArgumentException($"Item id {id} not existing");

        _dst.csAddress = _address; // Update the single Address
    }

    public async Task<List<csAttraction>> FilterAttractions(string cityName = null, string category = null, string description = null, string attractionName = null, string countryName = null)
    {
        var query = csMainDbContext.DbContext("sysadmin").Attractions
            .AsQueryable();

        if (!string.IsNullOrEmpty(cityName))
        {
            query = query.Where(attraction => attraction.csAddress.City == cityName);
        }

        if (!string.IsNullOrEmpty(countryName))
        {
            query = query.Where(attraction => attraction.csAddress.Country == countryName);
        }

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(attraction => attraction.strCategory == category);
        }

        if (!string.IsNullOrEmpty(description))
        {
            query = query.Where(attraction => attraction.Description.Contains(description));
        }

        if (!string.IsNullOrEmpty(attractionName))
        {
            query = query.Where(attraction => attraction.AttractionName.Contains(attractionName));
        }

        var filteredAttractions = await query.ToListAsync();

        return filteredAttractions;
    }

    public async Task<List<csAttraction>> GetAttractionsWithNoCommentsAsync()
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            var attractionsWithNoComments = await db.Attractions
                .Where(attraction => attraction.csComments == null || !attraction.csComments.Any())
                .ToListAsync();

            return attractionsWithNoComments;
        }
    }

    public async Task<csAttraction> GetAttractionByIdAsync(Guid id)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            var attraction = await db.Attractions
                .Include(attraction => attraction.csAddress)
                .Include(attraction => attraction.csComments)
                .FirstOrDefaultAsync(attraction => attraction.AttractionId == id);

            return attraction;
        }
    }

    public async Task<List<csUser>> GetAllUsersAsync(int pageSize, int skip)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            var query = db.Users
                .Include(user => user.csComments)
                .OrderBy(user => user.UserId) // You can order by a specific property
                .Skip(skip)
                .Take(pageSize);

            var users = await query.ToListAsync();

            return users;
        }
    }
}



