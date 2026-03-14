namespace MajhiPaithani.Application.Models.Response
{
    public class DeleteProductResponse
    {
        public int ProductId { get; set; }
        public DateTime DeletedDate { get; set; }
        public string Message { get; set; }
    }
}