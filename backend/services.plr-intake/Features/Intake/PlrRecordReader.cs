namespace PlrIntake.Features.Intake.RecordReader;

using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;

using PlrIntake.Models;

public class PlrRecordReader
{
    private const string Prefix = IntakeServiceOperationTuner.Prefix;

    private readonly ILogger logger;
    private readonly XmlNamespaceManager nsManager;

    public PlrRecordReader(ILogger logger)
    {
        this.logger = logger;

        this.nsManager = new XmlNamespaceManager(new NameTable());
        this.nsManager.AddNamespace(Prefix, IntakeServiceOperationTuner.Uri);
    }

    public PlrRecord ReadDistributionMessage(string messageContent)
    {
        var doc = new XmlDocument();
        var strReader = new StringReader(messageContent);
        doc.Load(strReader);
        XmlNode documentRoot = doc.DocumentElement!;

        var messageId = this.ReadNodeData($"//{Prefix}:id[@root='2.16.840.1.113883.3.40.1.5']/@extension", documentRoot);
        if (messageId == null)
        {
            this.logger.LogIdNotFound(messageContent);
            throw new ArgumentException("Message id missing.");
        }
        var internalProviderCode = this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:id[@root='2.16.840.1.113883.3.40.2.8']/@extension", documentRoot, messageId);
        if (internalProviderCode == null)
        {
            this.logger.LogIpcNotFound(messageId);
            throw new ArgumentException("IPC missing.");
        }
        this.logger.LogMessageRecieved(messageId, internalProviderCode);

        // Ignore CPN, IPC, and MPID respectively
        const string nonCollegeIdXPathExpr = "not (@root='2.16.840.1.113883.3.40.2.3') and not (@root='2.16.840.1.113883.3.40.2.8') and not (@root='2.16.840.1.113883.3.40.2.11')";
        const string postalWorkplaceUseExpr = "@use='PST WP'";

#pragma warning disable IDE0017 // Object initialization syntax is too messy here
        var result = new PlrRecord();
#pragma warning restore IDE0017

        // Primary attributes
        result.Ipc = internalProviderCode;
        // At this point, IdentifierType is OID
        result.IdentifierType = this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:id[{nonCollegeIdXPathExpr}]/@root", documentRoot, messageId);
        result.CollegeId = this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:id[{nonCollegeIdXPathExpr}]/@extension", documentRoot, messageId);
        result.ProviderRoleType = this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:code/@code", documentRoot, messageId);
        result.FirstName = this.ReadNodeData($"//{Prefix}:healthCarePrincipalPerson/{Prefix}:name[@use='L']/{Prefix}:given[1]", documentRoot, messageId);
        result.SecondName = this.ReadNodeData($"//{Prefix}:healthCarePrincipalPerson/{Prefix}:name[@use='L']/{Prefix}:given[2]", documentRoot, messageId);
        result.ThirdName = this.ReadNodeData($"//{Prefix}:healthCarePrincipalPerson/{Prefix}:name[@use='L']/{Prefix}:given[3]", documentRoot, messageId);
        result.LastName = this.ReadNodeData($"//{Prefix}:healthCarePrincipalPerson/{Prefix}:name[@use='L']/{Prefix}:family", documentRoot, messageId);
        result.DateOfBirth = ParseHL7v3DateTime(this.ReadNodeData($"//{Prefix}:healthCarePrincipalPerson/{Prefix}:birthTime/@value", documentRoot, messageId));
        result.StatusCode = this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:statusCode/@code", documentRoot, messageId);
        result.StatusReasonCode = this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:subjectOf2/{Prefix}:roleActivation/{Prefix}:reasonCode/@code", documentRoot, messageId);
        result.ConditionCode = this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:responsibleFor/{Prefix}:privilege/{Prefix}:code/@code", documentRoot, messageId);
        result.ConditionStartDate = ParseHL7v3DateTime(this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:responsibleFor/{Prefix}:privilege/{Prefix}:effectiveTime/{Prefix}:low/@value", documentRoot, messageId));
        result.ConditionEndDate = ParseHL7v3DateTime(this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:responsibleFor/{Prefix}:privilege/{Prefix}:effectiveTime/{Prefix}:high/@value", documentRoot, messageId));

        // Secondary attributes
        result.Cpn = this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:id[@root='2.16.840.1.113883.3.40.2.3']/@extension", documentRoot, messageId);
        result.Address1Line1 = this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:addr[{postalWorkplaceUseExpr}]/{Prefix}:streetAddressLine[1]", documentRoot, messageId);
        result.Address1Line2 = this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:addr[{postalWorkplaceUseExpr}]/{Prefix}:streetAddressLine[2]", documentRoot, messageId);
        result.Address1Line3 = this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:addr[{postalWorkplaceUseExpr}]/{Prefix}:streetAddressLine[3]", documentRoot, messageId);
        result.City1 = this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:addr[{postalWorkplaceUseExpr}]/{Prefix}:city", documentRoot, messageId);
        result.Province1 = this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:addr[{postalWorkplaceUseExpr}]/{Prefix}:state", documentRoot, messageId);
        result.Country1 = this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:addr[{postalWorkplaceUseExpr}]/{Prefix}:country", documentRoot, messageId);
        result.PostalCode1 = this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:addr[{postalWorkplaceUseExpr}]/{Prefix}:postalCode", documentRoot, messageId);
        result.Address1StartDate = ParseHL7v3DateTime(this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:addr[{postalWorkplaceUseExpr}]/{Prefix}:useablePeriod/{Prefix}:low/@value", documentRoot, messageId));

        // According to PLR team, Credentials will have a `reference` child node (i.e. designation text) ...
        var credentials = this.ReadMultiNodeData($"//{Prefix}:healthCareProvider/{Prefix}:relatedTo/{Prefix}:qualifiedEntity/{Prefix}:code[{Prefix}:originalText/{Prefix}:reference]/@code", documentRoot, messageId)
            ?? Enumerable.Empty<string>();
        result.Credentials = credentials.Select(x => new Credential(x)).ToList();

        var emailData = this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:telecom[@use='WP' and starts-with(@value, 'mailto')]/@value", documentRoot, messageId);
        if (emailData != null)
        {
            result.Email = RemoveHL7v3TelecomType(emailData);
        }

        // ... but Expertises will never have a `reference` child node
        var expertise = this.ReadMultiNodeData($"//{Prefix}:healthCareProvider/{Prefix}:relatedTo/{Prefix}:qualifiedEntity/{Prefix}:code[not({Prefix}:originalText/{Prefix}:reference)]/@code", documentRoot, messageId)
            ?? Enumerable.Empty<string>();
        result.Expertise = expertise.Select(x => new Expertise(x)).ToList();

        var faxNumberData = this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:telecom[@use='WP' and starts-with(@value, 'fax')]/@value", documentRoot, messageId);
        if (faxNumberData != null)
        {
            var faxNumberParts = SplitTelecomNumber(RemoveHL7v3TelecomType(faxNumberData));
            if (faxNumberParts.Length == 2)
            {
                result.FaxAreaCode = faxNumberParts[0];
                result.FaxNumber = faxNumberParts[1];
            }
            else
            {
                result.FaxNumber = faxNumberParts[0];
            }
        }

        result.Gender = this.ReadNodeData($"//{Prefix}:healthCarePrincipalPerson/{Prefix}:administrativeGenderCode/@code", documentRoot, messageId);
        result.MspId = this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:id[@root='2.16.840.1.113883.3.40.2.11']/@extension", documentRoot, messageId);
        result.NamePrefix = this.ReadNodeData($"//{Prefix}:healthCarePrincipalPerson/{Prefix}:name[@use='L']/{Prefix}:prefix", documentRoot, messageId);
        result.StatusStartDate = ParseHL7v3DateTime(this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:statusCode/{Prefix}:validTime/{Prefix}:low/@value", documentRoot, messageId));
        result.StatusExpiryDate = ParseHL7v3DateTime(this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:statusCode/{Prefix}:validTime/{Prefix}:high/@value", documentRoot, messageId));
        result.Suffix = this.ReadNodeData($"//{Prefix}:healthCarePrincipalPerson/{Prefix}:name[@use='L']/{Prefix}:suffix", documentRoot, messageId);
        var telephoneNumData = this.ReadNodeData($"//{Prefix}:healthCareProvider/{Prefix}:telecom[@use='WP' and starts-with(@value, 'tel')]/@value", documentRoot, messageId);
        if (telephoneNumData != null)
        {
            var telephoneNumberParts = SplitTelecomNumber(RemoveHL7v3TelecomType(telephoneNumData));
            if (telephoneNumberParts.Length == 2)
            {
                result.TelephoneAreaCode = telephoneNumberParts[0];
                result.TelephoneNumber = telephoneNumberParts[1];
            }
            else
            {
                result.TelephoneNumber = telephoneNumberParts[0];
            }
        }
        return result;
    }

    public static DateTime? ParseHL7v3DateTime(string? dateString)
    {
        if (dateString != null)
        {
            return DateTime.TryParseExact(dateString, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date : null;
        }
        else
        {
            return null;
        }
    }

    public static string RemoveHL7v3TelecomType(string telecomValue)
    {
        var colonIndex = telecomValue.IndexOf(':');
        return colonIndex != -1 ? telecomValue[(colonIndex + 1)..] : telecomValue;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="telecomNumber">Expects a 10-digit string but works with other input</param>
    /// <returns>A 2-element array containing area code and local number, if input was a 10-digit string.
    ///     Otherwise, simply returns <c>telecomNumber</c> as the single element in the array</returns>
    public static string?[] SplitTelecomNumber(string telecomNumber)
    {
        var allDigitsRegex = new Regex("^[0-9]+$");
        if (telecomNumber != null && telecomNumber.Length == 10 && allDigitsRegex.IsMatch(telecomNumber))
        {
            return new string[] { telecomNumber[0..3], telecomNumber[3..] };
        }
        else
        {
            return new string[] { telecomNumber! };
        }
    }

    private string? ReadNodeData(string xPath, XmlNode documentRoot, string? messageId = null)
    {
        var node = documentRoot.SelectSingleNode(xPath, this.nsManager);
        if (node != null)
        {
            this.logger.LogDebugXml(node.InnerXml);
            return node.InnerXml;
        }
        else
        {
            this.logger.LogNoMatchingXml(xPath, messageId);
            return null;
        }
    }

    private string[]? ReadMultiNodeData(string xPath, XmlNode documentRoot, string? messageId = null)
    {
        var nodes = documentRoot.SelectNodes(xPath, this.nsManager)!;
        if (nodes.Count != 0)
        {
            var results = new List<string>();
            foreach (XmlNode node in nodes)
            {
                this.logger.LogDebugXml(node.InnerXml);
                results.Add(node.InnerXml);
            }
            return results.ToArray();
        }
        else
        {
            this.logger.LogNoMatchingXml(xPath, messageId);
            return null;
        }
    }
}

public static partial class PlrRecordReaderLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "No ID was found for the message {messageContent}")]
    public static partial void LogIdNotFound(this ILogger logger, string messageContent);

    [LoggerMessage(2, LogLevel.Error, "Mandatory IPC was not found for the message '{messageId}'")]
    public static partial void LogIpcNotFound(this ILogger logger, string messageId);

    [LoggerMessage(3, LogLevel.Information, "Message received with ID {messageId} for provider with IPC of {internalProviderCode}.")]
    public static partial void LogMessageRecieved(this ILogger logger, string messageId, string internalProviderCode);

    [LoggerMessage(4, LogLevel.Debug, "{innerXml}")]
    public static partial void LogDebugXml(this ILogger logger, string innerXml);

    [LoggerMessage(5, LogLevel.Warning, "{xPath} did not match anything in the message with ID '{messageId}'")]
    public static partial void LogNoMatchingXml(this ILogger logger, string xPath, string? messageId);
}
