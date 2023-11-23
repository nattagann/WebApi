using System.Net;

public static class HttpStatusHelper
{
    public static string GetStatusMessage(HttpStatusCode statusCode)
    {
        string? statusName = Enum.GetName(typeof(HttpStatusCode), statusCode);
        return statusName ?? "Unknown";
    }
}
