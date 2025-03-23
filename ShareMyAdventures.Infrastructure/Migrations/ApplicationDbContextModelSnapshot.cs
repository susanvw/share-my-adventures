﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShareMyAdventures.Infrastructure.Persistence;

#nullable disable

namespace ShareMyAdventures.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FriendListParticipant", b =>
                {
                    b.Property<long>("FriendListsId")
                        .HasColumnType("bigint");

                    b.Property<string>("FriendsId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("FriendListsId", "FriendsId");

                    b.HasIndex("FriendsId");

                    b.ToTable("FriendListParticipant");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ShareMyAdventures.Domain.Entities.AdventureAggregate.Adventure", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("DestinationLocationId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("MeetupLocationId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DestinationLocationId");

                    b.HasIndex("MeetupLocationId");

                    b.ToTable("Adventures");
                });

            modelBuilder.Entity("ShareMyAdventures.Domain.Entities.AdventureAggregate.ParticipantAdventure", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("AdventureId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Distance")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(0.0);

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParticipantId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AdventureId");

                    b.HasIndex("ParticipantId");

                    b.ToTable("ParticipantAdventures");
                });

            modelBuilder.Entity("ShareMyAdventures.Domain.Entities.InvitationAggregate.AdventureInvitation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("AdventureId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AdventureId");

                    b.ToTable("AdventureInvitations");
                });

            modelBuilder.Entity("ShareMyAdventures.Domain.Entities.LocationAggregate.Location", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("AddressLine1")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("AddressLine2")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("City")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Latitude")
                        .HasColumnType("float");

                    b.Property<double?>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("PostalCode")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("Province")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Suburb")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("ShareMyAdventures.Domain.Entities.ParticipantAggregate.FriendList", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.ToTable("FriendLists");
                });

            modelBuilder.Entity("ShareMyAdventures.Domain.Entities.ParticipantAggregate.FriendRequest", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParticipantFriendId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ParticipantId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ParticipantId");

                    b.HasIndex("ParticipantFriendId", "ParticipantId")
                        .IsUnique();

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("ShareMyAdventures.Domain.Entities.ParticipantAggregate.Notification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsSent")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParticipantId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ParticipantId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("ShareMyAdventures.Domain.Entities.ParticipantAggregate.Participant", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("FollowMe")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrailColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("ShareMyAdventures.Domain.Entities.ParticipantAggregate.Position", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("ActivityType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Altitude")
                        .HasColumnType("float");

                    b.Property<double?>("BatteryLevel")
                        .HasColumnType("float");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Heading")
                        .HasColumnType("float");

                    b.Property<bool>("IsMoving")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<double?>("Odometer")
                        .HasColumnType("float");

                    b.Property<string>("ParticipantId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double?>("Speed")
                        .HasColumnType("float");

                    b.Property<string>("TimeStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Uuid")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ParticipantId");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("FriendListParticipant", b =>
                {
                    b.HasOne("ShareMyAdventures.Domain.Entities.ParticipantAggregate.FriendList", null)
                        .WithMany()
                        .HasForeignKey("FriendListsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShareMyAdventures.Domain.Entities.ParticipantAggregate.Participant", null)
                        .WithMany()
                        .HasForeignKey("FriendsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ShareMyAdventures.Domain.Entities.ParticipantAggregate.Participant", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ShareMyAdventures.Domain.Entities.ParticipantAggregate.Participant", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShareMyAdventures.Domain.Entities.ParticipantAggregate.Participant", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ShareMyAdventures.Domain.Entities.ParticipantAggregate.Participant", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ShareMyAdventures.Domain.Entities.AdventureAggregate.Adventure", b =>
                {
                    b.HasOne("ShareMyAdventures.Domain.Entities.LocationAggregate.Location", "DestinationLocationLookup")
                        .WithMany()
                        .HasForeignKey("DestinationLocationId");

                    b.HasOne("ShareMyAdventures.Domain.Entities.LocationAggregate.Location", "MeetupLocationLookup")
                        .WithMany()
                        .HasForeignKey("MeetupLocationId");

                    b.OwnsOne("ShareMyAdventures.Domain.Entities.AdventureAggregate.StatusLookup", "StatusLookup", b1 =>
                        {
                            b1.Property<long>("AdventureId")
                                .HasColumnType("bigint");

                            b1.Property<int>("Id")
                                .HasColumnType("int")
                                .HasColumnName("StatusLookupId");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("StatusLookupName");

                            b1.HasKey("AdventureId");

                            b1.ToTable("Adventures");

                            b1.WithOwner()
                                .HasForeignKey("AdventureId");
                        });

                    b.OwnsOne("ShareMyAdventures.Domain.Entities.AdventureAggregate.TypeLookup", "TypeLookup", b1 =>
                        {
                            b1.Property<long>("AdventureId")
                                .HasColumnType("bigint");

                            b1.Property<int>("Id")
                                .HasColumnType("int")
                                .HasColumnName("TypeLookupId");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("TypeLookupName");

                            b1.HasKey("AdventureId");

                            b1.ToTable("Adventures");

                            b1.WithOwner()
                                .HasForeignKey("AdventureId");
                        });

                    b.Navigation("DestinationLocationLookup");

                    b.Navigation("MeetupLocationLookup");

                    b.Navigation("StatusLookup")
                        .IsRequired();

                    b.Navigation("TypeLookup")
                        .IsRequired();
                });

            modelBuilder.Entity("ShareMyAdventures.Domain.Entities.AdventureAggregate.ParticipantAdventure", b =>
                {
                    b.HasOne("ShareMyAdventures.Domain.Entities.AdventureAggregate.Adventure", "Adventure")
                        .WithMany("Participants")
                        .HasForeignKey("AdventureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShareMyAdventures.Domain.Entities.ParticipantAggregate.Participant", "Participant")
                        .WithMany("Adventures")
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("ShareMyAdventures.Domain.Entities.AdventureAggregate.AccessLevelLookup", "AccessLevelLookup", b1 =>
                        {
                            b1.Property<long>("ParticipantAdventureId")
                                .HasColumnType("bigint");

                            b1.Property<int>("Id")
                                .HasColumnType("int")
                                .HasColumnName("AccessLevelLookupId");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("AccessLevelLookupName");

                            b1.HasKey("ParticipantAdventureId");

                            b1.ToTable("ParticipantAdventures");

                            b1.WithOwner()
                                .HasForeignKey("ParticipantAdventureId");
                        });

                    b.Navigation("AccessLevelLookup")
                        .IsRequired();

                    b.Navigation("Adventure");

                    b.Navigation("Participant");
                });

            modelBuilder.Entity("ShareMyAdventures.Domain.Entities.InvitationAggregate.AdventureInvitation", b =>
                {
                    b.HasOne("ShareMyAdventures.Domain.Entities.AdventureAggregate.Adventure", "Adventure")
                        .WithMany("Invitations")
                        .HasForeignKey("AdventureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("ShareMyAdventures.Domain.Entities.AdventureAggregate.AccessLevelLookup", "AccessLevelLookup", b1 =>
                        {
                            b1.Property<long>("AdventureInvitationId")
                                .HasColumnType("bigint");

                            b1.Property<int>("Id")
                                .HasColumnType("int")
                                .HasColumnName("AccessLevelLookupId");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("AccessLevelLookupName");

                            b1.HasKey("AdventureInvitationId");

                            b1.ToTable("AdventureInvitations");

                            b1.WithOwner()
                                .HasForeignKey("AdventureInvitationId");
                        });

                    b.OwnsOne("ShareMyAdventures.Domain.Entities.InvitationStatusLookup", "InvitationStatusLookup", b1 =>
                        {
                            b1.Property<long>("AdventureInvitationId")
                                .HasColumnType("bigint");

                            b1.Property<int>("Id")
                                .HasColumnType("int")
                                .HasColumnName("InvitationStatusLookupId");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("InvitationStatusLookupName");

                            b1.HasKey("AdventureInvitationId");

                            b1.ToTable("AdventureInvitations");

                            b1.WithOwner()
                                .HasForeignKey("AdventureInvitationId");
                        });

                    b.Navigation("AccessLevelLookup")
                        .IsRequired();

                    b.Navigation("Adventure");

                    b.Navigation("InvitationStatusLookup")
                        .IsRequired();
                });

            modelBuilder.Entity("ShareMyAdventures.Domain.Entities.ParticipantAggregate.FriendRequest", b =>
                {
                    b.HasOne("ShareMyAdventures.Domain.Entities.ParticipantAggregate.Participant", "ParticipantFriend")
                        .WithMany()
                        .HasForeignKey("ParticipantFriendId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ShareMyAdventures.Domain.Entities.ParticipantAggregate.Participant", "Participant")
                        .WithMany("Friends")
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsOne("ShareMyAdventures.Domain.Entities.InvitationStatusLookup", "InvitationStatusLookup", b1 =>
                        {
                            b1.Property<long>("FriendRequestId")
                                .HasColumnType("bigint");

                            b1.Property<int>("Id")
                                .HasColumnType("int")
                                .HasColumnName("InvitationStatusLookupId");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)")
                                .HasColumnName("InvitationStatusLookupName");

                            b1.HasKey("FriendRequestId");

                            b1.ToTable("Friends");

                            b1.WithOwner()
                                .HasForeignKey("FriendRequestId");
                        });

                    b.Navigation("InvitationStatusLookup")
                        .IsRequired();

                    b.Navigation("Participant");

                    b.Navigation("ParticipantFriend");
                });

            modelBuilder.Entity("ShareMyAdventures.Domain.Entities.ParticipantAggregate.Notification", b =>
                {
                    b.HasOne("ShareMyAdventures.Domain.Entities.ParticipantAggregate.Participant", "Participant")
                        .WithMany("Notifications")
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Participant");
                });

            modelBuilder.Entity("ShareMyAdventures.Domain.Entities.ParticipantAggregate.Position", b =>
                {
                    b.HasOne("ShareMyAdventures.Domain.Entities.ParticipantAggregate.Participant", "Participant")
                        .WithMany("Positions")
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Participant");
                });

            modelBuilder.Entity("ShareMyAdventures.Domain.Entities.AdventureAggregate.Adventure", b =>
                {
                    b.Navigation("Invitations");

                    b.Navigation("Participants");
                });

            modelBuilder.Entity("ShareMyAdventures.Domain.Entities.ParticipantAggregate.Participant", b =>
                {
                    b.Navigation("Adventures");

                    b.Navigation("Friends");

                    b.Navigation("Notifications");

                    b.Navigation("Positions");
                });
#pragma warning restore 612, 618
        }
    }
}
