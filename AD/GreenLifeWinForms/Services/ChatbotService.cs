using System;
using System.Collections.Generic;
using System.Linq;

namespace GreenLifeWinForms.Services
{
    public class ChatbotService
    {
        private readonly List<KnowledgeEntry> knowledgeBase;

        public ChatbotService()
        {
            knowledgeBase = BuildKnowledgeBase();
        }

        /// <summary>
        /// Process a user message and return a chatbot response
        /// </summary>
        public string GetResponse(string userMessage)
        {
            if (string.IsNullOrWhiteSpace(userMessage))
                return "Please type a message so I can help you! ??";

            string input = userMessage.Trim().ToLower();

            // Greeting detection
            if (IsGreeting(input))
                return "Hello! ?? Welcome to GreenLife Organic Store.\n\nHow can I help you today? You can ask me about:\n• Products & categories\n• Orders & tracking\n• Payments & pricing\n• Account & registration\n• Returns & refunds\n• Store information";

            // Farewell detection
            if (IsFarewell(input))
                return "Thank you for chatting with us! ??\nHave a wonderful day and happy shopping!";

            // Thank you detection
            if (IsThankYou(input))
                return "You're welcome! ?? Is there anything else I can help you with?";

            // Find best matching knowledge entry
            KnowledgeEntry bestMatch = null;
            int bestScore = 0;

            foreach (KnowledgeEntry entry in knowledgeBase)
            {
                int score = CalculateMatchScore(input, entry.Keywords);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMatch = entry;
                }
            }

            if (bestMatch != null && bestScore >= 2)
                return bestMatch.Response;

            // Fallback
            return "I'm sorry, I didn't quite understand that. ??\n\nHere are some things I can help with:\n• Product information & categories\n• How to place an order\n• Order tracking & status\n• Payment methods\n• Account registration\n• Returns & refunds\n\nPlease try rephrasing your question, or type \"help\" for more options.";
        }

        /// <summary>
        /// Get a list of quick-reply suggestions
        /// </summary>
        public List<string> GetQuickReplies()
        {
            return new List<string>
            {
                "What products do you have?",
                "How do I place an order?",
                "Track my order",
                "Payment methods",
                "Return policy",
                "Contact support"
            };
        }

        // ?? Match scoring ????????????????????????????????????????????????
        private int CalculateMatchScore(string input, string[] keywords)
        {
            int score = 0;
            string[] inputWords = input.Split(new[] { ' ', ',', '.', '?', '!', ';', ':' },
                                               StringSplitOptions.RemoveEmptyEntries);

            foreach (string keyword in keywords)
            {
                string kw = keyword.ToLower();
                // Exact word match
                if (inputWords.Any(w => w == kw))
                    score += 3;
                // Contains as substring
                else if (input.Contains(kw))
                    score += 2;
                // Partial overlap (first 4 chars)
                else if (kw.Length >= 4 && inputWords.Any(w => w.Length >= 4 && (w.StartsWith(kw.Substring(0, 4)) || kw.StartsWith(w.Substring(0, 4)))))
                    score += 1;
            }

            return score;
        }

        private bool IsGreeting(string input)
        {
            string[] greetings = { "hi", "hello", "hey", "good morning", "good afternoon",
                                   "good evening", "howdy", "greetings", "sup", "yo" };
            return greetings.Any(g => input == g || input.StartsWith(g + " ") || input.StartsWith(g + ","));
        }

        private bool IsFarewell(string input)
        {
            string[] farewells = { "bye", "goodbye", "see you", "take care", "good night",
                                   "farewell", "later", "cya", "quit", "exit" };
            return farewells.Any(f => input == f || input.Contains(f));
        }

        private bool IsThankYou(string input)
        {
            string[] thanks = { "thank", "thanks", "thx", "appreciate", "grateful" };
            return thanks.Any(t => input.Contains(t));
        }

