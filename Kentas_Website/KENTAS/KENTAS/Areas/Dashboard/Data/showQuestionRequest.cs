
namespace KENTAS.Areas.Dashboard.Data
{
    public class showQuestionRequest
    {
        public int Id { get; set; }
        public int AnketId { get; set; }
        public string Soru { get; set; }
        public int CevapTipi { get; set; }
        public int Count { get; set; }


    }
}