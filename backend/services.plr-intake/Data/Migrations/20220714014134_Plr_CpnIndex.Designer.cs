﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PlrIntake.Data;

#nullable disable

namespace PlrIntake.Data.Migrations
{
    [DbContext(typeof(PlrDbContext))]
    [Migration("20220714014134_Plr_CpnIndex")]
    partial class Plr_CpnIndex
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PlrIntake.Models.IdentifierType", b =>
                {
                    b.Property<string>("Oid")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Oid");

                    b.ToTable("Plr_IdentifierType");

                    b.HasData(
                        new
                        {
                            Oid = "2.16.840.1.113883.3.40.2.19",
                            Name = "RNID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.3.40.2.20",
                            Name = "RNPID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.4.608",
                            Name = "RPNRC"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.3.40.2.14",
                            Name = "PHID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.4.454",
                            Name = "RACID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.3.40.2.18",
                            Name = "RMID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.3.40.2.10",
                            Name = "LPNID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.3.40.2.4",
                            Name = "CPSID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.4.429",
                            Name = "OPTID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.3.40.2.6",
                            Name = "DENID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.4.363",
                            Name = "CCID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.4.364",
                            Name = "OTID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.4.362",
                            Name = "PSYCHID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.4.361",
                            Name = "SWID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.4.422",
                            Name = "CHIROID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.4.414",
                            Name = "PHYSIOID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.4.433",
                            Name = "RMTID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.4.439",
                            Name = "KNID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.4.401",
                            Name = "PHTID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.4.477",
                            Name = "COUNID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.4.452",
                            Name = "MFTID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.4.530",
                            Name = "RDID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.3.40.2.46",
                            Name = "MOAID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.3.40.2.44",
                            Name = "PPID"
                        },
                        new
                        {
                            Oid = "2.16.840.1.113883.4.538",
                            Name = "NDID"
                        });
                });

            modelBuilder.Entity("PlrIntake.Models.PlrRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address1Line1")
                        .HasColumnType("text");

                    b.Property<string>("Address1Line2")
                        .HasColumnType("text");

                    b.Property<string>("Address1Line3")
                        .HasColumnType("text");

                    b.Property<DateTime?>("Address1StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("City1")
                        .HasColumnType("text");

                    b.Property<string>("CollegeId")
                        .HasColumnType("text");

                    b.Property<string>("ConditionCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ConditionEndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("ConditionStartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Country1")
                        .HasColumnType("text");

                    b.Property<string>("Cpn")
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FaxAreaCode")
                        .HasColumnType("text");

                    b.Property<string>("FaxNumber")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("Gender")
                        .HasColumnType("text");

                    b.Property<string>("IdentifierType")
                        .HasColumnType("text");

                    b.Property<string>("Ipc")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("MspId")
                        .HasColumnType("text");

                    b.Property<string>("NamePrefix")
                        .HasColumnType("text");

                    b.Property<string>("PostalCode1")
                        .HasColumnType("text");

                    b.Property<string>("ProviderRoleType")
                        .HasColumnType("text");

                    b.Property<string>("Province1")
                        .HasColumnType("text");

                    b.Property<string>("SecondName")
                        .HasColumnType("text");

                    b.Property<string>("StatusCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("StatusExpiryDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("StatusReasonCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("StatusStartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Suffix")
                        .HasColumnType("text");

                    b.Property<string>("TelephoneAreaCode")
                        .HasColumnType("text");

                    b.Property<string>("TelephoneNumber")
                        .HasColumnType("text");

                    b.Property<string>("ThirdName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Cpn");

                    b.HasIndex("Ipc")
                        .IsUnique();

                    b.ToTable("Plr_PlrRecord");
                });

            modelBuilder.Entity("PlrIntake.Models.StatusChageLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("NewStatusCode")
                        .HasColumnType("text");

                    b.Property<string>("NewStatusReasonCode")
                        .HasColumnType("text");

                    b.Property<string>("OldStatusCode")
                        .HasColumnType("text");

                    b.Property<string>("OldStatusReasonCode")
                        .HasColumnType("text");

                    b.Property<int>("PlrRecordId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PlrRecordId");

                    b.ToTable("Plr_StatusChageLog");
                });

            modelBuilder.Entity("PlrIntake.Models.PlrRecord", b =>
                {
                    b.OwnsMany("PlrIntake.Models.Credential", "Credentials", b1 =>
                        {
                            b1.Property<int>("PlrRecordId")
                                .HasColumnType("integer");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<DateTime>("Created")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<DateTime>("Modified")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("PlrRecordId", "Id");

                            b1.ToTable("Plr_Credential", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("PlrRecordId");
                        });

                    b.OwnsMany("PlrIntake.Models.Expertise", "Expertise", b1 =>
                        {
                            b1.Property<int>("PlrRecordId")
                                .HasColumnType("integer");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<string>("Code")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<DateTime>("Created")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<DateTime>("Modified")
                                .HasColumnType("timestamp without time zone");

                            b1.HasKey("PlrRecordId", "Id");

                            b1.ToTable("Plr_Expertise", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("PlrRecordId");
                        });

                    b.Navigation("Credentials");

                    b.Navigation("Expertise");
                });

            modelBuilder.Entity("PlrIntake.Models.StatusChageLog", b =>
                {
                    b.HasOne("PlrIntake.Models.PlrRecord", "PlrRecord")
                        .WithMany()
                        .HasForeignKey("PlrRecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlrRecord");
                });
#pragma warning restore 612, 618
        }
    }
}
