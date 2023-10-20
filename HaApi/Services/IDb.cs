using HaApi.Models;

namespace HaApi.Services;

public interface IDb
{
    Task<int> CountSmsAsync();
    Task<Sms> GetSmsAsync(string smsId);
    Task<List<Sms>> ListAllSmsAsync();
    Task<PostResponse> AddSmsAsync(SmsBase sms);
    Task<PostResponse> EditSmsAsync(SmsBase sms);
    Task<PostResponse> DeleteSmsAsync(string smsId);
}
