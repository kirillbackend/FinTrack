using FinTrack.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Data.Mapping
{
    public class FinanceMap : IEntityTypeConfiguration<Finance>
    {
        public void Configure(EntityTypeBuilder<Finance> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
