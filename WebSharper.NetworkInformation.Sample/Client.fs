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
    // The template is loaded from the DOM, so editing index.html and refreshing
    // the browser is enough; a rebuild is only needed when holes change.
    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    let status = Var.Create "Checking network status…"
    let effectiveType = Var.Create "—"
    let downlink = Var.Create "—"
    let rtt = Var.Create "—"
    let saveData = Var.Create "—"

    let connection = JS.Window.Navigator.Connection

    let updateNetworkInfo () =
        if isNull connection then
            status := "The Network Information API is not available in this browser."
        else
            try
                effectiveType := connection.EffectiveType
                downlink := connection.Downlink.ToString("0.00")
                rtt := connection.Rtt.ToString("0.0")
                saveData := if connection.SaveData then "Enabled" else "Disabled"
                status := "Network status updated."
            with error ->
                status := $"Network Information error: {error.Message}"

    let networkListening () =
        if not (isNull connection) then
            connection.OnChange <- fun _ -> updateNetworkInfo ()

    [<SPAEntryPoint>]
    let Main () =
        updateNetworkInfo ()
        networkListening ()

        IndexTemplate()
            .status(status.V)
            .effectiveType(effectiveType.V)
            .downlink(downlink.V)
            .rtt(rtt.V)
            .saveData(saveData.V)
            .Bind()
