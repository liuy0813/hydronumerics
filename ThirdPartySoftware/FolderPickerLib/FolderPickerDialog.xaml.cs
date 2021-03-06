﻿using System.Windows;
using System.Windows.Interop;

namespace FolderPickerLib
{
    /// <summary>
    /// Interaction logic for FolderPickerDialog.xaml
    /// </summary>
    public partial class FolderPickerDialog : Window
    {
        public string SelectedPath { get; private set; }

        public FolderPickerDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedPath = FolderPickerControl.SelectedPath;
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (ComponentDispatcher.IsThreadModal)
            {
                DialogResult = false;
            }
            else
            {
                Close();
            }
        }
    }
}
