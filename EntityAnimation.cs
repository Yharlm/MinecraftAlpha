using MinecraftAlpha;
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
                entity.Animations = ReturnWhereID(entityList.IndexOf(entity));
            }

        }

        public void CreateAnimations(List<Entity> entityList)
        {
            var mob = entityList;
            entityAnimations = new List<EntityAnimation>
            {

                new EntityAnimation(-1,"Walk",new List<Frame>()
                {
                    new Frame(0,0,1,30),
                    new Frame(0,1,1,-30),

                }),
                new EntityAnimation(-1,"Walk",new List<Frame>()
                {
                    new Frame(0,0,1,30),
                    new Frame(0,1,1,-30),

                }),
            };





        }
        public List<EntityAnimation> ReturnWhereID(int ID)
        {
            List<EntityAnimation> list = new List<EntityAnimation>();
            foreach (var entity in entityAnimations)
            {
                if (entity.ID == ID || ID == -1) // -1 is universal for every mob to be able to play it
                    list.Add(entity);
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
        Time += 0.1f;
        foreach (var frame in frames)
        {

            var Parent = parent.Joints[frame.Joint];
            float Distance = GetDistanceBetweenAngles(Parent.orientation, frame.Angle);
            if (Looped)
            {
                Time = 0f;
            }


            if (frame.Durration >= Time && frame.start <= Time)
            {

                Parent.orientation += Distance / 12;
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

