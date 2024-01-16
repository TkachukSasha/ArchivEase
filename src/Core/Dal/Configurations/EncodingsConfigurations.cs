using Core.Encodings;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Abstractions;

namespace Core.Dal.Configurations;

internal sealed class EncodingConfigurations :
    IEntityTypeConfiguration<EncodingTable>,
    IEntityTypeConfiguration<EncodingLanguage>,
    IEntityTypeConfiguration<EncodingAlgorithm>
{
    public void Configure(EntityTypeBuilder<EncodingTable> builder)
    {
        builder.ToTable("EncodingTables");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new EncodingTableId(x));

        builder.OwnsMany(x => x.EncodingTableElements)
            .ToJson();

        builder.Property(x => x.RecordState).HasDefaultValue(RecordState.Active);
    }

    public void Configure(EntityTypeBuilder<EncodingLanguage> builder)
    {
        builder.ToTable("EncodingLanguages");

        builder.HasKey(x => x.Value);

        builder.HasIndex(x => x.Name, "idx_encoding_language_name");
    }

    public void Configure(EntityTypeBuilder<EncodingAlgorithm> builder)
    {
        builder.ToTable("EncodingAlgorithms");

        builder.HasKey(x => x.Value);

        builder.HasIndex(x => x.Name, "idx_encoding_algorithm_name");
    }
}