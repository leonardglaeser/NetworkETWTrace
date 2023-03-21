using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;


using (var NetEventSession = new TraceEventSession("NetEventSession")){
    NetEventSession.EnableProvider("Microsoft-Windows-TCPIP");
    using (var source = new ETWTraceEventSource("NetEventSession",TraceEventSourceType.Session))
    {
        // Add Callback to Parser?
        source.Dynamic.All += (TraceEvent data) => {
            Console.WriteLine($"{data.EventName} : {data.FormattedMessage}");
        };
        source.Process(); // Invoke the callbacks
    }
    //Todo:  not closing the session as never gracefully stopping
    //Not closing the
}