using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HarshitCommunications.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DisplayOrder", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Charger" },
                    { 2, 2, "Cables" },
                    { 3, 3, "Air Buds" },
                    { 4, 4, "Neck Band" },
                    { 5, 5, "Car Charger" },
                    { 6, 6, "Power Bank" },
                    { 7, 7, "Smart Watch" },
                    { 8, 8, "Speakers" },
                    { 9, 9, "Earphone" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "{\r\n                        \"Bluetooth Version\": \"5.1+EDR\",\r\n                        \"Music Time\": \"25 Hours in Total\",\r\n                        \"Earphone Battery Capacity\": \"26 mAh\",\r\n                        \"Earphone Talk Time\": \"4hr\",\r\n                        \"Earphone Charging Time\": \"1.5hr\",\r\n                        \"Speaker Size\": \"13mm\",\r\n                        \"Charging Case Battery Capacity\": \"300 mAh\",\r\n                        \"Charging Case Time\": \"2Hr\",\r\n                        \"Standby Time\": \"500 Hours\",\r\n                        \"Distance\": \"10M\"\r\n                    }", "COOLPODS 9", 3299.0 },
                    { 2, "{\r\n                        \"Bluetooth Version\": \"5.1+EDR\",\r\n                        \"Music Time\": \"40 Hours in Total\",\r\n                        \"Earphone Battery Capacity\": \"30 mAh\",\r\n                        \"Earphone Talk Time\": \"5hr\",\r\n                        \"Earphone Charging Time\": \"1.5hr\",\r\n                        \"Speaker Size\": \"8mm\",\r\n                        \"Charging Case Battery Capacity\": \"270 mAh\",\r\n                        \"Charging Case Time\": \"2Hr\",\r\n                        \"Standby Time\": \"500 Hours\",\r\n                        \"Distance\": \"10M\"\r\n                    }", "COOLPODS 12", 3299.0 },
                    { 3, "{\r\n                        \"Wireless Version\": \"5.0\",\r\n                        \"Talk Time\": \"40 Hours\",\r\n                        \"Music Time\": \"36 Hours\",\r\n                        \"Stand By Time\": \"400 Hours\",\r\n                        \"Charging Time\": \"2 Hours\",\r\n                        \"Charging Type\": \"Type-C\",\r\n                        \"Battery Capacity\": \"250mAh\",\r\n                        \"Transmission Distance\": \"10M\",\r\n                        \"Speaker Size\": \"10MM\"\r\n                    }", "ROVER 3", 3299.0 },
                    { 4, "{\r\n                        \"Wireless Version\": \"5.0\",\r\n                        \"Talk Time\": \"35 Hours\",\r\n                        \"Music Time\": \"30 Hours\",\r\n                        \"Stand By Time\": \"400 Hours\",\r\n                        \"Charging Time\": \"2 Hours\",\r\n                        \"Charging Type\": \"Micro USB\",\r\n                        \"Battery Capacity\": \"250mAh\",\r\n                        \"Transmission Distance\": \"10M\",\r\n                        \"Speaker Size\": \"10MM\"\r\n                    }", "ROVER 5", 3299.0 },
                    { 5, "{\r\n                        \"Model\": \"HYDRO 2\",\r\n                        \"Bluetooth Version\": \"5.0\",\r\n                        \"Standby Time\": \"Up to 150 Hours\",\r\n                        \"Talk Time\": \"28 Hours\",\r\n                        \"Music Time\": \"20 Hours\",\r\n                        \"Charging Time\": \"2 Hours\",\r\n                        \"Battery Capacity\": \"400mAh\",\r\n                        \"Speaker Size\": \"40MM\",\r\n                        \"Use Distance\": \"Up to 10mm\"\r\n                    }", "HYDRO 2", 3299.0 },
                    { 6, "{\r\n                        \"Model\": \"HYDRO 3\",\r\n                        \"Bluetooth Version\": \"5.3+EDR\",\r\n                        \"Standby Time\": \"Up to 150 Hours\",\r\n                        \"Talk Time\": \"25 Hours\",\r\n                        \"Music Time\": \"20 Hours\",\r\n                        \"Charging Time\": \"2 Hours\",\r\n                        \"Battery Capacity\": \"200mAh\",\r\n                        \"Speaker Size\": \"40MM\",\r\n                        \"Use Distance\": \"Up to 10mm\"\r\n                    }", "HYDRO 3", 3299.0 },
                    { 7, "{\r\n                        \"Battery\": \"Polymer Li-on Battery\",\r\n                        \"Battery Capacity=\": \"10000mAh\",\r\n                        \"Input Ports\": \"Micro USB, Type-C\",\r\n                        \"Output Ports\": \"USB-A\",\r\n                        \"Portable Cable\": \"Lightning, Type-C, Micro USB\",\r\n                        \"Material\": \"PC+ABS\"\r\n                    }", "POWERBOX 5", 3999.0 },
                    { 8, "{\r\n                        \"Battery\": \"Polymer Li-on Battery\",\r\n                        \"Battery Capacity=\": \"10000mAh\",\r\n                        \"Input Ports\": \"USB-A, Type-C\",\r\n                        \"Output Ports\": \"USB-A\",\r\n                        \"Portable Cable\": \"Lightning, Type-C, Micro, USB-A\",\r\n                        \"Material\": \"PC+ABS\"\r\n                    }", "POWERBOX 8", 4999.0 },
                    { 9, "{\r\n                        \"Model\": \"Lancer 2\",\r\n                        \"Screen Size\": \"2.0 inch\",\r\n                        \"Charging Mode\": \"Wireless Charger\",\r\n                        \"Battery Backup\": \"3-4 Days with BT\",\r\n                        \"Application\": \"FereFit\",\r\n                        \"Bluetooth Calling\": \"Yes\",\r\n                        \"AI Voice Assistant\": \"Yes\",\r\n                        \"Waterproof Level\": \"IP67\",\r\n                        \"BT Version\": \"3.0/5.0\"\r\n                    }", "Lancer 2", 3299.0 },
                    { 10, "{\r\n                        \"Model\": \"Lancer 3\",\r\n                        \"Screen Size\": \"2.0 inch\",\r\n                        \"Charging Mode\": \"Magnet Charge\",\r\n                        \"Battery Backup\": \"3-4 Days\",\r\n                        \"Application\": \"HryFine\",\r\n                        \"Bluetooth Calling\": \"Yes\",\r\n                        \"AI Voice Assistant\": \"Yes\",\r\n                        \"Waterproof Level\": \"IP67\",\r\n                        \"BT Version\": \"3.0/5.0\"\r\n                    }", "Lancer 3", 3299.0 },
                    { 11, "{\r\n                        \"Output\": \"22W MAX\",\r\n                        \"Ports\": \"Type-C\",\r\n                        \"Protocols Supported\": \"QC3.0, PD, VOOC\",\r\n                        \"Fast Charging Supported\": \"Samsung OPPO, ONE+, VIVO, REALME, MI\"\r\n                    }", "Chamber 4c", 3299.0 },
                    { 12, "{\r\n                        \"Output\": \"22W MAX\",\r\n                        \"Ports\": \"Type-C\",\r\n                        \"Protocols Supported\": \"QC3.0, PD, VOOC\",\r\n                        \"Fast Charging Supported\": \"Samsung OPPO, ONE+, VIVO, REALME, MI\"\r\n                    }", "Chamber 4i", 3299.0 },
                    { 13, "{\r\n                        \"Current\": \"4A Output\",\r\n                        \"Wire Material\": \"Braided\",\r\n                        \"Length\": \"1 Meter\",\r\n                        \"Shell\": \"TPE\",\r\n                        \"Core\": \"Tinned Copper Wire\",\r\n                        \"Color\": \"Black & Grey\"\r\n                    }", "Flexy 3", 3299.0 },
                    { 14, "{\r\n                        \"Current\": \"4A Output\",\r\n                        \"Wire Material\": \"Braided\",\r\n                        \"Length\": \"1 Meter\",\r\n                        \"Shell\": \"Alluminium Alloy + TPE\",\r\n                        \"Core\": \"Tinned Copper Wire\",\r\n                        \"Color\": \"Black, Brown, Grey\"\r\n                    }", "Flexy 6", 3299.0 },
                    { 15, "{\r\n                        \"Driver Unit\": \"10mm\",\r\n                        \"Connector\": \"3.5mm audio jack\",\r\n                        \"Cord Length\": \"1.2m\"\r\n                    }", "Photon 4", 3299.0 },
                    { 16, "{\r\n                        \"Driver Unit\": \"10mm\",\r\n                        \"Connector\": \"3.5mm audio jack\",\r\n                        \"Cord Length\": \"1.2m\"\r\n                    }", "Photon 5", 3299.0 },
                    { 17, "{\r\n                        \"Model Name\": \"Piston 5L\",\r\n                        \"Input\": \"DC 12-24V\",\r\n                        \"Output\": \"5V = 2.4A (Max)\",\r\n                        \"Material\": \"ABS+PC\"\r\n                    }", "Piston 5L (Without Cable)", 3299.0 },
                    { 18, "{\r\n                        \"Model Name\": \"Piston 5L\",\r\n                        \"Input\": \"DC 12-24V\",\r\n                        \"Output\": \"DC5V/3.0A\",\r\n                        \"Port\": \"USB-A Port *2\"\r\n                    }", "Piston 7", 3299.0 },
                    { 19, "{\r\n                        \"Input Power\": \"5V = 1A\",\r\n                        \"Output\": \"10W(RWS)\",\r\n                        \"BT Version\": \"5.0\",\r\n                        \"Battery\": \"3.7V(1200mAh)\",\r\n                        \"Connectivity Options\": \"Wireless\",\r\n                        \"Dimensions\": \"64mm x 380mm x 56mm\"\r\n                    }", "JUKEBOX 3", 3299.0 },
                    { 20, "{\r\n                        \"Input Power\": \"5V = 1A\",\r\n                        \"Output\": \"20W(RWS)\",\r\n                        \"BT Version\": \"5.0\",\r\n                        \"Battery\": \"3.7V(3000mAh)\",\r\n                        \"Connectivity Options\": \"Wireless\",\r\n                        \"Dimensions\": \"212mm x 367mm x 158mm\"\r\n                    }", "JUKEBOX 4", 3299.0 },
                    { 21, "{\r\n                        \"Cable Length\": \"1 meter\",\r\n                        \"Material\": \"Braided wire\",\r\n                        \"Color\": \"Black with Silver Alloy\",\r\n                        \"Interface\": \"Type-C to 3.5mm Aux\",\r\n                        \"Input\": \"USB Type-C\",\r\n                        \"Output\": \"3.5mm Aux\"\r\n                    }", "Flame 1t", 3299.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
