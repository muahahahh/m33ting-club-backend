using M33tingClub.Domain.Tags;

namespace M33tingClub.Application.Tags;

public interface ITagRepository
{
    Task Add(Tag tag);
    
    Task<Tag?> Get(TagName name);
    
    Task<List<Tag>> GetMany(List<TagName> names);
}