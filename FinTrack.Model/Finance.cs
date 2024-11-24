using FinTrack.Enums;

namespace FinTrack.Model
{
    public class Finance
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

        public Currency Currency { get; set; }
        public User User { get; set; }
    }
}
