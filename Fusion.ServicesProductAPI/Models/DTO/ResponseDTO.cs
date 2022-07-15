namespace Fusion.ServicesProductAPI.Models.DTO
{
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; } = true;
        public object Result { get; set; }
        public string DisplayMessage { get; set; } = "";
        public List<String> ErrorMessage { get; set; }
    }
}
