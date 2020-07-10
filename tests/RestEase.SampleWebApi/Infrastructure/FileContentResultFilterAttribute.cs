using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using RestEase.SampleWebApi.Contracts;
using System;
using System.Threading.Tasks;

namespace RestEase.SampleWebApi.Infrastructure
{
	public class FileContentResultFilterAttribute : Attribute, IAsyncResultFilter
	{
		public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
		{
			var controller = (ControllerBase) context.Controller;

			if ((context.Result as ObjectResult)?.Value is FileContent fileContent)
			{
				controller.Response.Headers.Add("Access-Control-Expose-Headers", new StringValues("Content-Disposition"));
				context.Result = controller.File(fileContent.Content, fileContent.MimeType, fileContent.FileName);
			}
			else
			{
				context.Result = controller.NotFound();
			}

			await next();
		}
	}
}
