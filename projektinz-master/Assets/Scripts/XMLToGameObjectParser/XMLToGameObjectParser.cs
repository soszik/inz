using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XMlParser;


namespace Assets.Scripts.XMLToGameObjectParser
{
    public static class XMLToGameObjectParser
    {
        private static Scene scn;

        public static void XMLToGameObjects(Scene scene, ref List<GameObject> gameObjectsFromScene,
           ref List<Vector3> puzzlesPlacements, ref List<AudioClip> AudioItems, ref List<GameObject> SmallObjects)
        {
            scn = scene;

            parseScene(scene, ref gameObjectsFromScene, ref puzzlesPlacements, ref AudioItems, ref SmallObjects);
        }

        private static void parseScene(Scene scene, ref List<GameObject> Puzzles,
           ref List<Vector3> puzzlesPlacements, ref List<AudioClip> AudioItems, 
           ref List<GameObject> SmallObjects)
        {
            foreach (var audio in scene.AudioItems)
            {
                AudioClip audioItem = UnityEngine.Object.Instantiate(
                    Resources.Load(audio.Path) as AudioClip);
                audioItem.name = audio.Id.ToString();
                AudioItems.Add(audioItem);
            }
            for (int i = Puzzles.Count - 1; i >= 0; i--)
            {
                bool isToSet = false;
               
                foreach (var puzzleToSet in scene.Puzzles)
                {
                    if (puzzleToSet.Name.Equals(Puzzles.ElementAt(i).name))
                    {
                        isToSet = true;
                    }                  
                }
                if (!isToSet)
                {
                    Puzzles.RemoveAt(i);
                }
            }

            for (int i = SmallObjects.Count - 1; i >= 0; i--)
            {
                foreach (var smallObjToSet in scene.SmallObjects)
                {
                    if (smallObjToSet.Name.Equals(SmallObjects.ElementAt(i).name))
                    {
                        SmallObjects.ElementAt(i).transform.position = new Vector3(smallObjToSet.bezierPoints.ElementAt(0)[0],
                            (float)smallObjToSet.bezierPoints.ElementAt(0)[1], (float)smallObjToSet.bezierPoints.ElementAt(0)[2]);
                        
                        GameObject nowy = (GameObject)Network.Instantiate(SmallObjects.ElementAt(i), SmallObjects.ElementAt(i).transform.position,
                            SmallObjects.ElementAt(i).transform.rotation, 1);
                        var comp = nowy.GetComponent<FlyingObjectScript>();
                        setFlyingScriptProperties(ref comp, smallObjToSet);
                    }
                }
            }
        }
       

        private static void setFlyingScriptProperties(ref FlyingObjectScript comp, SmallObject smallObject)
        {
            comp.bezierPoints = getBezierPoints(smallObject.bezierPoints);
            comp.bezierSpeed = smallObject.bezierSpeed;
            comp.pulsation = Boolean.Parse(smallObject.pulsation);
            if (comp.pulsation)
            {
                comp.pulsationAmplitudeMax = floatArrayToVector(smallObject.pulsationAmplitudeMax);
                comp.pulsationAmplitudeMin = floatArrayToVector(smallObject.pulsationAmplitudeMin);
                comp.pulsationFrequency = floatArrayToVector(smallObject.pulsationFrequency);
            }
            comp.rotate = Boolean.Parse(smallObject.rotate);
            if (comp.rotate)
            {
                comp.rotationDir = floatArrayToVector(smallObject.rotationDir);
                comp.rotationMax = floatArrayToVector(smallObject.rotationMax);
                comp.rotationMin = floatArrayToVector(smallObject.rotationMin);
                comp.rotationSpeed = floatArrayToVector(smallObject.rotationSpeed);
            }

            comp.vibrating = Boolean.Parse(smallObject.vibrating);
            if (comp.vibrating)
            {
                comp.vibrationAmplitude = floatArrayToVector(smallObject.vibrationAmplitude);
                comp.vibrationFrequency = floatArrayToVector(smallObject.vibrationFrequency);
            }
            comp.ChangeGroupNumber(smallObject.Group);
        }

        private static Vector3 floatArrayToVector(float[] toPrase)
        {
            Vector3 vector = new Vector3(toPrase[0], toPrase[1], toPrase[2]);
            
            return vector;
        }

        private static List<Vector3> getBezierPoints(List<float[]> bezierPointsToParse)
        {
            List<Vector3> bezierPoints = new List<Vector3>();

            foreach (var points in bezierPointsToParse)
            {
                bezierPoints.Add(new Vector3(points[0], points[1], points[2]));
            }

            return bezierPoints;
        }




    }
}
