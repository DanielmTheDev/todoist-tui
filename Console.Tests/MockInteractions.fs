namespace Console.Tests

open SpectreCoff

[<RequireQualifiedAccess>]
module MockInteractions =

    open System.Collections.Generic
    open Console.UserInteraction
    open TodoistAdapter.RestTypes

    type MockBuilder =
        { askResponses: string list
          askSuggestingResponses: string list
          chooseFromResponses: string list
          chooseGroupedFromResponses: TodoistTask list list }

    let create () =
        { askResponses = []
          askSuggestingResponses = []
          chooseFromResponses = []
          chooseGroupedFromResponses = [] }

    let addAsk (response: string) (builder: MockBuilder) =
        { builder with
            askResponses = builder.askResponses @ [ response ] }

    let addAskSuggesting (response: string) (builder: MockBuilder) =
        { builder with
            askSuggestingResponses = builder.askSuggestingResponses @ [ response ] }

    let addChooseFrom (response: string) (builder: MockBuilder) =
        { builder with
            chooseFromResponses = builder.chooseFromResponses @ [ response ] }

    let addChooseGroupedFrom (response: TodoistTask list) (builder: MockBuilder) =
        { builder with
            chooseGroupedFromResponses = builder.chooseGroupedFromResponses @ [ response ] }

    let addChooseGroupedFromWith (response: TodoistTask list) (builder: MockBuilder) =
        { builder with
            chooseGroupedFromResponses = builder.chooseGroupedFromResponses @ [ response ] }

    let build (builder: MockBuilder) : UserInteraction =

        let askQueue = Queue(builder.askResponses)
        let askSuggestingQueue = Queue(builder.askSuggestingResponses)
        let chooseFromQueue = Queue(builder.chooseFromResponses)
        let chooseGroupedFromQueue = Queue(builder.chooseGroupedFromResponses)

        { ask =
            fun _ ->
                if askQueue.Count > 0 then
                    askQueue.Dequeue()
                else
                    failwith "No more 'ask' responses available."

          askSuggesting =
            fun _ _ ->
                if askSuggestingQueue.Count > 0 then
                    askSuggestingQueue.Dequeue()
                else
                    failwith "No more 'askSuggesting' responses available."

          chooseFrom =
            fun _ _ ->
                if chooseFromQueue.Count > 0 then
                    chooseFromQueue.Dequeue()
                else
                    failwith "No more 'chooseFrom' responses available."

          chooseGroupedFromWith =
            fun _ _ _ ->
                if chooseGroupedFromQueue.Count > 0 then
                    chooseGroupedFromQueue.Dequeue()
                else
                    failwith "No more 'chooseGroupedFromWith' responses available."

          chooseGroupedFrom =
            fun _ _ ->
                if chooseGroupedFromQueue.Count > 0 then
                    chooseGroupedFromQueue.Dequeue()
                else
                    failwith "No more 'chooseGroupedFrom' responses available." }
