using System.ComponentModel.DataAnnotations;

namespace FinTrack.Services.Dtos
{
    public class CurrencyDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [StringLength (maximumLength: 3)]
        public string Code { get; set; }

        public string Symbol { get; set; }

        public bool IsDeleted { get; set; }
    }
}
