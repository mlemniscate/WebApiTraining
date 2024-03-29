﻿namespace Shared.RequestFeatures;

public abstract class RequestParameters
{
    private const int MaxPageSize = 50;
    private int _pageSize = MaxPageSize;

    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public string? SearchTerm { get; set; }

    public string? Fields { get; set; }

    public string? OrderBy { get; set; }

}