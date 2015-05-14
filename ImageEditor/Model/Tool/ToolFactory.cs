using System;
using System.Collections.Generic;
using System.Reflection;

namespace ImageEditor.Model.Tool
{
    enum ToolType { Drag, SelectRect, Eyedropper, Pen, Brush, Line, Bucket };
    class ToolFactory
    {
        private Dictionary<ToolType, Type> _toolConstructors;
        public Tool GetTool(ToolType type)
        {
            Type toolType = _toolConstructors[type];
            var constructor = toolType.GetConstructor(Type.EmptyTypes);
            if (constructor != null)
            {
                return (Tool) constructor.Invoke(null);
            }
            else
            {
                return null;
            }

        }

        public ToolFactory()
        {
            _toolConstructors = new Dictionary<ToolType, Type>();
            _toolConstructors[ToolType.Drag] = typeof(Drag);
            _toolConstructors[ToolType.SelectRect] = typeof(SelectRectangle);
            _toolConstructors[ToolType.Eyedropper] = typeof(Eyedropper);
            _toolConstructors[ToolType.Pen] = typeof(Pen);
            _toolConstructors[ToolType.Brush] = typeof(Brush);
            _toolConstructors[ToolType.Line] = typeof(Line);
            _toolConstructors[ToolType.Bucket] = typeof(Bucket);
        }
    }
}
