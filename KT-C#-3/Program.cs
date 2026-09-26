using System;
using System.Collections.Generic;

public class Program
{
    public enum Rarity
    {
        common,
        rare,
        uncommon,
        legendary
    }
    
    public class GameItem
    {
        public string Name { get; set; } = string.Empty;
        public Rarity rarity = Rarity.common;
    }

    public class Inventory 
    {
        public List<GameItem> inventory { get; set; } = new List<GameItem>();

        public List<GameItem> AddItem(GameItem item)
        {
           
            if (item.rarity == Rarity.legendary)
            {
                for (int i = 0; i < inventory.Count; i++)
                {
                    if (inventory[i].Name == item.Name && inventory[i].rarity == Rarity.legendary)
                    {
                        throw new DoubleLegendaryException("Легендарный предмет уже есть в инвентаре");
                    }
                }
            }

            inventory.Add(item);
            return inventory;
        }

        public GameItem GetItem(string name)
        {
            List<GameItem> template = new List<GameItem>();
            if (inventory.Count <= 0)
            {
                throw new InvalidArgumentException("Инвентарь пустой");
            }
            else
            {
                for (int i = 0; i < inventory.Count; i++)
                {
                    if (inventory[i].Name == name)
                    {
                        template.Add(inventory[i]);
                    }
                } 
                if (template.Count <= 0)
                {
                    throw new InvalidArgumentException("Предмета нету в списке");
                }
                else
                {
                    inventory.Remove(template[0]);
                    return template[0];
                }
            }
        }

    
        public List<GameItem> GetAllItems(string name)
        {
            List<GameItem> x = new List<GameItem>();
            for (int i = 0; i < inventory.Count; i++)
            {
                if (name == inventory[i].Name)
                {
                    x.Add(inventory[i]);
                }
            }

            if (x.Count <= 0)
            {
                throw new InvalidArgumentException("Предметов с таким именем нет");
            }

            for (int z = 0; z < x.Count; z++)
            {
                inventory.Remove(x[z]);
            }

            return x;
        }

        public int GetItemsCount(string name)
        {
            int count = 0;
            for (int i = 0; i < inventory.Count; i++)
            {
                if (name == inventory[i].Name)
                {
                    count++;
                }
            }

            if (count == 0)
            {
                throw new InvalidArgumentException("Такого предмета нету в инвентаре");
            }
            else
            {
                return count;
            }
        }
    }
        
    public class DoubleLegendaryException : Exception
    {
        public DoubleLegendaryException(string message) : base(message) { }
    }

    public class InvalidArgumentException : Exception
    {
        public InvalidArgumentException(string message) : base(message) { }
    }

    private static void Main()
    {
        Inventory invent = new Inventory();

        GameItem gameitem = new GameItem() { Name = "мечклинок" };
        GameItem gameitem3 = new GameItem() { Name = "мечклинок" };
        GameItem leg1 = new GameItem() { Name = "мечклинок3", rarity = Rarity.legendary };
        GameItem leg2 = new GameItem() { Name = "мечклинок3", rarity = Rarity.legendary };

        invent.AddItem(gameitem);
        invent.AddItem(gameitem3);
        invent.AddItem(leg1);

        try
        {
            invent.AddItem(leg2); 
        }
        catch (DoubleLegendaryException ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine(invent.GetItemsCount("мечклинок")); 
}
}