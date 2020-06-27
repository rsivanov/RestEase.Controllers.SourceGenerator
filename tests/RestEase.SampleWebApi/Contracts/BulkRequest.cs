namespace RestEase.SampleWebApi.Contracts
{
	public class BulkRequest
	{
		public string Value { get; set; }
		public int[] Ids { get; set; }
	}
}
