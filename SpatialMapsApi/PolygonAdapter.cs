using GeoLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SpatialMaps
{
    public class PolygonAdapter : ISerializable
    {
        public PolygonAdapter() { }
        protected PolygonAdapter(SerializationInfo info, StreamingContext context)
        {
            Points = (List<C2DPoint>)info.GetValue("Points", typeof(List<C2DPoint>));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Points", Points);
        }
        /// <summary>
        /// GetObjectData for serialization.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue("Points", Points);
        }
        public PolygonAdapter(IEnumerable<C2DPoint> points)
        {
            C2DPoly = new C2DPolygon(points.ToList(), true);
        }
        [XmlIgnore]
        public string Name { get; set; }
        [XmlIgnore]
        public C2DPolygon C2DPoly { get; private set; }
        public List<C2DPoint> Points {
            get
            {
                List<C2DPoint> temp = new List<C2DPoint>();
                C2DPoly.GetPointsCopy(temp);
                return temp;
            }
            set
            {
                C2DPoly = new C2DPolygon(value, true);
            }
        }
        public bool ArePointsEqual(PolygonAdapter other)
        {
            var pointsA = Points;
            var pointsB = other.Points;
            if (Enumerable.SequenceEqual(pointsA, pointsB))
                return false;
            return true;
        }
        public bool IsValueEqual(PolygonAdapter other)
        {
            if (!string.Equals(Name, other.Name)) return false;
            return ArePointsEqual(other);
        }

        public static implicit operator C2DPolygon(PolygonAdapter adapter)
        {
            return adapter.C2DPoly;
        }
    }
}
