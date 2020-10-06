using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Threading.Tasks;

namespace OrderBySpeechWebApp.Proxies
{
    internal class SpeechRecognitionProxy
    {
        private static SpeechRecognizer _speechRecognizer;

        public static void Initialize(string endPointString, string key)
        {
            Uri endPointUri = new Uri(endPointString);
            SpeechConfig speechConfig = SpeechConfig.FromEndpoint(endPointUri, key);

            AudioConfig audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            _speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);
        }

        public static async Task<string> Start()
        {
            if (_speechRecognizer is null)
            {
                throw new InvalidOperationException("SpeechRecognitionProxy was not initialized.");
            }

            string speech = string.Empty;

            var stopRecognition = new TaskCompletionSource<int>();

            _speechRecognizer.Recognized += (s, e) =>
            {
                if (e.Result.Reason == ResultReason.RecognizedSpeech)
                {
                    speech = e.Result.Text;
                    stopRecognition.TrySetResult(0);
                }
                else if (e.Result.Reason == ResultReason.NoMatch)
                {
                    throw new InvalidOperationException("SpeechRecognitionProxy could not recognize speech.");
                }
            };

            _speechRecognizer.Canceled += (s, e) =>
            {
                if (e.Reason == CancellationReason.Error)
                {
                    throw new InvalidOperationException($"SpeechRecognitionProxy encountered error:  ErrorCode={e.ErrorCode} ErrorDetails={e.ErrorDetails}");
                }

                stopRecognition.TrySetResult(0);
            };

            _speechRecognizer.SpeechEndDetected += (s, e) =>
            {
                stopRecognition.TrySetResult(0);
            };

            _speechRecognizer.SessionStopped += (s, e) =>
            {
                stopRecognition.TrySetResult(0);
            };

            // Starts continuous recognition. Uses StopContinuousRecognitionAsync() to stop recognition.
            await _speechRecognizer.StartContinuousRecognitionAsync();

            // Waits for completion. Use Task.WaitAny to keep the task rooted.
            Task.WaitAny(new[] { stopRecognition.Task });

            // Stops recognition.
            await _speechRecognizer.StopContinuousRecognitionAsync();

            return speech;
        }
    }
}
