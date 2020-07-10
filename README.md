# RestEase.Controllers.SourceGenerator
![Build](https://github.com/rsivanov/RestEase.Controllers.SourceGenerator/workflows/Build%20&%20test%20&%20publish%20Nuget/badge.svg?branch=master)
[![NuGet](https://img.shields.io/nuget/dt/RestEase.Controllers.SourceGenerator)](https://www.nuget.org/packages/RestEase.Controllers.SourceGenerator) 
[![NuGet](https://img.shields.io/nuget/v/RestEase.Controllers.SourceGenerator)](https://www.nuget.org/packages/RestEase.Controllers.SourceGenerator)

Provides a source generator of [RestEase](https://github.com/canton7/RestEase) interfaces with all required attributes for calling web application controllers. Useful for writing strongly-typed asp.net core integration tests.

Why?
===
[ASP.NET Core provides very convenient infrastructure](https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.1#aspnet-core-integration-tests) for writing and executing integration tests for web application controllers.
Imagine that we have a following controller:
```csharp
[Route("[controller]")]
public class EmployeesController : Controller
{
    [HttpGet("bulk")]
    public Task<BulkRequest> GetEmployeeByRequest([FromQuery] BulkRequest bulkRequest) =>
        Task.FromResult(bulkRequest);
}

public class BulkRequest
{
    public string Value { get; set; }
    public int[] Ids { get; set; }
}
```
We can write a strongly-typed integration test for this controller using [RestEase](https://github.com/canton7/RestEase) with [RestEase.Serialization.Extensions](https://github.com/rsivanov/RestEase.Serialization.Extensions):
```csharp
[BasePath("Employees")]
[SerializationMethods(Query = QuerySerializationMethod.Serialized)]
public interface IEmployeesController
{
    [Get("bulk")]
    Task<BulkRequest> GetEmployeeByRequest([Query] BulkRequest bulkRequest);
}
```
```csharp
[Fact]
public async Task GetEmployeeByRequest_Success()
{
    var requester = new Requester(_httpClient)
    {
        RequestQueryParamSerializer = new ComplexTypeRequestQueryParamSerializer(),
    };
    var controller = RestClient.For<IEmployeesController>(requester);
    var request = new BulkRequest { Ids = new[] { 1, 2, 3 }, Value = "Qwerty" };
    var response = await controller.GetEmployeeByRequest(request);
    Assert.Equal(request.Value, response.Value);
    Assert.True(request.Ids.SequenceEqual(response.Ids));
}
```
The problem is that you can make mistakes with attributes or parameter names writing a RestEase interface IEmployeesController by hand. It takes time and requires an additional effort to map ASP.Net Core MVC attributes correctly to their RestEase counterparts. That's where RestEase.Controllers.SourceGenerator comes to the rescue.

How to use
===
Just add [RestEase.Controllers.SourceGenerator](https://www.nuget.org/packages/RestEase.Controllers.SourceGenerator) source analyzer from Nuget to your web application project. It generates RestEase interfaces automatically for all controllers and keeps it up to date with the controller definitions.

Preview limitations
==
[Source generators](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/) are still in preview, so the tooling support isn't ideal yet. Use the latest version of Visual Studio or add a package reference to the latest preview version of [Microsoft.Net.Compilers.Toolset](https://www.nuget.org/packages/Microsoft.Net.Compilers.Toolset/3.7.0-3.final) to your web application project (that works even in [JetBrains Rider](https://www.jetbrains.com/rider/)).