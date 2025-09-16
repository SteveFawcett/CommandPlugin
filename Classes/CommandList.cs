using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CyberDog.Interfaces;
using System.Web;

namespace Command.Classes
{
    public class CommandList
    {
        public Item[] Items => items.ToArray();

        private static List<Item> items = [];
        public class Item : IUpdatableItem
        {
            public required string Id { get; set; }
            public required string Key { get; set; }
            public required string Description { get; set; }
            public required string Value { get; set; } 
            public override string ToString()
            {
                return $"[{Id}]:{Key}";
            }
        }

        public CommandList(ILogger<Command> logger, IConfiguration? config)
        {
            if (config == null) return;

            logger.LogInformation("Loading Value List");

            foreach (var i in config.GetSection("Commands").GetChildren())
            {
                items.Add(new Item
                {
                    Id = i["Id"] ?? "",
                    Key = i["Key"] ?? "",
                    Description = i["Description"] ?? "",
                    Value = i["Value"] ?? ""
                });
            }

            logger.LogInformation($"Loaded {items.Count} Commands");
            foreach (var item in items)
            {
                logger.LogInformation(item.ToString());
            }
        }

        public static Item GetCommandDetails(string id)
        {
            foreach (var item in items)
            {
                if (item.Id == id) return item;
            }

            throw new KeyNotFoundException($"Value with ID '{id}' not found.");
        }
    }
}
