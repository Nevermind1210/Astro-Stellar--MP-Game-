using Mirror;
using UnityEngine;

namespace Network_Learning.Scripts.Networking
{
    public static class ColorReadWriter 
    {
        public static void WriteColor(this NetworkWriter _writer, Color _color)
        {
            _writer.WriteFloat(_color.r);
            _writer.WriteFloat(_color.g);
            _writer.WriteFloat(_color.b);
            _writer.WriteFloat(_color.a);
        }

        public static Color ReadColor(this NetworkReader _reader)
        {
            Color color = new Color()
            {
                r = _reader.ReadFloat(),
                g = _reader.ReadFloat(),
                b = _reader.ReadFloat(),
                a = _reader.ReadFloat(),
            };

            return color;
        }
        
    }
}