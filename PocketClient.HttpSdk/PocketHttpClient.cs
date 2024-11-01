﻿using System.Collections.Specialized;
using System.Text.Json;
using System.Web;

namespace PocketClient.HttpSdk;

public class PocketHttpClient
{
    private readonly HttpClient httpClient;
    private readonly string baseUrl = "https://getpocket.com";

    private string comsumerKey;
    private string accessToken;
    private string requestToken;

    public PocketHttpClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;

        comsumerKey = string.Empty;
        accessToken = string.Empty;
        requestToken = string.Empty;
    }

    public void SetConsumerKey(string comsumerKey)
    {
        this.comsumerKey = comsumerKey;
    }

    public void SetAccessToken(string accessToken)
    {
        this.accessToken = accessToken;
    }

    public async Task<PocketItem> AddItemAsync(Uri url, string? title = null, CancellationToken cancellationToken = default)
    {
        var requestData = new PocketAddItemRequest()
        {
            Url = url.ToString(),
            Title = title,
        };
        var response = await SendRequestAsync<PocketAddItemRequest, PocketAddItemResponse>("/v3/add", requestData, true, cancellationToken);
        return response.Item;
    }

    public async Task ArchiveItemAsync(long itemId, CancellationToken cancellationToken = default)
    {
        await UpdateAsync<ArchiveItemAction, bool>(new ArchiveItemAction(itemId), cancellationToken);
    }

    public async Task ReAddItemAsync(long itemId, CancellationToken cancellationToken = default)
    {
        await UpdateAsync<ReAddItemAction, PocketItem>(new ReAddItemAction(itemId), cancellationToken);
    }

    public async Task FavoriteItemAsync(long itemId, CancellationToken cancellationToken = default)
    {
        await UpdateAsync<FavoriteItemAction, bool>(new FavoriteItemAction(itemId), cancellationToken);
    }

    public async Task UnfavoriteItemAsync(long itemId, CancellationToken cancellationToken = default)
    {
        await UpdateAsync<UnfavoriteItemAction, bool>(new UnfavoriteItemAction(itemId), cancellationToken);
    }

    public async Task DeleteItemAsync(long itemId, CancellationToken cancellationToken = default)
    {
        await UpdateAsync<DeleteItemAction, bool>(new DeleteItemAction(itemId), cancellationToken);
    }

    public async Task AddTagsAsync(long itemId, List<string> Tags, CancellationToken cancellationToken = default)
    {
        await UpdateAsync<AddTagsAction, bool>(new AddTagsAction(itemId, string.Join(",", Tags)), cancellationToken);
    }

    public async Task RemoveTagsAsync(long itemId, List<string> Tags, CancellationToken cancellationToken = default)
    {
        await UpdateAsync<RemoveTagsAction, bool>(new RemoveTagsAction(itemId, string.Join(",", Tags)), cancellationToken);
    }

    public async Task ReplaceTagsAsync(long itemId, List<string> Tags, CancellationToken cancellationToken = default)
    {
        await UpdateAsync<ReplaceTagsAction, bool>(new ReplaceTagsAction(itemId, string.Join(",", Tags)), cancellationToken);
    }

    public async Task ClearTagsAsync(long itemId, CancellationToken cancellationToken = default)
    {
        await UpdateAsync<ClearTagsAction, bool>(new ClearTagsAction(itemId), cancellationToken);
    }

    public async Task RenameTagAsync(string oldTag, string newTag, CancellationToken cancellationToken = default)
    {
        await UpdateAsync<RenameTagAction, bool>(new RenameTagAction(oldTag, newTag), cancellationToken);
    }

    public async Task DeleteTagAsync(string tag, CancellationToken cancellationToken = default)
    {
        await UpdateAsync<DeleteTagAction, bool>(new DeleteTagAction(tag), cancellationToken);
    }

    public async Task<List<PocketItem>> GetItemsAsync(PocketItemFilter filter, int count = 20, int offset = 0, CancellationToken cancellationToken = default)
    {
        var query = BuildQueryWithFilter(filter, count, offset);
        var requestUri = AppendAccessToken($"/v3/get?{query}");

        var response = await httpClient.GetAsync(requestUri, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStreamAsync();
        var result = await JsonSerializer.DeserializeAsync<PocketRetriveItemResponse>(responseContent);
        if (result != null && result.List != null)
        {
            return result.List.Values.ToList();
        }

        return new List<PocketItem>();
    }

    public async Task<List<string>> GetAllTagsAsync(CancellationToken cancellationToken = default)
    {
        var requestUri = AppendAccessToken("/v3/get?forcetaglist=1&taglist=1&count=1&offset=0");
        var response = await httpClient.GetAsync(requestUri, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStreamAsync();
        var result = await JsonSerializer.DeserializeAsync<PocketRetriveItemResponse>(responseContent);
        if (result != null)
        {
            return result.Tags;
        }

        return new List<string>();
    }

    public async Task<PocketUpdateResponse<U>> UpdateAsync<T, U>(T action, CancellationToken cancellationToken = default) where T : PocketAction
    {
        return await SendRequestAsync<PocketUpdateRequest<T>, PocketUpdateResponse<U>>(
            "/v3/send",
            new PocketUpdateRequest<T>()
            {
                Actions = new List<T>
                {
                    action
                }
            },
            true,
            cancellationToken
        );
    }

    public async Task<string> GetTokenAsync(string redirectUri, CancellationToken cancellationToken = default)
    {
        var request = new PocketTokenRequest()
        {
            ConsumerKey = comsumerKey,
            RedirectUri = redirectUri
        };
        var result = await SendRequestAsync<PocketTokenRequest, PocketTokenResponse>("/v3/oauth/request", request, false, cancellationToken);
        requestToken = result.Code;
        return result.Code;
    }

    public async Task<PocketAuthorizeResponse> AuthorizeAsync(string? code = null, CancellationToken cancellationToken = default)
    {
        var request = new PocketAuthorizeRequest()
        {
            ConsumerKey = comsumerKey,
            Code = code ?? requestToken
        };

        var result = await SendRequestAsync<PocketAuthorizeRequest, PocketAuthorizeResponse>("/v3/oauth/authorize", request, false, cancellationToken);

        accessToken = result.AccessToken;

        return result;
    }

    private async Task<Response> SendRequestAsync<Request, Response>(string url, Request data, bool requireAuth, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(data);
        var requestUri = requireAuth ? AppendAccessToken(url) : new Uri(baseUrl + url);

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri)
        {
            Content = new StringContent(json)
        };
        requestMessage.Content.Headers.ContentType.MediaType = "application/json";
        requestMessage.Content.Headers.ContentType.CharSet = "UTF-8";
        requestMessage.Headers.Add("X-Accept", "application/json");

        var response = await httpClient.SendAsync(requestMessage, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStreamAsync();
        var result = await JsonSerializer.DeserializeAsync<Response>(responseContent);

        if (result == null)
        {
            throw new Exception("Unexpected null result");
        }

        return result;
    }

    private Uri AppendAccessToken(string url)
    {
        var builder = new UriBuilder(baseUrl + url);
        var queryParams = HttpUtility.ParseQueryString(builder.Query);

        queryParams.Add("consumer_key", comsumerKey);
        queryParams.Add("access_token", accessToken);
        builder.Query = queryParams.ToString();

        return builder.Uri;
    }

    private string BuildQueryWithFilter(PocketItemFilter filter, int count, int offset)
    {
        var queryParams = new NameValueCollection();
        if (filter != null)
        {
            if (filter.State != null)
            {
                queryParams.Add("state", filter.State.ToString().ToLower());
            }
            else
            {
                queryParams.Add("state", "all");
            }

            if (filter.ContentType != null)
            {
                queryParams.Add("contentType", filter.ContentType.ToString().ToLower());
            }

            if (filter.DetailType != null)
            {
                queryParams.Add("detailType", filter.DetailType.ToString().ToLower());
            }
            else
            {
                queryParams.Add("detailType", PocketItemDetailType.Simple.ToString().ToLower());
            }

            if (!string.IsNullOrEmpty(filter.Search))
            {
                queryParams.Add("search", filter.Search);
            }

            if (!string.IsNullOrEmpty(filter.Tag))
            {
                queryParams.Add("tag", filter.Tag);
            }

            if (!string.IsNullOrEmpty(filter.Domain))
            {
                queryParams.Add("domain", filter.Domain);
            }

            if (filter.SortBy != null)
            {
                queryParams.Add("sort", filter.SortBy.ToString().ToLower());
            }
            else
            {
                queryParams.Add("sort", "newest");
            }

            if (filter.Favorite != null)
            {
                queryParams.Add("favorite", (bool)filter.Favorite ? "1" : "0");
            }

            if (filter.Since != null)
            {
                var timestamp = filter.Since.Value.ToUnixTimeSeconds();

                // The timestamp should be greater than 0.
                if (timestamp > 0)
                {
                    queryParams.Add("since", $"{timestamp}");
                }
            }
        }

        queryParams.Add("count", $"{count}");
        queryParams.Add("offset", $"{offset}");

        return ConvertToQueryString(queryParams);
    }

    private string ConvertToQueryString(NameValueCollection collection)
    {
        return string.Join("&", collection.AllKeys.Select(key => key + "=" + HttpUtility.UrlEncode(collection[key])));
    }
}

