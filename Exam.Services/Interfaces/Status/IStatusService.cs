using Exam.Services.DTOs.Status;

namespace Exam.Services.Interfaces.Status
{
    public interface IStatusService
    {
        public Task<StatusInfo?> GetStatusByIdAsync(int statusId);

        //Not sure if needed
        public Task<List<StatusInfo>> GetAllStatusesAsync();
    }
}
