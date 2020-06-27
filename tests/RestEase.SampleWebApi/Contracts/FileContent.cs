namespace RestEase.SampleWebApi.Contracts
{
	public class FileContent
	{
		public byte[] Content { get; set; }
		public string MimeType { get; set; }
		public string FileName { get; set; }
	}
}
