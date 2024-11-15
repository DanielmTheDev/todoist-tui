module MyHttpClientTests

open System.Net
open System.Net.Http
open System.Threading
open System.Threading.Tasks
open Xunit

// Derived HttpMessageHandler that allows overriding SendAsync
type MockHttpMessageHandler(responseMessage: HttpResponseMessage) =
    inherit HttpMessageHandler()

    override _.SendAsync(_request: HttpRequestMessage, _cancellationToken: CancellationToken) =
        Task.FromResult(responseMessage)

// Helper function to create an HttpClient with mocked response
let createMockedHttpClient(responseMessage: HttpResponseMessage) =
    let handler = MockHttpMessageHandler(responseMessage)
    HttpClient(handler)

// Sample test function
[<Fact>]
let ``Test My HttpClient Call`` () =
    // Arrange
    let expectedContent = "Hello, world!"
    let responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
    responseMessage.Content <- new StringContent(expectedContent)

    let httpClient = createMockedHttpClient(responseMessage)

    // Act
    let resultTask = httpClient.GetStringAsync("https://example.com/test")
    let result = resultTask.Result

    // Assert
    Assert.Equal(expectedContent, result)
