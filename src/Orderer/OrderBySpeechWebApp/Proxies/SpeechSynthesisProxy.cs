using Microsoft.CognitiveServices.Speech;
using System;
using System.Threading.Tasks;

namespace OrderBySpeechWebApp.Proxies
{
    public class SpeechSynthesisProxy
    {
        private static SpeechSynthesizer _speechSynthesizer;

        public static void Initialize(string endPointString, string key)
        {
            Uri endPointUri = new Uri(endPointString);
            SpeechConfig speechConfig = SpeechConfig.FromEndpoint(endPointUri, key);

            _speechSynthesizer = new SpeechSynthesizer(speechConfig);
        }

        public static async Task Start(string speechText)
        {
            await _speechSynthesizer.SpeakTextAsync(speechText);
        }
    }
}
