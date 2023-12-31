﻿using HaApi.Models;
using HaApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HaApi.Controllers;

[Route("api/sms")]
[ApiController]
public class SmsController : ControllerBase
{
    private readonly IDb db;

    public SmsController(IDb db)
    {
        this.db = db;
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> CountSms()
    {
        return await db.CountSmsAsync();
    }

    [HttpGet("get")]
    public async Task<ActionResult<Sms>> GetSms([FromQuery] string smsId)
    {
        return await db.GetSmsAsync(smsId);
    }

    [HttpGet("list-all")]
    public async Task<ActionResult<List<Sms>>> ListAllSms()
    {
        return await db.ListAllSmsAsync();
    }

    [HttpPost("add")]
    public async Task<ActionResult<PostResponse>> AddSms([FromBody] SmsBase sms)
    {
        return await db.AddSmsAsync(sms);
    }

    [HttpPut("edit")]
    public async Task<ActionResult<PostResponse>> EditSms([FromBody] SmsBase sms)
    {
        return await db.EditSmsAsync(sms);
    }

    [HttpDelete("delete")]
    public async Task<ActionResult<PostResponse>> DeleteSms([FromQuery] string smsId)
    {
        return await db.DeleteSmsAsync(smsId);
    }
}
