﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pokemons.DataLayer.Database;

#nullable disable

namespace Pokemons.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240731120540_aCtiveMissiosChanged")]
    partial class aCtiveMissiosChanged
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.ActiveMission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDifficult")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsEnded")
                        .HasColumnType("boolean");

                    b.Property<int>("Reward")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("ActiveMissions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsDifficult = false,
                            IsEnded = false,
                            Reward = 100
                        },
                        new
                        {
                            Id = 2,
                            IsDifficult = true,
                            IsEnded = false,
                            Reward = 100
                        },
                        new
                        {
                            Id = 3,
                            IsDifficult = true,
                            IsEnded = false,
                            Reward = 100
                        },
                        new
                        {
                            Id = 4,
                            IsDifficult = true,
                            IsEnded = false,
                            Reward = 100
                        });
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.ActiveNews", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("AddDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("ActiveNews");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Battle", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("BattleEndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("BattleStartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("BattleState")
                        .HasColumnType("integer");

                    b.Property<int>("EntityTypeId")
                        .HasColumnType("integer");

                    b.Property<long>("Health")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsGold")
                        .HasColumnType("boolean");

                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint");

                    b.Property<long>("RemainingHealth")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("Battles");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Guild", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("Balance")
                        .HasColumnType("integer");

                    b.Property<long>("GuildMasterId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PlayersCount")
                        .HasColumnType("integer");

                    b.Property<int>("TotalBalance")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Market", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("DamagePerClickCost")
                        .HasColumnType("bigint");

                    b.Property<int>("DamagePerClickLevel")
                        .HasColumnType("integer");

                    b.Property<int>("DamagePerClickNextValue")
                        .HasColumnType("integer");

                    b.Property<long>("EnergyChargeCost")
                        .HasColumnType("bigint");

                    b.Property<int>("EnergyChargeLevel")
                        .HasColumnType("integer");

                    b.Property<decimal>("EnergyChargeNextValue")
                        .HasColumnType("numeric");

                    b.Property<long>("EnergyCost")
                        .HasColumnType("bigint");

                    b.Property<int>("EnergyLevel")
                        .HasColumnType("integer");

                    b.Property<int>("EnergyNextValue")
                        .HasColumnType("integer");

                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint");

                    b.Property<long>("SuperChargeCooldownCost")
                        .HasColumnType("bigint");

                    b.Property<int>("SuperChargeCooldownLevel")
                        .HasColumnType("integer");

                    b.Property<decimal>("SuperChargeCooldownNextValue")
                        .HasColumnType("numeric");

                    b.Property<long>("SuperChargeCost")
                        .HasColumnType("bigint");

                    b.Property<int>("SuperChargeLevel")
                        .HasColumnType("integer");

                    b.Property<int>("SuperChargeNextValue")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId")
                        .IsUnique();

                    b.ToTable("Markets");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.MemberGuildStatus", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long?>("GuildId")
                        .HasColumnType("bigint");

                    b.Property<int>("MemberStatus")
                        .HasColumnType("integer");

                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.HasIndex("PlayerId")
                        .IsUnique();

                    b.ToTable("MemberGuildStatus");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Mission", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("ActiveMissionId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("CompleteTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ActiveMissionId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Missions");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.News", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("ActiveNewsId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean");

                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ActiveNewsId");

                    b.HasIndex("PlayerId");

                    b.ToTable("News");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Notification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("GuildMemberName")
                        .HasColumnType("text");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean");

                    b.Property<int>("NotificationType")
                        .HasColumnType("integer");

                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint");

                    b.Property<string>("ReferralName")
                        .HasColumnType("text");

                    b.Property<decimal?>("TopUpValue")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Player", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("Balance")
                        .HasColumnType("bigint");

                    b.Property<decimal>("CryptoBalance")
                        .HasColumnType("numeric");

                    b.Property<int>("CurrentEnergy")
                        .HasColumnType("integer");

                    b.Property<int>("DamagePerClick")
                        .HasColumnType("integer");

                    b.Property<int>("DefeatedEntities")
                        .HasColumnType("integer");

                    b.Property<int>("Energy")
                        .HasColumnType("integer");

                    b.Property<decimal>("EnergyCharge")
                        .HasColumnType("numeric");

                    b.Property<int>("Exp")
                        .HasColumnType("integer");

                    b.Property<long>("GoldBalance")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsFirstEntry")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsPremium")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastCommitDamageTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("LastSuperChargeActivatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Level")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("text");

                    b.Property<int>("SuperCharge")
                        .HasColumnType("integer");

                    b.Property<decimal>("SuperChargeCooldown")
                        .HasColumnType("numeric");

                    b.Property<string>("Surname")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("Taps")
                        .HasColumnType("integer");

                    b.Property<int>("TotalDamage")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Rating", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("GlobalRatingPosition")
                        .HasColumnType("bigint");

                    b.Property<long>("LeaguePosition")
                        .HasColumnType("bigint");

                    b.Property<int>("LeagueType")
                        .HasColumnType("integer");

                    b.Property<long>("PlayerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId")
                        .IsUnique();

                    b.ToTable("Rating");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.ReferralNode", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("BalanceValue")
                        .HasColumnType("integer");

                    b.Property<int>("Inline")
                        .HasColumnType("integer");

                    b.Property<long>("ReferralId")
                        .HasColumnType("bigint");

                    b.Property<long>("ReferrerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ReferralId");

                    b.HasIndex("ReferrerId");

                    b.ToTable("ReferralNodes");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Battle", b =>
                {
                    b.HasOne("Pokemons.DataLayer.Database.Models.Entities.Player", "Player")
                        .WithMany("Battles")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Market", b =>
                {
                    b.HasOne("Pokemons.DataLayer.Database.Models.Entities.Player", "Player")
                        .WithOne("Market")
                        .HasForeignKey("Pokemons.DataLayer.Database.Models.Entities.Market", "PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.MemberGuildStatus", b =>
                {
                    b.HasOne("Pokemons.DataLayer.Database.Models.Entities.Guild", "Guild")
                        .WithMany("Members")
                        .HasForeignKey("GuildId");

                    b.HasOne("Pokemons.DataLayer.Database.Models.Entities.Player", "Player")
                        .WithOne("GuildStatus")
                        .HasForeignKey("Pokemons.DataLayer.Database.Models.Entities.MemberGuildStatus", "PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guild");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Mission", b =>
                {
                    b.HasOne("Pokemons.DataLayer.Database.Models.Entities.ActiveMission", "ActiveMission")
                        .WithMany("Missions")
                        .HasForeignKey("ActiveMissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pokemons.DataLayer.Database.Models.Entities.Player", "Player")
                        .WithMany("Missions")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ActiveMission");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.News", b =>
                {
                    b.HasOne("Pokemons.DataLayer.Database.Models.Entities.ActiveNews", "ActiveNews")
                        .WithMany("PlayerNews")
                        .HasForeignKey("ActiveNewsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pokemons.DataLayer.Database.Models.Entities.Player", "Player")
                        .WithMany("News")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ActiveNews");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Notification", b =>
                {
                    b.HasOne("Pokemons.DataLayer.Database.Models.Entities.Player", "Player")
                        .WithMany("Notifications")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Rating", b =>
                {
                    b.HasOne("Pokemons.DataLayer.Database.Models.Entities.Player", "Player")
                        .WithOne("Rating")
                        .HasForeignKey("Pokemons.DataLayer.Database.Models.Entities.Rating", "PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.ReferralNode", b =>
                {
                    b.HasOne("Pokemons.DataLayer.Database.Models.Entities.Player", "Referral")
                        .WithMany("ReferrerInfo")
                        .HasForeignKey("ReferralId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pokemons.DataLayer.Database.Models.Entities.Player", "Referrer")
                        .WithMany("Referrals")
                        .HasForeignKey("ReferrerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Referral");

                    b.Navigation("Referrer");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.ActiveMission", b =>
                {
                    b.Navigation("Missions");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.ActiveNews", b =>
                {
                    b.Navigation("PlayerNews");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Guild", b =>
                {
                    b.Navigation("Members");
                });

            modelBuilder.Entity("Pokemons.DataLayer.Database.Models.Entities.Player", b =>
                {
                    b.Navigation("Battles");

                    b.Navigation("GuildStatus")
                        .IsRequired();

                    b.Navigation("Market")
                        .IsRequired();

                    b.Navigation("Missions");

                    b.Navigation("News");

                    b.Navigation("Notifications");

                    b.Navigation("Rating")
                        .IsRequired();

                    b.Navigation("Referrals");

                    b.Navigation("ReferrerInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
