using backend_v3.Dto;
using backend_v3.Models;

namespace backend_v3.Interfaces
{
    public interface IYKienGopYService
    {
        public Task<YkienGopY> ThemYKien(YKienGopYDo data);
        public Task<YKienGopYDo> GetYKienById(string id);
        public Task DeleteYKienById(string id);
        public Task PhanHoi(string id,string phanhoi);
        public Task<PaginatedList<YKienGopYDo>> GetPaginYKienGopY(YKienGopYRequest request);
        public Task<PaginatedList<YKienGopYDo>> GetYKienGopY(YKienGopYRequest request);
    }
}
