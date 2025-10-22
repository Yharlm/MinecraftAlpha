using MinecraftAlpha;
using System;
using System.Collections.Generic;

namespace MinecraftAlpha
{
    public class EntityAnimationService()
    {

        public List<EntityAnimation> entityAnimations = new List<EntityAnimation>();
        public void LoadAnimations(List<Entity> entityList)
        {
            foreach (Entity entity in entityList)
            {
                entity.Animations = ReturnWhereID(entityList.IndexOf(entity), entity);
            }

        }

        public static List<EntityAnimation> CreateAnimations()
        {
            
            var list = new List<EntityAnimation>
            {
                new EntityAnimation(0,"idle",new List<Frame>()
                {

                    new Frame(1,0,0f,180),
                    new Frame(2,0,0f,0),
                    new Frame(3,0,0f,0),
                    new Frame(4,0,0f,0),
                    new Frame(0,0,0f,0),


                })
                {   duration =0.2f,
                    Looped = false,
                },
                
                new EntityAnimation(0,"Running",new List<Frame>()
                {

                    new Frame(1,0f,2,180+60),
                    new Frame(1,2,2,180-60),
                    new Frame(3,0f,2,-60),
                    new Frame(3,2,2,60),

                    new Frame(4,0f,2,60),
                    new Frame(4,2,2,-60),
                    new Frame(2,0f,2,-60),
                    new Frame(2,2,2,60),


                })
                {
                    duration =4f,
                    Looped = true,
                },
                new EntityAnimation(0,"Swing",new List<Frame>()
                {

                    new Frame(1,0,0.5f,140),
                    new Frame(1,0.5f,0.7f,10),
                    new Frame(1,1,1,180),

                })
                {   duration =3f,
                    Looped = false,
                },



            };

            return list;



        }
        public List<EntityAnimation> ReturnWhereID(int ID,Entity parent)
        {
            List<EntityAnimation> list = new List<EntityAnimation>();
            foreach (var entity in entityAnimations)
            {
                if (entity.ID == ID || ID == -1) // -1 is universal for every mob to be able to play it
                    entity.parent = parent; list.Add(entity);

            }
            return list;
        }
    }
    
}
public class Frame
{
    public Frame(int JointID, float StartPos, float Durration, float DesiredAngle)
    {
        Joint = JointID;
        start = StartPos;
        this.Durration = Durration;
        Angle = DesiredAngle;

    }
    public float start = 3;
    public int Joint = 0;
    public float Angle = 0f;
    public float Durration = 1f;


}
public class EntityAnimation
{
    public bool Playing = false;
    public bool Fliped = false;
    public Entity parent;
    public bool Looped = false;
    public bool Paused = true;
    public string name;
    public int ID = -1;
    public EntityAnimation(int ID, string name, List<Frame> Frames)
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
        if (Paused)
        {
            return;
        }
        Time += 0.1f;
        foreach (var frame in frames)
        {
            
            var Parent = parent.Joints[frame.Joint];
            float Distance = GetDistanceBetweenAngles(Parent.orientation, frame.Angle);
            if (Looped && Time > duration )
            {
                //Playing = false;
                Time = 0f;
            }



            if (frame.Durration + frame.start >= Time && frame.start <= Time)
            {

                Parent.orientation += Distance / frame.Durration/5;
            }

            
        }
    }

    public void ResetAnim()
    {
        var Idle = parent.Animations.Find(x => x.name == "idle");
        foreach (var frame in Idle.frames)
        {
            var joint = parent.Joints[frame.Joint];
            joint.orientation = frame.Angle;
        }
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

