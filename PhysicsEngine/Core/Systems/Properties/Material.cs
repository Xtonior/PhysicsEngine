using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysEngine.Core.Systems.Properties
{
    public class Material
    {
        public Vector3 Color;

        public Material(Vector3 color)
        {
            Color = color;
        }

    }
}
