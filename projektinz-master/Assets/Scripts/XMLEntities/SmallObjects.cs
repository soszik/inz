using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XMlParser
{
    public class SmallObject
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<float[]> bezierPoints { get; set; }
        public int bezierSpeed { get; set; }

        public string vibrating { get; set; }
        public string pulsation { get; set; }
        public string rotate { get; set; }
        
        public float[] vibrationFrequency { get; set; }
        public float[] vibrationAmplitude { get; set; }

        public float[] pulsationFrequency { get; set; }
        public float[] pulsationAmplitudeMax { get; set; }
        public float[] pulsationAmplitudeMin { get; set; }

        public float[] rotationSpeed { get; set; }
        public float[] rotationMax { get; set; }
        public float[] rotationMin { get; set; }
        public float[] rotationDir { get; set; }
        public int Scale { get; set; }
        public int Group { get; set; }


    }
}
