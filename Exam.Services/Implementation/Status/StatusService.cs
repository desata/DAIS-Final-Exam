using Exam.Repository.Interfaces.Status;
using Exam.Repository.Interfaces.User;
using Exam.Services.DTOs.Status;
using Exam.Services.DTOs.User;
using Exam.Services.Interfaces.Status;

namespace Exam.Services.Implementation.Status
{
    public class StatusService : IStatusService
    {

        private readonly IStatusRepository _statusRepository;

        public StatusService(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public async Task<List<StatusInfo>> GetAllStatusesAsync()
        {
            var status = await _statusRepository.RetrieveCollectionAsync(new StatusFilter()).ToListAsync();

            return status.Select(MapStatusInfo).ToList();
        }

        public async Task<StatusInfo?> GetStatusByIdAsync(int statusId)
        {
            var status = await _statusRepository.RetrieveAsync(statusId);
            if (status == null)
            {
                return null;
            }

            return MapStatusInfo(status);

        }

        private StatusInfo MapStatusInfo(Models.Status status)
        {
            return new StatusInfo
            {
                StatusId = status.StatusId,
                Description = status.Description,
            };
        }
    
    }
}
