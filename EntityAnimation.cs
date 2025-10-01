using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace MinecraftAlpha
{
    public class EntityAnimationService()
    {
        
        public List<EntityAnimation> entityAnimations = new List<EntityAnimation>();
        public void LoadAnimations(List<Entity> entityList)
        {
            foreach (Entity entity in entityList)
            {
                entity.Animations = ReturnWhereID(entityList.IndexOf(entity));
            }
            
        }
        public List<EntityAnimation> ReturnWhereID(int ID)
        {
            List < EntityAnimation > list = new List <EntityAnimation>();
            foreach (var entity in entityAnimations) 
            {
                if (entity.ID == ID)
                    list.Add(entity);
            }
            return list;
        }
    }
    public class Frame
    {
        public Frame(Joint joint, float StartPos, float Durration, float DesiredAngle)
        {
            Joint = joint;
            start = StartPos;
            this.Durration = Durration;
            Angle = DesiredAngle;

        }
        public float start = 3;
        public Joint Joint = new Joint();
        public float Angle = 0f;
        public float Durration = 1f;


    }
    public class EntityAnimation
    {
        public bool Paused = true;
        public string name;
        public int ID = -1;
        public EntityAnimation(int ID,string name,List<Frame> Frames)
        {
            this.name = name;
            frames = Frames;
            this.ID = ID;
        }

        public List<Frame> frames = new List<Frame>();
        public float duration = 5f;
        public float Time = 0f;
        public void Update()
        {
            Time += 0.1f;
            foreach (var frame in frames)
            {
                float Distance = GetDistanceBetweenAngles(frame.Joint.orientation, frame.Angle);
                if (frame.Durration >= Time && frame.start <= Time)
                {

                    frame.Joint.orientation += Distance / 12;
                }

            }
        }

        public void Reset()
        {

            Time = 0;
        }

        
        public float GetDistanceBetweenAngles(float start, float end)
        {
            float Distance = end - start;
            if (float.Abs(Distance) > 180)
            {
                Distance = -360;
            }
            return Distance;

        }


    }
}
