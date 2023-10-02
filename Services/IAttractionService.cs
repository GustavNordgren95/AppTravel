using System;
using DbModels;
using Models.DTO;
using Models;

namespace Services
{
    public interface IAttractionService
    {
        public Task<int> Seed();

        public Task<int> RemoveSeed();

        public Task<List<csAttraction>> FilterAttractions(string cityName, string categoryName, string description, string attractionName, string countryName);

        public Task<List<csAttraction>> GetAttractionsWithNoCommentsAsync();

        public Task<csAttraction> GetAttractionByIdAsync(Guid id);

        public Task<List<csUser>> GetAllUsersAsync(int pageSize, int skip);

    }
}

