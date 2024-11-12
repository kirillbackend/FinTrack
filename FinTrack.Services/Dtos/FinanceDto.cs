using FinTrack.Enums;
using FinTrack.Model;

namespace FinTrack.Services.Dtos
{
    public class FinanceDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public bool IsDeleted { get; set; }
        public FinanceType FinanceType { get; set; }
    }
}
