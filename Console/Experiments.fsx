#r "bin/Debug/net8.0/Console.dll"
#r "nuget: EluciusFTW.SpectreCoff, 0.49.3"

open System
open System.Net.Http.Json
open Console.Communication
open Console.Types
open SpectreCoff

let scheduleTimes =
    List.init 24 (fun i -> DateTime.Now.Hour + 1 + i * 2)
    |> List.filter (fun i -> i < 24)
    |> List.map (fun i -> $"{i}:00")

scheduleTimes