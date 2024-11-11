namespace FinTrack.Services.Dtos
{
    public class CurrencyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
        public bool IsDeleted { get; set; }
    }
}
