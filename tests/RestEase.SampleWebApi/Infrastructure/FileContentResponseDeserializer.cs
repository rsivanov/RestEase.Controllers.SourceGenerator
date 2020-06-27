using RestEase.SampleWebApi.Contracts;
using System;
using System.Net.Http;

namespace RestEase.SampleWebApi.Infrastructure
{
	public class FileContentResponseDeserializer : JsonResponseDeserializer
	{
		public override T Deserialize<T>(string content, HttpResponseMessage response, ResponseDeserializerInfo info)
		{
			if (typeof(T) != typeof(FileContent))
			{
				return base.Deserialize<T>(content, response, info);
			}

			var instance = Activator.CreateInstance<T>(); // or JsonConvert SerializeObject + DeserializeObject

			var downloadable = instance as FileContent;
			downloadable.FileName = response.Content.Headers.ContentDisposition.FileName;
			downloadable.MimeType = response.Content.Headers.ContentType.MediaType;
			downloadable.Content = response.Content.ReadAsByteArrayAsync().Result;

			return instance;
		}
	}
}
