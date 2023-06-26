namespace FindJobs.Web.Models
{
    public class GenericRespnseApi<T> where T : class
    {
        public T Data { get; set; }
        public int MessageCode { get; set; }
    }
}
