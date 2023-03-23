using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;

var tokenSource = new CancellationTokenSource();
var token = tokenSource.Token;

var netCaptureTask = new Task(() =>
{
    token.ThrowIfCancellationRequested();
    using (var netEventSession = new TraceEventSession("NetEventSession"))
    {
        netEventSession.EnableProvider("Microsoft-Windows-TCPIP");
        using (var source = new ETWTraceEventSource("NetEventSession", TraceEventSourceType.Session))
        {
            // Register Callback in Parser?
            source.Dynamic.All += (TraceEvent data) =>
            {
                Console.WriteLine($"{data.EventName} : {data.FormattedMessage}");
             
            };
            //Todo: find a better way (is that better?)
            token.Register(()=> netEventSession.Stop());
            source.Process(); // Invoke the event processing with callbacks (forever)
        }
    }
},token);

//Auch nicht so richtig schön?!
netCaptureTask.Start();

//Wait for Keypress
Console.ReadLine();
tokenSource.Cancel();
tokenSource.Dispose();

