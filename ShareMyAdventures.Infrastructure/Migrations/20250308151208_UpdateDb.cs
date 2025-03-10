using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareMyAdventures.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParticipantId1",
                table: "Positions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AdventureId1",
                table: "AdventureInvitations",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Positions_ParticipantId1",
                table: "Positions",
                column: "ParticipantId1");

            migrationBuilder.CreateIndex(
                name: "IX_AdventureInvitations_AdventureId1",
                table: "AdventureInvitations",
                column: "AdventureId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AdventureInvitations_Adventures_AdventureId1",
                table: "AdventureInvitations",
                column: "AdventureId1",
                principalTable: "Adventures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_AspNetUsers_ParticipantId1",
                table: "Positions",
                column: "ParticipantId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdventureInvitations_Adventures_AdventureId1",
                table: "AdventureInvitations");

            migrationBuilder.DropForeignKey(
                name: "FK_Positions_AspNetUsers_ParticipantId1",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Positions_ParticipantId1",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_AdventureInvitations_AdventureId1",
                table: "AdventureInvitations");

            migrationBuilder.DropColumn(
                name: "ParticipantId1",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "AdventureId1",
                table: "AdventureInvitations");
        }
    }
}
