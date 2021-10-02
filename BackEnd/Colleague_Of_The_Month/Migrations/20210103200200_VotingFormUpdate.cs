using Microsoft.EntityFrameworkCore.Migrations;

namespace Colleague_Of_The_Month.Migrations
{
    public partial class VotingFormUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomineeName",
                table: "VotingForms");

            migrationBuilder.DropColumn(
                name: "VoterName",
                table: "VotingForms");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "VotingForms",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Division",
                table: "VotingForms",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Impact",
                table: "VotingForms",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Manager",
                table: "VotingForms",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomineeFullName",
                table: "VotingForms",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Values",
                table: "VotingForms",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "VotingForms");

            migrationBuilder.DropColumn(
                name: "Division",
                table: "VotingForms");

            migrationBuilder.DropColumn(
                name: "Impact",
                table: "VotingForms");

            migrationBuilder.DropColumn(
                name: "Manager",
                table: "VotingForms");

            migrationBuilder.DropColumn(
                name: "NomineeFullName",
                table: "VotingForms");

            migrationBuilder.DropColumn(
                name: "Values",
                table: "VotingForms");

            migrationBuilder.AddColumn<string>(
                name: "NomineeName",
                table: "VotingForms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VoterName",
                table: "VotingForms",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
