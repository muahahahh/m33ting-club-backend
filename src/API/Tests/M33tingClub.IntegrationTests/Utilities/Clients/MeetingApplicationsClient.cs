using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using M33tingClub.Application.MeetingApplications;
using M33tingClub.Application.MeetingApplications.AcceptMeetingApplication;
using M33tingClub.Application.MeetingApplications.RejectMeetingApplication;
using M33tingClub.IntegrationTests.Authentication;

namespace M33tingClub.IntegrationTests.Utilities.Clients;

internal class MeetingApplicationsClient : M33tingClubClientBase
{
    public MeetingApplicationsClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<M33tingClubResponse<List<ApplicationDTO>>> GetApplications(
        Guid meetingId,
        TestAuthUser? user = null)
        => await Get<List<ApplicationDTO>>($"applications/{meetingId}", user?.AuthToken);

    public async Task<M33tingClubResponse> AcceptApplication(
        Guid meetingId,
        Guid userId,
        TestAuthUser? user = null)
    {
        var command = new AcceptMeetingApplicationCommand(meetingId, userId);
        return await Patch($"applications/{meetingId}/user/{userId}/accept",
            command,
            user?.AuthToken);
    }
    
    public async Task<M33tingClubResponse> RejectApplication(
        Guid meetingId,
        Guid userId,
        TestAuthUser? user = null)
    {
        var command = new RejectMeetingApplicationCommand(meetingId, userId);
        return await Patch($"applications/{meetingId}/user/{userId}/reject",
            command,
            user?.AuthToken);
    }
}