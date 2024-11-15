module Console.Communication

open System
open System.Net.Http
open System.Net.Http.Headers
open System.Web

let httpClient =
    let client = new HttpClient()
    client.BaseAddress <- Uri("https://api.todoist.com/rest/v2/")
    client.DefaultRequestHeaders.Authorization <- AuthenticationHeaderValue("Bearer", "your-api-key")
    client

let buildUriWithQuery (baseUrl: string) queryParams (client: HttpClient) =
    let fullUrl = Uri(client.BaseAddress, baseUrl)
    let uriBuilder = UriBuilder(fullUrl)
    let query = HttpUtility.ParseQueryString(uriBuilder.Query)
    for key, value in queryParams do
        query[key] <- value
    uriBuilder.Query <- query.ToString()
    uriBuilder.Uri.ToString()