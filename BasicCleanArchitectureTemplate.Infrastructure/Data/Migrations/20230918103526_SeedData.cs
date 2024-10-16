using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestTask.Web.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SET IDENTITY_INSERT [dbo].[Events] ON");

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Name", "Category", "Place", "Date", "Time", "Description", "AdditionalInfo", "ImageUrl" },
                values: new object[]
                {
            1,
            "Nokturnal Mortum",
            "Black Metal",
            "Lviv",
            new DateTime(2023, 9, 14),
            new DateTime(2023, 9, 15, 4, 11, 0),
            "Stugna Stage",
            "No",
            "http://nokturnal-mortum.com/"
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Name", "Category", "Place", "Date", "Time", "Description", "AdditionalInfo", "ImageUrl" },
                values: new object[]
                {
            2,
            "White Ward",
            "Post Black",
            "Rivne",
            new DateTime(2023, 9, 13, 23, 12, 0),
            new DateTime(2023, 9, 28, 23, 12, 0),
            "Main stage",
            "First event",
            "https://slukh.media/new-music/white-ward-false-light/"
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Name", "Category", "Place", "Date", "Time" },
                values: new object[]
                {
            3,
            "Khors",
            "Black metal",
            "Kyiv",
            new DateTime(2023, 9, 13),
            new DateTime(2023, 9, 15, 17, 17, 0)
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Name", "Category", "Place", "Date", "Time" },
                values: new object[]
                {
            4,
            "Drudkh",
            "Black",
            "Rivne",
            new DateTime(2023, 9, 21),
            new DateTime(2023, 9, 18, 13, 26, 0)
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Name", "Category", "Place", "Date", "Time", "Description", "AdditionalInfo" },
                values: new object[]
                {
            5,
            "Hate Forest",
            "Black Metal/Ambient",
            "Kharkiv",
            new DateTime(2023, 9, 27),
            new DateTime(2023, 9, 18, 13, 26, 0),
            null,
            "Osmose Productions"
                });

            migrationBuilder.Sql("SET IDENTITY_INSERT [dbo].[Events] OFF");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
