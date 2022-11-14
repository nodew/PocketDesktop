using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using System.Linq;

namespace Pocket.Sdk;

public class PocketHttpClient
{
    private readonly string comsumerKey;

    private string accessToken;

    private readonly HttpClient httpClient;

    private string requestToken;

    private readonly string baseUrl = "https://getpocket.com";

    public PocketHttpClient(HttpClient httpClient, string comsumerKey)
    {
        this.httpClient = httpClient;
        this.comsumerKey = comsumerKey;
        accessToken = string.Empty;
        requestToken = string.Empty;
    }

    public PocketHttpClient(HttpClient httpClient, string comsumerKey, string accessToken) : this(httpClient, comsumerKey)
    {
        this.accessToken = accessToken;
    }

    public async Task<PocketItem> AddItemAsync(string url, CancellationToken cancellationToken = default)
    {
        PocketAddItemRequest requestData = new PocketAddItemRequest()
        {
            Url = url,
        };
        PocketAddItemResponse response = await SendRequestAsync<PocketAddItemRequest, PocketAddItemResponse>("/v3/add", requestData, true, cancellationToken);
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
        string query = BuildQueryWithFilter(filter, count, offset);
        Uri requestUri = AppendAccessToken($"/v3/get?{query}");

        HttpResponseMessage response = await httpClient.GetAsync(requestUri, cancellationToken);
        response.EnsureSuccessStatusCode();

        Stream responseContent = await response.Content.ReadAsStreamAsync();
        PocketRetriveItemResponse? result = await JsonSerializer.DeserializeAsync<PocketRetriveItemResponse>(responseContent);
        if (result != null && result.List != null)
        {
            return result.List.Values.ToList();
        }

        return new List<PocketItem>();
    }

    public async Task<List<string>> GetAllTagsAsync(CancellationToken cancellationToken = default)
    {
        Uri requestUri = AppendAccessToken("/v3/get?forcetaglist=1&taglist=1&count=1&offset=0");
        HttpResponseMessage response = await httpClient.GetAsync(requestUri, cancellationToken);
        response.EnsureSuccessStatusCode();

        Stream responseContent = await response.Content.ReadAsStreamAsync();
        PocketRetriveItemResponse? result = await JsonSerializer.DeserializeAsync<PocketRetriveItemResponse>(responseContent);
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
        PocketTokenRequest request = new PocketTokenRequest()
        {
            ConsumerKey = comsumerKey,
            RedirectUri = redirectUri
        };
        PocketTokenResponse result = await SendRequestAsync<PocketTokenRequest, PocketTokenResponse>("/v3/oauth/request", request, false, cancellationToken);
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

        PocketAuthorizeResponse result = await SendRequestAsync<PocketAuthorizeRequest, PocketAuthorizeResponse>("/v3/oauth/authorize", request, false, cancellationToken);

        accessToken = result.AccessToken;

        return result;
    }

    private async Task<Response> SendRequestAsync<Request, Response>(string url, Request data, bool requireAuth, CancellationToken cancellationToken = default)
    {
        string json = JsonSerializer.Serialize(data);
        Uri requestUri = requireAuth ? AppendAccessToken(url) : new Uri(baseUrl + url);

        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
        requestMessage.Content = new StringContent(json);
        requestMessage.Content.Headers.ContentType.MediaType = "application/json";
        requestMessage.Content.Headers.ContentType.CharSet = "UTF-8";
        requestMessage.Headers.Add("X-Accept", "application/json");

        HttpResponseMessage response = await httpClient.SendAsync(requestMessage, cancellationToken);
        response.EnsureSuccessStatusCode();

        Stream responseContent = await response.Content.ReadAsStreamAsync();
        Response? result = await JsonSerializer.DeserializeAsync<Response>(responseContent);

        if (result == null)
        {
            throw new Exception("Unexpected null result");
        }

        return result;
    }

    private Uri AppendAccessToken(string url)
    {
        UriBuilder builder = new UriBuilder(baseUrl + url);
        NameValueCollection queryParams = HttpUtility.ParseQueryString(builder.Query);

        queryParams.Add("consumer_key", comsumerKey);
        queryParams.Add("access_token", accessToken);
        builder.Query = queryParams.ToString();

        return builder.Uri;
    }

    private string BuildQueryWithFilter(PocketItemFilter filter, int count, int offset)
    {
        NameValueCollection queryParams = new NameValueCollection();
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
                queryParams.Add("since", $"{filter.Since?.ToUnixTimeSeconds()}");
            }
        }

        queryParams.Add("count", $"{count}");
        queryParams.Add("offset", $"{offset}");

        return convertToQueryString(queryParams);
    }

    private string convertToQueryString(NameValueCollection collection)
    {
        return string.Join("&", collection.AllKeys.Select(key => key + "=" + HttpUtility.UrlEncode(collection[key])));
    }
}

