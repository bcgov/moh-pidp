The manual intake utility presented here is copied as closley as possible from the implementation found in the PRIME application:
https://github.com/bcgov/moh-prime/blob/0180c1d79840dcf1d2205154a9449823305d97a7/utilities/PlrIntakeUtility/PlrIntakeUtility.csproj

Updates are mainly to conform the code to match the coding style of PIdP.


To run:  `dotnet run PRIME_Test_Data_PLR_IAT20210617_v2.0.csv intake.log` (Expecting path to .csv file and desired log file)

To target a different database, modify the connection string in `appsettings.json` as necessary to point at another database. 
