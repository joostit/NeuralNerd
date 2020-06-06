using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Joostit.NeuralNerd.NnLib.Configuration
{
    public class ConfigurationFileHandler
    {


        public void Save(NetworkConfiguration network, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(NetworkConfiguration));

            using (TextWriter tw = new StreamWriter(path, false))
            {
                serializer.Serialize(tw, network);
            }
        }



        public NetworkConfiguration Load(string path)
        {
            NetworkConfiguration retVal = null;
            XmlSerializer serializer = new XmlSerializer(typeof(NetworkConfiguration));

            using (TextReader reader = new StreamReader(path))
            {
                retVal = (NetworkConfiguration)serializer.Deserialize(reader);
            }
            return retVal;
        }

    }
}
