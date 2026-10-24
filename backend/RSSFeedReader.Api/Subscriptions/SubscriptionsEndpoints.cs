using Microsoft.AspNetCore.Mvc;

namespace RSSFeedReader.Api.Subscriptions;

public static class SubscriptionsEndpoints
{
    public static IEndpointRouteBuilder MapSubscriptionEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/subscriptions");
        group.MapGet("/", (SubscriptionStore store) => Results.Ok(store.GetAll()));
        group.MapPost("/", AddSubscription);
        return endpoints;
    }

    private static IResult AddSubscription(
        AddSubscriptionRequest? request,
        SubscriptionStore store)
    {
        if (!store.TryAdd(request?.Url, out var subscription))
        {
            return Results.Problem(
                title: "Invalid subscription",
                detail: "A non-empty URL is required.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        return Results.Ok(subscription);
    }

    private sealed record AddSubscriptionRequest(string? Url);
}
