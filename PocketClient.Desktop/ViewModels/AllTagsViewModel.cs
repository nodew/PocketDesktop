using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PocketClient.Core.Contracts.Services;
using PocketClient.Core.Models;
using PocketClient.Desktop.Contracts.Services;
using PocketClient.Desktop.Contracts.ViewModels;

namespace PocketClient.Desktop.ViewModels;

public class AllTagsViewModel : ObservableRecipient, INavigationAware
{
    private string _searchText;

    public AllTagsViewModel()
    {
        _searchText = string.Empty;

        SearchTagsCommand = new AsyncRelayCommand(SearchTagsAsync);
        SelectTagCommand = new RelayCommand<Tag>(SelectTag, (tag) => tag != null);
    }

    public ObservableCollection<Tag> Tags = new();

    public IAsyncRelayCommand SearchTagsCommand;

    public IRelayCommand SelectTagCommand;

    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public void OnNavigatedFrom()
    {

    }

    public async void OnNavigatedTo(object parameter)
    {
        await SearchTagsAsync();
    }

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

    private void SelectTag(Tag? tag)
    {
        App.GetService<INavigationService>().NavigateTo(typeof(TaggedItemsViewModel).FullName!, tag!.Name);
    }
}
