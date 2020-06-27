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
		public async Task<IEnumerable<Employee>> GetEmployees()
		{
			await Task.Yield();
			return Enumerable.Range(1, 10).Select(x => new Employee { Id = x, FullName = "User " + x });
		}

		[HttpGet("{id}")]
		public async Task<Employee> GetEmployee(int id)
		{
			await Task.Yield();
			return new Employee { Id = id, FullName = "User " + id };
		}

		[HttpGet("employee")]
		public async Task<BulkRequest> GetEmployeeByRequest([FromQuery] BulkRequest param)
		{
			await Task.Yield();
			return param;
		}

		[HttpPost]
		public async Task<int> CreateEmployee([FromBody] Employee employee)
		{
			await Task.Yield();
			return 123;
		}

		[HttpPut("{id}")]
		public async Task UpdateEmployee(int id, [FromBody] Employee employee) => await Task.Yield();

		[HttpDelete("{id}")]
		public async Task DeleteEmployee(int id) => await Task.Yield();

		[HttpDelete("deleteEmployeeFromQuery")]
		public async Task DeleteEmployeeFromQuery([FromQuery] Employee employee) => await Task.Yield();

		[HttpGet("byEnumFromQuery")]
		public Task<MyEnum> GetByEnumFromQuery([FromQuery] MyEnum myEnum) => Task.FromResult(myEnum);

		[HttpGet("byEnum/{myEnum}")]
		public Task<MyEnum> GetByEnumFromPath([FromRoute] MyEnum myEnum) => Task.FromResult(myEnum);

		[HttpGet("ByEnumFromRequest")]
		public Task<MyEnum> GetByEnumFromRequest([FromQuery] EnumRequest myEnumRequest)
			=> Task.FromResult(myEnumRequest.MyEnum);

		[HttpGet("{id}/download"), FileContentResultFilter]
		public async Task<FileContent> Download(int id)
		{
			await Task.Yield();
			return new FileContent { Content = Encoding.Default.GetBytes("123"), FileName = "password.txt", MimeType = "text/plain" };
		}

		[HttpPost("upload")]
		public async Task<long> Upload([FromBody] [ModelBinder(BinderType = typeof(FileContentModelBinder))]
			FileContent downloadable)
		{
			await Task.Yield();
			return downloadable.Content.Length;
		}
	}
}