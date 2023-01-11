using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PocketClient.Core.Contracts.Services;
using PocketClient.Core.Models;

namespace PocketClient.Desktop.ViewModels;

public class ManageTagsDialogContentViewModel : ObservableObject
{
    private List<Tag> _allTags = new();

    public ManageTagsDialogContentViewModel()
    {
        SelectedTags = new();
        SuggestedTags = new();

        AddNewTagCommand = new RelayCommand<string>(AddNewTag, CanAddNewTag);
        UpdateSuggestedTagsCommand = new RelayCommand<string>(UpdateSuggestedTags);
    }
    
    public ObservableCollection<Tag> SelectedTags;

    public ObservableCollection<Tag> SuggestedTags;

    public ICommand AddNewTagCommand;
    public ICommand UpdateSuggestedTagsCommand;

    public async Task InitializeAsync(List<Tag> tags)
    {
        var result = await App.GetService<IPocketDataService>().GetAllTagsAsync();
        _allTags = result.OrderBy(t => t.Name).ToList();
        SetSelectedTags(tags);
    }

    private void SetSelectedTags(List<Tag> tags)
    {
        SelectedTags.Clear();

        foreach (var tag in tags)
        {
            SelectedTags.Add(tag);
        }
    }

    private void SetSuggestedTags()
    {
        SuggestedTags.Clear();
        foreach(var tag in _allTags)
        {
            if (SelectedTags.All(item => item.Name != tag.Name))
            {
                SuggestedTags.Add(tag);
            }
        }
    }

    private void AddNewTag(string name)
    {
        var tagName = name.Trim().ToLower();
        var tag = SelectedTags.Where(tag => tag.Name.Equals(tagName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        if (tag == null)
        {
            SelectedTags.Add(new Tag { Name = tagName });
        }
    }

    private bool CanAddNewTag(string name)
    {
        return !string.IsNullOrWhiteSpace(name);
    }

    private void UpdateSuggestedTags(string? input)
    {
        SuggestedTags.Clear();

        List<Tag> suggests;

        if (!string.IsNullOrWhiteSpace(input))
        {
            suggests = _allTags.Where(item => item.Name.Contains(input, StringComparison.InvariantCultureIgnoreCase)).ToList();
        } 
        else
        {
            suggests = _allTags.ToList();
        }

        foreach (var tag in suggests)
        {
            if (SelectedTags.All(item => item.Name != tag.Name))
            {
                SuggestedTags.Add(tag);
            }
        }
    }
}
