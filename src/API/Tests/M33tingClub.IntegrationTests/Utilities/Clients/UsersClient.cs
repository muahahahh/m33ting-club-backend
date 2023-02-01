using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using M33tingClub.Application.MeetingNotifications.GetUnseenMeetingNotifications;
using M33tingClub.Application.MeetingNotifications.MarkMeetingNotificationsAsSeen;
using M33tingClub.Application.Users;
using M33tingClub.Application.Users.FinishUserSignUp;
using M33tingClub.Application.Users.Followers.GetFollowers;
using M33tingClub.Application.Users.Followers.GetFollowing;
using M33tingClub.Application.Users.GetUser;
using M33tingClub.Application.Users.GetUserSelf;
using M33tingClub.Application.Utilities;
using M33tingClub.IntegrationTests.Authentication;

namespace M33tingClub.IntegrationTests.Utilities.Clients;

internal class UsersClient : M33tingClubClientBase
{
    public UsersClient(HttpClient httpClient) : base(httpClient)
    {
        
    }

    public async Task<M33tingClubResponse> FinishSignUp(
        SignUpCommand signUpCommand,
        string authToken)
        => await Post(
            "users/sign-up", 
            signUpCommand, 
            authToken);
    
    public async Task<M33tingClubResponse<UserDTO>> Me(
        string authToken)
        => await Get<UserDTO>(
            "users/me",
            authToken);

    public async Task<M33tingClubResponse<List<MeetingNotificationDTO>>> GetUnseenMeetingNotifications(
        string authToken,
        List<string>? types = null)
    {
        var parameterWithValues = new List<string>();

        if (types is not null && types.Any())
        {
            foreach (var type in types)
                parameterWithValues.Add($"types={type}");
        }
        
        return await Get<List<MeetingNotificationDTO>>(
            $"users/meeting-notifications{BuildQuery(parameterWithValues)}",
            authToken);
    }

    public async Task<M33tingClubResponse> MarkAsSeenMeetingNotifications(
        MarkMeetingNotificationsAsSeenCommand command,
        string authToken)
        => await Patch(
            "users/meeting-notifications/mark-as-seen",
            command,
            authToken);
    
    public async Task<M33tingClubResponse> Delete(
        string authToken)
        => await Delete(
            "users",
            authToken);
    
    public async Task<M33tingClubResponse<UserDetailsDTO>> GetUser(
        string authToken,
        Guid userId)
        => await Get<UserDetailsDTO>(
            $"users/{userId}",
            authToken);
    
    public async Task<M33tingClubResponse<ObjectCreatedResponse>> UploadUserImage(MemoryStream stream, TestAuthUser? user = null)
        => await PostFile<ObjectCreatedResponse>("users/avatar", stream, user?.AuthToken);

    public async Task<M33tingClubResponse> FollowUser(Guid userToFollowId, TestAuthUser? user = null)
        => await Post($"users/{userToFollowId}/follow", user?.AuthToken);
    
    public async Task<M33tingClubResponse> UnfollowUser(Guid userToUnfollowId, TestAuthUser? user = null)
        => await Post($"users/{userToUnfollowId}/unfollow", user?.AuthToken);

    public async Task<M33tingClubResponse<PagingInfo<FollowerDTO>>> GetFollowers(
        Guid userId,
        int? limit = null,
        int? offset = null,
        TestAuthUser? user = null)
    {
        var parameterWithValues = new List<string>();

        if (limit is not null)
            parameterWithValues.Add($"limit={limit}");

        if (offset is not null)
            parameterWithValues.Add($"offset={offset}");

        return await Get<PagingInfo<FollowerDTO>>($"users/{userId}/followers{BuildQuery(parameterWithValues)}",
            user?.AuthToken);
    }

    public async Task<M33tingClubResponse<PagingInfo<FollowingDTO>>> GetFollowing(
        Guid userId,
        int? limit = null,
        int? offset = null,
        TestAuthUser? user = null)
    {
        var parameterWithValues = new List<string>();

        if (limit is not null)
            parameterWithValues.Add($"limit={limit}");

        if (offset is not null)
            parameterWithValues.Add($"offset={offset}");

        return await Get<PagingInfo<FollowingDTO>>($"users/{userId}/following{BuildQuery(parameterWithValues)}",
            user?.AuthToken);
    }
}