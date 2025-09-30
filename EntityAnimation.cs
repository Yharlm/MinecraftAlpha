using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftAlpha
{
    public class Frame
    {
        public float start = 3;
        public Joint Joint = new Joint();
        public float Angle = 0f;
        public float Durration = 1f;


    }
    public class EntityAnimation
    {
        
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
                    
                    frame.Joint.orientation += Distance/12;
                }   
                
            }
        }

        public void Reset()
        {

            Time = 0;
        }
        public float GetDistanceBetweenAngles(float start,float end)
        {
            float Distance = end - start;
            if (float.Abs(Distance) > 180)
            {
                Distance =- 360;
            }
            return Distance;

        }
        
        
    }
}
