using AutoMapper;
using LostAndFoundItems.Common;
using LostAndFoundItems.Common.DTOs;
using LostAndFoundItems.DAL;
using LostAndFoundItems.Models;

namespace LostAndFoundItems.BLL
{
    public class MatchStatusService
    {
        private readonly MatchStatusRepository _matchStatusRepository;
        private readonly IMapper _mapper;

        public MatchStatusService(MatchStatusRepository matchStatusRepository, IMapper mapper)
        {
            _matchStatusRepository = matchStatusRepository;
            _mapper = mapper;
        }

        public async Task<List<MatchStatusDTO>> GetAllMatchStatus()
        {
            List<MatchStatus> matchStatus = await _matchStatusRepository.GetAllMatchStatus();

            return _mapper.Map<List<MatchStatusDTO>>(matchStatus);
        }

        public async Task<MatchStatusDTO?> GetMatchStatusById(int id)
        {
            MatchStatus matchStatus = await _matchStatusRepository.GetMatchStatusById(id);

            if (matchStatus == null)
            {
                return null;
            }

            MatchStatusDTO matchStatusDTO = _mapper.Map<MatchStatusDTO>(matchStatus);

            return matchStatusDTO;
        }

        public async Task<(ServiceResult Result, MatchStatusDTO? CreatedMatchStatusDTO)> AddMatchStatus(MatchStatusWriteDTO matchStatusDTO)
        {
            try
            {
                MatchStatus matchStatus = _mapper.Map<MatchStatus>(matchStatusDTO);
                MatchStatus createdMatchStatus = await _matchStatusRepository.AddMatchStatus(matchStatus);
                MatchStatusDTO createdMatchStatusDTO = _mapper.Map<MatchStatusDTO>(createdMatchStatus);

                return (ServiceResult.Ok(), createdMatchStatusDTO);
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return (ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}"), null);
            }
        }

        public async Task<ServiceResult> UpdateMatchStatus(int id, MatchStatusWriteDTO matchStatusDTO)
        {
            try
            {
                MatchStatus matchStatus = _mapper.Map<MatchStatus>(matchStatusDTO);
                matchStatus.MatchStatusId = id;

                await _matchStatusRepository.UpdateMatchStatus(matchStatus);

                return ServiceResult.Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(Constants.NOT_FOUND_ERROR))
                {
                    return ServiceResult.Fail($"Match status {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }

                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return ServiceResult.Fail($"{Constants.UNEXPECTED_ERROR}{message}");
            }
        }

        public async Task<ServiceResult> DeleteMatchStatus(int id)
        {
            try
            {
                MatchStatus matchStatus = await _matchStatusRepository.GetMatchStatusById(id);

                if (matchStatus == null)
                {
                    return ServiceResult.Fail($"Match status {Constants.NOT_FOUND_ERROR}", Enums.Status.NotFound);
                }

                await _matchStatusRepository.DeleteMatchStatus(matchStatus);

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
