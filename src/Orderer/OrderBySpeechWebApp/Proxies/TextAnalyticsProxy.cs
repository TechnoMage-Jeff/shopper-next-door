using Azure;
using Azure.AI.TextAnalytics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderBySpeechWebApp.Models;

namespace OrderBySpeechWebApp.Proxies
{
    class TextAnalyticsProxy
    {
        private static TextAnalyticsClient _textAnalyticsClient;
        public static void Initialize(string endpointString, string key)
        {
            Uri endpointUri = new Uri(endpointString);
            AzureKeyCredential credential = new AzureKeyCredential(key);

            _textAnalyticsClient = new TextAnalyticsClient(endpointUri, credential);
        }

        public static async Task<OrderResponse> RecognizeEntities(string speechText)
        {
            Response<CategorizedEntityCollection> response = await _textAnalyticsClient.RecognizeEntitiesAsync(speechText);

            OrderResponse order = new OrderResponse();
           IEnumerator<CategorizedEntity> entities = response.Value.GetEnumerator();

            while (entities.MoveNext())
            {
                CategorizedEntity entity = entities.Current;
                if (entity.Category == EntityCategory.Person)
                {
                    order.Person.Add(entity.Text);
                }
                else if (entity.Category == EntityCategory.Location)
                {
                    order.Location.Add(entity.Text);
                }
                else if (entity.Category == EntityCategory.Product)
                {
                    order.Product.Add(entity.Text);
                }

            }

            return order;
        }

        // TODO: Generated links are not useful - is ther a way to improve linking
        public static async Task<List<OrderLink>> LinkEntities(string speechText)
        {
            Response<LinkedEntityCollection> response = await _textAnalyticsClient.RecognizeLinkedEntitiesAsync(speechText);

            //OrderResponse order = new OrderResponse();
            List<OrderLink> orderLinks = new List<OrderLink>();
            IEnumerator<LinkedEntity> entities = response.Value.GetEnumerator();

            while (entities.MoveNext())
            {
                LinkedEntity entity = entities.Current;

                OrderLink orderLink = new OrderLink() { Name = entity.Name, Url = entity.Url.ToString() };
                orderLinks.Add(orderLink);
            }

            return orderLinks;
        }

        public static async Task<List<string>> GetKeyPhrases(string speechText)
        {
            Response<KeyPhraseCollection> response = await _textAnalyticsClient.ExtractKeyPhrasesAsync(speechText);

            List<string> phrases = new List<string>();
            IEnumerator<string> phrasesResponse = response.Value.GetEnumerator();

            while (phrasesResponse.MoveNext())
            {
                string phrase = phrasesResponse.Current;
                phrases.Add(phrase);
            }

            return phrases;
        }
    }
}
