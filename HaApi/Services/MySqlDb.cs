using HaApi.Models;
using MySql.Data.MySqlClient;
using System.Text.Json;

namespace HaApi.Services;

public class MySqlDb : IDb
{
    private readonly ILogger<MySqlDb> logger;
    private readonly string dbConString;

    public MySqlDb(ILogger<MySqlDb> logger, IConfiguration config)
    {
        this.logger = logger;
        dbConString = GenerateConnectionString(config);
    }

    private static string GenerateConnectionString(IConfiguration config)
    {
        var server = config["MySqlDb:Server"];
        var dbName = config["MySqlDb:DbName"];
        var userId = config["MySqlDb:UserId"];
        var password = config["MySqlDb:Password"];

        return $"Server={server};Database={dbName};User={userId};Password={password};";
    }

    #region Helper
    private static object? GetObjectValue(object obj)
    {
        if (obj == DBNull.Value) return null;
        else return obj;
    }

    private static string? GetStringValue(object obj)
    {
        if (obj == DBNull.Value) return null;
        else return obj.ToString();
    }

    private static byte[]? GetByteArrayValue(object obj)
    {
        if (obj == DBNull.Value) return null;
        else return (byte[])obj;
    }

    private static DateTime? GetDateTimeValue(object obj)
    {
        if (obj == DBNull.Value) return null;
        else return Convert.ToDateTime(obj);
    }

    private static double? GetDoubleValue(object obj)
    {
        if (obj == DBNull.Value) return null;
        else
        {
            return obj.ToString() == null ? null : double.Parse(obj.ToString());
        }
    }
    private static int? GetIntValue(object obj)
    {
        if (obj == DBNull.Value) return null;
        else
        {
            return obj.ToString() == null ? null : int.Parse(obj.ToString());
        }
    }

    private static long? GetLongValue(object obj)
    {
        if (obj == DBNull.Value) return null;
        else
        {
            return obj.ToString() == null ? null : long.Parse(obj.ToString());
        }
    }

    /// <summary>
    /// Already handled if value is NULL or empty or whitespace
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static Dictionary<string, int> DeserializeDict(string value)
    {
        try
        {
            return string.IsNullOrWhiteSpace(value) ? new() : JsonSerializer.Deserialize<Dictionary<string, int>>(value);
        }
        catch
        {
            return new();
        }
    }

