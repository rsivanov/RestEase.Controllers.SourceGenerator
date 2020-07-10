using RestEase.SampleWebApi.Contracts;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RestEase.SampleWebApi.Infrastructure
{
	public class FileContentRequestBodySerializer : JsonRequestBodySerializer
	{
		public override HttpContent SerializeBody<T>(T body, RequestBodySerializerInfo info)
		{
			if (body is FileContent fileContent)
			{
				var content = new ByteArrayContent(fileContent.Content);
				content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-header") { FileName = $"\"{fileContent.FileName}\"" };
				content.Headers.ContentType = new MediaTypeHeaderValue(fileContent.MimeType);
				return content;			
			}
			return base.SerializeBody(body, info);
		}
	}
}
