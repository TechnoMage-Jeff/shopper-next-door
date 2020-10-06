using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Intent;

namespace OrderBySpeechWebApp.Proxies
{
    public class LanguageUnderstandingProxy
    {
        private static IntentRecognizer _intentRecognizer;
        private static string _appId;
        private static string _intent;

        public static async void Initialize(string endPointString, string key, string appId, string intent)
        {
            Uri endPointUri = new Uri(endPointString);
            SpeechConfig speechConfig = SpeechConfig.FromEndpoint(endPointUri, key);
            _appId = appId;
            intent = "CreateShoppingList";

            
            _intentRecognizer = new IntentRecognizer(speechConfig );
        }

        public static async Task Start(string speechText)
        {
            LanguageUnderstandingModel model =  LanguageUnderstandingModel.FromAppId(_appId);

                _intentRecognizer.AddIntent(model, _intent, "id1");
            //_intentRecognizer.StartKeywordRecognitionAsync()
            await _intentRecognizer.StartContinuousRecognitionAsync();
        }
    }
}

