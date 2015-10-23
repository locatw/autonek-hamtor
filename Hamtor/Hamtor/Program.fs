open Raspberry.IO.GeneralPurpose
open System
open System.Text
open System.Net

let pirSensorPin = ConnectorPin.P1Pin11.ToProcessor()

let timeout = TimeSpan.MaxValue

let monitorSensor (driver : IGpioConnectionDriver) =
    printfn "wait for up ..."
    // Raspberry.IO.GeneralPurpose cannot wait infinitely.
    driver.Wait(pirSensorPin, true, timeout)
    printfn "wake up"

    let url = sprintf "http://%s:%d/Switch" "192.168.0.8" 8083
    let postParameter = sprintf "state=%s" "on"
    let postBytes = Encoding.ASCII.GetBytes(postParameter)
    let request = HttpWebRequest.Create(url) :?> HttpWebRequest
    request.ProtocolVersion <- HttpVersion.Version11
    request.Method <- "POST"
    request.ContentType <- "application/x-www-form-urlencoded"
    request.ContentLength <- int64 postBytes.Length

    use requestStream = request.GetRequestStream()
    requestStream.Write(postBytes, 0, postBytes.Length)

[<EntryPoint>]
let main _ = 
    let driver = GpioConnectionSettings.DefaultDriver

    try
        driver.Allocate(pirSensorPin, PinDirection.Input)

        // monitor sensor loop.
        while true do
            try
                monitorSensor driver
            with
            // todo log error.
            | :? WebException -> ()

            // an interval to prevent sending consecutive request.
            async { do! Async.Sleep(1000) }
            |> Async.RunSynchronously

    finally
        driver.Release(pirSensorPin)
    0
