using Core.Encodings;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Abstractions;

namespace Core.Dal.Configurations;

internal sealed class EncodingConfigurations :
    IEntityTypeConfiguration<EncodingTable>,
    IEntityTypeConfiguration<EncodingFile>,
    IEntityTypeConfiguration<EncodingLanguage>,
    IEntityTypeConfiguration<EncodingAlgorithm>
{
    public void Configure(EntityTypeBuilder<EncodingTable> builder)
    {
        builder.ToTable("encoding_tables");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new EncodingTableId(x));

        builder.OwnsMany(x => x.EncodingTableElements)
            .ToJson();

        builder.Property(x => x.RecordState).HasDefaultValue(RecordState.Active);
    }

    public void Configure(EntityTypeBuilder<EncodingFile> builder)
    {
        builder.ToTable("encoding_files");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new EncodingFileId(x));

        builder.Property(x => x.FileName)
            .IsRequired();

        builder.Property(x => x.FilePath)
            .IsRequired();

        builder.Property(x => x.FileUnitsOfMeasurement)
            .IsRequired();

        builder.Property(x => x.ContentType)
            .IsRequired();

        builder.Property(x => x.EncodedSize)
            .IsRequired();

        builder.Property(x => x.DefaultSize)
            .IsRequired();

        builder.Property(x => x.EncodingTableId)
            .IsRequired();
    }

    public void Configure(EntityTypeBuilder<EncodingLanguage> builder)
    {
        builder.ToTable("encoding_languages");

        builder.HasKey(x => x.Value);

        builder.HasIndex(x => x.Name, "idx_encoding_languages_name");
    }

    public void Configure(EntityTypeBuilder<EncodingAlgorithm> builder)
    {
        builder.ToTable("encoding_algorithms");

        builder.HasKey(x => x.Value);

        builder.HasIndex(x => x.Name, "idx_encoding_algorithms_name");
    }
}