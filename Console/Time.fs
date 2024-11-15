module Console.Time

open System

let scheduleTimes =
    List.init 24 (fun i -> DateTime.Now.Hour + 1 + i * 2)
    |> List.filter (fun i -> i < 24)
    |> List.map (fun i -> $"{i}:00")