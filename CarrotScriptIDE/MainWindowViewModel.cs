using CarrotScript.Impl;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarrotScript
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string inputCode = "2**3\t+ 7.9E2+7*E6/-.32+(1+2*_x_)*-5";

        [ObservableProperty]
        private string stdOut = "";

        [ObservableProperty]
        private string stdIn = "";

        [RelayCommand]
        private void RunCode()
        {
            Lexar lexar = new(inputCode);
            lexar.Parse();
            StdOut += InputCode + Environment.NewLine;
            StdOut += "{" + Environment.NewLine;
            foreach (var t in lexar.Tokens)
            {
                StdOut += "\t" + t.ToString() + Environment.NewLine;
            }
            StdOut += "}" + Environment.NewLine;
        }

        [RelayCommand]
        private void StdInSend()
        {
            StdOut += StdIn + Environment.NewLine;
        }
    }
}
