
namespace FUExchange.ModelViews.BanModelViews
{
    public class BanModelView
    {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        required public string? ReportId { get; set; }
=======
        required public string ReportId { get; set; }
        
>>>>>>> Stashed changes
        required public DateTime? Expires { get; set; }
=======
        public required string Id { get; set; }
        public required string ReportId { get; set; }
        public DateTime? Expires {  get; set; } 
>>>>>>> Stashed changes
    }
}
