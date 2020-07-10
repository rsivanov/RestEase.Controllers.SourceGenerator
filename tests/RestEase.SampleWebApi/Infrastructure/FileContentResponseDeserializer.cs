using RestEase.SampleWebApi.Contracts;
using System;
using System.Net.Http;

namespace RestEase.SampleWebApi.Infrastructure
{
	public class FileContentResponseDeserializer : JsonResponseDeserializer
	{
		public override T Deserialize<T>(string content, HttpResponseMessage response, ResponseDeserializerInfo info)
		{
			if (typeof(T) == typeof(FileContent))
			{
				var instance = Activator.CreateInstance<T>();
				var fileContent = instance as FileContent;
				fileContent.FileName = response.Content.Headers.ContentDisposition.FileName;
				fileContent.MimeType = response.Content.Headers.ContentType.MediaType;
				fileContent.Content = response.Content.ReadAsByteArrayAsync().Result;

				return instance;
			}
			return base.Deserialize<T>(content, response, info);
		}
	}
}
