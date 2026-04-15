using MinecraftAlpha;
using System.Collections.Generic;
using System.Numerics;

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




        }//by its index by removing it form active
        public void Play(int ID, Entity parent)
        {
            if (parent.ID <= -1 || ID > parent.Animations.Count - 1) return; // Lol no ur an object
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




        }//by its index by Adding it to active
        public static EntityAnimation GetAnimation(string name, int ID)// litteraly overcomplicated Find() 
        {
            foreach (var anim in LoadAnimations())
            {
                if (anim.name == name && ID == anim.ID)
                {
                    return anim;
                }
            }
            return null;
        }
        public static List<EntityAnimation> LoadAnimations()
        {
            var
            entityAnimations = new List<EntityAnimation>
            {
                new EntityAnimation(0,"idle",new List<Frame>()
                {

                    new Frame(1,0,0f,0),
                    new Frame(2,0,0f,0),
                    new Frame(3,0,0f,0),
                    new Frame(4,0,0f,0),
                    new Frame(0,0,0f,0),


                })
                {   duration =0.2f,
                    Looped = false,
                },
                new EntityAnimation(1,"idle_zombie",new List<Frame>()
                {

                    //new Frame(1,0,0f,90),
                    new Frame(2,0,0f,0),
                    //new Frame(3,0,0f,90),
                    new Frame(4,0,0f,0),
                    new Frame(0,0,0f,0),


                })
                {   duration =1f,
                    Looped = false,
                },

                new EntityAnimation(0,"Running",new List<Frame>()
                {

                    new Frame(1,0f,2,+60),
                    new Frame(1,2,2,-60),
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
                new EntityAnimation(1,"Running_zombie",new List<Frame>()
                {

                    new Frame(1,0f,2,-90,true),
                    new Frame(3,0,2,-90,true),

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

                    new Frame(1,0,0.5f,-120,true),
                    new Frame(1,0.5f,0.7f,-30,true),
                    new Frame(1,1,1,0),

                })
                {   duration =1f,
                    Looped = false,
                },
                new EntityAnimation(2,"idle",new List<Frame>()
                {

                    new Frame(0,0f,0,0),
                    new Frame(1,0f,0,0),
                    new Frame(2,0f,0,0),
                    new Frame(3,0f,0,0),

                })
                {   duration =0.2f,
                    Looped = false,
                },

                new EntityAnimation(2,"pig_walk",new List<Frame>()
                {


                    //new Frame(1,0f,1,0),
                    new Frame(1,1f,1,50),
                    new Frame(1,2f,1f,-50),
                    //new Frame(1,3f,1,0),

                    //new Frame(2,0f,1,0),
                    new Frame(2,1f,1,-50),
                    new Frame(2,2f,1f,50),
                    //new Frame(2,3f,1,0),

                    //new Frame(3,0f,1,0),
                    new Frame(3,1f,1,50),
                    new Frame(3,2f,1f,-50),
                    //new Frame(3,3f,1,0),

                    //new Frame(0,0f,1,0),
                    new Frame(0,1f,1,-50),
                    new Frame(0,2f,1f,50),
                    //new Frame(0,3f,1,0),





                })
                {   duration =4f,
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
    public Frame(int JointID, float StartPos, float Durration, float DesiredAngle, bool Flip)
    {
        Joint = JointID;
        start = StartPos;
        this.Durration = Durration;
        Angle = DesiredAngle;
        this.Flip = Flip;

    }
    public Vector3 Position; // First 2 are for x y movement, 3 & 4 for fake rotation of z axis - LERP
    public float start = 3; // When the frame should start, in seconds
    public int Joint = 0; //Joint ID
    public float Angle = 0f; // Desired angle for the joint to be at when the frame is done
    public bool Flip = false; // fliped means angle fliped
    public float Durration = 1f; // How long the frame should take to complete


}
public class EntityAnimation
{

    public bool Playing = false;
    public bool Fliped = false;
    public Entity parent;
    public bool Looped = false;
    public bool Paused = false;
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
        if (Paused) return;

        Time += 0.1f;


        if (Time >= duration)
        {
            if (Looped)
            {
                Time = 0f;
            }
            else
            {
                ResetAnim();
                Paused = true;
                return;
            }
        }

        foreach (var frame in frames)
        {
            var Parent = parent.Joints[frame.Joint];

            float targetAngle = frame.Angle;

            if (parent.Fliped && frame.Flip)
            {
                targetAngle += 180f;
            }

            //current time is inside this frame
            if (frame.start <= Time && Time <= frame.start + frame.Durration)
            {
                float delta = GetDistanceBetweenAngles(Parent.orientation, targetAngle); //Delta


                Parent.orientation += delta / frame.Durration / 5;
            }
        }
    }

    public void ResetAnim()
    {

        var Idle = parent.Animations.Find(x => x.name.Contains("idle"));
        foreach (var frame in Idle.frames)
        {
            var joint = parent.Joints[frame.Joint];
            joint.orientation = frame.Angle;
        }
    }


    
    public float GetDistanceBetweenAngles(float start, float end)
    {
       
        float delta = (end - start) % 360f;

        if (delta > 180f)
            delta -= 360f;
        else if (delta < -180f)
            delta += 360f;

        return delta;

    }

    public static List<EntityAnimation> LoadAnimation(Entity parent, List<EntityAnimation> Newlist)
    {
        int id = parent.ID;
        var animations = new List<EntityAnimation>();

        foreach (var anim in Newlist)
        {
            
                var newAnim = new EntityAnimation(anim.ID, anim.name, anim.frames);
                newAnim.parent = parent;
                animations.Add(newAnim);
            
        }
        return animations;
    }


}

