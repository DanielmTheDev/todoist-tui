namespace SyncApi

open FsHttp
open FsUnit.Xunit
open TodoistAdapter.CommunicationRestApi
open TodoistAdapter.Initialization
open TodoistAdapter.LocalState
open TodoistAdapter.Tests.TestData
open Xunit

type LocalStateTests() =

    do initialize ()

    [<Fact>]
    member _.``Merges tasks in local state`` () =
        let t1 = TestTasks.createToday ()
        let t2 = TestTasks.createToday ()

        let beforeMerge = refreshedState ()

        beforeMerge.items |> List.map _.id |> should contain t1.id
        beforeMerge.items |> List.map _.id |> should contain t2.id

        let t3 = TestTasks.createToday ()

        let afterMerge = refreshedState ()

        afterMerge.items |> List.map _.id |> should contain t1.id
        afterMerge.items |> List.map _.id |> should contain t2.id
        afterMerge.items |> List.map _.id |> should contain t3.id

    [<Fact>]
    member _.``Completed tasks are removed on incremental sync`` () =
        let t1 = TestTasks.createToday ()
        let t2 = TestTasks.createToday ()

        let beforeCompletion = refreshedState ()

        beforeCompletion.items |> List.map _.id |> should contain t1.id
        beforeCompletion.items |> List.map _.id |> should contain t2.id

        completeTask t2.id |> Async.RunSynchronously |> Response.assert2xx |> ignore

        let afterCompletion = refreshedState ()

        afterCompletion.items |> List.map _.id |> should contain t1.id
        let completed = afterCompletion.items |> List.find (fun item -> item.id = t2.id)
        completed.``checked`` |> should be True