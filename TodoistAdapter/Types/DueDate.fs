module TodoistAdapter.Types.DueDate

open System
open System.Text.Json
open System.Text.Json.Serialization

type DueDate =
    | TodoistDateTime of DateTime
    | TodoistDateOnly of DateOnly

let isAtDate date dueDate =
    match dueDate with
    | TodoistDateOnly dateOnly -> (dateOnly.CompareTo date) = 0
    | TodoistDateTime dateTime -> (DateOnly.FromDateTime(dateTime).CompareTo date) = 0

type OptionDueDateConverter() =
    inherit JsonConverter<DueDate option>()

    override _.Read(reader, _, _) =
        if reader.TokenType = JsonTokenType.Null then
            None
        else
            let raw = reader.GetString()
            if String.IsNullOrWhiteSpace(raw) then None
            elif raw.Contains("T") then
                Some (DueDate.TodoistDateTime(DateTime.Parse raw))
            else
                Some (DueDate.TodoistDateOnly(DateOnly.Parse raw))

    override _.Write(writer, value, _) =
        match value with
        | None ->
            // You could write null, or skip, depending on your desired JSON
            writer.WriteNullValue()
        | Some (DueDate.TodoistDateTime dt) ->
            writer.WriteStringValue(dt.ToString("yyyy-MM-ddTHH:mm:ss"))
        | Some (DueDate.TodoistDateOnly d) ->
            writer.WriteStringValue(d.ToString("yyyy-MM-dd"))

let updateDay (newDate: DateOnly) (oldDueDate: DueDate) =
    match oldDueDate with
    | TodoistDateOnly _ -> DueDate.TodoistDateOnly newDate
    | TodoistDateTime oldTime ->
        let updatedDateTime = DateTime(newDate.Year, newDate.Month, newDate.Day, oldTime.Hour, oldTime.Minute, oldTime.Second)
        TodoistDateTime updatedDateTime

let updateTime newTime oldDueDate =
    match oldDueDate with
    | TodoistDateOnly dateOnly -> TodoistDateTime (dateOnly.ToDateTime newTime)
    | TodoistDateTime dateTime -> TodoistDateTime (DateTime ((DateOnly.FromDateTime dateTime), newTime))

let dateOnlyOf date =
    match date with
    | TodoistDateOnly date -> date
    | TodoistDateTime dateTime -> DateOnly.FromDateTime dateTime