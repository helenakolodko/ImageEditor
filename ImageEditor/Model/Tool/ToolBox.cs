using System;
using System.Collections.Generic;

namespace ImageEditor.Model.Tool
{
    class ToolBox
    {
        ToolFactory _factory;
        Dictionary<ToolType, Tool> _tools;

        public Tool GetTool(ToolType type)
        {
            Tool result;
            try
            {
                result = _tools[type];
            }
            catch(Exception e)
            {
                result = _factory.GetTool(type);
                _tools[type] = result;
            }
            return result;
        }
    }
}
