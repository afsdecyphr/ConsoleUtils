using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Serialization
{
    [Serializable]
    public struct GameRequest
    {
        public Player Player { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public bool Running { get; set; }
        public bool IsNewMap { get; set; }
        public string MapName { get; set; }
        public int MapNumber { get; set; }
        public char[,] Map { get; set; }
    };

    [Serializable]
    public enum PlayerID
    {
        Player1 = 1,
        Player2 = 2
    }

    [Serializable]
    public struct Player
    {
        public PlayerID PlayerID { get; set; }
        public char Character { get; set; }
        public int y { get; set; }
        public int x { get; set; }
    }

    public static class Serializer
    {
        public static byte[] ToByteArray<T>(this T graph)
        {
            using (var ms = new MemoryStream())
            {
                graph.Serialize(ms);

                return ms.ToArray();
            }
        }

        public static T FromByteArray<T>(this byte[] serialized)
        {
            using (var ms = new MemoryStream(serialized))
            {
                return ms.DeSerialize<T>();
            }
        }

        public static void Serialize<T>(this T graph, Stream target)
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Binder = TypeOnlyBinder.Default;
            formatter.Serialize(target, graph);
        }

        public static T DeSerialize<T>(this Stream source)
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Binder = TypeOnlyBinder.Default;
            return (T)formatter.Deserialize(source);
        }

        public class TypeOnlyBinder : SerializationBinder
        {
            private static SerializationBinder defaultBinder = new BinaryFormatter().Binder;

            public override Type BindToType(string assemblyName, string typeName)
            {
                if (assemblyName.Equals("NA"))
                    return Type.GetType(typeName);
                else
                    return defaultBinder.BindToType(assemblyName, typeName);
            }

            public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
            {
                assemblyName = "NA";
                typeName = serializedType.FullName;
            }

            private static object locker = new object();
            private static TypeOnlyBinder _default = null;

            public static TypeOnlyBinder Default
            {
                get
                {
                    lock (locker)
                    {
                        if (_default == null)
                            _default = new TypeOnlyBinder();
                    }
                    return _default;
                }
            }
        }
    }

    internal class MapLoader
    {
        private String _PATH;
        private char[,] _MAP;
        private string[] _MAPARRAY;
        private List<string> _LEVELSLIST = new List<string>();

        public MapLoader()
        {
            _MAP = new char[15, 35];
        }

        public string[] GetMapArray(string levelName)
        {
            _PATH = "maps/" + levelName + ".txt";
            string[] _TEXT = File.ReadAllLines(_PATH);
            return _TEXT;
        }

        public char[,] GetMapCharArray(string levelName)
        {
            _MAP = new char[15, 35];
            _PATH = "maps/" + levelName + ".txt";
            string[] _TEXT = GetMapArray(levelName);
            for (int i = 0; i <= _TEXT.Length - 1; i++)
            {
                for (int j = 0; j <= _TEXT[i].Length - 1; j++)
                {
                    _MAP[i, j] = Convert.ToChar(_TEXT[i][j]);
                }
            }
            return _MAP;
        }

        public List<string> Levels()
        {
            foreach (string name in Directory.GetFiles(@"maps\", "*.txt"))
            {
                FileInfo fileInfo = new FileInfo(name);
                _LEVELSLIST.Add(fileInfo.Name);
            }
            return _LEVELSLIST;
        }
    }
}