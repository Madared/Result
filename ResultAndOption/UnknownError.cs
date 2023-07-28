﻿using Microsoft.Extensions.Logging;

namespace Results;

public class UnknownError : IError
{
    private readonly IErrorLogger _logger;
    private const string _message = "Unknown Error";
    public string Message => _message;

    public UnknownError()
    {
    }
    public void Log(IErrorLogger logger) => logger.LogError(_message);
}