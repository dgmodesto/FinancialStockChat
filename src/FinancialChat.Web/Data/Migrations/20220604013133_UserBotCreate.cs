using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialChatBackend.Data.Migrations
{
    public partial class UserBotCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3370475C-F265-4CBF-8FDE-07C04FBFF15E", "ad1795e1-3920-48da-a597-4351d8137a8a", "admin", "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "3370475C-F265-4CBF-8FDE-07C04FBFF15E", 0, "4114e5b2-c72c-4ba3-80ce-30b01e1c168e", "bot@financialchat.com", true, false, null, "bot@financialchat.com", "Bot", "AQAAAAEAACcQAAAAEN66ZIzrRF5l7NJ4Fkam0pNm2oXr0VphT3x8bDjTPbUFlHMGJrFnotNgovXAECcpOw==", null, false, "", false, "bot@financialchat.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "3370475C-F265-4CBF-8FDE-07C04FBFF15E", "3370475C-F265-4CBF-8FDE-07C04FBFF15E" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3370475C-F265-4CBF-8FDE-07C04FBFF15E", "3370475C-F265-4CBF-8FDE-07C04FBFF15E" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3370475C-F265-4CBF-8FDE-07C04FBFF15E");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3370475C-F265-4CBF-8FDE-07C04FBFF15E");
        }
    }
}
