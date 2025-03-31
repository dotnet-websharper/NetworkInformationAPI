namespace WebSharper.NetworkInformation.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Notation
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.NetworkInformation

[<JavaScript>]
module Client =
    // The templates are loaded from the DOM, so you just can edit index.html
    // and refresh your browser, no need to recompile unless you add or remove holes.
    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    let statusMessage = Var.Create "Checking network status..."
    let connection = JS.Window.Navigator.Connection

    let updateNetworkInfo() =
        try            
            let resultText = 
                sprintf "<b>Network Type:</b> %s <br> \
                     <b>Downlink Speed:</b> %.2f Mbps <br> \
                     <b>RTT:</b> %.2f ms <br> \
                     <b>Save Data Mode:</b> %s"
                    (connection.EffectiveType)
                    (connection.Downlink)
                    (connection.Rtt)
                    (if connection.SaveData then "Enabled" else "Disabled")

            JS.Document.GetElementById("status").InnerHTML <- resultText

        with error ->
            statusMessage := $"Network Information error: {error.Message}"

    let networkListening() =
        //connection.AddEventListener("change", fun (evt: Dom.Event) -> updateNetworkInfo())
        connection.OnChange <- fun _ -> updateNetworkInfo()

    [<SPAEntryPoint>]
    let Main () =

        updateNetworkInfo()
        networkListening()

        IndexTemplate.Main()
            .Status(statusMessage.V)
            .Doc()
        |> Doc.RunById "main"
