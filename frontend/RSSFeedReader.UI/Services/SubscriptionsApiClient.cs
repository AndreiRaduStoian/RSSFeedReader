using System.Net.Http.Json;

namespace RSSFeedReader.UI.Services;

public sealed class SubscriptionsApiClient(HttpClient httpClient)
{
    public async Task<IReadOnlyList<Subscription>> GetSubscriptionsAsync(
        CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<List<Subscription>>(
                   "subscriptions",
                   cancellationToken)
               ?? throw new InvalidOperationException("The subscriptions API returned no response body.");
    }

    public async Task<Subscription> AddSubscriptionAsync(
        string url,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync(
            "subscriptions",
            new AddSubscriptionRequest(url),
            cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Subscription>(cancellationToken)
               ?? throw new InvalidOperationException("The subscriptions API returned no subscription.");
    }

    public sealed record Subscription(string Url);

    private sealed record AddSubscriptionRequest(string Url);
}
