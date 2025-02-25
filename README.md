# WebSharper Network Information API Binding

This repository provides an F# [WebSharper](https://websharper.com/) binding for the [Network Information API](https://developer.mozilla.org/en-US/docs/Web/API/Network_Information_API), enabling WebSharper applications to access network status and connection details.

## Repository Structure

The repository consists of two main projects:

1. **Binding Project**:

   - Contains the F# WebSharper binding for the Network Information API.

2. **Sample Project**:
   - Demonstrates how to use the Network Information API with WebSharper syntax.
   - Includes a GitHub Pages demo: [View Demo](https://dotnet-websharper.github.io/NetworkInformationAPI/).

## Installation

To use this package in your WebSharper project, add the NuGet package:

```bash
   dotnet add package WebSharper.NetworkInformation
```

## Building

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.

### Steps

1. Clone the repository:

   ```bash
   git clone https://github.com/dotnet-websharper/NetworkInformation.git
   cd NetworkInformation
   ```

2. Build the Binding Project:

   ```bash
   dotnet build WebSharper.NetworkInformation/WebSharper.NetworkInformation.fsproj
   ```

3. Build and Run the Sample Project:

   ```bash
   cd WebSharper.NetworkInformation.Sample
   dotnet build
   dotnet run
   ```

4. Open the hosted demo to see the Sample project in action:
   [https://dotnet-websharper.github.io/NetworkInformationAPI/](https://dotnet-websharper.github.io/NetworkInformationAPI/)

## Example Usage

Below is an example of how to use the Network Information API in a WebSharper project:

```fsharp
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

    // Variable to store the current network status message
    let statusMessage = Var.Create "Checking network status..."

    // Get the Network Information API object
    let connection = As<Navigator>(JS.Window.Navigator).Connection

    // Function to update network status information
    let updateNetworkInfo() =
        try
            let resultText =
                sprintf "<b>Network Type:</b> %s <br> \
                     <b>Downlink Speed:</b> %.2f Mbps <br> \
                     <b>RTT:</b> %.2f ms <br> \
                     <b>Save Data Mode:</b> %s"
                    (connection.EffectiveType)  // Type of connection (e.g., 4g, 3g, etc.)
                    (connection.Downlink)       // Estimated downlink speed in Mbps
                    (connection.Rtt)            // Estimated round-trip time in ms
                    (if connection.SaveData then "Enabled" else "Disabled") // Save data mode status

            // Update the UI with network status
            JS.Document.GetElementById("status").InnerHTML <- resultText

        with error ->
            // Handle errors and display the failure message
            statusMessage := $"Network Information error: {error.Message}"

    // Function to listen for changes in network status
    let networkListening() =
        connection.OnChange <- fun _ -> updateNetworkInfo()

    [<SPAEntryPoint>]
    let Main () =

        // Initialize network status and set up event listener
        updateNetworkInfo()
        networkListening()

        IndexTemplate.Main()
            .Status(statusMessage.V) // Bind network status message to the UI
            .Doc()
        |> Doc.RunById "main"
```

This example demonstrates how to retrieve and monitor network connection details using the Network Information API in WebSharper.

## Important Considerations

- **Limited Browser Support**: Some browsers do not fully support the Network Information API; check [MDN Network Information API](https://developer.mozilla.org/en-US/docs/Web/API/Network_Information_API) for the latest compatibility details.
- **Privacy Restrictions**: Some network details may be restricted or unavailable due to privacy concerns.
- **Dynamic Updates**: The API provides real-time updates when the network status changes.
