using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestEase.SampleWebApi.Services;

namespace RestEase.SampleWebApi.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	[Route("[controller]")]
	public class SampleController : ControllerBase
	{
		private readonly ISampleService _sampleService;

		public SampleController(ISampleService sampleService)
		{
			_sampleService = sampleService;
		}

		[HttpGet]
		public Task<string> GetSampleValue()
		{
			return _sampleService.GetSampleValue();
		}
	}
}