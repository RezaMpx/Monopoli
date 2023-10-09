using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monopoli.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Player = table.Column<int>(type: "INTEGER", nullable: false),
                    Tower = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    Coin = table.Column<int>(type: "INTEGER", nullable: false),
                    Place = table.Column<int>(type: "INTEGER", nullable: false),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false),
                    Room = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsOnline = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoomName = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    Coin = table.Column<int>(type: "INTEGER", nullable: false),
                    PreBuy = table.Column<int>(type: "INTEGER", nullable: false),
                    IsStart = table.Column<bool>(type: "INTEGER", nullable: false),
                    Turn = table.Column<byte>(type: "INTEGER", nullable: false),
                    Home = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
