using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PocketClient.Core.Models;
using PocketClient.Core.Specifications;

namespace PocketClient.Desktop.ViewModels;

public class TaggedItemsViewModel : ItemsViewModel
{
    private Tag? _currentTag;

    public TaggedItemsViewModel() : base()
    {
    }

    public Tag? CurrentTag
    {
        get => _currentTag;
        set => SetProperty(ref _currentTag, value);
    }

    protected override BaseSpecification<PocketItem> BuildFilter()
    {
        var filter = base.BuildFilter();
        filter.SetFilterCondition(item => item.Tags.Contains(CurrentTag!));
        return filter;
    }

    protected async override Task NavigatedTo(object parameter)
    {
        CurrentTag = (Tag)parameter;
        await base.NavigatedTo(parameter);
    }
}
