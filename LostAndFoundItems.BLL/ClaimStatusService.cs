using AutoMapper;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using LostAndFoundItems.DAL;
using LostAndFoundItems.Models;

namespace LostAndFoundItems.BLL
{
    public class ClaimStatusService
    {
        private readonly ClaimStatusRepository _claimStatusRepository;
        private readonly IMapper _mapper;

        public ClaimStatusService(ClaimStatusRepository claimStatusRepository, IMapper mapper)
        {
            _claimStatusRepository = claimStatusRepository;
            _mapper = mapper;
        }

        public async Task<List<ClaimStatusDTO>> GetAllClaimStatus()
        {
            List<ClaimStatus> claimStatus = await _claimStatusRepository.GetAllClaimStatus();

            return _mapper.Map<List<ClaimStatusDTO>>(claimStatus);
        }

        public async Task<ClaimStatusDTO?> GetClaimStatusById(int id)
        {
            ClaimStatus claimStatus = await _claimStatusRepository.GetClaimStatusById(id);

            if (claimStatus == null)
            {
                return null;
            }

            ClaimStatusDTO claimStatusDTO = _mapper.Map<ClaimStatusDTO>(claimStatus);

            return claimStatusDTO;
        }

        public async Task<(ServiceResult Result, ClaimStatusDTO? CreatedClaimStatusDTO)> AddClaimStatus(ClaimStatusWriteDTO claimStatusDTO)
        {
            try
            {
                ClaimStatus claimStatus = _mapper.Map<ClaimStatus>(claimStatusDTO);
                ClaimStatus createdClaimStatus = await _claimStatusRepository.AddClaimStatus(claimStatus);
                ClaimStatusDTO createdClaimStatusDTO = _mapper.Map<ClaimStatusDTO>(createdClaimStatus);

                return (ServiceResult.Ok(), createdClaimStatusDTO);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return (ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}"), null);
            }
        }

        public async Task<ServiceResult> UpdateClaimStatus(int id, ClaimStatusWriteDTO claimStatusDTO)
        {
            try
            {
                ClaimStatus claimStatus = _mapper.Map<ClaimStatus>(claimStatusDTO);
                claimStatus.ClaimStatusId = id;

                await _claimStatusRepository.UpdateClaimStatus(claimStatus);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(Constants.NOT_FOUND_ERROR))
                {
                    return ServiceResult.Fail($"Claim status {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }

                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}");
            }
        }

        public async Task<ServiceResult> DeleteClaimStatus(int id)
        {
            try
            {
                ClaimStatus claimStatus = await _claimStatusRepository.GetClaimStatusById(id);

                if (claimStatus == null)
                {
                    return ServiceResult.Fail($"Claim status {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }

                await _claimStatusRepository.DeleteClaimStatus(claimStatus);

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
