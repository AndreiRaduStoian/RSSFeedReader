using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace RSSFeedReader.Api.Tests;

public class SubscriptionsApiTests
{
    [Fact]
    public async Task GetSubscriptions_WhenNoSubscriptionsExist_ReturnsEmptyList()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var subscriptions = await client.GetFromJsonAsync<List<SubscriptionResponse>>("/api/subscriptions");

        Assert.NotNull(subscriptions);
        Assert.Empty(subscriptions);
    }

    [Fact]
    public async Task PostSubscription_AddsSubmittedUrlAndKeepsExistingEntries()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();
        const string firstUrl = "https://example.com/first.xml";
        const string secondUrl = "https://example.com/second.xml";

        var firstResponse = await client.PostAsJsonAsync("/api/subscriptions", new { url = firstUrl });
        var secondResponse = await client.PostAsJsonAsync("/api/subscriptions", new { url = secondUrl });

        Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, secondResponse.StatusCode);

        var subscriptions = await client.GetFromJsonAsync<List<SubscriptionResponse>>("/api/subscriptions");

        Assert.Equal([firstUrl, secondUrl], subscriptions?.Select(subscription => subscription.Url));
    }

    [Fact]
    public async Task PostSubscription_AllowsDuplicateUrlsAsSeparateEntries()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();
        const string url = "https://example.com/feed.xml";

        await client.PostAsJsonAsync("/api/subscriptions", new { url });
        await client.PostAsJsonAsync("/api/subscriptions", new { url });

        var subscriptions = await client.GetFromJsonAsync<List<SubscriptionResponse>>("/api/subscriptions");

        Assert.Equal([url, url], subscriptions?.Select(subscription => subscription.Url));
    }

    [Theory]
    [InlineData("""{"url":""}""")]
    [InlineData("{}")]
    public async Task PostSubscription_WhenUrlIsMissingOrEmpty_ReturnsBadRequestAndDoesNotAdd(
        string requestBody)
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();
        using var content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/api/subscriptions", content);
        var subscriptions = await client.GetFromJsonAsync<List<SubscriptionResponse>>("/api/subscriptions");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(subscriptions);
        Assert.Empty(subscriptions);
    }

    private sealed record SubscriptionResponse(string Url);
}
