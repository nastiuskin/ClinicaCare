using Domain.Helpers.PaginationStuff;

namespace ClinicaCare.Client.Services.Pagination
{
    public class PagingResponse<T>
    {
        public List<T> Items { get; set; }
        public MetaData MetaData { get; set; }
    }
}
