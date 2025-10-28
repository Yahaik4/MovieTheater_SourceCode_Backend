﻿using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Infrastructure.EF.Models;
using src.Infrastructure.Repositories.Interfaces;
using System.Xml.Linq;

namespace src.Infrastructure.Repositories
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly CinemaDbContext _context;

        public RoomTypeRepository(CinemaDbContext context)
        {
            _context = context;
        }

        public async Task<RoomType?> GetRoomTypeById(Guid id)
        {
            return await _context.RoomTypes.FirstOrDefaultAsync(rt => rt.Id == id);
        }

        public async Task<RoomType> CreateRoomType(RoomType roomType)
        {
            await _context.RoomTypes.AddAsync(roomType);
            await _context.SaveChangesAsync();
            return roomType;
        }

        public async Task<RoomType> UpdateRoomType(RoomType roomType)
        {
            _context.RoomTypes.Update(roomType);
            await _context.SaveChangesAsync();
            return roomType;
        }

        public async Task<IEnumerable<RoomType>> GetAllRoomType(Guid? id, string? type, decimal? basePrice)
        {
            var query = _context.RoomTypes.AsQueryable();

            if (id.HasValue)
                query = query.Where(rt => rt.Id == id);

            if (!string.IsNullOrWhiteSpace(type))
                query = query.Where(rt => rt.Type.ToLower().Contains(type.ToLower()));

            if (basePrice.HasValue)
                query = query.Where(rt => rt.BasePrice == basePrice);

            query = query.Where(x => x.IsDeleted == false);

            return await query.ToListAsync();
        }
    }
}
