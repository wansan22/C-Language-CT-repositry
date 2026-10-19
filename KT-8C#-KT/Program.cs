using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;

public  class Program
{  
    public class Product
    {
        public int Id { get; init;}
        public string Name {get; set; }
        public string Category {get; set;}
        public decimal Price {get; init;}
        public int Stock {get; set;}
    }


    private static void Main()
    {
    
        var listforlinq1 = Enumerable.Range(1,1000).Select( _ => Random.Shared.Next(-100,10001)).ToList();
         //  a
        
        IEnumerable<int> x1 = listforlinq1.Where(n => n % 2 == 0); // 1
        IEnumerable<int> x2 = listforlinq1.Where( n => n > 0).OrderByDescending(n => n);
        IEnumerable<int> x3 = listforlinq1.Select(n => n % 2 == 0 ? n * n : n * n * n);
        IEnumerable<int> x5 = listforlinq1.Where(n => n % 2 == 0 && n > 15 && n < 600).Select(n => n*n).OrderBy(n => n).Distinct();

        // б
       string y = "Apple, Io, Cat, Dog, Sun, Sky, Box, Car, Pen, Book, Tree, House, Computer, Elephant, Butterfly, Photography, Unbelievable, Reflection, Transportation, A, I, Go, Hi, It, We, At, On, Up, My, Zoo, Bus, Key, Cup, Map, Hat, Bed, Lamp, Door, Window, Garden, Forest, Mountain, River, Ocean, Universe, Astronomy, Mathematics, Programming, Artificial, Intelligence, Microscope, Telescope, Encyclopedia, Congratulations, Incomprehensibility, Antidisestablishmentarianism, Run, Eat, Sleep, Code, Read, Write, Sing, Dance, Play, Work, Study, Explore, Discover, Imagine, Create, Innovate, Communicate, Investigate, Elaborate, Celebrate, Orange, Banana, Cherry, Grape, Lemon, Melon, Kiwi, Peach, Plum, Mango, Strawberry, Raspberry, Blueberry, Blackberry, Pineapple, Watermelon, Pomegranate, Lightning, Thunder, Storm, Rain, Snow, Wind, Cloud, Rainbow, Hurricane, Tornado, Temperature, Atmosphere, Precipitation, Evaporation";
        List<string> wordlist = y.Split(", ", StringSplitOptions.RemoveEmptyEntries).ToList();
    
        var ex1 = wordlist.Where(w => w.StartsWith("A"));
        var ex2 = wordlist.Where(w => w.Length > 5).OrderBy(w => w, StringComparer.OrdinalIgnoreCase);
        string? ex3 = wordlist.MaxBy(w => w.Length);
        var ex4 = wordlist.Where(w => w.Length >= 3 && w.Length < 8).FirstOrDefault();

        // в
        List<Product> products = new List<Product>()
        {
new Product { Id = 1, Name = "iPhone 15 Pro", Category = "Electronics", Price = 999.99m, Stock = 25 },

new Product { Id = 2, Name = "Samsung Galaxy S24", Category = "Electronics", Price = 849.99m, Stock = 30 },

new Product { Id = 3, Name = "MacBook Air M3", Category = "Electronics", Price = 1299.99m, Stock = 15 },

new Product { Id = 4, Name = "Sony WH-1000XM5", Category = "Electronics", Price = 349.99m, Stock = 40 },

new Product { Id = 5, Name = "iPad Air", Category = "Electronics", Price = 599.99m, Stock = 20 },

new Product { Id = 6, Name = "Apple Watch Series 9", Category = "Electronics", Price = 399.99m, Stock = 35 },

new Product { Id = 7, Name = "Dell XPS 13", Category = "Electronics", Price = 1199.99m, Stock = 12 },

new Product { Id = 8, Name = "Nintendo Switch OLED", Category = "Electronics", Price = 349.99m, Stock = 28 },

new Product { Id = 9, Name = "PlayStation 5", Category = "Electronics", Price = 499.99m, Stock = 8 },

new Product { Id = 10, Name = "Xbox Series X", Category = "Electronics", Price = 499.99m, Stock = 10 },

new Product { Id = 11, Name = "AirPods Pro", Category = "Electronics", Price = 249.99m, Stock = 45 },

new Product { Id = 12, Name = "Smart TV 55\"", Category = "Electronics", Price = 699.99m, Stock = 18 },

new Product { Id = 13, Name = "Gaming Monitor", Category = "Electronics", Price = 399.99m, Stock = 22 },

new Product { Id = 14, Name = "External SSD 1TB", Category = "Electronics", Price = 129.99m, Stock = 60 },

new Product { Id = 15, Name = "Mechanical Keyboard", Category = "Electronics", Price = 89.99m, Stock = 35 },

new Product { Id = 16, Name = "KitchenAid Mixer", Category = "Kitchen", Price = 429.99m, Stock = 18 },

new Product { Id = 17, Name = "Non-Stick Frying Pan", Category = "Kitchen", Price = 49.99m, Stock = 65 },

new Product { Id = 18, Name = "Chef's Knife Set", Category = "Kitchen", Price = 129.99m, Stock = 32 },

new Product { Id = 19, Name = "Air Fryer", Category = "Kitchen", Price = 89.99m, Stock = 45 },

new Product { Id = 20, Name = "Blender", Category = "Kitchen", Price = 79.99m, Stock = 38 },

new Product { Id = 21, Name = "Coffee Maker", Category = "Kitchen", Price = 199.99m, Stock = 22 },

new Product { Id = 22, Name = "Toaster", Category = "Kitchen", Price = 39.99m, Stock = 55 },

new Product { Id = 23, Name = "Food Processor", Category = "Kitchen", Price = 149.99m, Stock = 25 },

new Product { Id = 24, Name = "Cutting Board", Category = "Kitchen", Price = 24.99m, Stock = 80 },

new Product { Id = 25, Name = "Dinner Plate Set", Category = "Kitchen", Price = 69.99m, Stock = 42 },

new Product { Id = 26, Name = "Pressure Cooker", Category = "Kitchen", Price = 119.99m, Stock = 28 },

new Product { Id = 27, Name = "Kitchen Scale", Category = "Kitchen", Price = 29.99m, Stock = 50 },

new Product { Id = 28, Name = "Leather Sofa", Category = "Furniture", Price = 1299.99m, Stock = 5 },

new Product { Id = 29, Name = "Coffee Table", Category = "Furniture", Price = 299.99m, Stock = 15 },

new Product { Id = 30, Name = "Bookshelf", Category = "Furniture", Price = 199.99m, Stock = 20 },

new Product { Id = 31, Name = "Office Chair", Category = "Furniture", Price = 249.99m, Stock = 25 },

new Product { Id = 32, Name = "Dining Table", Category = "Furniture", Price = 899.99m, Stock = 8 },

new Product { Id = 33, Name = "Bed Frame", Category = "Furniture", Price = 599.99m, Stock = 12 },

new Product { Id = 34, Name = "Wardrobe", Category = "Furniture", Price = 799.99m, Stock = 7 },

new Product { Id = 35, Name = "Nightstand", Category = "Furniture", Price = 149.99m, Stock = 30 },

new Product { Id = 36, Name = "Cotton T-Shirt", Category = "Clothing", Price = 19.99m, Stock = 100 },

new Product { Id = 37, Name = "Jeans", Category = "Clothing", Price = 49.99m, Stock = 75 },

new Product { Id = 38, Name = "Winter Jacket", Category = "Clothing", Price = 129.99m, Stock = 35 },

new Product { Id = 39, Name = "Running Shoes", Category = "Clothing", Price = 89.99m, Stock = 60 },

new Product { Id = 40, Name = "Dress Shirt", Category = "Clothing", Price = 39.99m, Stock = 85 },

new Product { Id = 41, Name = "Sweater", Category = "Clothing", Price = 59.99m, Stock = 45 },

new Product { Id = 42, Name = "Skirt", Category = "Clothing", Price = 34.99m, Stock = 55 },

new Product { Id = 43, Name = "Sports Shorts", Category = "Clothing", Price = 24.99m, Stock = 90 },

new Product { Id = 44, Name = "Winter Boots", Category = "Clothing", Price = 119.99m, Stock = 28 },

new Product { Id = 45, Name = "Baseball Cap", Category = "Clothing", Price = 14.99m, Stock = 120 },

new Product { Id = 46, Name = "Dress", Category = "Clothing", Price = 69.99m, Stock = 40 },

new Product { Id = 47, Name = "Socks Pack", Category = "Clothing", Price = 12.99m, Stock = 150 },

new Product { Id = 48, Name = "Leather Jacket", Category = "Clothing", Price = 199.99m, Stock = 20 },

new Product { Id = 49, Name = "Swimwear", Category = "Clothing", Price = 44.99m, Stock = 65 },

new Product { Id = 50, Name = "Blazer", Category = "Clothing", Price = 89.99m, Stock = 30 },

new Product { Id = 51, Name = "Pajamas", Category = "Clothing", Price = 34.99m, Stock = 70 },

new Product { Id = 52, Name = "Scarf", Category = "Clothing", Price = 19.99m, Stock = 85 },

new Product { Id = 53, Name = "Gloves", Category = "Clothing", Price = 24.99m, Stock = 60 },

new Product { Id = 54, Name = "Basketball", Category = "Sports", Price = 29.99m, Stock = 50 },

new Product { Id = 55, Name = "Yoga Mat", Category = "Sports", Price = 34.99m, Stock = 65 },

new Product { Id = 56, Name = "Dumbbell Set", Category = "Sports", Price = 149.99m, Stock = 25 },

new Product { Id = 57, Name = "Tennis Racket", Category = "Sports", Price = 89.99m, Stock = 30 },

new Product { Id = 58, Name = "Football", Category = "Sports", Price = 24.99m, Stock = 70 },

new Product { Id = 59, Name = "Running Watch", Category = "Sports", Price = 199.99m, Stock = 20 },

new Product { Id = 60, Name = "Cycling Helmet", Category = "Sports", Price = 59.99m, Stock = 40 },

new Product { Id = 61, Name = "Swimming Goggles", Category = "Sports", Price = 19.99m, Stock = 85 },

new Product { Id = 62, Name = "Hiking Backpack", Category = "Sports", Price = 129.99m, Stock = 32 },

new Product { Id = 63, Name = "Foundation", Category = "Beauty", Price = 34.99m, Stock = 60 },

new Product { Id = 64, Name = "Lipstick", Category = "Beauty", Price = 19.99m, Stock = 95 },

new Product { Id = 65, Name = "Perfume", Category = "Beauty", Price = 89.99m, Stock = 40 },

new Product { Id = 66, Name = "Hair Dryer", Category = "Beauty", Price = 49.99m, Stock = 35 },

new Product { Id = 67, Name = "Face Cream", Category = "Beauty", Price = 29.99m, Stock = 80 },

new Product { Id = 68, Name = "Electric Shaver", Category = "Beauty", Price = 79.99m, Stock = 25 },

new Product { Id = 69, Name = "Nail Polish", Category = "Beauty", Price = 9.99m, Stock = 150 },

new Product { Id = 70, Name = "Makeup Brushes", Category = "Beauty", Price = 39.99m, Stock = 45 },

new Product { Id = 71, Name = "Sunscreen", Category = "Beauty", Price = 14.99m, Stock = 110 },

new Product { Id = 72, Name = "Shampoo", Category = "Beauty", Price = 12.99m, Stock = 200 },

new Product { Id = 73, Name = "Face Mask", Category = "Beauty", Price = 7.99m, Stock = 180 },

new Product { Id = 74, Name = "Harry Potter Book Set", Category = "Books", Price = 79.99m, Stock = 28 },

new Product { Id = 75, Name = "Cookbook", Category = "Books", Price = 24.99m, Stock = 55 },

new Product { Id = 76, Name = "Sci-Fi Novel", Category = "Books", Price = 14.99m, Stock = 75 },

new Product { Id = 77, Name = "Children's Book", Category = "Books", Price = 9.99m, Stock = 120 },

new Product { Id = 78, Name = "Programming Guide", Category = "Books", Price = 44.99m, Stock = 35 },

new Product { Id = 79, Name = "Art Book", Category = "Books", Price = 59.99m, Stock = 22 },

new Product { Id = 80, Name = "LED Desk Lamp", Category = "Home", Price = 39.99m, Stock = 50 },

new Product { Id = 81, Name = "Throw Pillow", Category = "Home", Price = 24.99m, Stock = 85 },

new Product { Id = 82, Name = "Wall Clock", Category = "Home", Price = 34.99m, Stock = 40 },

new Product { Id = 83, Name = "Vase", Category = "Home", Price = 29.99m, Stock = 60 },

new Product { Id = 84, Name = "Curtains", Category = "Home", Price = 49.99m, Stock = 25 },

new Product { Id = 85, Name = "Rug", Category = "Home", Price = 129.99m, Stock = 18 },

new Product { Id = 86, Name = "Bedding Set", Category = "Home", Price = 89.99m, Stock = 32 },

new Product { Id = 87, Name = "Picture Frame", Category = "Home", Price = 14.99m, Stock = 95 },

new Product { Id = 88, Name = "Candle Set", Category = "Home", Price = 19.99m, Stock = 70 },

new Product { Id = 89, Name = "Mirror", Category = "Home", Price = 79.99m, Stock = 15 },

new Product { Id = 90, Name = "Blanket", Category = "Home", Price = 39.99m, Stock = 45 },

new Product { Id = 91, Name = "Storage Baskets", Category = "Home", Price = 24.99m, Stock = 65 },

new Product { Id = 92, Name = "Desk Organizer", Category = "Home", Price = 19.99m, Stock = 55 },

new Product { Id = 93, Name = "Action Figure", Category = "Toys", Price = 24.99m, Stock = 45 },

new Product { Id = 94, Name = "Lego Set", Category = "Toys", Price = 49.99m, Stock = 35 },

new Product { Id = 95, Name = "Board Game", Category = "Toys", Price = 39.99m, Stock = 28 },

new Product { Id = 96, Name = "Doll", Category = "Toys", Price = 29.99m, Stock = 40 },

new Product { Id = 97, Name = "Puzzle", Category = "Toys", Price = 19.99m, Stock = 55 },

new Product { Id = 98, Name = "Car Phone Mount", Category = "Automotive", Price = 19.99m, Stock = 60 },

new Product { Id = 99, Name = "Jump Starter", Category = "Automotive", Price = 89.99m, Stock = 18 },

new Product { Id = 100, Name = "Car Vacuum", Category = "Automotive", Price = 39.99m, Stock = 35 }
        };
        // В
        IEnumerable<Product> exb1 = products.Where(p => p.Category == "Electronics");
        var exb2 = products.OrderByDescending(p => p.Price).FirstOrDefault();
        var exb3 = products.Where(p => p.Stock > 0).Select(p => p.Name);
        var exb4 = products.GroupBy(p => p.Category).Select(p => p.Count());
        var exb5 = products.GroupBy(p => p.Category);
        
        foreach ( var i in exb5)
        {
            Console.WriteLine($"категория {i.Key}, кол-во {i.Count()}");
        }

        


    }
}

