using Mentora.Application.DTOs.CareerBuilder;

namespace Mentora.Application.Interfaces.Services
{
    public interface ICareerPlanService
    {
        Task<CareerPlanGeneratedDto> GenerateCareerPlanAsync(Guid userId, GenerateCareerPlanDto dto);
        Task<List<CareerPlanListDto>> GetAllPlansAsync(Guid userId);
        Task<CareerPlanDetailDto?> GetPlanDetailsAsync(Guid userId, Guid planId);
    }
}
