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


                new Event("KIll","", () => { Game.Entities.Clear(); }),

                //new Event("Rot","", () => { Game._entityManager.Workspace[0].Animations[1].Time = 0f; }),

            };
            return list;
        }

        public void PlaceBlock(int X, int Y)
        {
            var ItemSelected = Game._userInterfaceManager.selectedItem;
            if (ItemSelected != null && Game._userInterfaceManager.amount > 0 && Game.World[Y, X] == 0)
            {
                var BlockId = Game._blockManager.Blocks.IndexOf(ItemSelected); // Set to air

                Game.World[Y, X] = BlockId;
                Game._userInterfaceManager.amount -= 1;
            }
            if (Game._userInterfaceManager.amount <= 0)
            {
                Game._userInterfaceManager.selectedItem = null;

            }
        }

        public void BreakBlock(int X, int Y)
        {

            if (Game.World[Y, X] != 0)
            {

                foreach (var islot in Game._userInterfaceManager.windows[0].ItemsSlots)
                {
                    if (islot.Item != null && islot.Item != Game._blockManager.Blocks[Game.World[Y, X]])
                    { continue; }
                    else
                    {
                        islot.Item = Game._blockManager.Blocks[Game.World[Y, X]];


                        islot.Count += 1; break;


                    }
                }
                Game.World[Y, X] = 0;

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
