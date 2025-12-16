using Microsoft.Xna.Framework;
using MinecraftAlpha;
using System;
using System.Collections.Generic;

namespace MinecraftAlpha
{
    public class AnimateEvent
    {
        public Entity parent;
        public int id;
        
    }

    public class EntityAnimationService()
    {

        public List<AnimateEvent> entityAnimations = new List<AnimateEvent>();

        public void Stop(int ID, Entity parent)
        {
            var AnimEv = new AnimateEvent()
            {
                parent = parent,
                id = ID

            };
            
            if (!entityAnimations.Contains(AnimEv))
            {
                entityAnimations.Remove(AnimEv);
            }




        }
        public void Play(int ID,Entity parent)
        {
            
            var AnimEv = new AnimateEvent()
            {
                parent = parent,
                id = ID

            };
            parent.Animations[AnimEv.id].Time = 0;
            parent.Animations[AnimEv.id].Paused = false;
            if (!entityAnimations.Contains(AnimEv))
            {
                entityAnimations.Add(AnimEv);
            }
                
            


        }
        public static List<EntityAnimation> LoadAnimations()
        {
            var
            entityAnimations = new List<EntityAnimation>
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
                    Looped = false,
                },
                new EntityAnimation(0,"Swing",new List<Frame>()
                {

                    new Frame(1,0,0.5f,140,true),
                    new Frame(1,0.5f,0.7f,10,true),
                    new Frame(1,1,1,180),

                })
                {   duration =3f,
                    Looped = false,
                },



            };
            


            return entityAnimations;



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
    public Frame(int JointID, float StartPos, float Durration, float DesiredAngle,bool Flip)
    {
        Joint = JointID;
        start = StartPos;
        this.Durration = Durration;
        Angle = DesiredAngle;
        this.Flip = Flip;

    }

    public float start = 3;
    public int Joint = 0;
    public float Angle = 0f;
    public bool Flip = false;
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

            float Angle = frame.Angle;
            if(parent.Fliped)
            {
                if (frame.Flip)
                {
                    Angle = frame.Angle + 180;
                }
                
            }

            float Distance = GetDistanceBetweenAngles(Parent.orientation, Angle);
            if (Looped)
            {
                Time = 0f;

            }
            if (Time >= duration)
            {
                ResetAnim();
                Paused = true ;
                return;

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
        return Distance%360;

    }

    public static List<EntityAnimation> LoadAnimation(Entity parent, List<EntityAnimation> Newlist)
    {
        int id = parent.ID;
        var animations = new List<EntityAnimation>();

        foreach (var anim in Newlist)
        {
            if(anim.ID == id)
            {
                var newAnim = new EntityAnimation(anim.ID, anim.name, anim.frames);
                newAnim.parent = parent;
                animations.Add(newAnim);
            }
        }
        return animations;
    }


}

