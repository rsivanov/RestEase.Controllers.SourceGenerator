using Microsoft.AspNetCore.Mvc.Testing;
using RestEase.Implementation;
using RestEase.SampleWebApi.Contracts;
using RestEase.SampleWebApi.Controllers;
using RestEase.SampleWebApi.Infrastructure;
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
				RequestQueryParamSerializer = new ComplexRequestQueryParamSerializer(),
				RequestBodySerializer = new FileContentRequestBodySerializer(),
				ResponseDeserializer = new FileContentResponseDeserializer()
			};
			_controller = RestClient.For<IEmployeesController>(requester);
		}

		[Fact]
		public async Task GetByEnumFromQueryExistingValue()
		{
			Assert.Equal(MyEnum.ExistingValue, await _controller.GetByEnumFromQuery(MyEnum.ExistingValue));
		}

		[Fact]
		public async Task GetByEnumFromPathExistingValue()
		{
			Assert.Equal(MyEnum.ExistingValue, await _controller.GetByEnumFromPath(MyEnum.ExistingValue));
		}

		[Fact]
		public async Task GetEmployee_Always_FromQuerySuccess()
		{
			var request = new BulkRequest { Ids = new[] { 1, 2, 3 }, Value = "Qwerty" };
			var response = await _controller.GetEmployeeByRequest(request);
			Assert.Equal(request.Value, response.Value);
			Assert.True(request.Ids.SequenceEqual(response.Ids));
		}

		[Fact]
		public async Task GetEmployees_Always_Returns10Employees()
		{
			var employees = await _controller.GetEmployees();
			Assert.True(employees.Count() == 10);
		}

		[Fact]
		public async Task GetEmployee_Always_NotNull()
		{
			var employee = await _controller.GetEmployee(1);
			Assert.NotNull(employee);
		}

		[Fact]
		public async Task GetEmployee_Always_Success()
		{
			var request = new BulkRequest { Value = "Qwerty" };
			var response = await _controller.GetEmployeeByRequest(request);
			Assert.Equal(request.Value, response.Value);
			Assert.Null(response.Ids);
		}

		[Fact]
		public async Task UpdateEmployee_Always_WithoutErrors()
		{
			await _controller.UpdateEmployee(1, new Employee());
		}

		[Fact]
		public async Task DeleteEmployee_Always_WithoutErrors()
		{
			await _controller.DeleteEmployee(1);
		}

		[Fact]
		public async Task DeleteEmployeeFromQuery_Always_WithoutErrors()
		{
			await _controller.DeleteEmployeeFromQuery(new Employee());
		}

		[Fact]
		public async Task DownloadFile_Always_CorrectTransfer()
		{
			var downloadable = await _controller.Download(1);
			Assert.True(downloadable.Content.Length > default(int) && !string.IsNullOrEmpty(downloadable.FileName) &&
				!string.IsNullOrEmpty(downloadable.MimeType));
		}

		[Fact]
		public async Task UploadFile_Always_CorrectTransfer()
		{
			var downloadable = new FileContent
			{
				Content = Encoding.Default.GetBytes("123"),
				FileName = "password.txt",
				MimeType = "text/plain"
			};

			var fileId = await _controller.Upload(downloadable);
			Assert.True(fileId > default(int));
		}
	}
}
