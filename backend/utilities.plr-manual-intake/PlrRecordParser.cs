namespace PlrManualIntake;

using CsvHelper;
using Serilog;

using PlrIntake.Models;

public static class PlrRecordParser
{
    private static readonly DateTime PlrNullDateTime = new(9999, 12, 30);

    /// <summary>
    /// Reads from a row in Excel file into a PlrRecord object.
    /// </summary>
    public static PlrRecord ReadRow(CsvReader reader)
    {
        var provider = new PlrRecord
        {
            Ipc = GetString(reader, GetIndex("A"))!,
            Cpn = GetString(reader, GetIndex("B")),
            IdentifierType = GetString(reader, GetIndex("C")),
            CollegeId = GetString(reader, GetIndex("D")),
            ProviderRoleType = GetString(reader, GetIndex("E")),
            MspId = GetString(reader, GetIndex("F")),
            NamePrefix = GetString(reader, GetIndex("G")),
            FirstName = GetString(reader, GetIndex("H")),
            SecondName = GetString(reader, GetIndex("I")),
            ThirdName = GetString(reader, GetIndex("J")),
            LastName = GetString(reader, GetIndex("K")),
            Suffix = GetString(reader, GetIndex("L")),
            Gender = GetString(reader, GetIndex("M")),
            DateOfBirth = TryGetDateTime(reader, "N"),
            StatusCode = GetString(reader, GetIndex("O")),
            StatusReasonCode = GetString(reader, GetIndex("P")),
            StatusStartDate = TryGetDateTime(reader, "Q"),
            StatusExpiryDate = TryGetDateTime(reader, "R"),
            Expertise = GetMultipleElements(GetString(reader, GetIndex("S")))
                .Select(x => new Expertise(x))
                .ToList(),
            // Not collecting Languages
            // provider.Languages = GetString(reader, GetIndex("T"));
            Address1Line1 = GetString(reader, GetIndex("U")),
            Address1Line2 = GetString(reader, GetIndex("V")),
            Address1Line3 = GetString(reader, GetIndex("W")),
            City1 = GetString(reader, GetIndex("X")),
            Province1 = GetString(reader, GetIndex("Y")),
            Country1 = GetString(reader, GetIndex("Z")),
            PostalCode1 = GetString(reader, GetIndex("AA")),
            Address1StartDate = TryGetDateTime(reader, "AB"),
            Credentials = GetMultipleElements(GetString(reader, GetIndex("AC")))
                .Select(x => new Credential(x))
                .ToList(),
            TelephoneAreaCode = GetString(reader, GetIndex("AD")),
            TelephoneNumber = GetString(reader, GetIndex("AE")),
            FaxAreaCode = GetString(reader, GetIndex("AF")),
            FaxNumber = GetString(reader, GetIndex("AG")),
            Email = GetString(reader, GetIndex("AH")),
            ConditionCode = GetString(reader, GetIndex("AI")),
            ConditionStartDate = TryGetDateTime(reader, "AJ"),
            ConditionEndDate = TryGetDateTime(reader, "AK")
        };

        return provider;
    }

    /// <summary>
    /// Returns `null` if cell is empty
    /// </summary>
    private static string? GetString(CsvReader reader, int colIndex)
    {
        var value = reader.GetField<string>(colIndex);
        return string.IsNullOrEmpty(value) ? null : value;
    }

    /// <summary>
    /// Returns DateTime representing cell value, or `null` if cell is empty
    /// </summary>
    private static DateTime? TryGetDateTime(CsvReader reader, string columnId)
    {
        var dateTime = reader.GetField<DateTime?>(GetIndex(columnId));
        if (dateTime == null)
        {
            return null;
        }
        // Treat value meant to represent NULL as `null`
        else if (dateTime.Equals(PlrNullDateTime))
        {
            return null;
        }
        else
        {
            return dateTime;
        }
    }

    /// <param name="aValue">A pipe-delimited string</param>
    /// <returns>Array containing parsed elements or an empty array if given input is <c>null</c>.</returns>
    private static string[] GetMultipleElements(string? aValue)
    {
        if (aValue == null)
        {
            return Array.Empty<string>();
        }

        return aValue.Split('|');
    }

    /// <summary>
    /// Check fields that are expected to be populated, logging error if not populated.
    /// </summary>
    public static void CheckData(PlrRecord provider, int rowNum)
    {
        CheckRequiredField(provider.Ipc, nameof(provider.Ipc), rowNum);
        CheckRequiredField(provider.IdentifierType, nameof(provider.IdentifierType), rowNum);
        CheckRequiredField(provider.CollegeId, nameof(provider.CollegeId), rowNum);
        CheckRequiredField(provider.ProviderRoleType, nameof(provider.ProviderRoleType), rowNum);
        CheckRequiredField(provider.FirstName, nameof(provider.FirstName), rowNum);
        CheckRequiredField(provider.LastName, nameof(provider.LastName), rowNum);
        CheckRequiredField(provider.Gender, nameof(provider.Gender), rowNum);
        CheckRequiredField(provider.DateOfBirth, nameof(provider.DateOfBirth), rowNum);
        CheckRequiredField(provider.StatusCode, nameof(provider.StatusCode), rowNum);
        CheckRequiredField(provider.StatusReasonCode, nameof(provider.StatusReasonCode), rowNum);
        CheckRequiredField(provider.StatusStartDate, nameof(provider.StatusStartDate), rowNum);
        CheckRequiredField(provider.Address1Line1, nameof(provider.Address1Line1), rowNum);
        CheckRequiredField(provider.City1, nameof(provider.City1), rowNum);
        CheckRequiredField(provider.Province1, nameof(provider.Province1), rowNum);
        CheckRequiredField(provider.PostalCode1, nameof(provider.PostalCode1), rowNum);
        CheckRequiredField(provider.Address1StartDate, nameof(provider.Address1StartDate), rowNum);
        CheckRequiredField(provider.TelephoneAreaCode, nameof(provider.TelephoneAreaCode), rowNum);
        CheckRequiredField(provider.TelephoneNumber, nameof(provider.TelephoneNumber), rowNum);
        CheckRequiredField(provider.Email, nameof(provider.Email), rowNum);
    }

    private static bool CheckRequiredField(object? fieldValue, string fieldName, int rowNum)
    {
        if (fieldValue == null || fieldValue.Equals(DateTime.MinValue))
        {
            Log.Warning($"'{fieldName}' was empty at row number {rowNum}.");
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Algorithm courtesy of https://stackoverflow.com/questions/667802/what-is-the-algorithm-to-convert-an-excel-column-letter-into-its-number
    /// </summary>
    /// <param name="excelColumn">For example, "AC"</param>
    /// <returns>Zero-based index, e.g. 28</returns>
    public static int GetIndex(string excelColumn)
    {
        if (string.IsNullOrEmpty(excelColumn))
        {
            throw new ArgumentNullException(nameof(excelColumn));
        }

        excelColumn = excelColumn.ToUpperInvariant();

        var sum = 0;

        for (var i = 0; i < excelColumn.Length; i++)
        {
            sum *= 26;
            sum += excelColumn[i] - 'A' + 1;
        }

        // Zero-based indexing
        return sum - 1;
    }
}
