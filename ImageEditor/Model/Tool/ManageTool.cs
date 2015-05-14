using System.Windows;

namespace ImageEditor.Model.Tool
{
    abstract class ManageTool : Tool
    {
         public override void MouseDown(Point position)
         {
             throw new System.NotImplementedException();
         }

         public override void MouseMove(Point position)
         {
             throw new System.NotImplementedException();
         }

         public override void MouseUp(Point position)
         {
             throw new System.NotImplementedException();
         }
    }
}
