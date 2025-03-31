namespace WebSharper.NetworkInformation

open WebSharper
open WebSharper.JavaScript

[<JavaScript; AutoOpen>]
module Extensions =

    type Navigator with
        [<Inline "$this.connection">]
        member this.Connection with get(): NetworkInformation = X<NetworkInformation>
