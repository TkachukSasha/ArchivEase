using Core.Encodings;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Abstractions;

namespace Core.Dal.Configurations;

internal sealed class EncodingConfigurations :
    IEntityTypeConfiguration<EncodingTable>,
    IEntityTypeConfiguration<EncodingFile>,
    IEntityTypeConfiguration<EncodingTraining>,
    IEntityTypeConfiguration<EncodingLanguage>,
    IEntityTypeConfiguration<EncodingAlgorithm>
{
    public void Configure(EntityTypeBuilder<EncodingTable> builder)
    {
        builder.ToTable("encoding_tables");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new EncodingTableId(x));

        builder.Property(x => x.EncodingLanguageId)
            .HasComment("not always predicted");

        builder.Property(x => x.EncodingAlgorithmId)
            .IsRequired();

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

        builder.Property(x => x.EncodedFileUnitsOfMeasurement)
            .IsRequired();

        builder.Property(x => x.DefaultFileUnitsOfMeasurement)
          .IsRequired();

        builder.Property(x => x.ContentType)
            .IsRequired();

        builder.Property(x => x.EncodedSize)
            .IsRequired();

        builder.Property(x => x.DefaultSize)
            .IsRequired();

        builder.Property(x => x.EncodingTableId)
            .HasColumnName("EncodingTableId")
            .IsRequired();
    }

    public void Configure(EntityTypeBuilder<EncodingTraining> builder)
    {
        builder.ToTable("encoding_trainings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new EncodingTrainingId(x));

        builder.Property(x => x.Content)
            .IsRequired();

        builder.Property(x => x.Language)
            .IsRequired();

        builder.Property(x => x.Algorithm)
            .IsRequired();

        builder.HasIndex(x => x.Algorithm, "idx_encoding_trainings_algorithm");
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