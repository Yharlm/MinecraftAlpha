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

                new Event("Rot","", () => { Game._entityManager.Workspace[0].Animations[1].Time = 0f; }),

            };
            return list;
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
