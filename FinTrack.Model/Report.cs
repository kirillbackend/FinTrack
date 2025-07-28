
namespace FinTrack.Model
{
    public class Report
    {
        public decimal Amount { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
