using Microsoft.AspNetCore.Mvc.Testing;
using RestEase.Implementation;
using RestEase.SampleWebApi.Controllers;
using RestEase.Serialization.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace RestEase.SampleWebApi.Tests
{
	public class SampleControllerTests
	{
		private static readonly WebApplicationFactory<Startup> _factory = new WebApplicationFactory<Startup>();
		private readonly ISampleController _controller;

		public SampleControllerTests()
		{
			var client = _factory.CreateClient();
			var requester = new Requester(client)
			{
				RequestQueryParamSerializer = new ComplexTypeRequestQueryParamSerializer(),
			};
			_controller = RestClient.For<ISampleController>(requester);
		}

		[Fact]
		public async Task GetSampleValue_Success()
		{
			var value = await _controller.GetSampleValue(); 
			Assert.Equal("To get this value we had to do some really hard work", value);
		}
	}
}
