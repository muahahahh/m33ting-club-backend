using M33tingClub.Application.Tags;
using M33tingClub.Domain.Tags;
using M33tingClub.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace M33tingClub.Infrastructure.Tags;

public class TagRepository : ITagRepository
{
    private readonly M33tingClubDbContext _m33TingClubDbContext;

    public TagRepository(M33tingClubDbContext m33TingClubDbContext)
    {
        _m33TingClubDbContext = m33TingClubDbContext;
    }

    public async Task Add(Tag tag)
    {
        await _m33TingClubDbContext.AddAsync(tag);
    }

    public async Task<Tag?> Get(TagName name)
    {
        return await _m33TingClubDbContext.Tags.SingleOrDefaultAsync(x => x.Name == name);
    }

    public async Task<List<Tag>> GetMany(List<TagName> names)
    {
        return await _m33TingClubDbContext.Tags.Where(x => names.Contains(x.Name)).ToListAsync();
    }
}