    private static List<T> DeserializeList<T>(string value)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(value))
                return new();

            var t = JsonSerializer.Deserialize<List<T>>(value);
            var generic = (List<T>)Convert.ChangeType(t, typeof(List<T>));

            return generic;
        }
        catch
        {
            return new();
        }
    }
    #endregion

    public async Task<PostResponse> AddSmsAsync(SmsBase sms)
    {
        try
        {
            PostResponse resp = new() { IsSuccess = false };
            string query =
                "INSERT INTO sms (id, `from`, `to`, text) VALUES " +
                "(@a, @b, @c, @d);";

            using (MySqlConnection connection = new(this.dbConString))
            {
                await connection.OpenAsync();
                using MySqlCommand cmd = new(query, connection);
                cmd.Parameters.AddWithValue("@a", sms.SmsId);
                cmd.Parameters.AddWithValue("@b", sms.From);
                cmd.Parameters.AddWithValue("@c", sms.To);
                cmd.Parameters.AddWithValue("@d", sms.Text);

                await cmd.ExecuteNonQueryAsync();
                resp = new PostResponse
                {
                    IsSuccess = true,
                    Message = "SMS added"
                };
            }

            return resp;
        }
        catch (Exception ex)
        {
            logger.LogError($"{nameof(AddSmsAsync)}: {ex.Message}");
            return new PostResponse
            {
                IsSuccess = false,
                Message = ex.Message
            };
        }
    }

    public async Task<PostResponse> EditSmsAsync(SmsBase sms)
    {
        try
        {
            PostResponse resp = new() { IsSuccess = false };
            string query =
                "UPDATE sms SET `from` = @b, `to` = @c, text = @d WHERE id = @a;";

            using (MySqlConnection connection = new(this.dbConString))
            {
                await connection.OpenAsync();
                using MySqlCommand cmd = new(query, connection);
                cmd.Parameters.AddWithValue("@a", sms.SmsId);
                cmd.Parameters.AddWithValue("@b", sms.From);
                cmd.Parameters.AddWithValue("@c", sms.To);
                cmd.Parameters.AddWithValue("@d", sms.Text);

                await cmd.ExecuteNonQueryAsync();
                resp = new PostResponse
                {
                    IsSuccess = true,
                    Message = $"SMS '{sms.SmsId}' updated"
                };
            }

            return resp;
        }
        catch (Exception ex)
        {
            logger.LogError($"{nameof(EditSmsAsync)}: {ex.Message}");
            return new PostResponse
            {
                IsSuccess = false,
                Message = ex.Message
            };
        }
    }

    public async Task<PostResponse> DeleteSmsAsync(string smsId)
    {
        try
        {
            PostResponse resp = new() { IsSuccess = false };
            string query =
                "DELETE FROM sms WHERE id = @a;";

            using (MySqlConnection connection = new(this.dbConString))
            {
                await connection.OpenAsync();
                using MySqlCommand cmd = new(query, connection);
                cmd.Parameters.AddWithValue("@a", smsId);

                await cmd.ExecuteNonQueryAsync();
                resp = new PostResponse
                {
                    IsSuccess = true,
                    Message = $"SMS '{smsId}' deleted"
                };
            }

            return resp;
        }
        catch (Exception ex)
        {
            logger.LogError($"{nameof(DeleteSmsAsync)}: {ex.Message}");
            return new PostResponse
            {
                IsSuccess = false,
                Message = ex.Message
            };
        }
    }

    public async Task<int> CountSmsAsync()
    {
        try
        {
            int data = 0;
            string sql = "SELECT COUNT(*) FROM sms;";

            using (MySqlConnection connection = new(this.dbConString))
            {
                await connection.OpenAsync();
                using MySqlCommand cmd = new(sql, connection);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    data = GetIntValue(reader[0]).Value;
                }
            }

            return data;
        }
        catch (Exception ex)
        {
            logger.LogError($"{nameof(CountSmsAsync)}: {ex.Message}");
            return 0;
        }
    }


    public async Task<Sms> GetSmsAsync(string smsId)
    {
        try
        {
            Sms data = null;
            string sql = "SELECT * FROM sms WHERE id = @id;";

            using (MySqlConnection connection = new(this.dbConString))
            {
                await connection.OpenAsync();
                using MySqlCommand cmd = new(sql, connection);
                cmd.Parameters.AddWithValue("@id", smsId);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    data = new()
                    {
                        SmsId = smsId,
                        From = GetStringValue(reader["from"]),
                        To = GetStringValue(reader["to"]),
                        Text = GetStringValue(reader["text"]),
                        CreatedTime = GetDateTimeValue(reader["created_time"]).Value
                    };
                }
            }

            return data;
        }
        catch(Exception ex)
        {
            logger.LogError($"{nameof(GetSmsAsync)}: {ex.Message}");
            return null;
        }
    }

    public async Task<List<Sms>> ListAllSmsAsync()
    {
        try
        {
            List<Sms> data = new();
            string sql = "SELECT * FROM sms;";

            using (MySqlConnection connection = new(this.dbConString))
            {
                await connection.OpenAsync();
                using MySqlCommand cmd = new(sql, connection);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    data.Add(new()
                    {
                        SmsId = GetStringValue(reader["id"]),
                        From = GetStringValue(reader["from"]),
                        To = GetStringValue(reader["to"]),
                        Text = GetStringValue(reader["text"]),
                        CreatedTime = GetDateTimeValue(reader["created_time"]).Value
                    });
                }
            }

            return data;
        }
        catch (Exception ex)
        {
            logger.LogError($"{nameof(ListAllSmsAsync)}: {ex.Message}");
            return new();
        }
    }
}
