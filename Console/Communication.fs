module Console.Communication

open System
open System.Net.Http
open System.Net.Http.Headers

let httpClient =
    let client = new HttpClient()
    client.BaseAddress <- Uri("https://api.todoist.com/rest/v2/")
    client.DefaultRequestHeaders.Authorization <- AuthenticationHeaderValue("Bearer", "your-api-key")
    client