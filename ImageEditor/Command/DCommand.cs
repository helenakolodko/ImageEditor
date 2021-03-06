﻿using ImageEditor.ViewModel;
using ImageProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageEditor.Command
{
    public class DCommand : IReversableCommand, ICommand
    {
        private readonly ImageEditorViewModel _viewModel;
        public DCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public void Execute(object parameter)
        {
            _viewModel.ComandList.AddNew(_viewModel.Image.Source);
            _viewModel.Image.Source = Curves.ProcessD(_viewModel.Image.Source, _viewModel.G, _viewModel.F, _viewModel.SelectedRegion);
            _viewModel.RefreshImage();
            _viewModel.OnCommandExecuted();
        }

        public void Undo(CommandContext context)
        {
            throw new NotImplementedException();
        }

        public void Redo(CommandContext context)
        {
            throw new NotImplementedException();
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            return _viewModel.Image != null;
        }

        public event EventHandler CanExecuteChanged;
    }
}
