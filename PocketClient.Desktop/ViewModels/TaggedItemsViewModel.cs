using CommunityToolkit.Mvvm.Input;
using PocketClient.Core.Contracts.Services;
using PocketClient.Core.Models;
using PocketClient.Core.Specifications;
using PocketClient.Desktop.Contracts.Services;
using PocketClient.Desktop.Helpers;
using PocketClient.Desktop.Models;

namespace PocketClient.Desktop.ViewModels;

public class TaggedItemsViewModel : ItemsViewModel
{
    private Tag? _currentTag;

    public TaggedItemsViewModel() : base()
    {
        PinTagCommand = new AsyncRelayCommand(PinTagAsync);
        UnPinTagCommand = new AsyncRelayCommand(UnPinTagAsync);
        RenameTagCommand = new AsyncRelayCommand<string>(RenameTagAsync);
        DeleteTagCommand = new AsyncRelayCommand(DeleteTagAsync);
    }

    public Tag? CurrentTag
    {
        get => _currentTag;
        set => SetProperty(ref _currentTag, value);
    }

    public IAsyncRelayCommand PinTagCommand { get; }

    public IAsyncRelayCommand UnPinTagCommand { get; }

    public IAsyncRelayCommand RenameTagCommand { get; }

    public IAsyncRelayCommand DeleteTagCommand { get; }

    protected override BaseSpecification<PocketItem> BuildFilter()
    {
        var filter = base.BuildFilter();

        if (FilterOption == PocketItemFilterOption.All)
        {
            filter.SetFilterCondition(item => item.Tags.Contains(CurrentTag!));
        }
        else if (FilterOption == PocketItemFilterOption.UnArchived)
        {
            filter.SetFilterCondition(item => item.Tags.Contains(CurrentTag!) && item.IsArchived == false);
        }
        else if (FilterOption == PocketItemFilterOption.Archived)
        {
            filter.SetFilterCondition(item => item.Tags.Contains(CurrentTag!) && item.IsArchived == true);
        }
        else if (FilterOption == PocketItemFilterOption.Favorited)
        {
            filter.SetFilterCondition(item => item.Tags.Contains(CurrentTag!) && item.IsFavorited == true);
        }
        
        return filter;
    }

    protected async override Task NavigatedTo(object parameter)
    {
        var tagName = (string)parameter;

        var tag = await App.GetService<IPocketDataService>().GetTagByNameAsync(tagName);
        var pinnedTags = await App.GetService<ILocalSettingsService>().ReadSettingAsync<List<string>>("Tags");

        if (tag != null)
        {
            CurrentTag = tag;
        }
        else
        {
            CurrentTag = new Tag() { Name = tagName }; 
        }

        CurrentTag.IsPinned = pinnedTags != null && pinnedTags.Contains(tagName);
        
        await base.NavigatedTo(parameter);
    }

    private async Task PinTagAsync()
    {
        CurrentTag!.IsPinned = true;
        await App.GetService<INavigationViewService>().PinTagAsync(CurrentTag!);
        OnPropertyChanged(nameof(CurrentTag));
    }

    private async Task UnPinTagAsync()
    {
        CurrentTag!.IsPinned = false;
        await App.GetService<INavigationViewService>().RemovePinnedTagAsync(CurrentTag!);
        OnPropertyChanged(nameof(CurrentTag));
    }

    private async Task RenameTagAsync(string? newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            return;
        }

        newName = newName.Trim().ToLower();

        if (newName == CurrentTag!.Name)
        {
            return;
        }

        var tag = await App.GetService<IPocketDataService>().GetTagByNameAsync(newName);

        if (tag != null)
        {
            await App.MainWindow.ShowMessageDialogAsync("TagExisted_ErrorMessage".Format(tag.Name));
            return;
        }

        try
        {
            await App.GetService<IPocketDataService>().RenameTagAsync(CurrentTag!, newName);
            await App.GetService<INavigationViewService>().RenamePinnedTagAsync(CurrentTag!, newName);

            CurrentTag.Name = newName;
            OnPropertyChanged(nameof(CurrentTag));

            await RefreshListAsync();
        
            EnsureItemSelected();
        } 
        catch (Exception ex)
        {
            await App.MainWindow.ShowMessageDialogAsync(ex.Message);
        }


    }

    private async Task DeleteTagAsync()
    {
        try
        {
            var tag = await App.GetService<IPocketDataService>().GetTagByNameAsync(CurrentTag!.Name);

            if (tag != null)
            {
                await App.GetService<IPocketDataService>().RemoveTagAsync(CurrentTag!);
            }

            await App.GetService<INavigationViewService>().RemovePinnedTagAsync(CurrentTag!);

            App.GetService<INavigationService>().NavigateTo(typeof(AllTagsViewModel).FullName!);
        }
        catch (Exception ex)
        {
            await App.MainWindow.ShowMessageDialogAsync(ex.Message);
        }
    }
}
