namespace Pidp.Infrastructure.HttpClients.Mail;

/// <summary>
/// An abstraction of a document in memory
/// </summary>
public class File(string filename, byte[] data, string mediaType)
{
    public string Filename { get; set; } = filename;
    public byte[] Data { get; set; } = data;
    public string MediaType { get; set; } = mediaType;
}
