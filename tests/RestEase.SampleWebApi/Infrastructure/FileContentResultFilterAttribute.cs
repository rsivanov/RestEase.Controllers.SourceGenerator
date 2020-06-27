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
			var controller = (ControllerBase)context.Controller;

			if (!((context.Result as ObjectResult)?.Value is FileContent downloadable))
			{
				context.Result = controller.NotFound();
			}
			else
			{
				controller.Response.Headers.Add("Access-Control-Expose-Headers", new StringValues("Content-Disposition"));
				context.Result = controller.File(downloadable.Content, downloadable.MimeType, downloadable.FileName);
			}

			await next();
		}
	}
}
