using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using M33tingClub.Application.Tags;
using M33tingClub.Application.Tags.AddCommunityTag;
using M33tingClub.Application.Tags.AddOfficialTag;
using M33tingClub.Application.Utilities;
using M33tingClub.IntegrationTests.Authentication;

namespace M33tingClub.IntegrationTests.Utilities.Clients;

internal class TagsClient : M33tingClubClientBase
{
    public TagsClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<M33tingClubResponse<TagDTO>> GetTag(string name, TestAuthUser? user = null)
        => await Get<TagDTO>($"tags/{name}", user?.AuthToken);

    public async Task<M33tingClubResponse<List<TagDTO>>> GetTags(bool? isOfficial = null, TestAuthUser? user = null)
    {
        var parameterWithValues = new List<string>();

        if (isOfficial is not null)
        {
            var value = isOfficial.Value ? "true" : "false";
            parameterWithValues.Add($"isOfficial={value}");
        }

        return await Get<List<TagDTO>>($"tags{BuildQuery(parameterWithValues)}", user?.AuthToken);   
    }

    public async Task<M33tingClubResponse<PagingInfo<TagDTO>>> SearchTags(
        string? phrase = null,
        int? limit = null,
        int? offset = null,
        TestAuthUser? user = null)
    {
        var parameterWithValues = new List<string>();

        if (!string.IsNullOrWhiteSpace(phrase))
            parameterWithValues.Add($"phrase={phrase}");

        if (limit is not null)
            parameterWithValues.Add($"limit={limit}");
		
        if (limit is not null)
            parameterWithValues.Add($"offset={offset}");
        
        return await Get<PagingInfo<TagDTO>>($"tags/search{BuildQuery(parameterWithValues)}", user?.AuthToken); 
    }

    public async Task<M33tingClubResponse<NamedObjectCreatedResponse>> AddCommunityTag(
        AddCommunityTagCommand addCommunityTagCommand, 
        TestAuthUser? user = null)
        => await Post<AddCommunityTagCommand, NamedObjectCreatedResponse>("tags/community", addCommunityTagCommand, user?.AuthToken);
    
    public async Task<M33tingClubResponse<NamedObjectCreatedResponse>> AddOfficialTag(
        AddOfficialTagCommand addOfficialTagCommand, 
        TestAuthUser? user = null)
        => await Post<AddOfficialTagCommand, NamedObjectCreatedResponse>("tags/official", addOfficialTagCommand, user?.AuthToken);
}