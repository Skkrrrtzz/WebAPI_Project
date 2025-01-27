using EcommerceProductManagement.Repositories;
using static EcommerceProductManagement.DTOs.DTOs;
using static EcommerceProductManagement.Models.Models;

namespace EcommerceProductManagement.Services
{
    public class ReportsService
    {
        private readonly ReportsRepository _repository;

        public ReportsService(ReportsRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<ReportsProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            return _repository.GetProductsByCategoryAsync(categoryId);
        }

        public Task<IEnumerable<ReportsOrderDto>> GetOrdersFromLastMonthAsync()
        {
            return _repository.GetOrdersFromLastMonthAsync();
        }

        public Task<IEnumerable<ReportsProductSalesDto>> GetTotalSalesPerProductAsync()
        {
            return _repository.GetTotalSalesPerProductAsync();
        }

        public Task<IEnumerable<ReportsProductSalesDto>> GetTop5ProductsBySalesAsync()
        {
            return _repository.GetTop5ProductsBySalesAsync();
        }
    }
}