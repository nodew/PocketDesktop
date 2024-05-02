using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PocketClient.Core.Contracts.Services;
using PocketClient.Core.Models;
using PocketClient.Desktop.Contracts.Services;
using PocketClient.Desktop.Contracts.ViewModels;

namespace PocketClient.Desktop.ViewModels;

public partial class AllTagsViewModel : ObservableRecipient, INavigationAware
{
    [ObservableProperty]
    private string searchText;

    public AllTagsViewModel()
    {
        searchText = string.Empty;
    }

    public ObservableCollection<Tag> Tags = new();

    public void OnNavigatedFrom()
    {

    }

    public async void OnNavigatedTo(object parameter)
    {
        await SearchTagsAsync();
    }

    [RelayCommand]
    private async Task SearchTagsAsync()
    {
        var tags = await App.GetService<IPocketDataService>().GetAllTagsAsync();
        var tagNameToSearch = SearchText.Trim().ToLower();

        Tags.Clear();

        tags.Where(tag => tag.Name.ToLower().Contains(tagNameToSearch))
            .OrderBy(tag => tag.Name)
            .ToList()
            .ForEach(Tags.Add);
    }

    [RelayCommand(CanExecute = nameof(CanSelectTag))]
    private void SelectTag(Tag? tag)
    {
        App.GetService<INavigationService>().NavigateTo(typeof(TaggedItemsViewModel).FullName!, tag!.Name);
    }

    private static bool CanSelectTag(Tag? tag) => tag is not null;
}
