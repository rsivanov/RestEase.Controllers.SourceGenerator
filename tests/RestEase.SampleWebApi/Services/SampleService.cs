using System.Threading.Tasks;

namespace RestEase.SampleWebApi.Services
{
	public class SampleService : ISampleService
	{
		public Task<string> GetSampleValue()
		{
			return Task.FromResult("To get this value we had to do some really hard work");
		}
	}
}