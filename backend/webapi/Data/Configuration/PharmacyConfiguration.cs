namespace Pidp.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pidp.Models;

public class PharmacyConfiguration : IEntityTypeConfiguration<Pharmacy>
{
    public void Configure(EntityTypeBuilder<Pharmacy> builder)
    {
        builder.HasData(
            new Pharmacy {
                Id = 1,
                Name = "108 STOP PHARMACY",
                Address = "13444 108 Ave, Surrey BC V3T 2K1 Canada",
                ManagerName = "Muhammad Iqbal",
                Phone = "(604) 957-0711",
                Fax = "(604) 953-1700",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 2,
                Name = "1HEALTH PHARMACY",
                Address = "112 - 15315 66 Avenue, Surrey BC V3S 2A1 Canada",
                ManagerName = "Gurvinder Sudhan",
                Phone = "(778) 914-5000",
                Fax = "(236) 598-3956",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 3,
                Name = "360CARE DENMAN PHARMACY",
                Address = "683 Denman St, Vancouver BC V6G 2L3 Canada",
                ManagerName = "Michelle Ly",
                Phone = "(604) 683-6933",
                Fax = "(604) 683-6968",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 4,
                Name = "49TH PARALLEL PHARMACY",
                Address = "15229 Russell Ave, White Rock BC V4B 5C3 Canada",
                ManagerName = "Mary Mani",
                Phone = "(778) 294-7737",
                Fax = "(778) 294-8847",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 5,
                Name = "8TH STREET PHARMACY",
                Address = "Unit 202A, 780 Windsor Avenue, Kamloops BC V2B 2B6 Canada",
                ManagerName = "Matthew Lloyd",
                Phone = "(778) 470-9345",
                Fax = "(778) 470-9346",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 6,
                Name = "AARONSON'S PHARMACY (COOK ST.) LTD.",
                Address = "102-1711 Cook St, Victoria BC V8T 3P2 Canada",
                ManagerName = "Andrew Formosa",
                Phone = "(250) 383-6511",
                Fax = "(250) 383-1353",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 7,
                Name = "ABBOTSFORD PHARMACY",
                Address = "104 - 2596 McMillan Rd, Abbotsford BC V3G 1C4 Canada",
                ManagerName = "Sali Iskander",
                Phone = "(778) 314-1014",
                Fax = "(778) 314-1016",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 8,
                Name = "ABBY PHARMACY",
                Address = "#100 - 2845 Cruickshank Street, Abbotsford BC V2T 6X1 Canada",
                ManagerName = "Sukhraj Bassi",
                Phone = "(604) 504-0060",
                Fax = "(604) 504-0616",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 9,
                Name = "ACCURX DRUGSTORE 001",
                Address = "106 - 1849 Dufferin Crescent, Nanaimo BC V9S 0B1 Canada",
                ManagerName = "Munir Boghani",
                Phone = "(250) 591-2912",
                Fax = "(250) 591-2914",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 10,
                Name = "ACKROYD PHARMACY",
                Address = "160 - 8100 Ackroyd Rd, Richmond BC V6X 3K2 Canada",
                ManagerName = "Anthony Lee",
                Phone = "(604) 207-9972",
                Fax = "(604) 207-9080",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 11,
                Name = "AGASSIZ PHARMACY",
                Address = "7046 Pioneer Ave, Agassiz BC V0M 1A0 Canada",
                ManagerName = "Mohamed Hasanine",
                Phone = "(604) 491-1070",
                Fax = "(604) 491-1071",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 12,
                Name = "ALBERNI PHARMACY",
                Address = "4760 Johnston Rd, Port Alberni BC V9Y 5M3 Canada",
                ManagerName = "Scott Frombach",
                Phone = "(778) 419-3784",
                Fax = "(778) 419-3785",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 13,
                Name = "ALBERTO PHARMACY # 1",
                Address = "2516 Commercial Dr, Vancouver BC V5N 4C2 Canada",
                ManagerName = "Kenneth Wan",
                Phone = "(604) 873-4111",
                Fax = "(604) 873-6734",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 14,
                Name = "ALBERTO PHARMACY NO. 2",
                Address = "#101 - 2620 Commercial Dr, Vancouver BC V5N 4C4 Canada",
                ManagerName = "Jeffrey Wei",
                Phone = "(604) 879-8481",
                Fax = "(604) 879-8482",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 15,
                Name = "ALDERGROVE COMMUNITY PHARMACY",
                Address = "27105 Fraser Hwy, Aldergrove BC V4W 3R2 Canada",
                ManagerName = "Rinkal Patel",
                Phone = "(604) 607-7404",
                Fax = "(604) 607-7454",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 16,
                Name = "ALDERGROVE PHARMACHOICE",
                Address = "#120 - 3113 272 St, Aldergrove BC V4W 3R9 Canada",
                ManagerName = "Pavandeep Sarohia",
                Phone = "(604) 625-3784",
                Fax = "(604) 625-3785",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 17,
                Name = "ALERT BAY DRUGS",
                Address = "90 Fir Street, Alert Bay BC V0N 1A0 Canada",
                ManagerName = "Paul Fletcher",
                Phone = "(250) 974-5712",
                Fax = "(250) 974-2199",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 18,
                Name = "ALL CURE PHARMACY",
                Address = "101 - 12827 76 Ave, Surrey BC V3W 2V3 Canada",
                ManagerName = "Deepak Sharma",
                Phone = "(778) 592-2500",
                Fax = "(778) 592-2501",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 19,
                Name = "ALOUETTE PHARMACY",
                Address = "8 - 11937 227 St, Maple Ridge BC V2X 6J4 Canada",
                ManagerName = "Benton Lee",
                Phone = "(604) 467-3784",
                Fax = "(604) 467-3714",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 20,
                Name = "ALPINE DRUG MART IDA",
                Address = "2060 Columbia Ave., Rossland BC V0G 1Y0 Canada",
                ManagerName = "Jacqueline Corlett",
                Phone = "(250) 362-5622",
                Fax = "(250) 362-5151",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 21,
                Name = "AMAICARE PHARMACY CLINIC",
                Address = "102 - 1010 Talasa Way, Kamloops BC V2H 0G1 Canada",
                ManagerName = "Dereck Sigauke",
                Phone = "(778) 471-9711",
                Fax = "(778) 471-9712",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 22,
                Name = "ANCHOR COMPOUNDING PHARMACY",
                Address = "105 - 1450 Waddington Rd, Nanaimo BC V9S 4V9 Canada",
                ManagerName = "Rana Ullah",
                Phone = "(250) 591-4411",
                Fax = "(250) 591-4011",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 23,
                Name = "ANDERSON'S PHARMACY",
                Address = "127 3rd St W, North Vancouver BC V7M 1E7 Canada",
                ManagerName = "Maria Kwong",
                Phone = "(604) 988-5271",
                Fax = "(604) 998-1271",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 24,
                Name = "ANDREEN'S PHARMACY",
                Address = "101 - 879 Anders Road, West Kelowna BC V1Z 1K2 Canada",
                ManagerName = "Brian Smith",
                Phone = "(250) 769-2014",
                Fax = "(250) 769-2054",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 25,
                Name = "APEX PHARMACY",
                Address = "Unit 1A - 32943 Marshall Rd, Abbotsford BC V2S 1J8 Canada",
                ManagerName = "Elke Groening",
                Phone = "(604) 870-0171",
                Fax = "(604) 870-0172",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 26,
                Name = "ARGYLE PHARMACY",
                Address = "3054 3rd Avenue, Port Alberni BC V9Y 2A5 Canada",
                ManagerName = "Sukhjinder Sidhu",
                Phone = "778-421-3333",
                Fax = "778-421-1919",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 27,
                Name = "ARMSTRONG PHARMACY AND WELLNESS CENTRE",
                Address = "#5 - 3300 Smith Drive, Armstrong BC V0E 1B1 Canada",
                ManagerName = "Jason Buerfeind",
                Phone = "(250) 546-3195",
                Fax = "(250) 546-3894",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 28,
                Name = "ASHCROFT IDA PHARMACY",
                Address = "400 - 210 Railway Ave., Box 1060, Ashcroft BC V0K 1A0 Canada",
                ManagerName = "Nedal Elsawy",
                Phone = "(250) 453-2553",
                Fax = "(250) 453-2404",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 29,
                Name = "ASHER PHARMACY & CLINIC",
                Address = "207 Asher Rd, Kelowna BC V1X 3H5 Canada",
                ManagerName = "Bikramjeet Chahal",
                Phone = "(250) 223-0101",
                Fax = "(877) 651-0256",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 30,
                Name = "ASKEW'S PHARMACY",
                Address = "2701 11 Ave NE, Salmon Arm BC V1E 2S3 Canada",
                ManagerName = "Robert Moore",
                Phone = "(250) 832-7655",
                Fax = "(250) 832-7656",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 31,
                Name = "ASPEN REMEDY'SRX PHARMACY",
                Address = "102 - 2099 152 St, Surrey BC V4A 4N7 Canada",
                ManagerName = "Omar Omar",
                Phone = "(604) 560-2720",
                Fax = "(604) 560-2722",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 32,
                Name = "ASTRAL I.D.A. PHARMACY",
                Address = "111 - 3101 Highway 6, Vernon BC V1T 9H6 Canada",
                ManagerName = "Dipak Patel",
                Phone = "(250) 541-1999",
                Fax = "(250) 541-1777",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 33,
                Name = "ATLAS DRUG MART",
                Address = "713A Goldstream Ave, Victoria BC V9B 2X4 Canada",
                ManagerName = "Nazar Osman",
                Phone = "(250) 391-2964",
                Fax = "(250) 391-2911",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 34,
                Name = "BAINS PHARMACY LTD.",
                Address = "8681 120 St, Delta BC V4C 6R4 Canada",
                ManagerName = "Ryan Brar",
                Phone = "(604) 543-0911",
                Fax = "(604) 507-0988",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 35,
                Name = "BALANCED HEALTH INTEGRATIVE PHARMACY",
                Address = "135 Lonsdale Ave, North Vancouver BC V7M 2E7 Canada",
                ManagerName = "Peter Lee",
                Phone = "(604) 971-4075",
                Fax = "(604) 971-4074",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 36,
                Name = "BARNET PHARMACY",
                Address = "3-2773 Barnet Hwy, Coquitlam BC V3B 1C2 Canada",
                ManagerName = "Elahe Rahimi",
                Phone = "(604) 468-4038",
                Fax = "(604) 468-4091",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 37,
                Name = "BAY PHARMACY",
                Address = "6355 Bruce St, West Vancouver BC V7W 2G5 Canada",
                ManagerName = "Maie Naser",
                Phone = "(604) 305-0330",
                Fax = "(604) 281-0330",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 38,
                Name = "BAYSHORE PHARMACY",
                Address = "7313 Meadow Ave, Burnaby BC V5J 4Z2 Canada",
                ManagerName = "Hani Al-Tabbaa",
                Phone = "1-855-237-2473",
                Fax = "1-855-233-3146",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 39,
                Name = "BC DRUGS PHARMACY",
                Address = "9618 Cameron St, Burnaby BC V3J 1M2 Canada",
                ManagerName = "Anita Jalzabetic-Maravic",
                Phone = "(604) 422-8216",
                Fax = "(604) 422-8221",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 40,
                Name = "BELL PHARMACY",
                Address = "10519 King George Blvd, Surrey BC V3T 2X1 Canada",
                ManagerName = "Iraj Seyed Zehtab",
                Phone = "(604) 585-3355",
                Fax = "(604) 585-3350",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 41,
                Name = "BELMONT PRESCRIPTION X-PRESS",
                Address = "609 Belmont Street, New Westminster BC V3M 5Z9 Canada",
                ManagerName = "Vesna Vlacina Ljepic",
                Phone = "(604) 245-6200",
                Fax = "(604) 245-6213",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 42,
                Name = "BEST CARE PHARMACY",
                Address = "101 - 12181 Harris Rd, Pitt Meadows BC V3Y 2E9 Canada",
                ManagerName = "Priyanka Meka",
                Phone = "(604) 262-4949",
                Fax = "(604) 398-4937",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 43,
                Name = "BEST VALUE PHARMACY",
                Address = "477 Terminal Ave N, Nanaimo BC V9S 4J8 Canada",
                ManagerName = "Sneh Parikh",
                Phone = "(236) 897-0737",
                Fax = "(236) 938-2980",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 44,
                Name = "BEST VALUE PHARMACY #2",
                Address = "101-660 Beach Rd, Qualicum Beach BC V9K 2R1 Canada",
                ManagerName = "Stephanie Hahn",
                Phone = "(250) 909-0290",
                Fax = "(236) 935-2984",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 45,
                Name = "BEST VALUE PHARMACY #3",
                Address = "111-2197 Otter Point Rd, Sooke BC V9Z 1R9 Canada",
                ManagerName = "Rushi Sorathiya",
                Phone = "(778) 352-5033",
                Fax = "(236) 915-2907",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 46,
                Name = "BETA PHARMACY",
                Address = "104A-3701 Hastings St, Burnaby BC V5C 2H6 Canada",
                ManagerName = "Aysan Vahab Zadeh Memari",
                Phone = "(604) 299-6004",
                Fax = "(604) 299-5004",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 47,
                Name = "BEYOND PHARMACY",
                Address = "#101 - 19211 Fraser Hwy, Surrey BC V3S 7C9 Canada",
                ManagerName = "Harminder Mathroo",
                Phone = "(604) 245-6069",
                Fax = "(604) 245-6102",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 48,
                Name = "BEYOND PHARMACY 2",
                Address = "#150 - 1575 McCallum Rd, Abbotsford BC V2S 0K2 Canada",
                ManagerName = "Herleen Sidhu",
                Phone = "(604) 529-7600",
                Fax = "(604) 529-7603",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 49,
                Name = "BIOPRO BIOLOGICS PHARMACY",
                Address = "845 Broadway W, Vancouver BC V5Z 1J9 Canada",
                ManagerName = "Lysa Leong",
                Phone = "(778) 379-8161",
                Fax = "(778) 379-8160",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 50,
                Name = "BIOSCRIPT PHARMACY LTD.",
                Address = "13151 Vanier Place, Suite 180, Richmond BC V6V 2J1 Canada",
                ManagerName = "Calvin Chan",
                Phone = "(604) 214-3784",
                Fax = "(604) 244-3784",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 51,
                Name = "BIOSENSE COMPOUNDING PHARMACY",
                Address = "208-6011 Westminster Hwy, Richmond BC V7C 4V4 Canada",
                ManagerName = "David Fung",
                Phone = "(604) 278-7955",
                Fax = "(604) 278-7960",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 52,
                Name = "BLACK'S PHARMACY",
                Address = "2037 Quilchena Ave., Merritt BC V1K 1B8 Canada",
                ManagerName = "Blaine Martens",
                Phone = "(250) 378-2155",
                Fax = "(250) 378-4884",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 53,
                Name = "BONSOR PHARMACY",
                Address = "#107 - 6411 Nelson Avenue, Burnaby BC V5H 4H3 Canada",
                ManagerName = "Azmina Jiwa",
                Phone = "(604) 431-8877",
                Fax = "(604) 430-4700",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 54,
                Name = "BOOMER DRUGS",
                Address = "8574 Granville St, Vancouver BC V6P 4Z7 Canada",
                ManagerName = "Barry Jay",
                Phone = "(604) 266-9010",
                Fax = "(604) 568-9838",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 55,
                Name = "BORDER PHARMACY",
                Address = "3401 E Hastings St, Vancouver BC V5K 2A5 Canada",
                ManagerName = "Chidi Nwaogwugwu",
                Phone = "(877) 393-9265",
                Fax = "(604) 336-9805",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 56,
                Name = "BRENTWOOD PHARMACY",
                Address = "4451 Lougheed Hwy, Burnaby BC V5C 3Z2 Canada",
                ManagerName = "Layla Akbari",
                Phone = "(604) 564-3334",
                Fax = "(604) 564-3335",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 57,
                Name = "BROADWAY PHARMASAVE #73",
                Address = "101 - 2025 West Broadway, Vancouver BC V6J 1Z6 Canada",
                ManagerName = "David Le",
                Phone = "(604) 737-2025",
                Fax = "(604) 737-2046",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 58,
                Name = "BROOKSWOOD REMEDY'S RX PHARMACY #2",
                Address = "105-4061 200th St, Langley BC V3A 1K8 Canada",
                ManagerName = "Shrief Ahmed",
                Phone = "(604) 427-4377",
                Fax = "(604) 427-4378",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 59,
                Name = "BROOKSWOOD REMEDY'SRX",
                Address = "100 - 20103 40 Ave, Langley BC V3A 2W3 Canada",
                ManagerName = "Ahmed Zaiton",
                Phone = "(604) 427-2140",
                Fax = "(604) 427-2141",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 60,
                Name = "BUDGET PHARMACY",
                Address = "5-301 Festubert St, Duncan BC V9L 3T1 Canada",
                ManagerName = "Justin Pagan",
                Phone = "(250) 597-7751",
                Fax = "(250) 597-7752",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 61,
                Name = "BURKE MOUNTAIN PHARMACY",
                Address = "110 - 1465 Salisbury Ave, Port Coquitlam BC V3B 6J3 Canada",
                ManagerName = "Emad Habib",
                Phone = "(604) 941-5575",
                Fax = "(604) 941-5576",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 62,
                Name = "BURNABY PHARMACY",
                Address = "100A - 3300 Boundary Rd, Burnaby BC V5M 0A8 Canada",
                ManagerName = "Reem Zaghloul",
                Phone = "(604) 453-0136",
                Fax = "(604) 453-0137",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 63,
                Name = "BURNABY SQUARE PHARMACY",
                Address = "#107 - 7885 6th St, Burnaby BC V3N 3N4 Canada",
                ManagerName = "Chloe Charm",
                Phone = "(604) 523-1400",
                Fax = "(604) 523-1404",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 64,
                Name = "C.R.C. DRUGS",
                Address = "1008 - 8181 Cambie Road, Richmond BC V6X 3X9 Canada",
                ManagerName = "Jacky Huang",
                Phone = "(604) 285-2555",
                Fax = "(604) 285-2556",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 65,
                Name = "CAMBIE VILLAGE PHARMACY",
                Address = "3025 Cambie St, Vancouver BC V5Z 4N2 Canada",
                ManagerName = "Divyang Bhanvadia",
                Phone = "(604) 630-6900",
                Fax = "(604) 630-7066",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 66,
                Name = "CAMPBELL HEIGHTS PHARMACY",
                Address = "Unit 109 - 2677 192 Street, Surrey BC V3Z 3X1 Canada",
                ManagerName = "Gurmail Sandhu",
                Phone = "(604) 924-7445",
                Fax = "(604) 634-7472",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 67,
                Name = "CAMPBELL RIVER HEALTH & DRUG STORE",
                Address = "Unit B1B - 465 Merecroft Rd, Campbell River BC V9W 6K6 Canada",
                ManagerName = "Joe Myers",
                Phone = "(250) 286-1771",
                Fax = "(250) 286-4662",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 68,
                Name = "CANDRUG",
                Address = "#202 - 8322 130 St, Surrey BC V3W 8J9 Canada",
                ManagerName = "Carol Hou",
                Phone = "(604) 543-8711",
                Fax = "(604) 507-8706",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 69,
                Name = "CANPHARM DRUGS",
                Address = "5853 Victoria Drive, Vancouver BC V5P 3W5 Canada",
                ManagerName = "Stephen Pang",
                Phone = "(604) 321-5133",
                Fax = "(604) 324-4510",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 70,
                Name = "CAP PHARMACY",
                Address = "155 - 8155 Capstan Way, Richmond BC V6X 0V3 Canada",
                ManagerName = "Peter Gao",
                Phone = "(236) 987-7788",
                Fax = "(604) 227-7082",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 71,
                Name = "CAPILANO PHARMACY",
                Address = "2003 Curling Rd, North Vancouver BC V7P 0E5 Canada",
                ManagerName = "Shahrzad Bahrami",
                Phone = "(604) 969-2316",
                Fax = "(778) 309-6211",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 72,
                Name = "CAPITOL HILL PHARMACY",
                Address = "4656 Hastings St, Burnaby BC V5C 2K5 Canada",
                ManagerName = "Samuel Opoku",
                Phone = "(604) 299-9255",
                Fax = "(604) 299-9257",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 73,
                Name = "CARE BOOST PHARMACY",
                Address = "#101 - 9450 120 St, Surrey BC V3V 4B9 Canada",
                ManagerName = "Jagjit Bangar",
                Phone = "(604) 582-5111",
                Fax = "(604) 582-6262",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 74,
                Name = "CARE BOOST PHARMACY #2",
                Address = "#101 - 13190 58A Ave, Surrey BC V3X 0E4 Canada",
                ManagerName = "Eslam Shalaby",
                Phone = "(604) 630-6895",
                Fax = "(604) 630-6896",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 75,
                Name = "CARE FIRST PHARMACY",
                Address = "7612 6th Street, Burnaby BC V3N 0G6 Canada",
                ManagerName = "Regan Patel",
                Phone = "(604) 553-8611",
                Fax = "(604) 553-8610",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 76,
                Name = "CARE IN MOTION PHARMACY",
                Address = "2 - 9880 120 St, Surrey BC V3V 4C9 Canada",
                ManagerName = "Mohamed Ben-Eltriki",
                Phone = "(604) 496-2225",
                Fax = "(604) 496-2234",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 77,
                Name = "CARE N CURE PHARMACY",
                Address = "177 - 8138 128 St, Surrey BC V3W 1R1 Canada",
                ManagerName = "Kuljit Grewal",
                Phone = "(604) 598-3233",
                Fax = "(604) 598-3234",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 78,
                Name = "CARECONNECT IDA PHARMACY",
                Address = "120 - 5301 25 Ave, Vernon BC V1T 9R1 Canada",
                ManagerName = "Zainab Zaheer",
                Phone = "(236) 426-1721",
                Fax = "(833) 444-0296",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 79,
                Name = "CARECO'S PHARMACY",
                Address = "108-22633 Selkirk Ave, Maple Ridge BC V2X 1C7 Canada",
                ManagerName = "Sandeep Nimmagadda",
                Phone = "(778) 375-6299",
                Fax = "(778) 602-2985",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 80,
                Name = "CAREMED PHARMACY",
                Address = "920 Pandora Ave, Victoria BC V8V 3P3 Canada",
                ManagerName = "Ron Turk",
                Phone = "(250) 380-2212",
                Fax = "(250) 380-4959",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 81,
                Name = "CARERX BURNABY",
                Address = "8525 Commerce Crt, Burnaby BC V5A 4N3 Canada",
                ManagerName = "Saem Park",
                Phone = "(604) 872-6762",
                Fax = "(604) 872-6764",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 82,
                Name = "CARERX KELOWNA",
                Address = "120 - 3515 Spectrum Crt, Kelowna BC V1V 2Z1 Canada",
                ManagerName = "Nathan Howe",
                Phone = "(250) 807-6725",
                Fax = "(250) 807-6699",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 83,
                Name = "CARERX PARKSVILLE",
                Address = "Units 1 & 2, 1176 Franklin's Gull Rd, Parksville BC V9P 2M9 Canada",
                ManagerName = "Hafeez Dossa",
                Phone = "(250) 954-3666",
                Fax = "(250) 954-3633",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 84,
                Name = "CARERX VICTORIA",
                Address = "570 Bay St, Victoria BC V8T 1P9 Canada",
                ManagerName = "Frankey He",
                Phone = "(250) 590-3778",
                Fax = "(778) 430-5988",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 85,
                Name = "CAREVILLE PHARMACY FLEETWOOD",
                Address = "107 - 8927 152 St, Surrey BC V3R 4E5 Canada",
                ManagerName = "Manar Abu Sharkh",
                Phone = "(604) 634-1074",
                Fax = "(604) 634-1075",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 86,
                Name = "CARMI REMEDY'SRX",
                Address = "25 - 725 Carmi Ave, Penticton BC V2A 3G8 Canada",
                ManagerName = "David Zamorano",
                Phone = "(778) 476-0010",
                Fax = "(778) 476-0944",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 87,
                Name = "CARVOLTH PHARMACY",
                Address = "#120 - 20290 86 Ave, Langley BC V2Y 3L6 Canada",
                ManagerName = "Jignesh Mistry",
                Phone = "(778) 298-9618",
                Fax = "(778) 298-9621",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 88,
                Name = "CASCADE PHARMACY",
                Address = "Unit A - 46298 Yale Rd, Chilliwack BC V2P 2P6 Canada",
                ManagerName = "Chandrasekar Perumal",
                Phone = "(604) 795-6122",
                Fax = "(604) 795-3065",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 89,
                Name = "CATES PHARMASAVE",
                Address = "203 - 495 Bowen Island Trunk Rd, Bowen Island BC V0N 1G0 Canada",
                ManagerName = "Simin Mirpourzadeh",
                Phone = "(604) 947-0766",
                Fax = "(604) 947-0736",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 90,
                Name = "CEDAR VALLEY PHARMACY",
                Address = "201 - 32818 7th Ave, Mission BC V2V 2C3 Canada",
                ManagerName = "Chuck Foo",
                Phone = "1-866-287-7929",
                Fax = "1-866-287-8329",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 91,
                Name = "CEDAR VALLEY PHARMACY - CENTRAL FILL",
                Address = "1517 W 57th Ave, Vancouver BC V6P 6E9 Canada",
                ManagerName = "Arrian Janfada",
                Phone = "(604) 566-1517",
                Fax = "(604) 566-1515",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 92,
                Name = "CENTRAL COMPOUND PHARMACY",
                Address = "803 E Hasting St, Vancouver BC V6A 1R8 Canada",
                ManagerName = "Amin Janmohamed",
                Phone = "(236) 326-0928",
                Fax = "(236) 326-0929",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 93,
                Name = "CENTRAL DRUGS - BEBAN PLAZA",
                Address = "#16 - 2220 Bowen Road, Nanaimo BC V9S 1H9 Canada",
                ManagerName = "Tessa Kenning",
                Phone = "(250) 758-7711",
                Fax = "(250) 758-7765",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 94,
                Name = "CENTRAL DRUGS - BRICKYARD",
                Address = "#101 - 6010 Brickyard Road, Nanaimo BC V9V 1S5 Canada",
                ManagerName = "Cody Drzewiecki",
                Phone = "(250) 751-2576",
                Fax = "(250) 751-2439",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 95,
                Name = "CENTRAL DRUGS - COLVILE",
                Address = "102-1515 Dufferin Cres, Nanaimo BC V9S 5H6 Canada",
                ManagerName = "Taylor Reitmeier",
                Phone = "(778) 441-5140",
                Fax = "(778) 441-5141",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 96,
                Name = "CENTRAL DRUGS - DUFFERIN",
                Address = "#101 - 1125 Dufferin Cr., Nanaimo BC V9S 2B5 Canada",
                ManagerName = "Sarah Mah",
                Phone = "(250) 716-0063",
                Fax = "(250) 716-0005",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 97,
                Name = "CENTRAL DRUGS - DUNSMUIR",
                Address = "495 Dunsmuir St, Nanaimo BC V9R 6B9 Canada",
                ManagerName = "Jordan Mark",
                Phone = "(250) 753-6401",
                Fax = "(250) 753-6487",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 98,
                Name = "CENTRAL DRUGS - LANTZVILLE",
                Address = "7186 Lantzville Rd, Box 328, Lantzville BC V0R 2H0 Canada",
                ManagerName = "Kylee Power",
                Phone = "(250) 390-4423",
                Fax = "(250) 390-4425",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 99,
                Name = "CENTRAL DRUGS - PACIFIC STATION",
                Address = "103 - 5160 Dublin Way, Nanaimo BC V9T 0H2 Canada",
                ManagerName = "Sylvie Fraser",
                Phone = "(250) 585-6178",
                Fax = "(250) 585-6179",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 100,
                Name = "CENTRAL PARK PHARMACY",
                Address = "3963 Kingsway, Burnaby BC V5H 1Y7 Canada",
                ManagerName = "Miki Wong",
                Phone = "(604) 433-0110",
                Fax = "(604) 432-9327",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 101,
                Name = "CENTRAL PHARMACY SERVICES #5305",
                Address = "102-7635 North Fraser Way, Burnaby BC V5J 0B8 Canada",
                ManagerName = "Nicholas Mah",
                Phone = "(604) 430-4696",
                Fax = "(604) 430-5447",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 102,
                Name = "CHEMMART PHARMACY",
                Address = "1900 - 8171 Ackroyd Rd, Richmond BC V6X 3K1 Canada",
                ManagerName = "William Chen",
                Phone = "(604) 270-9091",
                Fax = "(604) 270-9092",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 103,
                Name = "CHEMMART PHARMACY NO.2",
                Address = "165-8119 Park Rd, Richmond BC V6Y 0M5 Canada",
                ManagerName = "Eric Cheng",
                Phone = "(604) 918-7818",
                Fax = "(604) 918-7817",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 104,
                Name = "CHETWYND DRUG MART PHARMACHOICE #9099",
                Address = "4733 - 51st Street, Chetwynd BC V0C 1J0 Canada",
                ManagerName = "Ronnie Bonifacio",
                Phone = "(250) 788-3393",
                Fax = "(250) 788-2386",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 105,
                Name = "CHIEF LOUIS PHARMACY LTD.",
                Address = "#304-302 Yellowhead Highway, Kamloops BC V2H 0E8 Canada",
                ManagerName = "Alexa Dauk",
                Phone = "(778) 943-1640",
                Fax = "(778) 943-1740",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 106,
                Name = "CHILLIWACK PHARMACY",
                Address = "101- 45863 Yale Rd, Chilliwack BC V2P2N6 Canada",
                ManagerName = "Mounir Khalil",
                Phone = "(604) 402-3100",
                Fax = "(604) 402-3101",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 107,
                Name = "CHOICES PHARMACY",
                Address = "105 - 9093 King George Blvd., Surrey BC V3V 5V7 Canada",
                ManagerName = "Khushbu Patel",
                Phone = "(604) 593-5322",
                Fax = "(604) 593-5320",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 108,
                Name = "CITY CENTRE PHARMACY",
                Address = "110-187 Nanaimo Ave W, Penticton BC V2A 1N2 Canada",
                ManagerName = "Travis Petrisor",
                Phone = "(250) 770-0047",
                Fax = "(250) 770-8853",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 109,
                Name = "CITY PHARMACY",
                Address = "108-7475 135 Street, Surrey BC V3W 0M8 Canada",
                ManagerName = "Rajni .",
                Phone = "(778) 369-3733",
                Fax = "(604) 503-1956",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 110,
                Name = "CITYMED PHARMACY",
                Address = "4 - 1493 Foster St, White Rock BC V4B 0C4 Canada",
                ManagerName = "Megha Patel",
                Phone = "(604) 385-1490",
                Fax = "(604) 385-1491",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 111,
                Name = "CITYMED PHARMACY #2",
                Address = "#107 - 14818 60 Ave, Surrey BC V3S 0B5 Canada",
                ManagerName = "Yash Patel",
                Phone = "(604) 593-8866",
                Fax = "(604) 593-8865",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 112,
                Name = "CLAYTON HEIGHTS CARE FIRST PHARMACY",
                Address = "Unit #101 19390 68 Ave, Surrey BC V4N 6A9 Canada",
                ManagerName = "Kashif Mehmood",
                Phone = "(604) 510-3549",
                Fax = "(604) 510-3568",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 113,
                Name = "CLAYTON HEIGHTS PHARMACY",
                Address = "102 - 7170 188 St, Surrey BC V4N 6R4 Canada",
                ManagerName = "Jaswinder Kahlon",
                Phone = "(604) 372-3808",
                Fax = "(604) 372-3812",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 114,
                Name = "CLINIC DRUG STORE",
                Address = "Medical Building, 816 - 103rd Ave., Dawson Creek BC V1G 2E9 Canada",
                ManagerName = "Anandkumar Patel",
                Phone = "(250) 782-3100",
                Fax = "(250) 782-8120",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 115,
                Name = "CLOUD PHARMACY",
                Address = "4918 Victoria Drive, Vancouver BC V5P 3T6 Canada",
                ManagerName = "Kenneth Choi",
                Phone = "(604) 558-1690",
                Fax = "(604) 558-1691",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 116,
                Name = "COAL HARBOUR PHARMACY",
                Address = "Unit B - 622 Bute St, Vancouver BC V6E 3M1 Canada",
                ManagerName = "Sina Salehi Pirooz",
                Phone = "(604) 336-3038",
                Fax = "(604) 336-3044",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 117,
                Name = "COASTAL CARE PHARMACY",
                Address = "101 - 10183 152A St, Surrey BC V3R 4H6 Canada",
                ManagerName = "Tegbir Rajasancy",
                Phone = "(604) 588-0484",
                Fax = "(604) 588-0485",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 118,
                Name = "COASTAL CARE PHARMACY #2",
                Address = "103 - 15420 Fraser Hwy, Surrey BC V3R 3P5 Canada",
                ManagerName = "Madhu Rana",
                Phone = "(604) 589-9064",
                Fax = "(604) 589-9066",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 119,
                Name = "COASTAL CARE PHARMACY #3",
                Address = "104 - 5795 176 Street, Surrey BC V3S 4E1 Canada",
                ManagerName = "Kashyap Patel",
                Phone = "(604) 283-9002",
                Fax = "(778) 547-6250",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 120,
                Name = "COASTAL CARE PHARMACY #4",
                Address = "118-18525 53 Ave, Surrey BC V3S 7A4 Canada",
                ManagerName = "Ashish Patel",
                Phone = "(236) 607-0531",
                Fax = "(604) 227-7688",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 121,
                Name = "CO-DRUG MART PHARMACY",
                Address = "4-100 Lombardy St, Parksville BC V9P 0G4 Canada",
                ManagerName = "Yasser Mahmoud",
                Phone = "(250) 951-9699",
                Fax = "(250) 951-9690",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 122,
                Name = "COLLINGWOOD PHARMACY",
                Address = "2732 41st Ave E, Vancouver BC V5R 2X1 Canada",
                ManagerName = "Elsie Lee",
                Phone = "(604) 428-6455",
                Fax = "(604) 428-6457",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 123,
                Name = "COLUMBIA PRESCRIPTION X-PRESS",
                Address = "317 Columbia St E, New Westminster BC V3L 3W8 Canada",
                ManagerName = "Aysen Paryab",
                Phone = "(604) 525-3784",
                Fax = "(604) 525-3734",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 124,
                Name = "COLUMBIA ST. PHARMACY",
                Address = "Columbia 300, #112 - 300 Columbia St, Kamloops BC V2C 6L1 Canada",
                ManagerName = "Cory Proctor",
                Phone = "(778) 471-5971",
                Fax = "(778) 471-5973",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 125,
                Name = "COMMON GOOD PHARMACY",
                Address = "105-3610 Carrington Rd, Westbank BC V4T 3K7 Canada",
                ManagerName = "Christopher Carter",
                Phone = "(778) 795-0529",
                Fax = "(778) 699-4768",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 126,
                Name = "COMMON GOOD PHARMACY - SUMMERLAND",
                Address = "101 - 13207 Victoria Road N, Summerland BC V0H 1Z0 Canada",
                ManagerName = "Austin Ojala",
                Phone = "(250) 404-0913",
                Fax = "(250) 404-0921",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 127,
                Name = "COMMUNITY APOTHECARY",
                Address = "402 - 3701 Hastings St, Burnaby BC V5C 2H6 Canada",
                ManagerName = "Bennedick Koh",
                Phone = "(604) 757-1254",
                Fax = "(604) 563-7852",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 128,
                Name = "COMMUNITY OUTREACH PHARMACY",
                Address = "#309 - 800 Carleton Crt, Delta BC V3M 6Y6 Canada",
                ManagerName = "Ricky Ram",
                Phone = "(604) 777-5601",
                Fax = "(778) 654-7172",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 129,
                Name = "COMMUNITY PHARMACY",
                Address = "Units 3 & 4 - 2785 Bourquin Cres W, Abbotsford BC V2S 5X6 Canada",
                ManagerName = "Pooja Nadpara",
                Phone = "(604) 776-2991",
                Fax = "(604) 776-2992",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 130,
                Name = "COMOX VALLEY PHARMACY",
                Address = "300 - 727 Anderton Road, Comox BC V9M 4A9 Canada",
                ManagerName = "Kushalkumar Patel",
                Phone = "(250) 941-6685",
                Fax = "(250) 941-6686",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 131,
                Name = "CONTINENTAL PHARMACY",
                Address = "1196 - 3779 Sexsmith Rd, Richmond BC V6X 3Z9 Canada",
                ManagerName = "Joseph Leung",
                Phone = "(604) 276-8938",
                Fax = "(604) 276-8940",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 132,
                Name = "CONTINENTAL PHARMACY #2",
                Address = "5316 Victoria Dr, Vancouver BC V5P 3V7 Canada",
                ManagerName = "Anita Liu",
                Phone = "(604) 327-6823",
                Fax = "(604) 327-0272",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 133,
                Name = "COOL-AID DISPENSARY",
                Address = "713 Johnson St, Victoria BC V8W 1M8 Canada",
                ManagerName = "Zvonimir Petrusa",
                Phone = "(250) 385-8469",
                Fax = "(250) 383-5933",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 134,
                Name = "CORNING DRUGS #2",
                Address = "102-236 Georgia St E, Vancouver BC V6A 1Z7 Canada",
                ManagerName = "Brenda Chow",
                Phone = "(604) 685-7609",
                Fax = "(604) 685-7672",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 135,
                Name = "CORNING DRUGS LTD.",
                Address = "101-515 Main St, Vancouver BC V6A 2V1 Canada",
                ManagerName = "Angela Ser",
                Phone = "(604) 685-9056",
                Fax = "(604) 685-8681",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 136,
                Name = "COSTCO PHARMACY #   51",
                Address = "3550 Brighton Ave., Burnaby BC V5A 4W3 Canada",
                ManagerName = "Eric Trinh",
                Phone = "(604) 420-9811",
                Fax = "(778) 309-6382",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 137,
                Name = "COSTCO PHARMACY #   54",
                Address = "9151 Bridgeport Road, Richmond BC V6X 3L9 Canada",
                ManagerName = "Lorraine Yee",
                Phone = "(604) 270-1163",
                Fax = "(778) 309-6385",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 138,
                Name = "COSTCO PHARMACY #   55",
                Address = "7423 King George Blvd, Surrey BC V3W 5A8 Canada",
                ManagerName = "Islam Eldeeb",
                Phone = "(604) 596-0757",
                Fax = "(778) 607-2952",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 139,
                Name = "COSTCO PHARMACY # 155",
                Address = "6700 Island Hwy N, Nanaimo BC V9V 1K8 Canada",
                ManagerName = "Anita Sorensen Wessel",
                Phone = "(250) 390-0585",
                Fax = "(236) 362-1096",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 140,
                Name = "COSTCO PHARMACY # 158",
                Address = "2555 Range Road, Prince George BC V2N 4G8 Canada",
                ManagerName = "Nathan Linkletter",
                Phone = "(250) 614-1759",
                Fax = "(236) 381-2071",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 141,
                Name = "COSTCO PHARMACY # 161",
                Address = "1675 Versatile Drive, Kamloops BC V1S 1W7 Canada",
                ManagerName = "Ripudaman Randhawa",
                Phone = "(250) 372-7348 x 0",
                Fax = "(778) 376-2193",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 142,
                Name = "COSTCO PHARMACY # 163",
                Address = "1127 Sumas Way, Abbotsford BC V2S 8H2 Canada",
                ManagerName = "Amar Mahal",
                Phone = "(604) 864-3935",
                Fax = "(778) 360-2993",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 143,
                Name = "COSTCO PHARMACY # 255",
                Address = "2370 Ottawa St, Port Coquitlam BC V3B 7Z1 Canada",
                ManagerName = "Thomas Zhang",
                Phone = "(604) 552-2298",
                Fax = "(604) 342-1127",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 144,
                Name = "COSTCO PHARMACY # 256",
                Address = "799 McCallum Rd, Victoria BC V9B 6A2 Canada",
                ManagerName = "Shelina Dawood",
                Phone = "(250) 391-8986",
                Fax = "(250) 410-1246",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 145,
                Name = "COSTCO PHARMACY # 259",
                Address = "20499 64th Ave, Langley BC V2Y 1N5 Canada",
                ManagerName = "Amritpal Bhathal",
                Phone = "(604) 539-8928",
                Fax = "(778) 309-6384",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 146,
                Name = "COSTCO PHARMACY # 548",
                Address = "4500 Still Creek Dr, Burnaby BC V5C 0E5 Canada",
                ManagerName = "Jordan Asayo",
                Phone = "(604) 296-5109",
                Fax = "(778) 309-6381",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 147,
                Name = "COSTCO PHARMACY # 552",
                Address = "605 Expo Blvd, Vancouver BC V6B 1V4 Canada",
                ManagerName = "Peggy Ku",
                Phone = "(604) 622-5059",
                Fax = "(778) 309-6386",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 148,
                Name = "COSTCO PHARMACY #1092",
                Address = "588 Crown Isle Blvd, Courtenay BC V9N 0A6 Canada",
                ManagerName = "Nathan Lueder",
                Phone = "(250) 331-8710",
                Fax = "(236) 269-2106",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 149,
                Name = "COSTCO PHARMACY #1240",
                Address = "Unit #1, 3550 Brighton Avenue, Burnaby BC V5A 4W3 Canada",
                ManagerName = "Amarvir Dosanjh",
                Phone = "(778) 732-1456",
                Fax = "(778) 309-6383",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 150,
                Name = "COSTCO PHARMACY #1578",
                Address = "2125 Baron Rd, Kelowna BC V1X 0B2 Canada",
                ManagerName = "Kelly Yee",
                Phone = "(250) 868-2548",
                Fax = "(778) 699-4648",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 151,
                Name = "COTTON PHARMACY",
                Address = "#100A - 504 Cottonwood Ave, Coquitlam BC V3J 2R5 Canada",
                ManagerName = "Jin Huh",
                Phone = "(604) 931-2396",
                Fax = "(604) 939-8311",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 152,
                Name = "COTTONWOOD REMEDYS'RX",
                Address = "100 - 45428 Luckakuck Way, Chilliwack BC V2R 3S9 Canada",
                ManagerName = "Rajesh Sharma",
                Phone = "(604) 858-9446",
                Fax = "(604) 858-9447",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 153,
                Name = "COUNTRY GROCER PHARMACY",
                Address = "372 Lower Ganges Rd, Salt Spring Island BC V8K 2V7 Canada",
                ManagerName = "Carla Grant",
                Phone = "(250) 538-0323",
                Fax = "(250) 538-0326",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 154,
                Name = "COURTENAY PHARMACY",
                Address = "5A - 2401 Cliffe Ave, Courtenay BC V9N 2L5 Canada",
                ManagerName = "Christopher Sutton",
                Phone = "(250) 871-8405",
                Fax = "(250) 871-8409",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 155,
                Name = "COVE PHARMACY",
                Address = "672 Plaza Rd, Box 614, Quadra Island, Quathiaski Cove BC V0P 1N0 Canada",
                ManagerName = "Clayton Palmer",
                Phone = "(250) 285-2275",
                Fax = "(250) 285-3375",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 156,
                Name = "COWRIE PHARMACY",
                Address = "101 - 5699 Cowrie St, Sechelt BC V0N 3A0 Canada",
                ManagerName = "Amany Mady",
                Phone = "(604) 885-0580",
                Fax = "(604) 885-0572",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 157,
                Name = "CP CLEARBROOK PHARMACY",
                Address = "#1 - 2629 Cedar Park Place, Abbotsford BC V2T 3S4 Canada",
                ManagerName = "Kajri Mehta",
                Phone = "(604) 852-4466",
                Fax = "(604) 744-3444",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 158,
                Name = "CRANBROOK IDA PHARMACY",
                Address = "13 24th Ave N, Cranbrook BC V1C 3H9 Canada",
                ManagerName = "Richard Lockhart",
                Phone = "(250) 420-4133",
                Fax = "(250) 420-4135",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 159,
                Name = "CRC CARE@HOME PHARMACY",
                Address = "Unit 2575 - 3700 No. 3 Road, Richmond BC V6X 3X2 Canada",
                ManagerName = "Wendy Tsai",
                Phone = "(604) 370-1001",
                Fax = "(604) 370-1864",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 160,
                Name = "CREATE COMPOUNDING PHARMACY",
                Address = "1140-577 Nicola Ave, Port Coquitlam BC V3B 0P2 Canada",
                ManagerName = "Anastasios Raptis",
                Phone = "(604) 409-4146",
                Fax = "(604) 409-4179",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 161,
                Name = "CRESCENT PHARMACY INTEGRATIVE HEALTH CENTRE",
                Address = "115-10880 No. 5 Rd, Richmond BC V6W 0B3 Canada",
                ManagerName = "Hong Yu Su",
                Phone = "(604) 370-8875",
                Fax = "(604) 370-8876",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 162,
                Name = "CRIDGE FAMILY PHARMACY",
                Address = "641 Fort St, Victoria BC V8W 1G1 Canada",
                ManagerName = "Patrick Falkiner",
                Phone = "(250) 686-7104",
                Fax = "(778) 433-7848",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 163,
                Name = "CRIDGE FAMILY PHARMACY 2",
                Address = "2136b Keating Cross Rd, Saanichton BC V8M 2A6 Canada",
                ManagerName = "Mateya Radisavljevic",
                Phone = "(250) 652-8880",
                Fax = "1-855-297-5608",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 164,
                Name = "CRIDGE FAMILY PHARMACY 3",
                Address = "108 - 1411 Cook St, Victoria BC V8V 0E8 Canada",
                ManagerName = "Danica Hart",
                Phone = "(250) 590-8711",
                Fax = "1-855-647-1319",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 165,
                Name = "CRIDGE FAMILY PHARMACY 4",
                Address = "1918 Oak Bay Ave, Victoria BC V8R 1C7 Canada",
                ManagerName = "Christopher Stokes",
                Phone = "(250) 940-0223",
                Fax = "(250) 410-3588",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 166,
                Name = "CROFTON PHARMACY",
                Address = "101 - 8146 Queen St, PO Box 1089, Crofton BC V0R 1R0 Canada",
                ManagerName = "Erika Dunlop",
                Phone = "(250) 324-5554",
                Fax = "(250) 324-5558",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 167,
                Name = "CROSSROADS PHARMACY",
                Address = "140 - 2061 Sumas Way, Abbotsford BC V2S 8H6 Canada",
                ManagerName = "Amir Morkos",
                Phone = "(778) 880-0125",
                Fax = "(778) 880-0282",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 168,
                Name = "CRYSTAL PHARMACY & MEDICAL SUPPLIES",
                Address = "1611 - 4500 Kingsway, Burnaby BC V5H 2A9 Canada",
                ManagerName = "Andre Lo",
                Phone = "(604) 433-2821",
                Fax = "(604) 433-2830",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 169,
                Name = "CUMBERLAND PHARMASAVE #1015",
                Address = "101-2665 Beaufort Ave, Cumberland BC V0R 1S0",
                ManagerName = "Amanda Nakagawa",
                Phone = "(250) 400-3456",
                Fax = "(250) 400-2942",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 170,
                Name = "CUSTOM HEALTH PHARMACY CHILLIWACK",
                Address = "A - 45555 Hodgins Ave, Chilliwack BC V2P 1P3 Canada",
                ManagerName = "Baher Habib",
                Phone = "(604) 402-4555",
                Fax = "(604) 402-4556",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 171,
                Name = "CUSTOM HEALTH PHARMACY LANGLEY",
                Address = "19967 96th Ave, Langley BC V1M 3C6 Canada",
                ManagerName = "Christine Wahba",
                Phone = "(604) 609-4111",
                Fax = "(604) 609-4114",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 172,
                Name = "DAILY CARE PHARMACY",
                Address = "4973 Victoria Dr, Vancouver BC V5P 3T7 Canada",
                ManagerName = "Surya Gunnam",
                Phone = "(604) 336-9440",
                Fax = "(604) 336-9441",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 173,
                Name = "DAVIE PHARMACY",
                Address = "1232 Davie St, Vancouver BC V6E 1N3 Canada",
                ManagerName = "Ian Stead",
                Phone = "(604) 559-9952",
                Fax = "(604) 559-7752",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 174,
                Name = "DAVIES PRESCRIPTIONS - ST. GEORGES",
                Address = "1401 St. Georges Ave., North Vancouver BC V7L 3J3 Canada",
                ManagerName = "Mohinder Jaswal",
                Phone = "(604) 985-8771",
                Fax = "(604) 985-8262",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 175,
                Name = "DAWSON PHARMACY",
                Address = "4218 Dawson St, Burnaby BC V5C 0B8 Canada",
                ManagerName = "Megha Patel",
                Phone = "(604) 428-9755",
                Fax = "(604) 428-9756",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 176,
                Name = "DEAN PHARMACY",
                Address = "102A - 6844 King George Blvd, Surrey BC V3W 4Z9 Canada",
                ManagerName = "Faisel Dean",
                Phone = "(778) 564-3326",
                Fax = "(778) 564-3777",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 177,
                Name = "DEEP COVE PHARMACY",
                Address = "4322 Gallant Ave, North Vancouver BC V7G 1K8 Canada",
                ManagerName = "Lani Ha",
                Phone = "(604) 985-3539",
                Fax = "(604) 985-3540",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 178,
                Name = "DELTA PRESCRIPTION CLINIC",
                Address = "#101 - 8425 120th St, Delta BC V4C 6R2 Canada",
                ManagerName = "Susan Minty",
                Phone = "(604) 594-4499",
                Fax = "(604) 594-4155",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 179,
                Name = "DOGWOOD PHARMACY",
                Address = "231B Dogwood St, Campbell River BC V9W 2Y1 Canada",
                ManagerName = "Mark Iosiphovich",
                Phone = "(778) 560-3511",
                Fax = "(250) 703-9988",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 180,
                Name = "DONEX PHARMACY & DEPARTMENT STORE",
                Address = "145 Birch Ave, 100 Mile House BC V0K 2E0 Canada",
                ManagerName = "Brian Oster",
                Phone = "(250) 395-4004",
                Fax = "(250) 644-2251",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 181,
                Name = "DOUGLAS COMPOUNDING PHARMACY",
                Address = "376 175A Street, Surrey BC V3Z 6S7 Canada",
                ManagerName = "Munpreet Sarao",
                Phone = "(604) 245-0727",
                Fax = "(604) 634-7497",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 182,
                Name = "DOWNTOWN CLINIC PHARMACY",
                Address = "569 Powell Street, Vancouver BC V6A 1G8 Canada",
                ManagerName = "Amy Huang",
                Phone = "(604) 216-4257",
                Fax = "(604) 216-4270",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 183,
                Name = "DOWNTOWN VANCOUVER PHARMACY",
                Address = "1102 Pender St West, Vancouver BC V6E 2S1 Canada",
                ManagerName = "Mahtab Dehboureh",
                Phone = "(604) 844-1801",
                Fax = "(604) 844-1824",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 184,
                Name = "DRUGLAND PHARMACY",
                Address = "#107 - 7738 Edmonds St, Burnaby BC V3N 1B8 Canada",
                ManagerName = "Grace Lee",
                Phone = "(604) 636-0666",
                Fax = "(604) 636-0663",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 185,
                Name = "DRUGSTORE PHARMACY #6722",
                Address = "200 Carmi Avenue, Penticton BC V2A 3G5 Canada",
                ManagerName = "Pradeepkumar Ramachandran Kabali",
                Phone = "(250) 493-0053",
                Fax = "(250) 493-4078",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 186,
                Name = "DRUGSTORE PHARMACY #6734",
                Address = "1000 South Lakeside Drive, Williams Lake BC V2G 3A6 Canada",
                ManagerName = "Helen Ibarra",
                Phone = "(250) 305-2158",
                Fax = "(250) 305-2162",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 187,
                Name = "DUNCAN PHARMACY",
                Address = "101-725 Canada Ave, Duncan BC V9L 1V1 Canada",
                ManagerName = "Nilesh Tanna",
                Phone = "250-597-4100",
                Fax = "250-597-4191",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 188,
                Name = "DUNDAS PHARMACY",
                Address = "2081 Dundas St, Vancouver BC V5L 1J5 Canada",
                ManagerName = "Afshin Talaie",
                Phone = "(778) 379-5095",
                Fax = "(778) 379-5094",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 189,
                Name = "DYCK'S PHARMACISTS (PANDOSY)",
                Address = "3039 Pandosy St, Kelowna BC V1Y 1W3 Canada",
                ManagerName = "Alex Mazurkewich",
                Phone = "(778) 478-0360",
                Fax = "(778) 478-0361",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 190,
                Name = "DYCK'S PHARMACISTS (SPRINGFIELD)",
                Address = "1111 Springfield Road, Kelowna BC V1Y 8R7 Canada",
                ManagerName = "Matthew Thompson",
                Phone = "(250) 762-7774",
                Fax = "(250) 762-7718",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 191,
                Name = "DYCK'S PHARMACISTS (ST. PAUL)",
                Address = "1460 St. Paul St, Kelowna BC V1Y 2E6 Canada",
                ManagerName = "Jane Lyons",
                Phone = "(250) 762-3333",
                Fax = "(250) 862-8829",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 192,
                Name = "DYCK'S PHARMACISTS IDA (GLENMORE)",
                Address = "116-1920 Summit Dr, Kelowna BC V1V 3E9 Canada",
                ManagerName = "Melissa Twaites",
                Phone = "(250) 762-4411",
                Fax = "(250) 762-6868",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 193,
                Name = "EAGLE LANDING PHARMACY",
                Address = "504 - 8236 Eagle Landing Parkway, Chilliwack BC V2R 0R5 Canada",
                ManagerName = "Bhaumik Shah",
                Phone = "(604) 392-5529",
                Fax = "(604) 392-5539",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 194,
                Name = "EASECARE PHARMACY",
                Address = "Unit 101 - 8585 160 St, Surrey BC V4N 1G4 Canada",
                ManagerName = "Rupinder Oberoi",
                Phone = "(778) 578-5160",
                Fax = "(778) 578-5165",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 195,
                Name = "EAST HASTINGS PHARMACY",
                Address = "633 E Hastings, Vancouver BC V6A 1R2 Canada",
                ManagerName = "Ranjeet Singh",
                Phone = "(604) 860-6000",
                Fax = "(604) 398-6465",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 196,
                Name = "EASTSIDE PHARMACY LTD.",
                Address = "398 Hastings St E, Vancouver BC V6A 1P4 Canada",
                ManagerName = "Jamie Harrison",
                Phone = "(604) 255-1714",
                Fax = "(604) 255-1753",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 197,
                Name = "EDGE PRO PHARMACY",
                Address = "100 - 6245 136th Street, Surrey BC V3X 1H3 Canada",
                ManagerName = "Sanaa Abdelati",
                Phone = "(604) 503-1766",
                Fax = "(604) 503-1776",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 198,
                Name = "EDMONDS PHARMACY",
                Address = "105 - 7315 Edmonds St, Burnaby BC V3N 1A7 Canada",
                ManagerName = "Azim Datoo",
                Phone = "(604) 526-1110",
                Fax = "(604) 526-1926",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 199,
                Name = "ELEMENTS COMPOUNDING PHARMACY",
                Address = "3540 Blanshard St, Victoria BC V8X 1W3 Canada",
                ManagerName = "Priti Bhathella",
                Phone = "(250) 590-6777",
                Fax = "(250) 590-6778",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 200,
                Name = "ELKFORD I.D.A. PHARMACY",
                Address = "4 Front Street, Suite B, Elkford BC V0B 1H0 Canada",
                ManagerName = "Douglas Pereverzoff",
                Phone = "(778) 521-5181",
                Fax = "(778) 521-5182",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 201,
                Name = "ELYSIAN PHARMACY LTD.",
                Address = "110 - 2626 Croydon Drive, Surrey BC V3Z 0S8 Canada",
                ManagerName = "Shane Lakerveld",
                Phone = "(604) 256-4132",
                Fax = "(778) 613-2089",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 202,
                Name = "EMC PHARMACY",
                Address = "#180 13091 Vanier Place, Richmond BC V6V 2J1 Canada",
                ManagerName = "Ricky Lee",
                Phone = "(604) 207-5433",
                Fax = "(604) 207-5437",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 203,
                Name = "ENDERBY PHARMACY AND WELLNESS CENTRE",
                Address = "513 Cliff Ave, Enderby BC V0E 1V0 Canada",
                ManagerName = "Allyson Lemke",
                Phone = "(250) 838-0502",
                Fax = "(250) 838-7233",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 204,
                Name = "ENHANCE RX",
                Address = "302 - 3965 Kingsway Ave, Burnaby BC V5H 1Y8 Canada",
                ManagerName = "Shayna Ding",
                Phone = "(604) 336-7280",
                Fax = "(604) 336-7281",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 205,
                Name = "ESTEVAN PHARMACY LIMITED",
                Address = "2517 Estevan Avenue, Victoria BC V8R 2S6 Canada",
                ManagerName = "Lesley Blackman",
                Phone = "(250) 598-2517",
                Fax = "(250) 598-2512",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 206,
                Name = "EVERGREEN PHARMACY",
                Address = "104 - 1168 The High St, Coquitlam BC V3B 0C6 Canada",
                ManagerName = "Fariba Pourghadiri",
                Phone = "(604) 474-3837",
                Fax = "(604) 474-3835",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 207,
                Name = "EVERWELL PHARMACY",
                Address = "8179 Granville St, Vancouver BC V6P 4Z6 Canada",
                ManagerName = "Michele Cheung",
                Phone = "(604) 563-8282",
                Fax = "(604) 563-8280",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 208,
                Name = "EXPRESS CARE PHARMACY",
                Address = "25 - 6014 Vedder Road, Chilliwack BC V2R 5M4 Canada",
                ManagerName = "Maruf Karuji",
                Phone = "(604) 705-0029",
                Fax = "(604) 705-0087",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 209,
                Name = "EXPRESS SCRIPTS CANADA PHARMACY",
                Address = "Unit 125 - 2250 Boundary Rd, Burnaby BC V5M 3Z3 Canada",
                ManagerName = "Mandy Yang",
                Phone = "1-855-550-6337",
                Fax = "1-888-807-6972",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 210,
                Name = "FAMILY CARE PHARMACY",
                Address = "Unit B, 12815 96 Ave, Surrey BC V3V 6V9 Canada",
                ManagerName = "Kiran Vithlani",
                Phone = "(604) 581-1900",
                Fax = "(604) 581-1902",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 211,
                Name = "FAMILY CARE PHARMACY #2",
                Address = "8925 120 St, Delta BC V4C 6R6 Canada",
                ManagerName = "Kinjalben Ahir",
                Phone = "(604) 599-0211",
                Fax = "(604) 599-4318",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 212,
                Name = "FAMILY CARE PHARMACY #3",
                Address = "1539 Johnston Rd, White Rock BC V4B 3Z6 Canada",
                ManagerName = "Thilak Kulasekaran",
                Phone = "(604) 536-1300",
                Fax = "(604) 536-1322",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 213,
                Name = "FAMILY CARE PHARMACY #4",
                Address = "B105 - 20020 84th Ave, Langley BC V2Y 5K9 Canada",
                ManagerName = "Alexsandra Cridge",
                Phone = "(604) 539-1611",
                Fax = "(604) 539-1655",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 214,
                Name = "FELIX PHARMACY WEST INC.",
                Address = "1150-955 Seaborne Avenue, Port Coquitlam BC V3B 0R9 Canada",
                ManagerName = "Jagdeep Tatla",
                Phone = "(604) 552-6941",
                Fax = "(604) 552-6942",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 215,
                Name = "FERNIE'S PHARMACY",
                Address = "3 - 562 2 Ave, Fernie BC V0B 1M0 Canada",
                ManagerName = "Ariel McLeod",
                Phone = "(250) 430-0525",
                Fax = "(236) 526-2057",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 216,
                Name = "FINLANDIA NATURAL PHARMACY",
                Address = "1111 West Broadway, Vancouver BC V6H 1G1 Canada",
                ManagerName = "Frederick Cheng",
                Phone = "(604) 733-5323",
                Fax = "(604) 733-5340",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 217,
                Name = "FIVE CORNERS PHARMACY",
                Address = "1189 Johnston Rd, White Rock BC V4B 3Y7 Canada",
                ManagerName = "Richard Cote",
                Phone = "(778) 545-3700",
                Fax = "(778) 545-3701",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 218,
                Name = "FLASH PHARMACY MEDICAL CLINIC CENTRE",
                Address = "1562 Lonsdale Ave, North Vancouver BC V7M 2J3 Canada",
                ManagerName = "Sina Soleimani Pari",
                Phone = "(604) 971-4464",
                Fax = "(604) 971-4465",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 219,
                Name = "FLEETWOOD PHARMACY REMEDY'SRX",
                Address = "305 - 9014 152 St, Surrey BC V3R 4E7 Canada",
                ManagerName = "Sarah Nina Binuya",
                Phone = "(604) 496-3303",
                Fax = "(604) 496-3301",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 220,
                Name = "FORBES PHARMACY - BURNSIDE",
                Address = "1 - 101 Burnside Rd W, Victoria BC V9A 1B7 Canada",
                ManagerName = "Cyrus Dinh",
                Phone = "(778) 432-3784",
                Fax = "(778) 432-3785",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 221,
                Name = "FORBES PHARMACY - FORT STREET",
                Address = "1775 Fort St., Victoria BC V8R 1J3 Canada",
                ManagerName = "Tom Fourt",
                Phone = "(250) 595-1471",
                Fax = "(250) 595-1911",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 222,
                Name = "FORBES PHARMACY - GOLDSTREAM",
                Address = "Unit 111 - 755 Goldstream Ave, Victoria BC V9B 2X4 Canada",
                ManagerName = "Kanrawee Taotawin",
                Phone = "(250) 478-7300",
                Fax = "(250) 478-7008",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 223,
                Name = "FORBES PHARMACY - GORGE",
                Address = "603 Gorge Rd E, Victoria BC V8T 2W7 Canada",
                ManagerName = "Mahmoud Ghoneim",
                Phone = "(250) 590-8811",
                Fax = "(250) 590-8824",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 224,
                Name = "FORBES PHARMACY - HELMCKEN",
                Address = "120 - 27 Helmcken Rd, Victoria BC V8Z 5G5 Canada",
                ManagerName = "Joy John Del Mundo",
                Phone = "(778) 265-8181",
                Fax = "(778) 265-7171",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 225,
                Name = "FORBES PHARMACY - MILLSTREAM",
                Address = "105 - 2349 Millstream Rd, Langford BC V9B 3R5 Canada",
                ManagerName = "Sunny Deol",
                Phone = "(250) 478-1600",
                Fax = "(250) 478-0400",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 226,
                Name = "FORBES PHARMACY - PANDORA",
                Address = "922 Pandora Ave, Victoria BC V8V 3P3 Canada",
                ManagerName = "Nancy Sunday",
                Phone = "(250) 385-3784",
                Fax = "(250) 385-3700",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 227,
                Name = "FORBES PHARMACY - QUEEN CHARLOTTE",
                Address = "3209 Oceanview Drive, Daajing Giids BC V0T 1S0 Canada",
                ManagerName = "Tegan Graetz",
                Phone = "(250) 559-4910",
                Fax = "(250) 559-4915",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 228,
                Name = "FORBES PHARMACY SOOKE",
                Address = "6691 Logan Lane, Sooke BC V9Z 1A5 Canada",
                ManagerName = "Meghan Major",
                Phone = "(778) 352-5040",
                Fax = "(778) 352-5041",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 229,
                Name = "FORKS PHARMACY",
                Address = "426 Central Ave, Grand Forks BC V0H 1H0 Canada",
                ManagerName = "Robert Zaborowski",
                Phone = "(250) 442-1868",
                Fax = "(833) 333-2965",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 230,
                Name = "FORT NELSON PHARMACY",
                Address = "4904 - 50th Ave. N., Box 540, Fort Nelson BC V0C 1R0 Canada",
                ManagerName = "Jaspreet Maan",
                Phone = "(250) 774-2323",
                Fax = "(250) 774-2326",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 231,
                Name = "FORT ROYAL PHARMACY",
                Address = "1803 Fort St, Victoria BC V8R 1J6 Canada",
                ManagerName = "Sunryoung Yi",
                Phone = "(250) 590-3707",
                Fax = "(250) 590-3708",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 232,
                Name = "FORT ROYAL PHARMACY COLWOOD INC.",
                Address = "B102-681 Allandale Rd, Victoria BC V9C 0S2 Canada",
                ManagerName = "Kush Khanna",
                Phone = "(250) 474-0605",
                Fax = "(250) 474-0606",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 233,
                Name = "FORT ROYAL PHARMACY HILLSIDE INC.",
                Address = "50-797 Hillside Ave, Victoria BC V8T1Z5 Canada",
                ManagerName = "Vikram Bawa",
                Phone = "(778) 406-2022",
                Fax = "(778) 406-2023",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 234,
                Name = "FORT ROYAL PHARMACY OAK BAY",
                Address = "2217 Oak Bay Ave, Victoria BC V8R1G4 Canada",
                ManagerName = "Paige Laufer",
                Phone = "(250) 590-9217",
                Fax = "(250) 590-9218",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 235,
                Name = "FORT ST. JOHN PHARMACY AND WELLNESS CENTRE",
                Address = "#300 - 9730 - 101st Ave, Fort St. John BC V1J 2A8 Canada",
                ManagerName = "Cory Hermans",
                Phone = "(250) 785-3234",
                Fax = "(250) 785-7696",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 236,
                Name = "FORTUNE CREEK PHARMACY",
                Address = "2 - 2860 Smith Dr, Armstrong BC V0E 1B1 Canada",
                ManagerName = "Melanie Lukens",
                Phone = "(250) 546-1411",
                Fax = "(250) 546-1422",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 237,
                Name = "FRASER CANYON PHARMACY",
                Address = "308 Wallace St., Hope BC V0X 1L0 Canada",
                ManagerName = "Tarek Mahmoud",
                Phone = "(604) 869-5654",
                Fax = "(604) 869-5665",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 238,
                Name = "FRASER CARE PHARMACY",
                Address = "6448 Fraser St., Vancouver BC V5W 3A4 Canada",
                ManagerName = "Ankit Raval",
                Phone = "(604) 325-8288",
                Fax = "(604) 558-0578",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 239,
                Name = "FRASER COMMONS PHARMACY",
                Address = "717 SE Marine Dr, Vancouver BC V5X 2T8 Canada",
                ManagerName = "Ripal Mistry",
                Phone = "(604) 800-2223",
                Fax = "(604) 398-8430",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 240,
                Name = "FRASER HEIGHTS PHARMACY LTD.",
                Address = "#102 - 16033 - 108th Ave., Surrey BC V4N 1P2 Canada",
                ManagerName = "Mariam Samaan",
                Phone = "(604) 930-9544",
                Fax = "(604) 930-9785",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 241,
                Name = "FRASER LAKE MEDICINE CENTRE",
                Address = "280 McMillan Ave, Fraser Lake BC V0J 1S0 Canada",
                ManagerName = "Murray Johnson",
                Phone = "(250) 699-0075",
                Fax = "(250) 699-0070",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 242,
                Name = "FRASER MEDICARE PHARMACY",
                Address = "4127 Fraser St, Vancouver BC V5V 4E9 Canada",
                ManagerName = "Nadia Hassanpour Fard",
                Phone = "(604) 620-8278",
                Fax = "(604) 620-8277",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 243,
                Name = "FRASER MEDICINE CENTRE PHARMACY",
                Address = "20200 Fraser Hwy, Langley BC V3A 4E6 Canada",
                ManagerName = "Aliya Khan",
                Phone = "(604) 530-8810",
                Fax = "(604) 530-8843",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 244,
                Name = "FRASER PHARMACHOICE",
                Address = "4207 Fraser St, Vancouver BC V5V 4G1 Canada",
                ManagerName = "Kevin Kang",
                Phone = "(604) 872-1151",
                Fax = "(604) 872-1187",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 245,
                Name = "FRASER VALLEY PHARMACY",
                Address = "105 - 2760 Trethewey St, Abbotsford BC V2T 3R1 Canada",
                ManagerName = "Harvinder Dhaliwal",
                Phone = "(604) 859-1794",
                Fax = "(778) 757-7770",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 246,
                Name = "FRASERGROVE PHARMACY",
                Address = "2941 272 St, Aldergrove BC V4W 3R3 Canada",
                ManagerName = "Avninder Sekhon",
                Phone = "(604) 381-5981",
                Fax = "(604) 381-5983",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 247,
                Name = "FRASERVIEW PHARMACY LTD.",
                Address = "#11 - 665 Front Street, Quesnel BC V2J 2K9 Canada",
                ManagerName = "Glen Boudreau",
                Phone = "(250) 992-3822",
                Fax = "(250) 992-3766",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 248,
                Name = "FRASERWAY MEDICINE CENTRE",
                Address = "107 - 32615 South Fraser Way, Abbotsford BC V2T 1X8 Canada",
                ManagerName = "Yaqub Shah",
                Phone = "(604) 504-2022",
                Fax = "(604) 504-2044",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 249,
                Name = "FRENCH CREEK PHARMACY",
                Address = "5 - 886 Wembley Rd, Parksville BC V9P 2E6 Canada",
                ManagerName = "Divine Gatpandan",
                Phone = "(250) 586-6212",
                Fax = "(250) 586-6214",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 250,
                Name = "FRESHCO PHARMACY #4252",
                Address = "6140 Blundell Road, Richmond BC V7C 1H6 Canada",
                ManagerName = "Joseph Del Rosario",
                Phone = "(604) 274-7370",
                Fax = "(604) 274-7647",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 251,
                Name = "FRESHCO PHARMACY #4254",
                Address = "10151 No. 3 Road, Richmond BC V7A 4R6 Canada",
                ManagerName = "Gregg Letendre",
                Phone = "(604) 271-7734",
                Fax = "(604) 271-7713",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 252,
                Name = "FRESHCO PHARMACY #4255",
                Address = "7165 - 138th Street, Surrey BC V3W 7T9 Canada",
                ManagerName = "Repinder Nagra",
                Phone = "(604) 594-4515",
                Fax = "(604) 594-4802",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 253,
                Name = "FRESHCO PHARMACY #4256",
                Address = "32520 Lougheed Highway, Mission BC V2V 1A5 Canada",
                ManagerName = "Hsin-Chieh Wu",
                Phone = "(604) 826-5398",
                Fax = "(604) 826-7967",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 254,
                Name = "FRESHCO PHARMACY #4257",
                Address = "7450 - 120th Street, Surrey BC V3W 3M9 Canada",
                ManagerName = "Harjit Dhillon",
                Phone = "(604) 594-9866",
                Fax = "(604) 594-1167",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 255,
                Name = "FRESHCO PHARMACY #4265",
                Address = "27566 Fraser Hwy, Aldergrove BC V4W 3N5 Canada",
                ManagerName = "Karim Gabra",
                Phone = "(604) 856-4667",
                Fax = "(604) 856-7223",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 256,
                Name = "FRESHCO PHARMACY #4269",
                Address = "45858 Yale Rd, Chilliwack BC V2P 2N9 Canada",
                ManagerName = "Derek Soochan",
                Phone = "(604) 795-6092",
                Fax = "(604) 795-6034",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 257,
                Name = "FRESHCO PHARMACY #4270",
                Address = "#300 - 20201 Lougheed Hwy, Maple Ridge BC V2X 2P6 Canada",
                ManagerName = "Michael Damjanovic",
                Phone = "(604) 460-7200",
                Fax = "(604) 460-7242",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 258,
                Name = "FRESHCO PHARMACY #4272",
                Address = "#100 - 32500 S. Fraser Way, Abbotsford BC V2T 4W1 Canada",
                ManagerName = "Helen Mak",
                Phone = "(604) 852-3558",
                Fax = "(604) 852-5660",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 259,
                Name = "FRESHCO PHARMACY #4274",
                Address = "451 Oliver St., Williams Lake BC V2G 1M5 Canada",
                ManagerName = "Cynthia Bolt",
                Phone = "(250) 398-8380",
                Fax = "(250) 398-7087",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 260,
                Name = "FRESHCO PHARMACY #4276",
                Address = "#5- 945 Columbia St W, Kamloops BC V2C 1L5 Canada",
                ManagerName = "Lisa Smillie",
                Phone = "(250) 372-1994",
                Fax = "(250) 374-9167",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 261,
                Name = "FRESHCO PHARMACY #4277",
                Address = "#500 - 2339 Hwy 97 N, Kelowna BC V1X 4H9 Canada",
                ManagerName = "Cody Loewen",
                Phone = "(250) 860-4431",
                Fax = "(250) 860-2584",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 262,
                Name = "FRESHCO PHARMACY #4278",
                Address = "3417 - 30th Avenue, Vernon BC V1T 2E3 Canada",
                ManagerName = "Andre Ortmayr",
                Phone = "(250) 542-8008",
                Fax = "(250) 542-5569",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 263,
                Name = "FRESHCO PHARMACY #4280",
                Address = "7040 Barnet Street, Powell River BC V8A 2A1 Canada",
                ManagerName = "Mervin Banting",
                Phone = "(604) 485-4244",
                Fax = "(604) 485-3063",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 264,
                Name = "FRONTIER STREET PHARMACY - IDA",
                Address = "7437 Frontier St., Pemberton BC V0N 2L0 Canada",
                ManagerName = "Iwona Bartnicka",
                Phone = "(604) 894-6707",
                Fax = "(604) 894-6258",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 265,
                Name = "FRUITVALE IDA PHARMACY",
                Address = "1942 Main St., Fruitvale BC V0G 1L0 Canada",
                ManagerName = "Sheryl Achaol",
                Phone = "(250) 367-9331",
                Fax = "(250) 367-7111",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 266,
                Name = "GAETZ PHARMASAVE",
                Address = "103 - 7408 Vedder Rd, Chilliwack BC V2R 0T8 Canada",
                ManagerName = "Wael Bebawy",
                Phone = "(604) 846-4226",
                Fax = "(604) 846-4228",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 267,
                Name = "GAIN PHARMACY",
                Address = "10677 King George Blvd, Surrey BC V3T 2X6 Canada",
                ManagerName = "Javad Ghane",
                Phone = "(604) 582-4246",
                Fax = "(604) 582-4276",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 268,
                Name = "GARIBALDI PHARMACY",
                Address = "102 - 1870 Dowad Drive, Squamish BC V8B 1C4 Canada",
                ManagerName = "Dean George",
                Phone = "(604) 848-7059",
                Fax = "(778) 605-2939",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 269,
                Name = "GARLANE PHARMACY LTD.",
                Address = "232 East Hastings St, Vancouver BC V6A 1P1 Canada",
                ManagerName = "Gary Siu",
                Phone = "(604) 684-6720",
                Fax = "(604) 398-2891",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 270,
                Name = "GARRISON PHARMACY",
                Address = "1 - 45555 Market Way, Chilliwack BC V2R 0M5 Canada",
                ManagerName = "Pamela Engar",
                Phone = "(604) 846-8782",
                Fax = "(604) 846-8794",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 271,
                Name = "GATEWAY PHARMACY",
                Address = "13717 72 Ave, Surrey BC V3W 2P2 Canada",
                ManagerName = "Tom Chang",
                Phone = "(604) 930-8608",
                Fax = "(604) 930-2732",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 272,
                Name = "GENERATION PHARMACY",
                Address = "A 11771 225 St, Maple Ridge BC V2X 6E6 Canada",
                ManagerName = "Hashem Moazzen-Ahmadi",
                Phone = "(604) 544-7111",
                Fax = "(604) 544-8333",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 273,
                Name = "GILMORE PHARMACY",
                Address = "320 Gilmore Ave, Burnaby BC V5C 4R1 Canada",
                ManagerName = "Marmar Rabieighahfarokhi",
                Phone = "(604) 568-8383",
                Fax = "(604) 568-8332",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 274,
                Name = "GLADWIN PHARMACY",
                Address = "#104 - 2955 Gladwin Rd, Abbotsford BC V2T 5T4 Canada",
                ManagerName = "Amrik Ghag",
                Phone = "(604) 850-2494",
                Fax = "(604) 853-9675",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 275,
                Name = "GLEN PHARMACY",
                Address = "208 - 1175 Johnson St, Coquitlam BC V3B 7K1 Canada",
                ManagerName = "Saeid Shahram",
                Phone = "(604) 944-5500",
                Fax = "(604) 944-3301",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 276,
                Name = "GLENCOURT DISTRIBUTORS",
                Address = "105-19515 56 Ave, Surrey BC V3S 6K3 Canada",
                ManagerName = "Kevin Youn",
                Phone = "(604) 539-0375",
                Fax = "(604) 539-1464",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 277,
                Name = "GOLDEN EARS PHARMACY",
                Address = "100 - 22722 Lougheed Hwy, Maple Ridge BC V2X 2V6 Canada",
                ManagerName = "Karim Virani",
                Phone = "(604) 477-3222",
                Fax = "(604) 477-3221",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 278,
                Name = "GOLDEN FAMILY PHARMACY",
                Address = "916 10th Avenue South, Golden BC V0A 1H0 Canada",
                ManagerName = "Hayley Pelletier",
                Phone = "(250) 344-6821",
                Fax = "(250) 344-6869",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 279,
                Name = "GONZALES PHARMACY",
                Address = "1845 Fairfield Road, Victoria BC V8S 1G9 Canada",
                ManagerName = "Grant Rowley",
                Phone = "(250) 598-5512",
                Fax = "(250) 590-3998",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 280,
                Name = "GOOD CURE PHARMACY",
                Address = "5 - 5725 Vedder Road, Chilliwack BC V2R 3N4 Canada",
                ManagerName = "Gaurav Dhankhar",
                Phone = "(604) 847-1255",
                Fax = "(236) 436-2607",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 281,
                Name = "GOOD FAITH PHARMACY",
                Address = "A-4035 Redford St, Port Alberni BC V9Y 3R9 Canada",
                ManagerName = "Nitin Saini",
                Phone = "(778) 421-1035",
                Fax = "(778) 421-0757",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 282,
                Name = "GORDON HEAD PHARMACY",
                Address = "102 - 1660 Feltham Rd, Victoria BC V8N 2A1 Canada",
                ManagerName = "Vladislav Stevanovic",
                Phone = "(250) 590-0557",
                Fax = "(250) 590-5898",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 283,
                Name = "GOURLAY'S GOLDEN PHARMACY",
                Address = "P.O. Box 1106, 826B - 9th Ave S, Golden BC V0A 1H0 Canada",
                ManagerName = "Janelle Cannon",
                Phone = "(250) 344-8600",
                Fax = "(250) 344-8622",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 284,
                Name = "GRACE PHARMACY",
                Address = "3835 Sunset St, Burnaby BC V5G 1T4 Canada",
                ManagerName = "Tan Thinh Le",
                Phone = "(604) 434-1722",
                Fax = "(604) 909-1722",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 285,
                Name = "GRANVILLE PHARMACY",
                Address = "1146 Granville St, Vancouver BC V6Z 1L8 Canada",
                ManagerName = "Emin Nadjafov",
                Phone = "(604) 558-0700",
                Fax = "(604) 558-0722",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 286,
                Name = "GRAY'S COMPOUNDING PHARMACY",
                Address = "417B 304th St, Kimberley BC V1A 3H4 Canada",
                ManagerName = "Michelle Gray",
                Phone = "(250) 427-0038",
                Fax = "(250) 427-0039",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 287,
                Name = "GREEN PHARMACY",
                Address = "204-901 Lougheed Hwy, Coquitlam BC V3K 0J3 Canada",
                ManagerName = "Jae Kwag",
                Phone = "(236) 607-0018",
                Fax = "1-833-664-5458",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 288,
                Name = "GREENSHIELD PHARMACY",
                Address = "#118 - 5589 Byrne Road, Burnaby BC V5J 3J1 Canada",
                ManagerName = "Sandy Chu",
                Phone = "(778) 244-1788",
                Fax = "(604) 648-9939",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 289,
                Name = "GROVE PHARMACY",
                Address = "Walnut Grove Prof. Corner, 402 - 21183 88 Ave, Langley BC V1M 2G5 Canada",
                ManagerName = "Sameer Premji",
                Phone = "(778) 298-1000",
                Fax = "(778) 298-1012",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 290,
                Name = "GUARDIAN HANIN PHARMACY",
                Address = "210 - 329 North Rd, Coquitlam BC V3K 3V8 Canada",
                ManagerName = "Jun Mo Ku",
                Phone = "(604) 939-7880",
                Fax = "(604) 939-7875",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 291,
                Name = "GUILDFORD DRUGS",
                Address = "200 - 15135 101 Ave, Surrey BC V3R 7Z1 Canada",
                ManagerName = "Ajay Arora",
                Phone = "(778) 394-3784",
                Fax = "(778) 395-3784",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 292,
                Name = "H&J FAMILY PHARMACY",
                Address = "115-1488 Flint Avenue, Langford BC V9B 5N1 Canada",
                ManagerName = "Hanan Thorne",
                Phone = "(250) 242-2323",
                Fax = "(250) 955-1111",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 293,
                Name = "HAFEZ PHARMACY AND TRAVEL CLINIC",
                Address = "108-2669 Langdon St, Abbotsford BC V2T 3L3 Canada",
                ManagerName = "Navid Danaee-Moghaddam",
                Phone = "(604) 852-8355",
                Fax = "(604) 852-8492",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 294,
                Name = "HANEY PHARMACY",
                Address = "Suite D, 22195 Dewdney Trunk Road, Maple Ridge BC V2X 3H7 Canada",
                ManagerName = "Miyoung Baek",
                Phone = "(604) 467-9100",
                Fax = "(604) 463-7189",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 295,
                Name = "HAPPY FACE PHARMACY",
                Address = "7 - 8590 200 St, Langley BC V2Y 2B9 Canada",
                ManagerName = "Patrick Akhnouh",
                Phone = "(604) 371-2794",
                Fax = "(604) 371-2795",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 296,
                Name = "HARBOUR PHARMACY",
                Address = "3855 9th Ave, Port Alberni BC V9Y 4T9 Canada",
                ManagerName = "Jyoti Modhgill",
                Phone = "(250) 723-8100",
                Fax = "(888) 675-9906",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 297,
                Name = "HARBOURSIDE PHARMACHOICE",
                Address = "1584 Broughton Blvd., Port McNeill BC V0N 2R0 Canada",
                ManagerName = "Brittany Swanson",
                Phone = "(250) 956-3126",
                Fax = "(250) 956-4245",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 298,
                Name = "HARBOURVIEW DRUGSTORE",
                Address = "1892 Peninsula Rd, Ucluelet BC V0R 3A0 Canada",
                ManagerName = "Jianxing Yang",
                Phone = "(250) 726-2733",
                Fax = "(250) 726-2734",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 299,
                Name = "HARDY BAY DRUGSTORE",
                Address = "Unit 100 - 8950 Granville St, PO Box 909, Port Hardy BC V0N 2P0 Canada",
                ManagerName = "Kristen Ireton",
                Phone = "(250) 949-9522",
                Fax = "(250) 949-9532",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 300,
                Name = "HART DRUG MART",
                Address = "6707 Dagg Rd, Prince George BC V2K 2R6 Canada",
                ManagerName = "Brett Chiasson",
                Phone = "(250) 962-9603",
                Fax = "(250) 962-8450",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 301,
                Name = "HARVARD PHARMACY",
                Address = "492 Kingsway, Vancouver BC V5T 3J9 Canada",
                ManagerName = "Peter Dang",
                Phone = "(604) 875-0200",
                Fax = "(604) 639-0020",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 302,
                Name = "HARVEST DRIVE PHARMACY",
                Address = "#100 - 4515 Harvest Dr., Delta BC V4K 4L1 Canada",
                ManagerName = "Laura Palle",
                Phone = "(604) 946-5220",
                Fax = "(604) 946-3902",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 303,
                Name = "HEALING PHARMACY",
                Address = "#7 - 103 4501 North Rd., Burnaby BC V3N 4R7 Canada",
                ManagerName = "Ikju Lee",
                Phone = "(604) 420-0756",
                Fax = "(604) 676-2795",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 304,
                Name = "HEALING PHARMACY #2",
                Address = "7414 Edmonds St, Burnaby BC V3N 1A8 Canada",
                ManagerName = "Sihyun Lee",
                Phone = "(236) 453-1186",
                Fax = "(778) 627-2834",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 305,
                Name = "HEALTHCARE PHARMACY",
                Address = "866 Twelfth St, New Westminster BC V3M 4K3 Canada",
                ManagerName = "Krupen Shah",
                Phone = "(604) 540-1325",
                Fax = "(604) 540-4315",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 306,
                Name = "HEALTHCARE PHARMACY #2",
                Address = "130 - 8780 Blundell Road, Richmond BC V6Y 3Y8 Canada",
                ManagerName = "Angus Li",
                Phone = "(604) 275-7800",
                Fax = "(604) 275-7400",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 307,
                Name = "HEALTHCARE PHARMACY #3",
                Address = "13352 Old Yale Road, Surrey BC V3T 5A4",
                ManagerName = "Aditi Shah",
                Phone = "(778) 975-0017",
                Fax = "(778) 607-2010",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 308,
                Name = "HEALTHMART PHARMACY",
                Address = "109-8556 120 St, Surrey BC V3W 3N5 Canada",
                ManagerName = "Ajaz Munshi",
                Phone = "(604) 503-3163",
                Fax = "(604) 503-3168",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 309,
                Name = "HEALTHPLUS PHARMACY",
                Address = "4115 No. 5 Rd, Richmond BC V6X 2T9 Canada",
                ManagerName = "Selina Grewal",
                Phone = "(604) 304-1294",
                Fax = "(604) 304-1295",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 310,
                Name = "HEALTHPLUS PHARMACY #2",
                Address = "170 - 8980 No. 3 Rd, Richmond BC V6Y 2E8 Canada",
                ManagerName = "Dave Aulakh",
                Phone = "(604) 370-6799",
                Fax = "(604) 370-6798",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 311,
                Name = "HEALTHRX PHARMACY",
                Address = "100 - 135 15th St E, North Vancouver BC V7L 2P7 Canada",
                ManagerName = "Celia Ma",
                Phone = "(604) 770-1609",
                Fax = "(604) 770-1610",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 312,
                Name = "HEALTHSIDE PHARMACY",
                Address = "5448 Victoria Drive, Vancouver BC V5P 3V8 Canada",
                ManagerName = "Ava So",
                Phone = "(604) 327-6768",
                Fax = "(604) 327-6761",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 313,
                Name = "HEART PHARMACY AT ESQUIMALT",
                Address = "H 890 Esquimalt Rd, Victoria BC V9A 3M4 Canada",
                ManagerName = "Derek Pacheco",
                Phone = "(778) 433-2721",
                Fax = "(833) 333-2916",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 314,
                Name = "HEART PHARMACY IDA AT CADBORO BAY VILLAGE",
                Address = "3825 Cadboro Bay Road, Victoria BC V8N 4G1 Canada",
                ManagerName = "Ian Lloyd",
                Phone = "(250) 477-2131",
                Fax = "(250) 477-5491",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 315,
                Name = "HEART PHARMACY IDA AT FAIRFIELD PLAZA",
                Address = "Fairfield Plaza, #15 - 1594 Fairfield Rd., Victoria BC V8S 1G1 Canada",
                ManagerName = "Mario Bruno Bossio",
                Phone = "(250) 598-9232",
                Fax = "(250) 598-9238",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 316,
                Name = "HEART PHARMACY IDA AT SHELBOURNE PLAZA",
                Address = "3643 Shelbourne Street, Victoria BC V8P 4H1 Canada",
                ManagerName = "Andrea Silver",
                Phone = "(250) 477-1881",
                Fax = "(250) 477-7672",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 317,
                Name = "HELLO PHARMACY #3301",
                Address = "4882 Main Street, Vancouver BC V5V 3R8 Canada",
                ManagerName = "Kareem Abdel Meguid",
                Phone = "(778) 606-7873",
                Fax = "(778) 724-0143",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 318,
                Name = "HELLO PHARMACY #3302",
                Address = "115, 1700 Garcia Street, Merritt BC V1K 1B8 Canada",
                ManagerName = "Alfie Zayn",
                Phone = "(236) 575-2273",
                Fax = "(236) 575-2274",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 319,
                Name = "HERBACY PHARMAHEALTH INTERNATIONAL LIMITED",
                Address = "Unit 102 - 14045 104 Ave, Surrey BC V3T 1X4 Canada",
                ManagerName = "Monique Mahil",
                Phone = "(604) 498-8102",
                Fax = "(604) 498-8103",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 320,
                Name = "HERITAGE PARK PHARMACY",
                Address = "Unit B150 - 7871 Stave Lake St, Mission BC V2V 0C5 Canada",
                ManagerName = "Manpreet Sandhu",
                Phone = "(604) 289-2447",
                Fax = "(604) 289-2448",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 321,
                Name = "HERITAGE PHARMACY",
                Address = "9136 Young Rd, Chilliwack BC V2P 4R4 Canada",
                ManagerName = "Dina Elhosary",
                Phone = "(604) 824-5300",
                Fax = "(236) 436-2989",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 322,
                Name = "HIGH GATE PHARMACY LTD.",
                Address = "#102 - 7188 Kingsway, Burnaby BC V5E 1G3 Canada",
                ManagerName = "Rahim Kanji",
                Phone = "(604) 777-4267",
                Fax = "(604) 777-4268",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 323,
                Name = "HIGH STREET PHARMACY",
                Address = "17-1161 The High St, Coquitlam BC V3B 7W3 Canada",
                ManagerName = "",
                Phone = "(778) 504-5117",
                Fax = "(778) 504-5118",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 324,
                Name = "HILLCREST PHARMACY LTD.",
                Address = "100 - 32156 Hillcrest Ave, Abbotsford BC V2T 1S5 Canada",
                ManagerName = "Simranjeet Sandhu",
                Phone = "(604) 746-1333",
                Fax = "(604) 746-1334",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 325,
                Name = "HILLSIDE FAMILY PHARMACY",
                Address = "A - 541 3rd Ave, Ladysmith BC V9G 1B9 Canada",
                ManagerName = "Sumitha Sasi",
                Phone = "(250) 924-2288",
                Fax = "(250) 924-2289",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 326,
                Name = "HOGARTH'S CLINIC PHARMACY LTD.",
                Address = "102 - 3310 - 32nd Ave, Vernon BC V1T 2M6 Canada",
                ManagerName = "Kerriann Fowler",
                Phone = "(250) 545-3660",
                Fax = "(250) 545-4392",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 327,
                Name = "HOMELINX PHARMACY",
                Address = "101 - 12126 90 Ave, Surrey BC V3V 1B5 Canada",
                ManagerName = "Shivinder Badyal",
                Phone = "(604) 503-6470",
                Fax = "(604) 503-6469",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 328,
                Name = "HOPE PHARMACY",
                Address = "224 Wallace St, Hope BC V0X 1L0 Canada",
                ManagerName = "Ron Najibnia",
                Phone = "(604) 860-2144",
                Fax = "(604) 860-2140",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 329,
                Name = "HOWE SOUND PHARMACY",
                Address = "#208 - 1100 Sunshine Coast Hwy, Gibsons BC V0N 1V7 Canada",
                ManagerName = "Chris Juozaitis",
                Phone = "(604) 886-3365",
                Fax = "(604) 886-3052",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 330,
                Name = "HUB PHARMACY",
                Address = "106-33069 Marshall Rd, Abbotsford BC V2S 1K4 Canada",
                ManagerName = "Ivan Glivenko",
                Phone = "(604) 556-8515",
                Fax = "(604) 556-8366",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 331,
                Name = "HUMBLE COMPOUNDING PHARMACY",
                Address = "#101 4769 222 Street, Langley BC V2Z 3C1 Canada",
                ManagerName = "Jaskarn Khaira",
                Phone = "(604) 835-5606",
                Fax = "(604) 756-2770",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 332,
                Name = "I.D.A. FAMILY DRUG MART",
                Address = "100 - 10504 100 Ave, Fort St. John BC V1J 1Z2 Canada",
                ManagerName = "Smita Bhatia",
                Phone = "(250) 261-7039",
                Fax = "(877) 376-0243",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 333,
                Name = "I.G.S. VALUE DRUG MART",
                Address = "441 - 2nd Ave., Fernie BC V0B 1M0 Canada",
                ManagerName = "Leland Sims",
                Phone = "(250) 423-4511",
                Fax = "(250) 423-4543",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 334,
                Name = "ICARE REMEDY'SRX",
                Address = "103 - 13805 104 Ave, Surrey BC V3T 1W7 Canada",
                ManagerName = "Rasha Guirguis",
                Phone = "(604) 498-0480",
                Fax = "(604) 498-0481",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 335,
                Name = "IDA BOUNDARY PHARMACY",
                Address = "612 - 6th Avenue, Box 400, Midway BC V0H 1M0 Canada",
                ManagerName = "Cris Bennett",
                Phone = "(250) 449-2866",
                Fax = "(250) 449-2867",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 336,
                Name = "IDEAL CARE PHARMACY",
                Address = "100 - 817 W Hastings St, Vancouver BC V6C 3N9 Canada",
                ManagerName = "Shrideep Patel",
                Phone = "(604) 757-1530",
                Fax = "(604) 398-8094",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 337,
                Name = "IHEALTH PHARMACY",
                Address = "101 - 45619 Yale Road, Chilliwack BC V2P 2N1 Canada",
                ManagerName = "Rutu Patel",
                Phone = "(604) 392-8393",
                Fax = "(236) 436-2986",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 338,
                Name = "IMPERIAL NEIGHBOURHOOD PHARMACY",
                Address = "2 - 4648 Imperial St, Burnaby BC V5J 1B8 Canada",
                ManagerName = "Samunder Sindhu",
                Phone = "(604) 428-9647",
                Fax = "(604) 428-9649",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 339,
                Name = "IMPERIAL PHARMACY #2",
                Address = "981 Carnarvon St, New Westminster BC V3M 1G2 Canada",
                ManagerName = "Rida Bazzi",
                Phone = "(604) 523-6767",
                Fax = "(604) 523-6768",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 340,
                Name = "IMPERIAL REMEDY'S RX PHARMACY",
                Address = "5262 Imperial St, Burnaby BC V5J 1E5 Canada",
                ManagerName = "Behnoosh Jaffari",
                Phone = "(604) 229-2360",
                Fax = "(604) 449-2291",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 341,
                Name = "INDIGICARE MEDICINES LTD.",
                Address = "2982 Nanaimo St, Vancouver BC V5N 5G3 Canada",
                ManagerName = "Yisa Yen",
                Phone = "(236) 521-9449",
                Fax = "(236) 259-5320",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 342,
                Name = "INDIGO PHARMACY",
                Address = "447 East Columbia Street, New Westminster BC V3L 3X3 Canada",
                ManagerName = "Joanne Hui",
                Phone = "(604) 553-8996",
                Fax = "(604) 553-8993",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 343,
                Name = "INDIGO PHARMACY COMMERCIAL",
                Address = "1623 Commercial Dr, Vancouver BC V5L 3Y3 Canada",
                ManagerName = "Min Jeong Kim",
                Phone = "(604) 566-8963",
                Fax = "(604) 566-8962",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 344,
                Name = "INDIGO PHARMACY MID-MAIN",
                Address = "2205 Main Street, Vancouver BC V5T 0K2 Canada",
                ManagerName = "Min Seok Seo",
                Phone = "(604) 423-2882",
                Fax = "(604) 423-2887",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 345,
                Name = "INGRAM CLINIC PHARMACY",
                Address = "#101 - 149 Ingram St., Duncan BC V9L 1N8 Canada",
                ManagerName = "Michael Allen",
                Phone = "(250) 746-5191",
                Fax = "(250) 746-8413",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 346,
                Name = "INLET PHARMACY LTD.",
                Address = "50 Electronic Avenue, Port Moody BC V3H 2R8 Canada",
                ManagerName = "Navid Ahmadzai",
                Phone = "(604) 937-2818",
                Fax = "(604) 937-2820",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 347,
                Name = "INNOMAR PHARMACY",
                Address = "100 - 5898 Trapp Ave, Burnaby BC V3N 5G4 Canada",
                ManagerName = "Alkarim Prebtani",
                Phone = "(604) 563-2700",
                Fax = "(604) 563-2701",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 348,
                Name = "ISLAND CENTRAL FILL PHARMACY",
                Address = "45 - 1400 Cowichan Bay Road, Cobble Hill BC V0R 1L3 Canada",
                ManagerName = "James Spring",
                Phone = "(250) 929-2100",
                Fax = "(250) 929-2102",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 349,
                Name = "ISLAND PHARMACY",
                Address = "#13 - 575 North Road, RR #3, Gabriola Island BC V0R 1X3 Canada",
                ManagerName = "Ameen Al-Tamawi",
                Phone = "(250) 247-8310",
                Fax = "(250) 247-8313",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 350,
                Name = "ISLAND PHARMACY # 1",
                Address = "138 South Shore Road, Box 38, Lake Cowichan BC V0R 2G0 Canada",
                ManagerName = "Espen Lyngberg",
                Phone = "(250) 749-3141",
                Fax = "(250) 749-4315",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 351,
                Name = "ISLAND PHARMACY # 2",
                Address = "62 Cowichan Lake Road, Lake Cowichan BC V0R 2G0 Canada",
                ManagerName = "Brad Cromwell",
                Phone = "(250) 749-0149",
                Fax = "(250) 749-7219",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 352,
                Name = "ISLAND PHARMACY # 3",
                Address = "102 - 330 Festubert St, Duncan BC V9L 3S9 Canada",
                ManagerName = "Dean Bryson",
                Phone = "(250) 746-7494",
                Fax = "(250) 746-7493",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 353,
                Name = "ISLAND PHARMACY # 4",
                Address = "192 Kenneth St, Duncan BC V9L 1N4 Canada",
                ManagerName = "Gemma Van Doesburg",
                Phone = "(250) 746-4680",
                Fax = "(250) 746-4682",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 354,
                Name = "ISLAND PHARMACY # 6",
                Address = "26 - 1400 Cowichan Bay Rd, RR 3, Cobble Hill BC V0R 1L0 Canada",
                ManagerName = "Frederick Bristow",
                Phone = "(250) 743-1448",
                Fax = "(250) 743-1480",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 355,
                Name = "ISLAND PHARMACY # 7",
                Address = "1-1769 Shawnigan Mill Bay Rd., Shawnigan Lake BC V0R 2W0 Canada",
                ManagerName = "Anna Callegari",
                Phone = "(250) 743-6977",
                Fax = "(250) 743-6976",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 356,
                Name = "ISLAND PHARMACY # 8",
                Address = "Unit A - 845 Deloume Rd, RR 2, Mill Bay BC V0R 2P2 Canada",
                ManagerName = "Angela Foss",
                Phone = "(250) 743-4421",
                Fax = "(250) 743-8897",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 357,
                Name = "ISLAND PHARMACY #10",
                Address = "#106 - 284 Helmcken Road, Victoria BC V9B 1T2 Canada",
                ManagerName = "Blaine Wilkins",
                Phone = "(250) 881-8887",
                Fax = "(250) 881-1210",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 358,
                Name = "ISLANDCARE PHARMACY",
                Address = "A 2440 Cliffe Ave, Courtenay BC V9N 2L6 Canada",
                ManagerName = "Dhruvalkumar Patel",
                Phone = "(250) 871-9090",
                Fax = "(250) 871-9099",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 359,
                Name = "JAMIE'S PHARMACY",
                Address = "103 - 2802 30th St, Vernon BC V1T 8G7 Canada",
                ManagerName = "Jamie Nicolson",
                Phone = "(250) 541-8999",
                Fax = "(250) 541-8907",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 360,
                Name = "JC PHARMACY",
                Address = "211 - 3214 Douglas St, Victoria BC V8Z 3K6 Canada",
                ManagerName = "Chandra Erant",
                Phone = "(250) 590-9080",
                Fax = "(250) 590-8033",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 361,
                Name = "JC PHARMACY - 02",
                Address = "47 Gorge Rd East, Victoria BC V9A 0J8 Canada",
                ManagerName = "Jayabarathi Erant",
                Phone = "(250) 940-7466",
                Fax = "(250) 410-1849",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 362,
                Name = "JERICHO PHARMACY & HEALTH FOOD STORE",
                Address = "Unit 290 - 2083 Alma St., Vancouver BC V6R 4N6 Canada",
                ManagerName = "Ding Gang Wang",
                Phone = "(604) 228-8978",
                Fax = "(604) 228-0798",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 363,
                Name = "JOE'S FAMILY PHARMACY",
                Address = "7819D East Saanich Rd, Saanichton BC V8M 2B4 Canada",
                ManagerName = "Theresa Tran Nguyen",
                Phone = "(778) 426-2420",
                Fax = "(778) 506-2391",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 364,
                Name = "JOE'S FAMILY PHARMACY #2",
                Address = "101-622 Admirals Rd, Esquimalt BC V9A 2N7 Canada",
                ManagerName = "Mackenzie Shyngera",
                Phone = "(778) 817-1754",
                Fax = "(778) 698-7054",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 365,
                Name = "JOE'S FAMILY PHARMACY #3",
                Address = "#116 - 10330 McDonald Park Road, North Saanich BC V8L 5X7 Canada",
                ManagerName = "Lara Ellis",
                Phone = "(778) 817-1591",
                Fax = "(250) 410-1851",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 366,
                Name = "JOE'S SPECIALTY PHARMACY",
                Address = "1515 McTavish Road, North Saanich BC V8L 5T3 Canada",
                ManagerName = "Mohamed Ihab Abd El Hady",
                Phone = "(250) 410-1000 ext: 41703",
                Fax = "(236) 916-2999",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 367,
                Name = "JOE'S SPECIALTY PHARMACY - VANCOUVER",
                Address = "2839 Kingsway, Vancouver BC V5R 5H9 Canada",
                ManagerName = "Ting Luu",
                Phone = "(604) 924-8686",
                Fax = "(833) 999-0871",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 368,
                Name = "JUNIPER PHARMACY",
                Address = "1607 Pandosy Street, Kelowna BC V1Y 1P6 Canada",
                ManagerName = "Craig Plain",
                Phone = "(250) 762-7306",
                Fax = "(250) 762-7193",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 369,
                Name = "KAMA HEALTH PHARMACY",
                Address = "201 - 7350 King George Blvd, Surrey BC V3W 5A5 Canada",
                ManagerName = "Janice Kolba",
                Phone = "1-888-533-9842",
                Fax = "1-855-710-6444",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 370,
                Name = "KAMLOOPS DOWNTOWN PHARMACY",
                Address = "205 Victoria Street, Kamloops BC V2C 2A1 Canada",
                ManagerName = "Clancy O'Malley",
                Phone = "(250) 374-2216",
                Fax = "(250) 374-2218",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 371,
                Name = "KASLO COMMUNITY PHARMACY",
                Address = "403 Front St, Kaslo BC V0G 1M0 Canada",
                ManagerName = "Ward Taylor",
                Phone = "(250) 353-2224",
                Fax = "(250) 353-2336",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 372,
                Name = "KATZIE PHARMACY",
                Address = "19700 Salish Rd, Pitt Meadows BC V3Y 2G1 Canada",
                ManagerName = "Shaymaa Aly",
                Phone = "(604) 457-1030",
                Fax = "(604) 457-1031",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 373,
                Name = "KENSINGTON PHARMACY",
                Address = "916 Kingsway, Vancouver BC V5V 3C4 Canada",
                ManagerName = "Jackson Wong",
                Phone = "(604) 428-6800",
                Fax = "(604) 428-6900",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 374,
                Name = "KERRISDALE MEDICINE CENTRE PHARMACY",
                Address = "5591 West Boulevard, Vancouver BC V6M 3W6 Canada",
                ManagerName = "Eugene Mar",
                Phone = "(604) 261-0333",
                Fax = "(604) 261-0311",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 375,
                Name = "KILLARNEY PHARMACY",
                Address = "#102 - 2607 East 49th Ave, Vancouver BC V5S 1J9 Canada",
                ManagerName = "Thuy Nguyen",
                Phone = "(778) 800-8528",
                Fax = "(778) 800-8530",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 376,
                Name = "KING GEORGE MEDIC PHARMACY",
                Address = "102 - 14333 104 Ave, Surrey BC V3T 0E1 Canada",
                ManagerName = "Grace Kim",
                Phone = "(604) 585-8866",
                Fax = "(604) 585-7036",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 377,
                Name = "KINGSTONS PHARMACY",
                Address = "10051 Whalley Blvd, Surrey BC V3T 4G1 Canada",
                ManagerName = "Amanjyoti Sagoo",
                Phone = "(604) 585-0525",
                Fax = "(604) 585-0522",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 378,
                Name = "KIPP-MALLERY DALLAS PHARMACY IDA",
                Address = "102 - 5170 Dallas Dr, Kamloops BC V2C 0C7 Canada",
                ManagerName = "Kassia Gifford",
                Phone = "(778) 469-5271",
                Fax = "(778) 469-5274",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 379,
                Name = "KIPP-MALLERY LANDMARK PHARMACY",
                Address = "Landmark Centre, 207 755 McGill Rd, Kamloops BC V2C 0B6 Canada",
                ManagerName = "Robert Caravan",
                Phone = "(236) 425-0025",
                Fax = "(236) 425-0040",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 380,
                Name = "KIPP-MALLERY PHARMACY",
                Address = "273 Victoria St., Kamloops BC V2C 2A1 Canada",
                ManagerName = "Kristina Gifford",
                Phone = "(250) 372-2531",
                Fax = "(250) 372-1736",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 381,
                Name = "KIWI PHARMACY & WELLNESS",
                Address = "109-810 Clement Ave, Kelowna BC V1Y 0J7 Canada",
                ManagerName = "James Epp",
                Phone = "(778) 940-1950",
                Fax = "(778) 940-5793",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 382,
                Name = "KLEO'S PHARMACY REMEDY'SRX",
                Address = "#90B - 1967 Trans Canada Hwy E, Kamloops BC V2C 4A4 Canada",
                ManagerName = "Kleo Dimopoulos",
                Phone = "(778) 765-1444",
                Fax = "(778) 765-1452",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 383,
                Name = "KNIGHTS PHARMACY",
                Address = "330 Main St., Penticton BC V2A 5C3 Canada",
                ManagerName = "Braden Thain",
                Phone = "(250) 492-8080",
                Fax = "(250) 492-6699",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 384,
                Name = "KNIGHTS PHARMACY KELOWNA",
                Address = "103-3320 Richter St, Kelowna BC V1W 4V5 Canada",
                ManagerName = "Brett Gehrke",
                Phone = "(778) 940-4222",
                Fax = "(778) 940-4221",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 385,
                Name = "KORNAK AND HAMM'S PHARMACY",
                Address = "366 Yorston St., Williams Lake BC V2G 4J5 Canada",
                ManagerName = "David Shand",
                Phone = "(250) 398-8177",
                Fax = "(250) 398-7393",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 386,
                Name = "KRIPPS HEALTHCARE RX",
                Address = "5413 West Blvd, Vancouver BC V6M 3W5 Canada",
                ManagerName = "Edward Thorpe",
                Phone = "(604) 687-2564",
                Fax = "(604) 685-9721",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 387,
                Name = "KROLL'S SURREY PHARMACY LTD.",
                Address = "#101 - 9645 137 St, Surrey BC V3T 4G8 Canada",
                ManagerName = "Gurinder Saran",
                Phone = "(604) 581-3636",
                Fax = "(604) 581-3637",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 388,
                Name = "LADYSMITH WHOLEHEALTH PHARMACY AND MORE",
                Address = "L - 17  Gatacre St, Ladysmith BC V9G 1A1 Canada",
                ManagerName = "Shabnam Rana",
                Phone = "(250) 924-1241",
                Fax = "(236) 933-2981",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 389,
                Name = "LAFARGE PHARMACY",
                Address = "106-3056 Glen Dr, Coquitlam BC V3B 0V1 Canada",
                ManagerName = "Mojtaba Rahimi Fard Jahromi",
                Phone = "(604) 474-0769",
                Fax = "(604) 474-0768",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 390,
                Name = "LAKESIDE MEDICINE CENTRE",
                Address = "#112A - 2365 Gordon Drive, Kelowna BC V1W 3C2 Canada",
                ManagerName = "Graham Foster",
                Phone = "(250) 860-3100",
                Fax = "(250) 860-3104",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 391,
                Name = "LAKESIDE PHARMACY",
                Address = "374B Stuart Dr W, Fort St. James BC V0J 1P0 Canada",
                ManagerName = "Wilson Odijie",
                Phone = "(250) 996-7202",
                Fax = "(250) 996-7366",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 392,
                Name = "LALLICARE PHARMACY",
                Address = "1139 Yates Street, Victoria BC V8V 3N2 Canada",
                ManagerName = "Sukhdev Lalli",
                Phone = "(250) 386-5100",
                Fax = "(250) 386-5527",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 393,
                Name = "LAMBERT-KIPP PHARMACY LTD.",
                Address = "PO Box 2919, 1301 - 7th Ave, Invermere BC V0A 1K0 Canada",
                ManagerName = "Laura Kipp",
                Phone = "(250) 342-6612",
                Fax = "(250) 342-6574",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 394,
                Name = "LANCASTER MEDICAL SUPPL. & PRESC. #4",
                Address = "#101 - 13710  94A Ave, Surrey BC V3V 1N1 Canada",
                ManagerName = "Howard Sham",
                Phone = "(604) 582-9181",
                Fax = "(604) 582-9167",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 395,
                Name = "LANCASTER MEDICAL SUPPLIES & PRESCRIPTIONS #1",
                Address = "Unit U1, 601 West Broadway, Vancouver BC V5Z 4C2 Canada",
                ManagerName = "Sarah Ching",
                Phone = "(604) 873-8585",
                Fax = "(604) 873-2381",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 396,
                Name = "LANDMARK PHARMACY",
                Address = "116 - 1631 Dickson Ave, Kelowna BC V1Y 0B5 Canada",
                ManagerName = "Neal King",
                Phone = "(250) 762-5031",
                Fax = "(250) 762-5032",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 397,
                Name = "LANGLEY PHARMACY",
                Address = "101 - 5568 206 St, Langley BC V3A 7T1 Canada",
                ManagerName = "Geoffrey Cridge",
                Phone = "(604) 539-9799",
                Fax = "(604) 539-9798",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 398,
                Name = "LANGLEY TOWNSHIP PHARMACY & COMPOUNDING",
                Address = "Unit 100 - 5068 221A Street, Langley BC V2Y 3V9 Canada",
                ManagerName = "Ranjit Sidhu",
                Phone = "(604) 532-2176",
                Fax = "(604) 342-1131",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 399,
                Name = "LAUREL PRESCRIPTIONS",
                Address = "#102 - 888 West 8th Ave., Vancouver BC V5Z 3Y1 Canada",
                ManagerName = "Ada Mew",
                Phone = "(604) 873-5511",
                Fax = "(604) 873-5581",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 400,
                Name = "LG PHARMACY",
                Address = "4473 Hastings St, Burnaby BC V5C 0L6 Canada",
                ManagerName = "Anandkumar Patel",
                Phone = "(604) 558-2006",
                Fax = "(604) 974-5444",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 401,
                Name = "LIFECARE PHARMACY",
                Address = "#101 - 1106 Austin Avenue, Coquitlam BC V3K 3P5 Canada",
                ManagerName = "Jasmine Patel",
                Phone = "(604) 937-5413",
                Fax = "(604) 937-5414",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 402,
                Name = "LIFECARE PHARMACY #2",
                Address = "#102 - 408 E Columbia Street, New Westminster BC V3L 0K5 Canada",
                ManagerName = "Fatemeh Bahrami",
                Phone = "(604) 515-0413",
                Fax = "(604) 515-0414",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 403,
                Name = "LIFECONNECT PHARMACY",
                Address = "102 - 5967 168 St, Surrey BC V3S 3X5 Canada",
                ManagerName = "Bimal Davda",
                Phone = "(604) 372-0912",
                Fax = "(604) 372-0913",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 404,
                Name = "LIFECONNECT PHARMACY #2",
                Address = "5623 177B Street, Surrey BC V3S 4H9 Canada",
                ManagerName = "Nidhi Gupta",
                Phone = "(604) 245-4050",
                Fax = "(604) 245-4052",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 405,
                Name = "LIFECONNECT PHARMACY #3",
                Address = "170 - 18811 72nd Ave, Surrey BC V4N 6W7 Canada",
                ManagerName = "Jai Gill",
                Phone = "(778) 366-2993",
                Fax = "(778) 366-2997",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 406,
                Name = "LINCOLN PHARMACY & COQUITLAM TRAVEL CLINIC",
                Address = "137 - 3030 Lincoln Ave, Coquitlam BC V3B 6B4 Canada",
                ManagerName = "Phuong Dung Truong",
                Phone = "(604) 464-1033",
                Fax = "(604) 464-1035",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 407,
                Name = "LINCOLN WHOLEHEALTH PHARMACY",
                Address = "6-2168 McCallum Rd, Abbotsford BC V2S 6R6 Canada",
                ManagerName = "Navjot Sran",
                Phone = "(778) 362-4282",
                Fax = "(778) 360-2997",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 408,
                Name = "LITTLE PRAIRIE PHARMACY",
                Address = "PO Box 1330, 5016 50th Ave, Chetwynd BC V0C 1J0 Canada",
                ManagerName = "Arpankumar Patel",
                Phone = "(250) 788-1060",
                Fax = "1-833-909-2074",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 409,
                Name = "LIVING ROOM PHARMACY LTD.",
                Address = "204 - 1530 Cliffe Ave, Courtenay BC V9N 2K4 Canada",
                ManagerName = "Lauren Fournier",
                Phone = "(250) 338-5665",
                Fax = "(250) 338-5855",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 410,
                Name = "LIVING WELL - GUARDIAN PHARMACY + GIFT SHOP",
                Address = "20 - 5725 Vedder Rd, Chilliwack BC V2R 3N4 Canada",
                ManagerName = "Victoria Alipio",
                Phone = "(604) 705-1007",
                Fax = "(604) 705-1009",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 411,
                Name = "LOBLAW PHARMACY #1517",
                Address = "350 Marine Dr SE, Vancouver BC V5X 2S5 Canada",
                ManagerName = "Michael Won",
                Phone = "(604) 322-3706",
                Fax = "(604) 322-7321",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 412,
                Name = "LOBLAW PHARMACY #1518",
                Address = "1105 Eaton Centre, 4700 Kingsway, Burnaby BC V5H 4M1 Canada",
                ManagerName = "Amy Lu",
                Phone = "(604) 439-4404",
                Fax = "(604) 439-4466",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 413,
                Name = "LOBLAW PHARMACY #1519",
                Address = "152-1301 Lougheed Highway, Coquitlam BC V3K 6P9 Canada",
                ManagerName = "Saba Mahmoudi",
                Phone = "(604) 520-8304",
                Fax = "(604) 520-8348",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 414,
                Name = "LOBLAW PHARMACY #1520",
                Address = "3185 Grandview Hwy., Vancouver BC V5M 2E9 Canada",
                ManagerName = "Fargol Ziabakhshdeilami",
                Phone = "(604) 436-6406",
                Fax = "(604) 436-6446",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 415,
                Name = "LOBLAW PHARMACY #1521",
                Address = "7550 King George Blvd, Surrey BC V3W 2T2 Canada",
                ManagerName = "Nafise Amiri",
                Phone = "(604) 599-3704",
                Fax = "(604) 599-3705",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 416,
                Name = "LOBLAW PHARMACY #1522",
                Address = "910 Columbia Street W., Kamloops BC V2C 1L2 Canada",
                ManagerName = "Yesir Al-Sharbati",
                Phone = "(250) 371-6435",
                Fax = "(250) 371-6433",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 417,
                Name = "LOBLAW PHARMACY #1523",
                Address = "45779 Luckakuck Way, Chilliwack BC V2R 4E8 Canada",
                ManagerName = "Areeg Sharafel-Deen",
                Phone = "(604) 824-4235",
                Fax = "(604) 824-4233",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 418,
                Name = "LOBLAW PHARMACY #1524",
                Address = "1424 Island Highway, Campbell River BC V9W 8C9 Canada",
                ManagerName = "George Zigah",
                Phone = "(250) 830-2730",
                Fax = "(250) 830-2733",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 419,
                Name = "LOBLAW PHARMACY #1525",
                Address = "6435 Metral Dr, Nanaimo BC V9T 2L9 Canada",
                ManagerName = "Kevin Cox",
                Phone = "(250) 390-5735",
                Fax = "(250) 390-5732",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 420,
                Name = "LOBLAW PHARMACY #1526",
                Address = "3000 Lougheed Hwy., Coquitlam BC V3B 1C5 Canada",
                ManagerName = "Amy Allen",
                Phone = "(604) 468-6735",
                Fax = "(604) 468-6732",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 421,
                Name = "LOBLAW PHARMACY #1527",
                Address = "835 Langford Pky, Victoria BC V9B 4V5 Canada",
                ManagerName = "Daniel Hauser",
                Phone = "(250) 391-3135",
                Fax = "(250) 391-3133",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 422,
                Name = "LOBLAW PHARMACY #1528",
                Address = "757 Ryan Road, Courtenay BC V9N 3R6 Canada",
                ManagerName = "Mohsin Navsariwala",
                Phone = "(250) 334-6935",
                Fax = "(250) 334-6940",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 423,
                Name = "LOBLAW PHARMACY #1531",
                Address = "5001 Anderson Way, Vernon BC V1T 9V1 Canada",
                ManagerName = "Chiamaka Amobi",
                Phone = "(250) 550-2335",
                Fax = "(250) 550-2334",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 424,
                Name = "LOBLAW PHARMACY #1532",
                Address = "100 - 2210 Main St, Penticton BC V2A 5H8 Canada",
                ManagerName = "Julie Traballo",
                Phone = "(250) 487-7715",
                Fax = "(250) 487-7711",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 425,
                Name = "LOBLAW PHARMACY #1551",
                Address = "2332 160 St, Surrey BC V3Z 0R5 Canada",
                ManagerName = "Prabh Kanwal Sahota",
                Phone = "(778) 545-0478",
                Fax = "(778) 545-0480",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 426,
                Name = "LOBLAW PHARMACY #1552",
                Address = "3020 Louie Dr, Westbank BC V4T 3E1 Canada",
                ManagerName = "Kathleen Thurs",
                Phone = "(250) 707-7015",
                Fax = "(250) 707-7011",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 427,
                Name = "LOBLAW PHARMACY #1553",
                Address = "2100 - 17th St N, Cranbrook BC V1C 7J1 Canada",
                ManagerName = "Jessica Pyrch",
                Phone = "(250) 420-2135",
                Fax = "(250) 420-2132",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 428,
                Name = "LOBLAW PHARMACY #1554",
                Address = "8195 120 St, Delta BC V4C 6P7 Canada",
                ManagerName = "Lutf Bajwa",
                Phone = "(604) 592-5235",
                Fax = "(604) 592-5232",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 429,
                Name = "LOBLAW PHARMACY #1555",
                Address = "201 - 19800 Lougheed Hwy, Pitt Meadows BC V3Y 2W1 Canada",
                ManagerName = "Mona Babaei Jadidi",
                Phone = "(604) 460-4335",
                Fax = "(604) 460-4333",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 430,
                Name = "LOBLAW PHARMACY #1556",
                Address = "14650 - 104 Ave, Surrey BC V3R 1M3 Canada",
                ManagerName = "Tommy Fong",
                Phone = "(604) 587-8535",
                Fax = "(604) 587-8532",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 431,
                Name = "LOBLAW PHARMACY #1557",
                Address = "4651 No. 3 Road, Richmond BC V6X 2C4 Canada",
                ManagerName = "Morenikeji Odukoya",
                Phone = "(604) 233-2430",
                Fax = "(604) 233-2432",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 432,
                Name = "LOBLAW PHARMACY #1558",
                Address = "2855 Gladwin Rd, Abbotsford BC V2T 6Y4 Canada",
                ManagerName = "Parminder Kullar",
                Phone = "(604) 557-5235",
                Fax = "(604) 557-5232",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 433,
                Name = "LOBLAW PHARMACY #1559",
                Address = "32136 Lougheed Highway, Mission BC V2V 1A4 Canada",
                ManagerName = "Hineshkumar Patel",
                Phone = "(604) 820-6430",
                Fax = "(604) 820-6433",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 434,
                Name = "LOBLAW PHARMACY #1560",
                Address = "333 Seymour Blvd., North Vancouver BC V7J 2J4 Canada",
                ManagerName = "Satvir Gill",
                Phone = "(604) 904-5535",
                Fax = "(604) 904-5533",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 435,
                Name = "LOBLAW PHARMACY #1561",
                Address = "19851 Willowbrook Dr, Langley BC V2Y 1A7 Canada",
                ManagerName = "Yifei Zhang",
                Phone = "(604) 532-5430",
                Fax = "(604) 532-5424",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 436,
                Name = "LOBLAW PHARMACY #1562",
                Address = "2155 Ferry Ave, Prince George BC V2N 5E8 Canada",
                ManagerName = "Rhea Everatt",
                Phone = "(250) 960-1335",
                Fax = "(250) 960-1344",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 437,
                Name = "LOBLAW PHARMACY #1563",
                Address = "291 Cowichan Way, Duncan BC V9L 6P5 Canada",
                ManagerName = "Jagjit Mann",
                Phone = "(250) 746-0535",
                Fax = "(250) 746-0540",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 438,
                Name = "LOBLAW PHARMACY #1564",
                Address = "2280 Baron Rd, Kelowna BC V1X 7W3 Canada",
                ManagerName = "Hailey Frame",
                Phone = "(250) 717-2535",
                Fax = "(250) 717-2533",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 439,
                Name = "LOBLAW PHARMACY #4369",
                Address = "3455 Johnston Rd, Port Alberni BC V9Y 8K1 Canada",
                ManagerName = "Shyama Kunnathodi",
                Phone = "(250) 723-1624",
                Fax = "(250) 723-1787",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 440,
                Name = "LOBLAW PHARMACY #4382",
                Address = "215C Port Augusta St, Comox BC V9M 3M9 Canada",
                ManagerName = "Shafaq Jaarah",
                Phone = "(250) 339-6626",
                Fax = "(250) 339-5593",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 441,
                Name = "LOBLAW PHARMACY #4399",
                Address = "Unit 1 - 9831 98A Ave, Fort St. John BC V1J 1S3 Canada",
                ManagerName = "Gordon Lee",
                Phone = "(250) 785-2547",
                Fax = "(250) 785-5351",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 442,
                Name = "LOBLAW PHARMACY #4534",
                Address = "142 - 8100 Rock Island Hwy., Trail BC V1R 4N7 Canada",
                ManagerName = "Jennifer Priddy",
                Phone = "(250) 368-8544",
                Fax = "(250) 368-8093",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 443,
                Name = "LOBLAW PHARMACY #4590",
                Address = "1650 Lonsdale Ave, North Vancouver BC V7M 2J3 Canada",
                ManagerName = "Hee Sun Chung",
                Phone = "(604) 983-3332",
                Fax = "(604) 983-3392",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 444,
                Name = "LOBLAW PHARMACY #4612",
                Address = "1792A 9th Ave, Fernie BC V0B 1M0 Canada",
                ManagerName = "Faith Navales",
                Phone = "(250) 423-3264",
                Fax = "(250) 423-8074",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 445,
                Name = "LOBLAW PHARMACY #4613",
                Address = "49 - 700 Tranquille Rd, Kamloops BC V2B 3H9 Canada",
                ManagerName = "Nicole Sherwood",
                Phone = "(250) 312-3326",
                Fax = "(250) 312-3324",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 446,
                Name = "LOBLAW PHARMACY #4614",
                Address = "127 - 1835 Gordon Drive, Kelowna BC V1Y 3H4 Canada",
                ManagerName = "Ranka Krunic",
                Phone = "(250) 861-1525",
                Fax = "(250) 861-3941",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 447,
                Name = "LOBLAW PHARMACY #4616",
                Address = "18765 Fraser Hwy, Surrey BC V3S 7Y3 Canada",
                ManagerName = "Dina Hana",
                Phone = "(604) 576-3126",
                Fax = "(604) 576-6844",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 448,
                Name = "LOBLAW PHARMACY #4617",
                Address = "Champlain Mall, 7190 Kerr Street, Vancouver BC V5S 4W2 Canada",
                ManagerName = "Grace Lo",
                Phone = "(604) 430-3381",
                Fax = "(604) 433-5347",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 449,
                Name = "LOBLAW PHARMACY #4627",
                Address = "5530 Sunshine Coast Hwy, Sechelt BC V0N 3A0 Canada",
                ManagerName = "Maurice Laycock",
                Phone = "(604) 740-5765",
                Fax = "(604) 740-5743",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 450,
                Name = "LOBLAW PHARMACY #4907",
                Address = "2110 Ryley Avenue, Vanderhoof BC V0J 3A0 Canada",
                ManagerName = "Earle Baggaley",
                Phone = "(250) 567-6005",
                Fax = "(250) 567-6009",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 451,
                Name = "LOBLAW PHARMACY #4979",
                Address = "310 Broadway St W, Vancouver BC V5Y 1R2 Canada",
                ManagerName = "Nancy Ly",
                Phone = "(604) 708-8084",
                Fax = "(604) 708-8663",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 452,
                Name = "LOBLAW PHARMACY #4980",
                Address = "22427 Dewdney Trunk Road, Maple Ridge BC V2X 7A7 Canada",
                ManagerName = "Pariya Bolukinaseri",
                Phone = "(604) 467-0753",
                Fax = "(604) 467-2981",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 453,
                Name = "LOBLAW PHARMACY #4981",
                Address = "2-1900 Garibaldi Way, Garibaldi Highlands BC V0N 1T0 Canada",
                ManagerName = "Jyoti Adhikary",
                Phone = "(604) 898-6818",
                Fax = "(604) 898-6821",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 454,
                Name = "LOBLAW PHARMACY #4983",
                Address = "2335 Maple Drive E, Quesnel BC V2J 7J6 Canada",
                ManagerName = "Hazem Hussein",
                Phone = "(250) 747-2812",
                Fax = "(250) 747-2879",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 455,
                Name = "LOBLAW PHARMACY #4985",
                Address = "1501 Cook Street, Creston BC V0B 1G0 Canada",
                ManagerName = "Sandra Adegoke",
                Phone = "(250) 402-6025",
                Fax = "(250) 402-6029",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 456,
                Name = "LOBLAW PHARMACY #6708",
                Address = "846 Viewfield Rd, Esquimalt BC V9A 4V1 Canada",
                ManagerName = "Faeza Haj-Ibrahim",
                Phone = "(250) 381-8266",
                Fax = "(250) 953-0008",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 457,
                Name = "LOBLAW PHARMACY #6732",
                Address = "2501 - 34th Street, Vernon BC V1T 9S3 Canada",
                ManagerName = "Harmandeep Harmandeep Kaur",
                Phone = "(250) 260-4558",
                Fax = "(250) 260-4562",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 458,
                Name = "LOCK'S PRESCRIPTION PHARMACY LTD.",
                Address = "Medical Dental Centre C/o Lock's Phcy, 9181 Main St, Chilliwack BC V2P 4M9 Canada",
                ManagerName = "Christopher Awrey",
                Phone = "(604) 795-9488",
                Fax = "(604) 792-0482",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 459,
                Name = "LONDON DRUGS #  2",
                Address = "710 Granville St, Vancouver BC V6Z 1E4 Canada",
                ManagerName = "Tiffany Ho",
                Phone = "(604) 685-5292",
                Fax = "(604) 685-5819",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 460,
                Name = "LONDON DRUGS #  3 - NEW WESTMINSTER",
                Address = "Westminster Centre, #100 - 555 Sixth St, New Westminster BC V3L 5H1 Canada",
                ManagerName = "Shirazali Thobani",
                Phone = "(604) 524-1121",
                Fax = "(604) 520-5417",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 461,
                Name = "LONDON DRUGS #  4 - BROADWAY",
                Address = "525 Broadway W, Vancouver BC V5Z 1E6 Canada",
                ManagerName = "Cally Tam",
                Phone = "(604) 872-5177",
                Fax = "(604) 872-5207",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 462,
                Name = "LONDON DRUGS #  5 - NORTH VANCOUVER",
                Address = "2032 Lonsdale Ave., North Vancouver BC V7M 2K5 Canada",
                ManagerName = "Kathryn Leong",
                Phone = "(604) 980-3661",
                Fax = "(604) 980-6791",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 463,
                Name = "LONDON DRUGS #  6 - BURNABY",
                Address = "4970 Kingsway, Burnaby BC V5H 2E2 Canada",
                ManagerName = "Chaoyang Duan",
                Phone = "(604) 437-9621",
                Fax = "(604) 435-0996",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 464,
                Name = "LONDON DRUGS #  7 - HASTINGS",
                Address = "2696 E. Hastings St., Vancouver BC V5K 1Z6 Canada",
                ManagerName = "Wally Lew",
                Phone = "(604) 253-4484",
                Fax = "(604) 251-5401",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 465,
                Name = "LONDON DRUGS #  8 - SURREY",
                Address = "Unit A, 10348 King George Blvd, Surrey BC V3T 2W5 Canada",
                ManagerName = "Lee Chen",
                Phone = "(604) 584-7300",
                Fax = "(604) 581-6771",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 466,
                Name = "LONDON DRUGS #  9 - GUILDFORD",
                Address = "2300 10355 152nd St, Surrey BC V3R 7B9 Canada",
                ManagerName = "Patrick Law",
                Phone = "(604) 588-7881",
                Fax = "(604) 588-7347",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 467,
                Name = "LONDON DRUGS #10",
                Address = "2091 West 42nd Ave, Vancouver BC V6M 2B4 Canada",
                ManagerName = "Sophia Chau",
                Phone = "(604) 263-1811",
                Fax = "(604) 261-0297",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 468,
                Name = "LONDON DRUGS #11 - RICHMOND",
                Address = "London Plaza, 5971 No. 3 Road, Richmond BC V6X 2E3 Canada",
                ManagerName = "Danny Tam",
                Phone = "(604) 278-4521",
                Fax = "(604) 278-4898",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 469,
                Name = "LONDON DRUGS #12 - KELOWNA",
                Address = "#400 - 1950 Harvey Ave., Kelowna BC V1Y 8J8 Canada",
                ManagerName = "Zachariah Stevens",
                Phone = "(250) 860-2232",
                Fax = "(250) 860-3167",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 470,
                Name = "LONDON DRUGS #14",
                Address = "#127 - 3995 Quadra St., Victoria BC V8X 1J8 Canada",
                ManagerName = "Oscar Ho",
                Phone = "(250) 727-2271",
                Fax = "(250) 479-7429",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 471,
                Name = "LONDON DRUGS #15",
                Address = "Unit 1030 Coquitlam Centre, 2929 Barnet Hwy., Coquitlam BC V3B 5R5 Canada",
                ManagerName = "Kai Lin Yang",
                Phone = "(604) 464-3322",
                Fax = "(604) 464-4376",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 472,
                Name = "LONDON DRUGS #16 - CLEARBROOK",
                Address = "3-32900 South Fraser Way, Abbotsford BC V2S 5A1 Canada",
                ManagerName = "Amandeep Rai",
                Phone = "(604) 853-6811",
                Fax = "(604) 853-7369",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 473,
                Name = "LONDON DRUGS #17",
                Address = "7303 - 120th Street, Delta BC V4C 6P5 Canada",
                ManagerName = "Amandeep Purewal",
                Phone = "(604) 591-9544",
                Fax = "(604) 591-6852",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 474,
                Name = "LONDON DRUGS #18 - LANGLEY",
                Address = "Unit B1 - 20202  66 Ave, Langley BC V2Y 1P3 Canada",
                ManagerName = "Melissa Meerkerk",
                Phone = "(604) 533-4631",
                Fax = "(604) 533-2039",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 475,
                Name = "LONDON DRUGS #19",
                Address = "1187 Robson St., Vancouver BC V6E 1B5 Canada",
                ManagerName = "Wynne Leong",
                Phone = "(604) 669-7374",
                Fax = "(604) 669-7341",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 476,
                Name = "LONDON DRUGS #25",
                Address = "Lougheed Mall, 101 - 9855 Austin Rd, Burnaby BC V3J 1N4 Canada",
                ManagerName = "John Wong",
                Phone = "(604) 444-2222",
                Fax = "(604) 444-9988",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 477,
                Name = "LONDON DRUGS #29",
                Address = "#201 - 911 Yates St., Victoria BC V8V 3M4 Canada",
                ManagerName = "Rene Leung",
                Phone = "(250) 381-1113",
                Fax = "(250) 361-9316",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 478,
                Name = "LONDON DRUGS #35",
                Address = "Lansdowne Village, 216 - 450 Lansdowne St, Kamloops BC V2C 1Y3 Canada",
                ManagerName = "Michael Staples",
                Phone = "(250) 372-3445",
                Fax = "(250) 372-3416",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 479,
                Name = "LONDON DRUGS #36",
                Address = "#2 - 650 Terminal Ave, Nanaimo BC V9R 5E2 Canada",
                ManagerName = "Heather Groome",
                Phone = "(250) 753-4433",
                Fax = "(250) 753-4286",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 480,
                Name = "LONDON DRUGS #37",
                Address = "Trenant Park Mall (48th), 5237 Ladner Trunk Road, Delta BC V4K 1W4 Canada",
                ManagerName = "Ka Chi Li",
                Phone = "(604) 946-5642",
                Fax = "(604) 946-8723",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 481,
                Name = "LONDON DRUGS #39",
                Address = "#700 - 4400 - 32nd St., Vernon BC V1T 9H2 Canada",
                ManagerName = "Chris Szeman",
                Phone = "(250) 549-2888",
                Fax = "(250) 558-4895",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 482,
                Name = "LONDON DRUGS #41 - CHILLIWACK",
                Address = "Cottonwood Mall, #21 - 45585 Luckakuck Way, Chilliwack BC V2R 1A1 Canada",
                ManagerName = "Baljinder Cheema",
                Phone = "(604) 858-8347",
                Fax = "(604) 858-0382",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 483,
                Name = "LONDON DRUGS #42 - PENINSULA VILLAGE",
                Address = "#100 - 15355 - 24 Ave, Surrey BC V4A 2H9 Canada",
                ManagerName = "Breanne Dhaliwal",
                Phone = "(604) 535-3281",
                Fax = "(604) 535-5402",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 484,
                Name = "LONDON DRUGS #44 - PARK ROYAL",
                Address = "875 Park Royal North, West Vancouver BC V7T 1H9 Canada",
                ManagerName = "Lisa Stevens",
                Phone = "(604) 926-9616",
                Fax = "(604) 926-4519",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 485,
                Name = "LONDON DRUGS #46 - COLWOOD",
                Address = "1907 Sooke Rd, Victoria BC V9B 1V8 Canada",
                ManagerName = "Kari Jacobsen",
                Phone = "(250) 474-6657",
                Fax = "(250) 391-0275",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 486,
                Name = "LONDON DRUGS #47 - MAPLE RIDGE",
                Address = "#101 - 22709 Lougheed Hwy, Maple Ridge BC V2X 2V5 Canada",
                ManagerName = "Michael Yee",
                Phone = "(604) 463-0991",
                Fax = "(604) 463-1522",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 487,
                Name = "LONDON DRUGS #50 - DAVIE ST.",
                Address = "1650 Davie Street, Vancouver BC V6G 1V9 Canada",
                ManagerName = "Brian Libunao",
                Phone = "(604) 669-2884",
                Fax = "(604) 669-2244",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 488,
                Name = "LONDON DRUGS #51 - PRINCE GEORGE",
                Address = "Parkwood Place Mall, #196 - 1600 - 15th Avenue, Prince George BC V2L 3X3 Canada",
                ManagerName = "Gilbert Kim",
                Phone = "(250) 561-1118",
                Fax = "(250) 561-1050",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 489,
                Name = "LONDON DRUGS #52 - IRONWOOD PLAZA",
                Address = "#3200 - 11666 Steveston Hwy., Richmond BC V7A 5J3 Canada",
                ManagerName = "Edwin Kwong",
                Phone = "(604) 448-5468",
                Fax = "(604) 448-9547",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 490,
                Name = "LONDON DRUGS #53 - VICTORIA DRIVE",
                Address = "5639 Victoria Drive, Vancouver BC V5P 3W2 Canada",
                ManagerName = "Sean Yoon",
                Phone = "(604) 322-6050",
                Fax = "(604) 322-6549",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 491,
                Name = "LONDON DRUGS #54 - TILLICUM ROAD",
                Address = "2 - 3170 Tillicum Road, Victoria BC V9A 7C5 Canada",
                ManagerName = "Kathryn Kroeker",
                Phone = "(250) 360-0296",
                Fax = "(250) 360-1629",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 492,
                Name = "LONDON DRUGS #55 - MISSION",
                Address = "#200 - 32555 London Ave, Mission BC V2V 6M7 Canada",
                ManagerName = "Roger Nandan",
                Phone = "(604) 820-8059",
                Fax = "(604) 820-9628",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 493,
                Name = "LONDON DRUGS #56",
                Address = "Brentwood Mall, 1116 - 1920 Willingdon Ave, Burnaby BC V5C 0K3 Canada",
                ManagerName = "Anita Fong",
                Phone = "(604) 570-0252",
                Fax = "(604) 570-0061",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 494,
                Name = "LONDON DRUGS #61 - GIBSONS",
                Address = "Sunnycrest Mall, #1 - 900 Gibsons Way, Gibsons BC V0N 1V7 Canada",
                ManagerName = "Atsushi Sato",
                Phone = "(604) 886-5710",
                Fax = "(604) 886-5713",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 495,
                Name = "LONDON DRUGS #67 - COURTENAY",
                Address = "Driftwood Mall, 4000 - 2751 Cliffe Ave, Courtenay BC V9N 2L8 Canada",
                ManagerName = "Tianyu He",
                Phone = "(250) 703-2398",
                Fax = "(250) 703-2825",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 496,
                Name = "LONDON DRUGS #70 - PENTICTON",
                Address = "Cherry Lane Shopping Centre, 165 - 2111 Main St, Penticton BC V2A 6W6 Canada",
                ManagerName = "Robert Winter",
                Phone = "(250) 487-3340",
                Fax = "(250) 492-4729",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 497,
                Name = "LONDON DRUGS #71 - MARINE & BYRNE",
                Address = "7280 Market Crossing, Burnaby BC V5J 0A2 Canada",
                ManagerName = "Youngwoo Kim",
                Phone = "(604) 412-4171",
                Fax = "(604) 412-4181",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 498,
                Name = "LONDON DRUGS #72 - RUTHERFORD MALL",
                Address = "175 - 4750 Rutherford Rd, Nanaimo BC V9T 4K6 Canada",
                ManagerName = "Shawn Dhaliwal",
                Phone = "(250) 760-2031",
                Fax = "(250) 760-2026",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 499,
                Name = "LONDON DRUGS #73",
                Address = "Mariner Square, #260 - 1400 Dogwood St, Campbell River BC V9W 3A6 Canada",
                ManagerName = "Tiffany Kuok",
                Phone = "(250) 286-7900",
                Fax = "(250) 286-4799",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 500,
                Name = "LONDON DRUGS #74",
                Address = "2230 Broadway W, Vancouver BC V6K 2E3 Canada",
                ManagerName = "Ryan Tse",
                Phone = "(604) 742-6000",
                Fax = "(604) 742-1843",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 501,
                Name = "LONDON DRUGS #75",
                Address = "821 - 17685 64 Ave, Surrey BC V3S 1Z2 Canada",
                ManagerName = "Oi Chow",
                Phone = "(604) 575-5880",
                Fax = "(604) 575-4630",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 502,
                Name = "LONDON DRUGS #76",
                Address = "2151 Louie Dr, Westbank BC V4T 3E6 Canada",
                ManagerName = "James Plaetzer",
                Phone = "(250) 707-2360",
                Fax = "(250) 768-5892",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 503,
                Name = "LONDON DRUGS #77",
                Address = "Duncan Village, 119 Trans Canada Hwy, Duncan BC V9L 3P8 Canada",
                ManagerName = "Shirley Woyke",
                Phone = "(250) 701-6220",
                Fax = "(250) 709-9634",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 504,
                Name = "LONDON DRUGS #80",
                Address = "Garibaldi Village, Unit G - 40282 Glenalder Pl, Squamish BC V8B 0G2 Canada",
                ManagerName = "Lukhvinder Lalli",
                Phone = "(604) 898-8860",
                Fax = "(604) 898-8250",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 505,
                Name = "LONDON DRUGS #81",
                Address = "#130 - 15850 26 Ave, Surrey BC V3Z 2N6 Canada",
                ManagerName = "Shelley Wu",
                Phone = "(778) 545-5380",
                Fax = "(604) 531-7022",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 506,
                Name = "LONDON DRUGS #82",
                Address = "1622 Salt St, Vancouver BC V5Y 0E4 Canada",
                ManagerName = "Michael Chan",
                Phone = "(604) 707-2030",
                Fax = "(604) 872-8690",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 507,
                Name = "LONDON DRUGS #88",
                Address = "4588 Dunbar St, Vancouver BC V6S 2G6 Canada",
                ManagerName = "Gianni DelNegro",
                Phone = "778-372-5272",
                Fax = "(604) 267-7712",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 508,
                Name = "LONDON DRUGS #90",
                Address = "201 - 1431 Continental St, Vancouver BC V6Z 0G3 Canada",
                ManagerName = "Donald Blyth",
                Phone = "(778) 309-1413",
                Fax = "(778) 372-3765",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 509,
                Name = "LONDON DRUGS LIMITED",
                Address = "12831 Horseshoe Pl, Richmond BC V7A 4X5 Canada",
                ManagerName = "Alvin Lau",
                Phone = "(604) 448-3999",
                Fax = "(604) 272-3751",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 510,
                Name = "LONGEVITY COMPOUNDING PHARMACY",
                Address = "711 Columbia St, New Westminster BC V3M 1B2 Canada",
                ManagerName = "Amandeep Grewal",
                Phone = "(604) 544-7760",
                Fax = "(604) 544-7761",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 511,
                Name = "LONSDALE & 3RD PHARMACY",
                Address = "105 3rd St E, North Vancouver BC V7M 2G1 Canada",
                ManagerName = "Shideh Shadfar",
                Phone = "(604) 971-5499",
                Fax = "(604) 971-5498",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 512,
                Name = "LONSDALE PHARMACY",
                Address = "1531 Lonsdale Ave, North Vancouver BC V7M 2J2 Canada",
                ManagerName = "Lisa Milne",
                Phone = "(604) 985-1901",
                Fax = "(604) 985-1907",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 513,
                Name = "LONSDALE SQUARE PHARMACY IDA",
                Address = "122 E 21st St, North Vancouver BC V7L 0J2 Canada",
                ManagerName = "Mahin Rahmati",
                Phone = "(236) 481-7673",
                Fax = "(778) 309-6233",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 514,
                Name = "LOW COST WESTERN PHARMACY",
                Address = "535 Main St., Vancouver BC V6A 2V1 Canada",
                ManagerName = "Victor Law",
                Phone = "(604) 689-5555",
                Fax = "(604) 689-5268",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 515,
                Name = "LOW COST WESTERN PHARMACY #2",
                Address = "5579 Victoria Dr, Vancouver BC V5P 3W2 Canada",
                ManagerName = "Kelly Chan",
                Phone = "(604) 322-6588",
                Fax = "(604) 322-6501",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 516,
                Name = "LUMBY I.D.A. PHARMACY",
                Address = "1823 Vernon St, Lumby BC V0E 2G0 Canada",
                ManagerName = "Kyle Brewer",
                Phone = "(250) 547-2324",
                Fax = "(250) 547-9593",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 517,
                Name = "LYNN VALLEY PHARMACY",
                Address = "104 - 1200 Lynn Valley Rd, North Vancouver BC V7J 2A2 Canada",
                ManagerName = "Nasser Kamani",
                Phone = "(604) 960-1187",
                Fax = "(604) 960-1183",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 518,
                Name = "LYTTON PHARMACY",
                Address = "1535 Silo Rd, Lytton BC V0K 1Z0 Canada",
                ManagerName = "Venkateswara Sajja",
                Phone = "(778) 254-5454",
                Fax = "(778) 254-5455",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 519,
                Name = "M & S PHARMACY",
                Address = "CRU130-19979 76 Avenue, Langley BC V2Y 3Y3 Canada",
                ManagerName = "Mohamed Fathy",
                Phone = "(778) 298-5041",
                Fax = "(778) 298-5045",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 520,
                Name = "MACDONALD'S PRESCRIPTIONS #3",
                Address = "2188 W. Broadway, Vancouver BC V6K 2C8 Canada",
                ManagerName = "Ruhee Dhanani",
                Phone = "(604) 738-0733",
                Fax = "(604) 738-5400",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 521,
                Name = "MACDONALD'S PRESCRIPTIONS #4",
                Address = "#130 - 943 W. Broadway, Vancouver BC V5Z 1K3 Canada",
                ManagerName = "Allan Baker",
                Phone = "(604) 734-4311",
                Fax = "(604) 734-4366",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 522,
                Name = "MACDONALD'S PRESCRIPTIONS LTD.",
                Address = "Fairmont Medical Bldg., 746 W. Broadway, Vancouver BC V5Z 1G8 Canada",
                ManagerName = "Mike Athanassakis",
                Phone = "(604) 872-2662",
                Fax = "(604) 876-0242",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 523,
                Name = "MACDONALD'S PRESCRIPTIONS RENAL PHARMACY",
                Address = "2nd Floor - 148 West 6th Ave, Vancouver BC V5Y 1K6 Canada",
                ManagerName = "Patrick Derritt",
                Phone = "(604) 872-4200",
                Fax = "(604) 872-4255",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 524,
                Name = "MACKENZIE PHARMACHOICE",
                Address = "700 Mackenzie Blvd, Mackenzie BC V0J 2C0 Canada",
                ManagerName = "Sheri Ukrainetz",
                Phone = "(250) 997-5460",
                Fax = "(250) 997-5480",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 525,
                Name = "MAGGIE'S PHARMACY",
                Address = "2591 Commercial Dr, Vancouver BC V5N 4C1 Canada",
                ManagerName = "Magdolna Kabok",
                Phone = "(778) 371-8721",
                Fax = "(778) 371-8722",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 526,
                Name = "MAIN PRESCRIPTIONS",
                Address = "506 Main St, Vancouver BC V6A 2T9 Canada",
                ManagerName = "Emmanuel Tse",
                Phone = "(604) 683-6381",
                Fax = "(604) 683-8623",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 527,
                Name = "MAIN STATION PHARMACY",
                Address = "1149 Main St, Vancouver BC V6A 4B6 Canada",
                ManagerName = "Youn Kim",
                Phone = "(604) 662-3883",
                Fax = "(604) 662-3887",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 528,
                Name = "MAINCARE PHARMACY",
                Address = "7297 Main St, Vancouver BC V5X 3J3 Canada",
                ManagerName = "Jaspreet Virdi",
                Phone = "(604) 325-0544",
                Fax = "(604) 325-0574",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 529,
                Name = "MANN'S PRESCRIPTION PHARMACY",
                Address = "325 Jubilee St., Duncan BC V9L 1W9 Canada",
                ManagerName = "Jason Czettisch",
                Phone = "(250) 746-7168",
                Fax = "(250) 746-7169",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 530,
                Name = "MANSHADI PHARMACY",
                Address = "477 St. Paul St, Kamloops BC V2C 2J7 Canada",
                ManagerName = "Laurel Williams",
                Phone = "(250) 372-2223",
                Fax = "(250) 372-2224",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 531,
                Name = "MANSHADI PHARMACY #2",
                Address = "374 Tranquille Rd, Kamloops BC V2B 3G7 Canada",
                ManagerName = "Missaghullah Manshadi",
                Phone = "(250) 434-2526",
                Fax = "(250) 434-2527",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 532,
                Name = "MAPLE MEADOWS PHARMACY",
                Address = "102 - 11893 227 St, Maple Ridge BC V2X 6H9 Canada",
                ManagerName = "James Kim",
                Phone = "(604) 380-4345",
                Fax = "(604) 380-4346",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 533,
                Name = "MARINE DRIVE PHARMACY",
                Address = "103 - 1061 Marine Drive, North Vancouver BC V7P 1S6 Canada",
                ManagerName = "Niloufar Aliramaji",
                Phone = "(778) 340-9308",
                Fax = "(833) 696-0783",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 534,
                Name = "MARK'S MARINE PHARMACY",
                Address = "239 Marine Dr SE, Vancouver BC V5X 2S4 Canada",
                ManagerName = "Robert Rosenblatt",
                Phone = "(604) 325-9265",
                Fax = "(604) 325-9805",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 535,
                Name = "MARKS PHARMACY",
                Address = "#101 - 8035 - 120th Street, Delta BC V4C 6P8 Canada",
                ManagerName = "Alan Glasser",
                Phone = "(604) 596-1774",
                Fax = "(604) 596-8334",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 536,
                Name = "MARMAR PHARMACY",
                Address = "#212 - 1200 Burrard St, Vancouver BC V6Z 2C7 Canada",
                ManagerName = "Kati Chan",
                Phone = "(604) 605-0211",
                Fax = "(604) 602-0210",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 537,
                Name = "MARY'S PHARMACY",
                Address = "#201B - 1194 Lansdowne Dr, Coquitlam BC V3E 1J7 Canada",
                ManagerName = "Hye Won Ahn",
                Phone = "(604) 941-0454",
                Fax = "(604) 941-0421",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 538,
                Name = "MCCALLUM PHARMACY",
                Address = "2481B McCallum Rd, Abbotsford BC V2S 3P8 Canada",
                ManagerName = "Ravleen Sidhu",
                Phone = "(604) 852-3603",
                Fax = "(604) 852-3601",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 539,
                Name = "MCCUE PHARMACY",
                Address = "#100 - 8120 Cook Road, Richmond BC V6Y 1T9 Canada",
                ManagerName = "Peter Cheng",
                Phone = "(604) 278-9601",
                Fax = "(604) 273-5321",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 540,
                Name = "MCKESSON SPECIALTY PHARMACY (BC)",
                Address = "102-3330 192 St, Surrey BC V3Z 1A1 Canada",
                ManagerName = "Julia Zhu",
                Phone = "1-866-246-0095",
                Fax = "1-866-246-7796",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 541,
                Name = "MCKIM CARE PHARMACY",
                Address = "1010 - 8766 McKim Way, Richmond BC V6X 4G4 Canada",
                ManagerName = "Sulaiman Lalji",
                Phone = "(604) 558-4315",
                Fax = "(604) 558-4314",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 542,
                Name = "MEADOWCURE PHARMACY + COFFEE",
                Address = "215-1331 Westhills Dr, Langford BC V9B 0S2 Canada",
                ManagerName = "Slava Lovesar",
                Phone = "(778) 557-0103",
                Fax = "(778) 557-0105",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 543,
                Name = "MEADOWS PHARMACY",
                Address = "12161 Harris Rd, Pitt Meadows BC V3Y 2E9 Canada",
                ManagerName = "Shamsuddin Budhwani",
                Phone = "(604) 460-0541",
                Fax = "(604) 460-0542",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 544,
                Name = "MEDCARE PHARMACY",
                Address = "#107 - 12025 Nordel Way, Surrey BC V3W 1W1 Canada",
                ManagerName = "Sudhir Singh",
                Phone = "(604) 593-1415",
                Fax = "(604) 593-1416",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 545,
                Name = "MEDICA PHARMACY LTD.",
                Address = "10030 King George Blvd, Surrey BC V3T 2W4 Canada",
                ManagerName = "Rupinder Kahlon",
                Phone = "(604) 496-1903",
                Fax = "(604) 496-1909",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 546,
                Name = "MEDICAL CENTRE PHARMACY",
                Address = "1722 Davie St, Vancouver BC V6G 1W2 Canada",
                ManagerName = "Melody Shirvan",
                Phone = "(604) 682-4321",
                Fax = "(604) 568-2023",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 547,
                Name = "MEDICAL CENTRE PRESCRIPTIONS",
                Address = "10225 King George Blvd, Surrey BC V3T 2W6 Canada",
                ManagerName = "Asif Walji",
                Phone = "(604) 581-2411",
                Fax = "(604) 589-2020",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 548,
                Name = "MEDICAL TOWER DRUGS LTD.",
                Address = "#180 - 2151 McCallum Rd, Abbotsford BC V2S 3N8 Canada",
                ManagerName = "Gordon Rowe",
                Phone = "(604) 859-7651",
                Fax = "(604) 859-7651",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 549,
                Name = "MEDICARE PHARMACY",
                Address = "Unit 190 - 7031 Westminster Hwy, Richmond BC V6X 1A3 Canada",
                ManagerName = "Sunny Chau",
                Phone = "(604) 278-7133",
                Fax = "(604) 278-7135",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 550,
                Name = "MEDICINE GURU",
                Address = "9180 120 St, Surrey BC V3V 4B5 Canada",
                ManagerName = "Mayankkumar Patel",
                Phone = "(604) 585-4878",
                Fax = "(604) 585-4876",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 551,
                Name = "MEDICINE SHOPPE #116",
                Address = "#6 - 3195 Granville St., Vancouver BC V6H 3K2 Canada",
                ManagerName = "Tristan Sze",
                Phone = "(604) 732-0777",
                Fax = "(604) 732-1199",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 552,
                Name = "MEDICINE SHOPPE #142 (THE)",
                Address = "413 East Columbia St., New Westminster BC V3L 3X3 Canada",
                ManagerName = "Sanjeev Saraf",
                Phone = "(604) 521-9313",
                Fax = "(604) 521-9614",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 553,
                Name = "MEDICINE SHOPPE #148",
                Address = "#9 - 31205 MacLure Rd, Abbotsford BC V2T 5E5 Canada",
                ManagerName = "Satwinder Maan",
                Phone = "(604) 854-5800",
                Fax = "(604) 854-5803",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 554,
                Name = "MEDICINE SHOPPE #156",
                Address = "Fleetwood Professional Centre, #104 - 16088 - 84th Ave, Surrey BC V4N 0V9 Canada",
                ManagerName = "Jayeshkumar Khunt",
                Phone = "(604) 507-0190",
                Fax = "(604) 507-0192",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 555,
                Name = "MEDICINE SHOPPE #168",
                Address = "102-192 Island Highway W, Parksville BC V9P 2H1 Canada",
                ManagerName = "Melquiades Azcarraga",
                Phone = "(250) 248-6695",
                Fax = "(250) 248-8991",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 556,
                Name = "MEDICINE SHOPPE #175 (THE)",
                Address = "#6 - 4330 Sunshine Coast Hwy., Sechelt BC V7Z 0A7 Canada",
                ManagerName = "Ken Grunenberg",
                Phone = "(604) 740-5813",
                Fax = "(604) 740-5814",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 557,
                Name = "MEDICINE SHOPPE #195",
                Address = "#104 - 1964 Fort St., Victoria BC V8R 6R3 Canada",
                ManagerName = "Rania Gomaa",
                Phone = "(250) 595-1323",
                Fax = "(250) 595-1325",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 558,
                Name = "MEDICINE SHOPPE #221",
                Address = "#101 - 777 West Broadway, Vancouver BC V5Z 4J7 Canada",
                ManagerName = "Peyman Haghighat",
                Phone = "(604) 675-6300",
                Fax = "(604) 675-6320",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 559,
                Name = "MEDICINE SHOPPE #253",
                Address = "4833 Southgate Rd, Port Alberni BC V9Y 5K5 Canada",
                ManagerName = "Lawrence Johannessen",
                Phone = "(250) 723-4940",
                Fax = "(250) 723-4924",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 560,
                Name = "MEDICINE SHOPPE #255",
                Address = "#4 - 4071 Shelbourne St, Victoria BC V8N 5Y1 Canada",
                ManagerName = "Amy Coyle",
                Phone = "(250) 477-6112",
                Fax = "(250) 477-6121",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 561,
                Name = "MEDICINE SHOPPE #268",
                Address = "143 Second Ave W, Qualicum Beach BC V9K 2R8 Canada",
                ManagerName = "Brandon Vandal",
                Phone = "(250) 752-6691",
                Fax = "(250) 752-8941",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 562,
                Name = "MEDICINE SHOPPE #292",
                Address = "M41-4277 Kingsway, Burnaby BC V5H 3Z2 Canada",
                ManagerName = "Dorna Sadeghi",
                Phone = "(604) 435-5353",
                Fax = "(604) 435-5358",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 563,
                Name = "MEDICINE SHOPPE #310",
                Address = "11 - 3993 Chatham St, Richmond BC V7E 2Z6 Canada",
                ManagerName = "Steven Chang",
                Phone = "(778) 297-5777",
                Fax = "(778) 297-5778",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 564,
                Name = "MEDICINE SHOPPE #312",
                Address = "130 - 8171 Cook Rd, Richmond BC V6Y 3T8 Canada",
                ManagerName = "Simon Cheng",
                Phone = "(604) 278-3828",
                Fax = "(604) 278-3839",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 565,
                Name = "MEDICINE SHOPPE #325",
                Address = "303 - 5800 Turner Rd, Nanaimo BC V9T 6J4 Canada",
                ManagerName = "Candy Cruz",
                Phone = "(250) 585-0325",
                Fax = "(250) 585-0327",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 566,
                Name = "MEDICINE SHOPPE #333",
                Address = "107 - 3949 Maple Way, Port Alberni BC V9Y 0B2 Canada",
                ManagerName = "James Osborne",
                Phone = "(250) 723-7270",
                Fax = "(250) 723-7271",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 567,
                Name = "MEDICINE SHOPPE #350",
                Address = "33 - 1150 Terminal Ave N, Nanaimo BC V9S 5L6 Canada",
                ManagerName = "Elijah Ssemaluulu",
                Phone = "(250) 591-4933",
                Fax = "(250) 591-4935",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 568,
                Name = "MEDICINE SHOPPE #352",
                Address = "2A - 1834 Cedar Rd, Nanaimo BC V9X 1H9 Canada",
                ManagerName = "Rachel Montejo",
                Phone = "(250) 323-8688",
                Fax = "(250) 323-8689",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 569,
                Name = "MEDICINE SHOPPE #356",
                Address = "D - 2388 McCallum Rd, Abbotsford BC V2S 3P4 Canada",
                ManagerName = "Waeel Ameen",
                Phone = "(604) 776-1000",
                Fax = "(604) 776-1100",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 570,
                Name = "MEDICINE SHOPPE #361",
                Address = "121 - 20353 64 Ave, Langley BC V2Y 1N5 Canada",
                ManagerName = "Fiona Tran",
                Phone = "(604) 510-3140",
                Fax = "(604) 510-3141",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 571,
                Name = "MEDICINE SHOPPE #379",
                Address = "111 - 4871 Joyce Ave, Powell River BC V8A 5P4 Canada",
                ManagerName = "Dirk De Villiers",
                Phone = "(604) 489-5919",
                Fax = "(604) 489-5920",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 572,
                Name = "MEDICINE SHOPPE #395",
                Address = "100 - 3605 31 St, Vernon BC V1T 5J4 Canada",
                ManagerName = "Jodi Cunningham",
                Phone = "(778) 475-1010",
                Fax = "(778) 475-1019",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 573,
                Name = "MEDICINE SHOPPE #402",
                Address = "3982 Hastings St, Burnaby BC V5C 6C1 Canada",
                ManagerName = "Qais Darwish",
                Phone = "(604) 229-8353",
                Fax = "(604) 229-7975",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 574,
                Name = "MEDICINE SHOPPE #409",
                Address = "1146 - 1470 Prairie Ave, Port Coquitlam BC V3B 5M8 Canada",
                ManagerName = "Becky Jiang",
                Phone = "604-554-0950",
                Fax = "604-554-0953",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 575,
                Name = "MEDICINE SHOPPE #414",
                Address = "1 - 2760 Cliffe Avenue, Courtenay BC V9N 2L6 Canada",
                ManagerName = "Janice Harvey",
                Phone = "(250) 338-4790",
                Fax = "(250) 338-4791",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 576,
                Name = "MEDICINE SHOPPE #415",
                Address = "Unit 104 - 2806 Jacklin Road, Victoria BC V9B 5A4 Canada",
                ManagerName = "Harish Sharma",
                Phone = "(250) 391-9367",
                Fax = "(250) 391-9369",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 577,
                Name = "MEDICINE SHOPPE #437",
                Address = "4011 Quadra St Unit 6, Victoria BC V8X 1K1 Canada",
                ManagerName = "Parul Malviya",
                Phone = "(778) 806-1134",
                Fax = "(778) 806-1135",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 578,
                Name = "MEDICINE SHOPPE 427",
                Address = "A-105-3292 Cowichan Lake Rd, Duncan BC V9L 4C3 Canada",
                ManagerName = "Gurneil Parmar",
                Phone = "(250) 597-1314",
                Fax = "(250) 597-1233",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 579,
                Name = "MEDICINE SHOPPE PHARMACY #169",
                Address = "Unit B - 1782 Comox Ave, Comox BC V9M 3M8 Canada",
                ManagerName = "Catherine McCann",
                Phone = "(250) 339-5050",
                Fax = "(250) 339-5040",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 580,
                Name = "MEDICINE SHOPPE PHARMACY #240",
                Address = "#303 - 15988 Fraser Hwy, Surrey BC V4N 0X8 Canada",
                ManagerName = "Dipak Koladiya",
                Phone = "(604) 507-0970",
                Fax = "(604) 507-0971",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 581,
                Name = "MEDICINE SHOPPE PHARMACY #254",
                Address = "Pinetree Village, 53 - 2991 Lougheed Hwy, Coquitlam BC V3B 6J6 Canada",
                ManagerName = "Mohanad Abdul-Ahad",
                Phone = "(604) 468-4711",
                Fax = "(604) 468-4707",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 582,
                Name = "MEDICINE SHOPPE PHARMACY #259",
                Address = "2441A Main St, Westbank BC V4T 1K5 Canada",
                ManagerName = "Paolo Sales",
                Phone = "(250) 707-2952",
                Fax = "(250) 707-2954",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 583,
                Name = "MEDICINE SHOPPE PHARMACY #281",
                Address = "108 - 2210 Main St, Penticton BC V2A 5H8 Canada",
                ManagerName = "Michael Kidd",
                Phone = "(250) 276-3876",
                Fax = "(250) 276-3076",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 584,
                Name = "MEDICINE SHOPPE PHARMACY #321",
                Address = "102A - 1100 Lawrence Ave, Kelowna BC V1Y 6M4 Canada",
                ManagerName = "Bhumi Patel",
                Phone = "(250) 763-5312",
                Fax = "(250) 763-8289",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 585,
                Name = "MEDICINE SHOPPE PHARMACY #332",
                Address = "A - 4186 Departure Bay Rd, Nanaimo BC V9T 4B7 Canada",
                ManagerName = "Kristen Azcarraga",
                Phone = "(250) 760-0073",
                Fax = "(250) 760-0083",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 586,
                Name = "MEDICINE SHOPPE PHARMACY #373",
                Address = "11919 224 St, Maple Ridge BC V2X 6B2 Canada",
                ManagerName = "Dilawar Paul",
                Phone = "(604) 380-1500",
                Fax = "(604) 380-1600",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 587,
                Name = "MEDICINE SHOPPE PHARMACY #406",
                Address = "100 - 847 Bruce Ave, Nanaimo BC V9R 4A1 Canada",
                ManagerName = "Ketan Prajapati",
                Phone = "250-591-4555",
                Fax = "250-591-4557",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 588,
                Name = "MEDIGLEN PHARMACY",
                Address = "102 - 1173 The High St, Coquitlam BC V3B 0B1 Canada",
                ManagerName = "Behnaz Alijani",
                Phone = "(778) 285-8811",
                Fax = "(778) 285-8812",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 589,
                Name = "MEDIMIX PHARMACY",
                Address = "Unit 145 - 19933 88th Ave, Langley BC V2Y 4K5 Canada",
                ManagerName = "Fred Chiang",
                Phone = "(778) 366-3530",
                Fax = "(778) 366-3531",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 590,
                Name = "MEDIPHARM PHARMACY",
                Address = "6447 Fraser St, Vancouver BC V5W 3A6 Canada",
                ManagerName = "Mehdi Majd",
                Phone = "(604) 336-3144",
                Fax = "(604) 336-3145",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 591,
                Name = "MEDIS PHARMACY",
                Address = "#6 - 2755 Lougheed Hwy, Port Coquitlam BC V3B 5Y9 Canada",
                ManagerName = "Maral Sobhanipour",
                Phone = "(604) 944-5544",
                Fax = "(604) 944-5548",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 592,
                Name = "MEDISAVE PHARMACY #07",
                Address = "#104 - 8056 King George Blvd, Surrey BC V3W 5B5 Canada",
                ManagerName = "Gurinder Gill",
                Phone = "(604) 599-5403",
                Fax = "(604) 599-5404",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 593,
                Name = "MEDISAVE PHARMACY #3",
                Address = "#2 - 8181 - 120A Street, Surrey BC V3W 3P2 Canada",
                ManagerName = "Harmander Jandu",
                Phone = "(604) 501-1114",
                Fax = "(604) 501-1914",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 594,
                Name = "MEDISAVE PHARMACY #4",
                Address = "Unit 12 - 6828 128 St, Surrey BC V3W 4C9 Canada",
                ManagerName = "Harshan Grewal",
                Phone = "(604) 501-7719",
                Fax = "(604) 501-7759",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 595,
                Name = "MEDISAVE PHARMACY #6",
                Address = "#125 - 8291 Ackroyd Rd, Richmond BC V6X 3K5 Canada",
                ManagerName = "Pushpinderpal Purba",
                Phone = "(604) 232-0811",
                Fax = "(604) 232-0851",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 596,
                Name = "MEDISYSTEM PHARMACY WEST LIMITED",
                Address = "Unit 1110, Ground Floor, 13560 Maycrest Way, Richmond BC V6V 2W9 Canada",
                ManagerName = "Christopher Faa",
                Phone = "(604) 270-4590",
                Fax = "(604) 270-4594",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 597,
                Name = "MEDLANDIA COMPOUNDING PHARMACY",
                Address = "100 - 20528 Lougheed Hwy, Maple Ridge BC V2X 2P8 Canada",
                ManagerName = "Tina Shafiee",
                Phone = "(604) 465-3375",
                Fax = "(604) 465-3378",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 598,
                Name = "MEDLIFE PHARMACY",
                Address = "65-11900 Haney Pl, Maple Ridge BC V2X 8R9 Canada",
                ManagerName = "Sanaz Shahriari",
                Phone = "(778) 504-4569",
                Fax = "(778) 504-8369",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 599,
                Name = "MEDMIX COMPOUNDING PHARMACY",
                Address = "120-7920 206th St, Langley BC V2Y 3X1 Canada",
                ManagerName = "Jagdeep Johal",
                Phone = "(778) 366-4554",
                Fax = "(604) 909-1787",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 600,
                Name = "MEDNOW PHARMACY",
                Address = "101-4484 Main Street, Vancouver BC V5V 3R3 Canada",
                ManagerName = "Joshua Thomas",
                Phone = "(604) 876-6410",
                Fax = "(604) 875-6808",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 601,
                Name = "MED-X PHARMACY LTD.",
                Address = "7 - 1449 Prairie Ave, Port Coquitlam BC V3B 1S9 Canada",
                ManagerName = "Alykhan Prebtani",
                Phone = "(604) 474-3050",
                Fax = "(604) 474-3051",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 602,
                Name = "MEDZONE METTRA PHARMACY",
                Address = "3680 East Hastings, Vancouver BC V5K 2A9 Canada",
                ManagerName = "Sama Hosseini-Montazeri",
                Phone = "(604) 900-9569",
                Fax = "(604) 901-5901",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 603,
                Name = "MEMORIAL COMPOUNDING PHARMACY",
                Address = "699 Memorial Ave, Qualicum Beach BC V9K 1S8 Canada",
                ManagerName = "Armandeep Gill",
                Phone = "(250) 752-9976",
                Fax = "(250) 752-2499",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 604,
                Name = "METCHOSIN PHARMACY",
                Address = "B - 4480 Happy Valley Rd., Victoria BC V9C 3Z3 Canada",
                ManagerName = "Shady Geris",
                Phone = "(778) 265-0122",
                Fax = "(778) 265-0133",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 605,
                Name = "METROCARE PHARMACY",
                Address = "5375 Lane St, Burnaby BC V5H 0H2 Canada",
                ManagerName = "Andy Nguyen",
                Phone = "(604) 568-1222",
                Fax = "(604) 566-9213",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 606,
                Name = "METROLIFE PHARMACY",
                Address = "Building B 107 - 11018 126A Street, Surrey BC V3V 0G1 Canada",
                ManagerName = "Abid Raja",
                Phone = "(604) 951-2011",
                Fax = "(604) 951-2012",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 607,
                Name = "METTRA PHARMACY DUNCAN",
                Address = "#105 - 15 Canada Ave, Duncan BC V9L 1T3 Canada",
                ManagerName = "Olga Shevchenko",
                Phone = "(250) 748-0104",
                Fax = "(236) 800-7384",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 608,
                Name = "MID-ISLAND PHARMACY",
                Address = "7-162 Harrison Avenue, Parksville BC V9P 2W4 Canada",
                ManagerName = "Luisa Loberiza",
                Phone = "(250) 757-0779",
                Fax = "(236) 935-2982",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 609,
                Name = "MINORU PHARMACY",
                Address = "7380 Westminster Hwy, Richmond BC V6X 1A1 Canada",
                ManagerName = "Alla Shmulevich",
                Phone = "(604) 270-2320",
                Fax = "(604) 270-2327",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 610,
                Name = "MINT HEALTH + DRUGS: CORDOVA BAY",
                Address = "5166 Cordova Bay Rd, Victoria BC V8Y 2K6 Canada",
                ManagerName = "Prabhjot Dhindsa",
                Phone = "(250) 590-6053",
                Fax = "(250) 590-6403",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 611,
                Name = "MINT HEALTH + DRUGS: LATORIA",
                Address = "115 - 611 Brookside Rd, Victoria BC V9C 0C3 Canada",
                ManagerName = "Shannon Sneddon",
                Phone = "(250) 590-7012",
                Fax = "(250) 590-7014",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 612,
                Name = "MINT PHARMACY: BOWSER",
                Address = "PO Box 8, 112 - 6996 Island Hwy W, Bowser BC V0R 1G0 Canada",
                ManagerName = "Joseph Geneau",
                Phone = "(250) 757-8631",
                Fax = "(250) 757-8632",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 613,
                Name = "MIRACLE PRESCRIPTIONS REMEDY'SRX",
                Address = "1268 Marine Dr, North Vancouver BC V7P 1T2 Canada",
                ManagerName = "Arash Pourzare",
                Phone = "(604) 770-2030",
                Fax = "(604) 770-2035",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 614,
                Name = "MJ'S NATURAL PHARMACY",
                Address = "6255 Victoria Dr, Vancouver BC V5P 3X5 Canada",
                ManagerName = "Frankie Cheung",
                Phone = "(604) 323-1293",
                Fax = "(604) 323-1294",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 615,
                Name = "MJ'S NATURAL PHARMACY #3",
                Address = "Richmond Public Market, 1130 - 8260 Westminster Hwy, Richmond BC V6X 3Y2 Canada",
                ManagerName = "Kenny Chan",
                Phone = "(604) 232-1293",
                Fax = "(604) 232-1296",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 616,
                Name = "MJ'S NATURAL PHARMACY NO. 2",
                Address = "6689 Victoria Dr, Vancouver BC V5P 3Y2 Canada",
                ManagerName = "Joshua Cheung",
                Phone = "(604) 324-1293",
                Fax = "(604) 324-1273",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 617,
                Name = "MODERN PHARMACY",
                Address = "51296 Yale Road, Chilliwack BC V0X 1X0 Canada",
                ManagerName = "Javed Jokhoo",
                Phone = "(778) 704-0474",
                Fax = "(236) 436-2035",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 618,
                Name = "MONTROSE PHARMACY",
                Address = "#104 - 2493 Montrose Ave, Abbotsford BC V2S 0L5 Canada",
                ManagerName = "Kaushal Patel",
                Phone = "(604) 621-0843",
                Fax = "(604) 621-0844",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 619,
                Name = "MORGAN CREEK PHARMACY",
                Address = "105A - 15252 32 Ave, Surrey BC V3Z 0R7 Canada",
                ManagerName = "Robyn Mahil",
                Phone = "(604) 538-6333",
                Fax = "(604) 538-6387",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 620,
                Name = "MOUNT IDA PHARMACY",
                Address = "200 Trans Canada Hwy SW, Salmon Arm BC V1E 1V4 Canada",
                ManagerName = "Regan Ready",
                Phone = "(250) 804-0844",
                Fax = "(250) 804-0899",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 621,
                Name = "MOUNT PLEASANT PHARMACY",
                Address = "93 Kingsway, Vancouver BC V5T 3J1 Canada",
                ManagerName = "Francis Dong",
                Phone = "(604) 872-2039",
                Fax = "(604) 872-2049",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 622,
                Name = "MOUNTAIN & PESTLE PHARMACY",
                Address = "100 Deer Park Ave, Kimberley BC V1A 2J4 Canada",
                ManagerName = "Carley Frasca",
                Phone = "(250) 432-1317",
                Fax = "(236) 528-2056",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 623,
                Name = "MOUNTAIN VIEW PHARMACY (PHARMACHOICE)",
                Address = "Unit B - 111 Dogwood St, Campbell River BC V9W 6B9 Canada",
                ManagerName = "Jean Claude Kouadio",
                Phone = "(250) 914-2233",
                Fax = "(250) 914-2234",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 624,
                Name = "MT. LEHMAN PHARMACY LTD.",
                Address = "110 - 30495 Cardinal Ave, Abbotsford BC V2T 0A5 Canada",
                ManagerName = "Vismay Mehta",
                Phone = "(604) 856-7176",
                Fax = "(604) 856-7178",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 625,
                Name = "MUNRO'S SORRENTO PRESCRIPTIONS",
                Address = "Box 239, 1250 Trans Canada Highway, Sorrento BC V0E 2W0 Canada",
                ManagerName = "Trent Tschirgi",
                Phone = "(250) 675-4411",
                Fax = "(250) 675-4422",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 626,
                Name = "MY PHARMACY",
                Address = "304 - 2537 Beacon Ave, Sidney BC V8L 1Y3 Canada",
                ManagerName = "Chris Lam",
                Phone = "(250) 800-0187",
                Fax = "(250) 984-0881",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 627,
                Name = "NAKUSP PHARMACHOICE",
                Address = "88 Broadway St, Nakusp BC V0G 1R0 Canada",
                ManagerName = "Troy Clark",
                Phone = "(250) 265-2228",
                Fax = "(250) 265-2218",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 628,
                Name = "NANAIMO COMPOUNDING PHARMACY",
                Address = "560 - 2980 Island Hwy N, Nanaimo BC V9T 5V4 Canada",
                ManagerName = "Tariq Ijaz",
                Phone = "(250) 755-6365",
                Fax = "(250) 585-9616",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 629,
                Name = "NANAIMO PHARMACY",
                Address = "1299 Nanaimo St, Vancouver BC V5L 4T5 Canada",
                ManagerName = "Ben Yeung",
                Phone = "(604) 251-1299",
                Fax = "(604) 251-1280",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 630,
                Name = "NANOOSE PHARMACY",
                Address = "4 - 2451 Collins Cres, Nanoose Bay BC V9P 9J9 Canada",
                ManagerName = "Chasz Hodgson",
                Phone = "(250) 468-9921",
                Fax = "(250) 468-9621",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 631,
                Name = "NATURE'S COMPOUNDING PHARMACY",
                Address = "102 - 9103 Glover Rd, Langley BC V1M 0E8 Canada",
                ManagerName = "Majdolene Qasim",
                Phone = "(604) 888-2895",
                Fax = "(604) 888-1862",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 632,
                Name = "NAZ'S PHARMACY #2",
                Address = "113 - 12578 72nd Ave, Surrey BC V3W 2M6 Canada",
                ManagerName = "Nafisa Merali",
                Phone = "(604) 596-3241",
                Fax = "(604) 597-3267",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 633,
                Name = "NAZ'S PHARMACY #3",
                Address = "105 - 9385 120 St, Delta BC V4C 0B5 Canada",
                ManagerName = "Basima Spindari",
                Phone = "(604) 585-1210",
                Fax = "(604) 585-2601",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 634,
                Name = "NAZ'S PHARMACY #4",
                Address = "101 - 12565 88 Ave, Surrey BC V3W 3J7 Canada",
                ManagerName = "Benjamin Chan",
                Phone = "(604) 543-8850",
                Fax = "(604) 543-8857",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 635,
                Name = "NAZ'S PHARMACY NO. 1 LTD.",
                Address = "#108 - 5990 Fraser St., Vancouver BC V5W 2Z7 Canada",
                ManagerName = "Aazil Nazarali",
                Phone = "(604) 323-1268",
                Fax = "(604) 323-1226",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 636,
                Name = "NAZ'S PHARMACY NO. 5",
                Address = "Unit 17 - 15300 105 Ave, Surrey BC V3R 6A7 Canada",
                ManagerName = "Saba Baig",
                Phone = "(604) 634-0303",
                Fax = "(604) 634-0304",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 637,
                Name = "NAZ'S PRESCRIPTION PLUS PHARMACY",
                Address = "6410 Main St., Vancouver BC V5W 2V4 Canada",
                ManagerName = "Christina Wei",
                Phone = "(604) 325-3241",
                Fax = "(604) 325-3276",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 638,
                Name = "NEIGHBOURLY BC CENTRAL FILL SOLUTIONS #1055",
                Address = "3246 Beta Ave, Burnaby BC V5G 4K4 Canada",
                ManagerName = "Nader Khattab",
                Phone = "(236) 326-1010",
                Fax = "(236) 326-1060",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 639,
                Name = "NELSON CLINIC PHARMACY",
                Address = "405 Hendryx Street, Nelson BC V1L 2A6 Canada",
                ManagerName = "Andrew Hoffert",
                Phone = "(250) 352-3121",
                Fax = "(250) 352-2389",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 640,
                Name = "NELSON REMEDY'SRX",
                Address = "737 Baker St, Nelson BC V1L 4J5 Canada",
                ManagerName = "Trevor Sawchuk",
                Phone = "(250) 352-0022",
                Fax = "(250) 352-0033",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 641,
                Name = "NESTER'S PHARMACY LTD.",
                Address = "205-7015 Nesters Rd, Whistler BC V8E 0X1 Canada",
                ManagerName = "Eric Poulin",
                Phone = "(604) 905-0429",
                Fax = "(604) 905-0427",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 642,
                Name = "NESTER'S PHARMACY SFU",
                Address = "9000 University High St, Burnaby BC V5A 0C1 Canada",
                ManagerName = "Edan Hu",
                Phone = "(604) 298-1566",
                Fax = "(604) 298-1535",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 643,
                Name = "NEW ERA PHARMACY",
                Address = "1 - 1589 George St., White Rock BC V4B 0C6 Canada",
                ManagerName = "Jatinder Sidhu",
                Phone = "(778) 357-3474",
                Fax = "(778) 357-3475",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 644,
                Name = "NEW WEST PHARMACHOICE",
                Address = "25 - Eighth Ave, New Westminster BC V3L 1X6 Canada",
                ManagerName = "Todd Verabioff",
                Phone = "(604) 525-2474",
                Fax = "(604) 525-6286",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 645,
                Name = "NEWGEN PHARMACHOICE",
                Address = "#101 - 1945 McCallum Rd, Abbotsford BC V2S 3N2 Canada",
                ManagerName = "Andrew Leung",
                Phone = "(604) 859-2351",
                Fax = "(604) 859-1997",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 646,
                Name = "NEWPORT VILLAGE PHARMACY",
                Address = "103 - 205 Newport Dr, Port Moody BC V3H 5C9 Canada",
                ManagerName = "Sarfaraz Jeraj",
                Phone = "(604) 461-0136",
                Fax = "(604) 461-0137",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 647,
                Name = "NOLAN'S CLINIC PHARMACY",
                Address = "2nd Floor, 3207 30 Ave, Vernon BC V1T 2C6 Canada",
                ManagerName = "Susan Carrie",
                Phone = "(250) 542-5866",
                Fax = "(250) 542-2709",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 648,
                Name = "NOLAN'S NORTH END PHARMACY",
                Address = "102 - 4710 31 St, Vernon BC V1T 5J9 Canada",
                ManagerName = "William Beley",
                Phone = "(250) 542-2265",
                Fax = "(250) 542-2264",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 649,
                Name = "NOLAN'S PHARMASAVE DRUGS #222",
                Address = "3101 - 30th Ave., Vernon BC V1T 2C4 Canada",
                ManagerName = "Ian Johnstone",
                Phone = "(250) 542-4181",
                Fax = "(250) 549-3391",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 650,
                Name = "NORDLYS PHARMACY",
                Address = "102 - 1100 Alaska Ave, Dawson Creek BC V1G 4V8 Canada",
                ManagerName = "Tamara Christensen",
                Phone = "(250) 782-0601",
                Fax = "(250) 782-0622",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 651,
                Name = "NORTH ISLAND PHARMACY",
                Address = "1371A Cedar St, Campbell River BC V9W 2W6 Canada",
                ManagerName = "Masih Alaeitafti",
                Phone = "(250) 286-4522",
                Fax = "(250) 286-4530",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 652,
                Name = "NORTH ISLAND PHARMACY - COURTENAY",
                Address = "949 Fitzgerald Avenue, Courtenay BC V9N 2R6 Canada",
                ManagerName = "Duane Biblow",
                Phone = "(250) 331-6306",
                Fax = "(250) 871-6305",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 653,
                Name = "NORTH ROAD PHARMASAVE",
                Address = "103 - 655 North Road, Coquitlam BC V3J 1P5 Canada",
                ManagerName = "Reza Najmabadi",
                Phone = "(604) 937-5544",
                Fax = "(604) 937-5554",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 654,
                Name = "NORTH SHORE PHARMACY",
                Address = "#4 - 517 Tranquille Rd, Kamloops BC V2B 3H3 Canada",
                ManagerName = "Pradeep Damodaran",
                Phone = "(250) 376-9991",
                Fax = "(250) 376-9922",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 655,
                Name = "NORTH WEST MARINE PHARMACY",
                Address = "1877 Marine Dr, North Vancouver BC V7P 1V5 Canada",
                ManagerName = "Ana Kabirnoshanagh",
                Phone = "(604) 982-0981",
                Fax = "(604) 982-0903",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 656,
                Name = "NORTHMOUNT PHARMACY LTD.",
                Address = "165 E 15th Street, North Vancouver BC V7L 2P7 Canada",
                ManagerName = "Jack Chu",
                Phone = "(604) 985-8241",
                Fax = "(604) 985-1240",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 657,
                Name = "NORTHSHORE DRUGSTORE",
                Address = "113 West 16th St, North Vancouver BC V7M 1T3 Canada",
                ManagerName = "Kamran Salehi",
                Phone = "(604) 770-3299",
                Fax = "(604) 770-3298",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 658,
                Name = "NORTHVIEW PHARMACY",
                Address = "120 - 1100 Lonsdale Ave, North Vancouver BC V7M 2H3 Canada",
                ManagerName = "Tom Yoon",
                Phone = "(604) 904-9992",
                Fax = "(604) 904-0222",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 659,
                Name = "NOVACARE PHARMACY",
                Address = "15 E. Hastings St., Vancouver BC V6A 1M9 Canada",
                ManagerName = "Ashkan Tahmasebi Boldaji",
                Phone = "(604) 303-6344",
                Fax = "(604) 303-6345",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 660,
                Name = "NOVARA WELLNESS PHARMACY",
                Address = "3482 Main St, Vancouver BC V5V 3N2 Canada",
                ManagerName = "Tarana Novrouzova",
                Phone = "(604) 875-6625",
                Fax = "(604) 875-8477",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 661,
                Name = "OASES HEALTH PHARMACY #1",
                Address = "790 East Hastings St., Vancouver BC V6A 1R5 Canada",
                ManagerName = "Thomas Grgic",
                Phone = "(604) 254-4633",
                Fax = "(604) 254-3364",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 662,
                Name = "OCEAN PHARMACY",
                Address = "1880 Marine Drive, West Vancouver BC V7V 1J6 Canada",
                ManagerName = "Aaron Wong",
                Phone = "(604) 922-1238",
                Fax = "(604) 926-3908",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 663,
                Name = "OCEANSIDE PHARMACY",
                Address = "105 - 2506 Beacon Ave, Sidney BC V8L 1Y2 Canada",
                ManagerName = "Khalid Hammad",
                Phone = "778-351-2111",
                Fax = "778-351-2110",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 664,
                Name = "OKANAGAN PHARMACY",
                Address = "24 - 5500 Clements Cres, Peachland BC V0H 1X5 Canada",
                ManagerName = "Chelsea Argent",
                Phone = "(250) 767-2911",
                Fax = "(250) 767-2906",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 665,
                Name = "OLIVER PHARMACY",
                Address = "105 - 291 Fairview Rd, PO Box 1871, Oliver BC V0H 1T0 Canada",
                ManagerName = "Christopher Pasin",
                Phone = "(250) 485-4007",
                Fax = "(250) 485-4002",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 666,
                Name = "OMNIA SPECIALTY PHARMACY",
                Address = "103 - 32625 South Fraser Way, Abbotsford BC V2T 1X8 Canada",
                ManagerName = "Kin Ng",
                Phone = "(604) 776-3377",
                Fax = "(604) 776-3355",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 667,
                Name = "OMNICARE PHARMACY LTD.",
                Address = "#130 - 1 East Cordova St, Vancouver BC V6A 4H3 Canada",
                ManagerName = "Victor Chan",
                Phone = "(604) 633-1289",
                Fax = "(604) 633-1298",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 668,
                Name = "OPTIMA PHARMACY",
                Address = "103-6625 152A Street, Surrey BC V3S 0B3 Canada",
                ManagerName = "Karman Sohi",
                Phone = "(604) 593-6113",
                Fax = "(604) 593-6114",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 669,
                Name = "ORCHARD PHARMACY",
                Address = "152 - 1876 Cooper Rd, Kelowna BC V1Y 9N6 Canada",
                ManagerName = "Bryan McIntyre",
                Phone = "(236) 420-2882",
                Fax = "(236) 420-2881",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 670,
                Name = "OTTER CO-OP AT PARALLEL PHARMACY",
                Address = "Unit 100 - 1888 North Parallel Rd., Abbotsford BC V3G 2C6 Canada",
                ManagerName = "Harikrishna Dasani",
                Phone = "(778) 771-0410",
                Fax = "(604) 851-9665",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 671,
                Name = "OTTER CO-OP PHARMACY",
                Address = "3650 248 St, Langley BC V4W 1X7 Canada",
                ManagerName = "Hesham Metwaly",
                Phone = "(604) 607-6934",
                Fax = "(604) 856-3101",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 672,
                Name = "OTTER CO-OP PHARMACY (MT. LEHMAN)",
                Address = "250-3270 Mt Lehman Rd, Abbotsford BC V4X 2M9 Canada",
                ManagerName = "Sundeep Dhillon",
                Phone = "778-655-5170",
                Fax = "778-655-5171",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 673,
                Name = "OUT REACH PHARMACY LTD.",
                Address = "#250 - 55 Victoria Rd, Nanaimo BC V9R 5N9 Canada",
                ManagerName = "Lalit Dahiya",
                Phone = "(250) 753-9606",
                Fax = "(250) 753-9608",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 674,
                Name = "OVAL INTEGRATIVE PHARMACY",
                Address = "Unit 160 - 6111 River Road, Richmond BC V7C 0A2 Canada",
                ManagerName = "Grace Chong",
                Phone = "(604) 838-9123",
                Fax = "(604) 838-9123",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 675,
                Name = "OWL DRUGS",
                Address = "199 East Hastings St., Vancouver BC V6A 1N5 Canada",
                ManagerName = "Dhaval Patel",
                Phone = "(604) 681-3024",
                Fax = "(604) 681-3048",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 676,
                Name = "OXFORD PHARMACY",
                Address = "3190 - 1971 Lougheed Hwy, Port Coquitlam BC V3B 0K2 Canada",
                ManagerName = "Farnaz Bondar",
                Phone = "(604) 945-9591",
                Fax = "(604) 945-9592",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 677,
                Name = "P & A PHARMACY",
                Address = "4336 Fraser St., Vancouver BC V5V 4G3 Canada",
                ManagerName = "Thomas Tse",
                Phone = "(604) 876-4424",
                Fax = "(604) 876-8845",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 678,
                Name = "P.D.C. PHARMACY",
                Address = "2370 United Blvd, Coquitlam BC V3K 6A3 Canada",
                ManagerName = "Alan Huang",
                Phone = "(604) 927-2620",
                Fax = "(604) 941-0532",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 679,
                Name = "PACIFIC CARE PHARMACY",
                Address = "2651 Kingsway, Vancouver BC V5R 5H4 Canada",
                ManagerName = "Jeevan Tamana",
                Phone = "(604) 438-6200",
                Fax = "(604) 438-6201",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 680,
                Name = "PACIFIC HEIGHTS PRESCRIPTION X-PRESS PHARMACY",
                Address = "Unit 101, 42 6th Street, New Westminster BC V3L 2Z2 Canada",
                ManagerName = "Rajeev Chauhan",
                Phone = "(604) 244-4580",
                Fax = "(604) 676-2625",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 681,
                Name = "PACIFIC PHARMACY #1",
                Address = "101 - 15122 72 Ave, Surrey BC V3S 2G2 Canada",
                ManagerName = "Harinder Khattra",
                Phone = "(604) 590-0399",
                Fax = "(604) 590-0349",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 682,
                Name = "PACIFIC PHARMACY #2",
                Address = "11944 88 Ave, Delta BC V4C 3C8 Canada",
                ManagerName = "Gurminder Gill",
                Phone = "(778) 578-6900",
                Fax = "(778) 578-6901",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 683,
                Name = "PACIFIC PHARMACY #4",
                Address = "106-1522 Finlay St, White Rock BC V4B 4L9 Canada",
                ManagerName = "Randeep Sekhon",
                Phone = "(604) 232-9557",
                Fax = "(604) 560-0870",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 684,
                Name = "PARK PACIFIC PHARMACY",
                Address = "Yaohan Centre, #2260 - 3700 No. 3 Road, Richmond BC V6X 3X2 Canada",
                ManagerName = "Cheng Yu Lin",
                Phone = "(604) 273-9812",
                Fax = "(604) 270-2228",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 685,
                Name = "PARK WEST PHARMACY",
                Address = "1635 Capilano Rd, North Vancouver BC V7P 0E5 Canada",
                ManagerName = "Ebrahim Khosravi Haghighi",
                Phone = "(236) 551-2260",
                Fax = "(236) 551-2261",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 686,
                Name = "PARKRIDGE PHARMACY & HEALTH CENTRE",
                Address = "#3 - 802 George St, Enderby BC V0E 1V0 Canada",
                ManagerName = "David Robertson",
                Phone = "(250) 838-5866",
                Fax = "(250) 838-5877",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 687,
                Name = "PARKSVILLE PHARMACY",
                Address = "#1 - 383 Alberni Highway, Parksville BC V9P 1J9 Canada",
                ManagerName = "Elizabeth Abad",
                Phone = "(250) 586-2625",
                Fax = "(250) 586-2626",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 688,
                Name = "PARTNERSHIP RX",
                Address = "105-15336 67 Ave, Surrey BC V3S 7C6 Canada",
                ManagerName = "Ajreet Bassi",
                Phone = "(888) 588-2605",
                Fax = "(236) 484-9425",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 689,
                Name = "PEACE PHARMACY",
                Address = "9840 101 Ave, Fort St. John BC V1J 2B2 Canada",
                ManagerName = "Joby Joseph",
                Phone = "(250) 785-1140",
                Fax = "(250) 785-1145",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 690,
                Name = "PEACHLAND PHARMACY",
                Address = "5848A Beach Ave, Peachland BC V0H 1X7 Canada",
                ManagerName = "Curtis Fieseler",
                Phone = "(250) 767-2611",
                Fax = "(250) 767-3477",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 691,
                Name = "PEAK HEALTH PHARMACY",
                Address = "120-8063 199 St, Langley BC V2Y 0E2 Canada",
                ManagerName = "Andy Dhillon",
                Phone = "(604) 918-8779",
                Fax = "(604) 918-8739",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 692,
                Name = "PEERCONNECT PHARMACY",
                Address = "8086 Park Road, Richmond BC V6Y 1T1 Canada",
                ManagerName = "Rajdeep Pooni",
                Phone = "(604) 207-2222",
                Fax = "(604) 207-6666",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 693,
                Name = "PENDER ISLAND PHARMACY",
                Address = "Box 134, R.R. #1, #10 - 4605 Bedwell Harbour Road, Pender Island BC V0N 2M0 Canada",
                ManagerName = "Christine Swan",
                Phone = "(250) 629-6555",
                Fax = "(250) 629-6533",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 694,
                Name = "PENINSULA IDA PHARMACY",
                Address = "2 - 2379 Bevan Avenue, Sidney BC V8L4M9 Canada",
                ManagerName = "Waverly Lam",
                Phone = "(250) 656-0882",
                Fax = "(250) 656-0822",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 695,
                Name = "PEOPLES INWELL PHARMACY",
                Address = "Unit 1 - 4655 Central Blvd, Burnaby BC V5H 4H7 Canada",
                ManagerName = "Tony Wang",
                Phone = "(604) 568-8713",
                Fax = "(604) 568-8723",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 696,
                Name = "PEOPLES PHARMACY",
                Address = "103 - 1910 Sooke Rd, Victoria BC V9B 1V7 Canada",
                ManagerName = "Henry Kwok",
                Phone = "(250) 474-9331",
                Fax = "(250) 474-9336",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 697,
                Name = "PEOPLES PHARMACY #325",
                Address = "107 - 15551 Fraser Hwy, Surrey BC V3S 2V8 Canada",
                ManagerName = "May Yi",
                Phone = "(604) 585-6227",
                Fax = "(604) 585-0105",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 698,
                Name = "PEOPLES PHARMACY #369",
                Address = "101-8386 120 St, Surrey BC V3W 3N4 Canada",
                ManagerName = "Manju Koruthu",
                Phone = "(604) 593-1788",
                Fax = "(604) 593-1769",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 699,
                Name = "PEOPLES PHARMACY #377",
                Address = "115-6363 168 St, Surrey BC V3S 3Y2 Canada",
                ManagerName = "Purakkumar Patel",
                Phone = "(604) 576-6815",
                Fax = "(604) 576-6857",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 700,
                Name = "PEOPLES PHARMACY #387",
                Address = "102 - 1200 Lonsdale Ave, North Vancouver BC V7M 3H6 Canada",
                ManagerName = "Ossama Abou Assi",
                Phone = "(604) 984-0686",
                Fax = "(604) 984-2766",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 701,
                Name = "PEOPLES PHARMACY ALDERGROVE",
                Address = "27265 Fraser Hwy, Aldergrove BC V4W 3P9 Canada",
                ManagerName = "Mohamed Saleh",
                Phone = "(604) 624-0660",
                Fax = "(604) 624-0662",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 702,
                Name = "PEOPLES SHIFA PHARMACY",
                Address = "103-2388 156 St, Surrey BC V4A 4V4 Canada",
                ManagerName = "Mohmmedsahil Tirmiji",
                Phone = "(604) 502-7030",
                Fax = "(604) 502-7031",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 703,
                Name = "PG GATEWAY PHARMACY",
                Address = "118 - 1811 Victoria St, Prince George BC V2L 2L6 Canada",
                ManagerName = "Samy Hanna",
                Phone = "(250) 564-9993",
                Fax = "(250) 564-9997",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 704,
                Name = "PHARMACHOICE #9045",
                Address = "120-10200 8 St, Dawson Creek BC V1G 3P8 Canada",
                ManagerName = "Francis Obeta",
                Phone = "(250) 782-1902",
                Fax = "(250) 782-1908",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 705,
                Name = "PHARMACHOICE #9100",
                Address = "101 - 2280 Hastings St E, Vancouver BC V5L 1V4 Canada",
                ManagerName = "Abed Samman",
                Phone = "(604) 305-0345",
                Fax = "(604) 305-0346",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 706,
                Name = "PHARMACHOICE 007",
                Address = "2529 Shaughnessy St., Port Coquitlam BC V3C 3G1 Canada",
                ManagerName = "Chevy Anne Pabustan",
                Phone = "(604) 941-2413",
                Fax = "(604) 941-6754",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 707,
                Name = "PHARMACHOICE FOOD AND DRUG #9062",
                Address = "825 Shuswap Ave, Chase BC V0E 1M0 Canada",
                ManagerName = "Ellen Bovair",
                Phone = "(250) 679-8611",
                Fax = "(778) 599-0990",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 708,
                Name = "PHARMACHOICE LANGFORD PHARMACY",
                Address = "109 - 2854 Peatt Rd, Victoria BC V9B 0W3 Canada",
                ManagerName = "Iriny Serabana",
                Phone = "(778) 265-5550",
                Fax = "(778) 265-5559",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 709,
                Name = "PHARMACHOICE MAPLE RIDGE PHARMACY",
                Address = "100 - 22470 Dewdney Trunk Rd, Maple Ridge BC V2X 5Z6 Canada",
                ManagerName = "Hanea Ismail",
                Phone = "(604) 380-2551",
                Fax = "(604) 380-2552",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 710,
                Name = "PHARMACHOICE NORTHERN PHARMACY",
                Address = "1649 15th Ave, Prince George BC V2L 3X2 Canada",
                ManagerName = "Mohit Mahajan",
                Phone = "(236) 423-2211",
                Fax = "(236) 423-2214",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 711,
                Name = "PHARMACHOICE PHARMACY #9063",
                Address = "P.O. Box 24020, #1-3874 Squilax Anglemont Hwy., Scotch Creek BC V0E 3L0 Canada",
                ManagerName = "Michael Hoenmans",
                Phone = "(250) 955-0602",
                Fax = "(250) 955-0394",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 712,
                Name = "PHARMACHOICE PHARMACY #9072",
                Address = "571 West 57th Ave., Vancouver BC V6P 1R8 Canada",
                ManagerName = "Frank Qi",
                Phone = "(604) 324-2258",
                Fax = "(604) 324-2259",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 713,
                Name = "PHARMACHOICE TRAIL APOTHECARY",
                Address = "1101 Dewdney Ave, Trail BC V1R 4T1 Canada",
                ManagerName = "Jillian Hewitt",
                Phone = "(250) 364-1993",
                Fax = "(250) 364-1936",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 714,
                Name = "PHARMACHOICE WALNUT GROVE PHARMACY",
                Address = "150 - 20330 88th Ave, Langley BC V1M 2Y4 Canada",
                ManagerName = "Rafik Ramadan",
                Phone = "(604) 371-1388",
                Fax = "(604) 371-1389",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 715,
                Name = "PHARMACISTS CLINIC",
                Address = "2405 Wesbrook Mall, Vancouver BC V6T 1Z3 Canada",
                ManagerName = "Jamie Yuen",
                Phone = "(604) 827-2584",
                Fax = "(604) 827-2579",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 716,
                Name = "PHARMACITY DRUGSTORE",
                Address = "101 - 8338 120 St, Surrey BC V3W 3N4 Canada",
                ManagerName = "Gursimran Panesar",
                Phone = "(604) 595-2873",
                Fax = "(604) 595-2840",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 717,
                Name = "PHARMACITY DRUGSTORE #2",
                Address = "Unit 102 - 2752 Allwood St, Abbotsford BC V2T 3R7 Canada",
                ManagerName = "Archna Sood",
                Phone = "(604) 855-8882",
                Fax = "(604) 855-8836",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 718,
                Name = "PHARMACITY DRUGSTORE #3",
                Address = "#105 - 15240 56 Ave, Surrey BC V3S 5K7 Canada",
                ManagerName = "Rajeshkumar Shah",
                Phone = "(604) 574-3331",
                Fax = "(604) 574-3342",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 719,
                Name = "PHARMACY 24",
                Address = "#108 - 2777 Gladwin Rd, Abbotsford BC V2T 4V1 Canada",
                ManagerName = "Kuldeep Sandhu",
                Phone = "(604) 853-8884",
                Fax = "(604) 853-8808",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 720,
                Name = "PHARMACY 24 NO 5",
                Address = "#126 - 15299 68 Ave, Surrey BC V3S 2C1 Canada",
                ManagerName = "Jaspal Sandhu",
                Phone = "(778) 218-4102",
                Fax = "(778) 218-4103",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 721,
                Name = "PHARMACY APOTEKA",
                Address = "Unit A - 7487 Edmonds St, Burnaby BC V3N 1B3 Canada",
                ManagerName = "Radmila Veljovic",
                Phone = "(604) 526-7778",
                Fax = "(604) 540-1555",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 722,
                Name = "PHARMALIFE PHARMACY",
                Address = "Capilano Mall, 60 - 935 Marine Dr, North Vancouver BC V7P 1S3 Canada",
                ManagerName = "Mona Azadmoghaddam",
                Phone = "(778) 340-1800",
                Fax = "(778) 340-1888",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 723,
                Name = "PHARMAPLUS RX",
                Address = "2021 Commercial Drive, Vancouver BC V5N 4B1 Canada",
                ManagerName = "Hitesh Patel",
                Phone = "(778) 331-3847",
                Fax = "(778) 331-3848",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 724,
                Name = "PHARMASAVE #   21",
                Address = "105 - 13585 16 Ave, Surrey BC V4A 1P6 Canada",
                ManagerName = "Nasir Shaik",
                Phone = "(604) 385-1175",
                Fax = "(604) 385-1177",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 725,
                Name = "PHARMASAVE #   25",
                Address = "104 - 1824 Gordon Dr, Kelowna BC V1Y 0E2 Canada",
                ManagerName = "Bijalbharthi Goswami",
                Phone = "(778) 484-4732",
                Fax = "(778) 484-4735",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 726,
                Name = "PHARMASAVE #   32",
                Address = "3429 10th St, Houston BC V0J 1Z0 Canada",
                ManagerName = "Ajay Nair",
                Phone = "(250) 845-3700",
                Fax = "(250) 845-3750",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 727,
                Name = "PHARMASAVE #   34",
                Address = "Box 2577, 1211 Main St, Smithers BC V0J 2N0 Canada",
                ManagerName = "Mike Brinnen",
                Phone = "(250) 847-8750",
                Fax = "(250) 847-8760",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 728,
                Name = "PHARMASAVE #   42",
                Address = "3979 Lakeshore Rd, Kelowna BC V1W 1V3 Canada",
                ManagerName = "Michelle Stevens",
                Phone = "(250) 764-6410",
                Fax = "(250) 764-6439",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 729,
                Name = "PHARMASAVE #   52",
                Address = "101 - 1302 7th Ave, Prince George BC V2L 3P1 Canada",
                ManagerName = "Maureen Brin",
                Phone = "(250) 562-5309",
                Fax = "(250) 562-5324",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 730,
                Name = "PHARMASAVE #   57",
                Address = "6323 Main St, Oliver BC V0H 1T0 Canada",
                ManagerName = "Jim Shekula",
                Phone = "(250) 498-2830",
                Fax = "(250) 498-2835",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 731,
                Name = "PHARMASAVE #   71",
                Address = "110 - 6350 120 St, Surrey BC V3X 3K1 Canada",
                ManagerName = "Elizabeth Mathew",
                Phone = "(604) 507-3999",
                Fax = "(604) 507-3939",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 732,
                Name = "PHARMASAVE #   79",
                Address = "#205 - 650 West 41st Ave., Vancouver BC V5Z 2M9 Canada",
                ManagerName = "Flora Wang",
                Phone = "(604) 266-8455",
                Fax = "(604) 266-8975",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 733,
                Name = "PHARMASAVE #   87",
                Address = "1070 Howe St, Vancouver BC V6Z 1P5 Canada",
                ManagerName = "Mona Kwong",
                Phone = "(604) 899-0930",
                Fax = "(604) 899-0934",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 734,
                Name = "PHARMASAVE #   96",
                Address = "101 - 12005 238B St, Maple Ridge BC V4R 1W1 Canada",
                ManagerName = "Vilma Rafael Mucha",
                Phone = "(604) 476-1420",
                Fax = "(604) 476-1410",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 735,
                Name = "PHARMASAVE #  006",
                Address = "#110 - 7343 Hurd St., Mission BC V2V 3H7 Canada",
                ManagerName = "Darcy D'Amours",
                Phone = "(604) 820-1669",
                Fax = "(604) 820-1460",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 736,
                Name = "PHARMASAVE #  022",
                Address = "Suite 1103.8 - 3880 Grant McConachie Way, Vanc. Int'l Airport, Domestic Terminal, Richmond BC V7B 0A5 Canada",
                ManagerName = "Jeannie Mah",
                Phone = "(604) 303-7033",
                Fax = "(604) 303-0739",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 737,
                Name = "PHARMASAVE #  028",
                Address = "Unit 1020 - 4151 Hazelbridge Way, Richmond BC V6X 4J7 Canada",
                ManagerName = "Annie Sun",
                Phone = "(604) 273-8020",
                Fax = "(604) 273-8999",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 738,
                Name = "PHARMASAVE #  065",
                Address = "1308 Commercial Dr, Vancouver BC V5L 3X6 Canada",
                ManagerName = "Kunakar Pou",
                Phone = "(604) 215-5500",
                Fax = "(604) 215-5504",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 739,
                Name = "PHARMASAVE #  067",
                Address = "Vernon Hospital, 2101 32 St, Vernon BC V1T 5L2 Canada",
                ManagerName = "Samuel Nolan",
                Phone = "(778) 475-4929",
                Fax = "(778) 475-4930",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 740,
                Name = "PHARMASAVE #  081",
                Address = "302 - 1150 Marine Dr, North Vancouver BC V7P 1S8 Canada",
                ManagerName = "Lienny Thio",
                Phone = "(604) 971-5163",
                Fax = "(604) 971-5183",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 741,
                Name = "PHARMASAVE #  082",
                Address = "4628 Main St, Vancouver BC V5V 3R7 Canada",
                ManagerName = "Connie Ng",
                Phone = "(604) 873-3138",
                Fax = "(604) 873-3132",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 742,
                Name = "PHARMASAVE #  089 TABOR PLAZA",
                Address = "#227 - 100 Tabor Blvd S, Prince George BC V2M 5T4 Canada",
                ManagerName = "Jeremy Comba",
                Phone = "(250) 562-3784",
                Fax = "(250) 564-7283",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 743,
                Name = "PHARMASAVE #  100",
                Address = "101B - 3055 Oak St, RR 1, Chemainus BC V0R 1K1 Canada",
                ManagerName = "Nicolas Jones",
                Phone = "(250) 324-4488",
                Fax = "(250) 324-4484",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 744,
                Name = "PHARMASAVE #  102",
                Address = "1109 Austin Ave., Coquitlam BC V3K 3P4 Canada",
                ManagerName = "Jane Moiseyenko",
                Phone = "(604) 936-1488",
                Fax = "(604) 936-1409",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 745,
                Name = "PHARMASAVE #  105",
                Address = "3752 - 4th Ave., PO Box 2530, Smithers BC V0J 2N0 Canada",
                ManagerName = "Tinka VonKeyserlingk",
                Phone = "(250) 847-4474",
                Fax = "(250) 847-4760",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 746,
                Name = "PHARMASAVE #  106",
                Address = "330 Central Ave., Grand Forks BC V0H 1H0 Canada",
                ManagerName = "Emma Wey",
                Phone = "(250) 442-3515",
                Fax = "(250) 442-3225",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 747,
                Name = "PHARMASAVE #  108",
                Address = "1128 3rd Street, Castlegar BC V1N 3H4 Canada",
                ManagerName = "Kevin Ralloff",
                Phone = "(250) 365-7813",
                Fax = "(250) 365-2874",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 748,
                Name = "PHARMASAVE #  115",
                Address = "1 - 1153 Esquimalt Road, Victoria BC V9A 3N7 Canada",
                ManagerName = "Lisa Luu",
                Phone = "(250) 388-6451",
                Fax = "(250) 388-6832",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 749,
                Name = "PHARMASAVE #  129",
                Address = "3 - 575B Alder Ave, 100 Mile House BC V0K 2E0 Canada",
                ManagerName = "Stephanie Daoust",
                Phone = "(250) 395-2921",
                Fax = "(250) 395-3652",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 750,
                Name = "PHARMASAVE #  139",
                Address = "441 - 1st Avenue, Ladysmith BC V9G 1A3 Canada",
                ManagerName = "Louise Dynna",
                Phone = "(250) 245-3113",
                Fax = "(250) 245-3224",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 751,
                Name = "PHARMASAVE #  148",
                Address = "685 Baker St., Nelson BC V1L 4J3 Canada",
                ManagerName = "Chauncy Blair",
                Phone = "(250) 352-2316",
                Fax = "(250) 352-3768",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 752,
                Name = "PHARMASAVE #  151",
                Address = "11198 - 84th Ave., Delta BC V4C 2L7 Canada",
                ManagerName = "Bhanu Seelaboyina",
                Phone = "(604) 596-9551",
                Fax = "(604) 596-9521",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 753,
                Name = "PHARMASAVE #  162",
                Address = "101-7111 West Saanich Rd, Brentwood Bay BC V8M 1P7 Canada",
                ManagerName = "Greg Fong",
                Phone = "(250) 652-1235",
                Fax = "(778) 351-2544",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 754,
                Name = "PHARMASAVE #  179",
                Address = "Dell Shopping Centre, 10654 King George Blvd, Surrey BC V3T 2X3 Canada",
                ManagerName = "Mohammad Sarkar",
                Phone = "(604) 581-4431",
                Fax = "(604) 581-4130",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 755,
                Name = "PHARMASAVE #  182",
                Address = "720 Memorial Ave, Qualicum Beach BC V9K 1T3 Canada",
                ManagerName = "Dean Bonthuis",
                Phone = "(250) 752-3421",
                Fax = "(250) 752-3479",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 756,
                Name = "PHARMASAVE #  183",
                Address = "285 Craig St, Duncan BC V9L 1W2 Canada",
                ManagerName = "Thomas Lee",
                Phone = "(250) 748-5252",
                Fax = "(250) 748-0729",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 757,
                Name = "PHARMASAVE #  190",
                Address = "3295 Coast Meridian Rd., Port Coquitlam BC V3B 3N3 Canada",
                ManagerName = "Darin Fenton",
                Phone = "(604) 942-9813",
                Fax = "(604) 942-1561",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 758,
                Name = "PHARMASAVE #  198",
                Address = "235 Wallace Street, Hope BC V0X 1L0 Canada",
                ManagerName = "Michael McLoughlin",
                Phone = "(604) 869-2486",
                Fax = "(604) 869-2931",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 759,
                Name = "PHARMASAVE #  202",
                Address = "417 10th Ave, Invermere BC V0A 1K0 Canada",
                ManagerName = "Lizel Contreras",
                Phone = "(250) 342-8877",
                Fax = "(250) 342-8897",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 760,
                Name = "PHARMASAVE #  203",
                Address = "8697 - 10th Ave, Burnaby BC V3N 2S9 Canada",
                ManagerName = "Ellen Yam",
                Phone = "(604) 522-8050",
                Fax = "(604) 522-8779",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 761,
                Name = "PHARMASAVE #  205",
                Address = "200 - 9810 - 7th Street, Sidney BC V8L 4W6 Canada",
                ManagerName = "Shamim Rajan",
                Phone = "(250) 656-1148",
                Fax = "(250) 656-2235",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 762,
                Name = "PHARMASAVE #  206",
                Address = "#119 - 15280 - 101st Ave., Surrey BC V3R 8X7 Canada",
                ManagerName = "Trisha Bautista",
                Phone = "(604) 584-3331",
                Fax = "(604) 584-3321",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 763,
                Name = "PHARMASAVE #  211",
                Address = "#101 - 8850 Walnut Grove Dr, Langley BC V1M 2C9 Canada",
                ManagerName = "Henary Ibrahim",
                Phone = "(604) 888-5602",
                Fax = "(604) 888-7206",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 764,
                Name = "PHARMASAVE #  213",
                Address = "307 Victoria Rd W, Box 680, Revelstoke BC V0E 2S0 Canada",
                ManagerName = "Remon Saad",
                Phone = "(250) 837-2028",
                Fax = "(250) 837-4636",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 765,
                Name = "PHARMASAVE #  214",
                Address = "5331 Headland Dr, West Vancouver BC V7W 3C6 Canada",
                ManagerName = "Helen Joannou",
                Phone = "(604) 926-5331",
                Fax = "(604) 926-6052",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 766,
                Name = "PHARMASAVE #  221",
                Address = "#11A - 2720 Mill Bay Road, Box 160, Mill Bay BC V0R 2P0 Canada",
                ManagerName = "Eric Skoretz",
                Phone = "(250) 743-9111",
                Fax = "(250) 743-9066",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 767,
                Name = "PHARMASAVE #  230",
                Address = "201 - 365 Murtle Cres, Clearwater BC V0E 1N1 Canada",
                ManagerName = "Michelle Leins",
                Phone = "(250) 674-0059",
                Fax = "(250) 674-0056",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 768,
                Name = "PHARMASAVE #  232",
                Address = "#310 - 777 Royal Oak Dr., Victoria BC V8X 4V1 Canada",
                ManagerName = "Troy Giesbrecht",
                Phone = "(250) 727-2284",
                Fax = "(250) 727-2093",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 769,
                Name = "PHARMASAVE #  242",
                Address = "#41 - 3155 Lakeshore Rd, Kelowna BC V1W 3S9 Canada",
                ManagerName = "Bob Der",
                Phone = "(250) 717-5330",
                Fax = "(250) 717-5332",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 770,
                Name = "PHARMASAVE #  244",
                Address = "270 Hudson Avenue N.E., Salmon Arm BC V1E 4P4 Canada",
                ManagerName = "Muffadal Shamshuddin",
                Phone = "(250) 832-2111",
                Fax = "(250) 832-9329",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 771,
                Name = "PHARMASAVE #  246",
                Address = "132-4857 Elliott St, Delta BC V4K 2X7 Canada",
                ManagerName = "Kristine Lin",
                Phone = "(604) 946-7685",
                Fax = "(604) 940-6816",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 772,
                Name = "PHARMASAVE #  248",
                Address = "#409 - 15940 Fraser Highway, Surrey BC V4N 0X8 Canada",
                ManagerName = "Francis Chin",
                Phone = "(604) 501-2711",
                Fax = "(604) 501-2710",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 773,
                Name = "PHARMASAVE #  249",
                Address = "9515 Main Street, Summerland BC V0H 1Z0 Canada",
                ManagerName = "Jeffrey Wyse",
                Phone = "(250) 494-7088",
                Fax = "(250) 494-7086",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 774,
                Name = "PHARMASAVE #  250",
                Address = "526 - 7th Avenue, Keremeos BC V0X 1N0 Canada",
                ManagerName = "Darian Ngai",
                Phone = "(250) 499-5543",
                Fax = "(250) 499-5212",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 775,
                Name = "PHARMASAVE #  253",
                Address = "105 - 437 Glenmore Rd, Kelowna BC V1V 1Y5 Canada",
                ManagerName = "Craig Tostenson",
                Phone = "(250) 861-4443",
                Fax = "(250) 861-4943",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 776,
                Name = "PHARMASAVE #  255 - CRANBROOK",
                Address = "1005 Baker St., Cranbrook BC V1C 1A6 Canada",
                ManagerName = "Linda MacIntyre",
                Phone = "(250) 426-3368",
                Fax = "(250) 426-2365",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 777,
                Name = "PHARMASAVE #  257",
                Address = "5663 Cowrie St, Sechelt BC V0N 3A0 Canada",
                ManagerName = "Gurjene Dass",
                Phone = "(604) 885-9614",
                Fax = "(604) 885-7257",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 778,
                Name = "PHARMASAVE #  260",
                Address = "1816 Bowen Rd, Nanaimo BC V9S 5W4 Canada",
                ManagerName = "Kayla Wharton",
                Phone = "(250) 740-3880",
                Fax = "(250) 740-3885",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 779,
                Name = "PHARMASAVE #  264",
                Address = "Unit 18 - 117 Hwy 16 E, Burns Lake BC V0J 1E0 Canada",
                ManagerName = "Denis Nawrocki",
                Phone = "(250) 692-7077",
                Fax = "(250) 692-7066",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 780,
                Name = "PHARMASAVE #  265",
                Address = "7 - 2225 Guthrie Rd, Comox BC V9M 4G1 Canada",
                ManagerName = "Jacquie Nichol",
                Phone = "(250) 339-9879",
                Fax = "(250) 339-2343",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 781,
                Name = "PHARMASAVE #  267",
                Address = "Unit 5 - 1273 Island Hwy S, Nanaimo BC V9R 7A4 Canada",
                ManagerName = "Maria De Bruyns",
                Phone = "(250) 755-1830",
                Fax = "(250) 755-1832",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 782,
                Name = "PHARMASAVE #  275",
                Address = "130 - 1005 Columbia St, New Westminster BC V3M 6H5 Canada",
                ManagerName = "Christine Pothier",
                Phone = "(604) 525-5607",
                Fax = "(604) 525-5608",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 783,
                Name = "PHARMASAVE #  276",
                Address = "#1012 - 505 Doyle Ave., Kelowna BC V1Y 0C5 Canada",
                ManagerName = "Brett Federko",
                Phone = "(250) 860-0828",
                Fax = "(250) 860-0092",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 784,
                Name = "PHARMASAVE #  278",
                Address = "4367 Hastings St, Burnaby BC V5C 2J7 Canada",
                ManagerName = "Bernice Lam",
                Phone = "(604) 298-5910",
                Fax = "(604) 298-5930",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 785,
                Name = "PHARMASAVE #  280",
                Address = "1280 Cedar Ave, Trail BC V1R 4C1 Canada",
                ManagerName = "Lee Boyer",
                Phone = "(250) 368-3363",
                Fax = "(250) 368-5012",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 786,
                Name = "PHARMASAVE #  282",
                Address = "1118 Canyon St, Creston BC V0B 1G0 Canada",
                ManagerName = "Tamer Boctor",
                Phone = "(250) 428-9080",
                Fax = "(250) 428-9082",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 787,
                Name = "PHARMASAVE #  284",
                Address = "2401F Millstream Rd, Langford BC V9B 3R5 Canada",
                ManagerName = "Christopher Innes",
                Phone = "(250) 478-0123",
                Fax = "(250) 478-0129",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 788,
                Name = "PHARMASAVE #  285",
                Address = "101 - 1497 Admirals Rd, Victoria BC V9A 2P8 Canada",
                ManagerName = "Farah Caflisch",
                Phone = "(250) 388-5051",
                Fax = "(250) 388-5059",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 789,
                Name = "PHARMASAVE #  287 SULLIVAN SQUARE",
                Address = "107-15325 56 Ave, Surrey BC V3S 0X9 Canada",
                ManagerName = "Jashanjot Bal",
                Phone = "(604) 303-6343",
                Fax = "(604) 372-2772",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 790,
                Name = "PHARMASAVE #  291",
                Address = "9 - 2484 Main St, Westbank BC V4T 2G2 Canada",
                ManagerName = "Nelson Kuhlen",
                Phone = "(250) 707-0745",
                Fax = "(250) 707-0738",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 791,
                Name = "PHARMASAVE #  294 ORCHARDS WALK",
                Address = "101 - 3200 Valleyview Drive, Kamloops BC V2C 4S2 Canada",
                ManagerName = "Kim Winters",
                Phone = "(250) 828-8000",
                Fax = "(250) 828-8181",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 792,
                Name = "PHARMASAVE #  296",
                Address = "Box 850, #8 - 6716 West Coast Rd, Sooke BC V9Z 1H8 Canada",
                ManagerName = "Mike Stuber",
                Phone = "(250) 642-2226",
                Fax = "(250) 642-7742",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 793,
                Name = "PHARMASAVE # 059",
                Address = "100 - 180 Wilson St, Victoria BC V9A 7N6 Canada",
                ManagerName = "Naoya Wakako",
                Phone = "(250) 380-0049",
                Fax = "(250) 380-0118",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 794,
                Name = "PHARMASAVE #1009",
                Address = "6 - 33324 South Fraser Way, Abbotsford BC V2S 2B4 Canada",
                ManagerName = "Muhammad Arshad",
                Phone = "(604) 746-7117",
                Fax = "(604) 746-7114",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 795,
                Name = "PHARMASAVE #1014",
                Address = "101-8318 120 St, Surrey BC V3W 3N4 Canada",
                ManagerName = "Navneet Gohalwar",
                Phone = "604-510-0000",
                Fax = "(604) 510-1010",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 796,
                Name = "PHARMASAVE #1016",
                Address = "125 - 23233 Gilley Road, Richmond BC V6V 1E6 Canada",
                ManagerName = "Marcus Afan",
                Phone = "(604) 553-8431",
                Fax = "(604) 553-8432",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 797,
                Name = "PHARMASAVE #1019",
                Address = "123 West Esplanade, North Vancouver BC V7M 0G7 Canada",
                ManagerName = "Mahnoosh Jabbarzadehkaboli",
                Phone = "(604) 757-2962",
                Fax = "(604) 757-2963",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 798,
                Name = "PHARMASAVE #1020",
                Address = "1418 E 41st Ave, Vancouver BC V5P 1J7 Canada",
                ManagerName = "Farshid Tehrani",
                Phone = "(604) 423-5584",
                Fax = "(604) 558-1941",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 799,
                Name = "PHARMASAVE #1021",
                Address = "102-330 Highway 33 W, Kelowna BC V1X 1X9 Canada",
                ManagerName = "Navjot Bal",
                Phone = "(250) 491-1999",
                Fax = "(250) 491-4565",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 800,
                Name = "PHARMASAVE #1025",
                Address = "1715 Ellis St, Kelowna BC V1Y 8M9 Canada",
                ManagerName = "Andrew Low",
                Phone = "(250) 712-2484",
                Fax = "(250) 712-9266",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 801,
                Name = "PHARMASAVE #1026",
                Address = "101-1912 Enterprise Way, Kelowna BC V1Y 9S9 Canada",
                ManagerName = "Gaurav Chauhan",
                Phone = "(778) 760-9555",
                Fax = "(855) 942-4804",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 802,
                Name = "PHARMASAVE #1029",
                Address = "104 - 34143 Marshall Rd, Abbotsford BC V2S 1L8 Canada",
                ManagerName = "Ali Asghar",
                Phone = "(604) 744-1705",
                Fax = "(604) 744-1706",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 803,
                Name = "PHARMASAVE #1032",
                Address = "100 - 525 Third St, Nanaimo BC V9R 1W7 Canada",
                ManagerName = "Pushpinder Singh",
                Phone = "(250) 591-9000",
                Fax = "(250) 591-5979",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 804,
                Name = "PHARMASAVE #1037",
                Address = "2140 Main Street, Vancouver BC V5T 0K1 Canada",
                ManagerName = "Lori Hurd",
                Phone = "(604) 566-7767",
                Fax = "(604) 566-7768",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 805,
                Name = "PHARMASAVE #1043",
                Address = "334 2nd Ave West, Prince Rupert BC V8J 1G6 Canada",
                ManagerName = "Vishal Patel",
                Phone = "(250) 624-3333",
                Fax = "(250) 624-6666",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 806,
                Name = "PHARMASAVE #1046",
                Address = "10771 Delsom Crescent, Delta BC V4C 0A5 Canada",
                ManagerName = "Afzal Shaik",
                Phone = "(604) 531-7781",
                Fax = "(604) 531-7782",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 807,
                Name = "PHARMASAVE #1049",
                Address = "201-4851 Cedar Ridge Place, Nanaimo BC V9T 6M3 Canada",
                ManagerName = "Jaimishkumar Patel",
                Phone = "(250) 824-0700",
                Fax = "(250) 824-0701",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 808,
                Name = "PHARMASAVE #1088",
                Address = "Unit 3 - 555 W 12th Ave, Vancouver BC V5Z 3X7 Canada",
                ManagerName = "Minmin Xiang",
                Phone = "(604) 564-9288",
                Fax = "(604) 564-9188",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 809,
                Name = "PHARMASAVE #1101",
                Address = "111-505 Burrard Street, Vancouver BC V7X 1M3 Canada",
                ManagerName = "Victor Chu",
                Phone = "(604) 682-7785",
                Fax = "(604) 682-7744",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 810,
                Name = "PHARMASAVE #1106",
                Address = "#101 - 2276 S. Island Hwy., Campbell River BC V9W 1C3 Canada",
                ManagerName = "Trevor Choo",
                Phone = "(250) 923-7311",
                Fax = "(250) 923-3132",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 811,
                Name = "PHARMASAVE #1109",
                Address = "107 Centennial Square, PO Box 717, Sparwood BC V0B 2G0 Canada",
                ManagerName = "Ranbir Heir",
                Phone = "(250) 425-2015",
                Fax = "(250) 425-0294",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 812,
                Name = "PHARMASAVE #1110",
                Address = "103 - 4741 Lakelse Ave, Terrace BC V8G 1R5 Canada",
                ManagerName = "Johanne Chaine",
                Phone = "(250) 635-2206",
                Fax = "(250) 635-2207",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 813,
                Name = "PHARMASAVE & FOOD #142",
                Address = "110 - 1641 Hillside Ave., Victoria BC V8T 5G1 Canada",
                ManagerName = "Mohammed Siraj",
                Phone = "(250) 595-8106",
                Fax = "(250) 595-7308",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 814,
                Name = "PHARMASAVE 1051 OKANAGAN FALLS",
                Address = "5217 9th Ave, Okanagan Falls BC V0H 1R0 Canada",
                ManagerName = "Tawnya Froese",
                Phone = "(250) 497-8050",
                Fax = "(250) 497-8400",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 815,
                Name = "PHARMASAVE ABBOTT ST",
                Address = "101-2245 Abbott St, Kelowna BC V1Y 1E2 Canada",
                ManagerName = "John Tang",
                Phone = "(250) 980-5559",
                Fax = "(236) 573-2981",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 816,
                Name = "PHARMASAVE ABERDEEN",
                Address = "68 - 1395 Hillside Drive, Kamloops BC V2E 2R7 Canada",
                ManagerName = "Julie Ford",
                Phone = "(250) 314-1177",
                Fax = "(250) 314-1133",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 817,
                Name = "PHARMASAVE AGASSIZ #1044",
                Address = "7078 Pioneer Avenue, Agassiz BC V0M 1A0 Canada",
                ManagerName = "Ghada Abdel Aziz",
                Phone = "(604) 491-1619",
                Fax = "(604) 491-1675",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 818,
                Name = "PHARMASAVE BELLEVUE",
                Address = "202 16th St, West Vancouver BC V7V 3R5 Canada",
                ManagerName = "Parastoo Sharghi",
                Phone = "(604) 925-3304",
                Fax = "(604) 925-3312",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 819,
                Name = "PHARMASAVE COURTENAY",
                Address = "101-397 5th St, Courtenay BC V9N 1J9 Canada",
                ManagerName = "Simon McPhedran",
                Phone = "(250) 331-6961",
                Fax = "(250) 331-6982",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 820,
                Name = "PHARMASAVE DRUGS #154",
                Address = "Quilchena Square, 1800 Garcia St., Merritt BC V1K 1B8 Canada",
                ManagerName = "Mark Kunzli",
                Phone = "(250) 378-6066",
                Fax = "(250) 378-9296",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 821,
                Name = "PHARMASAVE EDGEMONT #107",
                Address = "3233 Connaught Cres, North Vancouver BC V7R 2V7 Canada",
                ManagerName = "Naz Teymouri Bayat",
                Phone = "(604) 988-6396",
                Fax = "(604) 988-3403",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 822,
                Name = "PHARMASAVE GANGES #120",
                Address = "104 Lower Ganges Road, Salt Spring Island BC V8K 2S7 Canada",
                ManagerName = "Christine Steffich",
                Phone = "(250) 537-5534",
                Fax = "(250) 537-8831",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 823,
                Name = "PHARMASAVE GLADWIN",
                Address = "100 - 3010 Gladwin Rd, Abbotsford BC V2T 0H5 Canada",
                ManagerName = "Ahmed Elmaddah",
                Phone = "604-853-8577",
                Fax = "604-853-8578",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 824,
                Name = "PHARMASAVE HEALTH CENTRE # 001",
                Address = "#100 - 2255 Elgin Ave., Port Coquitlam BC V3C 2B4 Canada",
                ManagerName = "Susan Behm",
                Phone = "(604) 942-7117",
                Fax = "(604) 942-4665",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 825,
                Name = "PHARMASAVE HEALTH CENTRE # 002",
                Address = "#106 - 2800 1st Ave E, Vancouver BC V5M 4N8 Canada",
                ManagerName = "Charles Fong",
                Phone = "(604) 215-8284",
                Fax = "(604) 215-8443",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 826,
                Name = "PHARMASAVE HEALTH CENTRE # 003",
                Address = "7 - 8948 202 St, Langley BC V1M 4A7 Canada",
                ManagerName = "Carol-Lynne Evens",
                Phone = "(604) 513-1414",
                Fax = "(604) 513-1420",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 827,
                Name = "PHARMASAVE HEALTH CENTRE # 005",
                Address = "105 - 19070 Lougheed Hwy, Pitt Meadows BC V3Y 2M6 Canada",
                ManagerName = "Karam Abdul-Ahad",
                Phone = "(604) 465-8807",
                Fax = "(604) 465-8809",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 828,
                Name = "PHARMASAVE HEALTH CENTRE # 007",
                Address = "#100 - 23148 96 Ave, Langley BC V1M 2S3 Canada",
                ManagerName = "Ranvir Dhanoya",
                Phone = "(604) 882-0611",
                Fax = "(604) 882-0610",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 829,
                Name = "PHARMASAVE HEALTH CENTRE # 010",
                Address = "#101 - 1160 Burrard St., Vancouver BC V6Z 2E8 Canada",
                ManagerName = "Thao Dao",
                Phone = "(604) 669-7700",
                Fax = "(604) 669-7282",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 830,
                Name = "PHARMASAVE HEALTH CENTRE # 011",
                Address = "C - 8301 78 Ave, Osoyoos BC V0H 1V0 Canada",
                ManagerName = "Gagandeep Kulaar",
                Phone = "(250) 495-7424",
                Fax = "(250) 495-7121",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 831,
                Name = "PHARMASAVE HEALTH CENTRE # 012 - LANGLEY CITY",
                Address = "101 - 20644 Fraser Hwy, Langley BC V3A 4G5 Canada",
                ManagerName = "Khaled Ezzeldin",
                Phone = "(604) 533-7322",
                Fax = "(604) 533-7331",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 832,
                Name = "PHARMASAVE HEALTH CENTRE # 014",
                Address = "250 - 1311 2nd St N, Cranbrook BC V1C 3L1 Canada",
                ManagerName = "Allan Hudock",
                Phone = "(250) 417-0270",
                Fax = "(250) 417-0274",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 833,
                Name = "PHARMASAVE HEALTH CENTRE # 015",
                Address = "5778 - 176A St, Surrey BC V3S 4H3 Canada",
                ManagerName = "Ankit Parikh",
                Phone = "(604) 576-2888",
                Fax = "(604) 576-2882",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 834,
                Name = "PHARMASAVE HEALTH CENTRE # 016",
                Address = "277 Evergreen Road, Campbell River BC V9W 5Y4 Canada",
                ManagerName = "Robert Simpson",
                Phone = "(250) 287-3222",
                Fax = "(250) 287-3284",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 835,
                Name = "PHARMASAVE HEALTH CENTRE # 017",
                Address = "1025 15th Ave W, Vancouver BC V6H 1R7 Canada",
                ManagerName = "Sabiha Zafar",
                Phone = "(604) 558-4006",
                Fax = "(604) 558-4007",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 836,
                Name = "PHARMASAVE HEALTH CENTRE # 020",
                Address = "#103 - 625 - 5th Ave, New Westminster BC V3M 1X4 Canada",
                ManagerName = "Agnes Fridl Poljak",
                Phone = "(604) 526-2233",
                Fax = "(604) 526-2205",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 837,
                Name = "PHARMASAVE HEALTH CENTRE # 024",
                Address = "1 - 281 East Island Hwy, Parksville BC V9P 2G3 Canada",
                ManagerName = "Lonny Barr",
                Phone = "(250) 951-0227",
                Fax = "(250) 951-0343",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 838,
                Name = "PHARMASAVE HEALTH CENTRE # 038",
                Address = "Richlea Shopping Centre, 116 - 10151 No. 3 Rd, Richmond BC V7A 4R6 Canada",
                ManagerName = "Ying Lin",
                Phone = "(604) 241-2898",
                Fax = "(604) 241-2810",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 839,
                Name = "PHARMASAVE HEALTH CENTRE # 049",
                Address = "131 First St, PO Box 1080, Tofino BC V0R 2Z0 Canada",
                ManagerName = "Laura McDonald",
                Phone = "(250) 725-4949",
                Fax = "(250) 725-1249",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 840,
                Name = "PHARMASAVE HEALTH CENTRE # 050",
                Address = "15168 Fraser Hwy, Surrey BC V3R 3P1 Canada",
                ManagerName = "Omer Ismail",
                Phone = "(604) 580-1456",
                Fax = "(604) 580-1455",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 841,
                Name = "PHARMASAVE HEALTH CENTRE # 051",
                Address = "2525 St. John's St, Port Moody BC V3H 2B3 Canada",
                ManagerName = "Effie Tsalkitzis",
                Phone = "(604) 936-2273",
                Fax = "(604) 936-2278",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 842,
                Name = "PHARMASAVE HEALTH CENTRE # 056",
                Address = "300 - 32900 Marshall Rd, Abbotsford BC V2S 0C2 Canada",
                ManagerName = "Sukhbir Gohal",
                Phone = "(604) 870-5600",
                Fax = "(604) 870-2955",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 843,
                Name = "PHARMASAVE HEALTH CENTRE # 058",
                Address = "102 - 2048 41st Ave W, Vancouver BC V6M 1Y8 Canada",
                ManagerName = "Sumar",
                Phone = "(604) 261-3335",
                Fax = "(604) 261-3336",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 844,
                Name = "PHARMASAVE HEALTH CENTRE # 063",
                Address = "2580 Granville St, Vancouver BC V6H 3G8 Canada",
                ManagerName = "Yesha Patel",
                Phone = "(604) 558-3003",
                Fax = "(604) 558-3004",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 845,
                Name = "PHARMASAVE HEALTH CENTRE # 064",
                Address = "#114 - 300 Riverside Dr, Penticton BC V2A 9C9 Canada",
                ManagerName = "Gregory Nikkel",
                Phone = "(250) 493-5533",
                Fax = "(250) 493-5587",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 846,
                Name = "PHARMASAVE HEALTH CENTRE # 086",
                Address = "#520 - 3033 Immel St, Abbotsford BC V2S 6S2 Canada",
                ManagerName = "Brian Lock",
                Phone = "(604) 853-6696",
                Fax = "(604) 853-9917",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 847,
                Name = "PHARMASAVE HEALTH CENTRE # 091 NEWTON",
                Address = "107 - 14199 62 Ave, Surrey BC V3X 0B1 Canada",
                ManagerName = "Ramandeep Arora",
                Phone = "(604) 568-4750",
                Fax = "(604) 503-5181",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 848,
                Name = "PHARMASAVE HEALTH CENTRE # 092",
                Address = "10 - 1601 Burnwood Dr, Burnaby BC V5A 4H1 Canada",
                ManagerName = "Miguel Lopez-Dee",
                Phone = "(604) 428-2648",
                Fax = "(604) 428-2649",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 849,
                Name = "PHARMASAVE HEALTH CENTRE # 094",
                Address = "1808 Kingsway, Vancouver BC V5N 2S7 Canada",
                ManagerName = "Balhar Shergill",
                Phone = "(778) 379-4470",
                Fax = "(778) 379-4472",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 850,
                Name = "PHARMASAVE HEALTH CENTRE # 098",
                Address = "#3 - 22323 48 Ave, Langley BC V3A 0C1 Canada",
                ManagerName = "Kuldeesh Grewal",
                Phone = "(604) 510-5522",
                Fax = "(604) 510-5523",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 851,
                Name = "PHARMASAVE HEALTH CENTRE # 099",
                Address = "#10 - 2448 160 St, Surrey BC V3Z 0C8 Canada",
                ManagerName = "HJ Chen",
                Phone = "(604) 531-2690",
                Fax = "(604) 531-2691",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 852,
                Name = "PHARMASAVE HEALTH CENTRE # 207",
                Address = "#110 - 9193 Main Street, Chilliwack BC V2P 7S5 Canada",
                ManagerName = "Alvin Kim",
                Phone = "(604) 792-1240",
                Fax = "(604) 792-7208",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 853,
                Name = "PHARMASAVE HEALTH CENTRE #1000",
                Address = "101 - 1688 152 St, Surrey BC V4A 4N2 Canada",
                ManagerName = "Hanna Berglund",
                Phone = "(604) 538-6334",
                Fax = "(604) 538-6389",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 854,
                Name = "PHARMASAVE HEALTH CENTRE #1001",
                Address = "Unit 103B - 19161 Fraser Hwy, Surrey BC V3S 7H2 Canada",
                ManagerName = "Harpreet Sekhon",
                Phone = "(604) 372-1234",
                Fax = "(604) 372-1254",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 855,
                Name = "PHARMASAVE HEALTH CENTRE #1002",
                Address = "2207-F Glenmore Rd, Campbell River BC V9H 1E1 Canada",
                ManagerName = "Faith Cecilia Lim",
                Phone = "(778) 420-4311",
                Fax = "(778) 420-1311",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 856,
                Name = "PHARMASAVE HEALTH CENTRE #1005",
                Address = "2 - 19126 Ford Rd, Pitt Meadows BC V3Y 2P1 Canada",
                ManagerName = "Danny Wong",
                Phone = "(604) 460-4808",
                Fax = "(604) 460-4807",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 857,
                Name = "PHARMASAVE HEALTH CENTRE #1008 - METROTOWN",
                Address = "4390 Beresford St, Burnaby BC V5H 0E7 Canada",
                ManagerName = "Nisarg Shah",
                Phone = "(604) 563-4390",
                Fax = "(604) 563-4391",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 858,
                Name = "PHARMASAVE HEALTH CENTRE #1010",
                Address = "304 - 1750 Pier Mac Way, Kelowna BC V1V 3E7 Canada",
                ManagerName = "Sara Marshall",
                Phone = "(778) 484-8960",
                Fax = "(778) 484-8964",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 859,
                Name = "PHARMASAVE HEALTH CENTRE #1011",
                Address = "101-2359 Clearbrook Rd, Abbotsford BC V2T 2X6 Canada",
                ManagerName = "Kuljeet Thiara",
                Phone = "(778) 880-7011",
                Fax = "(778) 880-7012",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 860,
                Name = "PHARMASAVE HEALTH CENTRE #1022",
                Address = "#1 - 1530 West 7th Avenue, Vancouver BC V6J 1S2 Canada",
                ManagerName = "Audrey Fung",
                Phone = "(604) 738-7181",
                Fax = "(604) 738-7127",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 861,
                Name = "PHARMASAVE HEALTH CENTRE #1023",
                Address = "22338 Selkirk Ave, Maple Ridge BC V2X 2X5 Canada",
                ManagerName = "Laura Walker",
                Phone = "(604) 477-1666",
                Fax = "(604) 477-1672",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 862,
                Name = "PHARMASAVE JAMES BAY #130",
                Address = "113-230 Menzies St, Victoria BC V8V 2G7 Canada",
                ManagerName = "Raj Shah",
                Phone = "(250) 383-7196",
                Fax = "(250) 383-7186",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 863,
                Name = "PHARMASAVE LIONS GATE",
                Address = "152 E 13th Street, North Vancouver BC V7L 4W8 Canada",
                ManagerName = "Kyle Denley",
                Phone = "(778) 340-1018",
                Fax = "1-833-952-0979",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 864,
                Name = "PHARMASAVE NO. 008",
                Address = "#110 - 13798 - 94A Ave., Surrey BC V3V 1N1 Canada",
                ManagerName = "Amin Nanji",
                Phone = "(604) 588-9888",
                Fax = "(604) 583-5402",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 865,
                Name = "PHARMASAVE NORTHWOODS VILLAGE",
                Address = "2150 Dollarton Hwy, North Vancouver BC V7H 0B5 Canada",
                ManagerName = "Kevin Liew",
                Phone = "(604) 988-0562",
                Fax = "(833) 888-1641",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 866,
                Name = "PHARMASAVE OAK BAY #152",
                Address = "2200 Oak Bay Ave., Victoria BC V8R 1G3 Canada",
                ManagerName = "Steven Chauvin",
                Phone = "(250) 598-3380",
                Fax = "(250) 598-9820",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 867,
                Name = "PHARMASAVE OLD MILL PLAZA #178",
                Address = "Old Mill Plaza, 155 Main St, Lillooet BC V0K 1V0 Canada",
                ManagerName = "Carmen Pallot",
                Phone = "(250) 256-4262",
                Fax = "(250) 256-0362",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 868,
                Name = "PHARMASAVE RIVERBEND #1033",
                Address = "3-760 Mayfair Street, Kamloops BC V2B 0E5 Canada",
                ManagerName = "Jodi Fisher",
                Phone = "236-421-4263",
                Fax = "236-421-4431",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 869,
                Name = "PHARMASAVE SCOTT TOWN #167",
                Address = "9558 - 120th St, Surrey BC V3V 4C1 Canada",
                ManagerName = "Gurroop Dhaliwal",
                Phone = "(604) 581-4671",
                Fax = "(604) 581-4673",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 870,
                Name = "PHARMASAVE STEVESTON VILLAGE",
                Address = "105 - 12420 No. 1 Rd, Richmond BC V7E 6N2 Canada",
                ManagerName = "Hank Lin",
                Phone = "(604) 232-0159",
                Fax = "(604) 232-0526",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 871,
                Name = "PHARMASAVE WILLOWBROOK #1017",
                Address = "301 - 20055 Willowbrook Dr, Langley BC V2Y 2T5 Canada",
                ManagerName = "Nagy Ellahham",
                Phone = "(604) 427-0090",
                Fax = "(604) 427-0695",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 872,
                Name = "PHARMATRUST PHARMACY #2",
                Address = "178 Keefer Street, Vancouver BC V6A1X4 Canada",
                ManagerName = "Henry Tung",
                Phone = "(604) 694-0988",
                Fax = "(604) 694-0933",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 873,
                Name = "PHARMAWISE PHARMACY",
                Address = "101 - 8488 160 St., Surrey BC V4N 0V7 Canada",
                ManagerName = "Harpreet Sandhu",
                Phone = "778-565-0600",
                Fax = "778-565-0700",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 874,
                Name = "PHOENIX PHARMACY",
                Address = "#103 - 2155 - 10th Ave., Prince George BC V2M 5J6 Canada",
                ManagerName = "Christopher Pallot",
                Phone = "(250) 562-3383",
                Fax = "(250) 562-5113",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 875,
                Name = "PHOENIX PHARMASAVE #37",
                Address = "990 West Broadway, Vancouver BC V5Z 1K7 Canada",
                ManagerName = "Nelli Jakac",
                Phone = "(604) 873-9277",
                Fax = "(604) 873-9270",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 876,
                Name = "PIER HEALTH RESOURCE CENTRE",
                Address = "223 Main St, Vancouver BC V6A 2S7 Canada",
                ManagerName = "Sara Bucholtz",
                Phone = "(604) 891-1480",
                Fax = "(604) 891-1490",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 877,
                Name = "PILL4ME",
                Address = "#101 - 5625 Promontory Rd, Chilliwack BC V2R 4M5 Canada",
                ManagerName = "Bukola Ijatuyi",
                Phone = "604-705-3644",
                Fax = "604-705-3694",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 878,
                Name = "PILLARS PHARMACY",
                Address = "3030 Skaha Lake Rd, Penticton BC V2A 7H2 Canada",
                ManagerName = "Cameron Needham",
                Phone = "(250) 488-9360",
                Fax = "(250) 488-9363",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 879,
                Name = "PILLOSOPHY PHARMACY",
                Address = "1502 E Hastings St, Vancouver BC V5L 1S5 Canada",
                ManagerName = "David Bae",
                Phone = "(236) 521-2262",
                Fax = "(778) 783-9190",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 880,
                Name = "PILLWAY",
                Address = "104 - 7063 Venture St, Delta BC V4G 1H8 Canada",
                ManagerName = "Kurt Kramer",
                Phone = "1 (833) 745-5929",
                Fax = "(604) 630-4949",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 881,
                Name = "PINETREE PHARMACY",
                Address = "103 - 3007 Glen Dr, Coquitlam BC V3B 0L8 Canada",
                ManagerName = "Danqing Su",
                Phone = "(604) 474-4977",
                Fax = "(604) 474-4978",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 882,
                Name = "PINNACLE PHARMACY",
                Address = "3 - 5771 Turner Rd, Nanaimo BC V9T 6L8 Canada",
                ManagerName = "Deryn Edgett",
                Phone = "(250) 585-1980",
                Fax = "(250) 585-1017",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 883,
                Name = "PIVOT RX",
                Address = "3309 30th Ave, Vernon BC V1T 2C9 Canada",
                ManagerName = "Pascal Coombs",
                Phone = "(250) 503-1344",
                Fax = "(250) 503-2350",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 884,
                Name = "PIVOT RX 2",
                Address = "413 Tranquille Rd, Kamloops BC V2B 3G9 Canada",
                ManagerName = "Joseph Falsetta",
                Phone = "(250) 376-0358",
                Fax = "(250) 376-0425",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 885,
                Name = "PIX PHARMACY",
                Address = "203-19365 22 Ave, Surrey BC V3Z 3S6 Canada",
                ManagerName = "Dawson Bremner",
                Phone = "(888) 244-8998",
                Fax = "(888) 526-0408",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 886,
                Name = "PLANET HEALTH PHARMACY",
                Address = "160 - 28040 Fraser Hwy, Abbotsford BC V4X 0C1 Canada",
                ManagerName = "Varun Parikh",
                Phone = "(604) 743-1695",
                Fax = "(778) 360-2982",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 887,
                Name = "PLATINUM CARE PHARMACY",
                Address = "3 - 3238 King George Blvd, Surrey BC V4P 1A5 Canada",
                ManagerName = "Inderjit Hundal",
                Phone = "(604) 385-0188",
                Fax = "(604) 385-0189",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 888,
                Name = "PLAZA PHARMACY",
                Address = "#174 - 2655 Clearbrook Rd, Abbotsford BC V2T 2Y6 Canada",
                ManagerName = "Charnjit Herr",
                Phone = "(604) 504-4460",
                Fax = "(604) 504-4465",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 889,
                Name = "PLITIO",
                Address = "116 - 2455 Dollarton Hwy, North Vancouver BC V7H 0A2 Canada",
                ManagerName = "Jason Chan",
                Phone = "(778) 279-3901",
                Fax = "(778) 279-3868",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 890,
                Name = "POCKETPILLS PHARMACY",
                Address = "UNIT 107/108, 5433 152 St, Surrey BC V3S 5A5 Canada",
                ManagerName = "Min Woo Kim",
                Phone = "(855) 950-7225",
                Fax = "(855) 950-7226",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 891,
                Name = "POINT GREY PHARMACY",
                Address = "4516 West 10th Ave., Vancouver BC V6R 2J1 Canada",
                ManagerName = "Safouh El Rayes",
                Phone = "(604) 224-1377",
                Fax = "(604) 225-0019",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 892,
                Name = "PORT ALICE PHARMACY",
                Address = "1031 Marine Dr, Port Alice BC V0N 2N0 Canada",
                ManagerName = "Abdolsamad Chalangarian",
                Phone = "(250) 284-0261",
                Fax = "(250) 284-0264",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 893,
                Name = "POSTRX WEST",
                Address = "13988 Maycrest Way Unit 150, Richmond BC V6V 3C3 Canada",
                ManagerName = "James Wigston",
                Phone = "(236) 312-7300",
                Fax = "(236) 312-7270",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 894,
                Name = "POWELL RIVER PHARMACY",
                Address = "4280 Joyce Ave, Powell River BC V8A 3A2 Canada",
                ManagerName = "Alaa Amara",
                Phone = "(604) 489-9272",
                Fax = "(604) 489-9273",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 895,
                Name = "PRATT'S COMPOUNDING PHARMACY INC.",
                Address = "#100 - 321 Nicola St., Kamloops BC V2C 6G6 Canada",
                ManagerName = "Christopher Cameron",
                Phone = "(250) 374-7226",
                Fax = "(250) 851-0776",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 896,
                Name = "PRICESMART FOODS PHARMACY #2274 - RICHMOND",
                Address = "8200 Ackroyd Road, Richmond BC V6X 1B5 Canada",
                ManagerName = "Patway Yeung",
                Phone = "(604) 278-8408",
                Fax = "(604) 278-7227",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 897,
                Name = "PRICESMART FOODS PHARMACY #2280",
                Address = "9899 Austin Road, Burnaby BC V3J 0L6 Canada",
                ManagerName = "Andrew Kim",
                Phone = "(604) 899-1357",
                Fax = "(236) 455-5092",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 898,
                Name = "PRICESMART FOODS PHARMACY #2281 - STATION SQUARE",
                Address = "4650 Kingsway, Burnaby BC V5H 4L9 Canada",
                ManagerName = "Ada Lui",
                Phone = "(604) 433-3760",
                Fax = "(604) 433-7109",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 899,
                Name = "PRIME CURE PHARMACY",
                Address = "#7 - 8388 128 Street, Surrey BC V3W 4G2 Canada",
                ManagerName = "Anand Kapoor",
                Phone = "778-761-3131",
                Fax = "778-761-3132",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 900,
                Name = "PRIME CURE PHARMACY NO. 2",
                Address = "#103/104 - 12030 80 Ave, Surrey BC V3W 3M1 Canada",
                ManagerName = "Maneet Sanghera",
                Phone = "(604) 242-0323",
                Fax = "(604) 242-0324",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 901,
                Name = "PRIME WEST PHARMACY",
                Address = "13990 92 Ave, Surrey BC V3V 1J4 Canada",
                ManagerName = "Amrik Singh",
                Phone = "(778) 359-1313",
                Fax = "(778) 368-0069",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 902,
                Name = "PRIME WEST PHARMACY #3",
                Address = "106 - 9547 152 Street, Surrey BC V3R 5Y5 Canada",
                ManagerName = "Jaspreet Kainth",
                Phone = "(604) 515-4422",
                Fax = "(604) 930-3501",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 903,
                Name = "PRIMECARE PHARMACY",
                Address = "Unit 101/102, 7093 King George Blvd, Surrey BC V3W 5A2 Canada",
                ManagerName = "Kiran Arora",
                Phone = "(778) 654-0013",
                Fax = "(778) 654-0039",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 904,
                Name = "PRINCETON PHARMACY",
                Address = "#3 - 136 Tapton Ave, Princeton BC V0X 1W0 Canada",
                ManagerName = "Robert New",
                Phone = "(250) 295-7670",
                Fax = "(250) 295-7650",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 905,
                Name = "PRO-HEALTH PHARMACY #2",
                Address = "3 - 8580 Young Road, Chilliwack BC V2P 6Z8 Canada",
                ManagerName = "Rudy Langstaff",
                Phone = "(604) 845-8084",
                Fax = "(833) 487-1053",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 906,
                Name = "PRO-HEALTH PHARMACY #3",
                Address = "1 - 7900 Lochside Dr, Saanichton BC V8M 0B9 Canada",
                ManagerName = "Jaskirt Chawdhary",
                Phone = "(250) 544-5222",
                Fax = "(236) 916-2983",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 907,
                Name = "PROMONTORY PHARMACY",
                Address = "105 - 5615 Teskey Way, Chilliwack BC V2R 0K5 Canada",
                ManagerName = "Silvia Poelstra Porra",
                Phone = "(604) 705-3800",
                Fax = "(604) 705-3801",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 908,
                Name = "PROSPER PHARMACY 24",
                Address = "Unit 2 & 3 - 12818 72 Ave, Surrey BC V3W 2M9 Canada",
                ManagerName = "Inderpreet Bains",
                Phone = "(604) 543-6677",
                Fax = "(604) 543-4433",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 909,
                Name = "PULSE PHARMACY",
                Address = "2 - 19567 64 Ave, Surrey BC V3S 7H8 Canada",
                ManagerName = "Jasbir Lail",
                Phone = "(236) 477-4362",
                Fax = "(604) 634-7570",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 910,
                Name = "PURE CURE PHARMACY",
                Address = "#100 - 8820 120th Street, Surrey BC V3V 0C9 Canada",
                ManagerName = "Aditya Patel",
                Phone = "236-598-8896",
                Fax = "236-598-8895",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 911,
                Name = "PURE INTEGRATIVE PHARMACY # 1",
                Address = "111 - 15388 24 Ave, Surrey BC V4A 2J2 Canada",
                ManagerName = "Halyna Hritzkiv",
                Phone = "(604) 542-7780",
                Fax = "(604) 542-3263",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 912,
                Name = "PURE INTEGRATIVE PHARMACY # 4",
                Address = "3533 4th Ave W, Vancouver BC V6R 1N9 Canada",
                ManagerName = "Gabriel Mason",
                Phone = "(604) 733-7211",
                Fax = "(604) 733-7215",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 913,
                Name = "PURE INTEGRATIVE PHARMACY # 5",
                Address = "2685 Broadway W, Vancouver BC V6K 2G2 Canada",
                ManagerName = "Trevor Chan",
                Phone = "(604) 568-8844",
                Fax = "(604) 568-8944",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 914,
                Name = "PURE INTEGRATIVE PHARMACY # 9",
                Address = "238 Robson St, Vancouver BC V6B 6A1 Canada",
                ManagerName = "Bita Sabetmoghaddam",
                Phone = "(604) 681-8190",
                Fax = "(604) 681-8195",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 915,
                Name = "PURE INTEGRATIVE PHARMACY #10",
                Address = "3228 Dunbar Street, Vancouver BC V6S2B7 Canada",
                ManagerName = "Mahshid Poursartip",
                Phone = "(604) 732-3010",
                Fax = "(604) 732-3011",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 916,
                Name = "PURE INTEGRATIVE PHARMACY #17",
                Address = "Hollyburn Plaza, #117 - 1760 Marine Dr, West Vancouver BC V7V 1J4 Canada",
                ManagerName = "Masoud Majlesi",
                Phone = "(604) 281-3393",
                Fax = "(604) 281-3392",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 917,
                Name = "PURE INTEGRATIVE PHARMACY #18",
                Address = "3750 Oak St, Vancouver BC V6H 2M3 Canada",
                ManagerName = "Milosz Makarewicz",
                Phone = "(604) 731-8535",
                Fax = "(604) 731-8534",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 918,
                Name = "PURE INTEGRATIVE PHARMACY #19",
                Address = "103 - 3053 Edgemont Blvd, North Vancouver BC V7R 2N5 Canada",
                ManagerName = "Anthea Law",
                Phone = "(604) 770-3501",
                Fax = "(604) 770-3503",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 919,
                Name = "PURE INTEGRATIVE PHARMACY #20",
                Address = "130 - 13711 Mayfield Place, Richmond BC V6V 2G9 Canada",
                ManagerName = "Gurbir Ahluwalia",
                Phone = "(604) 565-7873",
                Fax = "(604) 428-3426",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 920,
                Name = "PURE INTEGRATIVE PHARMACY #21",
                Address = "#101 - 301 Columbia St E, New Westminster BC V3L 3W5 Canada",
                ManagerName = "Jinna Park",
                Phone = "(604) 553-7145",
                Fax = "(604) 553-7146",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 921,
                Name = "PURE INTEGRATIVE PHARMACY #23",
                Address = "210 - 2425 Hemlock St, Vancouver BC V6H 4E1 Canada",
                ManagerName = "Rebecca San",
                Phone = "(604) 559-9200",
                Fax = "(604) 559-9201",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 922,
                Name = "PURE INTEGRATIVE PHARMACY #24",
                Address = "4444 10th Ave W, Vancouver BC V6R 2H9 Canada",
                ManagerName = "Shirin Saffari",
                Phone = "(604) 563-4888",
                Fax = "(604) 563-4810",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 923,
                Name = "PURE INTEGRATIVE PHARMACY #25",
                Address = "#102 - 23242 Mavis Ave, Fort Langley BC V1M 2R4 Canada",
                ManagerName = "Lindsey Moncey",
                Phone = "(604) 371-1828",
                Fax = "(604) 371-2066",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 924,
                Name = "PURE INTEGRATIVE PHARMACY #26",
                Address = "104 - 382 Lerwick Rd, Courtenay BC V9N 9E5 Canada",
                ManagerName = "Gregory Ouellette",
                Phone = "(250) 871-7900",
                Fax = "(250) 871-7901",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 925,
                Name = "PURE INTEGRATIVE PHARMACY #27",
                Address = "102-1318 Marine Dr, West Vancouver BC V7T 1B5 Canada",
                ManagerName = "Maral Hajigholamreza",
                Phone = "(604) 281-3784",
                Fax = "(604) 281-3785",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 926,
                Name = "PURE INTEGRATIVE PHARMACY #29",
                Address = "102 - 88 Lonsdale Avenue, North Vancouver BC V7M 2E6 Canada",
                ManagerName = "Elaha Shakirin",
                Phone = "(604) 770-3390",
                Fax = "(604) 770-3391",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 927,
                Name = "PURE INTEGRATIVE PHARMACY #31",
                Address = "102 - 557 Superior Street, Victoria BC V8V 0E4 Canada",
                ManagerName = "Edwin Sy",
                Phone = "(778) 405-0860",
                Fax = "(778) 405-0859",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 928,
                Name = "PURE INTEGRATIVE PHARMACY #33",
                Address = "1240 56 Street, Delta BC V4L 2A4 Canada",
                ManagerName = "Kristine Horner",
                Phone = "(778) 729-7873",
                Fax = "(778) 729-7874",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 929,
                Name = "PURE INTEGRATIVE PHARMACY #34",
                Address = "16 - 2949 Main St, Vancouver BC V5T 3G4 Canada",
                ManagerName = "Adeline Sin",
                Phone = "(604) 879-1885",
                Fax = "(604) 879-1887",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 930,
                Name = "PURE INTEGRATIVE PHARMACY #35",
                Address = "130-2320 Acadia Rd, Vancouver BC V6T 0E3 Canada",
                ManagerName = "Deanna Lam",
                Phone = "(604) 874-4248",
                Fax = "(604) 874-4246",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 931,
                Name = "PURE INTEGRATIVE PHARMACY #36",
                Address = "101-345A Latoria Blvd, Victoria BC V9C 0S9 Canada",
                ManagerName = "Jennifer Eggen",
                Phone = "(778) 247-7873",
                Fax = "(778) 247-7874",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 932,
                Name = "PURE INTEGRATIVE PHARMACY #39",
                Address = "630 -1200 Hunter Pl, Squamish BC V8B 0G8 Canada",
                ManagerName = "Ondrej Machotka",
                Phone = "(604) 892-0226",
                Fax = "(604) 892-0171",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 933,
                Name = "PURE INTEGRATIVE PHARMACY #40",
                Address = "A-319 Island Highway E, Parksville BC V9P 2G9 Canada",
                ManagerName = "Liisa Stover",
                Phone = "(250) 947-8001",
                Fax = "(250) 947-7142",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 934,
                Name = "PURE INTEGRATIVE PHARMACY #42",
                Address = "1285 Hornby St, Vancouver BC V6Z 1W4 Canada",
                ManagerName = "Jason Wilhelm",
                Phone = "(778) 730-7873",
                Fax = "(778) 730-7874",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 935,
                Name = "PURE INTEGRATIVE PHARMACY #43",
                Address = "137 East 13th Street, North Vancouver BC V7L 2L3 Canada",
                ManagerName = "Farshad Felfelian",
                Phone = "(604) 770-3554",
                Fax = "(604) 770-3556",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 936,
                Name = "PURE INTEGRATIVE PHARMACY #44",
                Address = "1-370 Davis Rd, Ladysmith BC V9G 1T9 Canada",
                ManagerName = "Li Lin",
                Phone = "(778) 841-7873",
                Fax = "(778) 841-7874",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 937,
                Name = "PURE INTEGRATIVE PHARMACY #72",
                Address = "Unit 10 - 1380 Summit Dr, Kamloops BC V2C 1T8 Canada",
                ManagerName = "Aaron Glover",
                Phone = "(250) 851-3131",
                Fax = "(250) 851-3133",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 938,
                Name = "PURPLE PHARMACY",
                Address = "2 - 32618 Logan Avenue, Mission BC V2V 6C7 Canada",
                ManagerName = "Cynthia Chase",
                Phone = "(604) 287-2245",
                Fax = "(604) 287-2255",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 939,
                Name = "QUADRA VILLAGE DRUG MART",
                Address = "3 - 2631 Quadra St, Victoria BC V8T 4E3 Canada",
                ManagerName = "Brian Martindale",
                Phone = "(250) 383-1188",
                Fax = "(250) 383-4563",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 940,
                Name = "QUALITY CARE DRUGSTORE",
                Address = "5169 Argyle St, Port Alberni BC V9Y 1V3 Canada",
                ManagerName = "Tahereh Torki Baghbaderani",
                Phone = "(250) 724-1353",
                Fax = "(250) 724-1333",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 941,
                Name = "QUEENSBOROUGH COMMUNITY PHARMACY",
                Address = "1028 Ewen Ave, New Westminster BC V3M 5E1 Canada",
                ManagerName = "Rawinder Dhasi",
                Phone = "(778) 397-1132",
                Fax = "(778) 397-1129",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 942,
                Name = "R.E.A.C.H. CENTRE PHARMACY",
                Address = "1145 Commercial Drive, Vancouver BC V5L 3X3 Canada",
                ManagerName = "Melodie Tong",
                Phone = "(604) 216-3136",
                Fax = "(604) 215-1315",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 943,
                Name = "RAGE'S PHARMACY",
                Address = "166 - 1848 Main St, Penticton BC V2A 5H3 Canada",
                ManagerName = "Melissa Machial",
                Phone = "(250) 493-7200",
                Fax = "(250) 493-7201",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 944,
                Name = "RAINDROP PHARMACY",
                Address = "#170 - 1200 W 73rd Ave, Vancouver BC V6P 6G5 Canada",
                ManagerName = "Michael Xu",
                Phone = "(604) 373-9099",
                Fax = "(604) 373-9099",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 945,
                Name = "RAINFOREST WELLNESS PHARMACY",
                Address = "327 City Centre, Kitimat BC V8C 1T6 Canada",
                ManagerName = "Paul Sherman",
                Phone = "(250) 632-2914",
                Fax = "(250) 632-2750",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 946,
                Name = "REGENCY MEDICINE CENTRE",
                Address = "#100 - 6091 Gilbert Road, Richmond BC V7C 5L9 Canada",
                ManagerName = "Adam Smollan",
                Phone = "(604) 273-5544",
                Fax = "(604) 273-5037",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 947,
                Name = "REID STREET PHARMACY LTD.",
                Address = "359 Reid Street, Quesnel BC V2J 2M5 Canada",
                ManagerName = "Joshua Evanson",
                Phone = "(250) 985-0557",
                Fax = "(250) 985-0535",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 948,
                Name = "REMEDY'SRX #263",
                Address = "102 - 5796 Glover Rd, Langley BC V3A 4H9 Canada",
                ManagerName = "Sandra Shaw",
                Phone = "(604) 534-8686",
                Fax = "(604) 534-8383",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 949,
                Name = "REMEDY'SRX #298",
                Address = "#102 - 22112 - 52nd Ave, Langley BC V2Y 2M6 Canada",
                ManagerName = "Heather Wiersma",
                Phone = "(604) 534-6600",
                Fax = "(604) 534-6076",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 950,
                Name = "REMEDY'SRX #367",
                Address = "D/C Hospital, 310 - 2268 Pandosy St, Kelowna BC V1Y 1T2 Canada",
                ManagerName = "Glenda Dela Cruz",
                Phone = "(778) 484-3836",
                Fax = "(778) 484-3837",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 951,
                Name = "REVELSTOKE FAMILY PHARMACY",
                Address = "Alpine Village Shopp Ctr., Unit 12 - 555 Victoria Rd, Revelstoke BC V0E 2S0 Canada",
                ManagerName = "David Lafreniere",
                Phone = "(250) 837-5191",
                Fax = "(250) 837-5658",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 952,
                Name = "REXALL #7102",
                Address = "206 Port Augusta St., Comox BC V9M 3N1 Canada",
                ManagerName = "Hana Benko-Smalley",
                Phone = "(250) 339-2235",
                Fax = "(250) 339-2230",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 953,
                Name = "REXALL #7103",
                Address = "1511 Admirals Rd, Victoria BC V9A 2P8 Canada",
                ManagerName = "Mijeong Kim",
                Phone = "(250) 385-1800",
                Fax = "(250) 385-1870",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 954,
                Name = "REXALL #7104",
                Address = "8925 Granville Street, Box 190, Port Hardy BC V0N 2P0 Canada",
                ManagerName = "Queenie Lau",
                Phone = "(250) 949-6552",
                Fax = "(250) 949-6598",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 955,
                Name = "REXALL #7110",
                Address = "Rm.108 - 1015 Austin Ave., Coquitlam BC V3K 3N9 Canada",
                ManagerName = "Zahra Morovatdar",
                Phone = "(604) 937-3122",
                Fax = "(604) 937-3143",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 956,
                Name = "REXALL #7111",
                Address = "9762 Willow St., Chemainus BC V0R 1K0 Canada",
                ManagerName = "Christopher Kolek",
                Phone = "(250) 246-3821",
                Fax = "(250) 246-2579",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 957,
                Name = "REXALL #7114",
                Address = "Unit 1 - 9892 Esplanade St, Chemainus BC V0R 1K1 Canada",
                ManagerName = "Mohamed Osman",
                Phone = "(250) 246-2151",
                Fax = "(250) 246-3511",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 958,
                Name = "REXALL #7116",
                Address = "201-4204 Village Sq, Whistler BC V8E 1H5 Canada",
                ManagerName = "Linda Weigel",
                Phone = "(604) 932-4251",
                Fax = "(604) 932-6324",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 959,
                Name = "REXALL #7121",
                Address = "912 Douglas St, Victoria BC V8W 2C1 Canada",
                ManagerName = "Firas Madanat",
                Phone = "(250) 384-1195",
                Fax = "(250) 384-8794",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 960,
                Name = "REXALL #7122",
                Address = "101-1801 Princeton Kamloops Hwy, Kamloops BC V2E 2J7 Canada",
                ManagerName = "Jennifer Perozak",
                Phone = "(250) 372-2207",
                Fax = "(250) 372-2257",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 961,
                Name = "REXALL #7126",
                Address = "103 - 1646 McKenzie Ave, Victoria BC V8N 0A3 Canada",
                ManagerName = "Jeff Ho",
                Phone = "(250) 477-2225",
                Fax = "(250) 477-2285",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 962,
                Name = "REXALL #7127",
                Address = "101 - 230 Cook St, Victoria BC V8V 3X3 Canada",
                ManagerName = "Yifan Gong",
                Phone = "(250) 386-6171",
                Fax = "(250) 386-7659",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 963,
                Name = "REXALL #7129",
                Address = "Victoria Med-Dental Bldg., #101 - 1120 Yates St., Victoria BC V8V 3M9 Canada",
                ManagerName = "Zohreh Khalili Samani",
                Phone = "(250) 385-7701",
                Fax = "(250) 385-9672",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 964,
                Name = "REXALL #7136",
                Address = "Unit D - 4794 Joyce Ave, Powell River BC V8A 3B6 Canada",
                ManagerName = "Cornelia Yeung",
                Phone = "(604) 485-2929",
                Fax = "(604) 485-2924",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 965,
                Name = "REXALL #7137",
                Address = "#107 - 575 16th St, West Vancouver BC V7V 4Y1 Canada",
                ManagerName = "Allan Yang",
                Phone = "(604) 922-4174",
                Fax = "(604) 913-3159",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 966,
                Name = "REXALL #7141",
                Address = "#150 - 13655 Fraser Hwy, Surrey BC V3T 0P8 Canada",
                ManagerName = "Ranvir Suddi",
                Phone = "(778) 368-0526",
                Fax = "(778) 368-0528",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 967,
                Name = "REXALL #7149",
                Address = "#122 - 1055 Georgia St W, Vancouver BC V6E 3P3 Canada",
                ManagerName = "Framin Mark",
                Phone = "(604) 684-8204",
                Fax = "(604) 684-7329",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 968,
                Name = "REXALL #7157",
                Address = "150 - 17475 No 10 (56 Ave) Hwy, Surrey BC V3S 2X6 Canada",
                ManagerName = "Sarb Sihota",
                Phone = "(604) 576-7823",
                Fax = "(604) 576-7829",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 969,
                Name = "REXALL #7159",
                Address = "6580 Fraser St, Vancouver BC V5X 3T3 Canada",
                ManagerName = "Brian Chow",
                Phone = "(604) 235-2115",
                Fax = "(604) 235-2118",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 970,
                Name = "REXALL #7161",
                Address = "110 - 32471 Lougheed Hwy, Mission BC V2V 0C8 Canada",
                ManagerName = "Jake Jeong",
                Phone = "(604) 820-2128",
                Fax = "(604) 820-2146",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 971,
                Name = "REXALL #7163",
                Address = "Station Square, 140 - 6200 McKay Ave, Burnaby BC V5H 4L7 Canada",
                ManagerName = "Erica Tsai",
                Phone = "(604) 438-9370",
                Fax = "(604) 438-9375",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 972,
                Name = "REXALL #7167",
                Address = "107 - 15331 16 Ave., Surrey BC V4A 1R6 Canada",
                ManagerName = "Elaine Xu",
                Phone = "(604) 536-4211",
                Fax = "(604) 536-6864",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 973,
                Name = "REXALL #7169",
                Address = "Ocean Park Shopping Ctr., 12851 - 16th Ave., Surrey BC V4A 1N5 Canada",
                ManagerName = "Jenny Creus-Winship",
                Phone = "(604) 536-7611",
                Fax = "(604) 536-4261",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 974,
                Name = "REXALL #7170",
                Address = "#102 - 15451 Russell Ave, White Rock BC V4B 2R5 Canada",
                ManagerName = "Chengyuan Bo",
                Phone = "(604) 536-8225",
                Fax = "(604) 536-8405",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 975,
                Name = "REXALL #7171",
                Address = "1463 Johnston Rd, White Rock BC V4B 3Z4 Canada",
                ManagerName = "Maricor Pascua",
                Phone = "(604) 531-4636",
                Fax = "(604) 531-9299",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 976,
                Name = "REXALL #7174",
                Address = "499 Granville St, Vancouver BC V6C 1T1 Canada",
                ManagerName = "Manroop Dhaliwal",
                Phone = "(604) 801-6991",
                Fax = "(604) 801-6997",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 977,
                Name = "REXALL #7175",
                Address = "Victoria Medical Bldg., 1669 Victoria Street, Prince George BC V2L 2L5 Canada",
                ManagerName = "Stephen Schien",
                Phone = "(250) 564-6666",
                Fax = "(250) 562-5677",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 978,
                Name = "REXALL #7177",
                Address = "101-7143 West Saanich Rd, Brentwood Bay BC V8M 1P7 Canada",
                ManagerName = "Mike March",
                Phone = "(250) 652-8813",
                Fax = "(250) 652-8537",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 979,
                Name = "REXALL #7181",
                Address = "1750 Dufferin Cres, Nanaimo BC V9S 0A4 Canada",
                ManagerName = "Carmen Troje",
                Phone = "(250) 753-6655",
                Fax = "(250) 753-8945",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 980,
                Name = "REXALL #7184",
                Address = "#7 - 200 Burrard St, Vancouver BC V6C 3L6 Canada",
                ManagerName = "Sophie Park",
                Phone = "(604) 681-2195",
                Fax = "(604) 681-2708",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 981,
                Name = "REXALL #7191",
                Address = "Southgate Centre, #116 - 50 - 10th Street, Nanaimo BC V9R 6L1 Canada",
                ManagerName = "Nikhilesh Meraiya",
                Phone = "(250) 753-7195",
                Fax = "(250) 753-9862",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 982,
                Name = "REXALL #7193",
                Address = "2241 Louie Dr, Westbank BC V4T 3K3 Canada",
                ManagerName = "Jonathan Waslen",
                Phone = "(250) 768-1459",
                Fax = "(250) 768-4038",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 983,
                Name = "REXALL #7194",
                Address = "1 - 9200 Mary St., Chilliwack BC V2P 4H6 Canada",
                ManagerName = "Marina Bishara",
                Phone = "(604) 792-7334",
                Fax = "(604) 792-7708",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 984,
                Name = "REXALL DIRECT",
                Address = "1003 - 7495 132 St, Surrey BC V3W 1J8 Canada",
                ManagerName = "Michael Engelberts",
                Phone = "1-888-792-3667",
                Fax = "1-800-563-8934",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 985,
                Name = "REXALL DRUG STORE #7117",
                Address = "#103 - 4360 Lorimer Road, RR 4, Whistler BC V8E 1A5 Canada",
                ManagerName = "Lynnette Chiu",
                Phone = "(604) 932-2303",
                Fax = "(604) 932-9755",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 986,
                Name = "REXALL DRUG STORE #7139",
                Address = "102 - 1880 Island Hwy, Victoria BC V9B 1J2 Canada",
                ManagerName = "Dean Ourdev",
                Phone = "(250) 478-1735",
                Fax = "(250) 478-5508",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 987,
                Name = "REXALL DRUG STORE #7152",
                Address = "#107/108 - 15966 108 Ave, Surrey BC V4N 5V6 Canada",
                ManagerName = "Rui Yi Chen",
                Phone = "(604) 588-8330",
                Fax = "(604) 588-8342",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 988,
                Name = "REXALL DRUG STORE #7182",
                Address = "#1173 - 88 Pender St W, Vancouver BC V6B 6N9 Canada",
                ManagerName = "Linda Young",
                Phone = "(604) 683-4244",
                Fax = "(604) 683-4248",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 989,
                Name = "RIDGE MEADOWS PHARMACY",
                Address = "106 - 11743 224th Street, Maple Ridge BC V2X 6A4 Canada",
                ManagerName = "Naser Esbati",
                Phone = "(604) 463-4771",
                Fax = "(604) 463-4770",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 990,
                Name = "RIDGEWAY PHARMACY",
                Address = "1057 Ridgeway Ave., Coquitlam BC V3J 1S6 Canada",
                ManagerName = "Owen Lee",
                Phone = "(604) 931-5252",
                Fax = "(604) 931-8300",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 991,
                Name = "RISE CHC PHARMACY",
                Address = "5198 Joyce St, Vancouver BC V5R 4H1 Canada",
                ManagerName = "Lunia Que",
                Phone = "(604) 558-8090",
                Fax = "(778) 653-3558",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 992,
                Name = "RITECARE PHARMACY",
                Address = "#103 - 12837 88 Ave, Surrey BC V3W 3K2 Canada",
                ManagerName = "Shiv Sharma",
                Phone = "(778) 565-5925",
                Fax = "(778) 565-5926",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 993,
                Name = "RITECARE PHARMACY #2",
                Address = "110 - 18438 64 Ave, Surrey BC V3S 1E9 Canada",
                ManagerName = "Ravinder Puri",
                Phone = "(604) 372-3388",
                Fax = "(604) 372-3387",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 994,
                Name = "ROBIN'S PHARMACY",
                Address = "908 Commercial Dr, Vancouver BC V5L 3W7 Canada",
                ManagerName = "Kimberly Chin",
                Phone = "(604) 876-3784",
                Fax = "(604) 876-3766",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 995,
                Name = "ROSE PHARMACY",
                Address = "1483 Marine Dr, West Vancouver BC V7T 1B8 Canada",
                ManagerName = "Frough Khakpour",
                Phone = "(604) 281-4199",
                Fax = "(604) 281-4198",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 996,
                Name = "ROSE VALLEY PHARMACY",
                Address = "102 - 1135 Stevens Rd, West Kelowna BC V1Z 2S8 Canada",
                ManagerName = "Brandon Shul",
                Phone = "(778) 755-6715",
                Fax = "(778) 755-6153",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 997,
                Name = "ROYAL DRUGS PHARMACY",
                Address = "#130 - 8061 Lougheed Hwy, Burnaby BC V5A 1W9 Canada",
                ManagerName = "Vivian Lee",
                Phone = "(604) 294-1500",
                Fax = "(604) 299-3940",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 998,
                Name = "ROYAL JUBILEE PRESCRIPTIONS",
                Address = "DT 1200 - 1952 Bay St, Victoria BC V8R 1J8 Canada",
                ManagerName = "Maia Kozak",
                Phone = "(250) 370-8153",
                Fax = "(250) 519-1823",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 999,
                Name = "RUTLAND MEDICAL PHARMACY",
                Address = "#203 - 285 Aurora Cres., Kelowna BC V1X 7N6 Canada",
                ManagerName = "Steven Hopp",
                Phone = "(778) 753-7070",
                Fax = "(778) 753-7071",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1000,
                Name = "RX COUNTER",
                Address = "138 - 11860 Hammersmith Way, Richmond BC V7A 5G1 Canada",
                ManagerName = "Wilfred Mak",
                Phone = "(604) 275-3279",
                Fax = "(604) 275-2976",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1001,
                Name = "RX DRUG MART #1000",
                Address = "418 Yellowhead Hwy, Burns Lake BC V0J 1E0 Canada",
                ManagerName = "Elham Rafighi",
                Phone = "(250) 692-7531",
                Fax = "(250) 692-7398",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1002,
                Name = "RX DRUG MART #1001",
                Address = "188 East Stewart St, Vanderhoof BC V0J 3A0 Canada",
                ManagerName = "Mehdi Mirzaei",
                Phone = "(250) 567-2281",
                Fax = "(250) 567-3934",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1003,
                Name = "RX DRUG MART #1002",
                Address = "5740 Teredo St, Sechelt BC V0N 3A0 Canada",
                ManagerName = "Karina Penner",
                Phone = "(604) 885-9833",
                Fax = "(604) 885-1071",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1004,
                Name = "RX OUTREACH PHARMACY",
                Address = "5 - 1080 Cliveden Ave, Delta BC V3M 6G6 Canada",
                ManagerName = "Brenda O'Leary",
                Phone = "(604) 515-4088",
                Fax = "(604) 515-9717",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1005,
                Name = "RX PEOPLES PHARMACY",
                Address = "7 - 22214 Dewdney Trunk Rd, Maple Ridge BC V2X 0E6 Canada",
                ManagerName = "Ethan Kim",
                Phone = "(604) 479-1661",
                Fax = "(604) 479-1494",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1006,
                Name = "RX PHARMACHOICE PHARMACY",
                Address = "503 - 22259 48 Ave, Langley BC V3A 8T1 Canada",
                ManagerName = "Shaymaa Elbaharia",
                Phone = "(604) 534-2841",
                Fax = "(604) 534-3250",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1007,
                Name = "RX RAM PHARMACY SPECIALISTS (MILLSTONE)",
                Address = "104 - 1621 Dufferin Cres, Nanaimo BC V9S 5T4 Canada",
                ManagerName = "David Ram",
                Phone = "(250) 591-0144",
                Fax = "(250) 591-0145",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1008,
                Name = "RXCARE PHARMACY & COMPOUNDING",
                Address = "4695 Canada Way, Burnaby BC V5G 1K9 Canada",
                ManagerName = "Saideh Ozgur",
                Phone = "(604) 428-7911",
                Fax = "(604) 428-7912",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1009,
                Name = "SAFEMED PHARMACY MARINE DRIVE",
                Address = "746 Marine Dr, North Vancouver BC V7M 1H3 Canada",
                ManagerName = "Parisa Hosseini",
                Phone = "(604) 770-4414",
                Fax = "(604) 770-4415",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1010,
                Name = "SAFEMED PHARMACY MOUNTAIN HIGHWAY",
                Address = "102 - 467 Mountain Highway, North Vancouver BC V7J 2L3 Canada",
                ManagerName = "Radfar Mousavi",
                Phone = "(604) 980-4446",
                Fax = "(604) 980-4448",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1011,
                Name = "SAFEWAY PHARMACY #4900",
                Address = "10388 City Parkway, Unit 102 and 103, Surrey BC V3T 4Y8 Canada",
                ManagerName = "Thomas Chan",
                Phone = "(604) 584-7812",
                Fax = "(604) 584-7197",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1012,
                Name = "SAFEWAY PHARMACY #4901",
                Address = "2733 West Broadway, Vancouver BC V6K 2G5 Canada",
                ManagerName = "Jennifer Woo",
                Phone = "(604) 732-5030",
                Fax = "(604) 732-5722",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1013,
                Name = "SAFEWAY PHARMACY #4903",
                Address = "6401 - 120th Street, Delta BC V4E 3G3 Canada",
                ManagerName = "Punam Sandhu",
                Phone = "(604) 596-5634",
                Fax = "(604) 590-8296",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1014,
                Name = "SAFEWAY PHARMACY #4905",
                Address = "Westview Shopping Centre, #780 - 2601 Westview Dr, North Vancouver BC V7N 3X4 Canada",
                ManagerName = "Guissou Adl Golchin",
                Phone = "(604) 988-4476",
                Fax = "(604) 988-5853",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1015,
                Name = "SAFEWAY PHARMACY #4908",
                Address = "1766 Robson St, Vancouver BC V6G 1E2 Canada",
                ManagerName = "Tri Nguyen",
                Phone = "(604) 683-0202",
                Fax = "(604) 683-5057",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1016,
                Name = "SAFEWAY PHARMACY #4909",
                Address = "5385 Headland Drive, West Vancouver BC V7W 3E7 Canada",
                ManagerName = "Mohamed Nathoo",
                Phone = "(604) 926-2034",
                Fax = "(604) 926-2539",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1017,
                Name = "SAFEWAY PHARMACY #4911",
                Address = "4440 Hastings St, Burnaby BC V5C 2K2 Canada",
                ManagerName = "Meiling Liu",
                Phone = "(604) 205-7497",
                Fax = "(604) 205-5876",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1018,
                Name = "SAFEWAY PHARMACY #4912",
                Address = "20871 Fraser Hwy, Langley BC V3A 4G7 Canada",
                ManagerName = "Nabiha Murad Agha",
                Phone = "(604) 534-4245",
                Fax = "(604) 534-4276",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1019,
                Name = "SAFEWAY PHARMACY #4913",
                Address = "6564 Hastings St., Burnaby BC V5B 1S2 Canada",
                ManagerName = "Clara Lee",
                Phone = "(604) 291-0118",
                Fax = "(604) 291-1339",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1020,
                Name = "SAFEWAY PHARMACY #4914",
                Address = "#700 - 15355 - 24th Ave, Surrey BC V4A 2H9 Canada",
                ManagerName = "Chihyun Kim",
                Phone = "(604) 535-8879",
                Fax = "(833) 376-3147",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1021,
                Name = "SAFEWAY PHARMACY #4916",
                Address = "750 Fortune Drive, Kamloops BC V2B 2L2 Canada",
                ManagerName = "Pouyan Ghandi",
                Phone = "(250) 376-9672",
                Fax = "(250) 376-7701",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1022,
                Name = "SAFEWAY PHARMACY #4917",
                Address = "B 800 McBride Blvd, New Westminster BC V3L 2B8 Canada",
                ManagerName = "Bin Gao",
                Phone = "(604) 516-6547",
                Fax = "(604) 516-6247",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1023,
                Name = "SAFEWAY PHARMACY #4918",
                Address = "6153 200th Street, Langley BC V2Y 1A2 Canada",
                ManagerName = "Randi Vose",
                Phone = "(604) 530-6131",
                Fax = "(604) 530-9117",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1024,
                Name = "SAFEWAY PHARMACY #4919",
                Address = "4300  32 St, Vernon BC V1T 9H1 Canada",
                ManagerName = "Anmol Sooch",
                Phone = "(250) 542-0313",
                Fax = "(250) 542-2166",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1025,
                Name = "SAFEWAY PHARMACY #4920",
                Address = "8870 - 152nd Street, Surrey BC V3R 4E4 Canada",
                ManagerName = "Sukhdeep Gill",
                Phone = "(604) 589-5226",
                Fax = "(604) 589-3717",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1026,
                Name = "SAFEWAY PHARMACY #4924",
                Address = "1599 - 2nd Avenue, Trail BC V1R 1M3 Canada",
                ManagerName = "Idaylia Swanson",
                Phone = "(250) 368-3790",
                Fax = "(250) 368-3513",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1027,
                Name = "SAFEWAY PHARMACY #4925",
                Address = "445 Reid Street, Quesnel BC V2J 2M7 Canada",
                ManagerName = "Victoria Obanye",
                Phone = "(250) 992-6898",
                Fax = "(250) 992-6147",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1028,
                Name = "SAFEWAY PHARMACY #4928",
                Address = "200  2nd Ave W, Prince Rupert BC V8J 1G5 Canada",
                ManagerName = "Robert Hays",
                Phone = "(250) 627-8129",
                Fax = "(250) 627-4971",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1029,
                Name = "SAFEWAY PHARMACY #4930",
                Address = "#220 - 800 Carnarvon St, New Westminster BC V3M 0G3 Canada",
                ManagerName = "Alex Brown",
                Phone = "(604) 522-2069",
                Fax = "(604) 522-2843",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1030,
                Name = "SAFEWAY PHARMACY #4931",
                Address = "3410 Kingsway, Vancouver BC V5R 5L4 Canada",
                ManagerName = "Jordan Wang",
                Phone = "(604) 439-1050",
                Fax = "(604) 439-9611",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1031,
                Name = "SAFEWAY PHARMACY #4936",
                Address = "#1100 - 2850 Shaughnessy St., Port Coquitlam BC V3C 6K5 Canada",
                ManagerName = "Sunae Min",
                Phone = "(604) 945-7018",
                Fax = "(604) 945-6242",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1032,
                Name = "SAFEWAY PHARMACY #4939",
                Address = "12825 - 16th Ave, Surrey BC V4A 1N5 Canada",
                ManagerName = "Dominika Kincer",
                Phone = "(604) 531-9694",
                Fax = "(604) 531-2602",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1033,
                Name = "SAFEWAY PHARMACY #4941",
                Address = "990 W King Edward Ave, Vancouver BC V5Z 2E2 Canada",
                ManagerName = "Monica Tanaka",
                Phone = "(604) 733-9342",
                Fax = "(604) 733-7551",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1034,
                Name = "SAFEWAY PHARMACY #4946",
                Address = "4655 Lakelse Ave., Terrace BC V8G 1R3 Canada",
                ManagerName = "Eric Durando",
                Phone = "(250) 635-1375",
                Fax = "(250) 635-9079",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1035,
                Name = "SAFEWAY PHARMACY #4948",
                Address = "211 Anderson St, Nelson BC V1L 3X8 Canada",
                ManagerName = "Brody Blair",
                Phone = "(250) 352-7765",
                Fax = "(250) 352-5755",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1036,
                Name = "SAFEWAY PHARMACY #4950",
                Address = "Westlynn Mall, 1170 East 27th Street, North Vancouver BC V7J 1S1 Canada",
                ManagerName = "Edward Cheung",
                Phone = "(604) 988-7095",
                Fax = "(604) 980-8272",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1037,
                Name = "SAFEWAY PHARMACY #4952",
                Address = "1450 King Street, Smithers BC V0J 2N0 Canada",
                ManagerName = "Dustin Schibli",
                Phone = "(250) 847-4744",
                Fax = "(250) 847-1748",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1038,
                Name = "SAFEWAY PHARMACY #4955",
                Address = "#801 - 1301 Main St, Penticton BC V2A 5E9 Canada",
                ManagerName = "Jennifer Fox",
                Phone = "(250) 493-2433",
                Fax = "(250) 493-7570",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1039,
                Name = "SAFEWAY PHARMACY #4958",
                Address = "1175 Mt. Seymour Road, North Vancouver BC V7H 2Y4 Canada",
                ManagerName = "Roshan Amiriara",
                Phone = "(604) 924-1325",
                Fax = "(604) 929-3153",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1040,
                Name = "SAFEWAY PHARMACY #4960",
                Address = "697 Bernard Ave., Kelowna BC V1Y 6P4 Canada",
                ManagerName = "Nick Fleming",
                Phone = "(250) 860-0583",
                Fax = "(250) 860-9166",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1041,
                Name = "SAFEWAY PHARMACY #4966",
                Address = "1780 East Broadway, Vancouver BC V5N 1W3 Canada",
                ManagerName = "Anna Cheung",
                Phone = "(604) 879-0505",
                Fax = "(604) 873-6144",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1042,
                Name = "SAFEWAY PHARMACY #4967",
                Address = "8671 No. 1 Road, Richmond BC V7C 1V2 Canada",
                ManagerName = "Florence Lau",
                Phone = "(604) 241-4013",
                Fax = "1-(833)-376-3451",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1043,
                Name = "SAFEWAY PHARMACY #4968",
                Address = "Chilliwack Mall, 200 45610 Luckakuck Way, Chilliwack BC V2R 1A2 Canada",
                ManagerName = "Adewole Monebi",
                Phone = "(604) 858-0437",
                Fax = "(604) 858-3116",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1044,
                Name = "SAFEWAY PHARMACY #4970",
                Address = "1721 Columbia Ave, Castlegar BC V1N 2W6 Canada",
                ManagerName = "Tiffany Ryan",
                Phone = "(250) 365-7141",
                Fax = "(250) 365-7175",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1045,
                Name = "SAFEWAY PHARMACY #4972",
                Address = "11216 8 St, Dawson Creek BC V1G 3R4 Canada",
                ManagerName = "Adele Douanla Saadio",
                Phone = "(250) 782-9561",
                Fax = "(250) 782-9728",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1046,
                Name = "SAFEWAY PHARMACY #4973",
                Address = "9123 - 100 St, Fort St. John BC V1J 3X3 Canada",
                ManagerName = "Shubam Sachdeva",
                Phone = "(250) 261-5479",
                Fax = "(250) 261-5480",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1047,
                Name = "SAFEWAY PHARMACY #4974",
                Address = "1200 Baker Street, Cranbrook BC V1C 1A8 Canada",
                ManagerName = "Lori Kobe",
                Phone = "(250) 417-0221",
                Fax = "(250) 417-0277",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1048,
                Name = "SAFEWAY PHARMACY #4976",
                Address = "8475 Granville St, Vancouver BC V6P 4Z9 Canada",
                ManagerName = "David Dong",
                Phone = "(604) 263-7267",
                Fax = "(604) 263-5075",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1049,
                Name = "SAFEWAY PHARMACY #4977",
                Address = "580 Clarke Rd, Coquitlam BC V3J 3X5 Canada",
                ManagerName = "Albert Wong",
                Phone = "(604) 931-0111",
                Fax = "(604) 931-0116",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1050,
                Name = "SAFEWAY PHARMACY #4979",
                Address = "2101 Lahb Avenue, Vancouver BC V6L 0B9 Canada",
                ManagerName = "Ted Mah",
                Phone = "(604) 731-9611",
                Fax = "(604) 731-6730",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1051,
                Name = "SAFEWAY PHARMACY #4980",
                Address = "1033 Austin Ave, Coquitlam BC V3K 3P2 Canada",
                ManagerName = "Jennifer Yeh",
                Phone = "(604) 939-1764",
                Fax = "(604) 939-7007",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1052,
                Name = "SAFEWAY PHARMACY #4998",
                Address = "1611 Davie St, Vancouver BC V6G 1W1 Canada",
                ManagerName = "Nindy Badesha",
                Phone = "(604) 669-8131",
                Fax = "(604) 669-0779",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1053,
                Name = "SAGE PHARMACY",
                Address = "102 - 245 E Columbia St, New Westminster BC V3L 3W4 Canada",
                ManagerName = "Samy Elsisi",
                Phone = "(604) 901-0987",
                Fax = "(604) 901-1050",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1054,
                Name = "SAIGON PHARMACY",
                Address = "1080 Kingsway, Vancouver BC V5V 3C6 Canada",
                ManagerName = "Hien Huynh",
                Phone = "(604) 872-6708",
                Fax = "(604) 874-6708",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1055,
                Name = "SALMO VALLEY PHARMACY",
                Address = "107 - 4th Street, Salmo BC V0G 1Z0 Canada",
                ManagerName = "Lindsay Swanson",
                Phone = "(250) 357-9444",
                Fax = "(888) 715-1813",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1056,
                Name = "SALMON ARM PRESCRIPTION CENTRE",
                Address = "581B Hudson Ave NE, Salmon Arm BC V1E 4P1 Canada",
                ManagerName = "Daryl Neufeld",
                Phone = "(250) 804-0700",
                Fax = "(250) 804-0790",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1057,
                Name = "SALT SPRING PHARMACY",
                Address = "2101 - 115 Fulford-Ganges Rd, Salt Spring Island BC V8K 2T9 Canada",
                ManagerName = "Elizabeth O'Connell",
                Phone = "(250) 931-7774",
                Fax = "(250) 931-8874",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1058,
                Name = "SAM'S PHARMACY LTD.",
                Address = "465 Main St., Vancouver BC V6A 2T7 Canada",
                ManagerName = "Sammy Kam",
                Phone = "(604) 688-6323",
                Fax = "(604) 688-6323",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1059,
                Name = "SARDIS PHARMACY",
                Address = "7 - 7201 Vedder Road, Chilliwack BC V2R 4G5 Canada",
                ManagerName = "Mostafa El Hennawy",
                Phone = "(604) 705-1030",
                Fax = "(604) 705-1031",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1060,
                Name = "SATNAM PHARMACY",
                Address = "Satnam Plaza, #115 - 7130 120 St, Surrey BC V3W 3M8 Canada",
                ManagerName = "Vipra Sethi",
                Phone = "(604) 597-5947",
                Fax = "(604) 597-5945",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1061,
                Name = "SAVE-ON FOODS PHARMACY # 954",
                Address = "818 Island Hwy W, Parksville BC V9P 2B7 Canada",
                ManagerName = "Heeyoun Park",
                Phone = "(250) 248-3260",
                Fax = "(250) 248-4012",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1062,
                Name = "SAVE-ON-FOODS PHARMACY # 902 - ALDERGROVE",
                Address = "100 - 26310 Fraser Highway, Langley BC V4W 2Z7 Canada",
                ManagerName = "Milkah Soriano",
                Phone = "(604) 607-6550",
                Fax = "(604) 607-6557",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1063,
                Name = "SAVE-ON-FOODS PHARMACY # 903 - SURREY",
                Address = "South Point Exchange, 3033 - 152nd Street, Surrey BC V4P 3K1 Canada",
                ManagerName = "Jennifer Liou",
                Phone = "(604) 538-5467",
                Fax = "(604) 538-6451",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1064,
                Name = "SAVE-ON-FOODS PHARMACY # 904 - ABBOTSFORD",
                Address = "2140 Sumas Way, Abbotsford BC V2S 2C7 Canada",
                ManagerName = "Tony Shin",
                Phone = "(604) 504-3041",
                Fax = "(604) 504-4023",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1065,
                Name = "SAVE-ON-FOODS PHARMACY # 907 - HIGHGATE",
                Address = "200 - 7155 Kingsway, Burnaby BC V5E 2V1 Canada",
                ManagerName = "Benny Lam",
                Phone = "(604) 540-1389",
                Fax = "(604) 540-1452",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1066,
                Name = "SAVE-ON-FOODS PHARMACY # 909 - PORT ALBERNI",
                Address = "3756 - 10th Ave., Port Alberni BC V9Y 4W6 Canada",
                ManagerName = "Kathryn Michelle Esquivel",
                Phone = "(250) 723-6204",
                Fax = "(250) 723-1664",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1067,
                Name = "SAVE-ON-FOODS PHARMACY # 910 - NEWTON",
                Address = "100 - 7320 King George Blvd, Surrey BC V3W 5A5 Canada",
                ManagerName = "Yuan Gao",
                Phone = "(604) 599-6702",
                Fax = "(604) 599-6703",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1068,
                Name = "SAVE-ON-FOODS PHARMACY # 915 - CRANBROOK",
                Address = "505 Victoria Ave N, Cranbrook BC V1C 6S3 Canada",
                ManagerName = "Marc Wilson",
                Phone = "(250) 489-5711",
                Fax = "(250) 489-3358",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1069,
                Name = "SAVE-ON-FOODS PHARMACY # 916 - SAPPERTON",
                Address = "270 Columbia St E, New Westminster BC V3L 0E3 Canada",
                ManagerName = "Doris Lee",
                Phone = "(604) 523-2583",
                Fax = "(604) 523-2584",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1070,
                Name = "SAVE-ON-FOODS PHARMACY # 918",
                Address = "9014 - 152nd St, Surrey BC V3R 4E7 Canada",
                ManagerName = "George Ko",
                Phone = "(604) 930-1120",
                Fax = "(604) 582-4852",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1071,
                Name = "SAVE-ON-FOODS PHARMACY # 919 - ROYAL CITY",
                Address = "Lower Level, 198 - 610 Sixth St, New Westminster BC V3L 3C2 Canada",
                ManagerName = "Narmin Khimji",
                Phone = "(604) 520-6087",
                Fax = "(604) 515-9409",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1072,
                Name = "SAVE-ON-FOODS PHARMACY # 921 - FORT AND FOUL BAY",
                Address = "1950 Foul Bay Road, Victoria BC V8R 5A7 Canada",
                ManagerName = "Rachel Yang",
                Phone = "(250) 370-0772",
                Fax = "(250) 370-5155",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1073,
                Name = "SAVE-ON-FOODS PHARMACY # 922 - SIDNEY",
                Address = "2345 Beacon Ave, Sidney BC V8L 1W9 Canada",
                ManagerName = "Thomas Lebbetter",
                Phone = "(250) 656-6659",
                Fax = "(250) 656-3926",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1074,
                Name = "SAVE-ON-FOODS PHARMACY # 923 - TILLICUM",
                Address = "Tillicum Mall, 108 - 3170 Tillicum Road, Victoria BC V9A 7C5 Canada",
                ManagerName = "Trevor Leggat",
                Phone = "(250) 386-1641",
                Fax = "(250) 384-7785",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1075,
                Name = "SAVE-ON-FOODS PHARMACY # 928 - W. MAPLE RIDGE",
                Address = "#300 - 20395 Lougheed Highway, Maple Ridge BC V2X 2P9 Canada",
                ManagerName = "Carson Ko",
                Phone = "(604) 465-8606",
                Fax = "(604) 465-3652",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1076,
                Name = "SAVE-ON-FOODS PHARMACY # 929 - 100 MILE HOUSE",
                Address = "1-95 A Cariboo Highway 97, 100 Mile House BC V0K 2E0 Canada",
                ManagerName = "Rizza Jane Pimienta",
                Phone = "(250) 395-2139",
                Fax = "(250) 395-2031",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1077,
                Name = "SAVE-ON-FOODS PHARMACY # 930 - KITIMAT",
                Address = "535 Mountainview Sq, Kitimat BC V8C 2N1 Canada",
                ManagerName = "Cherry Paulo",
                Phone = "(250) 632-7262",
                Fax = "(250) 632-5193",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1078,
                Name = "SAVE-ON-FOODS PHARMACY # 931 - SAHALI",
                Address = "#100 - 1210 Summit Drive, Kamloops BC V2C 6M1 Canada",
                ManagerName = "Chalermlat Suktap",
                Phone = "(250) 374-5558",
                Fax = "(250) 374-5344",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1079,
                Name = "SAVE-ON-FOODS PHARMACY # 932 - LAKESHORE",
                Address = "3175 Lakeshore Road, Kelowna BC V1W 3S9 Canada",
                ManagerName = "Maria Christina Calayag",
                Phone = "(250) 860-6646",
                Fax = "(250) 860-0726",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1080,
                Name = "SAVE-ON-FOODS PHARMACY # 933 - DUNCAN",
                Address = "181 Trans Canada Hwy, Duncan BC V9L 3P8 Canada",
                ManagerName = "Manjit Dale",
                Phone = "(250) 746-3655",
                Fax = "(250) 746-3696",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1081,
                Name = "SAVE-ON-FOODS PHARMACY # 935 - E.MAPLE RIDGE",
                Address = "22703 Lougheed Hwy, Maple Ridge BC V2X 2V5 Canada",
                ManagerName = "Kevin Liew",
                Phone = "(604) 463-3329",
                Fax = "(604) 466-8266",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1082,
                Name = "SAVE-ON-FOODS PHARMACY # 936 - LADNER",
                Address = "5186 Ladner Trunk Rd, Ladner BC V4K 1W3 Canada",
                ManagerName = "Herman Ho",
                Phone = "(604) 946-4474",
                Fax = "(604) 946-5944",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1083,
                Name = "SAVE-ON-FOODS PHARMACY # 937 - 7TH & CAMBIE",
                Address = "2308 Cambie St, Vancouver BC V5Z 2T8 Canada",
                ManagerName = "Anil Kanji",
                Phone = "(604) 876-7085",
                Fax = "(604) 876-7086",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1084,
                Name = "SAVE-ON-FOODS PHARMACY # 939 - NORDEL CROSSING",
                Address = "12130 Nordel Way, Surrey BC V3W 1P6 Canada",
                ManagerName = "Sandeep Sekhon",
                Phone = "(604) 501-9354",
                Fax = "(604) 501-9364",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1085,
                Name = "SAVE-ON-FOODS PHARMACY # 940 - KING EDWARD",
                Address = "1403 King Edward Ave E, Vancouver BC V5N 5Z4 Canada",
                ManagerName = "Joey Chan",
                Phone = "(604) 874-9331",
                Fax = "(604) 874-9332",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1086,
                Name = "SAVE-ON-FOODS PHARMACY # 942 - PRAIRIE",
                Address = "1110-1470 Prairie Ave, Port Coquitlam BC V3B 5M8 Canada",
                ManagerName = "Maria Denice Bucsit",
                Phone = "(604) 464-5089",
                Fax = "(604) 464-5174",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1087,
                Name = "SAVE-ON-FOODS PHARMACY # 943 - TSAWWASSEN",
                Address = "1143 56 St, Delta BC V4L 2A2 Canada",
                ManagerName = "Kevin Lau",
                Phone = "(604) 943-0514",
                Fax = "(604) 943-5531",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1088,
                Name = "SAVE-ON-FOODS PHARMACY # 946 - MISSION",
                Address = "400 - 32555 London Ave, Mission BC V2V 6M7 Canada",
                ManagerName = "Anna Lissa Vergara",
                Phone = "(604) 820-7622",
                Fax = "(604) 820-0117",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1089,
                Name = "SAVE-ON-FOODS PHARMACY # 947 - BROOKS LANDING",
                Address = "130-2000 Island Hwy N, Nanaimo BC V9S 5W3 Canada",
                ManagerName = "Kate Han",
                Phone = "(250) 753-5865",
                Fax = "(250) 753-9722",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1090,
                Name = "SAVE-ON-FOODS PHARMACY # 948 - WOODGROVE",
                Address = "6901 Island Hwy N, Nanaimo BC V9T 6N8 Canada",
                ManagerName = "Richard Johnston",
                Phone = "(250) 390-4613",
                Fax = "(250) 390-4529",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1091,
                Name = "SAVE-ON-FOODS PHARMACY # 951 - COUNTRY CLUB",
                Address = "Country Club Mall, 3200 N. Island Hwy., Nanaimo BC V9T 1W1 Canada",
                ManagerName = "Shabnam Manhas",
                Phone = "(250) 751-1412",
                Fax = "(250) 751-1248",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1092,
                Name = "SAVE-ON-FOODS PHARMACY # 952 - SARDIS",
                Address = "Vedder Crossing Plaza, Unit 31 - 6014 Vedder Rd, Chilliwack BC V2R 5M4 Canada",
                ManagerName = "Melissa Santiago",
                Phone = "(604) 824-1106",
                Fax = "(604) 824-0556",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1093,
                Name = "SAVE-ON-FOODS PHARMACY # 955 - CAMPBELL RIVER",
                Address = "#400 - 1400 Dogwood St, Campbell River BC V9W 3A6 Canada",
                ManagerName = "Khaled Al Sous",
                Phone = "(250) 286-1532",
                Fax = "(250) 286-8887",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1094,
                Name = "SAVE-ON-FOODS PHARMACY # 956 - WESTBANK",
                Address = "#1 - 2475 Dobbin Rd., West Kelowna BC V4T 2E9 Canada",
                ManagerName = "Pooja Patel Kachhiyapatel",
                Phone = "(250) 768-2323",
                Fax = "(250) 768-2110",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1095,
                Name = "SAVE-ON-FOODS PHARMACY # 957 - PORT COQUITLAM",
                Address = "2385 Ottawa St, Port Coquitlam BC V3B 8A4 Canada",
                ManagerName = "Richard Chan",
                Phone = "(604) 464-5046",
                Fax = "(604) 464-3814",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1096,
                Name = "SAVE-ON-FOODS PHARMACY # 962 - PENTICTON",
                Address = "Cherry Lane Shopping Ctr., Unit 161 - 2111 Main Street, Penticton BC V2A 6W6 Canada",
                ManagerName = "Timothy Balo",
                Phone = "(250) 492-3455",
                Fax = "(250) 492-2745",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1097,
                Name = "SAVE-ON-FOODS PHARMACY # 963 - NORTH DELTA",
                Address = "Scottsdale Mall, 7015 - 120th St., Delta BC V4E 2A9 Canada",
                ManagerName = "Iqbal Bains",
                Phone = "(604) 596-7784",
                Fax = "(604) 596-9338",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1098,
                Name = "SAVE-ON-FOODS PHARMACY # 965 - COLLEGE HEIGHTS",
                Address = "5232 Domano Blvd, Prince George BC V2N 4A1 Canada",
                ManagerName = "Chad Harvey",
                Phone = "(250) 964-3839",
                Fax = "(250) 964-6701",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1099,
                Name = "SAVE-ON-FOODS PHARMACY # 967 - SPRUCELAND",
                Address = "555 Central St W, Prince George BC V2M 3C6 Canada",
                ManagerName = "Christine Santos",
                Phone = "(250) 564-2168",
                Fax = "(250) 564-6358",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1100,
                Name = "SAVE-ON-FOODS PHARMACY # 968 - HART HIGHWAY",
                Address = "3885 Austin Rd W, Prince George BC V2K 2H7 Canada",
                ManagerName = "Sreena Rajan",
                Phone = "(250) 962-2662",
                Fax = "(250) 962-5916",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1101,
                Name = "SAVE-ON-FOODS PHARMACY # 969 - IRONWOOD MALL",
                Address = "Ironwood Mall, #3000-11666 Steveston Hwy., Richmond BC V7A 5J3 Canada",
                ManagerName = "Kenny Choi",
                Phone = "(604) 448-1203",
                Fax = "(604) 448-1261",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1102,
                Name = "SAVE-ON-FOODS PHARMACY # 970 - PRINCE RUPERT",
                Address = "841 - 3rd Ave W, Prince Rupert BC V8J 1M7 Canada",
                ManagerName = "Richard Ron Nocon",
                Phone = "(250) 624-9032",
                Fax = "(250) 624-9055",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1103,
                Name = "SAVE-ON-FOODS PHARMACY # 971 - TERRA NOVA",
                Address = "Terra Nova Village, 3673 Westminster Highway, Richmond BC V7C 5V2 Canada",
                ManagerName = "Conway Wan",
                Phone = "(604) 273-3939",
                Fax = "(604) 273-5247",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1104,
                Name = "SAVE-ON-FOODS PHARMACY # 972 - QUESNEL",
                Address = "#7 - 155 Malcolm Dr, Quesnel BC V2J 3K2 Canada",
                ManagerName = "Katherine Del Rosario",
                Phone = "(250) 992-2291",
                Fax = "(250) 992-3691",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1105,
                Name = "SAVE-ON-FOODS PHARMACY # 973 - WESTSIDE",
                Address = "Westside Village, #100 - 172 Wilson St, Victoria BC V9A 7N6 Canada",
                ManagerName = "Steven Quon",
                Phone = "(250) 389-0131",
                Fax = "(250) 389-0673",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1106,
                Name = "SAVE-ON-FOODS PHARMACY # 975 - SALMON ARM",
                Address = "100 - 1151 10th Ave SW, Salmon Arm BC V1E 1T3 Canada",
                ManagerName = "Alida Boulianne",
                Phone = "(250) 832-6551",
                Fax = "(250) 832-6545",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1107,
                Name = "SAVE-ON-FOODS PHARMACY # 977 - SAANICH",
                Address = "3510 Blanshard St., Victoria BC V8X 1W3 Canada",
                ManagerName = "Vivian Leung",
                Phone = "(250) 475-3301",
                Fax = "(250) 475-1245",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1108,
                Name = "SAVE-ON-FOODS PHARMACY # 979 - SQUAMISH",
                Address = "1301 Pemberton Ave, Squamish BC V8B 0A1 Canada",
                ManagerName = "Jaspaul Kaila",
                Phone = "(604) 815-0743",
                Fax = "(604) 815-4318",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1109,
                Name = "SAVE-ON-FOODS PHARMACY # 980 - ORCHARD PLAZA",
                Address = "#101 - 1876 Cooper Rd, Kelowna BC V1Y 9N6 Canada",
                ManagerName = "Terralyn Gotro",
                Phone = "(250) 763-5510",
                Fax = "(250) 763-5347",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1110,
                Name = "SAVE-ON-FOODS PHARMACY # 981 - WHATCOM",
                Address = "2388 Whatcom Rd, Abbotsford BC V3G 0C1 Canada",
                ManagerName = "Maybelle Guevarra",
                Phone = "(604) 851-9626",
                Fax = "(604) 851-9627",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1111,
                Name = "SAVE-ON-FOODS PHARMACY # 982",
                Address = "792B - 2nd Ave, Fernie BC V0B 1M0 Canada",
                ManagerName = "Daryl Kay Yanez",
                Phone = "(250) 423-7704",
                Fax = "(250) 423-3916",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1112,
                Name = "SAVE-ON-FOODS PHARMACY # 983 - TERRACE",
                Address = "4731 Lakelse Ave, Terrace BC V8G 1R5 Canada",
                ManagerName = "Ana Laman",
                Phone = "(250) 635-4021",
                Fax = "(250) 635-3639",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1113,
                Name = "SAVE-ON-FOODS PHARMACY # 984 - WALNUT GROVE",
                Address = "8840 - 210th St, Langley BC V1M 2Y2 Canada",
                ManagerName = "Thomas Ling",
                Phone = "(604) 882-0883",
                Fax = "(604) 882-8521",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1114,
                Name = "SAVE-ON-FOODS PHARMACY # 985 - POWELL RIVER",
                Address = "Town Centre Shopping Mall, 3 - 7100 Alberni St, Powell River BC V8A 5K9 Canada",
                ManagerName = "Paul Macalintal",
                Phone = "(604) 485-2629",
                Fax = "(604) 485-0958",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1115,
                Name = "SAVE-ON-FOODS PHARMACY # 987 - WILLIAMS LAKE",
                Address = "730 Oliver Street, Williams Lake BC V2G 1N1 Canada",
                ManagerName = "Maria Corazon Del Rosario",
                Phone = "(250) 392-7266",
                Fax = "(250) 392-5839",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1116,
                Name = "SAVE-ON-FOODS PHARMACY # 988 - VERNON",
                Address = "245 4900 - 27th St, Vernon BC V1T 7G7 Canada",
                ManagerName = "Bradley Adams",
                Phone = "(250) 558-4854",
                Fax = "(250) 542-6895",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1117,
                Name = "SAVE-ON-FOODS PHARMACY # 990 - PARK & TILFORD",
                Address = "333 Brooksbank Ave. 600, North Vancouver BC V7J 3S8 Canada",
                ManagerName = "Brianna Truong",
                Phone = "(604) 983-2147",
                Fax = "(604) 983-0669",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1118,
                Name = "SAVE-ON-FOODS PHARMACY # 991 - AUSTIN",
                Address = "1 - 2662 Austin Ave, Coquitlam BC V3K 6C4 Canada",
                ManagerName = "Eun Sung Kim",
                Phone = "604-931-0503",
                Fax = "604-931-0679",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1119,
                Name = "SAVE-ON-FOODS PHARMACY # 992 - WILLOUGHBY",
                Address = "1 - 20255 - 64 Ave, Langley BC V2Y 1M9 Canada",
                ManagerName = "Aleena Hildebrand",
                Phone = "(604) 532-5833",
                Fax = "(604) 532-8671",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1120,
                Name = "SAVE-ON-FOODS PHARMACY # 993 - UBC",
                Address = "5945 Berton Ave, Vancouver BC V6S 0B3 Canada",
                ManagerName = "Lillian Yong",
                Phone = "(604) 221-5152",
                Fax = "(604) 221-5643",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1121,
                Name = "SAVE-ON-FOODS PHARMACY # 995 - PITT MEADOWS",
                Address = "122-19150 Lougheed Hwy, Pitt Meadows BC V3Y 2H6 Canada",
                ManagerName = "Gina Galindez",
                Phone = "(604) 465-0426",
                Fax = "(604) 465-0141",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1122,
                Name = "SAVE-ON-FOODS PHARMACY # 996 - MADISON",
                Address = "4399 Lougheed Hwy., Burnaby BC V5C 3Y7 Canada",
                ManagerName = "Maedeh Maghsoudiashtiani",
                Phone = "(604) 298-5173",
                Fax = "(604) 298-4891",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1123,
                Name = "SAVE-ON-FOODS PHARMACY # 999 - NORTH VANCOUVER",
                Address = "Pemberton Plaza, 1250 Marine Drive, North Vancouver BC V7P 1T2 Canada",
                ManagerName = "Anne Sison",
                Phone = "(604) 985-2150",
                Fax = "(604) 985-3779",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1124,
                Name = "SAVE-ON-FOODS PHARMACY #2202",
                Address = "1A - 11000 8th Street, Dawson Creek BC V1G 4K6 Canada",
                ManagerName = "Ramelynne Galicia",
                Phone = "(250) 719-0167",
                Fax = "(250) 719-0210",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1125,
                Name = "SAVE-ON-FOODS PHARMACY #2208 - CLEARBROOK",
                Address = "#300-32700 S. Fraser Way, Abbotsford BC V2T 4M5 Canada",
                ManagerName = "Mina Fahim",
                Phone = "(604) 854-6293",
                Fax = "(604) 852-2483",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1126,
                Name = "SAVE-ON-FOODS PHARMACY #2210 - WESTSYDE",
                Address = "18 - 3435 Westsyde Rd, Kamloops BC V2B 7H1 Canada",
                ManagerName = "Jenny Kennedy",
                Phone = "(250) 579-5218",
                Fax = "(250) 579-2297",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1127,
                Name = "SAVE-ON-FOODS PHARMACY #2213 - CHILLIWACK",
                Address = "46020 Yale Road, Chilliwack BC V2P 7V2 Canada",
                ManagerName = "Brandon Morton",
                Phone = "(604) 792-9156",
                Fax = "(604) 792-6487",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1128,
                Name = "SAVE-ON-FOODS PHARMACY #2214",
                Address = "441 Central Ave, Grand Forks BC V0H 1H0 Canada",
                ManagerName = "Janette Bowering",
                Phone = "(250) 442-3147",
                Fax = "(250) 442-3419",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1129,
                Name = "SAVE-ON-FOODS PHARMACY #2215 - CLOVERDALE",
                Address = "17745 64 Ave, Surrey BC V3S 1Z2 Canada",
                ManagerName = "Maria Cristina Cajipe",
                Phone = "(604) 575-7162",
                Fax = "(604) 575-7165",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1130,
                Name = "SAVE-ON-FOODS PHARMACY #2216",
                Address = "#3 - 1000 Northwest Blvd, Creston BC V0B 1G6 Canada",
                ManagerName = "Christine Hoffman",
                Phone = "(250) 428-0030",
                Fax = "(250) 428-9120",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1131,
                Name = "SAVE-ON-FOODS PHARMACY #2217",
                Address = "5104 Airport Dr, Fort Nelson BC V0C 1R0 Canada",
                ManagerName = "Shafiq Ur Rehman",
                Phone = "(250) 233-8914",
                Fax = "(250) 774-4103",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1132,
                Name = "SAVE-ON-FOODS PHARMACY #2221 - CAMERON",
                Address = "#102 - 3433 North Rd, Burnaby BC V3J 0A9 Canada",
                ManagerName = "Carmen Yan",
                Phone = "(604) 415-9992",
                Fax = "(604) 415-0288",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1133,
                Name = "SAVE-ON-FOODS PHARMACY #2224",
                Address = "600 - 1984 Kane Rd, Kelowna BC V1V 3C4 Canada",
                ManagerName = "Youngwoo Sohn",
                Phone = "(250) 712-9581",
                Fax = "(250) 712-9816",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1134,
                Name = "SAVE-ON-FOODS PHARMACY #2225",
                Address = "4469 Kingsway, Burnaby BC V5H 2A1 Canada",
                ManagerName = "Karen Jane Ang",
                Phone = "(604) 435-8283",
                Fax = "(604) 436-5131",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1135,
                Name = "SAVE-ON-FOODS PHARMACY #2227",
                Address = "759 McCallum Rd, Langford BC V9B 6A2 Canada",
                ManagerName = "Shaun Parmar",
                Phone = "(250) 475-0438",
                Fax = "(250) 475-0310",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1136,
                Name = "SAVE-ON-FOODS PHARMACY #2228 - MARINE WAY",
                Address = "7501 Market Crossing, Burnaby BC V5J 0A3 Canada",
                ManagerName = "Alnoor Suleman",
                Phone = "(604) 433-6314",
                Fax = "(604) 433-6814",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1137,
                Name = "SAVE-ON-FOODS PHARMACY #2231",
                Address = "#101 - 940 Frost Road, Kelowna BC V1W 0E4 Canada",
                ManagerName = "Rae Martina Gonzales",
                Phone = "(778) 940-1257",
                Fax = "(250) 764-3578",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1138,
                Name = "SAVE-ON-FOODS PHARMACY #2235 - WHITE ROCK",
                Address = "1641 152 St, Surrey BC V4A 4N3 Canada",
                ManagerName = "Gwendoline Jolicoeur",
                Phone = "(604) 536-6530",
                Fax = "(604) 536-9838",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1139,
                Name = "SAVE-ON-FOODS PHARMACY #2237",
                Address = "400 - 45585 Luckakuck Way, Chilliwack BC V2R 1A1 Canada",
                ManagerName = "Claudia Chan",
                Phone = "(604) 847-4348",
                Fax = "(604) 847-2235",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1140,
                Name = "SAVE-ON-FOODS PHARMACY #2242 - LANGLEY",
                Address = "100 - 20151 Fraser Highway, Langley BC V3A 4E4 Canada",
                ManagerName = "Azim Kassamali",
                Phone = "(604) 533-0400",
                Fax = "(604) 533-0362",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1141,
                Name = "SAVE-ON-FOODS PHARMACY #2244",
                Address = "8550 River District Crossing, Vancouver BC V5S 0E3 Canada",
                ManagerName = "Cynthia So",
                Phone = "604-438-3231",
                Fax = "604-432-1527",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1142,
                Name = "SAVE-ON-FOODS PHARMACY #2246",
                Address = "6455 West Boulevard, Vancouver BC V6M 3X6 Canada",
                ManagerName = "Grace Lilian Ang",
                Phone = "(604) 264-0980",
                Fax = "(604) 264-1646",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1143,
                Name = "SAVE-ON-FOODS PHARMACY #2249 - CAPILANO",
                Address = "140 - 879 Marine Dr, North Vancouver BC V7P 1R7 Canada",
                ManagerName = "Taylor Quon",
                Phone = "(604) 983-2299",
                Fax = "(604) 983-2279",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1144,
                Name = "SAVE-ON-FOODS PHARMACY #2250",
                Address = "1010 Pandora Ave, Victoria BC V8V 3P5 Canada",
                ManagerName = "Jonathan Hansen",
                Phone = "(236) 475-8645",
                Fax = "(236) 475-8642",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1145,
                Name = "SAVE-ON-FOODS PHARMACY #2252",
                Address = "120 - 12088 3rd Ave, Richmond BC V7E 0C3 Canada",
                ManagerName = "Grace Leung",
                Phone = "(604) 272-9741",
                Fax = "(604) 272-4408",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1146,
                Name = "SAVE-ON-FOODS PHARMACY #2255",
                Address = "15615 - 104th Ave, Surrey BC V4N 2H4 Canada",
                ManagerName = "Racquel Sese",
                Phone = "(604) 589-0187",
                Fax = "(604) 589-0188",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1147,
                Name = "SAVE-ON-FOODS PHARMACY #2263 - CLAYTON",
                Address = "Unit 2 - 18710 Fraser Hwy, Surrey BC V3S 7Y4 Canada",
                ManagerName = "Michael Wong",
                Phone = "(604) 574-1231",
                Fax = "(604) 574-1264",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1148,
                Name = "SAVE-ON-FOODS PHARMACY #2266",
                Address = "2999 Massey Drive, Prince George BC V2N 2S9 Canada",
                ManagerName = "Christopher Li",
                Phone = "(250) 561-0240",
                Fax = "(250) 561-0340",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1149,
                Name = "SAVE-ON-FOODS PHARMACY #2267",
                Address = "140-13630 George Junction, Surrey BC V3T 0P9 Canada",
                ManagerName = "Maria Cecilia Gozun",
                Phone = "(604) 588-6292",
                Fax = "(604) 930-4883",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1150,
                Name = "SAVE-ON-FOODS PHARMACY #2285",
                Address = "100 - 1913 Sooke Rd, Victoria BC V9B 1V8 Canada",
                ManagerName = "Christian Padilla",
                Phone = "(236) 475-8343",
                Fax = "(236) 475-8076",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1151,
                Name = "SAVE-ON-FOODS PHARMACY #2289",
                Address = "106 - 2770 Valley Centre Ave, North Vancouver BC V7J 0C8 Canada",
                ManagerName = "Lloyd Khaodhiar",
                Phone = "(604) 980-4658",
                Fax = "(604) 980-6972",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1152,
                Name = "SAVE-ON-FOODS PHARMACY #2290",
                Address = "100 - 3025 Lougheed Hwy, Coquitlam BC V3B 6S2 Canada",
                ManagerName = "Kent Ling",
                Phone = "(604) 464-8811",
                Fax = "(604) 552-4705",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1153,
                Name = "SAVE-ON-FOODS PHARMACY #624",
                Address = "C - 3945 Quadra St, Victoria BC V8X 1J5 Canada",
                ManagerName = "Kurt Wideski",
                Phone = "(250) 477-2522",
                Fax = "(250) 477-9059",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1154,
                Name = "SCOTT 75 PHARMACY",
                Address = "#113-7500 120 St, Surrey BC V3W 3N1",
                ManagerName = "Rajbir Bains",
                Phone = "(604) 503-8355",
                Fax = "(604) 503-8356",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1155,
                Name = "SCOTT ROAD PHARMACY",
                Address = "102 - 6905 120th St, Delta BC V4E 2A8 Canada",
                ManagerName = "Parmjit Rai",
                Phone = "(604) 591-5080",
                Fax = "(604) 394-2115",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1156,
                Name = "SCOTTCARE PHARMACY",
                Address = "Unit 202 - 9278 120 St, Surrey BC V3V 4B8 Canada",
                ManagerName = "Phani Damerla",
                Phone = "(604) 498-0856",
                Fax = "(604) 498-0857",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1157,
                Name = "SCRIPT CARE PHARMACY",
                Address = "114 - 6741 Cariboo Rd, Burnaby BC V3N 4A3 Canada",
                ManagerName = "Dominic Chiu",
                Phone = "(604) 415-9607",
                Fax = "(604) 415-9608",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1158,
                Name = "SEABIRD PHARMACY",
                Address = "2895 Chowat Rd, Agassiz BC V0M 1A2 Canada",
                ManagerName = "Ramez Istafanous",
                Phone = "(604) 491-4477",
                Fax = "(604) 491-4478",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1159,
                Name = "SECHELT PHARMACY",
                Address = "5648 Dolphin St, Sechelt BC V0N 3A0 Canada",
                ManagerName = "Rami Al Khatib",
                Phone = "(604) 740-8111",
                Fax = "(604) 740-8851",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1160,
                Name = "SENTREX PHARMACY",
                Address = "1696 West 75th Ave, Vancouver BC V6P 6G2 Canada",
                ManagerName = "Victorine Ssozi",
                Phone = "1-888-891-7539",
                Fax = "(778) 309-6230",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1161,
                Name = "SERVICE PLUS #3",
                Address = "Unit 1560 - 4380 No. 3 Rd, Richmond BC V6X 3V7 Canada",
                ManagerName = "Paco Chan",
                Phone = "(604) 278-8830",
                Fax = "(604) 279-8961",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1162,
                Name = "SHAUGHNESSY PHARMACY",
                Address = "1265 W Broadway, Vancouver BC V6H 1G7 Canada",
                ManagerName = "Ngai Li",
                Phone = "(604) 423-4246",
                Fax = "(604) 423-4376",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1163,
                Name = "SHAYONA PHARMACY",
                Address = "108 - 1656 Martin Drive, Surrey BC V4A 6E7 Canada",
                ManagerName = "Ketankumar Patel",
                Phone = "(778) 738-1119",
                Fax = "(778) 738-1109",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1164,
                Name = "SHOPPERS DRUG MART # 201",
                Address = "1020 Denman St., Vancouver BC V6G 2M5 Canada",
                ManagerName = "Kevin Huang",
                Phone = "(604) 681-3411",
                Fax = "(604) 681-3280",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1165,
                Name = "SHOPPERS DRUG MART # 202",
                Address = "2888 Granville St., Vancouver BC V6H 3J5 Canada",
                ManagerName = "Nico Wang",
                Phone = "(604) 738-3107",
                Fax = "(604) 738-3162",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1166,
                Name = "SHOPPERS DRUG MART # 203",
                Address = "#701 - 1301 Main St., Penticton BC V2A 5E9 Canada",
                ManagerName = "Sungtaek Huh",
                Phone = "(250) 492-8000",
                Fax = "(250) 492-6210",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1167,
                Name = "SHOPPERS DRUG MART # 204",
                Address = "Unit 100 - 370 East Broadway, Vancouver BC V5T 4G5 Canada",
                ManagerName = "Anthony Lee",
                Phone = "(604) 873-3558",
                Fax = "(604) 873-3501",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1168,
                Name = "SHOPPERS DRUG MART # 205",
                Address = "Sevenoaks Shopping Ctr., #143 - 32900 S. Fraser Way, Abbotsford BC V2S 5A1 Canada",
                ManagerName = "Alex Xu",
                Phone = "(604) 853-9481",
                Fax = "(604) 853-5900",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1169,
                Name = "SHOPPERS DRUG MART # 208",
                Address = "1627 Fort Street, Victoria BC V8R 1H8 Canada",
                ManagerName = "Kimberly Myers",
                Phone = "(250) 592-4541",
                Fax = "(250) 370-9149",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1170,
                Name = "SHOPPERS DRUG MART # 209",
                Address = "1297 Shoppers Row, Campbell River BC V9W 2C7 Canada",
                ManagerName = "Jimmy Hu",
                Phone = "(250) 286-1166",
                Fax = "(250) 287-4381",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1171,
                Name = "SHOPPERS DRUG MART # 210",
                Address = "Spruceland Shopping Ctr., 693 Central St. W, Prince George BC V2M 3C6 Canada",
                ManagerName = "Krishnaben Thakkar",
                Phone = "(250) 562-2311",
                Fax = "(250) 563-3034",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1172,
                Name = "SHOPPERS DRUG MART # 211",
                Address = "C-1215 56 St, Delta BC V4L 2A6 Canada",
                ManagerName = "Simranjit Singh",
                Phone = "(604) 943-1144",
                Fax = "(604) 943-8466",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1173,
                Name = "SHOPPERS DRUG MART # 212",
                Address = "1583 Marine Drive, West Vancouver BC V7V 1H9 Canada",
                ManagerName = "Lyndsie Lojpur",
                Phone = "(604) 922-1271",
                Fax = "(604) 922-3424",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1174,
                Name = "SHOPPERS DRUG MART # 213",
                Address = "225 St. Laurent Ave., Quesnel BC V2J 2C8 Canada",
                ManagerName = "Garry Solecki",
                Phone = "(250) 992-2214",
                Fax = "(250) 992-8870",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1175,
                Name = "SHOPPERS DRUG MART # 214",
                Address = "6508 Hastings St., Burnaby BC V5B 1S2 Canada",
                ManagerName = "Bojana Dzombeta",
                Phone = "(604) 291-0638",
                Fax = "(604) 291-6828",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1176,
                Name = "SHOPPERS DRUG MART # 216",
                Address = "Unit #22, 11000 8 St, Dawson Creek BC V1G 4K6 Canada",
                ManagerName = "Rudo Chinhoro",
                Phone = "(250) 782-5903",
                Fax = "(250) 782-5593",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1177,
                Name = "SHOPPERS DRUG MART # 217",
                Address = "32 - 45905 Yale Road, Chilliwack BC V2P 2M6 Canada",
                ManagerName = "Kirandeep Basran",
                Phone = "(604) 792-7377",
                Fax = "(604) 792-7307",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1178,
                Name = "SHOPPERS DRUG MART # 219",
                Address = "1339 Pemberton Ave, Squamish BC V8B 0J8 Canada",
                ManagerName = "John Cameron",
                Phone = "(604) 892-5258",
                Fax = "(604) 892-5251",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1179,
                Name = "SHOPPERS DRUG MART # 221",
                Address = "1212 Douglas St., Victoria BC V8W 2E5 Canada",
                ManagerName = "Diana Crossan",
                Phone = "(250) 384-0544",
                Fax = "(250) 384-8640",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1180,
                Name = "SHOPPERS DRUG MART # 222",
                Address = "6305 Fraser St., Vancouver BC V5W 3A3 Canada",
                ManagerName = "Angelique Fidel",
                Phone = "(604) 324-7909",
                Fax = "(604) 324-2405",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1181,
                Name = "SHOPPERS DRUG MART # 223",
                Address = "Unit #4000 - 6660 Sooke Road, Sooke BC V9Z 0A5 Canada",
                ManagerName = "Vadim Milchin",
                Phone = "(250) 642-5229",
                Fax = "(250) 642-2515",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1182,
                Name = "SHOPPERS DRUG MART # 225",
                Address = "Orchard Park Mall, #1300 - 2271 Harvey Ave., Kelowna BC V1Y 6H2 Canada",
                ManagerName = "Rishi Patel",
                Phone = "(250) 860-3764",
                Fax = "(250) 860-9104",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1183,
                Name = "SHOPPERS DRUG MART # 226",
                Address = "Hillside Shopping Centre, #126 - 1644 Hillside Ave., Victoria BC V8T 2C5 Canada",
                ManagerName = "Amy Carroll",
                Phone = "(250) 595-5111",
                Fax = "(250) 595-6459",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1184,
                Name = "SHOPPERS DRUG MART # 227",
                Address = "#11  - 8671 No. 1 Road, Richmond BC V7C 1V2 Canada",
                ManagerName = "Marzena Gray",
                Phone = "(604) 277-2611",
                Fax = "(604) 277-0173",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1185,
                Name = "SHOPPERS DRUG MART # 228",
                Address = "#380 - 9100 Blundell Road, Richmond BC V6Y 3X9 Canada",
                ManagerName = "Kam Rattanpal",
                Phone = "(604) 276-8757",
                Fax = "(604) 278-4435",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1186,
                Name = "SHOPPERS DRUG MART # 230",
                Address = "3417 - 31st Ave, Vernon BC V1T 2H6 Canada",
                ManagerName = "Ae Ra Moon",
                Phone = "(250) 542-3371",
                Fax = "(250) 542-6842",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1187,
                Name = "SHOPPERS DRUG MART # 231",
                Address = "108 - 1960 Como Lake Ave, Coquitlam BC V3J 3R3 Canada",
                ManagerName = "Sunny Park",
                Phone = "(604) 936-1433",
                Fax = "(604) 936-6148",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1188,
                Name = "SHOPPERS DRUG MART # 232",
                Address = "2303 West 41st Ave., Vancouver BC V6M 2A3 Canada",
                ManagerName = "Serena Lam",
                Phone = "(604) 266-5344",
                Fax = "(604) 266-5337",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1189,
                Name = "SHOPPERS DRUG MART # 233",
                Address = "3511 Blanshard St, Victoria BC V8Z 0B9 Canada",
                ManagerName = "Zakaria Al-Odatallah",
                Phone = "(250) 475-7572",
                Fax = "(250) 475-2681",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1190,
                Name = "SHOPPERS DRUG MART # 234",
                Address = "2302 West 4th Ave, Vancouver BC V6K 1P1 Canada",
                ManagerName = "Marco Cheung",
                Phone = "(604) 738-3138",
                Fax = "(604) 738-8693",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1191,
                Name = "SHOPPERS DRUG MART # 236",
                Address = "#152 - 8180 No. 2 Road, Richmond BC V7C 5K1 Canada",
                ManagerName = "Karen Ong",
                Phone = "(604) 274-3023",
                Fax = "(604) 271-0995",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1192,
                Name = "SHOPPERS DRUG MART # 237",
                Address = "2286 - 6060 Minoru Blvd, Richmond BC V6Y 2V7 Canada",
                ManagerName = "Kory Hu",
                Phone = "(604) 273-6187",
                Fax = "(604) 214-3714",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1193,
                Name = "SHOPPERS DRUG MART # 238",
                Address = "3020 Broadway W, Vancouver BC V6K 2H1 Canada",
                ManagerName = "Christopher Chan",
                Phone = "(604) 733-9128",
                Fax = "(604) 733-7964",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1194,
                Name = "SHOPPERS DRUG MART # 239",
                Address = "310 - 8 St, Courtenay BC V9N 1N3 Canada",
                ManagerName = "Ting-Yun Wei",
                Phone = "(250) 334-3134",
                Fax = "(250) 334-8066",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1195,
                Name = "SHOPPERS DRUG MART # 241",
                Address = "10108 Jubilee Road, Summerland BC V0H 1Z0 Canada",
                ManagerName = "Hojong Lee",
                Phone = "(250) 494-3155",
                Fax = "(250) 494-0733",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1196,
                Name = "SHOPPERS DRUG MART # 242",
                Address = "2337 Beacon Ave, Sidney BC V8L 1W9 Canada",
                ManagerName = "Rimsha Faisal",
                Phone = "(250) 656-1102",
                Fax = "(250) 655-3596",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1197,
                Name = "SHOPPERS DRUG MART # 243",
                Address = "Park Royal Shopping Ctr., 802 Main Street, West Vancouver BC V7T 2Y5 Canada",
                ManagerName = "Afrooz Sheikhi",
                Phone = "(604) 926-1114",
                Fax = "(604) 926-5717",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1198,
                Name = "SHOPPERS DRUG MART # 244",
                Address = "3717 10th Ave, Port Alberni BC V9Y 4W5 Canada",
                ManagerName = "Kris L'Heureux",
                Phone = "(250) 723-7387",
                Fax = "(250) 723-0686",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1199,
                Name = "SHOPPERS DRUG MART # 246",
                Address = "Old Orchard Shopping Ctr., 30 - 4429 Kingsway, Burnaby BC V5H 2A1 Canada",
                ManagerName = "Shorouk Elsayyad",
                Phone = "(604) 434-2408",
                Fax = "(604) 434-0476",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1200,
                Name = "SHOPPERS DRUG MART # 248",
                Address = "Piccadilly Place, #101 - 1151 - 10th Ave. S.W., Salmon Arm BC V1E 1T3 Canada",
                ManagerName = "Michael Huitema",
                Phone = "(250) 832-2181",
                Fax = "(250) 832-2147",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1201,
                Name = "SHOPPERS DRUG MART # 251",
                Address = "107 - 552 Clarke Rd, Coquitlam BC V3J 3X5 Canada",
                ManagerName = "Christina Park",
                Phone = "(604) 936-7255",
                Fax = "(604) 936-7294",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1202,
                Name = "SHOPPERS DRUG MART # 252",
                Address = "154 - 3650 Mt. Seymour Parkway, North Vancouver BC V7H 2Y5 Canada",
                ManagerName = "Nayan Darji",
                Phone = "(604) 924-1788",
                Fax = "(604) 924-0488",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1203,
                Name = "SHOPPERS DRUG MART # 253",
                Address = "#250 - 7155 Kingsway, Burnaby BC V5E 2V1 Canada",
                ManagerName = "Bilvinder Ahira",
                Phone = "(604) 526-2848",
                Fax = "(604) 526-1219",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1204,
                Name = "SHOPPERS DRUG MART # 254",
                Address = "361 Trans Canada Hwy, Duncan BC V9L 3R5 Canada",
                ManagerName = "Ramy Itaoui",
                Phone = "(250) 746-6118",
                Fax = "(250) 746-8861",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1205,
                Name = "SHOPPERS DRUG MART # 255",
                Address = "Langley Crossing, 101 - 6339 200 St, Langley BC V2Y 1A2 Canada",
                ManagerName = "Jainak Patel",
                Phone = "(604) 533-2132",
                Fax = "(604) 533-1000",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1206,
                Name = "SHOPPERS DRUG MART # 257",
                Address = "2947 Tillicum Road, Victoria BC V9A 2A6 Canada",
                ManagerName = "Jasneek Manhas",
                Phone = "(250) 383-7702",
                Fax = "(250) 383-6952",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1207,
                Name = "SHOPPERS DRUG MART # 258",
                Address = "#100 - 22196 50 Ave, Langley BC V2Y 2V4 Canada",
                ManagerName = "Stephen Atia",
                Phone = "(604) 532-0515",
                Fax = "(604) 532-1336",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1208,
                Name = "SHOPPERS DRUG MART # 261",
                Address = "2 - 2121 E. Trans Canada Hwy., Kamloops BC V2C 4A6 Canada",
                ManagerName = "Rajat Talwar",
                Phone = "(250) 374-3131",
                Fax = "(250) 374-4664",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1209,
                Name = "SHOPPERS DRUG MART # 262",
                Address = "8305 Main St, Box 329, Osoyoos BC V0H 1V0 Canada",
                ManagerName = "Christine Choi",
                Phone = "(250) 495-6055",
                Fax = "(250) 495-5395",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1210,
                Name = "SHOPPERS DRUG MART # 263",
                Address = "885 West Broadway, Vancouver BC V5Z 1J9 Canada",
                ManagerName = "Ruxin Shen",
                Phone = "(604) 708-1135",
                Fax = "(604) 708-3304",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1211,
                Name = "SHOPPERS DRUG MART # 265",
                Address = "Clover Square Village, Unit 104 - 17790 No. 10 Highway, Surrey BC V3S 1C7 Canada",
                ManagerName = "Sheikh Jahan",
                Phone = "(604) 574-7436",
                Fax = "(604) 574-0857",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1212,
                Name = "SHOPPERS DRUG MART # 266",
                Address = "102 - 4647 Lakelse Ave, Terrace BC V8G 1R3 Canada",
                ManagerName = "Kelvin Cheung",
                Phone = "(250) 635-7261",
                Fax = "(250) 635-3574",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1213,
                Name = "SHOPPERS DRUG MART # 267",
                Address = "Central City Shopping Centre, #3100 - 10153 King George Blvd, Surrey BC V3T 2W1 Canada",
                ManagerName = "Atheer Bidawid",
                Phone = "(604) 588-6451",
                Fax = "(604) 588-4926",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1214,
                Name = "SHOPPERS DRUG MART # 268",
                Address = "Unit 185 - 3055 Massey Drive, Prince George BC V2N 2S9 Canada",
                ManagerName = "Cornelia Jonker",
                Phone = "(250) 562-8169",
                Fax = "(250) 562-7369",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1215,
                Name = "SHOPPERS DRUG MART # 269",
                Address = "#100 - 4440 W. Saanich Road, Victoria BC V8Z 3E9 Canada",
                ManagerName = "Jawad Alam",
                Phone = "(250) 881-1980",
                Fax = "(250) 881-8299",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1216,
                Name = "SHOPPERS DRUG MART # 271",
                Address = "1305 Cedar Ave., Trail BC V1R 4C3 Canada",
                ManagerName = "Linda Seib",
                Phone = "(250) 368-3341",
                Fax = "(250) 368-3393",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1217,
                Name = "SHOPPERS DRUG MART # 272",
                Address = "1125 Davie St, Vancouver BC V6E 1N2 Canada",
                ManagerName = "Vincent Yeung",
                Phone = "(604) 669-2424",
                Fax = "(604) 681-2328",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1218,
                Name = "SHOPPERS DRUG MART # 273",
                Address = "15105 16 Ave, Surrey BC V4A 6G3 Canada",
                ManagerName = "Aidin Babaei",
                Phone = "(604) 536-8211",
                Fax = "(604) 536-5047",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1219,
                Name = "SHOPPERS DRUG MART # 274",
                Address = "10351 100 St, Fort St. John BC V1J 3Z2 Canada",
                ManagerName = "Mohamed Ahmed",
                Phone = "(250) 785-6155",
                Fax = "(250) 787-0862",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1220,
                Name = "SHOPPERS DRUG MART # 275",
                Address = "Chahko Mika Mall, 1116 Lakeside Drive, Nelson BC V1L 5Z3 Canada",
                ManagerName = "Steven Luca",
                Phone = "(250) 352-7268",
                Fax = "(250) 352-5750",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1221,
                Name = "SHOPPERS DRUG MART # 276",
                Address = "Town Centre Mall, #15 - 7100 Alberni St., Powell River BC V8A 5K9 Canada",
                ManagerName = "Mohammed Abdelatif",
                Phone = "(604) 485-2844",
                Fax = "(604) 485-9477",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1222,
                Name = "SHOPPERS DRUG MART # 277",
                Address = "Columbia Place, 110 - 1210 Summit Dr, Kamloops BC V2C 6M1 Canada",
                ManagerName = "Kenneth Dyer",
                Phone = "(250) 374-0477",
                Fax = "(250) 374-4009",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1223,
                Name = "SHOPPERS DRUG MART # 278",
                Address = "Westshore Town Centre, #300 - 2945 Jacklin Rd., Victoria BC V9B 5E3 Canada",
                ManagerName = "Rabah Zahr Eddine",
                Phone = "(250) 474-3251",
                Fax = "(250) 478-6623",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1224,
                Name = "SHOPPERS DRUG MART # 279",
                Address = "#141 - 610 Sixth St, New Westminster BC V3L 3C2 Canada",
                ManagerName = "Mark Eng",
                Phone = "(604) 521-0767",
                Fax = "(604) 521-8237",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1225,
                Name = "SHOPPERS DRUG MART # 280",
                Address = "The Hub, 1755 East Broadway, Vancouver BC V5N 1W2 Canada",
                ManagerName = "Reno Sihota",
                Phone = "(604) 872-8451",
                Fax = "(604) 872-8470",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1226,
                Name = "SHOPPERS DRUG MART # 281",
                Address = "6681 Mary Ellen Dr, Nanaimo BC V9V 1T7 Canada",
                ManagerName = "Nicole Leung",
                Phone = "(250) 390-4911",
                Fax = "(250) 390-5353",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1227,
                Name = "SHOPPERS DRUG MART # 283",
                Address = "101 - 715 Oliver St., Williams Lake BC V2G 1M9 Canada",
                ManagerName = "Erin Trott",
                Phone = "(250) 392-3333",
                Fax = "(250) 392-2408",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1228,
                Name = "SHOPPERS DRUG MART # 286",
                Address = "North Hills Shop. Centre, #48 - 700 Tranquille Road, Kamloops BC V2B 3H9 Canada",
                ManagerName = "Tim Phillips",
                Phone = "(250) 376-9010",
                Fax = "(250) 376-8775",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1229,
                Name = "SHOPPERS DRUG MART # 287",
                Address = "1 - 4030 - 200 St, Langley BC V3A 1K7 Canada",
                ManagerName = "Maninder Johal",
                Phone = "(604) 530-5388",
                Fax = "(604) 534-5009",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1230,
                Name = "SHOPPERS DRUG MART # 288",
                Address = "4326 Dunbar St., Vancouver BC V6S 2G3 Canada",
                ManagerName = "Rahim Rahemtulla",
                Phone = "(604) 732-8855",
                Fax = "(604) 732-8870",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1231,
                Name = "SHOPPERS DRUG MART # 290",
                Address = "Tamarack Shopping Ctr., #275 - 1500 Cranbrook St N, Cranbrook BC V1C 3S8 Canada",
                ManagerName = "Laureen Andriashek",
                Phone = "(250) 489-3438",
                Fax = "(250) 489-3402",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1232,
                Name = "SHOPPERS DRUG MART # 291",
                Address = "1221 Lynn Valley Rd, North Vancouver BC V7J 3H2 Canada",
                ManagerName = "Mahindokht Hanifian",
                Phone = "(604) 987-4468",
                Fax = "(604) 984-9187",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1233,
                Name = "SHOPPERS DRUG MART #2100",
                Address = "3260 Edgemont Blvd, North Vancouver BC V7R 0A7 Canada",
                ManagerName = "Jacob Aichmair",
                Phone = "(778) 338-6363",
                Fax = "(778) 338-6367",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1234,
                Name = "SHOPPERS DRUG MART #2107",
                Address = "#250 - 221 Ioco Rd, Port Moody BC V3H 4H2 Canada",
                ManagerName = "Leslie Liang",
                Phone = "(604) 461-1541",
                Fax = "(604) 461-1121",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1235,
                Name = "SHOPPERS DRUG MART #2109",
                Address = "#100 - 7820 Williams Rd, Richmond BC V7A 1G3 Canada",
                ManagerName = "Andy Wu",
                Phone = "(778) 296-4065",
                Fax = "(778) 296-4069",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1236,
                Name = "SHOPPERS DRUG MART #2112",
                Address = "102 - 3387 David Ave, Coquitlam BC V3E 0K4 Canada",
                ManagerName = "Shelly Jin",
                Phone = "(778) 284-2701 x 33",
                Fax = "(778) 284-2705",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1237,
                Name = "SHOPPERS DRUG MART #2113",
                Address = "A100 - 20678 Willoughby Town Centre Dr, Langley BC V2Y 0L7 Canada",
                ManagerName = "Nasalyn Espinosa",
                Phone = "(604) 882-3566",
                Fax = "(604) 882-3557",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1238,
                Name = "SHOPPERS DRUG MART #2115",
                Address = "107 - 20151 Fraser Hwy, Langley BC V3A 4E4 Canada",
                ManagerName = "Ranjeet Sidhu",
                Phone = "(604) 534-3870",
                Fax = "(604) 534-3874",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1239,
                Name = "SHOPPERS DRUG MART #2118",
                Address = "3868 Steveston Highway, Richmond BC V7E 2K1 Canada",
                ManagerName = "Peter Lok",
                Phone = "(604) 288-6343",
                Fax = "(604) 288-6347",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1240,
                Name = "SHOPPERS DRUG MART #2121",
                Address = "#35 - 1800 Tranquille Rd., Kamloops BC V2B 3L9 Canada",
                ManagerName = "Regan Wetherill",
                Phone = "(250) 376-5611",
                Fax = "(250) 376-5657",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1241,
                Name = "SHOPPERS DRUG MART #2122",
                Address = "7816 E. Saanich Road, Saanichton BC V8M 2B3 Canada",
                ManagerName = "Marwan Kasim",
                Phone = "(250) 652-9119",
                Fax = "(250) 652-9944",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1242,
                Name = "SHOPPERS DRUG MART #2123",
                Address = "232 Bridge St., Princeton BC V0X 1W0 Canada",
                ManagerName = "Tintu Babu",
                Phone = "(250) 295-3383",
                Fax = "(250) 295-3888",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1243,
                Name = "SHOPPERS DRUG MART #2125",
                Address = "Oliver Plaza, 1100 - 5955 Main St, Oliver BC V0H 1T0 Canada",
                ManagerName = "David Kim",
                Phone = "(250) 498-3663",
                Fax = "(250) 498-4988",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1244,
                Name = "SHOPPERS DRUG MART #2126",
                Address = "121 - 4295 Blackcomb Way, Whistler BC V8E 0X2 Canada",
                ManagerName = "Caleb McHallam",
                Phone = "(604) 905-5666",
                Fax = "(604) 905-5305",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1245,
                Name = "SHOPPERS DRUG MART #2127",
                Address = "4303 Hastings St, Burnaby BC V5C 2J7 Canada",
                ManagerName = "Rita Afkari",
                Phone = "(604) 298-4101",
                Fax = "(604) 298-4131",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1246,
                Name = "SHOPPERS DRUG MART #2130",
                Address = "8525 River District Crossing, Vancouver BC V5S 0C8 Canada",
                ManagerName = "Joslyn Koh",
                Phone = "(604) 258-9667",
                Fax = "(604) 225-9197",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1247,
                Name = "SHOPPERS DRUG MART #2131",
                Address = "100 - 3327 Lakeshore Rd, Kelowna BC V1W 3S9 Canada",
                ManagerName = "Wade Rains",
                Phone = "(250) 868-9521",
                Fax = "(250) 868-9532",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1248,
                Name = "SHOPPERS DRUG MART #2132",
                Address = "1030 Frost Rd, Kelowna BC V1W 0E4 Canada",
                ManagerName = "Holly Sumner",
                Phone = "(250) 448-0270",
                Fax = "(250) 764-3497",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1249,
                Name = "SHOPPERS DRUG MART #2133",
                Address = "7130 Pioneer Ave., Agassiz BC V0M 1A0 Canada",
                ManagerName = "Baljit Hayre",
                Phone = "(604) 796-2241",
                Fax = "(604) 796-3528",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1250,
                Name = "SHOPPERS DRUG MART #2134",
                Address = "250 11939 240th Street, Maple Ridge BC V4R 1M7 Canada",
                ManagerName = "Faezeh Vahdatihassani",
                Phone = "(604) 466-3225",
                Fax = "(604) 466-2176",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1251,
                Name = "SHOPPERS DRUG MART #2139",
                Address = "101 - 801 Hilchey Rd, Campbell River BC V9W 0B8 Canada",
                ManagerName = "Sam Chu",
                Phone = "(250) 923-1575",
                Fax = "(250) 923-1524",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1252,
                Name = "SHOPPERS DRUG MART #2143",
                Address = "Shaughnessy Station Mall, 3150 - 2850 Shaughnessy St, Port Coquitlam BC V3C 6K5 Canada",
                ManagerName = "Bill Li",
                Phone = "(604) 461-7506",
                Fax = "(604) 461-7535",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1253,
                Name = "SHOPPERS DRUG MART #2144",
                Address = "101 - 10835 City Pwky, Surrey BC V3T 0L2 Canada",
                ManagerName = "Ravjot Sra",
                Phone = "(604) 495-9949",
                Fax = "(604) 495-9953",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1254,
                Name = "SHOPPERS DRUG MART #2149",
                Address = "107 - 15691 104 Ave, Surrey BC V4N 2H4 Canada",
                ManagerName = "Haoyue Zhang",
                Phone = "(236) 474-0335",
                Fax = "(778) 368-0501",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1255,
                Name = "SHOPPERS DRUG MART #2153",
                Address = "Unit A - 9970 Main St, Lake Country BC V4V 2T9 Canada",
                ManagerName = "Keunchul Wee",
                Phone = "(250) 766-2345",
                Fax = "(250) 766-4503",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1256,
                Name = "SHOPPERS DRUG MART #2156",
                Address = "#31 - 590 Hwy. 33 W., Kelowna BC V1X 6A8 Canada",
                ManagerName = "Darryl Deadmarsh",
                Phone = "(250) 860-1788",
                Fax = "(250) 868-2964",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1257,
                Name = "SHOPPERS DRUG MART #2158",
                Address = "110F - 6640 Vedder Rd, Chilliwack BC V2R 0J2 Canada",
                ManagerName = "Oluwakemi Monebi",
                Phone = "(604) 847-3496",
                Fax = "(604) 824-1811",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1258,
                Name = "SHOPPERS DRUG MART #2160",
                Address = "730 - 333 Brooksbank Ave, North Vancouver BC V7J 3S8 Canada",
                ManagerName = "Heather Wozny",
                Phone = "(778) 338-6183",
                Fax = "(778) 338-6187",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1259,
                Name = "SHOPPERS DRUG MART #2161",
                Address = "#217 - 5000 Canoe Pass Way, Tsawwassen BC V4M 0B3 Canada",
                ManagerName = "Hani Moukhachen",
                Phone = "(604) 948-0164",
                Fax = "(604) 948-1341",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1260,
                Name = "SHOPPERS DRUG MART #2200",
                Address = "1306 Lonsdale Avenue, North Vancouver BC V7M 2H8 Canada",
                ManagerName = "Amir Khazand",
                Phone = "(604) 904-0505",
                Fax = "(604) 904-0504",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1261,
                Name = "SHOPPERS DRUG MART #2201",
                Address = "597 Bernard Ave, Kelowna BC V1Y 6N9 Canada",
                ManagerName = "Ari Song",
                Phone = "(250) 763-1232",
                Fax = "(250) 763-1273",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1262,
                Name = "SHOPPERS DRUG MART #2203",
                Address = "Sunwood Square, 810 - 3025 Lougheed Hwy, Coquitlam BC V3B 6S2 Canada",
                ManagerName = "Jeffrey Pan",
                Phone = "(604) 468-8814",
                Fax = "(604) 468-8815",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1263,
                Name = "SHOPPERS DRUG MART #2204",
                Address = "#130 - 20395 Lougheed Hwy, Maple Ridge BC V2X 2P9 Canada",
                ManagerName = "Simon Ting",
                Phone = "(604) 465-8123",
                Fax = "(604) 465-8138",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1264,
                Name = "SHOPPERS DRUG MART #2205",
                Address = "Thunderbird Village, F 20159 88 Ave, Langley BC V1M 0A4 Canada",
                ManagerName = "Zyrel Zaparilla",
                Phone = "(604) 881-9921",
                Fax = "(604) 881-9923",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1265,
                Name = "SHOPPERS DRUG MART #2207",
                Address = "#102 - 22441 Dewdney Trunk Rd, Maple Ridge BC V2X 7X7 Canada",
                ManagerName = "Ellie Yousefian",
                Phone = "(604) 467-5218",
                Fax = "(604) 463-0459",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1266,
                Name = "SHOPPERS DRUG MART #2208",
                Address = "206 - 32530 Lougheed Highway, Mission BC V2V 1A5 Canada",
                ManagerName = "Sumit Manchanda",
                Phone = "(604) 826-1244",
                Fax = "(604) 820-7162",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1267,
                Name = "SHOPPERS DRUG MART #2209",
                Address = "#110-19150 Lougheed Hwy., Pitt Meadows BC V3Y 2H6 Canada",
                ManagerName = "Chapman Chan",
                Phone = "(604) 465-8122",
                Fax = "(604) 465-1021",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1268,
                Name = "SHOPPERS DRUG MART #2210",
                Address = "4460 Lougheed Hwy, Burnaby BC V5C 3Z3 Canada",
                ManagerName = "Anderson Zheng",
                Phone = "(604) 235-9027",
                Fax = "(604) 235-9032",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1269,
                Name = "SHOPPERS DRUG MART #2211",
                Address = "#38 - 3200 North Island Hwy., Nanaimo BC V9T 1W1 Canada",
                ManagerName = "Enas Audeh",
                Phone = "(250) 756-4991",
                Fax = "(250) 756-4983",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1270,
                Name = "SHOPPERS DRUG MART #2212",
                Address = "8962 152 St, Surrey BC V3R 4E4 Canada",
                ManagerName = "Jora Lidder",
                Phone = "(604) 581-4544",
                Fax = "(604) 583-7642",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1271,
                Name = "SHOPPERS DRUG MART #2213",
                Address = "870 Esquimalt Rd, Victoria BC V9A 3M4 Canada",
                ManagerName = "Radha Gupta",
                Phone = "(250) 361-2011",
                Fax = "(250) 361-2014",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1272,
                Name = "SHOPPERS DRUG MART #2214",
                Address = "1965 Columbia Ave, Castlegar BC V1N 2W8 Canada",
                ManagerName = "MD Mynol Vhuiyan",
                Phone = "(250) 365-5875",
                Fax = "(250) 365-7236",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1273,
                Name = "SHOPPERS DRUG MART #2215",
                Address = "140 East Island Hwy., PO Box 369, Parksville BC V9P 2G5 Canada",
                ManagerName = "Michael Biagioni",
                Phone = "(250) 248-3611",
                Fax = "(250) 248-0633",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1274,
                Name = "SHOPPERS DRUG MART #2216",
                Address = "Town Centre Mall, 3 - 2475 Dobbin Rd, West Kelowna BC V4T 2E9 Canada",
                ManagerName = "Tamer Elmansi",
                Phone = "(250) 707-0891",
                Fax = "(250) 707-0468",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1275,
                Name = "SHOPPERS DRUG MART #2217",
                Address = "#38 - 301 Highway 33 W, Kelowna BC V1X 1X8 Canada",
                ManagerName = "Jaya Prakash Gunda",
                Phone = "(250) 765-4156",
                Fax = "(250) 765-6825",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1276,
                Name = "SHOPPERS DRUG MART #2218",
                Address = "102 - 510 Fifth St, Nanaimo BC V9R 1P1 Canada",
                ManagerName = "Elizabeth Rutledge",
                Phone = "(250) 753-8234",
                Fax = "(250) 753-8025",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1277,
                Name = "SHOPPERS DRUG MART #2221",
                Address = "1780 Broadway W, Vancouver BC V6J 1Y1 Canada",
                ManagerName = "Rochelle Ong",
                Phone = "(604) 736-6006",
                Fax = "(604) 736-6042",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1278,
                Name = "SHOPPERS DRUG MART #2222",
                Address = "#100 - 7322 King George Blvd, Surrey BC V3W 5A5 Canada",
                ManagerName = "Karanvir Gill",
                Phone = "(604) 590-2271",
                Fax = "(604) 590-2241",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1279,
                Name = "SHOPPERS DRUG MART #2223",
                Address = "#120 - 150 Esplanade W, North Vancouver BC V7M 1A3 Canada",
                ManagerName = "Jocelyn Ha",
                Phone = "(604) 904-0150",
                Fax = "(604) 904-0160",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1280,
                Name = "SHOPPERS DRUG MART #2224",
                Address = "14867A 108 Ave, Surrey BC V3R 1W2 Canada",
                ManagerName = "Kim-Thu Pham",
                Phone = "(604) 584-8393",
                Fax = "(604) 581-4089",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1281,
                Name = "SHOPPERS DRUG MART #2225",
                Address = "Unit 110 - 879 Marine Dr, North Vancouver BC V7P 1R7 Canada",
                ManagerName = "Kimia Malekzadeh",
                Phone = "(604) 983-3631",
                Fax = "(604) 983-7037",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1282,
                Name = "SHOPPERS DRUG MART #2226",
                Address = "College Heights Plaza, 470 - 5240 Domano Blvd, Prince George BC V2N 4A1 Canada",
                ManagerName = "Shafqutullah Khan",
                Phone = "(250) 964-1888",
                Fax = "(250) 964-1884",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1283,
                Name = "SHOPPERS DRUG MART #2227",
                Address = "432 Marine Dr SW, Vancouver BC V5X 0C4 Canada",
                ManagerName = "Wilfred Lee",
                Phone = "(604) 235-7095",
                Fax = "(604) 235-7099",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1284,
                Name = "SHOPPERS DRUG MART #2228",
                Address = "Champlain Square, 7160 Kerr Street, Vancouver BC V5S 4W2 Canada",
                ManagerName = "Justin Loo",
                Phone = "(604) 434-2656",
                Fax = "(604) 434-3326",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1285,
                Name = "SHOPPERS DRUG MART #2230",
                Address = "#155-5555 Gilbert Road, Richmond BC V7C 0B8 Canada",
                ManagerName = "Yin Song",
                Phone = "(604) 295-4080",
                Fax = "(604) 295-4085",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1286,
                Name = "SHOPPERS DRUG MART #2231",
                Address = "Unit 6 - 12830 96 Ave, Surrey BC V3V 6A8 Canada",
                ManagerName = "Horia Jalily Hasani",
                Phone = "(604) 588-3488",
                Fax = "(604) 588-2065",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1287,
                Name = "SHOPPERS DRUG MART #2234",
                Address = "3277 Cambie St, Vancouver BC V5Z 2W3 Canada",
                ManagerName = "Chester Ha",
                Phone = "(604) 708-9090",
                Fax = "(604) 708-2442",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1288,
                Name = "SHOPPERS DRUG MART #2235",
                Address = "7538 120 St, Surrey BC V3W 3N1 Canada",
                ManagerName = "Ricky Samra",
                Phone = "(604) 495-8382",
                Fax = "(604) 495-8386",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1289,
                Name = "SHOPPERS DRUG MART #2236",
                Address = "F8 - 1410 Parkway Blvd, Coquitlam BC V3E 3J7 Canada",
                ManagerName = "Kaitlyn Lee",
                Phone = "(604) 468-8878",
                Fax = "(604) 468-8765",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1290,
                Name = "SHOPPERS DRUG MART #2237",
                Address = "Cambie Plaza, 11800 Cambie Rd, Richmond BC V6X 1L5 Canada",
                ManagerName = "Alexander Dar Santos",
                Phone = "(604) 278-9105",
                Fax = "(604) 270-6415",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1291,
                Name = "SHOPPERS DRUG MART #2238",
                Address = "101-12080 Nordel Way, Surrey BC V3W 1P6 Canada",
                ManagerName = "Ekamdeep Romana",
                Phone = "(604) 543-8155",
                Fax = "(604) 543-8165",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1292,
                Name = "SHOPPERS DRUG MART #2239",
                Address = "18677 Fraser Hwy, Surrey BC V3S 7Y3 Canada",
                ManagerName = "Jordan Nijjer",
                Phone = "(604) 575-4994",
                Fax = "(604) 575-4995",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1293,
                Name = "SHOPPERS DRUG MART #2241",
                Address = "Unit 1 - 2871 Livingstone Ave, Abbotsford BC V2T 0E2 Canada",
                ManagerName = "Harbhajan Amar",
                Phone = "(604) 851-8052",
                Fax = "(604) 851-8062",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1294,
                Name = "SHOPPERS DRUG MART #2243",
                Address = "Unit 1 - 811 Columbia St, New Westminster BC V3M 1B9 Canada",
                ManagerName = "Lucas Chu",
                Phone = "(604) 395-5717",
                Fax = "(604) 395-5721",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1295,
                Name = "SHOPPERS DRUG MART #2244",
                Address = "3215 St. Johns St, Port Moody BC V3H 2E1 Canada",
                ManagerName = "Grace Tsang",
                Phone = "(604) 461-4030",
                Fax = "(604) 461-3086",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1296,
                Name = "SHOPPERS DRUG MART #2245",
                Address = "748 Burrard St, Vancouver BC V6Z 2V6 Canada",
                ManagerName = "Sunshine Co",
                Phone = "(778) 330-4711",
                Fax = "(778) 330-4718",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1297,
                Name = "SHOPPERS DRUG MART #2246",
                Address = "1006 Homer St, Vancouver BC V6B 2W9 Canada",
                ManagerName = "Cindy Ho",
                Phone = "(604) 669-0330",
                Fax = "(604) 669-0322",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1298,
                Name = "SHOPPERS DRUG MART #2247",
                Address = "5968 Webber Lane, Vancouver BC V6S 0J9 Canada",
                ManagerName = "Ryan Kullar",
                Phone = "(604) 224-3086",
                Fax = "(604) 224-1409",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1299,
                Name = "SHOPPERS DRUG MART #2250",
                Address = "Metrotown Centre, Unit 343 - 4800 Kingsway, Burnaby BC V5H 4J2 Canada",
                ManagerName = "Andrew Chen",
                Phone = "(604) 419-0524",
                Fax = "(604) 419-0230",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1300,
                Name = "SHOPPERS DRUG MART #2251",
                Address = "45800 Promontory Rd, Chilliwack BC V2R 5Z5 Canada",
                ManagerName = "Donald Martens",
                Phone = "(604) 824-1036",
                Fax = "(604) 824-1037",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1301,
                Name = "SHOPPERS DRUG MART #2252",
                Address = "3303 Main St, Vancouver BC V5V 0B7 Canada",
                ManagerName = "Henry Huang",
                Phone = "(778) 328-9580",
                Fax = "(778) 328-9584",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1302,
                Name = "SHOPPERS DRUG MART #2254",
                Address = "288 Columbia St E, New Westminster BC V3L 0E7 Canada",
                ManagerName = "Susanta Dan",
                Phone = "(604) 395-1410",
                Fax = "(604) 395-1416",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1303,
                Name = "SHOPPERS DRUG MART #2255",
                Address = "104 - 3433 North Rd, Burnaby BC V3J 0A9 Canada",
                ManagerName = "Hajera Baqi",
                Phone = "(604) 415-0312",
                Fax = "(604) 415-0314",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1304,
                Name = "SHOPPERS DRUG MART #2256",
                Address = "Rupert Square, #249 - 500 2nd Ave W, Prince Rupert BC V8J 3T6 Canada",
                ManagerName = "Ming-Fung Sha",
                Phone = "(250) 624-9656",
                Fax = "(250) 624-9834",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1305,
                Name = "SHOPPERS DRUG MART #2257",
                Address = "1235 Main Street, Smithers BC V0J 3W0 Canada",
                ManagerName = "Valerie De Ruyter",
                Phone = "(250) 847-2288",
                Fax = "(250) 847-9034",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1306,
                Name = "SHOPPERS DRUG MART #2259",
                Address = "#100 - 4634 Park Ave, Terrace BC V8G 1V7 Canada",
                ManagerName = "Sundeep Singh",
                Phone = "(250) 615-5151",
                Fax = "(250) 615-5152",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1307,
                Name = "SHOPPERS DRUG MART #2260",
                Address = "120 City Centre, Kitimat BC V8C 1T6 Canada",
                ManagerName = "Jordan Almeida",
                Phone = "(250) 632-6177",
                Fax = "(250) 632-6023",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1308,
                Name = "SHOPPERS DRUG MART #2262",
                Address = "314 - 5500 Sunshine Coast Hwy, Sechelt BC V0N 3A2 Canada",
                ManagerName = "Erika Shinchi",
                Phone = "(604) 740-0052",
                Fax = "(604) 740-0681",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1309,
                Name = "SHOPPERS DRUG MART #2263",
                Address = "105 - 16050 24 Ave, Surrey BC V3Z 0R5 Canada",
                ManagerName = "Kyeong Jin Lee",
                Phone = "(604) 538-1893",
                Fax = "(604) 536-2600",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1310,
                Name = "SHOPPERS DRUG MART #2264",
                Address = "876 Village Dr, Port Coquitlam BC V3B 0G9 Canada",
                ManagerName = "Sameer Lail",
                Phone = "(604) 944-8690",
                Fax = "(604) 944-8675",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1311,
                Name = "SHOPPERS DRUG MART #2266",
                Address = "Unit 700 - 26310 Fraser Hwy, Aldergrove BC V4W 2Z7 Canada",
                ManagerName = "Ayaz Karmali",
                Phone = "(604) 607-1445",
                Fax = "(604) 607-1354",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1312,
                Name = "SHOPPERS DRUG MART #2267",
                Address = "Unit 100 - 1972 Kane Road, Kelowna BC V1V 3C4 Canada",
                ManagerName = "Nathan Klaassen",
                Phone = "(250) 869-0132",
                Fax = "(250) 869-4870",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1313,
                Name = "SHOPPERS DRUG MART #2270",
                Address = "Unit 100, 15157 No. 10 (56 Ave) Hwy, Surrey BC V3S 9A5 Canada",
                ManagerName = "Joy Francisco",
                Phone = "(604) 574-1081",
                Fax = "(604) 574-1053",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1314,
                Name = "SHOPPERS DRUG MART #2271",
                Address = "4376 27 St, Vernon BC V1T 4Y4 Canada",
                ManagerName = "Mark Pastro",
                Phone = "(250) 549-3326",
                Fax = "(250) 549-8026",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1315,
                Name = "SHOPPERS DRUG MART #2273",
                Address = "5940 University Blvd, Vancouver BC V6T 1Z3 Canada",
                ManagerName = "Benny Sio",
                Phone = "(604) 228-1533",
                Fax = "(604) 228-1532",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1316,
                Name = "SHOPPERS DRUG MART #2275",
                Address = "1202 Pender St W, Vancouver BC V6E 2S9 Canada",
                ManagerName = "Henry Yau",
                Phone = "(604) 605-1200",
                Fax = "(604) 899-1454",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1317,
                Name = "SHOPPERS DRUG MART #2276",
                Address = "4590 Fraser St, Vancouver BC V5V 4G7 Canada",
                ManagerName = "Ana Dominique De Guzman",
                Phone = "(604) 873-2681",
                Fax = "(604) 873-2650",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1318,
                Name = "SHOPPERS DRUG MART #2277",
                Address = "586 Granville St, Vancouver BC V6C 1X5 Canada",
                ManagerName = "Aron Ha",
                Phone = "(604) 683-4063",
                Fax = "(604) 683-6931",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1319,
                Name = "SHOPPERS DRUG MART #2279",
                Address = "2730 Oak St, Vancouver BC V6H 0A5 Canada",
                ManagerName = "Brian Chan",
                Phone = "(604) 714-1199",
                Fax = "(604) 714-1127",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1320,
                Name = "SHOPPERS DRUG MART #2282",
                Address = "350 Ross St, Kimberley BC V1A 2Z9 Canada",
                ManagerName = "Sarah Duggleby",
                Phone = "(250) 427-2181",
                Fax = "(250) 427-2171",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1321,
                Name = "SHOPPERS DRUG MART #2283",
                Address = "4827 Kingsway, Burnaby BC V5H 4T6 Canada",
                ManagerName = "Jackie Liu",
                Phone = "(604) 433-2721",
                Fax = "(604) 433-2736",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1322,
                Name = "SHOPPERS DRUG MART #2284",
                Address = "1030 Canyon St., Creston BC V0B 3G0 Canada",
                ManagerName = "Shane Cherrington",
                Phone = "(250) 428-9334",
                Fax = "(250) 428-9304",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1323,
                Name = "SHOPPERS DRUG MART #2287",
                Address = "6760 Madill Rd, Prince George BC V2K 0A8 Canada",
                ManagerName = "Sukhjeet Lidder",
                Phone = "(250) 962-1814",
                Fax = "(250) 962-1816",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1324,
                Name = "SHOPPERS DRUG MART #2288",
                Address = "2332 Whatcom Rd, Abbotsford BC V3G 0C1 Canada",
                ManagerName = "Joel Samuel",
                Phone = "(604) 851-8635",
                Fax = "(604) 851-8642",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1325,
                Name = "SHOPPERS DRUG MART #2289",
                Address = "2330 Kingsway, Vancouver BC V5R 5G9 Canada",
                ManagerName = "Sahar Ziaei",
                Phone = "(604) 484-1470",
                Fax = "(604) 484-1476",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1326,
                Name = "SHOPPERS DRUG MART #2290",
                Address = "Unit 1 - 32390 South Fraser Way, Abbotsford BC V2T 1X2 Canada",
                ManagerName = "David Le",
                Phone = "(604) 850-3517",
                Fax = "(604) 850-3041",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1327,
                Name = "SHOPPERS DRUG MART #2291",
                Address = "Unit D108 - 1966 Guthrie Rd, Comox BC V9M 3X7 Canada",
                ManagerName = "Marcy Cursley",
                Phone = "(250) 890-9327",
                Fax = "(250) 890-9357",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1328,
                Name = "SHOPPERS DRUG MART #2292",
                Address = "1295 Seymour St, Vancouver BC V6B 3N6 Canada",
                ManagerName = "Nichole Ata",
                Phone = "(604) 801-5708",
                Fax = "(604) 801-5730",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1329,
                Name = "SHOPPERS DRUG MART #2294",
                Address = "2748 East Hastings St, Vancouver BC V5K 1Z9 Canada",
                ManagerName = "Anoop Khurana",
                Phone = "(604) 251-5358",
                Fax = "(604) 251-6612",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1330,
                Name = "SHOPPERS DRUG MART #2297",
                Address = "Unit 100 - 525 Highway 97 S, Kelowna BC V1Z 4C9 Canada",
                ManagerName = "Shimelis Desha",
                Phone = "(250) 769-7012",
                Fax = "(250) 769-7023",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1331,
                Name = "SHOPPERS DRUG MART #2299",
                Address = "Polson Place Mall, 265 2306 Hwy 6, Vernon BC V1T 7E3 Canada",
                ManagerName = "Anjli Kanwar",
                Phone = "(250) 260-8576",
                Fax = "(250) 260-8279",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1332,
                Name = "SHOPPERS DRUG MART #5852",
                Address = "805 Boyd St, F150, New Westminster BC V3M 5X2 Canada",
                ManagerName = "Jaskaran Singh",
                Phone = "(604) 395-8326",
                Fax = "(604) 540-3423",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1333,
                Name = "SHOPPERS SIMPLY PHARMACY # 260",
                Address = "The Gateway, Unit 103 -2051 McCallum Road, Abbotsford BC V2S 3N5 Canada",
                ManagerName = "Samin Nagi",
                Phone = "(604) 853-1624",
                Fax = "(604) 853-9662",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1334,
                Name = "SHOPPERS SIMPLY PHARMACY #2249",
                Address = "1517 Commercial Drive, Vancouver BC V5L 3Y1 Canada",
                ManagerName = "Simon Sandhu",
                Phone = "(604) 255-0434",
                Fax = "(604) 253-3420",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1335,
                Name = "SHOPPERS SIMPLY PHARMACY #3082",
                Address = "#101 - 383 Ellis St., Penticton BC V2A 4L8 Canada",
                ManagerName = "Jamie Burnett",
                Phone = "(250) 493-4151",
                Fax = "(250) 493-4105",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1336,
                Name = "SHOPPERS SIMPLY PHARMACY #3087",
                Address = "113 - 1001 Austin Ave, Coquitlam BC V3K 3N9 Canada",
                ManagerName = "David Kim",
                Phone = "(604) 936-0024",
                Fax = "(604) 936-0034",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1337,
                Name = "SHUSWAP VALLEY PHARMACY",
                Address = "230 Ross St NE, Salmon Arm BC V1E 4N2 Canada",
                ManagerName = "Akarsha Bhat Mairaje",
                Phone = "(250) 835-1095",
                Fax = "(250) 835-1108",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1338,
                Name = "SIDNEY PHARMACY II",
                Address = "2425B Bevan Ave., Sidney BC V8L 4R7 Canada",
                ManagerName = "James McCullough",
                Phone = "(250) 656-0744",
                Fax = "(250) 656-0757",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1339,
                Name = "SILVERMERE PHARMACY",
                Address = "105 - 32423 Lougheed Hwy, Mission BC V2V 7B8 Canada",
                ManagerName = "Veronica Varela",
                Phone = "(604) 820-8002",
                Fax = "(604) 820-8040",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1340,
                Name = "SIMILKAMEEN PHARMACY",
                Address = "633 7th Ave, Keremeos BC V0X 1N0 Canada",
                ManagerName = "Nicholas Ko",
                Phone = "(250) 499-5086",
                Fax = "(250) 499-5108",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1341,
                Name = "SIMPLES DRUGS",
                Address = "916 - 5300 No. 3 Road, Richmond BC V6X 2X9 Canada",
                ManagerName = "Oi Ching Wong",
                Phone = "(604) 370-9228",
                Fax = "(604) 370-9229",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1342,
                Name = "SIMPLES DRUGS #2",
                Address = "Unit 1471 & 1473, 8388 Capstan Way, Richmond BC V6X 4A7 Canada",
                ManagerName = "I Ju Chen",
                Phone = "(604) 285-0099",
                Fax = "(604) 285-0090",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1343,
                Name = "SINA PHARMACY",
                Address = "505 Smithe St, Vancouver BC V6B 6H1 Canada",
                ManagerName = "Fatemeh Soleiman-Panah",
                Phone = "(604) 336-7462",
                Fax = "(604) 336-7461",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1344,
                Name = "SMART SAVE PHARMACY",
                Address = "103 - 7110 120 St, Surrey BC V3W 3M8 Canada",
                ManagerName = "Matiullah Matiullah",
                Phone = "(778) 565-5760",
                Fax = "(778) 565-5761",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1345,
                Name = "SOLACE PHARMACY",
                Address = "102 - 1277 Marine Drive, North Vancouver BC V7P 1T3 Canada",
                ManagerName = "Udaykumar Jain",
                Phone = "(604) 971-4444",
                Fax = "(604) 971-0099",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1346,
                Name = "SOMERSET PHARMACY SERVICES",
                Address = "#7 - 13791 - 72nd Ave, Surrey BC V3W 9Y9 Canada",
                ManagerName = "Davinder Purewal",
                Phone = "(604) 590-5509",
                Fax = "(604) 590-5523",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1347,
                Name = "SOUL RX",
                Address = "1 - 22335 Lougheed Hwy, Maple Ridge BC V2X 2T3 Canada",
                ManagerName = "Joyce Lam",
                Phone = "(604) 479-9791",
                Fax = "(604) 479-9792",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1348,
                Name = "SOUTH POINT PHARMACY & MEDICAL CLINIC",
                Address = "102 - 3211 152nd Street, Surrey BC V3Z 1H8 Canada",
                ManagerName = "Mehulgiri Gosai",
                Phone = "(604) 305-5447",
                Fax = "(604) 305-5449",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1349,
                Name = "SOUTH SLOPE PHARMACY",
                Address = "5203 Rumble Street, Burnaby BC V5J 2B7 Canada",
                ManagerName = "Edwin Chan",
                Phone = "(604) 245-5299",
                Fax = "(604) 245-5299",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1350,
                Name = "SOUTHLAND PHARMACY",
                Address = "3556 West 41st Ave, Unit 1, Vancouver BC V6N 3E6 Canada",
                ManagerName = "Joseph Chu",
                Phone = "(604) 266-2882",
                Fax = "(604) 263-9850",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1351,
                Name = "SPARWOOD REMEDY'S RX",
                Address = "#74 - 101 Red Cedar Dr, Sparwood BC V0B 2G0 Canada",
                ManagerName = "Sarina Beran",
                Phone = "(250) 425-6604",
                Fax = "(250) 425-6614",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1352,
                Name = "SPRUCE SPECIALTY PHARMACY",
                Address = "116 - 10928 132 St, Surrey BC V3T 0R3 Canada",
                ManagerName = "Tarndeep Chohan",
                Phone = "(604) 359-5003",
                Fax = "(604) 398-8515",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1353,
                Name = "SQUAMISH PHARMACY",
                Address = "101 - 37989 Cleveland Ave, Squamish BC V8B 0A7 Canada",
                ManagerName = "Eric Novak",
                Phone = "(604) 390-2004",
                Fax = "(604) 390-2005",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1354,
                Name = "ST GEORGES PHARMACY",
                Address = "1309 St. Georges Ave, North Vancouver BC V7L 3J2 Canada",
                ManagerName = "Golnoosh Yaghooti",
                Phone = "(604) 988-7199",
                Fax = "(604) 985-8855",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1355,
                Name = "ST. ANTHONY'S CLINIC PHARMACY #2",
                Address = "109 - 582 Goldstream Ave, Victoria BC V9B 2W7 Canada",
                ManagerName = "Linda Gutenberg",
                Phone = "(250) 478-8338",
                Fax = "(250) 478-7866",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1356,
                Name = "ST. RAPHAEL PHARMACY",
                Address = "101 - 30461 Blueridge Drive, Abbotsford BC V2T 0B1 Canada",
                ManagerName = "Laith Georgie",
                Phone = "(604) 744-2038",
                Fax = "(604) 744-2064",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1357,
                Name = "STEPPING STONE PHARMACY",
                Address = "523 Main Street, Vancouver BC V6A 2V1 Canada",
                ManagerName = "Philip Mang",
                Phone = "(604) 282-3973",
                Fax = "(604) 282-3974",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1358,
                Name = "STERLINGRX PHARMACY",
                Address = "102-3210 25 Ave, Vernon BC V1T 1P1 Canada",
                ManagerName = "Graeme Nevins",
                Phone = "(778) 475-7600",
                Fax = "(778) 475-7601",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1359,
                Name = "STEVESTON PHARMACY",
                Address = "#101 - 3811 Chatham St., Richmond BC V7E 2Z4 Canada",
                ManagerName = "Jeremy Tse",
                Phone = "(604) 271-2820",
                Fax = "(604) 272-2863",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1360,
                Name = "STS PAIN PHARMACY",
                Address = "820 Cormorant St, Victoria BC V8W 1R1 Canada",
                ManagerName = "Alain Vincent",
                Phone = "(778) 433-7246",
                Fax = "(250) 483-6448",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1361,
                Name = "STUART LAKE PHARMACY",
                Address = "470 W Stuart Drive Unit #14, Fort St. James BC V0J 1P0 Canada",
                ManagerName = "Ankur Pipaliya",
                Phone = "(250) 996-7815",
                Fax = "(250) 996-7659",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1362,
                Name = "SUKH'S PHARMACY",
                Address = "7166 120 St, Surrey BC V3W 3M8 Canada",
                ManagerName = "Ramnik Bhangu",
                Phone = "(604) 594-0069",
                Fax = "(604) 594-0804",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1363,
                Name = "SULLIVAN HEIGHTS PHARMACY",
                Address = "#105 - 14340 64 Ave, Surrey BC V3W 1Z1 Canada",
                ManagerName = "Rapinder Toor",
                Phone = "(778) 564-0018",
                Fax = "(778) 564-0012",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1364,
                Name = "SULLIVAN PHARMACY",
                Address = "#202 - 6355 152 St, Surrey BC V3S 3K8 Canada",
                ManagerName = "Basit Khan",
                Phone = "(778) 593-0052",
                Fax = "(778) 593-0053",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1365,
                Name = "SUMMERLAND PHARMACY",
                Address = "100 - 13009 Rosedale Ave, PO Box 579, Summerland BC V0H 1Z0 Canada",
                ManagerName = "Jonathan Kiesman",
                Phone = "(250) 494-0531",
                Fax = "(250) 494-0778",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1366,
                Name = "SUN PEAKS PHARMACY",
                Address = "#5 - 1240 Alpine Rd, Sun Peaks BC V0E 5N0 Canada",
                ManagerName = "Anthony Rinaldi",
                Phone = "778-760-4179",
                Fax = "778-760-4179",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1367,
                Name = "SUN VALLEY PHARMACY",
                Address = "1-9145 Main Street, Osoyoos BC V0H 1V2 Canada",
                ManagerName = "Andrew Stewart",
                Phone = "(778) 738-1115",
                Fax = "(236) 577-2982",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1368,
                Name = "SUNCOAST PHARMACY",
                Address = "103 - 5531 Inlet Ave, Sechelt BC V0N 3A0 Canada",
                ManagerName = "James Harte",
                Phone = "(604) 885-2899",
                Fax = "(604) 885-2820",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1369,
                Name = "SUNCOAST PHARMACY #15",
                Address = "12887 Madeira Park Rd, Madeira Park BC V0N 2H0 Canada",
                ManagerName = "Dayle Larson",
                Phone = "(604) 883-2888",
                Fax = "(604) 883-2804",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1370,
                Name = "SUNNYSIDE EVERGREEN PHARMACY",
                Address = "2397 King George Blvd, Surrey BC V4A 5A4 Canada",
                ManagerName = "Charandeep Sidhu",
                Phone = "(604) 536-4404",
                Fax = "(604) 536-4572",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1371,
                Name = "SUNSET PHARMACY LTD.",
                Address = "3818 Sunset St., Burnaby BC V5G 1T3 Canada",
                ManagerName = "Willie Seto",
                Phone = "(604) 435-3830",
                Fax = "(604) 435-3834",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1372,
                Name = "SUNWEST PHARMACY",
                Address = "105 - 14888 104 Ave, Surrey BC V3R 1M4 Canada",
                ManagerName = "Seema Prihar",
                Phone = "(778) 293-2273",
                Fax = "(778) 293-2274",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1373,
                Name = "SUPER GROCER AND PHARMACY",
                Address = "130 - 3591 Chatham Street, Richmond BC V7E 2Z1 Canada",
                ManagerName = "Flora Luk",
                Phone = "(604) 274-7878",
                Fax = "(604) 274-7800",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1374,
                Name = "SUPER-CARE PHARMACY",
                Address = "9225 Main St, Chilliwack BC V2P 4M8 Canada",
                ManagerName = "Mohan Ravi",
                Phone = "(604) 792-6260",
                Fax = "(604) 792-6217",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1375,
                Name = "SUPERSAVE PHARMACY",
                Address = "3443 Kingsway, Vancouver BC V5R 5L3 Canada",
                ManagerName = "Farrah Nanji",
                Phone = "(604) 438-5773",
                Fax = "(604) 438-5767",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1376,
                Name = "SUPRA NORTHSHORE PHARMACY",
                Address = "748 Tranquille Road, Kamloops BC V2B 3J2 Canada",
                ManagerName = "Suni Pradeep",
                Phone = "(250) 434-4441",
                Fax = "(250) 434-4449",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1377,
                Name = "SURLANG MEDICINE CENTRE PHARMACY",
                Address = "105 - 19475 Fraser Hwy, Surrey BC V3S 6K7 Canada",
                ManagerName = "Bob Sangha",
                Phone = "(604) 533-1041",
                Fax = "(604) 533-1051",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1378,
                Name = "SURREY PHARMACY",
                Address = "7141 King George Blvd, Surrey BC V3W 5A4 Canada",
                ManagerName = "Pawan Grover",
                Phone = "(604) 572-3060",
                Fax = "(604) 572-3065",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1379,
                Name = "SURREY TRUE HEALTH PHARMACY INC.",
                Address = "105-10334 152A St, Surrey BC V3R 7P8 Canada",
                ManagerName = "Richa Bhatia",
                Phone = "(604) 954-1581",
                Fax = "1(833) 444-0309",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1380,
                Name = "TABLET PHARMACY",
                Address = "4 - 154 Middleton Avenue, Parksville BC V9P 2G9 Canada",
                ManagerName = "Kuldeep Kaur Kaler",
                Phone = "(250) 947-9929",
                Fax = "(250) 947-9083",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1381,
                Name = "TABLET PHARMACY #3",
                Address = "101-6373 Hammond Bay Rd, Nanaimo BC V9T 5Y1 Canada",
                ManagerName = "Akshit Shah",
                Phone = "(250) 933-0093",
                Fax = "(250) 933-1033",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1382,
                Name = "TABLET PHARMACY QUALICUM",
                Address = "2 - 219 Fern Rd W, Qualicum Beach BC V9K 2M2 Canada",
                ManagerName = "Mahdie Sarabi",
                Phone = "(250) 947-1230",
                Fax = "(250) 410-1248",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1383,
                Name = "TANG'S PRESCRIPTIONS",
                Address = "1306 Central St E, Prince George BC V2M 3C1 Canada",
                ManagerName = "Patrick Dinelle",
                Phone = "(250) 596-6888",
                Fax = "(250) 596-6889",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1384,
                Name = "TELUS HEALTH VIRTUAL PHARMACY",
                Address = "#165 - 21320 Gordon Way, Richmond BC V6W 1J8 Canada",
                ManagerName = "Brian Lee",
                Phone = "(604) 370-1999",
                Fax = "(604) 370-2030",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1385,
                Name = "TERRA NOVA PHARMACHOICE",
                Address = "135 - 6011 No 1 Rd, Richmond BC V7C 1T4 Canada",
                ManagerName = "Ayah Kapani",
                Phone = "(604) 284-3784",
                Fax = "(604) 284-3785",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1386,
                Name = "THE DRIVE PHARMACY",
                Address = "1684 Commercial Dr, Vancouver BC V5L 3Y4 Canada",
                ManagerName = "Simranjit Sidhu",
                Phone = "(604) 254-0133",
                Fax = "(604) 254-0134",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1387,
                Name = "THE LOCAL PHARMACY",
                Address = "Unit 102 - 225 Rutland Road South, Kelowna BC V1X 2Z3 Canada",
                ManagerName = "Sajad War",
                Phone = "(778) 583-5992",
                Fax = "(778) 583-5994",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1388,
                Name = "THE MEDICINE SHOPPE #163 - SURREY",
                Address = "#122 - 1959 - 152nd St, Surrey BC V4A 9E3 Canada",
                ManagerName = "Asher Anjum",
                Phone = "(604) 531-4400",
                Fax = "(604) 531-6560",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1389,
                Name = "THE MEDICINE SHOPPE #239",
                Address = "2030 Kingsway, Vancouver BC V5N 2T3 Canada",
                ManagerName = "Max Khondaker",
                Phone = "(604) 876-2511",
                Fax = "(604) 876-2519",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1390,
                Name = "THE MEDICINE SHOPPE #358",
                Address = "105 - 3957 Lakeshore Rd, Kelowna BC V1W 1V3 Canada",
                ManagerName = "Krunal Patel",
                Phone = "(778) 477-3811",
                Fax = "(778) 477-3812",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1391,
                Name = "THE MEDICINE SHOPPE PHARMACY #231",
                Address = "6180 Fraser St, Vancouver BC V5W 3A1 Canada",
                ManagerName = "Paul Dhudwal",
                Phone = "(604) 327-3898",
                Fax = "(604) 327-3803",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1392,
                Name = "THE MEDICINE SHOPPE PHARMACY #285",
                Address = "3039 Kingsway, Vancouver BC V5R 5J6 Canada",
                ManagerName = "Connie Huen",
                Phone = "(604) 437-5442",
                Fax = "(604) 638-0194",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1393,
                Name = "THE MEDICINE SHOPPE PHARMACY #367",
                Address = "#4 - 1363 56 St, Delta BC V4L 2P7 Canada",
                ManagerName = "Ma. Cristina Gumangan",
                Phone = "(778) 434-3300",
                Fax = "(778) 434-3303",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1394,
                Name = "THE MEDICINE SHOPPE PHARMACY #419",
                Address = "Unit 109 - 9639 137A Street, Surrey BC V3T 0M1 Canada",
                ManagerName = "Sahilpreet Kingra",
                Phone = "(604) 589-5454",
                Fax = "(604) 589-5455",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1395,
                Name = "THE MEDICINE SHOPPE PHARMACY #433",
                Address = "401 Dollarton Highway North, North Vancouver BC V7G 1M9 Canada",
                ManagerName = "Michelle Melanie Liao",
                Phone = "(236) 481-6959",
                Fax = "(236) 481-6960",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1396,
                Name = "THE PHARMACY KITSILANO (PHARMACHOICE)",
                Address = "2955 Broadway W, Vancouver BC V6K 2G9 Canada",
                ManagerName = "Judy Xie",
                Phone = "(604) 564-3331",
                Fax = "(604) 564-3332",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1397,
                Name = "THE PHARMACY LANGLEY",
                Address = "116 - 5501 204 St, Langley BC V3A 5N8 Canada",
                ManagerName = "Moez Karim",
                Phone = "(778) 277-0024",
                Fax = "(778) 277-0025",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1398,
                Name = "THE PHARMACY WEST END (PHARMACHOICE)",
                Address = "1747 Robson St, Vancouver BC V6G 1C9 Canada",
                ManagerName = "Manh Dao",
                Phone = "(604) 669-6927",
                Fax = "(604) 669-6928",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1399,
                Name = "THE PHARMACY WESTVIEW (PHARMACHOICE)",
                Address = "#760 - 2601 Westview Drive, North Vancouver BC V7N 3X4 Canada",
                ManagerName = "Stella Oh",
                Phone = "(604) 986-2292",
                Fax = "(604) 986-2293",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1400,
                Name = "THE PHARMACY YALETOWN (PHARMACHOICE)",
                Address = "1251 Pacific Blvd, Vancouver BC V6Z 2R6 Canada",
                ManagerName = "Leanne Dale",
                Phone = "(604) 684-8488",
                Fax = "(604) 684-8499",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1401,
                Name = "THE VILLAGE PHARMACY",
                Address = "9025 160 St, Surrey BC V4N 2X7 Canada",
                ManagerName = "Ganas Moodley",
                Phone = "(604) 589-0012",
                Fax = "(604) 589-0344",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1402,
                Name = "THIRD AVENUE PHARMACY",
                Address = "1467 - 3rd Ave, Prince George BC V2L 3G1 Canada",
                ManagerName = "Brianna Pallot",
                Phone = "(250) 564-7147",
                Fax = "(250) 564-2517",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1403,
                Name = "THREE RIVERS PHARMACY",
                Address = "2510 Highway 62, Hazelton BC V0J 1Y0 Canada",
                ManagerName = "Alyssa Tilley",
                Phone = "(250) 842-6040",
                Fax = "(250) 842-0154",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1404,
                Name = "THRIFTY FOODS AND PHARMACY #9454",
                Address = "475 Simcoe Street, Victoria BC V8V 4T4 Canada",
                ManagerName = "Mohamed Zeid",
                Phone = "(250) 386-8337",
                Fax = "(250) 386-8334",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1405,
                Name = "THRIFTY FOODS AND PHARMACY #9458",
                Address = "280 East Island Hwy, Parksville BC V9P 2H1 Canada",
                ManagerName = "Madison Finnigan",
                Phone = "(250) 947-2535",
                Fax = "(250) 947-2536",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1406,
                Name = "THRIFTY FOODS AND PHARMACY #9461",
                Address = "3475 Quadra Street, Victoria BC V8X 1G8 Canada",
                ManagerName = "Alan Hicke",
                Phone = "(250) 382-2881",
                Fax = "(250) 382-2801",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1407,
                Name = "THRIFTY FOODS AND PHARMACY #9464",
                Address = "5801 Turner Road, Nanaimo BC V9T 6L8 Canada",
                ManagerName = "Jacklyn McDonald",
                Phone = "(250) 729-7240",
                Fax = "(250) 729-9802",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1408,
                Name = "THRIFTY FOODS AND PHARMACY #9467",
                Address = "1400 Ironwood St, Campbell River BC V9W 5T5 Canada",
                ManagerName = "Mohammad Ali Jalloh",
                Phone = "(250) 850-3585",
                Fax = "(250) 850-3586",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1409,
                Name = "THRIFTY FOODS AND PHARMACY #9470",
                Address = "170 Brew St, Port Moody BC V3H 0E7 Canada",
                ManagerName = "Ying Joe",
                Phone = "(604) 949-4253",
                Fax = "(604) 949-4254",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1410,
                Name = "THRIFTY FOODS AND PHARMACY #9471",
                Address = "1 2755 Beverly St, Duncan BC V9L 6X2 Canada",
                ManagerName = "Neemet McDowell",
                Phone = "(250) 715-2654",
                Fax = "(250) 715-2655",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1411,
                Name = "THRIFTY FOODS AND PHARMACY #9472",
                Address = "#102 - 15745 Croydon Dr, Surrey BC V3Z 2L5 Canada",
                ManagerName = "Akhil Bhai",
                Phone = "(604) 542-7853",
                Fax = "(604) 542-7854",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1412,
                Name = "THRIFTY FOODS AND PHARMACY #9478",
                Address = "#100 - 444 Lerwick Rd, Courtenay BC V9N 0A9 Canada",
                ManagerName = "Michael Kennedy",
                Phone = "(250) 331-5103",
                Fax = "(250) 331-5104",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1413,
                Name = "THRIFTY FOODS AND PHARMACY #9480",
                Address = "3011 Merchant Way, Langford BC V9B 0W9 Canada",
                ManagerName = "Alireza Zamanian",
                Phone = "250-391-4048",
                Fax = "250-391-4073",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1414,
                Name = "THRIFTY FOODS AND PHARMACY #9481",
                Address = "1551 Cliffe Ave, Courtenay BC V9N 2K6 Canada",
                ManagerName = "Stephanie Clarke",
                Phone = "(250) 331-4999",
                Fax = "(250) 331-4991",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1415,
                Name = "THRIVE PHARMACY",
                Address = "Unit 101 - 10663 King George Blvd, Surrey BC V3T 2X6 Canada",
                ManagerName = "Parinaz Amirimoghadam",
                Phone = "(604) 498-4444",
                Fax = "(604) 498-8266",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1416,
                Name = "TOFINO PHARMACY",
                Address = "360 Campbell Street, P.O. Box 509, Tofino BC V0R 2Z0 Canada",
                ManagerName = "Philip Petranek",
                Phone = "(250) 725-3101",
                Fax = "(250) 725-4491",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1417,
                Name = "TOTALCARE RX",
                Address = "4345 Hastings St, Burnaby BC V5C 2J7 Canada",
                ManagerName = "Pardeep Sandhu",
                Phone = "(604) 451-0005",
                Fax = "(604) 451-0007",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1418,
                Name = "TOWN CENTRE PHARMACY",
                Address = "130 - 1153 The High St, Coquitlam BC V3B 0B7 Canada",
                ManagerName = "Dennis Taruc",
                Phone = "(604) 475-8508",
                Fax = "(604) 475-8509",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1419,
                Name = "TSAWWASSEN PHARMACY",
                Address = "#104 - 1077 - 56th St., Delta BC V4L 2A2 Canada",
                ManagerName = "Andrew Corcoran",
                Phone = "(604) 943-9341",
                Fax = "(604) 943-2935",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1420,
                Name = "TUMBLER RIDGE PHARMACY",
                Address = "#110 - 230 Main Street, P.O. Box 1739, Tumbler Ridge BC V0C 2W0 Canada",
                ManagerName = "Charissa Tonnesen",
                Phone = "(250) 242-3333",
                Fax = "(250) 242-3343",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1421,
                Name = "TURTLE BAY PHARMACY",
                Address = "801 - 11850 Oceola Rd, Lake Country BC V4V 2T5 Canada",
                ManagerName = "Tianna Proctor",
                Phone = "(778) 480-6644",
                Fax = "(778) 480-8100",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1422,
                Name = "TWIG FERTILITY PHARMACY",
                Address = "200-525 West 8th Avenue West, Vancouver BC V5Z 1C6 Canada",
                ManagerName = "Paula Do",
                Phone = "(778) 743-8944",
                Fax = "(778) 693-4390",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1423,
                Name = "TWO NICE GUYS PHARMACY",
                Address = "102 - 555 Montgomery Rd, Kelowna BC V1X 3C6 Canada",
                ManagerName = "Kevin Medwedew",
                Phone = "(778) 753-6897",
                Fax = "(778) 753-6903",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1424,
                Name = "UCLUELET CO-OP PHARMACY",
                Address = "1580 Peninsula Rd, Box 100, Ucluelet BC V0R 3A0 Canada",
                ManagerName = "Rizmarc Gumpac",
                Phone = "(250) 726-4342",
                Fax = "(250) 726-2760",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1425,
                Name = "ULTRACARE GUARDIAN PHARMACY #2",
                Address = "424 Columbia St, New Westminster BC V3L 1B1 Canada",
                ManagerName = "Fabina Kara",
                Phone = "(604) 522-3400",
                Fax = "(604) 522-3402",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1426,
                Name = "UNICARE PHARMACY",
                Address = "102 - 3325 Kingsway, Vancouver BC V5R 5K6 Canada",
                ManagerName = "Peter Law",
                Phone = "(604) 438-5155",
                Fax = "(604) 438-5155",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1427,
                Name = "UNIONBAY PHARMACY",
                Address = "313 Mcleod Rd, Union Bay BC V0R 3B0 Canada",
                ManagerName = "Benilda Maglaya",
                Phone = "(250) 335-4590",
                Fax = "(250) 335-4592",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1428,
                Name = "UNIVERSAL PHARMACY",
                Address = "1115 - 7318 137 St, Surrey BC V3W 1A3 Canada",
                ManagerName = "Sukhvinder Sran",
                Phone = "(604) 594-9535",
                Fax = "(604) 594-9540",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1429,
                Name = "UNIVERSITY HEIGHTS PHARMACY",
                Address = "103B - 3994 Shelbourne Street, Victoria BC V8N 3E2 Canada",
                ManagerName = "Andrew Tam",
                Phone = "(250) 590-8886",
                Fax = "(250) 590-8377",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1430,
                Name = "UNIVERSITY PHARMACY (1987) LTD.",
                Address = "5754 University Blvd., Vancouver BC V6T 1K6 Canada",
                ManagerName = "Mario Linaksita",
                Phone = "(604) 224-3202",
                Fax = "(604) 224-3203",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1431,
                Name = "UPRX",
                Address = "224 - 3989 Henning Drive, Burnaby BC V5C 6P8 Canada",
                ManagerName = "Mehrdad Rezaei",
                Phone = "604-291-1094",
                Fax = "604-291-0827",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1432,
                Name = "UPTOWN DRUGS PEOPLES PHARMACY",
                Address = "508 Sixth Ave, New Westminster BC V3L 1V3 Canada",
                ManagerName = "David Zhao",
                Phone = "(604) 520-3009",
                Fax = "(833) 694-1527",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1433,
                Name = "URBAN CARE PHARMACY",
                Address = "100 - 678 Hastings St E, Vancouver BC V6A 1R1 Canada",
                ManagerName = "Vishal Deshmukh",
                Phone = "(604) 566-0800",
                Fax = "(604) 566-0801",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1434,
                Name = "URBAN FARE PHARMACY #7614 - YALETOWN",
                Address = "177 Davie St, Vancouver BC V6Z 2Y1 Canada",
                ManagerName = "Kam Fung",
                Phone = "(604) 975-7544",
                Fax = "(604) 975-7551",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1435,
                Name = "UVIC CAMPUS PHARMACY",
                Address = "University of Victoria, S.U.B. #B138, 3800 Finnerty Rd, Victoria BC V8P 5C2 Canada",
                ManagerName = "Joe Frketic",
                Phone = "(250) 721-3400",
                Fax = "(250) 472-5183",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1436,
                Name = "VALLEY EVERGREEN PHARMACY",
                Address = "20577 Douglas Crescent, Langley BC V3A 4B6 Canada",
                ManagerName = "Jasmine Parhar",
                Phone = "(604) 534-1332",
                Fax = "(604) 534-8678",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1437,
                Name = "VALLEY OUTREACH PHARMACY",
                Address = "#48 - 3347 262 St, Aldergrove BC V4W 3V9 Canada",
                ManagerName = "Nick Singh",
                Phone = "(604) 381-5977",
                Fax = "(604) 381-5978",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1438,
                Name = "VALLEY PHARMACY",
                Address = "Unit 10 - 45955 Yale Rd, Chilliwack BC V2P 2M4 Canada",
                ManagerName = "Joon Ho Lee",
                Phone = "(604) 392-2211",
                Fax = "(604) 392-2212",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1439,
                Name = "VALUE DRUG MART WILLOUGHBY",
                Address = "A125 - 8045 204 Street, Langley BC V2Y 5K1 Canada",
                ManagerName = "Gunveen Kaur",
                Phone = "(236) 471-6978",
                Fax = "(236) 471-6977",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1440,
                Name = "VANCOUVER PHARMACY",
                Address = "The Lux, 67 Hastings St E, Vancouver BC V6A 0A7 Canada",
                ManagerName = "Vida Ghavamzadeh",
                Phone = "(604) 669-5990",
                Fax = "(604) 669-5981",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1441,
                Name = "VANDERHOOF MEDICINE CENTRE",
                Address = "2436 Church Ave, Vanderhoof BC V0J 3A0 Canada",
                ManagerName = "Alana Nikolitsas",
                Phone = "(250) 567-5568",
                Fax = "(250) 567-5561",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1442,
                Name = "VEDDER PHARMACY",
                Address = "106 - 5535 Vedder Road, Chilliwack BC V2R 6H8 Canada",
                ManagerName = "Sumit Khanna",
                Phone = "(604) 647-3571",
                Fax = "(604) 800-0628",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1443,
                Name = "VERSA PHARMACHOICE",
                Address = "102-32526 George Ferguson Way, Abbotsford BC V2T 4Y1 Canada",
                ManagerName = "Sukhjit Dhaliwal",
                Phone = "(604) 746-7088",
                Fax = "(604) 746-7089",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1444,
                Name = "VICTORIA COMPOUNDING PHARMACY",
                Address = "1089 Fort St., Victoria BC V8V 3K5 Canada",
                ManagerName = "John Forster-Coull",
                Phone = "(250) 388-5181",
                Fax = "(250) 388-5191",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1445,
                Name = "VIEW ST. PHARMACY",
                Address = "867 View St, Victoria BC V8W 1K1 Canada",
                ManagerName = "Irish Fernandez",
                Phone = "(250) 361-3773",
                Fax = "(250) 361-3730",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1446,
                Name = "VILLAGE PHARMACY",
                Address = "9537 Erickson Drive, Burnaby BC V3J 1M9 Canada",
                ManagerName = "Hassanali Dewji",
                Phone = "(604) 421-4388",
                Fax = "(604) 421-4589",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1447,
                Name = "VINA PHARMACY",
                Address = "#118 - 888 Kingsway, Vancouver BC V5V 3C3 Canada",
                ManagerName = "Oanh Le",
                Phone = "(604) 669-1623",
                Fax = "(604) 669-1623",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1448,
                Name = "VITAL HEALTH PHARMACY",
                Address = "1825 Fort St, Victoria BC V8R 1J6 Canada",
                ManagerName = "Jonathan Cox",
                Phone = "(778) 433-6060",
                Fax = "(778) 433-7071",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1449,
                Name = "VITAL HEALTH PHARMACY #2",
                Address = "560 West Ave, Kelowna BC V1Y 4Z4 Canada",
                ManagerName = "Karl Pister",
                Phone = "(778) 738-3195",
                Fax = "(778) 738-3196",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1450,
                Name = "VITAL HEALTH PHARMACY #3",
                Address = "Suite 101 - 1990 Fort Street, Victoria BC V8R 6V4 Canada",
                ManagerName = "Naren Bollipalli",
                Phone = "(778) 949-8482",
                Fax = "(778) 247-2309",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1451,
                Name = "VIVA MED PHARMACY",
                Address = "Unit 120 - 6345 120 St, Delta BC V4E 2A6 Canada",
                ManagerName = "Harwinder Dhasi",
                Phone = "(604) 599-9442",
                Fax = "(604) 599-9380",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1452,
                Name = "VSTAR PHARMACY",
                Address = "4012 Hastings St, Burnaby BC V5C 2H9 Canada",
                ManagerName = "Shreyaben Patel",
                Phone = "(604) 291-1205",
                Fax = "(604) 291-1206",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1453,
                Name = "WALLACE PHARMACY",
                Address = "102 100 Wallace Street, Nanaimo BC V9R 5B1 Canada",
                ManagerName = "Samer Qasem",
                Phone = "(250) 753-8686",
                Fax = "(250) 753-8871",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1454,
                Name = "WALMART CENTRAL FILL PHARMACY #1502",
                Address = "2-2355 160 St, Surrey BC V3Z 9N6 Canada",
                ManagerName = "Justin Lee",
                Phone = "(778) 292-8264",
                Fax = "(604) 398-8513",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1455,
                Name = "WAL-MART PHARMACY #1011",
                Address = "1601 Marcolin Drive, Trail BC V1R 4Y1 Canada",
                ManagerName = "Marlise Swankhuizen",
                Phone = "(250) 364-2707",
                Fax = "1-(855)-983-1231",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1456,
                Name = "WAL-MART PHARMACY #1015",
                Address = "39210 Discovery Way, Squamish BC V8B 0N1 Canada",
                ManagerName = "Raelene Vandenbosch",
                Phone = "(604) 815-4630",
                Fax = "(604) 815-4657",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1457,
                Name = "WAL-MART PHARMACY #1018",
                Address = "3355 Johnston Rd, Port Alberni BC V9Y 8K1 Canada",
                ManagerName = "Charles Winternitz",
                Phone = "(250) 720-0916",
                Fax = "1-855-983-0996",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1458,
                Name = "WAL-MART PHARMACY #1036",
                Address = "100 - 3900 Crawford Ave, Merritt BC V1K 0A4 Canada",
                ManagerName = "Tracey Schmidt",
                Phone = "(250) 315-1371",
                Fax = "(250) 315-1356",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1459,
                Name = "WAL-MART PHARMACY #1077",
                Address = "1477 Island Hwy, Campbell River BC V9W 8E5 Canada",
                ManagerName = "Ryan Henderson",
                Phone = "(250) 287-2372",
                Fax = "(250) 287-7335",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1460,
                Name = "WAL-MART PHARMACY #1093",
                Address = "2170 Louie Dr, Westbank BC V4T 3E5 Canada",
                ManagerName = "Marisol Sohn",
                Phone = "(250) 768-1759",
                Fax = "(250) 768-3204",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1461,
                Name = "WAL-MART PHARMACY #1100",
                Address = "2991A 9 Ave SW, Salmon Arm BC V1E 0C3 Canada",
                ManagerName = "Dino Santos",
                Phone = "(250) 803-4403",
                Fax = "1 (855) 983-1015",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1462,
                Name = "WAL-MART PHARMACY #1104",
                Address = "3585 Grandview Hwy, Vancouver BC V5M 2G7 Canada",
                ManagerName = "Janet Marconato",
                Phone = "(604) 435-6150",
                Fax = "(604) 435-6437",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1463,
                Name = "WAL-MART PHARMACY #1106",
                Address = "1205 Prosperity Way, Williams Lake BC V2G 0A6 Canada",
                ManagerName = "Karl Ilao",
                Phone = "(250) 305-6899",
                Fax = "(250) 305-1145",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1464,
                Name = "WAL-MART PHARMACY #1112",
                Address = "2150 Hawkins St, Port Coquitlam BC V3B 0G6 Canada",
                ManagerName = "Pavan Bhatti",
                Phone = "(604) 472-1260",
                Fax = "(604) 472-1216",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1465,
                Name = "WAL-MART PHARMACY #1113",
                Address = "Ground Level, Unit A100 - 3122 Mt Lehman Rd, Abbotsford BC V2T 0C5 Canada",
                ManagerName = "John Ezema",
                Phone = "(604) 504-2070",
                Fax = "(604) 504-2093",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1466,
                Name = "WAL-MART PHARMACY #1119",
                Address = "31956 Lougheed Hwy, Mission BC V2V 0C6 Canada",
                ManagerName = "Tejinder Bhatt",
                Phone = "(604) 820-4248",
                Fax = "(604) 820-8046",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1467,
                Name = "WAL-MART PHARMACY #1181",
                Address = "5143 Canoe Pass Way, Tsawwassen BC V4M 0B2 Canada",
                ManagerName = "Janet Ha",
                Phone = "(778) 783-5381",
                Fax = "(604) 948-0642",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1468,
                Name = "WAL-MART PHARMACY #1192",
                Address = "Royal City Center, 101 - 610 Sixth St, New Westminster BC V3L 3C2 Canada",
                ManagerName = "Azin Movahhed",
                Phone = "(604) 395-8482",
                Fax = "(604) 395-8304",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1469,
                Name = "WAL-MART PHARMACY #1205",
                Address = "2151 - 10153 King George Blvd, Surrey BC V3T 2W3 Canada",
                ManagerName = "Ravinder Grewal",
                Phone = "(604) 495-8698",
                Fax = "(604) 495-8682",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1470,
                Name = "WAL-MART PHARMACY #1206",
                Address = "11850 224 St, Maple Ridge BC V2X 8S1 Canada",
                ManagerName = "Thanh Thai",
                Phone = "(778) 306-9937",
                Fax = "(604) 467-0916",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1471,
                Name = "WAL-MART PHARMACY #1207",
                Address = "7155 120 St, Delta BC V4E 2B1 Canada",
                ManagerName = "Jasdev Johal",
                Phone = "(604) 595-3618",
                Fax = "(604) 595-5553",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1472,
                Name = "WAL-MART PHARMACY #1208",
                Address = "Coquitlam Centre, 3010 - 2929 Barnet Hwy, Coquitlam BC V3B 5R5 Canada",
                ManagerName = "Wonjip Kim",
                Phone = "(778) 284-3311",
                Fax = "(778) 284-2542",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1473,
                Name = "WAL-MART PHARMACY #1213",
                Address = "4545 Central Blvd, Burnaby BC V5H 4J5 Canada",
                ManagerName = "Wilson Yee",
                Phone = "(778) 328-1121",
                Fax = "(778) 783-4275",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1474,
                Name = "WAL-MART PHARMACY #1214",
                Address = "1644 Hillside Ave, Victoria BC V8T 2C5 Canada",
                ManagerName = "Kirankumar Gautam",
                Phone = "(250) 220-2573",
                Fax = "(250) 410-1251",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1475,
                Name = "WAL-MART PHARMACY #3008",
                Address = "300-9855 Austin Rd, Burnaby BC V3J 1N5 Canada",
                ManagerName = "August Ma",
                Phone = "(604) 421-0353",
                Fax = "(604) 421-5941",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1476,
                Name = "WAL-MART PHARMACY #3019",
                Address = "1812 Vedder Way, Abbotsford BC V2S 8K1 Canada",
                ManagerName = "Fatin Sawalha",
                Phone = "(604) 854-1375",
                Fax = "(604) 854-1477",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1477,
                Name = "WAL-MART PHARMACY #3025",
                Address = "3020 Drinkwater Rd, Duncan BC V9L 6C6 Canada",
                ManagerName = "Ahmed Salem",
                Phone = "(250) 748-1226",
                Fax = "1-855-983-1031",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1478,
                Name = "WAL-MART PHARMACY #3040",
                Address = "Unit #100 - 1055 Hillside Drive, Kamloops BC V2E 2S5 Canada",
                ManagerName = "Michelle Penney",
                Phone = "(250) 374-8874",
                Fax = "(250) 377-0125",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1479,
                Name = "WAL-MART PHARMACY #3042",
                Address = "1555 Banks Rd, Kelowna BC V1X 7Y8 Canada",
                ManagerName = "James Farr",
                Phone = "(250) 860-9145",
                Fax = "(250) 860-8172",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1480,
                Name = "WAL-MART PHARMACY #3057",
                Address = "Capilano Mall, 925 Marine Drive, North Vancouver BC V7P 1S2 Canada",
                ManagerName = "Amanjeet Taggar",
                Phone = "(604) 984-3441",
                Fax = "(604) 984-3345",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1481,
                Name = "WAL-MART PHARMACY #3059",
                Address = "Woodgrove Centre, 6801 Island Hwy N, Nanaimo BC V9T 6N8 Canada",
                ManagerName = "Mark Bahm",
                Phone = "(250) 390-2334",
                Fax = "(250) 390-2611",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1482,
                Name = "WAL-MART PHARMACY #3060",
                Address = "Chahko Mika Mall, 1000 Lakeside Drive, Nelson BC V1L 5Z4 Canada",
                ManagerName = "Hayley Lehnert",
                Phone = "(250) 352-7842",
                Fax = "(250) 352-3090",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1483,
                Name = "WAL-MART PHARMACY #3070",
                Address = "Peachtree Mall, 275 West Green Ave., Penticton BC V2A 7J2 Canada",
                ManagerName = "Ambrose Wong",
                Phone = "(250) 493-6677",
                Fax = "(250) 493-7318",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1484,
                Name = "WAL-MART PHARMACY #3072",
                Address = "Town Centre Mall, #50 - 7100 Alberni Street, Powell River BC V8A 5K9 Canada",
                ManagerName = "Rene Santamaria-Tinoco",
                Phone = "(604) 485-0141",
                Fax = "1-855-983-1054",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1485,
                Name = "WAL-MART PHARMACY #3098",
                Address = "Guildford Town Ctr., Unit 1000, 10355 152 St, Surrey BC V3R 7C3 Canada",
                ManagerName = "Rajdeep Gill",
                Phone = "(604) 585-7440",
                Fax = "(604) 588-3759",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1486,
                Name = "WAL-MART PHARMACY #3109",
                Address = "3460 Saanich Rd, Victoria BC V8Z 0B9 Canada",
                ManagerName = "Hansuk Kim",
                Phone = "(250) 475-7512",
                Fax = "(250) 475-7506",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1487,
                Name = "WAL-MART PHARMACY #3158",
                Address = "A 20202 66th Avenue, Langley BC V2Y 1P3 Canada",
                ManagerName = "Harjit Pannu",
                Phone = "(604) 539-5230",
                Fax = "(604) 539-5290",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1488,
                Name = "WAL-MART PHARMACY #3163",
                Address = "3199 Cliffe Ave, Courtenay BC V9N 2L9 Canada",
                ManagerName = "Harpreet Ghai",
                Phone = "(250) 898-8955",
                Fax = "(250) 898-8954",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1489,
                Name = "WAL-MART PHARMACY #3167",
                Address = "Eagle Landing South, 8249 Eagle Landing Pky, Chilliwack BC V2R 0P9 Canada",
                ManagerName = "Chris Choi",
                Phone = "(604) 792-7638",
                Fax = "(604) 792-7643",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1490,
                Name = "WAL-MART PHARMACY #3169",
                Address = "2200 - 58 Avenue, Vernon BC V1T 9T2 Canada",
                ManagerName = "Amit Soneja",
                Phone = "(250) 558-0562",
                Fax = "(250) 558-0591",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1491,
                Name = "WAL-MART PHARMACY #3183",
                Address = "2100 Willowbrook Drive, Cranbrook BC V1C 7H2 Canada",
                ManagerName = "Shauna Carson",
                Phone = "(250) 489-5338",
                Fax = "(250) 489-5008",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1492,
                Name = "WAL-MART PHARMACY #3188",
                Address = "860 Langford Parkway, Victoria BC V9B 2P3 Canada",
                ManagerName = "Mabel You",
                Phone = "(250) 391-0244",
                Fax = "(250) 391-0813",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1493,
                Name = "WAL-MART PHARMACY #3199",
                Address = "890 Rita Rd, Quesnel BC V2J 7J3 Canada",
                ManagerName = "Gurveer Shergill",
                Phone = "(833) 768-1326",
                Fax = "(250) 747-2368",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1494,
                Name = "WAL-MART PHARMACY #3651",
                Address = "6565 Southridge Ave, Prince George BC V2N 6Z4 Canada",
                ManagerName = "Akhlaq Hakim",
                Phone = "(250) 906-3206",
                Fax = "(250) 906-3263",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1495,
                Name = "WAL-MART PHARMACY #3652",
                Address = "9251 Alderbridge Way, Richmond BC V6X 0N1 Canada",
                ManagerName = "Young Jin Kang",
                Phone = "(604) 288-4396",
                Fax = "(778) 783-5389",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1496,
                Name = "WAL-MART PHARMACY #3661",
                Address = "9007 96A St, Fort St. John BC V1J 7B6 Canada",
                ManagerName = "Oseyi Oseghale",
                Phone = "(250) 261-5585",
                Fax = "(250) 261-5556",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1497,
                Name = "WAL-MART PHARMACY #5776",
                Address = "#600 Highway 2, Dawson Creek BC V1G 4E8 Canada",
                ManagerName = "Gwendolyne Anderson",
                Phone = "(250) 719-0241",
                Fax = "(250) 719-0238",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1498,
                Name = "WAL-MART PHARMACY #5777",
                Address = "A100 805 Boyd St, New Westminster BC V3M 5G7 Canada",
                ManagerName = "Hyunyong Kim",
                Phone = "(604) 524-1264",
                Fax = "(604) 524-1329",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1499,
                Name = "WAL-MART PHARMACY #5834",
                Address = "4427 16 Hwy W, Terrace BC V8G 5L5 Canada",
                ManagerName = "Manpreet Gill",
                Phone = "(250) 615-2047",
                Fax = "(250) 615-3247",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1500,
                Name = "WAL-MART PHARMACY #5838",
                Address = "12451 - 88 Ave, Surrey BC V3W 1P8 Canada",
                ManagerName = "Gurmeet Sukhija",
                Phone = "(604) 597-9169",
                Fax = "(604) 597-6178",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1501,
                Name = "WAL-MART PHARMACY #5853",
                Address = "2355 160 St, Surrey BC V3Z 9N6 Canada",
                ManagerName = "Kiren Toor",
                Phone = "(604) 541-8567",
                Fax = "(604) 541-6074",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1502,
                Name = "WE CARE PHARMACY",
                Address = "100 - 6329 King George Blvd, Surrey BC V3X 1G1 Canada",
                ManagerName = "Trupti Patel",
                Phone = "(604) 593-6924",
                Fax = "(604) 593-6925",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1503,
                Name = "WE CARE PHARMACY #2",
                Address = "13588 88 Ave, Unit #110, Surrey BC V3W 3K8 Canada",
                ManagerName = "Bhavikkumar Patel",
                Phone = "(778) 565-7988",
                Fax = "(778) 565-7989",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1504,
                Name = "WE CARE PHARMACY #3",
                Address = "103 - 6321 King George Blvd, Surrey BC V3X 1G1 Canada",
                ManagerName = "Manmohan Bharaj",
                Phone = "(778) 564-1898",
                Fax = "(778) 564-1897",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1505,
                Name = "WE PHARMACY",
                Address = "B110 - 20487 65 Ave, Langley BC V2Y 3K7 Canada",
                ManagerName = "Hye Ran Seo",
                Phone = "(778) 366-5400",
                Fax = "(778) 366-5401",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1506,
                Name = "WELDON PHARMACY",
                Address = "4676 Hastings St, Burnaby BC V5C 2K5 Canada",
                ManagerName = "Prabhjot Sran",
                Phone = "(604) 365-4444",
                Fax = "(604) 368-4444",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1507,
                Name = "WELL PHARMACY",
                Address = "#106 - 2504 Skaha Lake Road, Penticton BC V2A 6G1 Canada",
                ManagerName = "Christie Crassweller",
                Phone = "(778) 476-1492",
                Fax = "(778) 476-1711",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1508,
                Name = "WELLCARE RX",
                Address = "#107 - 7445 120 St, Delta BC V4C 0B3 Canada",
                ManagerName = "Simranpal Uppal",
                Phone = "(604) 572-7755",
                Fax = "(604) 572-7752",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1509,
                Name = "WELLNESS PHARMACY",
                Address = "#109 - 805 West Broadway, Vancouver BC V5Z 1K1 Canada",
                ManagerName = "Carly Sanderson",
                Phone = "(604) 709-3131",
                Fax = "(604) 709-3121",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1510,
                Name = "WELLNESS PHARMACY #2",
                Address = "#100 - 22314 Fraser Hwy, Langley BC V3A 8M6 Canada",
                ManagerName = "Shelly Liang",
                Phone = "(604) 530-5300",
                Fax = "(604) 530-7250",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1511,
                Name = "WELLNESS PHARMACY NO.  3",
                Address = "103 - 13737 96 Ave, Surrey BC V3V 0C6 Canada",
                ManagerName = "Fayazullah Malik",
                Phone = "(604) 951-1002",
                Fax = "(604) 951-1003",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1512,
                Name = "WELLNESS PHARMACY NO.  4",
                Address = "115 - 6180 Blundell Rd, Richmond BC V7C 4W7 Canada",
                ManagerName = "Seema Alber",
                Phone = "(604) 277-3747",
                Fax = "(604) 277-3748",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1513,
                Name = "WELLNESS PHARMACY NO.  5",
                Address = "5138 Joyce St, Vancouver BC V5R 4H1 Canada",
                ManagerName = "Mostafa Shahin",
                Phone = "(604) 638-0353",
                Fax = "(604) 638-0354",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1514,
                Name = "WELLNESS PHARMACY NO.  6",
                Address = "100 - 1133 Lonsdale Ave, North Vancouver BC V7M 2H4 Canada",
                ManagerName = "Pegah Arasteh",
                Phone = "(604) 971-5400",
                Fax = "(604) 971-5401",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1515,
                Name = "WELLNESS PHARMACY NO.  8",
                Address = "420 Abbott St, Vancouver BC V6B 2L1 Canada",
                ManagerName = "Siamak Anbarani",
                Phone = "(604) 681-2272",
                Fax = "(604) 681-3372",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1516,
                Name = "WELLNESS PHARMACY NO.  9",
                Address = "102 - 2180 Gladwin Rd, Abbotsford BC V2S 0H4 Canada",
                ManagerName = "Viral Sheth",
                Phone = "(604) 859-8883",
                Fax = "(604) 859-8862",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1517,
                Name = "WELLNESS PHARMACY NO. 12",
                Address = "101 - 9123 Mary St, Chilliwack BC V2P 4H7 Canada",
                ManagerName = "Nayan Sharma",
                Phone = "604-795-9501",
                Fax = "604-795-9525",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1518,
                Name = "WESCANA PHARMACY # 1",
                Address = "6686 Fraser St., Vancouver BC V5X 3T5 Canada",
                ManagerName = "Danielle Chan",
                Phone = "(604) 324-6734",
                Fax = "(604) 324-1114",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1519,
                Name = "WESCANA PHARMACY DELTA",
                Address = "#103 - 6935 - 120 St, Delta BC V4E 2A8 Canada",
                ManagerName = "Imran Rajani",
                Phone = "(604) 591-7453",
                Fax = "(604) 591-7463",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1520,
                Name = "WEST 10TH MEDICAL PHARMACY LTD.",
                Address = "4439 West 10th Ave., Vancouver BC V6R 2H8 Canada",
                ManagerName = "Jui-lien Horng",
                Phone = "(604) 222-2028",
                Fax = "(604) 222-2086",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1521,
                Name = "WEST END MEDICINE CENTRE",
                Address = "2004 Eighth Ave., New Westminster BC V3M 2T5 Canada",
                ManagerName = "Mark Labonte",
                Phone = "(604) 522-5636",
                Fax = "(604) 524-6488",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1522,
                Name = "WESTRIDGE PHARMASAVE",
                Address = "105 - 3670 Townline Road, Abbotsford BC V2T 0H2 Canada",
                ManagerName = "Harkirat Gill",
                Phone = "(604) 776-3111",
                Fax = "(604) 776-3110",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1523,
                Name = "WHATCOM PHARMACY AND COMPOUNDING",
                Address = "106 - 2100 Whatcom Rd, Abbotsford BC V3G 2K8 Canada",
                ManagerName = "Herman Gill",
                Phone = "(604) 744-1266",
                Fax = "(604) 744-1267",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1524,
                Name = "WHITE ROCK PHARMACY",
                Address = "102 - 1440 George St, White Rock BC V4B 4A3 Canada",
                ManagerName = "Jatinkumar Patel",
                Phone = "(604) 542-4878",
                Fax = "(604) 542-4895",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1525,
                Name = "WHOLE HEALTH PHARMACY",
                Address = "101 - 6007 Southridge Avenue, Prince George BC V2N 6Z4 Canada",
                ManagerName = "Berdine Fazakas",
                Phone = "(778) 416-5010",
                Fax = "(778) 693-3624",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1526,
                Name = "WILLOUGHBY IDA PHARMACY",
                Address = "A110 - 20161 86 Ave, Langley BC V2Y 2C1 Canada",
                ManagerName = "Khaled Anany",
                Phone = "(604) 371-1114",
                Fax = "(604) 371-1117",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1527,
                Name = "WILLOWBROOK PHARMACY",
                Address = "158B - 19653 Willowbrook Dr, Langley BC V2Y 1A5 Canada",
                ManagerName = "Shaheer Muhammad",
                Phone = "(604) 530-9888",
                Fax = "(604) 530-9828",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1528,
                Name = "WILSON PHARMACY",
                Address = "#3 - 2185 Wilson Ave., Port Coquitlam BC V3C 6C1 Canada",
                ManagerName = "Rajnikant Rakholiya",
                Phone = "(604) 942-4611",
                Fax = "(604) 942-1554",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1529,
                Name = "WILSON PHARMACY NO. 2",
                Address = "709 - 2071 Kingsway Ave, Port Coquitlam BC V3C 6N2 Canada",
                ManagerName = "Parmjeet Johal",
                Phone = "(778) 285-2800",
                Fax = "(604) 554-0201",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1530,
                Name = "WINFIELD IDA PHARMACY",
                Address = "50 - 9522 Main St, Lake Country BC V4V 2L9 Canada",
                ManagerName = "Danielle Schaeffer",
                Phone = "(250) 766-2666",
                Fax = "(250) 766-2608",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1531,
                Name = "WINMED PHARMACY",
                Address = "#104 - 32450 Simon Ave, Abbotsford BC V2T 4J2 Canada",
                ManagerName = "Craig Pudlas",
                Phone = "(604) 746-0201",
                Fax = "(604) 746-0203",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1532,
                Name = "YALE PHARMACY",
                Address = "1284 Granville St, Vancouver BC V6Z 1M4 Canada",
                ManagerName = "Aida Bilalovic",
                Phone = "(604) 692-0211",
                Fax = "(604) 683-0211",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1533,
                Name = "YALE ROAD PHARMACY",
                Address = "101 - 46198 Yale Rd, Chilliwack BC V2P 2P1 Canada",
                ManagerName = "Charan Singh",
                Phone = "(604) 795-1157",
                Fax = "(604) 702-0656",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1534,
                Name = "YNP DUNBAR PHARMACY - YOUR NEIGHBORHOOD PHARMACY",
                Address = "4198 Dunbar St, Vancouver BC V6S 2E7 Canada",
                ManagerName = "Pourya Eslami",
                Phone = "(604) 730-1788",
                Fax = "(604) 730-1789",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1535,
                Name = "YNP FRASER PHARMACY",
                Address = "3373 Fraser Street, Vancouver BC V5V 4C2 Canada",
                ManagerName = "Vanessa Cheng",
                Phone = "(604) 669-4364",
                Fax = "(604) 669-4308",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1536,
                Name = "YORK PHARMACY",
                Address = "110 - 7938 128 St, Surrey BC V3W 4E8 Canada",
                ManagerName = "Grace Kim",
                Phone = "(604) 598-4679",
                Fax = "(604) 598-4684",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1537,
                Name = "YORKSON PHARMACY",
                Address = "140-20144 86 Ave, Langley BC V2Y 3W6 Canada",
                ManagerName = "Hon Shing Lam",
                Phone = "(778) 366-3552",
                Fax = "(778) 366-3559",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1538,
                Name = "YYOUNG MEDICINE MART",
                Address = "5570 Cambie St, Vancouver BC V5Z 3A2 Canada",
                ManagerName = "Winni Ye",
                Phone = "(604) 324-3848",
                Fax = "(604) 324-1727",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1539,
                Name = "YYOUNG MEDICINE MART MAIN",
                Address = "1721 Main St, Vancouver BC V5T 3B5 Canada",
                ManagerName = "Lisa Le",
                Phone = "(604) 658-8881",
                Fax = "(604) 630-1001",
                IsCareConnectCompleted = true
            },
            new Pharmacy {
                Id = 1540,
                Name = "ZEN PHARMACY",
                Address = "2424 St Johns St, Port Moody BC V3H 2B1 Canada",
                ManagerName = "Simrit Khatra",
                Phone = "(604) 937-6069",
                Fax = "(604) 634-7502",
                IsCareConnectCompleted = true
            }
        );
    }
}

