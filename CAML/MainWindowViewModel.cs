using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CAML
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string inputCode = "1*2-3";

        [ObservableProperty]
        private string stdOut = "";

        [ObservableProperty]
        private string stdIn = "";

        [RelayCommand]
        private void RunCode()
        {
            StdOut += InputCode + Environment.NewLine;
        }

        [RelayCommand]
        private void StdInSend()
        {
            StdOut += StdIn + Environment.NewLine;
        }
    }
}
