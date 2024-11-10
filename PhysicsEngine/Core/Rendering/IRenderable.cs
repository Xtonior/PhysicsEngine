using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysEngine.Core.Rendering
{
    public interface IRenderable
    {
        public void Render(OpenGLRenderer renderer);
    }
}
