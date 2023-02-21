using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisualAcademy.Migrations
{
    /// <inheritdoc />
    public partial class CandidateColumnAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Candidates",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BirthCity",
                table: "Candidates",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BirthCountry",
                table: "Candidates",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BirthState",
                table: "Candidates",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Candidates",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DOB",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverLicenseNumber",
                table: "Candidates",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverLicenseState",
                table: "Candidates",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Candidates",
                type: "nvarchar(254)",
                maxLength: 254,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Candidates",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenseNumber",
                table: "Candidates",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Candidates",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Candidates",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryPhone",
                table: "Candidates",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SSN",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryPhone",
                table: "Candidates",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Candidates",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "BirthCity",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "BirthCountry",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "BirthState",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "DOB",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "DriverLicenseNumber",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "DriverLicenseState",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "LicenseNumber",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "PrimaryPhone",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "SSN",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "SecondaryPhone",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Candidates");
        }
    }
}
