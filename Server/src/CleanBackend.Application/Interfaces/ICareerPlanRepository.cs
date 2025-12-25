using CleanBackend.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanBackend.Application.Interfaces
{
    public interface ICareerPlanRepository
    {
       
        Task<CareerPlan> CreateAsync(CareerPlan careerPlan);
        Task<List<CareerPlan>> GetAllByUserIdAsync(string userId);
        Task<CareerPlan?> GetByIdAndUserIdAsync(int id, string userId);
        Task UpdateAsync(CareerPlan careerPlan); 
        Task DeleteAsync(CareerPlan careerPlan);
        Task<bool> SaveChangesAsync(); 
    }
}