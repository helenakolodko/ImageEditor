using System;
using System.Collections.Generic;

namespace ImageEditor.Model.Tool
{
    public enum ToolType { Drag, Selection, Eyedropper, Pen, Brush, Line, Bucket };
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
            _toolConstructors[ToolType.Selection] = typeof(Selection);
            _toolConstructors[ToolType.Eyedropper] = typeof(Eyedropper);
            _toolConstructors[ToolType.Pen] = typeof(Pencil);
            _toolConstructors[ToolType.Brush] = typeof(Brush);
            _toolConstructors[ToolType.Line] = typeof(Line);
            _toolConstructors[ToolType.Bucket] = typeof(Bucket);
        }
    }
}
