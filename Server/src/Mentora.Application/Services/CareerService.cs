//using Mentora.App.DTOs;
//using Mentora.Application.DTOs;
//using Mentora.Application.Interfaces;
//using Mentora.Domain.Entities;

//namespace Mentora.Application.Services
//{
//    public interface ICareerService
//    {
//        Task<CareerPlanViewDto> CreateAsync(string userId, CareerPlanCreateDto dto);

//        Task<List<CareerPlanViewDto>> GetAllAsync(string userId);

//        Task<CareerPlanViewDto?> UpdateAsync(int id, string userId, CareerPlanUpdateDto dto);

//        Task<bool> DeleteAsync(int id, string userId);
//    }

//    public class CareerService : ICareerService
//    {
//        private readonly ICareerPlanRepository _repository;

//        public CareerService(ICareerPlanRepository repository)
//        {
//            _repository = repository;
//        }

//        public async Task<CareerPlanViewDto> CreateAsync(string userId, CareerPlanCreateDto dto)
//        {
//            var plan = new CareerPlan
//            {
//                UserId = userId,
//                Title = dto.Title,
//                CurrentLevel = dto.CurrentLevel,
//                TargetLevel = dto.TargetLevel,
//                Description = dto.Description,
//                Steps = dto.Steps,
//                IsCompleted = false
//            };

//            await _repository.CreateAsync(plan);

//            return ToViewDto(plan);
//        }

//        public async Task<List<CareerPlanViewDto>> GetAllAsync(string userId)
//        {
//            var plans = await _repository.GetAllByUserIdAsync(userId);

//            return plans.Select(p => ToViewDto(p)).ToList();
//        }

//        public async Task<CareerPlanViewDto?> UpdateAsync(int id, string userId, CareerPlanUpdateDto dto)
//        {
//            var plan = await _repository.GetByIdAndUserIdAsync(id, userId);

//            if (plan == null) return null;

//            plan.Title = dto.Title;
//            plan.CurrentLevel = dto.CurrentLevel;
//            plan.TargetLevel = dto.TargetLevel;
//            plan.Description = dto.Description;
//            plan.Steps = dto.Steps;

//            if (dto.IsCompleted) plan.IsCompleted = true;

//            await _repository.UpdateAsync(plan);

//            return ToViewDto(plan);
//        }

//        public async Task<bool> DeleteAsync(int id, string userId)
//        {
//            var plan = await _repository.GetByIdAndUserIdAsync(id, userId);
//            if (plan == null) return false;

//            await _repository.DeleteAsync(plan);
//            return true;
//        }

//        private static CareerPlanViewDto ToViewDto(CareerPlan plan)
//        {
//            return new CareerPlanViewDto
//            {
//                Id = plan.Id,
//                Title = plan.Title,
//                CurrentLevel = plan.CurrentLevel,
//                TargetLevel = plan.TargetLevel,
//                Description = plan.Description,
//                Steps = plan.Steps,
//                IsCompleted = plan.IsCompleted,
//                CreatedAt = plan.CreatedAt
//            };
//        }
//    }
//}