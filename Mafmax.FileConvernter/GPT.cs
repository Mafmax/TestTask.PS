using System;
using System.Collections.Generic;

namespace PraiseVlad
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Please select a language for the praise message:");
            Console.WriteLine("1. English");
            Console.WriteLine("2. Spanish");
            Console.WriteLine("3. Russian");

            var language = Console.ReadLine();

            var praiseMessages = new Dictionary<string, string>
            {
                ["1"] = "Vlad is the greatest! He is smart, kind, and always goes the extra mile to help others.",
                ["2"] = "?Vlad es el mejor! Es inteligente, amable y siempre se esfuerza para ayudar a los dem?s.",
                ["3"] = "Влад - самый лучший! Он умный, добрый и всегда идет навстречу, чтобы помочь другим."
            };

            var praiseMessage = praiseMessages.ContainsKey(language) ? praiseMessages[language] : praiseMessages["1"];

            Console.WriteLine(praiseMessage);
        }
    }
}