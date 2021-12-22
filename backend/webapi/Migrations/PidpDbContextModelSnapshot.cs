﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pidp.Data;

#nullable disable

namespace Pidp.Migrations
{
    [DbContext(typeof(PidpDbContext))]
    partial class PidpDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Pidp.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CountryCode")
                        .HasColumnType("integer");

                    b.Property<Instant>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Instant>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Postal")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ProvinceCode")
                        .HasColumnType("integer");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CountryCode");

                    b.HasIndex("ProvinceCode");

                    b.ToTable("Address");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Address");
                });

            modelBuilder.Entity("Pidp.Models.Facility", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Instant>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FacilityName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Instant>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("PartyId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PartyId")
                        .IsUnique();

                    b.ToTable("Facility");
                });

            modelBuilder.Entity("Pidp.Models.Lookups.College", b =>
                {
                    b.Property<int>("Code")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.ToTable("CollegeLookup");

                    b.HasData(
                        new
                        {
                            Code = 1,
                            Name = "College of Physicians and Surgeons of BC (CPSBC)"
                        },
                        new
                        {
                            Code = 2,
                            Name = "College of Pharmacists of BC (CPBC)"
                        },
                        new
                        {
                            Code = 3,
                            Name = "BC College of Nurses and Midwives (BCCNM)"
                        });
                });

            modelBuilder.Entity("Pidp.Models.Lookups.Country", b =>
                {
                    b.Property<int>("Code")
                        .HasColumnType("integer");

                    b.Property<string>("IsoCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.ToTable("CountryLookup");

                    b.HasData(
                        new
                        {
                            Code = 1,
                            IsoCode = "CA",
                            Name = "Canada"
                        },
                        new
                        {
                            Code = 2,
                            IsoCode = "US",
                            Name = "United States"
                        });
                });

            modelBuilder.Entity("Pidp.Models.Lookups.Province", b =>
                {
                    b.Property<int>("Code")
                        .HasColumnType("integer");

                    b.Property<int>("CountryCode")
                        .HasColumnType("integer");

                    b.Property<string>("IsoCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.ToTable("ProvinceLookup");

                    b.HasData(
                        new
                        {
                            Code = 1,
                            CountryCode = 1,
                            IsoCode = "AB",
                            Name = "Alberta"
                        },
                        new
                        {
                            Code = 2,
                            CountryCode = 1,
                            IsoCode = "BC",
                            Name = "British Columbia"
                        },
                        new
                        {
                            Code = 3,
                            CountryCode = 1,
                            IsoCode = "MB",
                            Name = "Manitoba"
                        },
                        new
                        {
                            Code = 4,
                            CountryCode = 1,
                            IsoCode = "NB",
                            Name = "New Brunswick"
                        },
                        new
                        {
                            Code = 5,
                            CountryCode = 1,
                            IsoCode = "NL",
                            Name = "Newfoundland and Labrador"
                        },
                        new
                        {
                            Code = 6,
                            CountryCode = 1,
                            IsoCode = "NS",
                            Name = "Nova Scotia"
                        },
                        new
                        {
                            Code = 7,
                            CountryCode = 1,
                            IsoCode = "ON",
                            Name = "Ontario"
                        },
                        new
                        {
                            Code = 8,
                            CountryCode = 1,
                            IsoCode = "PE",
                            Name = "Prince Edward Island"
                        },
                        new
                        {
                            Code = 9,
                            CountryCode = 1,
                            IsoCode = "QC",
                            Name = "Quebec"
                        },
                        new
                        {
                            Code = 10,
                            CountryCode = 1,
                            IsoCode = "SK",
                            Name = "Saskatchewan"
                        },
                        new
                        {
                            Code = 11,
                            CountryCode = 1,
                            IsoCode = "NT",
                            Name = "Northwest Territories"
                        },
                        new
                        {
                            Code = 12,
                            CountryCode = 1,
                            IsoCode = "NU",
                            Name = "Nunavut"
                        },
                        new
                        {
                            Code = 13,
                            CountryCode = 1,
                            IsoCode = "YT",
                            Name = "Yukon"
                        },
                        new
                        {
                            Code = 14,
                            CountryCode = 2,
                            IsoCode = "AL",
                            Name = "Alabama"
                        },
                        new
                        {
                            Code = 15,
                            CountryCode = 2,
                            IsoCode = "AK",
                            Name = "Alaska"
                        },
                        new
                        {
                            Code = 16,
                            CountryCode = 2,
                            IsoCode = "AS",
                            Name = "American Samoa"
                        },
                        new
                        {
                            Code = 17,
                            CountryCode = 2,
                            IsoCode = "AZ",
                            Name = "Arizona"
                        },
                        new
                        {
                            Code = 18,
                            CountryCode = 2,
                            IsoCode = "AR",
                            Name = "Arkansas"
                        },
                        new
                        {
                            Code = 19,
                            CountryCode = 2,
                            IsoCode = "CA",
                            Name = "California"
                        },
                        new
                        {
                            Code = 20,
                            CountryCode = 2,
                            IsoCode = "CO",
                            Name = "Colorado"
                        },
                        new
                        {
                            Code = 21,
                            CountryCode = 2,
                            IsoCode = "CT",
                            Name = "Connecticut"
                        },
                        new
                        {
                            Code = 22,
                            CountryCode = 2,
                            IsoCode = "DE",
                            Name = "Delaware"
                        },
                        new
                        {
                            Code = 23,
                            CountryCode = 2,
                            IsoCode = "DC",
                            Name = "District of Columbia"
                        },
                        new
                        {
                            Code = 24,
                            CountryCode = 2,
                            IsoCode = "FL",
                            Name = "Florida"
                        },
                        new
                        {
                            Code = 25,
                            CountryCode = 2,
                            IsoCode = "GA",
                            Name = "Georgia"
                        },
                        new
                        {
                            Code = 26,
                            CountryCode = 2,
                            IsoCode = "GU",
                            Name = "Guam"
                        },
                        new
                        {
                            Code = 27,
                            CountryCode = 2,
                            IsoCode = "HI",
                            Name = "Hawaii"
                        },
                        new
                        {
                            Code = 28,
                            CountryCode = 2,
                            IsoCode = "ID",
                            Name = "Idaho"
                        },
                        new
                        {
                            Code = 29,
                            CountryCode = 2,
                            IsoCode = "IL",
                            Name = "Illinois"
                        },
                        new
                        {
                            Code = 30,
                            CountryCode = 2,
                            IsoCode = "IN",
                            Name = "Indiana"
                        },
                        new
                        {
                            Code = 31,
                            CountryCode = 2,
                            IsoCode = "IA",
                            Name = "Iowa"
                        },
                        new
                        {
                            Code = 32,
                            CountryCode = 2,
                            IsoCode = "KS",
                            Name = "Kansas"
                        },
                        new
                        {
                            Code = 33,
                            CountryCode = 2,
                            IsoCode = "KY",
                            Name = "Kentucky"
                        },
                        new
                        {
                            Code = 34,
                            CountryCode = 2,
                            IsoCode = "LA",
                            Name = "Louisiana"
                        },
                        new
                        {
                            Code = 35,
                            CountryCode = 2,
                            IsoCode = "ME",
                            Name = "Maine"
                        },
                        new
                        {
                            Code = 36,
                            CountryCode = 2,
                            IsoCode = "MD",
                            Name = "Maryland"
                        },
                        new
                        {
                            Code = 37,
                            CountryCode = 2,
                            IsoCode = "MA",
                            Name = "Massachusetts"
                        },
                        new
                        {
                            Code = 38,
                            CountryCode = 2,
                            IsoCode = "MI",
                            Name = "Michigan"
                        },
                        new
                        {
                            Code = 39,
                            CountryCode = 2,
                            IsoCode = "MN",
                            Name = "Minnesota"
                        },
                        new
                        {
                            Code = 40,
                            CountryCode = 2,
                            IsoCode = "MS",
                            Name = "Mississippi"
                        },
                        new
                        {
                            Code = 41,
                            CountryCode = 2,
                            IsoCode = "MO",
                            Name = "Missouri"
                        },
                        new
                        {
                            Code = 42,
                            CountryCode = 2,
                            IsoCode = "MT",
                            Name = "Montana"
                        },
                        new
                        {
                            Code = 43,
                            CountryCode = 2,
                            IsoCode = "NE",
                            Name = "Nebraska"
                        },
                        new
                        {
                            Code = 44,
                            CountryCode = 2,
                            IsoCode = "NV",
                            Name = "Nevada"
                        },
                        new
                        {
                            Code = 45,
                            CountryCode = 2,
                            IsoCode = "NH",
                            Name = "New Hampshire"
                        },
                        new
                        {
                            Code = 46,
                            CountryCode = 2,
                            IsoCode = "NJ",
                            Name = "New Jersey"
                        },
                        new
                        {
                            Code = 47,
                            CountryCode = 2,
                            IsoCode = "NM",
                            Name = "New Mexico"
                        },
                        new
                        {
                            Code = 48,
                            CountryCode = 2,
                            IsoCode = "NY",
                            Name = "New York"
                        },
                        new
                        {
                            Code = 49,
                            CountryCode = 2,
                            IsoCode = "NC",
                            Name = "North Carolina"
                        },
                        new
                        {
                            Code = 50,
                            CountryCode = 2,
                            IsoCode = "ND",
                            Name = "North Dakota"
                        },
                        new
                        {
                            Code = 51,
                            CountryCode = 2,
                            IsoCode = "MP",
                            Name = "Northern Mariana Islands"
                        },
                        new
                        {
                            Code = 52,
                            CountryCode = 2,
                            IsoCode = "OH",
                            Name = "Ohio"
                        },
                        new
                        {
                            Code = 53,
                            CountryCode = 2,
                            IsoCode = "OK",
                            Name = "Oklahoma"
                        },
                        new
                        {
                            Code = 54,
                            CountryCode = 2,
                            IsoCode = "OR",
                            Name = "Oregon"
                        },
                        new
                        {
                            Code = 55,
                            CountryCode = 2,
                            IsoCode = "PA",
                            Name = "Pennsylvania"
                        },
                        new
                        {
                            Code = 56,
                            CountryCode = 2,
                            IsoCode = "PR",
                            Name = "Puerto Rico"
                        },
                        new
                        {
                            Code = 57,
                            CountryCode = 2,
                            IsoCode = "RI",
                            Name = "Rhode Island"
                        },
                        new
                        {
                            Code = 58,
                            CountryCode = 2,
                            IsoCode = "SC",
                            Name = "South Carolina"
                        },
                        new
                        {
                            Code = 59,
                            CountryCode = 2,
                            IsoCode = "SD",
                            Name = "South Dakota"
                        },
                        new
                        {
                            Code = 60,
                            CountryCode = 2,
                            IsoCode = "TN",
                            Name = "Tennessee"
                        },
                        new
                        {
                            Code = 61,
                            CountryCode = 2,
                            IsoCode = "TX",
                            Name = "Texas"
                        },
                        new
                        {
                            Code = 62,
                            CountryCode = 2,
                            IsoCode = "UM",
                            Name = "United States Minor Outlying Islands"
                        },
                        new
                        {
                            Code = 63,
                            CountryCode = 2,
                            IsoCode = "UT",
                            Name = "Utah"
                        },
                        new
                        {
                            Code = 64,
                            CountryCode = 2,
                            IsoCode = "VT",
                            Name = "Vermont"
                        },
                        new
                        {
                            Code = 65,
                            CountryCode = 2,
                            IsoCode = "VI",
                            Name = "Virgin Islands, U.S."
                        },
                        new
                        {
                            Code = 66,
                            CountryCode = 2,
                            IsoCode = "VA",
                            Name = "Virginia"
                        },
                        new
                        {
                            Code = 67,
                            CountryCode = 2,
                            IsoCode = "WA",
                            Name = "Washington"
                        },
                        new
                        {
                            Code = 68,
                            CountryCode = 2,
                            IsoCode = "WV",
                            Name = "West Virginia"
                        },
                        new
                        {
                            Code = 69,
                            CountryCode = 2,
                            IsoCode = "WI",
                            Name = "Wisconsin"
                        },
                        new
                        {
                            Code = 70,
                            CountryCode = 2,
                            IsoCode = "WY",
                            Name = "Wyoming"
                        });
                });

            modelBuilder.Entity("Pidp.Models.Party", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Instant>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<LocalDate>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("JobTitle")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Instant>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("PreferredFirstName")
                        .HasColumnType("text");

                    b.Property<string>("PreferredLastName")
                        .HasColumnType("text");

                    b.Property<string>("PreferredMiddleName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Party");
                });

            modelBuilder.Entity("Pidp.Models.PartyCertification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CollegeCode")
                        .HasColumnType("integer");

                    b.Property<Instant>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LicenceNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Instant>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("PartyId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CollegeCode");

                    b.HasIndex("PartyId")
                        .IsUnique();

                    b.ToTable("PartyCertification");
                });

            modelBuilder.Entity("Pidp.Models.FacilityAddress", b =>
                {
                    b.HasBaseType("Pidp.Models.Address");

                    b.Property<int>("FacilityId")
                        .HasColumnType("integer");

                    b.HasIndex("FacilityId")
                        .IsUnique();

                    b.ToTable("Address");

                    b.HasDiscriminator().HasValue("FacilityAddress");
                });

            modelBuilder.Entity("Pidp.Models.PartyAddress", b =>
                {
                    b.HasBaseType("Pidp.Models.Address");

                    b.Property<int>("PartyId")
                        .HasColumnType("integer");

                    b.HasIndex("PartyId")
                        .IsUnique();

                    b.ToTable("Address");

                    b.HasDiscriminator().HasValue("PartyAddress");
                });

            modelBuilder.Entity("Pidp.Models.Address", b =>
                {
                    b.HasOne("Pidp.Models.Lookups.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pidp.Models.Lookups.Province", "Province")
                        .WithMany()
                        .HasForeignKey("ProvinceCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");

                    b.Navigation("Province");
                });

            modelBuilder.Entity("Pidp.Models.Facility", b =>
                {
                    b.HasOne("Pidp.Models.Party", "Party")
                        .WithOne("Facility")
                        .HasForeignKey("Pidp.Models.Facility", "PartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Party");
                });

            modelBuilder.Entity("Pidp.Models.PartyCertification", b =>
                {
                    b.HasOne("Pidp.Models.Lookups.College", "College")
                        .WithMany()
                        .HasForeignKey("CollegeCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pidp.Models.Party", "Party")
                        .WithOne("PartyCertification")
                        .HasForeignKey("Pidp.Models.PartyCertification", "PartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("College");

                    b.Navigation("Party");
                });

            modelBuilder.Entity("Pidp.Models.FacilityAddress", b =>
                {
                    b.HasOne("Pidp.Models.Facility", "Facility")
                        .WithOne("PhysicalAddress")
                        .HasForeignKey("Pidp.Models.FacilityAddress", "FacilityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Facility");
                });

            modelBuilder.Entity("Pidp.Models.PartyAddress", b =>
                {
                    b.HasOne("Pidp.Models.Party", "Party")
                        .WithOne("MailingAddress")
                        .HasForeignKey("Pidp.Models.PartyAddress", "PartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Party");
                });

            modelBuilder.Entity("Pidp.Models.Facility", b =>
                {
                    b.Navigation("PhysicalAddress");
                });

            modelBuilder.Entity("Pidp.Models.Party", b =>
                {
                    b.Navigation("Facility");

                    b.Navigation("MailingAddress");

                    b.Navigation("PartyCertification");
                });
#pragma warning restore 612, 618
        }
    }
}
