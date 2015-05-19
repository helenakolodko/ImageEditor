using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Hosting;
using ImageEditor.ViewModel;

namespace ImageEditor.Model.Tool
{
    public enum ToolType { Drag, Selection, Eyedropper, Pencil, Brush, Line, Bucket };
    class ToolFactory
    {
        private Dictionary<ToolType, Type> _toolConstructors;
        public Tool GetTool(ToolType type, object[] arguments)
        {
            Type toolType = _toolConstructors[type];
            var argumentTypes = GetArgumentTypes(arguments);
            var constructor = toolType.GetConstructor(argumentTypes);
            if (constructor != null)
            {
                return (Tool) constructor.Invoke(arguments);
            }
            return null;
        }

        private static Type[] GetArgumentTypes(object[] arguments)
        {
            Type[] argumentTypes = new Type[arguments.Length];
            for (int i = 0; i < arguments.Length; i++)
            {
                argumentTypes[i] = arguments[i].GetType();
            }
            return argumentTypes;
        }

        public ToolFactory()
        {
            _toolConstructors = new Dictionary<ToolType, Type>();
            _toolConstructors[ToolType.Drag] = typeof(Drag);
            _toolConstructors[ToolType.Selection] = typeof(Selection);
            _toolConstructors[ToolType.Eyedropper] = typeof(Eyedropper);
            _toolConstructors[ToolType.Pencil] = typeof(Pencil);
            _toolConstructors[ToolType.Brush] = typeof(Brush);
            _toolConstructors[ToolType.Line] = typeof(Line);
            _toolConstructors[ToolType.Bucket] = typeof(Bucket);
        }
    }
}
