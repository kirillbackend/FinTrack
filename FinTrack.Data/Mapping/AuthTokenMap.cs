using FinTrack.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Data.Mapping
{
    public class AuthTokenMap : IEntityTypeConfiguration<AuthToken>
    {
        public void Configure(EntityTypeBuilder<AuthToken> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
