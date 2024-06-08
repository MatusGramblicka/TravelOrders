using Contracts.RequestFeatures;

namespace TravelOrdersClient.Features;

public class PagingResponse<T> where T : class
{
    public IEnumerable<T> Items { get; set; }
    public MetaData MetaData { get; set; }
}