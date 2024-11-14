//Grupp 6, Alex Bullerjahn, Angelica Bergström, Adam Axelsson-Hedman, David Berglin

namespace Query_Expressions_Gruppövning
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Quic;

    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime LastRestocked { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Category: {Category}, Quantity: {Quantity}, Price: {Price:C}, Last Restocked: {LastRestocked:d}";
        }
    }

    class Program
    {
        static List<Product> inventory;
        //Quit used to shut down program if choosen in menu
        static Boolean _quit = false;

        static void Main(string[] args)
        {
            //InventoryFileGenerator inventoryFileGenerator = new InventoryFileGenerator();
            //inventoryFileGenerator.GenerateInventoryFile("inventory.txt", 5000);
            var prod = LoadInventoryData();


            do
            {
                QueryMenu(inventory);
            } while (_quit == false);

            //Filename
            string fileName = "inventory.txt";

            //Save to file
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine("Id,Name,Category,Quantity,Price,LastRestocked");


                //for (int i = 1; i <= counting; i++)
                foreach (var tools in inventory)
                {
                    int i = tools.Id;
                    string name = tools.Name;
                    string category = tools.Category;
                    int quantity = tools.Quantity;
                    decimal price = Math.Round(tools.Price);
                    DateTime lastRestocked = tools.LastRestocked;

                    writer.WriteLine($"{i},{name},{category},{quantity},{price},{lastRestocked:yyyy-MM-dd}");
                };
            }

        }

        static List<Product> LoadInventoryData()
        {
            string[] lines = File.ReadAllLines("inventory.txt");
            inventory = lines.Skip(1) // Hoppa över rubrikraden
                            .Select(line =>
                            {
                                var parts = line.Split(',');
                                return new Product
                                {
                                    Id = int.Parse(parts[0]),
                                    Name = parts[1],
                                    Category = parts[2],
                                    Quantity = int.Parse(parts[3]),
                                    Price = decimal.Parse(parts[4], CultureInfo.InvariantCulture),
                                    LastRestocked = DateTime.ParseExact(parts[5], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                };
                            }).ToList();
            return inventory;
        }
        //Increase cost of every product in the category "Elektronik" by 10%
        static void IncreaseCostOfElectronics(List<Product> products)
        {
            foreach (var product in products.Where(electronics => electronics.Category == "Elektronik").OrderBy(x => x.Id))
            {
                //B4 update price
                Console.WriteLine($"{product.ToString()}");
                product.Price += product.Price * 0.10m;
                //After update price
                Console.WriteLine($"{product.ToString()}");
            }

        }
        //List average product cost of each category
        static void CreateAndListCategoryGroupAverageCost(List<Product> products)
        {
            var sortByCategoryPrice = products.GroupBy(product => product.Category)
            .Select(groups => new
            {
                Category = groups.Key,
                AveragePrice = groups.Average(x => x.Price),
                ProductCount = groups.Count()
            });

            sortByCategoryPrice.OrderBy(x => x.AveragePrice);
            foreach (var group in sortByCategoryPrice)
            {
                Console.WriteLine($"Category: {group.Category} Average price: {group.AveragePrice:F2} Product count: {group.ProductCount}");
            }
        }
        //Menu to chose query
        static void QueryMenu(List<Product> products)
        {
            do
            {
                Console.WriteLine("Choose Query: " +
                    "\n1. List all products in category \"Verktyg\" " +
                    "\n2. Find the 5 products with the lowest quantity" +
                    "\n3. Calculate the value of all products in inventory" +
                    "\n4. Group products into categories and list the number of products in each category" +
                    "\n5. Find products that havent been restocked in the last 30 days" +
                    "\n6. Increase price of all electronics by 10%" +
                    "\n7. Create a list with product name and inventory value (price * quantity) for product with" +
                    "inventory value above 1000 kr." +
                    "\n8. Find the category with the highest average price" +
                    "\n9. Quit");


                switch (Console.ReadLine())
                {
                    case "1": ListProductsInCategoryVerktyg(); Console.WriteLine(); break;
                    case "2": InventoryLowValue(); Console.WriteLine(); break;
                    case "3": TotalValue(); Console.WriteLine(); break;
                    case "4": SortByCategory(products); Console.WriteLine(); break;
                    case "5": MoreThan30DaysSinceLastRestock(products); Console.WriteLine(); break;
                    case "6": IncreaseCostOfElectronics(products); Console.WriteLine(); break;
                    case "7": InventoryValue1000(); Console.WriteLine(); break;
                    case "8": CreateAndListCategoryGroupAverageCost(products); Console.WriteLine(); break;
                    case "9": _quit = true; break;
                    default: Console.WriteLine("Not a valid choice"); Console.WriteLine(); break;
                }
            }while (_quit == false);
        }

        //Hitta de 5 produkter som har lägst lagersaldo och behöver beställas.
        static void InventoryLowValue()
        {
            var lagersaldo = inventory
            .OrderBy(x => x.Quantity)
            .Take(5)
            .Select(x => new { x.Name, x.Quantity });

            foreach (var product in lagersaldo)
            {
                Console.WriteLine(product);
            }
            Console.WriteLine();
        }

        //Skapa en lista med produktnamn och dess lagervärde (pris * kvantitet) för produkter med ett lagervärde över 1000 kr.

        static void InventoryValue1000()
        {

            var inventoryValue = inventory.Where(x => x.Price * x.Quantity > 1000);

            foreach (var product in inventoryValue)
            {
                var inventoryValueSolo = product.Price * product.Quantity;
                Console.WriteLine($"Name:{product.Name}\nQuantity: {inventoryValueSolo}\n");
            }
            Console.WriteLine();
        }

        //SortByCategory(prod);
        private static void SortByCategory(List<Product> prod)
        {
            Console.WriteLine("\n\nÖvning 4: Gruppera produkterna efter kategori och visa antalet produkter i varje kategori. \n");


            var productsCategoryAmount = prod.GroupBy(x => x.Category).OrderBy(y => y.Key)
                                         .Select(y => new
                                         {
                                             Type = y.Key,
                                             Count = y.Count(),
                                             Tools = y.OrderBy(p => p.Name)
                                         });

            foreach (var group in productsCategoryAmount)
            {
                Console.WriteLine($"Kategori: {group.Type}");
                Console.WriteLine($"Antal olika produkter: {group.Count}");
                Console.WriteLine("Produkter:");
                foreach (var tools in group.Tools)
                {
                    Console.WriteLine($" Produkct: {tools.Name} (Antal: {tools.Quantity}, Pris: {tools.Price})");
                }
                Console.WriteLine();
            }
        }


        //MoreThan30DaysSinceLastRestock(prod);
        private static void MoreThan30DaysSinceLastRestock(List<Product> prod)
        {
            Console.WriteLine("\n\nÖvning 5: Hitta alla produkter som inte har blivit påfyllda de senaste 30 dagarna. \n");

            DateTime timeNow = DateTime.Now;
            TimeSpan month = TimeSpan.FromDays(30);

            var lastRestocked = prod.Where(x => timeNow.Subtract(x.LastRestocked) > month).OrderBy(x => x.LastRestocked);

            foreach (var tools in lastRestocked)
            {
                Console.WriteLine($"Produkt: {tools.Name}, Antal: {tools.Quantity}, senast påfyllda: {tools.LastRestocked}. ");
            }
            Console.WriteLine();
        }
        static void ToolPriceOrder()
        {
            var order = inventory
         .OrderBy(p => p.Price);
            foreach (var item in order)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
        }
        //Inventory total value
        static void TotalValue()
        {
            decimal totalV = inventory.Sum(p => p.Quantity * p.Price);
            Console.WriteLine($"Inventory value{totalV}");
            Console.WriteLine();
        }
        //Lists products in category "Verktyg"
        static void ListProductsInCategoryVerktyg()
        {
            foreach (var product in inventory.Where(x => x.Category == "Verktyg"))
            {
                Console.WriteLine(product.ToString());
            }
            Console.WriteLine();
        }

    }
}