using System;
using System.Collections.Generic;

namespace MinecraftAlpha
{
    public class Event
    {



        public string Name = "New Event";
        public string Description = "This is a new event";
        public Action Action;

        public Event(string name, string description, Action action)
        {
            Name = name;
            Description = description;
            Action = action;
        }
    }
    public class ActionManager
    {
        public Game1 Game;
        public List<Event> Actions = new List<Event>();
        public ActionManager() { Actions = LoadActions(); }

        public List<Event> LoadActions()
        {
            var list = new List<Event>()
            {

                //Kills all entities
                new Event("KIll","", () => { Game.Entities.Clear(); }),
                // 
                new Event("LeftInteract","",() => {  })
                

            };
            return list;
        }

        public bool CheckAround(int x,int y, TileGrid[,] Map)
        {
            bool Placable = true;
            if (Map[y, x].ID == 0) Placable = false;
            if (Map[y, x+1].ID == 0) Placable = false;
            if (Map[y, x-1].ID == 0) Placable = false;
            if (Map[y+1, x].ID == 0) Placable = false;
            if (Map[y-1, x].ID == 0) Placable = false;
            return Placable;
        }
        public void PlaceBlock(int X, int Y)
        {
            foreach(var Grid in Game.Layers)
            {
                if(!CheckAround(X,Y,Grid)) continue;
                var ItemSelected = Game._userInterfaceManager.selectedItem;
                if (ItemSelected != null && Game._userInterfaceManager.amount > 0 && Grid[Y, X].ID == 0)
                {
                    var BlockId = Game._blockManager.Blocks.IndexOf(ItemSelected); // Set to air

                    Grid[Y, X].ID = BlockId;
                    Game._userInterfaceManager.amount -= 1;
                }
                if (Game._userInterfaceManager.amount <= 0)
                {
                    Game._userInterfaceManager.selectedItem = null;

                }
            }
            
        }

        public void BreakBlock(int X, int Y)
        {var block = Game._blockManager.Blocks[Game.World[Y, X].ID];

            if (Game.World[Y, X].ID != 0)
            {
                
                foreach (var islot in Game._userInterfaceManager.windows[0].ItemsSlots)
                {
                    if (islot.Item != null && islot.Item != Game._blockManager.Blocks[Game.World[Y, X].ID])
                    { continue; }
                    else
                    {
                        islot.Item = Game._blockManager.Blocks[Game.World[Y, X].ID];


                        islot.Count += 1; break;


                    }
                }
               
                if(block.Health > Game.World[Y, X].MinedHealth)
                {
                    Game.World[Y, X].MinedHealth += 0.1f;
                    return;
                }
                Game.World[Y, X].ID = 0;

            }

        }
        public void GetAction(string action)
        {
            foreach (var act in Actions)
            {
                if (act.Name == action)
                {
                    act.Action.Invoke();
                }
            }
        }
    }
}
