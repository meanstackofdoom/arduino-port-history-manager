using SimHub.Plugins;
using System.Net;
using System.Text;
using System.Threading.Tasks;

[PluginDescription("Arduino Identify Plugin")]
[PluginAuthor("You")]
[PluginName("ArduinoIdentify")]
public class ArduinoIdentifyPlugin : IPlugin
{
    private HttpListener _listener;

    public void Init(PluginManager pluginManager)
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add("http://localhost:16888/");
        _listener.Start();

        Task.Run(() => ListenLoop());
    }

    private async Task ListenLoop()
    {
        while (_listener.IsListening)
        {
            var ctx = await _listener.GetContextAsync();
            var request = ctx.Request;
            var response = ctx.Response;

            if (request.Url.AbsolutePath == "/identify")
            {
                string com = request.QueryString["com"];

                if (!string.IsNullOrEmpty(com))
                {
                    IdentifyArduino(com);
                    WriteResponse(response, $"Blinking {com}");
                }
                else
                {
                    WriteResponse(response, "Missing COM parameter");
                }
            }
        }
    }

    private void IdentifyArduino(string comPort)
    {
        // TODO:
        // Trigger SimHub test effect or LED animation
        // targeting Arduino bound to comPort

        // This is where you'd:
        // - Find Arduino device by COM
        // - Trigger LED blink / gauge sweep
    }

    private void WriteResponse(HttpListenerResponse response, string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.OutputStream.Close();
    }

    public void End(PluginManager pluginManager)
    {
        _listener?.Stop();
    }
}
