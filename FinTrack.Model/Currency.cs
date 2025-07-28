
namespace FinTrack.Model
{
    public class Currency
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
        public bool IsDeleted { get; set; }

        public IEnumerable<Finance> Finances { get; set; } 
    }
}
