using LostAndFoundItems.Common;
using LostAndFoundItems.Models;
using Microsoft.EntityFrameworkCore;

namespace LostAndFoundItems.DAL
{
    public class ClaimRequestRepository
    {
        private readonly LostAndFoundDbContext _context; 
        
        public ClaimRequestRepository(LostAndFoundDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClaimRequest>> GetAllClaimRequests()
        {
            return await _context.ClaimRequests
                .Include(fi => fi.FoundItem)
                .Include(u => u.User)
                .Include(cs => cs.ClaimStatus)
                .ToListAsync();
        }

        public async Task<ClaimRequest> GetClaimRequestById(int id)
        {
            return await _context.ClaimRequests
                .Include(fi => fi.FoundItem)
                .Include(u => u.User)
                .Include(cs => cs.ClaimStatus)
                .SingleOrDefaultAsync(r => r.ClaimRequestId == id);
        }

        public async Task<ClaimRequest> AddClaimRequest(ClaimRequest claimRequest)
        {
            _context.ClaimRequests.Add(claimRequest);
            claimRequest.User = await _context.Users.FindAsync(claimRequest.ClaimingUserId);
            claimRequest.ClaimStatus = await _context.ClaimStatuses.FindAsync(claimRequest.ClaimStatusId);
            await _context.SaveChangesAsync();

            return claimRequest;
        }


        public async Task UpdateClaimRequest(ClaimRequest claimRequest)
        {
            ClaimRequest existingClaimRequest = await _context.ClaimRequests.FirstOrDefaultAsync(r => r.ClaimRequestId == claimRequest.ClaimRequestId);

            if (existingClaimRequest == null)
            {
                throw new Exception(Constants.NOT_FOUND_ERROR);
            }

            existingClaimRequest.FoundItemId = claimRequest.FoundItemId;
            existingClaimRequest.ClaimingUserId = claimRequest.ClaimingUserId;
            existingClaimRequest.CreatedAt = claimRequest.CreatedAt;
            existingClaimRequest.ClaimStatusId = claimRequest.ClaimStatusId;
            existingClaimRequest.Message = claimRequest.Message;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteClaimRequest(ClaimRequest claimRequest)
        {
            if (claimRequest != null)
            {
                _context.ClaimRequests.Remove(claimRequest);
                await _context.SaveChangesAsync();
            }
        }
    }
}
