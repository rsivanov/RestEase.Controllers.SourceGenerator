using Microsoft.AspNetCore.Mvc.Testing;
using RestEase.Implementation;
using RestEase.SampleWebApi.Contracts;
using RestEase.SampleWebApi.Controllers;
using RestEase.SampleWebApi.Infrastructure;
using RestEase.Serialization.Extensions;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RestEase.SampleWebApi.Tests
{
	public class EmployeesControllerTests
	{
		private static readonly WebApplicationFactory<Startup> _factory = new WebApplicationFactory<Startup>();
		private readonly IEmployeesController _controller;

		public EmployeesControllerTests()
		{
			var client = _factory.CreateClient();
			var requester = new Requester(client)
			{
				RequestQueryParamSerializer = new ComplexTypeRequestQueryParamSerializer(),
				RequestBodySerializer = new FileContentRequestBodySerializer(),
				ResponseDeserializer = new FileContentResponseDeserializer()
			};
			_controller = RestClient.For<IEmployeesController>(requester);
		}

		[Fact]
		public async Task GetByEnumFromQuery_Success()
		{
			var response = await _controller.GetByEnumFromQuery(MyEnum.ExistingValue);
			Assert.Equal(MyEnum.ExistingValue, response);
		}

		[Fact]
		public async Task GetByEnumFromRoute_Success()
		{
			var response = await _controller.GetByEnumFromRoute(MyEnum.ExistingValue);
			Assert.Equal(MyEnum.ExistingValue, response);
		}

		[Fact]
		public async Task GetEmployeeByRequest_Success()
		{
			var request = new BulkRequest { Ids = new[] { 1, 2, 3 }, Value = "Qwerty" };
			var response = await _controller.GetEmployeeByRequest(request);
			Assert.Equal(request.Value, response.Value);
			Assert.True(request.Ids.SequenceEqual(response.Ids));
		}

		[Fact]
		public async Task GetEmployees_Returns10Employees()
		{
			var employees = await _controller.GetEmployees();
			Assert.Equal(10, employees.Count());
		}

		[Fact]
		public async Task GetEmployee_Success()
		{
			var employee = await _controller.GetEmployee(1);
			Assert.NotNull(employee);
		}

		[Fact]
		public async Task UpdateEmployee_Success()
		{
			await _controller.UpdateEmployee(1, new Employee());
		}

		[Fact]
		public async Task DeleteEmployee_Success()
		{
			await _controller.DeleteEmployee(1);
		}

		[Fact]
		public async Task DeleteEmployeeFromQuery_Success()
		{
			await _controller.DeleteEmployeeFromQuery(new Employee());
		}

		[Fact]
		public async Task Download_Success()
		{
			var fileContent = await _controller.Download(1);
			Assert.NotNull(fileContent);
			Assert.NotEmpty(fileContent.Content);
			Assert.False(string.IsNullOrEmpty(fileContent.FileName));
			Assert.False(string.IsNullOrEmpty(fileContent.MimeType));
		}

		[Fact]
		public async Task Upload_Success()
		{
			var fileContent = new FileContent
			{
				Content = Encoding.Default.GetBytes("123"),
				FileName = "password.txt",
				MimeType = "text/plain"
			};

			var fileId = await _controller.Upload(fileContent);
			Assert.True(fileId > default(int));
		}
	}
}
