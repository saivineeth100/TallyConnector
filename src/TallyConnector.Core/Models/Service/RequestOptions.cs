﻿namespace TallyConnector.Core.Models.Service;
public class BaseRequestOptions
{
    public string? Company { get; set; }
    public XmlAttributeOverrides? XMLAttributeOverrides { get; set; }
}
public class PostRequestOptions : BaseRequestOptions
{

}

public class DateFilterRequestOptions : BaseRequestOptions
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class CountRequestOptions : DateFilterRequestOptions
{
    public List<Filter>? Filters { get; set; }
    public bool IsInitialize { get; set; }

    public string CollectionType { get; set; }
}
public class RequestOptions : DateFilterRequestOptions
{
    public List<Filter>? Filters { get; set; }
    public bool IsInitialize { get; set; } 
    public List<string>? FetchList { get; set; }
    public List<string>? Compute { get; set; } = new();
    public List<string>? ComputeVar { get; set; } = new();

 //   public List<TallyCustomObject>? Objects { get; set; }
}

public class PaginatedRequestOptions : RequestOptions
{
    public int PageNum { get; set; } = 1;
    public int? RecordsPerPage { get; set; }
}
public class MasterRequestOptions : RequestOptions
{
    public MasterLookupField LookupField { get; set; } = MasterLookupField.Name;
}
public class VoucherRequestOptions : RequestOptions
{
    public VoucherLookupField LookupField { get; set; } = VoucherLookupField.VoucherNumber;
}

public class CollectionRequestOptions : PaginatedRequestOptions
{
    public CollectionRequestOptions()
    {
    }

    public CollectionRequestOptions(string collectionType)
    {
        CollectionType = collectionType;
    }

    public bool Pagination { get; set; }
    public string CollectionType { get; set; }
    public string? ChildOf { get; set; }

}

public enum MasterLookupField
{
    MasterId = 1,
    AlterId = 2,
    Name = 3,
    GUID = 4,
}
public enum VoucherLookupField
{
    MasterId = 1,
    AlterId = 2,
    VoucherNumber = 3,
    GUID = 4,
}