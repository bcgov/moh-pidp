using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pidp.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExpandPharmacyScope : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 5, columns: new[] { "Address1", "Address2" }, values: new object[] { "202A-780 Windsor Avenue", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 28, columns: new[] { "Address1", "Address2" }, values: new object[] { "400 - 210 Railway Ave., Box 1060", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 50, columns: new[] { "Address1", "Address2" }, values: new object[] { "180-13151 Vanier Place", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 98, columns: new[] { "Address1", "Address2" }, values: new object[] { "7186 Lantzville Rd. Box 328", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 149, columns: new[] { "Address1", "Address2" }, values: new object[] { "Unit #1 3550 Brighton Avenue", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 155, columns: new[] { "Address1", "Address2" }, values: new object[] { "672 Plaza Rd, Box 614, Quadra Island", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 166, columns: new[] { "Address1", "Address2" }, values: new object[] { "101 - 8146 Queen St, PO Box 1089", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 200, columns: new[] { "Address1", "Address2" }, values: new object[] { "4 Front Street Suite B", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 210, columns: new[] { "Address1", "Address2" }, values: new object[] { "Unit B-12815 96 Ave", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 230, columns: new[] { "Address1", "Address2" }, values: new object[] { "4904 50th Ave. N., Box 540", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 283, columns: new[] { "Address1", "Address2" }, values: new object[] { "826B - 9th Ave S, Box 1106", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 294, columns: new[] { "Address1", "Address2" }, values: new object[] { "Suite D-22195 Dewdney Trunk Road", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 299, columns: new[] { "Address1", "Address2" }, values: new object[] { "Unit 100 - 8950 Granville St, PO Box 909", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 318, columns: new[] { "Address1", "Address2" }, values: new object[] { "115-1700 Garcia Street", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 335, columns: new[] { "Address1", "Address2" }, values: new object[] { "612 - 6th Avenue, Box 400", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 349, columns: new[] { "Address1", "Address2" }, values: new object[] { "#13 - 575 North Road RR #3", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 350, columns: new[] { "Address1", "Address2" }, values: new object[] { "138 South Shore Road, Box 38", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 354, columns: new[] { "Address1", "Address2" }, values: new object[] { "26 - 1400 Cowichan Bay Rd, RR 3", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 356, columns: new[] { "Address1", "Address2" }, values: new object[] { "Unit A - 845 Deloume Rd, RR 2", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 395, columns: new[] { "Address1", "Address2" }, values: new object[] { "Unit U1, 601 West Broadway", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 408, columns: new[] { "Address1", "Address2" }, values: new object[] { "5016 50th Ave, PO Box 1330", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 412, columns: new[] { "Address1", "Address2" }, values: new object[] { "1105-4700 Kingsway", "Eaton Centre" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 458, column: "Address2", value: "Medical Dental Centre c/o Lock's Phcy");
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 471, columns: new[] { "Address1", "Address2" }, values: new object[] { "Unit 1030-2929 Barnet Hwy.", "Coquitlam Centre" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 596, columns: new[] { "Address1", "Address2" }, values: new object[] { "Unit 1110-13560 Maycrest Way", "Ground Floor" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 665, columns: new[] { "Address1", "Address2" }, values: new object[] { "105 - 291 Fairview Rd, PO Box 1871", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 680, columns: new[] { "Address1", "Address2" }, values: new object[] { "Unit 101-42 6th Street", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 693, columns: new[] { "Address1", "Address2" }, values: new object[] { "#10 - 4605 Bedwell Harbour Road", "R.R. #1, Box 134" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 736, columns: new[] { "Address1", "Address2" }, values: new object[] { "Suite 1103.8 - 3880 Grant McConachie Way", "Vanc. Int'l Airport, Domestic Terminal" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 743, columns: new[] { "Address1", "Address2" }, values: new object[] { "101B - 3055 Oak St", "RR 1" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 745, columns: new[] { "Address1", "Address2" }, values: new object[] { "3752 - 4th Ave.", "PO Box 2530" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 764, columns: new[] { "Address1", "Address2" }, values: new object[] { "307 Victoria Rd W", "Box 680" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 766, columns: new[] { "Address1", "Address2" }, values: new object[] { "#11A - 2720 Mill Bay Road", "Box 160" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 811, columns: new[] { "Address1", "Address2" }, values: new object[] { "107 Centennial Square, PO Box 717", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 839, columns: new[] { "Address1", "Address2" }, values: new object[] { "131 First St, PO Box 1080", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 954, columns: new[] { "Address1", "Address2" }, values: new object[] { "8925 Granville Street, Box 190", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 985, columns: new[] { "Address1", "Address2" }, values: new object[] { "#103 - 4360 Lorimer Road, ", "" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 1011, columns: new[] { "Address1", "Address2" }, values: new object[] { "Unit 102 - 10388 City Parkway", "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 5, columns: new[] { "Address1", "Address2" }, values: new object[] { "780 Windsor Avenue", "Unit 202A" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 28, columns: new[] { "Address1", "Address2" }, values: new object[] { "Box 1060", "400 - 210 Railway Ave." });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 50, columns: new[] { "Address1", "Address2" }, values: new object[] { "Suite 180", "13151 Vanier Place" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 98, columns: new[] { "Address1", "Address2" }, values: new object[] { "Box 328", "7186 Lantzville Rd" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 149, columns: new[] { "Address1", "Address2" }, values: new object[] { "3550 Brighton Avenue", "Unit #1" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 155, columns: new[] { "Address1", "Address2" }, values: new object[] { "Box 614, Quadra Island", "672 Plaza Rd" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 166, columns: new[] { "Address1", "Address2" }, values: new object[] { "PO Box 1089", "101 - 8146 Queen St" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 200, columns: new[] { "Address1", "Address2" }, values: new object[] { "Suite B", "4 Front Street" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 210, columns: new[] { "Address1", "Address2" }, values: new object[] { "12815 96 Ave", "Unit B" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 230, columns: new[] { "Address1", "Address2" }, values: new object[] { "4904 50th Ave. N.", "Box 540" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 283, columns: new[] { "Address1", "Address2" }, values: new object[] { "826B - 9th Ave S", "P.O. Box 1106" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 294, columns: new[] { "Address1", "Address2" }, values: new object[] { "22195 Dewdney Trunk Road", "Suite D" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 299, columns: new[] { "Address1", "Address2" }, values: new object[] { "PO Box 909", "Unit 100 - 8950 Granville St" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 318, columns: new[] { "Address1", "Address2" }, values: new object[] { "1700 Garcia Street", "115" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 335, columns: new[] { "Address1", "Address2" }, values: new object[] { "Box 400", "612 - 6th Avenue" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 349, columns: new[] { "Address1", "Address2" }, values: new object[] { "RR #3", "#13 - 575 North Road" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 350, columns: new[] { "Address1", "Address2" }, values: new object[] { "Box 38", "138 South Shore Road" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 354, columns: new[] { "Address1", "Address2" }, values: new object[] { "RR 3", "26 - 1400 Cowichan Bay Rd" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 356, columns: new[] { "Address1", "Address2" }, values: new object[] { "RR 2", "Unit A - 845 Deloume Rd" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 395, columns: new[] { "Address1", "Address2" }, values: new object[] { "601 West Broadway", "Unit U1" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 408, columns: new[] { "Address1", "Address2" }, values: new object[] { "5016 50th Ave", "PO Box 1330" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 412, columns: new[] { "Address1", "Address2" }, values: new object[] { "4700 Kingsway", "1105 Eaton Centre" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 458, column: "Address2", value: "Medical Dental Centre C/o Lock's Phcy");
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 471, columns: new[] { "Address1", "Address2" }, values: new object[] { "Unit 1030-2929 Barnet Hwy.", "Coquitlam Centre" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 596, columns: new[] { "Address1", "Address2" }, values: new object[] { "Ground Floor, 13560 Maycrest Way", "Unit 1110" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 665, columns: new[] { "Address1", "Address2" }, values: new object[] { "PO Box 1871", "105 - 291 Fairview Rd" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 680, columns: new[] { "Address1", "Address2" }, values: new object[] { "42 6th Street", "Unit 101" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 693, columns: new[] { "Address1", "Address2" }, values: new object[] { "R.R. #1, #10 - 4605 Bedwell Harbour Road", "Box 134" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 736, columns: new[] { "Address1", "Address2" }, values: new object[] { "Vanc. Int'l Airport, Domestic Terminal", "Suite 1103.8 - 3880 Grant McConachie Way" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 743, columns: new[] { "Address1", "Address2" }, values: new object[] { "RR 1", "101B - 3055 Oak St" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 745, columns: new[] { "Address1", "Address2" }, values: new object[] { "PO Box 2530", "3752 - 4th Ave." });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 764, columns: new[] { "Address1", "Address2" }, values: new object[] { "Box 680", "307 Victoria Rd W" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 766, columns: new[] { "Address1", "Address2" }, values: new object[] { "Box 160", "#11A - 2720 Mill Bay Road" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 811, columns: new[] { "Address1", "Address2" }, values: new object[] { "PO Box 717", "107 Centennial Square" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 839, columns: new[] { "Address1", "Address2" }, values: new object[] { "PO Box 1080", "131 First St" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 954, columns: new[] { "Address1", "Address2" }, values: new object[] { "Box 190", "8925 Granville Street" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 985, columns: new[] { "Address1", "Address2" }, values: new object[] { "RR 4", "#103 - 4360 Lorimer Road" });
            migrationBuilder.UpdateData( table: "Pharmacies", keyColumn: "Id", keyValue: 1011, columns: new[] { "Address1", "Address2" }, values: new object[] { "Unit 102 and 103", "10388 City Parkway" });
        }
    }
}
