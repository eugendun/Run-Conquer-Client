using UnityEngine;

namespace AssemblyCSharp
{
    public class MapModel
    {
        public int Id { get; set; }
        public Vector2 LatLon { get; set; }
        public Vector2 Size { get; set; }
        public int Zoom { get; set; }
    }
}

