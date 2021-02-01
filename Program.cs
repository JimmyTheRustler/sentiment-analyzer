using Azure;
using System;
using System.Globalization;
using Azure.AI.TextAnalytics;
using Reddit;
using Reddit.Controllers;
using Reddit.Controllers.EventArgs;
using Reddit.Inputs;
using Reddit.Inputs.LinksAndComments;
using Reddit.Inputs.Subreddits;
using Reddit.Inputs.Users;
using Reddit.Things;
using System.Collections.Generic;



namespace sentiment_analyzer
{
    class Program
    {
        private static readonly AzureKeyCredential credentials = new AzureKeyCredential("");
        private static readonly Uri endpoint = new Uri(""); 
        private static string redditAppID = "";
        private static string redditRefreshToken = "";
        private static string redditSecret = "";
        static void Main(string[] args)
        {
            var client = new TextAnalyticsClient(endpoint, credentials);
            var reddit = new RedditClient(appId: redditAppID, appSecret: redditSecret, refreshToken: redditRefreshToken);
            //Console.WriteLine("Username: " + reddit.Account.Me.Name);
            //Console.WriteLine("Cake Day: " + reddit.Account.Me.Created.ToString("D"));
            var subredditObj = reddit.Models.Listings.Top(new TimedCatSrListingInput(), "AskReddit");
            //Console.WriteLine(subredditObj.Data.Children[0].Data.URL);
           // Console.WriteLine(subredditObj.Data.Children.Count);

            List<string> posts = new List<string>();
            
            var max = 25;
            if(subredditObj.Data.Children.Count < max)
                max = subredditObj.Data.Children.Count;
            for(int i = 0; i < max; i++)
            {
                posts.Add(subredditObj.Data.Children[i].Data.URL);
            }
            Console.WriteLine(posts[6]);
            var tmp = subredditObj.Data.Children[1].Data.Id;
            Console.WriteLine(reddit.Models.);

            //SentimentAnalysisExample(client);
            
            //LanguageDetectionExample(client);
            //EntityRecognitionExample(client);
            //EntityLinkingExample(client);
            //KeyPhraseExtractionExample(client);



            //Console.Write("Press any key to exit.");
            //Console.ReadKey();
        }

        static void SentimentAnalysisExample(TextAnalyticsClient client)
        {
            string inputText = "The FitnessGram PACER Test is a multistage aerobic capacity test that progressively gets more difficult as it continues.

The test is used to measure a student's aerobic capacity as part of the FitnessGram assessment. Students run back and forth as many times as they can, each lap signaled by a beep sound. The test get progressively faster as it continues until the student reaches their max lap score.";
            DocumentSentiment documentSentiment = client.AnalyzeSentiment(inputText);
            Console.WriteLine($"Document sentiment: {documentSentiment.Sentiment}\n");

            foreach (var sentence in documentSentiment.Sentences)
            {
                Console.WriteLine($"\tText: \"{sentence.Text}\"");
                Console.WriteLine($"\tSentence sentiment: {sentence.Sentiment}");
                Console.WriteLine($"\tPositive score: {sentence.ConfidenceScores.Positive:0.00}");
                Console.WriteLine($"\tNegative score: {sentence.ConfidenceScores.Negative:0.00}");
                Console.WriteLine($"\tNeutral score: {sentence.ConfidenceScores.Neutral:0.00}\n");
            }
        }

        static void LanguageDetectionExample(TextAnalyticsClient client)
        {
            DetectedLanguage detectedLanguage = client.DetectLanguage("Ce document est rédigé en Français.");
            Console.WriteLine("Language:");
            Console.WriteLine($"\t{detectedLanguage.Name},\tISO-6391: {detectedLanguage.Iso6391Name}\n");
        }

        static void EntityRecognitionExample(TextAnalyticsClient client)
        {
            var response = client.RecognizeEntities("I had a wonderful trip to Seattle last week.");
            Console.WriteLine("Named Entities:");
            foreach (var entity in response.Value)
            {
                Console.WriteLine($"\tText: {entity.Text},\tCategory: {entity.Category},\tSub-Category: {entity.SubCategory}");
                Console.WriteLine($"\t\tScore: {entity.ConfidenceScore:F2}\n");
            }    
        }
        static void EntityLinkingExample(TextAnalyticsClient client)
        {
            var response = client.RecognizeLinkedEntities(
                "Microsoft was founded by Bill Gates and Paul Allen on April 4, 1975, " +
                "to develop and sell BASIC interpreters for the Altair 8800. " +
                "During his career at Microsoft, Gates held the positions of chairman, " +
                "chief executive officer, president and chief software architect, " +
                "while also being the largest individual shareholder until May 2014.");
            Console.WriteLine("Linked Entities:");
            foreach (var entity in response.Value)
            {
                Console.WriteLine($"\tName: {entity.Name},\tID: {entity.DataSourceEntityId},\tURL: {entity.Url}\tData Source: {entity.DataSource}");
                Console.WriteLine("\tMatches:");
                foreach (var match in entity.Matches)
                {
                    Console.WriteLine($"\t\tText: {match.Text}");
                    Console.WriteLine($"\t\tScore: {match.ConfidenceScore:F2}\n");
                }
            }
        }
        static void KeyPhraseExtractionExample(TextAnalyticsClient client)
        {
            var response = client.ExtractKeyPhrases("My cat might need to see a veterinarian.");

            // Printing key phrases
            Console.WriteLine("Key phrases:");

            foreach (string keyphrase in response.Value)
            {
                Console.WriteLine($"\t{keyphrase}");
            }
        }


    }
}
