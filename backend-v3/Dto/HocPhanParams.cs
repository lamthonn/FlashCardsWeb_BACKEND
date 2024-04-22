using backend_v3.Seriloger;

namespace backend_v3.Dto
{
    public class HocPhanParams : DtoToLogging
    {
        public HocPhanDto? hocphan { get; set; }
        public List<TheHocDto>? theHocs { get; set; }
    }
}
