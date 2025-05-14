module Console.Async

let map f asyncValue = async {
    let! x = asyncValue
    return f x
}