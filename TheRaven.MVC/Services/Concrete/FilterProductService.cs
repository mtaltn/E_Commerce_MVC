using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using TheRaven.MVC.Services.Abstract;
using TheRaven.Shared.Dto;

namespace TheRaven.MVC.Services.Concrete;

public class FilterProductService : IFilterProductService
{
    private readonly ElasticsearchClient _elasticsearchClient;
    private const string indexName = "the_raven_mvc";

    public FilterProductService(ElasticsearchClient elasticsearchClient)
    {
        _elasticsearchClient = elasticsearchClient;
    }

    public async Task<List<ProductElasticDto>> SearchAsync(ProductElasticDto productViewModel)
    {
        List<Action<QueryDescriptor<ProductElasticDto>>> listQuery = new();
        if (productViewModel is null)
        {
            listQuery.Add(q => q.MatchAll());
            return await CalculateAgeResultSet(listQuery);
        }
        if (!string.IsNullOrEmpty(productViewModel.ProductName))
        {
            listQuery.Add((q) => q.Match(m => m.Field(f => f.ProductName).Query(productViewModel.ProductName)));

        }
        if (!string.IsNullOrEmpty(productViewModel.ProductDescription))
        {                          //eneme                 //Deneme          //denem   
            string searchValue = "*" + productViewModel.ProductDescription + "*";//Regex
            listQuery.Add((q) => q.Wildcard(m => m.Field(f => f.ProductDescription).Value(searchValue)));

        }
        if (!string.IsNullOrEmpty(productViewModel.Price.ToString()))
        {
            listQuery.Add((q) => q.Range(m => m.NumberRange(f => f.Field(a => a.Price).Gte(productViewModel.Price))));

        }
        if (!string.IsNullOrEmpty(productViewModel.LtePrice.ToString()))
        {
            listQuery.Add((q) => q.Range(m => m.NumberRange(f => f.Field(a => a.Price).Lte(productViewModel.LtePrice))));

        }
        if (!listQuery.Any())
        {
            listQuery.Add(q => q.MatchAll());
        }

        return await CalculateAgeResultSet(listQuery);
    }

    private async Task<List<ProductElasticDto>> CalculateAgeResultSet(List<Action<QueryDescriptor<ProductElasticDto>>> listQuery)
    {
        var result = await _elasticsearchClient.SearchAsync<ProductElasticDto>(s => s.Index(indexName).Query(q => q.Bool(b => b.Must(listQuery.ToArray()))));
        foreach (var hit in result.Hits)
        {
            hit.Source.Id = hit.Id;
        }
        return result.Documents.ToList();
    }
}
