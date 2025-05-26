using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Threading;

class Chatbot2
{
    // Memory storage for user interests or past topics
    static Dictionary<string, string> userMemory = new Dictionary<string, string>();

    // Stores the last discussed topic for follow-up support.
    static string lastTopic = "";
    static string userName = "friend"; // Default name

    // Predefined keyword-based cybersecurity responses
    static Dictionary<string, string[]> keywordResponses = new Dictionary<string, string[]>
    {
        { "password", new string[] {
            "Use strong, unique passwords for each account.",
            "Avoid common passwords like '123456' or 'password'.",
            "Consider using a password manager to generate secure passwords."
        } },
        { "scam", new string[] {
            "Beware of unsolicited emails requesting sensitive information.",
            "Use two-factor authentication to protect your accounts.",
            "Never share login credentials via email!"
        } },
        { "privacy", new string[] {
            "Review privacy settings on social media regularly.",
            "Use encryption tools to protect your data online.",
            "Disable location tracking on apps you don’t need."
        } }
    };
    // Predefined responses based on user sentiment
    static Dictionary<string, string> sentimentResponses = new Dictionary<string, string>
    {
        { "worried", "It's understandable to feel that way. Cyber threats can be scary, but there are ways to stay safe." },
        { "curious", "Curiosity is great. Learning about cybersecurity will help you protect yourself online." },
        { "frustrated", "I get it—cybersecurity can be confusing. Let me help by breaking things down." }
    };
    // Entry point of app
    static void Main()
    {
        PlayGreeting();             // Play audio greeting or show fallback text
        DisplayWelcomeMessage();    // Show ASCII welcome art and instructions
        RunChatbot();               // Start interactive chatbot loop
    }
    // Play audio greeting or display fallback message if sound fails
    static void PlayGreeting()
    {
        try
        {
            SoundPlayer player = new SoundPlayer("C:\\Users\\viank\\source\\repos\\Cybersecurity\\Cybersecurity\\Resources\\welcome_message.wav");
            player.Play();
            Thread.Sleep(2000); // Pause briefly after playing the sound
        }
        catch
        {
            Console.WriteLine("Welcome to the Cybersecurity Awareness Bot.");
        }
    }
    // Displays welcome text, ASCII art, and instructions
    static void DisplayWelcomeMessage()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n=========================================");
        Console.WriteLine("Welcome to the Cybersecurity Awareness Bot");
        Console.WriteLine("=========================================\n");

        Console.WriteLine(@"
                  ____      _                                        _ _             
 / ___|   _| |__   ___ _ __ ___  ___  ___ _   _ _ __(_) |_ _   _     
| |  | | | | '_ \ / _ \ '__/ __|/ _ \/ __| | | | '__| | __| | | |   
| |__| |_| | |_) |  __/ |  \__ \  __/ (__| |_| | |  | | |_| |_| |   
 \____\__, |_.__/ \___|_|  |___/\___|\___|\__,_|_|  |_|\__|\__, |   
   / \|___/   ____ _ _ __ ___ _ __   ___  ___ ___  | __ )  |___/ |_ 
  / _ \ \ /\ / / _` | '__/ _ \ '_ \ / _ \/ __/ __| |  _ \ / _ \| __|
 / ___ \ V  V / (_| | | |  __/ | | |  __/\__ \__ \ | |_) | (_) | |_ 
/_/   \_\_/\_/ \__,_|_|  \___|_| |_|\___||___/___/ |____/ \___/ \__|       
        ");
        // Troelson, A. and Japikse, P., 2022. Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming. 11th ed. Chambersburg: Apress. 
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nI’m here to help you stay safe online.");
        Console.WriteLine("Ask me about password security, scams and privacy settings.");
        Console.ResetColor();
    }
    // Main chatbot loop for user interaction.
    static void RunChatbot()
    {
        // Ask for the user's name
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Before we begin, what's your name? ");
        Console.ResetColor();
        userName = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(userName))
        {
            userName = "friend"; // Fallback name
        }

        TypeEffect($"Nice to meet you, {userName}! Let’s talk about staying safe online.");
        // Main conversation loop
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"\n{userName}, ask me something about cybersecurity: ");
            Console.ResetColor();

            string userInput = Console.ReadLine()?.ToLower();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                TypeEffect("I didn’t quite understand that. Could you rephrase?");
                Console.ResetColor();
                continue;
            }

            bool responded = false;

            // Sentiment Detection
            foreach (var sentiment in sentimentResponses.Keys)
            {
                if (userInput.Contains(sentiment))
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    TypeEffect($"{sentimentResponses[sentiment]} {userName}, I'm here to guide you.");
                    Console.ResetColor();
                    responded = true;
                    break;
                }
            }

            // Basic Response System
            if (!responded && userInput.Contains("how are you"))
            {
                TypeEffect($"I'm a bot, so I don’t have feelings, but I’m always ready to help you, {userName}.");
                responded = true;
            }

            if (!responded && userInput.Contains("what’s your purpose"))
            {
                TypeEffect("I educate users about cybersecurity to help them stay safe online.");
                responded = true;
            }

            if (!responded && userInput.Contains("what can i ask you about"))
            {
                TypeEffect("You can ask me about password security, scams and privacy settings.");
                responded = true;
            }

            // Memory Storage - Save user interests
            if (!responded && userInput.Contains("interested in"))
            {
                string topic = userInput.Split(new[] { "interested in" }, StringSplitOptions.None).Last().Trim();
                userMemory["topic"] = topic;
                TypeEffect($"Got it, {userName}. I'll remember that you're interested in {topic}.");
                responded = true;
            }

            // Memory Recall
            if (!responded && userMemory.ContainsKey("topic") &&
                (userInput.Contains("remember") || userInput.Contains("privacy") || userInput.Contains("security settings")))
            {
                TypeEffect($"{userName}, since you're interested in {userMemory["topic"]}, you might want to review your account's security settings.");
                responded = true;
            }

            // Keyword Recognition
            foreach (var keyword in keywordResponses.Keys)
            {
                if (userInput.Contains(keyword))
                {
                    string response = keywordResponses[keyword][new Random().Next(keywordResponses[keyword].Length)];
                    lastTopic = keyword;
                    TypeEffect($"{userName}, here's a tip: {response}");
                    responded = true;
                    break;
                }
            }

            // Follow-up Questions
            if (!responded && (userInput.Contains("explain more") || userInput.Contains("tell me more") || userInput.Contains("i don't understand")))
            {
                if (!string.IsNullOrEmpty(lastTopic) && keywordResponses.ContainsKey(lastTopic))
                {
                    string extraResponse = keywordResponses[lastTopic][new Random().Next(keywordResponses[lastTopic].Length)];
                    TypeEffect($"Here’s more on {lastTopic}, {userName}: {extraResponse}");
                }
                else
                {
                    TypeEffect($"I’d love to help, {userName}. Could you specify which topic you need more details on?");
                }
                responded = true;
            }

            // Default Response
            if (!responded)
            {
                TypeEffect($"I’m not sure how to respond to that, {userName}. Try asking about cybersecurity topics.");
            }
        }
    }
    // Simulates typing effect 
    static void TypeEffect(string message)
    {
        foreach (char c in message)
        {
            Console.Write(c);
            Thread.Sleep(50); // Delay to mimic typing
        }
        Console.WriteLine();
    }
}
// Troelson, A. and Japikse, P., 2022. Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming. 11th ed. Chambersburg: Apress. 