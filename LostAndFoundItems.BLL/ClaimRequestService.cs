using AutoMapper;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using LostAndFoundItems.DAL;
using LostAndFoundItems.Models;

namespace LostAndFoundItems.BLL
{
    public class ClaimRequestService
    {
        private readonly ClaimRequestRepository _claimRequestRepository;
        private readonly IMapper _mapper;

        public ClaimRequestService(ClaimRequestRepository claimRequestRepository, IMapper mapper)
        {
            _claimRequestRepository = claimRequestRepository;
            _mapper = mapper;
        }

        public async Task<List<ClaimRequestDTO>> GetAllClaimRequests()
        {
            List<ClaimRequest> claimRequests = await _claimRequestRepository.GetAllClaimRequests();
            List<ClaimRequestDTO> claimRequestsDTO = claimRequests
                                                     .Select(cr => new ClaimRequestDTO
                                                     {
                                                         ClaimRequestId = cr.ClaimRequestId,
                                                         FoundItemId = cr.FoundItemId,
                                                         ClaimingUserId = cr.ClaimingUserId,
                                                         ClaimingUserFullName = $"{cr.User.FirstName} {cr.User.LastName}",
                                                         ClaimStatusId = cr.ClaimStatusId,
                                                         ClaimStatusName = cr.ClaimStatus.Name,
                                                         FoundItemTitle = cr.FoundItem.Title,
                                                         CreatedAt = cr.CreatedAt,
                                                         Message = cr.Message
                                                     }).ToList();

            return claimRequestsDTO;
        }

        public async Task<ClaimRequestDTO?> GetClaimRequestById(int id)
        {
            ClaimRequest claimRequest = await _claimRequestRepository.GetClaimRequestById(id);

            if (claimRequest == null)
            {
                return null;
            }

            ClaimRequestDTO claimRequestDTO = new ClaimRequestDTO()
            {
                ClaimRequestId = claimRequest.ClaimRequestId,
                FoundItemId = claimRequest.FoundItemId,
                ClaimingUserId = claimRequest.ClaimingUserId,
                ClaimingUserFullName = $"{claimRequest.User.FirstName} {claimRequest.User.LastName}",
                ClaimStatusId = claimRequest.ClaimStatusId,
                ClaimStatusName = claimRequest.ClaimStatus.Name,
                FoundItemTitle = claimRequest.FoundItem.Title,
                CreatedAt = claimRequest.CreatedAt,
                Message = claimRequest.Message
            };

            return claimRequestDTO;
        }

        public async Task<(ServiceResult Result, ClaimRequestDTO? CreatedClaimRequestDTO)> AddClaimRequest(ClaimRequestWriteDTO claimRequestDTO)
        {
            try
            {
                ClaimRequest claimRequest = _mapper.Map<ClaimRequest>(claimRequestDTO);
                ClaimRequest createdClaimRequest = await _claimRequestRepository.AddClaimRequest(claimRequest);
                ClaimRequestDTO createdClaimRequestDTO = new ClaimRequestDTO()
                {
                    ClaimRequestId = createdClaimRequest.ClaimRequestId,
                    FoundItemId = createdClaimRequest.FoundItemId,
                    ClaimingUserId = createdClaimRequest.ClaimingUserId,
                    ClaimingUserFullName = $"{createdClaimRequest.User.FirstName} {createdClaimRequest.User.LastName}",
                    CreatedAt = createdClaimRequest.CreatedAt,
                    ClaimStatusId = createdClaimRequest.ClaimStatusId,
                    ClaimStatusName = createdClaimRequest.ClaimStatus.Name,
                    Message = createdClaimRequest.Message
                };

                return (ServiceResult.Ok(), createdClaimRequestDTO);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return (ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}"), null);
            }
        }

        public async Task<ServiceResult> UpdateClaimRequest(int id, ClaimRequestWriteDTO claimRequestDTO)
        {
            try
            {
                ClaimRequest claimRequest = _mapper.Map<ClaimRequest>(claimRequestDTO);
                claimRequest.ClaimRequestId = id;

                await _claimRequestRepository.UpdateClaimRequest(claimRequest);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(Constants.NOT_FOUND_ERROR))
                {
                    return ServiceResult.Fail($"Claim request {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }

                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}");
            }
        }

        public async Task<ServiceResult> DeleteClaimRequest(int id)
        {
            try
            {
                ClaimRequest claimRequest = await _claimRequestRepository.GetClaimRequestById(id);

                if (claimRequest == null)
                {
                    return ServiceResult.Fail($"Claim request {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }

                await _claimRequestRepository.DeleteClaimRequest(claimRequest);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}");
            }
        }
    }
}
