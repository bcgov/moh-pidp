namespace Pidp.Infrastructure.HttpClients.Mail;

/// <summary>
/// An abstraction of a document in memory
/// </summary>
public class File
{
    public string Filename { get; set; }
    public byte[] Data { get; set; }
    public string MediaType { get; set; }

    public File(string filename, byte[] data, string mediaType)
    {
        this.Filename = filename;
        this.Data = data;
        this.MediaType = mediaType;
    }
}

public class Pdf : File
{
    public Pdf(string filename, byte[] data)
        : base(filename, data, "application/pdf")
    { }
}
