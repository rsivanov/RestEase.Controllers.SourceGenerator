using System.Threading.Tasks;

namespace RestEase.SampleWebApi.Services
{
	public interface ISampleService
	{
		Task<string> GetSampleValue();
	}
}