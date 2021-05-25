using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class AddAdminTestUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                //keyColumn: "Id",
                //keyValue: "23174cf0–9412–4cfe-afb-59f706d72cf6",
                columns: new[] { "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "Name", "Surname", "PhoneNumberConfirmed", "ConcurrencyStamp", "PasswordHash", "RegisterDate", "SecurityStamp", "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount" },
                values: new object[] { "5bb913e9-40cd-47a5-8e6f-ae867633f4db", "test@gmail.com", "TEST@GMAIL.COM", "test@gmail.com", "TEST@GMAIL.COM", true, "John", "Doe", false, "01560f0e-ee51-457a-813f-26b5d4a70f04", "AQAAAAEAACcQAAAAEADkFqeCHn1VntKrsoKth3iFRp68VjIj3kpGmilN+Zi1Ke/V77EA7FsVGl6YtW9VIQ==", new DateTimeOffset(new DateTime(2021, 5, 25, 20, 05, 23, 617, DateTimeKind.Unspecified).AddTicks(1204), new TimeSpan(0, 0, 0, 0, 0)), "WEKK4EAWQCPAXQ2V6OQSKKNZAY6GI64Z", false, true, 0 });
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] {"Id", "Name", "NormalizedName", "ConcurrencyStamp"},
                values: new object[]
                {
                    "5bb913e9-andr-47a5-8e6f-ae867633f4db", "Administrator", "ADMINISTRATOR",
                    "WEKK4EAWQCPAXQ2V6OQSKKNZAY6GI64Z"
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[]
                {
                    "5bb913e9-40cd-47a5-8e6f-ae867633f4db", "5bb913e9-andr-47a5-8e6f-ae867633f4db"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5bb913e9-40cd-47a5-8e6f-ae867633f4db");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5bb913e9-andr-47a5-8e6f-ae867633f4db");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumn: "UserId",
                keyValue: "5bb913e9-40cd-47a5-8e6f-ae867633f4db");
        }
    }
}
