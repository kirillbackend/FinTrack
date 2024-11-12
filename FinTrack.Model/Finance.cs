using FinTrack.Enums;

namespace FinTrack.Model
{
    public class Finance
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public bool IsDeleted { get; set; }
        public FinanceType FinanceType { get; set; }
    }
}
