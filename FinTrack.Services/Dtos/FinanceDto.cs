using FinTrack.Enums;

namespace FinTrack.Services.Dtos
{
    public class FinanceDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public int CurrencyId { get; set; }
        public bool IsDeleted { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public FinanceType FinanceType { get; set; }
        public FinanceCategoryType FinanceCategoryType { get; set; }
    }
}
