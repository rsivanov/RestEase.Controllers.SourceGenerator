using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using RestEase.SampleWebApi.Contracts;
using RestEase.SampleWebApi.Infrastructure;

namespace RestEase.SampleWebApi.Controllers
{
	[Route("[controller]")]
	public class EmployeesController : Controller
	{
		[HttpGet]
		public Task<IEnumerable<Employee>> GetEmployees()
		{
			var result = Enumerable.Range(1, 10).Select(x => new Employee {Id = x, FullName = "User " + x});
			return Task.FromResult(result);
		}

		[HttpGet("{id}")]
		public Task<Employee> GetEmployee(int id)
		{
			return Task.FromResult(new Employee { Id = id, FullName = "User " + id });
		}

		[HttpGet("bulk")]
		public Task<BulkRequest> GetEmployeeByRequest([FromQuery] BulkRequest bulkRequest) => Task.FromResult(bulkRequest);

		[HttpPost]
		public Task<int> CreateEmployee([FromBody] Employee employee) => Task.FromResult(123);

		[HttpPut("{id}")]
		public Task UpdateEmployee(int id, [FromBody] Employee employee) => Task.CompletedTask;

		[HttpDelete("{id}")]
		public Task DeleteEmployee(int id) => Task.CompletedTask;

		[HttpDelete("deleteEmployeeFromQuery")]
		public Task DeleteEmployeeFromQuery([FromQuery] Employee employee) => Task.CompletedTask;

		[HttpGet("byEnumFromQuery")]
		public Task<MyEnum> GetByEnumFromQuery([FromQuery] MyEnum myEnum) => Task.FromResult(myEnum);

		[HttpGet("byEnum/{myEnum}")]
		public Task<MyEnum> GetByEnumFromRoute([FromRoute] MyEnum myEnum) => Task.FromResult(myEnum);

		[HttpGet("ByEnumFromRequest")]
		public Task<MyEnum> GetByEnumFromRequest([FromQuery] EnumRequest myEnumRequest) => Task.FromResult(myEnumRequest.MyEnum);

		[HttpGet("{id}/download"), FileContentResultFilter]
		public Task<FileContent> Download(int id)
		{
			var fileContent = new FileContent { Content = Encoding.Default.GetBytes("123"), FileName = "password.txt", MimeType = "text/plain" };
			return Task.FromResult(fileContent);
		}

		[HttpPost("upload")]
		public Task<int> Upload([FromBody] [ModelBinder(BinderType = typeof(FileContentModelBinder))]
			FileContent downloadable) => Task.FromResult(downloadable.Content.Length);
	}
}