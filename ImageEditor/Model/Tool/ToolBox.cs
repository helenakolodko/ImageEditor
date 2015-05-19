using System.Collections.Generic;
using ImageEditor.ViewModel;

namespace ImageEditor.Model.Tool
{
    class ToolBox
    {
        private ToolFactory _factory;
        private Dictionary<ToolType, Tool> _tools;
        private ImageEditorViewModel _viewModel;

        public Tool GetTool(ToolType type)
        {
            Tool result;
            _tools.TryGetValue(type, out result);
            if (result == null)
            {
                result = _factory.GetTool(type, new object[]{_viewModel});
                _tools[type] = result;
            }
            return result;
        }

        public ToolBox(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
            _factory = new ToolFactory();
            _tools = new Dictionary<ToolType, Tool>();
        }
    }
}
