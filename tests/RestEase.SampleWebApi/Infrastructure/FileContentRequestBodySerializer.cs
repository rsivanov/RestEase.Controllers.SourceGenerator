using RestEase.SampleWebApi.Contracts;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RestEase.SampleWebApi.Infrastructure
{
	public class FileContentRequestBodySerializer : JsonRequestBodySerializer
	{
		public override HttpContent SerializeBody<T>(T body, RequestBodySerializerInfo info)
		{
			if (typeof(T) != typeof(FileContent))
			{
				return base.SerializeBody(body, info);
			}

			var downloadable = body as FileContent;

			var content = new ByteArrayContent(downloadable.Content);
			content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-header") { FileName = $"\"{downloadable.FileName}\"" };
			content.Headers.ContentType = new MediaTypeHeaderValue(downloadable.MimeType);
			return content;
		}
	}
}
