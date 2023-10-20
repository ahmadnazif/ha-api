using HaApi.Models;

namespace HaApi.Services;

public class InMemoryDb : IDb
{
    private Dictionary<string, Sms> SmsDict { get; set; } = new();

    public async Task<int> CountSmsAsync() => await Task.FromResult(SmsDict.Count);

    public async Task<PostResponse> AddSmsAsync(SmsBase sms)
    {
        var id = Guid.NewGuid().ToString("N").ToUpper();
        SmsDict.Add(id, new()
        {
            SmsId = id,
            Text = sms.Text,
            To = sms.To,
            From = sms.From,
            CreatedTime = DateTime.Now
        });

        return await Task.FromResult(new PostResponse { IsSuccess = true });

    }

    public async Task<PostResponse> DeleteSmsAsync(string smsId)
    {
        var succ = SmsDict.Remove(smsId);
        return await Task.FromResult(new PostResponse { IsSuccess = succ });
    }

    public async Task<PostResponse> EditSmsAsync(SmsBase sms)
    {
        var succ = SmsDict.TryGetValue(sms.SmsId, out var data);
        if(succ)
        {
            // TODO
        }

        return await Task.FromResult(new PostResponse { IsSuccess = false, Message = "Not implemented yet" });
    }

    public async Task<Sms> GetSmsAsync(string smsId)
    {
        var succ = SmsDict.TryGetValue(smsId, out var data);
        return succ ? await Task.FromResult(data) : await Task.FromResult((Sms)null);
    }

    public async Task<List<Sms>> ListAllSmsAsync()
    {
        var list = SmsDict.Select(d => d.Value);
        return await Task.FromResult(list.ToList());
    }
}
