using FalloutSolver.ViewModels;

namespace FalloutSolver.Views;

public partial class SolverPage : ContentPage
{
	public SolverViewModel ViewModel { get; } = new SolverViewModel();

	public SolverPage()
	{
		InitializeComponent();
	}

    private void OnEntrySwipe(object sender, SwipedEventArgs e)
    {
		EntryViewModel? entry = e.Parameter as EntryViewModel;
		if (entry == null)
			return;

		ViewModel.DeleteEntryCommand.Execute(entry);
    }
}