using System.Collections.Concurrent;

namespace RSSFeedReader.Api.Subscriptions;

public sealed class SubscriptionStore
{
    private readonly ConcurrentQueue<Subscription> subscriptions = new();

    public IReadOnlyList<Subscription> GetAll() => subscriptions.ToArray();

    public bool TryAdd(string? url, out Subscription? subscription)
    {
        if (string.IsNullOrEmpty(url))
        {
            subscription = null;
            return false;
        }

        subscription = new Subscription(url);
        subscriptions.Enqueue(subscription);
        return true;
    }
}
