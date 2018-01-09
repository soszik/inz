using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;


namespace XMlParser
{
    public static class XmlLoader
    {
        //TODO List of gameobjects
        public static Scene LoadGameObjectsFromFile(String path)
        {
            //List<Scene> gameObjects = new List<Scene>();
            XDocument doc = XDocument.Load(path);
            Scene gameObjects = parseFromFile(doc);
           
            return gameObjects;
        }

        private static Scene parseFromFile(XDocument doc)
        {
            IEnumerable<Scene> result = from s in doc.Descendants("scene")
                                        select new Scene()
                                        {
                                            Type = (string)s.Attribute("type"),
                                            Name = (string)s.Attribute("name"),
                                            X = (int)s.Attribute("x"),
                                            Y = (int)s.Attribute("y"),
                                            Z = (int)s.Attribute("z"),
                                            PuzzleSize = float.Parse((string)s.Attribute("size")),
                                            GroupCount = (int)s.Attribute("groupCount"),
                                            AudioItems = s.Descendants("audio").Select(audio => new Audio()
                                            {
                                                  Id = (string)audio.Attribute("id"),
                                                  Path = (String)audio.Attribute("path"),
                                            }).ToList(),
                                            Puzzles = s.Descendants("puzzle").Select(p => new Puzzle()
                                            {                                           
                                                Name = (string)p.Attribute("name"),                                             
                                            }).ToList(),
                                           SmallObjects = s.Descendants("smallObject").Select(smallObject => new SmallObject()
                                           {
                                               Scale = (int)smallObject.Attribute("scale"),
                                               Name = (string)smallObject.Attribute("name"),
                                               Type = (string)smallObject.Attribute("type"),
                                               rotate = (string)smallObject.Attribute("rotate"),
                                               Group = (int)smallObject.Attribute("group"),
                                               pulsation = (string)smallObject.Attribute("pulsation"),
                                               vibrating = (string)smallObject.Attribute("vibrating"),
                                               pulsationFrequency = xmlStringToFloatArray((string)smallObject.Attribute("pulsationFrequency"), 3),
                                               pulsationAmplitudeMax = xmlStringToFloatArray((string)smallObject.Attribute("pulsationAmplitudeMax"), 3),
                                               pulsationAmplitudeMin = xmlStringToFloatArray((string)smallObject.Attribute("pulsationAmplitudeMin"), 3),
                                               rotationSpeed = xmlStringToFloatArray((string)smallObject.Attribute("rotationSpeed"), 3),
                                               rotationMax = xmlStringToFloatArray((string)smallObject.Attribute("rotationMax"), 3),
                                               rotationMin = xmlStringToFloatArray((string)smallObject.Attribute("rotationMin"), 3),
                                               bezierSpeed = (int)smallObject.Attribute("bezierSpeed"),
											   bezierPoints = smallObject.Descendants("bezierPoint").Select(bezpierPoint =>
                                               new float[] { float.Parse((string)bezpierPoint.Attribute("x")),
                                                                        float.Parse((string)bezpierPoint.Attribute("y")),
                                                                        float.Parse((string)bezpierPoint.Attribute("z"))}

                                                        ).ToList(),
                                           }).ToList(),
                                        };
            return result.ToList<Scene>().ElementAt(0);
        }
        //later parsed to Vertex3 in XMLToGameObjectParser
        private static float[] xmlStringToFloatArray(string toParse, int length)
        {
            float[] array = new float[length];

            array = Array.ConvertAll(toParse.Split(','), float.Parse);

            return array;
        }
    }
}
