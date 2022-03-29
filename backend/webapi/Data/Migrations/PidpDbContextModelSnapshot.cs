﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pidp.Data;

#nullable disable

namespace Pidp.Data.Migrations
{
    [DbContext(typeof(PidpDbContext))]
    partial class PidpDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Pidp.Models.AccessRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessType")
                        .HasColumnType("integer");

                    b.Property<Instant>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Instant>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("PartyId")
                        .HasColumnType("integer");

                    b.Property<Instant>("RequestedOn")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("PartyId");

                    b.ToTable("AccessRequest");
                });

            modelBuilder.Entity("Pidp.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasColumnType("text");

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

                    b.Property<string>("ProvinceCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CountryCode");

                    b.HasIndex("ProvinceCode");

                    b.ToTable("Address");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Address");
                });

            modelBuilder.Entity("Pidp.Models.EmailLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Cc")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Instant>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Instant?>("DateSent")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LatestStatus")
                        .HasColumnType("text");

                    b.Property<Instant>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("MsgId")
                        .HasColumnType("uuid");

                    b.Property<string>("SendType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SentTo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StatusMessage")
                        .HasColumnType("text");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UpdateCount")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("EmailLog");
                });

            modelBuilder.Entity("Pidp.Models.Facility", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Instant>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Instant>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

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

                    b.Property<string>("Acronym")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.ToTable("CollegeLookup");

                    b.HasData(
                        new
                        {
                            Code = 1,
                            Acronym = "CPSBC",
                            Name = "College of Physicians and Surgeons of BC"
                        },
                        new
                        {
                            Code = 2,
                            Acronym = "CPBC",
                            Name = "College of Pharmacists of BC"
                        },
                        new
                        {
                            Code = 3,
                            Acronym = "BCCNM",
                            Name = "BC College of Nurses and Midwives"
                        },
                        new
                        {
                            Code = 4,
                            Acronym = "CNPBC",
                            Name = "College of Naturopathic Physicians of BC"
                        },
                        new
                        {
                            Code = 5,
                            Acronym = "CDSBC",
                            Name = "College of Dental Surgeons of British Columbia"
                        },
                        new
                        {
                            Code = 6,
                            Acronym = "COBC",
                            Name = "College Of Optometrists of British Columbia"
                        });
                });

            modelBuilder.Entity("Pidp.Models.Lookups.Country", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.ToTable("CountryLookup");

                    b.HasData(
                        new
                        {
                            Code = "CA",
                            Name = "Canada"
                        },
                        new
                        {
                            Code = "US",
                            Name = "United States"
                        });
                });

            modelBuilder.Entity("Pidp.Models.Lookups.Province", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("CountryCode")
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
                            Code = "AB",
                            CountryCode = "CA",
                            Name = "Alberta"
                        },
                        new
                        {
                            Code = "BC",
                            CountryCode = "CA",
                            Name = "British Columbia"
                        },
                        new
                        {
                            Code = "MB",
                            CountryCode = "CA",
                            Name = "Manitoba"
                        },
                        new
                        {
                            Code = "NB",
                            CountryCode = "CA",
                            Name = "New Brunswick"
                        },
                        new
                        {
                            Code = "NL",
                            CountryCode = "CA",
                            Name = "Newfoundland and Labrador"
                        },
                        new
                        {
                            Code = "NS",
                            CountryCode = "CA",
                            Name = "Nova Scotia"
                        },
                        new
                        {
                            Code = "ON",
                            CountryCode = "CA",
                            Name = "Ontario"
                        },
                        new
                        {
                            Code = "PE",
                            CountryCode = "CA",
                            Name = "Prince Edward Island"
                        },
                        new
                        {
                            Code = "QC",
                            CountryCode = "CA",
                            Name = "Quebec"
                        },
                        new
                        {
                            Code = "SK",
                            CountryCode = "CA",
                            Name = "Saskatchewan"
                        },
                        new
                        {
                            Code = "NT",
                            CountryCode = "CA",
                            Name = "Northwest Territories"
                        },
                        new
                        {
                            Code = "NU",
                            CountryCode = "CA",
                            Name = "Nunavut"
                        },
                        new
                        {
                            Code = "YT",
                            CountryCode = "CA",
                            Name = "Yukon"
                        },
                        new
                        {
                            Code = "AL",
                            CountryCode = "US",
                            Name = "Alabama"
                        },
                        new
                        {
                            Code = "AK",
                            CountryCode = "US",
                            Name = "Alaska"
                        },
                        new
                        {
                            Code = "AS",
                            CountryCode = "US",
                            Name = "American Samoa"
                        },
                        new
                        {
                            Code = "AZ",
                            CountryCode = "US",
                            Name = "Arizona"
                        },
                        new
                        {
                            Code = "AR",
                            CountryCode = "US",
                            Name = "Arkansas"
                        },
                        new
                        {
                            Code = "CA",
                            CountryCode = "US",
                            Name = "California"
                        },
                        new
                        {
                            Code = "CO",
                            CountryCode = "US",
                            Name = "Colorado"
                        },
                        new
                        {
                            Code = "CT",
                            CountryCode = "US",
                            Name = "Connecticut"
                        },
                        new
                        {
                            Code = "DE",
                            CountryCode = "US",
                            Name = "Delaware"
                        },
                        new
                        {
                            Code = "DC",
                            CountryCode = "US",
                            Name = "District of Columbia"
                        },
                        new
                        {
                            Code = "FL",
                            CountryCode = "US",
                            Name = "Florida"
                        },
                        new
                        {
                            Code = "GA",
                            CountryCode = "US",
                            Name = "Georgia"
                        },
                        new
                        {
                            Code = "GU",
                            CountryCode = "US",
                            Name = "Guam"
                        },
                        new
                        {
                            Code = "HI",
                            CountryCode = "US",
                            Name = "Hawaii"
                        },
                        new
                        {
                            Code = "ID",
                            CountryCode = "US",
                            Name = "Idaho"
                        },
                        new
                        {
                            Code = "IL",
                            CountryCode = "US",
                            Name = "Illinois"
                        },
                        new
                        {
                            Code = "IN",
                            CountryCode = "US",
                            Name = "Indiana"
                        },
                        new
                        {
                            Code = "IA",
                            CountryCode = "US",
                            Name = "Iowa"
                        },
                        new
                        {
                            Code = "KS",
                            CountryCode = "US",
                            Name = "Kansas"
                        },
                        new
                        {
                            Code = "KY",
                            CountryCode = "US",
                            Name = "Kentucky"
                        },
                        new
                        {
                            Code = "LA",
                            CountryCode = "US",
                            Name = "Louisiana"
                        },
                        new
                        {
                            Code = "ME",
                            CountryCode = "US",
                            Name = "Maine"
                        },
                        new
                        {
                            Code = "MD",
                            CountryCode = "US",
                            Name = "Maryland"
                        },
                        new
                        {
                            Code = "MA",
                            CountryCode = "US",
                            Name = "Massachusetts"
                        },
                        new
                        {
                            Code = "MI",
                            CountryCode = "US",
                            Name = "Michigan"
                        },
                        new
                        {
                            Code = "MN",
                            CountryCode = "US",
                            Name = "Minnesota"
                        },
                        new
                        {
                            Code = "MS",
                            CountryCode = "US",
                            Name = "Mississippi"
                        },
                        new
                        {
                            Code = "MO",
                            CountryCode = "US",
                            Name = "Missouri"
                        },
                        new
                        {
                            Code = "MT",
                            CountryCode = "US",
                            Name = "Montana"
                        },
                        new
                        {
                            Code = "NE",
                            CountryCode = "US",
                            Name = "Nebraska"
                        },
                        new
                        {
                            Code = "NV",
                            CountryCode = "US",
                            Name = "Nevada"
                        },
                        new
                        {
                            Code = "NH",
                            CountryCode = "US",
                            Name = "New Hampshire"
                        },
                        new
                        {
                            Code = "NJ",
                            CountryCode = "US",
                            Name = "New Jersey"
                        },
                        new
                        {
                            Code = "NM",
                            CountryCode = "US",
                            Name = "New Mexico"
                        },
                        new
                        {
                            Code = "NY",
                            CountryCode = "US",
                            Name = "New York"
                        },
                        new
                        {
                            Code = "NC",
                            CountryCode = "US",
                            Name = "North Carolina"
                        },
                        new
                        {
                            Code = "ND",
                            CountryCode = "US",
                            Name = "North Dakota"
                        },
                        new
                        {
                            Code = "MP",
                            CountryCode = "US",
                            Name = "Northern Mariana Islands"
                        },
                        new
                        {
                            Code = "OH",
                            CountryCode = "US",
                            Name = "Ohio"
                        },
                        new
                        {
                            Code = "OK",
                            CountryCode = "US",
                            Name = "Oklahoma"
                        },
                        new
                        {
                            Code = "OR",
                            CountryCode = "US",
                            Name = "Oregon"
                        },
                        new
                        {
                            Code = "PA",
                            CountryCode = "US",
                            Name = "Pennsylvania"
                        },
                        new
                        {
                            Code = "PR",
                            CountryCode = "US",
                            Name = "Puerto Rico"
                        },
                        new
                        {
                            Code = "RI",
                            CountryCode = "US",
                            Name = "Rhode Island"
                        },
                        new
                        {
                            Code = "SC",
                            CountryCode = "US",
                            Name = "South Carolina"
                        },
                        new
                        {
                            Code = "SD",
                            CountryCode = "US",
                            Name = "South Dakota"
                        },
                        new
                        {
                            Code = "TN",
                            CountryCode = "US",
                            Name = "Tennessee"
                        },
                        new
                        {
                            Code = "TX",
                            CountryCode = "US",
                            Name = "Texas"
                        },
                        new
                        {
                            Code = "UM",
                            CountryCode = "US",
                            Name = "United States Minor Outlying Islands"
                        },
                        new
                        {
                            Code = "UT",
                            CountryCode = "US",
                            Name = "Utah"
                        },
                        new
                        {
                            Code = "VT",
                            CountryCode = "US",
                            Name = "Vermont"
                        },
                        new
                        {
                            Code = "VI",
                            CountryCode = "US",
                            Name = "Virgin Islands, U.S."
                        },
                        new
                        {
                            Code = "VA",
                            CountryCode = "US",
                            Name = "Virginia"
                        },
                        new
                        {
                            Code = "WA",
                            CountryCode = "US",
                            Name = "Washington"
                        },
                        new
                        {
                            Code = "WV",
                            CountryCode = "US",
                            Name = "West Virginia"
                        },
                        new
                        {
                            Code = "WI",
                            CountryCode = "US",
                            Name = "Wisconsin"
                        },
                        new
                        {
                            Code = "WY",
                            CountryCode = "US",
                            Name = "Wyoming"
                        });
                });

            modelBuilder.Entity("Pidp.Models.Party", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<LocalDate?>("Birthdate")
                        .HasColumnType("date");

                    b.Property<Instant>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Hpdid")
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

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Hpdid")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

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

                    b.Property<string>("Ipc")
                        .HasColumnType("text");

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

            modelBuilder.Entity("Pidp.Models.HcimReEnrolmentAccessRequest", b =>
                {
                    b.HasBaseType("Pidp.Models.AccessRequest");

                    b.Property<string>("LdapUsername")
                        .IsRequired()
                        .HasColumnType("text");

                    b.ToTable("HcimReEnrolmentAccessRequest");
                });

            modelBuilder.Entity("Pidp.Models.AccessRequest", b =>
                {
                    b.HasOne("Pidp.Models.Party", "Party")
                        .WithMany("AccessRequests")
                        .HasForeignKey("PartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Party");
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

            modelBuilder.Entity("Pidp.Models.HcimReEnrolmentAccessRequest", b =>
                {
                    b.HasOne("Pidp.Models.AccessRequest", null)
                        .WithOne()
                        .HasForeignKey("Pidp.Models.HcimReEnrolmentAccessRequest", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Pidp.Models.Facility", b =>
                {
                    b.Navigation("PhysicalAddress");
                });

            modelBuilder.Entity("Pidp.Models.Party", b =>
                {
                    b.Navigation("AccessRequests");

                    b.Navigation("Facility");

                    b.Navigation("PartyCertification");
                });
#pragma warning restore 612, 618
        }
    }
}
