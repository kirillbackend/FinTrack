
namespace FinTrack.Model
{
    public abstract class FinanceBase
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public bool IsDeleted { get; set; }
    }
}
