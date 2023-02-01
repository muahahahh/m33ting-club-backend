namespace M33tingClub.Application.Utilities.ImageStorageServiceInterfaces;


public interface IImageStorageService
{
    public Task PutObject(string name, MemoryStream request);

    public List<string> GetAllObjectKeys();

    public Task DeleteObjects(List<string> keys);
}