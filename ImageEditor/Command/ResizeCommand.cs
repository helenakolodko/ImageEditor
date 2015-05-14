﻿using System;
using System.Windows.Input;
using ImageEditor.ViewModel;

namespace ImageEditor.Command
{
    public class ResizeCommand : ICommand
    {
        private readonly ImageEditorViewModel _viewModel;

        public ResizeCommand(ImageEditorViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            if (_viewModel.Image != null)
            {
                return true;
            }
            return false;
        }

        public void Execute(object param)
        {
            _viewModel.Image.Resize(_viewModel.ImageWidth, _viewModel.ImageHeight);
        }

        public event EventHandler CanExecuteChanged;

        public void Undo(CommandContext context)
        {
            throw new System.NotImplementedException();
        }

        public void Redo(CommandContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}