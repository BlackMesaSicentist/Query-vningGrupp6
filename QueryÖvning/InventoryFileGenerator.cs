using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query_Expressions_Gruppövning
{
    class InventoryFileGenerator
    {
        private static Random random = new Random();

        private static List<string> categories = new List<string>
    {
        "Verktyg", "Fästmaterial", "Elektronik", "Säkerhetsutrustning",
        "Elektriska verktyg", "Måleriutrustning", "Byggmaterial", "Belysning",
        "Rördelar", "Trädgårdsredskap"
    };

        private static List<string> productNames = new List<string>
    {
        "Skruvmejsel", "Hammare", "Spik 100-pack", "Multimeter", "Skyddsglasögon",
        "Arbetshandskar", "Batteridriven borrmaskin", "Målarpensel set", "Gipsskivor 120x240cm",
        "LED-lampa 60W", "Skiftnyckel", "Vattenpass", "Muttrar M8 50-pack", "Lödkolv",
        "Hörselskydd", "Skyddsskor", "Cirkelsåg", "Roller med skaft", "OSB-skivor 120x240cm",
        "Arbetslampa LED", "Rörtång", "Trädgårdsspade", "Bultpistol", "Laseravståndsmätare"
    };

        public void GenerateInventoryFile(string fileName, int numberOfProducts)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine("Id,Name,Category,Quantity,Price,LastRestocked");

                for (int i = 1; i <= numberOfProducts; i++)
                {
                    string name = productNames[random.Next(productNames.Count)];
                    string category = categories[random.Next(categories.Count)];
                    int quantity = random.Next(10, 1001);
                    decimal price = Math.Round((decimal)(random.NextDouble() * 1000));
                    DateTime lastRestocked = DateTime.Now.AddDays(-random.Next(365));

                    writer.WriteLine($"{i},{name},{category},{quantity},{price},{lastRestocked:yyyy-MM-dd}");
                }
            }

            Console.WriteLine($"Inventory file '{fileName}' has been generated with {numberOfProducts} products.");
        }


    }
}