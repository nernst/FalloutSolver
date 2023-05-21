using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutSolver.ViewModels
{
    public partial class EntryViewModel : ObservableObject
    {
        [ObservableProperty]
        string entry = string.Empty;

        [ObservableProperty]
        int? count;

        [ObservableProperty]
        bool? isCandidate;

        [ObservableProperty]
        bool hasError = false;
    }
}
