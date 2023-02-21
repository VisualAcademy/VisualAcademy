using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisualAcademy.Migrations
{
    /// <inheritdoc />
    public partial class AddColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Candidates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AliasNames",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BirthCounty",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BirthPlace",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessStructure",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessStructureOther",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "County",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DriverLicenseExpiration",
                table: "Candidates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EyeColor",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HairColor",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeightFeet",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeightInches",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomePhone",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaritalStatus",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MobilePhone",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameSuffix",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficeAddress",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficeCity",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficeState",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhysicalMarks",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsCitizen",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Weight",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkFax",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkPhone",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "AliasNames",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "BirthCounty",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "BirthPlace",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "BusinessStructure",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "BusinessStructureOther",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "County",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "DriverLicenseExpiration",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "EyeColor",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "HairColor",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "HeightFeet",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "HeightInches",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "HomePhone",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "MaritalStatus",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "MobilePhone",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "NameSuffix",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "OfficeAddress",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "OfficeCity",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "OfficeState",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "PhysicalMarks",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "UsCitizen",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "WorkFax",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "WorkPhone",
                table: "Candidates");
        }
    }
}