        // ?? Knowledge base ???????????????????????????????????????????????
        private List<KnowledgeEntry> BuildKnowledgeBase()
        {
            return new List<KnowledgeEntry>
            {
                // Products
                new KnowledgeEntry(
                    new[] { "product", "products", "sell", "available", "items", "what", "have", "offer", "catalog" },
                    "?? We offer a wide range of organic products including:\n\n" +
                    "?? Fruits — Fresh organic fruits\n" +
                    "?? Vegetables — Farm-fresh vegetables\n" +
                    "?? Dairy — Milk, cheese, yogurt\n" +
                    "?? Grains — Rice, wheat, oats\n" +
                    "?? Beverages — Juices, teas, organic drinks\n" +
                    "?? Snacks — Healthy organic snacks\n" +
                    "?? Meat & Poultry — Organic meats\n" +
                    "?? Bakery — Fresh baked goods\n" +
                    "?? Seafood — Fresh organic seafood\n\n" +
                    "Browse our full catalog from the dashboard using 'Browse Products'!"),

                new KnowledgeEntry(
                    new[] { "category", "categories", "types", "sections", "departments" },
                    "?? Our product categories are:\n• Fruits\n• Vegetables\n• Dairy\n• Grains\n• Beverages\n• Snacks\n• Meat & Poultry\n• Bakery\n• Seafood\n• Spreads\n\nUse the category filter on the Browse Products page to find what you need!"),

                new KnowledgeEntry(
                    new[] { "organic", "natural", "healthy", "quality", "fresh" },
                    "?? All our products are 100% certified organic!\n\nWe source directly from trusted organic farms and suppliers to ensure the highest quality. Every product is fresh, natural, and free from harmful chemicals."),

                new KnowledgeEntry(
                    new[] { "price", "cost", "expensive", "cheap", "affordable", "pricing", "how much" },
                    "?? Our prices are displayed in LKR (Sri Lankan Rupees).\n\nWe offer competitive pricing on all organic products. Many items also have special discounts — look for the discount badges on product cards!\n\nCheck the Browse Products page for current prices."),

                new KnowledgeEntry(
                    new[] { "discount", "offer", "sale", "promotion", "deal", "coupon" },
                    "??? We regularly offer discounts on selected products!\n\nLook for the red discount badges on product cards showing the percentage off. Discounted prices are calculated automatically at checkout.\n\nKeep checking back for new promotions!"),

                // Orders
                new KnowledgeEntry(
                    new[] { "order", "place", "buy", "purchase", "how", "checkout" },
                    "??? How to place an order:\n\n1?? Click 'Browse Products' from the dashboard\n2?? Find products you like and set the quantity\n3?? Click 'Add to Cart' for each product\n4?? Click 'View Cart' to review your items\n5?? Adjust quantities if needed\n6?? Click 'Proceed to Checkout'\n7?? Confirm your order — done! ?\n\nYou'll receive an order confirmation with your Order ID."),

                new KnowledgeEntry(
                    new[] { "track", "tracking", "status", "where", "delivery", "shipped", "pending" },
                    "?? To track your order:\n\n1?? Go to the Dashboard\n2?? Click 'Track Orders'\n3?? View all your orders with their current status\n4?? Click 'View Details' for full order information\n\nOrder statuses:\n• Pending — Order received, being processed\n• Shipped — On the way to you\n• Delivered — Successfully delivered"),

                new KnowledgeEntry(
                    new[] { "cart", "shopping cart", "basket", "bag" },
                    "?? Your shopping cart:\n\n• Add items from the Browse Products page\n• View your cart using the '??? View Cart' button\n• Change quantities directly in the cart\n• Remove items you don't want\n• See subtotal, discounts, and grand total\n• Proceed to checkout when ready!"),

                // Account
                new KnowledgeEntry(
                    new[] { "register", "signup", "sign up", "create", "account", "new" },
                    "?? To create a new account:\n\n1?? On the login screen, click 'Register here'\n2?? Fill in your details:\n   • Full Name\n   • Email Address\n   • Phone Number\n   • Address\n   • Password (min 6 characters)\n3?? Click 'Register'\n4?? Login with your email and password\n\nYour password is securely encrypted! ??"),

                new KnowledgeEntry(
                    new[] { "login", "sign in", "signin", "log in", "access" },
                    "?? To login:\n\n1?? Enter your registered email address\n2?? Enter your password\n3?? Click 'Login'\n\nIf you forgot your password, please contact our support team for assistance."),

                new KnowledgeEntry(
                    new[] { "password", "forgot", "reset", "change", "security" },
                    "?? Password information:\n\n• Your password is encrypted with SHA-256 for security\n• Minimum 6 characters required\n• To change your password, go to My Profile\n• If you forgot your password, contact support\n\nWe never store passwords in plain text!"),

                new KnowledgeEntry(
                    new[] { "profile", "my profile", "personal", "details", "information", "update" },
                    "?? Your profile:\n\nYou can view and manage your profile from the 'My Profile' button on the dashboard. Here you can see your:\n• Full Name\n• Email Address\n• Phone Number\n• Address\n• Registration Date"),

                // Reviews
                new KnowledgeEntry(
                    new[] { "review", "reviews", "rating", "rate", "star", "feedback" },
                    "? Product reviews:\n\n• View reviews on the Browse Products page (star ratings)\n• Write reviews from 'My Reviews' on the dashboard\n• Rate products from 1 to 5 stars\n• Add comments about your experience\n• Edit or delete your reviews anytime\n\nYour feedback helps other customers!"),

                // Payment
                new KnowledgeEntry(
                    new[] { "payment", "pay", "money", "method", "cash", "card", "transfer" },
                    "?? Payment information:\n\nCurrently we accept:\n• Cash on Delivery (COD)\n• Bank Transfer\n\nAll prices are shown in LKR (Sri Lankan Rupees). Payment is collected upon delivery of your order."),

                // Returns
                new KnowledgeEntry(
                    new[] { "return", "refund", "exchange", "complaint", "problem", "issue", "damaged" },
                    "?? Returns & Refunds:\n\n• If your product arrives damaged, contact us within 24 hours\n• We offer full refunds for defective items\n• Returns must be in original packaging\n• Perishable items must be reported immediately\n\nContact our support team for return requests."),

                // Contact / Support
                new KnowledgeEntry(
                    new[] { "contact", "support", "help", "phone", "email", "reach", "customer service" },
                    "?? Contact Us:\n\n• Email: support@greenlife.lk\n• Phone: +94 11 234 5678\n• Hours: Mon-Sat 8:00 AM - 8:00 PM\n• Address: 123 Green Street, Colombo, Sri Lanka\n\nOur support team is always happy to help!"),

                // Store info
                new KnowledgeEntry(
                    new[] { "about", "store", "greenlife", "company", "who", "mission" },
                    "?? About GreenLife Organic Store:\n\nGreenLife is Sri Lanka's trusted organic store, committed to bringing you the freshest, healthiest organic products.\n\nOur mission: Making organic living accessible and affordable for everyone.\n\nWe source directly from certified organic farms to ensure quality you can trust."),

                new KnowledgeEntry(
                    new[] { "hours", "open", "close", "time", "working", "schedule" },
                    "?? Store Hours:\n\n• Monday - Friday: 8:00 AM - 8:00 PM\n• Saturday: 8:00 AM - 6:00 PM\n• Sunday: 9:00 AM - 4:00 PM\n\nOnline orders can be placed 24/7!"),

                new KnowledgeEntry(
                    new[] { "delivery", "shipping", "deliver", "ship", "arrive", "time", "long" },
                    "?? Delivery Information:\n\n• Standard delivery: 2-3 business days\n• Colombo area: Next-day delivery available\n• Free delivery for orders above LKR 5,000\n• Delivery fee: LKR 300 for orders below LKR 5,000\n\nTrack your delivery from the 'Track Orders' page!"),

                // Help
                new KnowledgeEntry(
                    new[] { "help", "assist", "guide", "how to", "instructions", "tutorial" },
                    "?? Here's what I can help you with:\n\n?? Products — Ask about our product categories\n?? Orders — How to place and track orders\n?? Payments — Payment methods accepted\n?? Account — Registration and login help\n? Reviews — How to rate products\n?? Returns — Return and refund policy\n?? Contact — How to reach us\n?? Delivery — Shipping information\n??? Discounts — Current promotions\n\nJust ask me anything!"),
            };
        }

        // ?? Knowledge entry model ????????????????????????????????????????
        private class KnowledgeEntry
        {
            public string[] Keywords { get; }
            public string Response { get; }

            public KnowledgeEntry(string[] keywords, string response)
            {
                Keywords = keywords;
                Response = response;
            }
        }
    }
}
