using DotNetWeeklyClient.ViewModels;
using System.Runtime.CompilerServices;

namespace DotNetWeeklyClient;

public partial class MainPage : ContentPage
{
	private MainViewModel viewModel;
	public MainPage(MainViewModel vm)
	{
		InitializeComponent();
		viewModel = vm;
		BindingContext = vm;
	}

	protected override async void OnAppearing()
	{
		await viewModel.Init();
	}
}

