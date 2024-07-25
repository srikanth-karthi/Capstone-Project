using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventCategories",
                columns: table => new
                {
                    EventCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Rating = table.Column<float>(type: "real", nullable: true),
                    NumberOfRatings = table.Column<int>(type: "int", nullable: false),
                    TotalRating = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCategories", x => x.EventCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Poster = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfTickets = table.Column<int>(type: "int", nullable: false),
                    RemainingTickets = table.Column<int>(type: "int", nullable: false),
                    TicketCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "QuotationRequests",
                columns: table => new
                {
                    QuotationRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EventCategoryId = table.Column<int>(type: "int", nullable: false),
                    VenueType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoodPreference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CateringInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialInstructions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuotationStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationRequests", x => x.QuotationRequestId);
                    table.ForeignKey(
                        name: "FK_QuotationRequests_EventCategories_EventCategoryId",
                        column: x => x.EventCategoryId,
                        principalTable: "EventCategories",
                        principalColumn: "EventCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuotationRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    TicketId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AttendeeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttendeeEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CheckedInTickets = table.Column<int>(type: "int", nullable: false),
                    NumberOfTickets = table.Column<int>(type: "int", nullable: false),
                    TicketCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_Tickets_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuotationResponses",
                columns: table => new
                {
                    QuotationResponseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuotationRequestId = table.Column<int>(type: "int", nullable: false),
                    RequestStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuotedAmount = table.Column<double>(type: "float", nullable: true),
                    Currency = table.Column<int>(type: "int", nullable: true),
                    ResponseMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationResponses", x => x.QuotationResponseId);
                    table.ForeignKey(
                        name: "FK_QuotationResponses_QuotationRequests_QuotationRequestId",
                        column: x => x.QuotationRequestId,
                        principalTable: "QuotationRequests",
                        principalColumn: "QuotationRequestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientResponses",
                columns: table => new
                {
                    ClientResponseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuotationResponseId = table.Column<int>(type: "int", nullable: false),
                    ClientDecision = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientResponseDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientResponses", x => x.ClientResponseId);
                    table.ForeignKey(
                        name: "FK_ClientResponses_QuotationResponses_QuotationResponseId",
                        column: x => x.QuotationResponseId,
                        principalTable: "QuotationResponses",
                        principalColumn: "QuotationResponseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventCategoryId = table.Column<int>(type: "int", nullable: false),
                    ClientResponseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<double>(type: "float", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_ClientResponses_ClientResponseId",
                        column: x => x.ClientResponseId,
                        principalTable: "ClientResponses",
                        principalColumn: "ClientResponseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_EventCategories_EventCategoryId",
                        column: x => x.EventCategoryId,
                        principalTable: "EventCategories",
                        principalColumn: "EventCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientResponseId = table.Column<int>(type: "int", nullable: false),
                    EventCategoryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<float>(type: "real", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Reviews_ClientResponses_ClientResponseId",
                        column: x => x.ClientResponseId,
                        principalTable: "ClientResponses",
                        principalColumn: "ClientResponseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_EventCategories_EventCategoryId",
                        column: x => x.EventCategoryId,
                        principalTable: "EventCategories",
                        principalColumn: "EventCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledEvents",
                columns: table => new
                {
                    ScheduledEventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventCategoryId = table.Column<int>(type: "int", nullable: false),
                    ClienResponseId = table.Column<int>(type: "int", nullable: false),
                    QuotationRequestId = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledEvents", x => x.ScheduledEventId);
                    table.ForeignKey(
                        name: "FK_ScheduledEvents_ClientResponses_ClienResponseId",
                        column: x => x.ClienResponseId,
                        principalTable: "ClientResponses",
                        principalColumn: "ClientResponseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduledEvents_EventCategories_EventCategoryId",
                        column: x => x.EventCategoryId,
                        principalTable: "EventCategories",
                        principalColumn: "EventCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduledEvents_QuotationRequests_QuotationRequestId",
                        column: x => x.QuotationRequestId,
                        principalTable: "QuotationRequests",
                        principalColumn: "QuotationRequestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduledEvents_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FullName", "Role" },
                values: new object[] { 1, new DateTime(2024, 7, 24, 12, 29, 42, 321, DateTimeKind.Local).AddTicks(9129), "bookmyevent24@gmail.com", "Book My Event", "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientResponses_QuotationResponseId",
                table: "ClientResponses",
                column: "QuotationResponseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientResponseId",
                table: "Orders",
                column: "ClientResponseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EventCategoryId",
                table: "Orders",
                column: "EventCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationRequests_EventCategoryId",
                table: "QuotationRequests",
                column: "EventCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationRequests_UserId",
                table: "QuotationRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationResponses_QuotationRequestId",
                table: "QuotationResponses",
                column: "QuotationRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ClientResponseId",
                table: "Reviews",
                column: "ClientResponseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_EventCategoryId",
                table: "Reviews",
                column: "EventCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledEvents_ClienResponseId",
                table: "ScheduledEvents",
                column: "ClienResponseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledEvents_EventCategoryId",
                table: "ScheduledEvents",
                column: "EventCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledEvents_QuotationRequestId",
                table: "ScheduledEvents",
                column: "QuotationRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledEvents_UserId",
                table: "ScheduledEvents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EventId",
                table: "Tickets",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "ScheduledEvents");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "ClientResponses");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "QuotationResponses");

            migrationBuilder.DropTable(
                name: "QuotationRequests");

            migrationBuilder.DropTable(
                name: "EventCategories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
