using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using M33tingClub.Application.Meetings;
using M33tingClub.Application.Meetings.AddMeeting;
using M33tingClub.Application.Meetings.CancelMeeting;
using M33tingClub.Application.Meetings.EditMeeting;
using M33tingClub.Application.Meetings.GetFeed;
using M33tingClub.Application.Meetings.JoinMeeting;
using M33tingClub.Application.Meetings.LeaveMeeting;
using M33tingClub.Application.Utilities;
using M33tingClub.IntegrationTests.Authentication;

namespace M33tingClub.IntegrationTests.Utilities.Clients;

internal class MeetingsClient : M33tingClubClientBase
{
	public MeetingsClient(HttpClient httpClient) : base(httpClient)
	{
	}

	public async Task<M33tingClubResponse<MeetingDTO>> GetMeeting(
		Guid id, 
		double longitude = 0, 
		double latitude = 0, 
		TestAuthUser? user = null)
		=> await Get<MeetingDTO>($"meetings/{id}?longitude={longitude}&latitude={latitude}", user?.AuthToken);

	public async Task<M33tingClubResponse<PagingInfo<MeetingDTO>>> GetMeetings(
		List<string>? tags = null,
		string? status = null,
		double? longitude = null,
		double? latitude = null,
		int? limit = null,
		int? offset = null,
		TestAuthUser? user = null)
	{
		var parameterWithValues = new List<string>();

		if (tags is not null)
		{
			foreach (var tag in tags)
				parameterWithValues.Add($"tags={tag}");
		}

		if (status is not null)
			parameterWithValues.Add($"status={status}");
		
		if (longitude is not null)
			parameterWithValues.Add($"longitude={longitude}");
		
		if (latitude is not null)
			parameterWithValues.Add($"latitude={latitude}");
		
		if (limit is not null)
			parameterWithValues.Add($"limit={limit}");
		
		if (offset is not null)
			parameterWithValues.Add($"offset={offset}");
		
		return await Get<PagingInfo<MeetingDTO>>($"meetings{BuildQuery(parameterWithValues)}", user?.AuthToken);
	}

	public async Task<M33tingClubResponse<ObjectCreatedResponse>> AddMeeting(
		AddMeetingCommand addMeetingCommand,
		TestAuthUser? user = null)
		=> await Post<AddMeetingCommand, ObjectCreatedResponse>("meetings", addMeetingCommand, user?.AuthToken);

	public async Task<M33tingClubResponse<List<MeetingDTO>>> GetOwnMeetings(
		List<string> statuses, int limit, int offset, string? role = null, TestAuthUser? user = null)
	{
		var parameterWithValues = new List<string>();
		if (role is not null)
		{
			parameterWithValues.Add($"role={role}");
		}
		
		foreach (var status in statuses)
			parameterWithValues.Add($"statuses={status}");
			
		parameterWithValues.Add($"limit={limit}");
		parameterWithValues.Add($"offset={offset}");
		return await Get<List<MeetingDTO>>($"meetings/own{BuildQuery(parameterWithValues)}", user?.AuthToken);
	}

	public async Task<M33tingClubResponse> EditMeeting(EditMeetingCommand editMeetingCommand, TestAuthUser? user = null)
		=> await Put("meetings", editMeetingCommand, user?.AuthToken);
	
	public async Task<M33tingClubResponse> JoinMeeting(Guid id, TestAuthUser? user = null)
	{
		var joinMeetingCommand = new JoinMeetingCommand(id);
		return await Post($"meetings/{id}/join", joinMeetingCommand, user?.AuthToken);
	}
	
	public async Task<M33tingClubResponse> LeaveMeeting(Guid id, TestAuthUser? user = null)
	{
		var leaveMeetingCommand = new LeaveMeetingCommand(id);
		return await Post($"meetings/{id}/leave", leaveMeetingCommand, user?.AuthToken);
	}

	public async Task<M33tingClubResponse> CancelMeeting(Guid id, TestAuthUser? user = null)
	{
		var cancelMeetingCommand = new CancelMeetingCommand(id);
		return await Patch($"meetings/{id}/cancel", cancelMeetingCommand, user?.AuthToken);
	}

	public async Task<M33tingClubResponse<ObjectCreatedResponse>> UploadBackgroundImage(MemoryStream stream, TestAuthUser? user = null)
		=> await PostFile<ObjectCreatedResponse>("meetings/backgrounds", stream, user?.AuthToken);
	
	public async Task<M33tingClubResponse<PagingInfo<FeedDTO>>> GetFeed(
		int? limit = null,
		int? offset = null,
		TestAuthUser? user = null)
	{
		var parameterWithValues = new List<string>();

		if (limit is not null)
			parameterWithValues.Add($"limit={limit}");
		
		if (offset is not null)
			parameterWithValues.Add($"offset={offset}");
		
		return await Get<PagingInfo<FeedDTO>>($"meetings/feed{BuildQuery(parameterWithValues)}", user?.AuthToken);
	}
}