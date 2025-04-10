﻿using LostAndFoundItems.Common;
using LostAndFoundItems.Models;
using Microsoft.EntityFrameworkCore;

namespace LostAndFoundItems.DAL
{
    public class LocationRepository
    {
        private readonly LostAndFoundDbContext _context;

        public LocationRepository(LostAndFoundDbContext context)
        {
            _context = context;
        }

        public async Task<List<Location>> GetAllLocations()
        {
            return await _context.Locations
                .Include(l => l.FoundItems)
                .Include(l => l.LostItems)
                .ToListAsync();
        }

        public async Task<Location> GetLocationById(int id)
        {
            return await _context.Locations
                .Include(l => l.FoundItems)
                .Include(l => l.LostItems)
                .SingleOrDefaultAsync(r => r.LocationId == id);
        }

        public async Task<Location> AddLocation(Location location)
        {
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return location;
        }


        public async Task UpdateLocation(Location location)
        {
            Location existingLocation = await _context.Locations.FirstOrDefaultAsync(r => r.LocationId == location.LocationId);

            if (existingLocation == null)
            {
                throw new Exception(Constants.NOT_FOUND_ERROR);
            }

            existingLocation.Name = location.Name;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteLocation(Location location)
        {
            if (location != null)
            {
                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
            }
        }
    }
}