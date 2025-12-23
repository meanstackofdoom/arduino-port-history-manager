using SimHub.Plugins;
using System.Threading.Tasks;

namespace ArduinoIdentifyPlugin
{
    [PluginName("Arduino Identify Plugin")]
    [PluginDescription("Identify Arduino devices by numeric target")]
    public class ArduinoIdentifyPlugin : IPlugin
    {
            public PluginManager PluginManager { get; set; }

            // Parsed state from the last trigger, for use in Arduino scripts:
            // - IdentifyTargetId : which board should react
            // - IdentifyChannel  : which logical channel/zone on the board
            // - IdentifyMode     : 0 = normal identify, 1 = test pattern, etc.

        public void Init(PluginManager pluginManager)
        {
            PluginManager = pluginManager;

            PluginManager.AddProperty(
                "IdentifyPulse",
                typeof(ArduinoIdentifyPlugin),
                0
            );

            PluginManager.AddProperty(
                "IdentifyTargetId",
                typeof(ArduinoIdentifyPlugin),
                0
            );

            PluginManager.AddProperty(
                "IdentifyChannel",
                typeof(ArduinoIdentifyPlugin),
                0
            );

            PluginManager.AddProperty(
                "IdentifyMode",
                typeof(ArduinoIdentifyPlugin),
                0
            );

            PluginManager.AddAction<ArduinoIdentifyPlugin>(
                "Trigger Identify Blink",
                OnIdentifyPressed
            );

            // Optional: a separate "test" action that always uses mode 1.
            PluginManager.AddAction<ArduinoIdentifyPlugin>(
                "Trigger Test Pattern",
                OnTestPressed
            );
        }

        public void End(PluginManager pluginManager) { }

        // Main identify action: expects value in the form "id:channel:mode".
        // All parts are optional:
        //   "5"           -> id=5,  channel=0, mode=0
        //   "5:2"         -> id=5,  channel=2, mode=0
        //   "5:2:1"       -> id=5,  channel=2, mode=1
        //   "" / invalid  -> id=1,  channel=0, mode=0 (safe defaults)
        private async void OnIdentifyPressed(PluginManager manager, string value)
        {
            ParseValue(value, out int targetId, out int channel, out int mode);
            await RunPattern(manager, targetId, channel, mode);
        }

        // Convenience action: same parsing as above but forces mode = 1 (test).
        private async void OnTestPressed(PluginManager manager, string value)
        {
            ParseValue(value, out int targetId, out int channel, out int mode);
            mode = 1; // force test mode
            await RunPattern(manager, targetId, channel, mode);
        }

        private void ParseValue(string value, out int targetId, out int channel, out int mode)
        {
            // Safe defaults
            targetId = 1;
            channel  = 0;
            mode     = 0;

            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var parts = value.Split(':');

            if (parts.Length > 0 && int.TryParse(parts[0], out var id))
            {
                targetId = id;
            }

            if (parts.Length > 1 && int.TryParse(parts[1], out var ch))
            {
                channel = ch;
            }

            if (parts.Length > 2 && int.TryParse(parts[2], out var m))
            {
                mode = m;
            }
        }

        private async Task RunPattern(PluginManager manager, int targetId, int channel, int mode)
        {
            // Expose parsed values to SimHub / Arduino
            manager.SetPropertyValue(
                "IdentifyTargetId",
                typeof(ArduinoIdentifyPlugin),
                targetId
            );

            manager.SetPropertyValue(
                "IdentifyChannel",
                typeof(ArduinoIdentifyPlugin),
                channel
            );

            manager.SetPropertyValue(
                "IdentifyMode",
                typeof(ArduinoIdentifyPlugin),
                mode
            );

            // Different blink behaviour depending on mode:
            // mode 0 = normal identify (short, distinct)
            // mode 1 = test pattern (longer, more obvious)
            int pulses = mode == 1 ? 10 : 5;
            int onMs   = mode == 1 ? 200 : 300;
            int offMs  = mode == 1 ? 200 : 300;

            for (int i = 0; i < pulses; i++)
            {
                manager.SetPropertyValue(
                    "IdentifyPulse",
                    typeof(ArduinoIdentifyPlugin),
                    1
                );

                await Task.Delay(onMs);

                manager.SetPropertyValue(
                    "IdentifyPulse",
                    typeof(ArduinoIdentifyPlugin),
                    0
                );

                await Task.Delay(offMs);
            }

            // Optional: reset after blink so idle state is clean
            manager.SetPropertyValue(
                "IdentifyTargetId",
                typeof(ArduinoIdentifyPlugin),
                0
            );

            manager.SetPropertyValue(
                "IdentifyChannel",
                typeof(ArduinoIdentifyPlugin),
                0
            );

            manager.SetPropertyValue(
                "IdentifyMode",
                typeof(ArduinoIdentifyPlugin),
                0
            );
        }
    }
}
