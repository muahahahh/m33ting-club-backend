using Newtonsoft.Json;

namespace M33tingClub.Application.Utilities;

public class PagingInfo<T> 
    where T : class
{
    public List<T> Records { get; }

    public int NumberOfRecords => Records.Count;

    public int TotalNumberOfRecords { get; }
    
    public PagingInfo(List<T> records, int totalNumberOfRecords)
    {
        Records = records;
        TotalNumberOfRecords = totalNumberOfRecords;
    }
}