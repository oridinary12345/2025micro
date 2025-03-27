using PlayFab.ClientModels;
using PlayFab.Internal;
using PlayFab.SharedModels;
using System;

namespace PlayFab.Events
{
	public class PlayFabEvents
	{
		public delegate void PlayFabErrorEvent(PlayFabRequestCommon request, PlayFabError error);

		public delegate void PlayFabResultEvent<in TResult>(TResult result) where TResult : PlayFabResultCommon;

		public delegate void PlayFabRequestEvent<in TRequest>(TRequest request) where TRequest : PlayFabRequestCommon;

		private static PlayFabEvents _instance;

		public event PlayFabResultEvent<LoginResult> OnLoginResultEvent;

		public event PlayFabRequestEvent<AcceptTradeRequest> OnAcceptTradeRequestEvent;

		public event PlayFabResultEvent<AcceptTradeResponse> OnAcceptTradeResultEvent;

		public event PlayFabRequestEvent<AddFriendRequest> OnAddFriendRequestEvent;

		public event PlayFabResultEvent<AddFriendResult> OnAddFriendResultEvent;

		public event PlayFabRequestEvent<AddGenericIDRequest> OnAddGenericIDRequestEvent;

		public event PlayFabResultEvent<AddGenericIDResult> OnAddGenericIDResultEvent;

		public event PlayFabRequestEvent<AddOrUpdateContactEmailRequest> OnAddOrUpdateContactEmailRequestEvent;

		public event PlayFabResultEvent<AddOrUpdateContactEmailResult> OnAddOrUpdateContactEmailResultEvent;

		public event PlayFabRequestEvent<AddSharedGroupMembersRequest> OnAddSharedGroupMembersRequestEvent;

		public event PlayFabResultEvent<AddSharedGroupMembersResult> OnAddSharedGroupMembersResultEvent;

		public event PlayFabRequestEvent<AddUsernamePasswordRequest> OnAddUsernamePasswordRequestEvent;

		public event PlayFabResultEvent<AddUsernamePasswordResult> OnAddUsernamePasswordResultEvent;

		public event PlayFabRequestEvent<AddUserVirtualCurrencyRequest> OnAddUserVirtualCurrencyRequestEvent;

		public event PlayFabResultEvent<ModifyUserVirtualCurrencyResult> OnAddUserVirtualCurrencyResultEvent;

		public event PlayFabRequestEvent<AndroidDevicePushNotificationRegistrationRequest> OnAndroidDevicePushNotificationRegistrationRequestEvent;

		public event PlayFabResultEvent<AndroidDevicePushNotificationRegistrationResult> OnAndroidDevicePushNotificationRegistrationResultEvent;

		public event PlayFabRequestEvent<AttributeInstallRequest> OnAttributeInstallRequestEvent;

		public event PlayFabResultEvent<AttributeInstallResult> OnAttributeInstallResultEvent;

		public event PlayFabRequestEvent<CancelTradeRequest> OnCancelTradeRequestEvent;

		public event PlayFabResultEvent<CancelTradeResponse> OnCancelTradeResultEvent;

		public event PlayFabRequestEvent<ConfirmPurchaseRequest> OnConfirmPurchaseRequestEvent;

		public event PlayFabResultEvent<ConfirmPurchaseResult> OnConfirmPurchaseResultEvent;

		public event PlayFabRequestEvent<ConsumeItemRequest> OnConsumeItemRequestEvent;

		public event PlayFabResultEvent<ConsumeItemResult> OnConsumeItemResultEvent;

		public event PlayFabRequestEvent<CreateSharedGroupRequest> OnCreateSharedGroupRequestEvent;

		public event PlayFabResultEvent<CreateSharedGroupResult> OnCreateSharedGroupResultEvent;

		public event PlayFabRequestEvent<ExecuteCloudScriptRequest> OnExecuteCloudScriptRequestEvent;

		public event PlayFabResultEvent<ExecuteCloudScriptResult> OnExecuteCloudScriptResultEvent;

		public event PlayFabRequestEvent<GetAccountInfoRequest> OnGetAccountInfoRequestEvent;

		public event PlayFabResultEvent<GetAccountInfoResult> OnGetAccountInfoResultEvent;

		public event PlayFabRequestEvent<ListUsersCharactersRequest> OnGetAllUsersCharactersRequestEvent;

		public event PlayFabResultEvent<ListUsersCharactersResult> OnGetAllUsersCharactersResultEvent;

		public event PlayFabRequestEvent<GetCatalogItemsRequest> OnGetCatalogItemsRequestEvent;

		public event PlayFabResultEvent<GetCatalogItemsResult> OnGetCatalogItemsResultEvent;

		public event PlayFabRequestEvent<GetCharacterDataRequest> OnGetCharacterDataRequestEvent;

		public event PlayFabResultEvent<GetCharacterDataResult> OnGetCharacterDataResultEvent;

		public event PlayFabRequestEvent<GetCharacterInventoryRequest> OnGetCharacterInventoryRequestEvent;

		public event PlayFabResultEvent<GetCharacterInventoryResult> OnGetCharacterInventoryResultEvent;

		public event PlayFabRequestEvent<GetCharacterLeaderboardRequest> OnGetCharacterLeaderboardRequestEvent;

		public event PlayFabResultEvent<GetCharacterLeaderboardResult> OnGetCharacterLeaderboardResultEvent;

		public event PlayFabRequestEvent<GetCharacterDataRequest> OnGetCharacterReadOnlyDataRequestEvent;

		public event PlayFabResultEvent<GetCharacterDataResult> OnGetCharacterReadOnlyDataResultEvent;

		public event PlayFabRequestEvent<GetCharacterStatisticsRequest> OnGetCharacterStatisticsRequestEvent;

		public event PlayFabResultEvent<GetCharacterStatisticsResult> OnGetCharacterStatisticsResultEvent;

		public event PlayFabRequestEvent<GetContentDownloadUrlRequest> OnGetContentDownloadUrlRequestEvent;

		public event PlayFabResultEvent<GetContentDownloadUrlResult> OnGetContentDownloadUrlResultEvent;

		public event PlayFabRequestEvent<CurrentGamesRequest> OnGetCurrentGamesRequestEvent;

		public event PlayFabResultEvent<CurrentGamesResult> OnGetCurrentGamesResultEvent;

		public event PlayFabRequestEvent<GetFriendLeaderboardRequest> OnGetFriendLeaderboardRequestEvent;

		public event PlayFabResultEvent<GetLeaderboardResult> OnGetFriendLeaderboardResultEvent;

		public event PlayFabRequestEvent<GetFriendLeaderboardAroundPlayerRequest> OnGetFriendLeaderboardAroundPlayerRequestEvent;

		public event PlayFabResultEvent<GetFriendLeaderboardAroundPlayerResult> OnGetFriendLeaderboardAroundPlayerResultEvent;

		public event PlayFabRequestEvent<GetFriendsListRequest> OnGetFriendsListRequestEvent;

		public event PlayFabResultEvent<GetFriendsListResult> OnGetFriendsListResultEvent;

		public event PlayFabRequestEvent<GameServerRegionsRequest> OnGetGameServerRegionsRequestEvent;

		public event PlayFabResultEvent<GameServerRegionsResult> OnGetGameServerRegionsResultEvent;

		public event PlayFabRequestEvent<GetLeaderboardRequest> OnGetLeaderboardRequestEvent;

		public event PlayFabResultEvent<GetLeaderboardResult> OnGetLeaderboardResultEvent;

		public event PlayFabRequestEvent<GetLeaderboardAroundCharacterRequest> OnGetLeaderboardAroundCharacterRequestEvent;

		public event PlayFabResultEvent<GetLeaderboardAroundCharacterResult> OnGetLeaderboardAroundCharacterResultEvent;

		public event PlayFabRequestEvent<GetLeaderboardAroundPlayerRequest> OnGetLeaderboardAroundPlayerRequestEvent;

		public event PlayFabResultEvent<GetLeaderboardAroundPlayerResult> OnGetLeaderboardAroundPlayerResultEvent;

		public event PlayFabRequestEvent<GetLeaderboardForUsersCharactersRequest> OnGetLeaderboardForUserCharactersRequestEvent;

		public event PlayFabResultEvent<GetLeaderboardForUsersCharactersResult> OnGetLeaderboardForUserCharactersResultEvent;

		public event PlayFabRequestEvent<GetPaymentTokenRequest> OnGetPaymentTokenRequestEvent;

		public event PlayFabResultEvent<GetPaymentTokenResult> OnGetPaymentTokenResultEvent;

		public event PlayFabRequestEvent<GetPhotonAuthenticationTokenRequest> OnGetPhotonAuthenticationTokenRequestEvent;

		public event PlayFabResultEvent<GetPhotonAuthenticationTokenResult> OnGetPhotonAuthenticationTokenResultEvent;

		public event PlayFabRequestEvent<GetPlayerCombinedInfoRequest> OnGetPlayerCombinedInfoRequestEvent;

		public event PlayFabResultEvent<GetPlayerCombinedInfoResult> OnGetPlayerCombinedInfoResultEvent;

		public event PlayFabRequestEvent<GetPlayerProfileRequest> OnGetPlayerProfileRequestEvent;

		public event PlayFabResultEvent<GetPlayerProfileResult> OnGetPlayerProfileResultEvent;

		public event PlayFabRequestEvent<GetPlayerSegmentsRequest> OnGetPlayerSegmentsRequestEvent;

		public event PlayFabResultEvent<GetPlayerSegmentsResult> OnGetPlayerSegmentsResultEvent;

		public event PlayFabRequestEvent<GetPlayerStatisticsRequest> OnGetPlayerStatisticsRequestEvent;

		public event PlayFabResultEvent<GetPlayerStatisticsResult> OnGetPlayerStatisticsResultEvent;

		public event PlayFabRequestEvent<GetPlayerStatisticVersionsRequest> OnGetPlayerStatisticVersionsRequestEvent;

		public event PlayFabResultEvent<GetPlayerStatisticVersionsResult> OnGetPlayerStatisticVersionsResultEvent;

		public event PlayFabRequestEvent<GetPlayerTagsRequest> OnGetPlayerTagsRequestEvent;

		public event PlayFabResultEvent<GetPlayerTagsResult> OnGetPlayerTagsResultEvent;

		public event PlayFabRequestEvent<GetPlayerTradesRequest> OnGetPlayerTradesRequestEvent;

		public event PlayFabResultEvent<GetPlayerTradesResponse> OnGetPlayerTradesResultEvent;

		public event PlayFabRequestEvent<GetPlayFabIDsFromFacebookIDsRequest> OnGetPlayFabIDsFromFacebookIDsRequestEvent;

		public event PlayFabResultEvent<GetPlayFabIDsFromFacebookIDsResult> OnGetPlayFabIDsFromFacebookIDsResultEvent;

		public event PlayFabRequestEvent<GetPlayFabIDsFromGameCenterIDsRequest> OnGetPlayFabIDsFromGameCenterIDsRequestEvent;

		public event PlayFabResultEvent<GetPlayFabIDsFromGameCenterIDsResult> OnGetPlayFabIDsFromGameCenterIDsResultEvent;

		public event PlayFabRequestEvent<GetPlayFabIDsFromGenericIDsRequest> OnGetPlayFabIDsFromGenericIDsRequestEvent;

		public event PlayFabResultEvent<GetPlayFabIDsFromGenericIDsResult> OnGetPlayFabIDsFromGenericIDsResultEvent;

		public event PlayFabRequestEvent<GetPlayFabIDsFromGoogleIDsRequest> OnGetPlayFabIDsFromGoogleIDsRequestEvent;

		public event PlayFabResultEvent<GetPlayFabIDsFromGoogleIDsResult> OnGetPlayFabIDsFromGoogleIDsResultEvent;

		public event PlayFabRequestEvent<GetPlayFabIDsFromKongregateIDsRequest> OnGetPlayFabIDsFromKongregateIDsRequestEvent;

		public event PlayFabResultEvent<GetPlayFabIDsFromKongregateIDsResult> OnGetPlayFabIDsFromKongregateIDsResultEvent;

		public event PlayFabRequestEvent<GetPlayFabIDsFromSteamIDsRequest> OnGetPlayFabIDsFromSteamIDsRequestEvent;

		public event PlayFabResultEvent<GetPlayFabIDsFromSteamIDsResult> OnGetPlayFabIDsFromSteamIDsResultEvent;

		public event PlayFabRequestEvent<GetPlayFabIDsFromTwitchIDsRequest> OnGetPlayFabIDsFromTwitchIDsRequestEvent;

		public event PlayFabResultEvent<GetPlayFabIDsFromTwitchIDsResult> OnGetPlayFabIDsFromTwitchIDsResultEvent;

		public event PlayFabRequestEvent<GetPublisherDataRequest> OnGetPublisherDataRequestEvent;

		public event PlayFabResultEvent<GetPublisherDataResult> OnGetPublisherDataResultEvent;

		public event PlayFabRequestEvent<GetPurchaseRequest> OnGetPurchaseRequestEvent;

		public event PlayFabResultEvent<GetPurchaseResult> OnGetPurchaseResultEvent;

		public event PlayFabRequestEvent<GetSharedGroupDataRequest> OnGetSharedGroupDataRequestEvent;

		public event PlayFabResultEvent<GetSharedGroupDataResult> OnGetSharedGroupDataResultEvent;

		public event PlayFabRequestEvent<GetStoreItemsRequest> OnGetStoreItemsRequestEvent;

		public event PlayFabResultEvent<GetStoreItemsResult> OnGetStoreItemsResultEvent;

		public event PlayFabRequestEvent<GetTimeRequest> OnGetTimeRequestEvent;

		public event PlayFabResultEvent<GetTimeResult> OnGetTimeResultEvent;

		public event PlayFabRequestEvent<GetTitleDataRequest> OnGetTitleDataRequestEvent;

		public event PlayFabResultEvent<GetTitleDataResult> OnGetTitleDataResultEvent;

		public event PlayFabRequestEvent<GetTitleNewsRequest> OnGetTitleNewsRequestEvent;

		public event PlayFabResultEvent<GetTitleNewsResult> OnGetTitleNewsResultEvent;

		public event PlayFabRequestEvent<GetTitlePublicKeyRequest> OnGetTitlePublicKeyRequestEvent;

		public event PlayFabResultEvent<GetTitlePublicKeyResult> OnGetTitlePublicKeyResultEvent;

		public event PlayFabRequestEvent<GetTradeStatusRequest> OnGetTradeStatusRequestEvent;

		public event PlayFabResultEvent<GetTradeStatusResponse> OnGetTradeStatusResultEvent;

		public event PlayFabRequestEvent<GetUserDataRequest> OnGetUserDataRequestEvent;

		public event PlayFabResultEvent<GetUserDataResult> OnGetUserDataResultEvent;

		public event PlayFabRequestEvent<GetUserInventoryRequest> OnGetUserInventoryRequestEvent;

		public event PlayFabResultEvent<GetUserInventoryResult> OnGetUserInventoryResultEvent;

		public event PlayFabRequestEvent<GetUserDataRequest> OnGetUserPublisherDataRequestEvent;

		public event PlayFabResultEvent<GetUserDataResult> OnGetUserPublisherDataResultEvent;

		public event PlayFabRequestEvent<GetUserDataRequest> OnGetUserPublisherReadOnlyDataRequestEvent;

		public event PlayFabResultEvent<GetUserDataResult> OnGetUserPublisherReadOnlyDataResultEvent;

		public event PlayFabRequestEvent<GetUserDataRequest> OnGetUserReadOnlyDataRequestEvent;

		public event PlayFabResultEvent<GetUserDataResult> OnGetUserReadOnlyDataResultEvent;

		public event PlayFabRequestEvent<GetWindowsHelloChallengeRequest> OnGetWindowsHelloChallengeRequestEvent;

		public event PlayFabResultEvent<GetWindowsHelloChallengeResponse> OnGetWindowsHelloChallengeResultEvent;

		public event PlayFabRequestEvent<GrantCharacterToUserRequest> OnGrantCharacterToUserRequestEvent;

		public event PlayFabResultEvent<GrantCharacterToUserResult> OnGrantCharacterToUserResultEvent;

		public event PlayFabRequestEvent<LinkAndroidDeviceIDRequest> OnLinkAndroidDeviceIDRequestEvent;

		public event PlayFabResultEvent<LinkAndroidDeviceIDResult> OnLinkAndroidDeviceIDResultEvent;

		public event PlayFabRequestEvent<LinkCustomIDRequest> OnLinkCustomIDRequestEvent;

		public event PlayFabResultEvent<LinkCustomIDResult> OnLinkCustomIDResultEvent;

		public event PlayFabRequestEvent<LinkFacebookAccountRequest> OnLinkFacebookAccountRequestEvent;

		public event PlayFabResultEvent<LinkFacebookAccountResult> OnLinkFacebookAccountResultEvent;

		public event PlayFabRequestEvent<LinkGameCenterAccountRequest> OnLinkGameCenterAccountRequestEvent;

		public event PlayFabResultEvent<LinkGameCenterAccountResult> OnLinkGameCenterAccountResultEvent;

		public event PlayFabRequestEvent<LinkGoogleAccountRequest> OnLinkGoogleAccountRequestEvent;

		public event PlayFabResultEvent<LinkGoogleAccountResult> OnLinkGoogleAccountResultEvent;

		public event PlayFabRequestEvent<LinkIOSDeviceIDRequest> OnLinkIOSDeviceIDRequestEvent;

		public event PlayFabResultEvent<LinkIOSDeviceIDResult> OnLinkIOSDeviceIDResultEvent;

		public event PlayFabRequestEvent<LinkKongregateAccountRequest> OnLinkKongregateRequestEvent;

		public event PlayFabResultEvent<LinkKongregateAccountResult> OnLinkKongregateResultEvent;

		public event PlayFabRequestEvent<LinkSteamAccountRequest> OnLinkSteamAccountRequestEvent;

		public event PlayFabResultEvent<LinkSteamAccountResult> OnLinkSteamAccountResultEvent;

		public event PlayFabRequestEvent<LinkTwitchAccountRequest> OnLinkTwitchRequestEvent;

		public event PlayFabResultEvent<LinkTwitchAccountResult> OnLinkTwitchResultEvent;

		public event PlayFabRequestEvent<LinkWindowsHelloAccountRequest> OnLinkWindowsHelloRequestEvent;

		public event PlayFabResultEvent<LinkWindowsHelloAccountResponse> OnLinkWindowsHelloResultEvent;

		public event PlayFabRequestEvent<LoginWithAndroidDeviceIDRequest> OnLoginWithAndroidDeviceIDRequestEvent;

		public event PlayFabRequestEvent<LoginWithCustomIDRequest> OnLoginWithCustomIDRequestEvent;

		public event PlayFabRequestEvent<LoginWithEmailAddressRequest> OnLoginWithEmailAddressRequestEvent;

		public event PlayFabRequestEvent<LoginWithFacebookRequest> OnLoginWithFacebookRequestEvent;

		public event PlayFabRequestEvent<LoginWithGameCenterRequest> OnLoginWithGameCenterRequestEvent;

		public event PlayFabRequestEvent<LoginWithGoogleAccountRequest> OnLoginWithGoogleAccountRequestEvent;

		public event PlayFabRequestEvent<LoginWithIOSDeviceIDRequest> OnLoginWithIOSDeviceIDRequestEvent;

		public event PlayFabRequestEvent<LoginWithKongregateRequest> OnLoginWithKongregateRequestEvent;

		public event PlayFabRequestEvent<LoginWithPlayFabRequest> OnLoginWithPlayFabRequestEvent;

		public event PlayFabRequestEvent<LoginWithSteamRequest> OnLoginWithSteamRequestEvent;

		public event PlayFabRequestEvent<LoginWithTwitchRequest> OnLoginWithTwitchRequestEvent;

		public event PlayFabRequestEvent<LoginWithWindowsHelloRequest> OnLoginWithWindowsHelloRequestEvent;

		public event PlayFabRequestEvent<MatchmakeRequest> OnMatchmakeRequestEvent;

		public event PlayFabResultEvent<MatchmakeResult> OnMatchmakeResultEvent;

		public event PlayFabRequestEvent<OpenTradeRequest> OnOpenTradeRequestEvent;

		public event PlayFabResultEvent<OpenTradeResponse> OnOpenTradeResultEvent;

		public event PlayFabRequestEvent<PayForPurchaseRequest> OnPayForPurchaseRequestEvent;

		public event PlayFabResultEvent<PayForPurchaseResult> OnPayForPurchaseResultEvent;

		public event PlayFabRequestEvent<PurchaseItemRequest> OnPurchaseItemRequestEvent;

		public event PlayFabResultEvent<PurchaseItemResult> OnPurchaseItemResultEvent;

		public event PlayFabRequestEvent<RedeemCouponRequest> OnRedeemCouponRequestEvent;

		public event PlayFabResultEvent<RedeemCouponResult> OnRedeemCouponResultEvent;

		public event PlayFabRequestEvent<RegisterForIOSPushNotificationRequest> OnRegisterForIOSPushNotificationRequestEvent;

		public event PlayFabResultEvent<RegisterForIOSPushNotificationResult> OnRegisterForIOSPushNotificationResultEvent;

		public event PlayFabRequestEvent<RegisterPlayFabUserRequest> OnRegisterPlayFabUserRequestEvent;

		public event PlayFabResultEvent<RegisterPlayFabUserResult> OnRegisterPlayFabUserResultEvent;

		public event PlayFabRequestEvent<RegisterWithWindowsHelloRequest> OnRegisterWithWindowsHelloRequestEvent;

		public event PlayFabRequestEvent<RemoveContactEmailRequest> OnRemoveContactEmailRequestEvent;

		public event PlayFabResultEvent<RemoveContactEmailResult> OnRemoveContactEmailResultEvent;

		public event PlayFabRequestEvent<RemoveFriendRequest> OnRemoveFriendRequestEvent;

		public event PlayFabResultEvent<RemoveFriendResult> OnRemoveFriendResultEvent;

		public event PlayFabRequestEvent<RemoveGenericIDRequest> OnRemoveGenericIDRequestEvent;

		public event PlayFabResultEvent<RemoveGenericIDResult> OnRemoveGenericIDResultEvent;

		public event PlayFabRequestEvent<RemoveSharedGroupMembersRequest> OnRemoveSharedGroupMembersRequestEvent;

		public event PlayFabResultEvent<RemoveSharedGroupMembersResult> OnRemoveSharedGroupMembersResultEvent;

		public event PlayFabRequestEvent<DeviceInfoRequest> OnReportDeviceInfoRequestEvent;

		public event PlayFabResultEvent<EmptyResult> OnReportDeviceInfoResultEvent;

		public event PlayFabRequestEvent<ReportPlayerClientRequest> OnReportPlayerRequestEvent;

		public event PlayFabResultEvent<ReportPlayerClientResult> OnReportPlayerResultEvent;

		public event PlayFabRequestEvent<RestoreIOSPurchasesRequest> OnRestoreIOSPurchasesRequestEvent;

		public event PlayFabResultEvent<RestoreIOSPurchasesResult> OnRestoreIOSPurchasesResultEvent;

		public event PlayFabRequestEvent<SendAccountRecoveryEmailRequest> OnSendAccountRecoveryEmailRequestEvent;

		public event PlayFabResultEvent<SendAccountRecoveryEmailResult> OnSendAccountRecoveryEmailResultEvent;

		public event PlayFabRequestEvent<SetFriendTagsRequest> OnSetFriendTagsRequestEvent;

		public event PlayFabResultEvent<SetFriendTagsResult> OnSetFriendTagsResultEvent;

		public event PlayFabRequestEvent<SetPlayerSecretRequest> OnSetPlayerSecretRequestEvent;

		public event PlayFabResultEvent<SetPlayerSecretResult> OnSetPlayerSecretResultEvent;

		public event PlayFabRequestEvent<StartGameRequest> OnStartGameRequestEvent;

		public event PlayFabResultEvent<StartGameResult> OnStartGameResultEvent;

		public event PlayFabRequestEvent<StartPurchaseRequest> OnStartPurchaseRequestEvent;

		public event PlayFabResultEvent<StartPurchaseResult> OnStartPurchaseResultEvent;

		public event PlayFabRequestEvent<SubtractUserVirtualCurrencyRequest> OnSubtractUserVirtualCurrencyRequestEvent;

		public event PlayFabResultEvent<ModifyUserVirtualCurrencyResult> OnSubtractUserVirtualCurrencyResultEvent;

		public event PlayFabRequestEvent<UnlinkAndroidDeviceIDRequest> OnUnlinkAndroidDeviceIDRequestEvent;

		public event PlayFabResultEvent<UnlinkAndroidDeviceIDResult> OnUnlinkAndroidDeviceIDResultEvent;

		public event PlayFabRequestEvent<UnlinkCustomIDRequest> OnUnlinkCustomIDRequestEvent;

		public event PlayFabResultEvent<UnlinkCustomIDResult> OnUnlinkCustomIDResultEvent;

		public event PlayFabRequestEvent<UnlinkFacebookAccountRequest> OnUnlinkFacebookAccountRequestEvent;

		public event PlayFabResultEvent<UnlinkFacebookAccountResult> OnUnlinkFacebookAccountResultEvent;

		public event PlayFabRequestEvent<UnlinkGameCenterAccountRequest> OnUnlinkGameCenterAccountRequestEvent;

		public event PlayFabResultEvent<UnlinkGameCenterAccountResult> OnUnlinkGameCenterAccountResultEvent;

		public event PlayFabRequestEvent<UnlinkGoogleAccountRequest> OnUnlinkGoogleAccountRequestEvent;

		public event PlayFabResultEvent<UnlinkGoogleAccountResult> OnUnlinkGoogleAccountResultEvent;

		public event PlayFabRequestEvent<UnlinkIOSDeviceIDRequest> OnUnlinkIOSDeviceIDRequestEvent;

		public event PlayFabResultEvent<UnlinkIOSDeviceIDResult> OnUnlinkIOSDeviceIDResultEvent;

		public event PlayFabRequestEvent<UnlinkKongregateAccountRequest> OnUnlinkKongregateRequestEvent;

		public event PlayFabResultEvent<UnlinkKongregateAccountResult> OnUnlinkKongregateResultEvent;

		public event PlayFabRequestEvent<UnlinkSteamAccountRequest> OnUnlinkSteamAccountRequestEvent;

		public event PlayFabResultEvent<UnlinkSteamAccountResult> OnUnlinkSteamAccountResultEvent;

		public event PlayFabRequestEvent<UnlinkTwitchAccountRequest> OnUnlinkTwitchRequestEvent;

		public event PlayFabResultEvent<UnlinkTwitchAccountResult> OnUnlinkTwitchResultEvent;

		public event PlayFabRequestEvent<UnlinkWindowsHelloAccountRequest> OnUnlinkWindowsHelloRequestEvent;

		public event PlayFabResultEvent<UnlinkWindowsHelloAccountResponse> OnUnlinkWindowsHelloResultEvent;

		public event PlayFabRequestEvent<UnlockContainerInstanceRequest> OnUnlockContainerInstanceRequestEvent;

		public event PlayFabResultEvent<UnlockContainerItemResult> OnUnlockContainerInstanceResultEvent;

		public event PlayFabRequestEvent<UnlockContainerItemRequest> OnUnlockContainerItemRequestEvent;

		public event PlayFabResultEvent<UnlockContainerItemResult> OnUnlockContainerItemResultEvent;

		public event PlayFabRequestEvent<UpdateAvatarUrlRequest> OnUpdateAvatarUrlRequestEvent;

		public event PlayFabResultEvent<EmptyResult> OnUpdateAvatarUrlResultEvent;

		public event PlayFabRequestEvent<UpdateCharacterDataRequest> OnUpdateCharacterDataRequestEvent;

		public event PlayFabResultEvent<UpdateCharacterDataResult> OnUpdateCharacterDataResultEvent;

		public event PlayFabRequestEvent<UpdateCharacterStatisticsRequest> OnUpdateCharacterStatisticsRequestEvent;

		public event PlayFabResultEvent<UpdateCharacterStatisticsResult> OnUpdateCharacterStatisticsResultEvent;

		public event PlayFabRequestEvent<UpdatePlayerStatisticsRequest> OnUpdatePlayerStatisticsRequestEvent;

		public event PlayFabResultEvent<UpdatePlayerStatisticsResult> OnUpdatePlayerStatisticsResultEvent;

		public event PlayFabRequestEvent<UpdateSharedGroupDataRequest> OnUpdateSharedGroupDataRequestEvent;

		public event PlayFabResultEvent<UpdateSharedGroupDataResult> OnUpdateSharedGroupDataResultEvent;

		public event PlayFabRequestEvent<UpdateUserDataRequest> OnUpdateUserDataRequestEvent;

		public event PlayFabResultEvent<UpdateUserDataResult> OnUpdateUserDataResultEvent;

		public event PlayFabRequestEvent<UpdateUserDataRequest> OnUpdateUserPublisherDataRequestEvent;

		public event PlayFabResultEvent<UpdateUserDataResult> OnUpdateUserPublisherDataResultEvent;

		public event PlayFabRequestEvent<UpdateUserTitleDisplayNameRequest> OnUpdateUserTitleDisplayNameRequestEvent;

		public event PlayFabResultEvent<UpdateUserTitleDisplayNameResult> OnUpdateUserTitleDisplayNameResultEvent;

		public event PlayFabRequestEvent<ValidateAmazonReceiptRequest> OnValidateAmazonIAPReceiptRequestEvent;

		public event PlayFabResultEvent<ValidateAmazonReceiptResult> OnValidateAmazonIAPReceiptResultEvent;

		public event PlayFabRequestEvent<ValidateGooglePlayPurchaseRequest> OnValidateGooglePlayPurchaseRequestEvent;

		public event PlayFabResultEvent<ValidateGooglePlayPurchaseResult> OnValidateGooglePlayPurchaseResultEvent;

		public event PlayFabRequestEvent<ValidateIOSReceiptRequest> OnValidateIOSReceiptRequestEvent;

		public event PlayFabResultEvent<ValidateIOSReceiptResult> OnValidateIOSReceiptResultEvent;

		public event PlayFabRequestEvent<ValidateWindowsReceiptRequest> OnValidateWindowsStoreReceiptRequestEvent;

		public event PlayFabResultEvent<ValidateWindowsReceiptResult> OnValidateWindowsStoreReceiptResultEvent;

		public event PlayFabRequestEvent<WriteClientCharacterEventRequest> OnWriteCharacterEventRequestEvent;

		public event PlayFabResultEvent<WriteEventResponse> OnWriteCharacterEventResultEvent;

		public event PlayFabRequestEvent<WriteClientPlayerEventRequest> OnWritePlayerEventRequestEvent;

		public event PlayFabResultEvent<WriteEventResponse> OnWritePlayerEventResultEvent;

		public event PlayFabRequestEvent<WriteTitleEventRequest> OnWriteTitleEventRequestEvent;

		public event PlayFabResultEvent<WriteEventResponse> OnWriteTitleEventResultEvent;

		public event PlayFabErrorEvent OnGlobalErrorEvent;

		private PlayFabEvents()
		{
		}

		public static PlayFabEvents Init()
		{
			if (_instance == null)
			{
				_instance = new PlayFabEvents();
			}
			PlayFabHttp.ApiProcessingEventHandler += _instance.OnProcessingEvent;
			PlayFabHttp.ApiProcessingErrorEventHandler += _instance.OnProcessingErrorEvent;
			return _instance;
		}

		public void UnregisterInstance(object instance)
		{
			if (this.OnLoginResultEvent != null)
			{
				Delegate[] invocationList = this.OnLoginResultEvent.GetInvocationList();
				foreach (Delegate @delegate in invocationList)
				{
					if (object.ReferenceEquals(@delegate.Target, instance))
					{
						OnLoginResultEvent -= (PlayFabResultEvent<LoginResult>)@delegate;
					}
				}
			}
			if (this.OnAcceptTradeRequestEvent != null)
			{
				Delegate[] invocationList2 = this.OnAcceptTradeRequestEvent.GetInvocationList();
				foreach (Delegate delegate2 in invocationList2)
				{
					if (object.ReferenceEquals(delegate2.Target, instance))
					{
						OnAcceptTradeRequestEvent -= (PlayFabRequestEvent<AcceptTradeRequest>)delegate2;
					}
				}
			}
			if (this.OnAcceptTradeResultEvent != null)
			{
				Delegate[] invocationList3 = this.OnAcceptTradeResultEvent.GetInvocationList();
				foreach (Delegate delegate3 in invocationList3)
				{
					if (object.ReferenceEquals(delegate3.Target, instance))
					{
						OnAcceptTradeResultEvent -= (PlayFabResultEvent<AcceptTradeResponse>)delegate3;
					}
				}
			}
			if (this.OnAddFriendRequestEvent != null)
			{
				Delegate[] invocationList4 = this.OnAddFriendRequestEvent.GetInvocationList();
				foreach (Delegate delegate4 in invocationList4)
				{
					if (object.ReferenceEquals(delegate4.Target, instance))
					{
						OnAddFriendRequestEvent -= (PlayFabRequestEvent<AddFriendRequest>)delegate4;
					}
				}
			}
			if (this.OnAddFriendResultEvent != null)
			{
				Delegate[] invocationList5 = this.OnAddFriendResultEvent.GetInvocationList();
				foreach (Delegate delegate5 in invocationList5)
				{
					if (object.ReferenceEquals(delegate5.Target, instance))
					{
						OnAddFriendResultEvent -= (PlayFabResultEvent<AddFriendResult>)delegate5;
					}
				}
			}
			if (this.OnAddGenericIDRequestEvent != null)
			{
				Delegate[] invocationList6 = this.OnAddGenericIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate6 in invocationList6)
				{
					if (object.ReferenceEquals(delegate6.Target, instance))
					{
						OnAddGenericIDRequestEvent -= (PlayFabRequestEvent<AddGenericIDRequest>)delegate6;
					}
				}
			}
			if (this.OnAddGenericIDResultEvent != null)
			{
				Delegate[] invocationList7 = this.OnAddGenericIDResultEvent.GetInvocationList();
				foreach (Delegate delegate7 in invocationList7)
				{
					if (object.ReferenceEquals(delegate7.Target, instance))
					{
						OnAddGenericIDResultEvent -= (PlayFabResultEvent<AddGenericIDResult>)delegate7;
					}
				}
			}
			if (this.OnAddOrUpdateContactEmailRequestEvent != null)
			{
				Delegate[] invocationList8 = this.OnAddOrUpdateContactEmailRequestEvent.GetInvocationList();
				foreach (Delegate delegate8 in invocationList8)
				{
					if (object.ReferenceEquals(delegate8.Target, instance))
					{
						OnAddOrUpdateContactEmailRequestEvent -= (PlayFabRequestEvent<AddOrUpdateContactEmailRequest>)delegate8;
					}
				}
			}
			if (this.OnAddOrUpdateContactEmailResultEvent != null)
			{
				Delegate[] invocationList9 = this.OnAddOrUpdateContactEmailResultEvent.GetInvocationList();
				foreach (Delegate delegate9 in invocationList9)
				{
					if (object.ReferenceEquals(delegate9.Target, instance))
					{
						OnAddOrUpdateContactEmailResultEvent -= (PlayFabResultEvent<AddOrUpdateContactEmailResult>)delegate9;
					}
				}
			}
			if (this.OnAddSharedGroupMembersRequestEvent != null)
			{
				Delegate[] invocationList10 = this.OnAddSharedGroupMembersRequestEvent.GetInvocationList();
				foreach (Delegate delegate10 in invocationList10)
				{
					if (object.ReferenceEquals(delegate10.Target, instance))
					{
						OnAddSharedGroupMembersRequestEvent -= (PlayFabRequestEvent<AddSharedGroupMembersRequest>)delegate10;
					}
				}
			}
			if (this.OnAddSharedGroupMembersResultEvent != null)
			{
				Delegate[] invocationList11 = this.OnAddSharedGroupMembersResultEvent.GetInvocationList();
				foreach (Delegate delegate11 in invocationList11)
				{
					if (object.ReferenceEquals(delegate11.Target, instance))
					{
						OnAddSharedGroupMembersResultEvent -= (PlayFabResultEvent<AddSharedGroupMembersResult>)delegate11;
					}
				}
			}
			if (this.OnAddUsernamePasswordRequestEvent != null)
			{
				Delegate[] invocationList12 = this.OnAddUsernamePasswordRequestEvent.GetInvocationList();
				foreach (Delegate delegate12 in invocationList12)
				{
					if (object.ReferenceEquals(delegate12.Target, instance))
					{
						OnAddUsernamePasswordRequestEvent -= (PlayFabRequestEvent<AddUsernamePasswordRequest>)delegate12;
					}
				}
			}
			if (this.OnAddUsernamePasswordResultEvent != null)
			{
				Delegate[] invocationList13 = this.OnAddUsernamePasswordResultEvent.GetInvocationList();
				foreach (Delegate delegate13 in invocationList13)
				{
					if (object.ReferenceEquals(delegate13.Target, instance))
					{
						OnAddUsernamePasswordResultEvent -= (PlayFabResultEvent<AddUsernamePasswordResult>)delegate13;
					}
				}
			}
			if (this.OnAddUserVirtualCurrencyRequestEvent != null)
			{
				Delegate[] invocationList14 = this.OnAddUserVirtualCurrencyRequestEvent.GetInvocationList();
				foreach (Delegate delegate14 in invocationList14)
				{
					if (object.ReferenceEquals(delegate14.Target, instance))
					{
						OnAddUserVirtualCurrencyRequestEvent -= (PlayFabRequestEvent<AddUserVirtualCurrencyRequest>)delegate14;
					}
				}
			}
			if (this.OnAddUserVirtualCurrencyResultEvent != null)
			{
				Delegate[] invocationList15 = this.OnAddUserVirtualCurrencyResultEvent.GetInvocationList();
				foreach (Delegate delegate15 in invocationList15)
				{
					if (object.ReferenceEquals(delegate15.Target, instance))
					{
						OnAddUserVirtualCurrencyResultEvent -= (PlayFabResultEvent<ModifyUserVirtualCurrencyResult>)delegate15;
					}
				}
			}
			if (this.OnAndroidDevicePushNotificationRegistrationRequestEvent != null)
			{
				Delegate[] invocationList16 = this.OnAndroidDevicePushNotificationRegistrationRequestEvent.GetInvocationList();
				foreach (Delegate delegate16 in invocationList16)
				{
					if (object.ReferenceEquals(delegate16.Target, instance))
					{
						OnAndroidDevicePushNotificationRegistrationRequestEvent -= (PlayFabRequestEvent<AndroidDevicePushNotificationRegistrationRequest>)delegate16;
					}
				}
			}
			if (this.OnAndroidDevicePushNotificationRegistrationResultEvent != null)
			{
				Delegate[] invocationList17 = this.OnAndroidDevicePushNotificationRegistrationResultEvent.GetInvocationList();
				foreach (Delegate delegate17 in invocationList17)
				{
					if (object.ReferenceEquals(delegate17.Target, instance))
					{
						OnAndroidDevicePushNotificationRegistrationResultEvent -= (PlayFabResultEvent<AndroidDevicePushNotificationRegistrationResult>)delegate17;
					}
				}
			}
			if (this.OnAttributeInstallRequestEvent != null)
			{
				Delegate[] invocationList18 = this.OnAttributeInstallRequestEvent.GetInvocationList();
				foreach (Delegate delegate18 in invocationList18)
				{
					if (object.ReferenceEquals(delegate18.Target, instance))
					{
						OnAttributeInstallRequestEvent -= (PlayFabRequestEvent<AttributeInstallRequest>)delegate18;
					}
				}
			}
			if (this.OnAttributeInstallResultEvent != null)
			{
				Delegate[] invocationList19 = this.OnAttributeInstallResultEvent.GetInvocationList();
				foreach (Delegate delegate19 in invocationList19)
				{
					if (object.ReferenceEquals(delegate19.Target, instance))
					{
						OnAttributeInstallResultEvent -= (PlayFabResultEvent<AttributeInstallResult>)delegate19;
					}
				}
			}
			if (this.OnCancelTradeRequestEvent != null)
			{
				Delegate[] invocationList20 = this.OnCancelTradeRequestEvent.GetInvocationList();
				foreach (Delegate delegate20 in invocationList20)
				{
					if (object.ReferenceEquals(delegate20.Target, instance))
					{
						OnCancelTradeRequestEvent -= (PlayFabRequestEvent<CancelTradeRequest>)delegate20;
					}
				}
			}
			if (this.OnCancelTradeResultEvent != null)
			{
				Delegate[] invocationList21 = this.OnCancelTradeResultEvent.GetInvocationList();
				foreach (Delegate delegate21 in invocationList21)
				{
					if (object.ReferenceEquals(delegate21.Target, instance))
					{
						OnCancelTradeResultEvent -= (PlayFabResultEvent<CancelTradeResponse>)delegate21;
					}
				}
			}
			if (this.OnConfirmPurchaseRequestEvent != null)
			{
				Delegate[] invocationList22 = this.OnConfirmPurchaseRequestEvent.GetInvocationList();
				foreach (Delegate delegate22 in invocationList22)
				{
					if (object.ReferenceEquals(delegate22.Target, instance))
					{
						OnConfirmPurchaseRequestEvent -= (PlayFabRequestEvent<ConfirmPurchaseRequest>)delegate22;
					}
				}
			}
			if (this.OnConfirmPurchaseResultEvent != null)
			{
				Delegate[] invocationList23 = this.OnConfirmPurchaseResultEvent.GetInvocationList();
				foreach (Delegate delegate23 in invocationList23)
				{
					if (object.ReferenceEquals(delegate23.Target, instance))
					{
						OnConfirmPurchaseResultEvent -= (PlayFabResultEvent<ConfirmPurchaseResult>)delegate23;
					}
				}
			}
			if (this.OnConsumeItemRequestEvent != null)
			{
				Delegate[] invocationList24 = this.OnConsumeItemRequestEvent.GetInvocationList();
				foreach (Delegate delegate24 in invocationList24)
				{
					if (object.ReferenceEquals(delegate24.Target, instance))
					{
						OnConsumeItemRequestEvent -= (PlayFabRequestEvent<ConsumeItemRequest>)delegate24;
					}
				}
			}
			if (this.OnConsumeItemResultEvent != null)
			{
				Delegate[] invocationList25 = this.OnConsumeItemResultEvent.GetInvocationList();
				foreach (Delegate delegate25 in invocationList25)
				{
					if (object.ReferenceEquals(delegate25.Target, instance))
					{
						OnConsumeItemResultEvent -= (PlayFabResultEvent<ConsumeItemResult>)delegate25;
					}
				}
			}
			if (this.OnCreateSharedGroupRequestEvent != null)
			{
				Delegate[] invocationList26 = this.OnCreateSharedGroupRequestEvent.GetInvocationList();
				foreach (Delegate delegate26 in invocationList26)
				{
					if (object.ReferenceEquals(delegate26.Target, instance))
					{
						OnCreateSharedGroupRequestEvent -= (PlayFabRequestEvent<CreateSharedGroupRequest>)delegate26;
					}
				}
			}
			if (this.OnCreateSharedGroupResultEvent != null)
			{
				Delegate[] invocationList27 = this.OnCreateSharedGroupResultEvent.GetInvocationList();
				foreach (Delegate delegate27 in invocationList27)
				{
					if (object.ReferenceEquals(delegate27.Target, instance))
					{
						OnCreateSharedGroupResultEvent -= (PlayFabResultEvent<CreateSharedGroupResult>)delegate27;
					}
				}
			}
			if (this.OnExecuteCloudScriptRequestEvent != null)
			{
				Delegate[] invocationList28 = this.OnExecuteCloudScriptRequestEvent.GetInvocationList();
				foreach (Delegate delegate28 in invocationList28)
				{
					if (object.ReferenceEquals(delegate28.Target, instance))
					{
						OnExecuteCloudScriptRequestEvent -= (PlayFabRequestEvent<ExecuteCloudScriptRequest>)delegate28;
					}
				}
			}
			if (this.OnExecuteCloudScriptResultEvent != null)
			{
				Delegate[] invocationList29 = this.OnExecuteCloudScriptResultEvent.GetInvocationList();
				foreach (Delegate delegate29 in invocationList29)
				{
					if (object.ReferenceEquals(delegate29.Target, instance))
					{
						OnExecuteCloudScriptResultEvent -= (PlayFabResultEvent<ExecuteCloudScriptResult>)delegate29;
					}
				}
			}
			if (this.OnGetAccountInfoRequestEvent != null)
			{
				Delegate[] invocationList30 = this.OnGetAccountInfoRequestEvent.GetInvocationList();
				foreach (Delegate delegate30 in invocationList30)
				{
					if (object.ReferenceEquals(delegate30.Target, instance))
					{
						OnGetAccountInfoRequestEvent -= (PlayFabRequestEvent<GetAccountInfoRequest>)delegate30;
					}
				}
			}
			if (this.OnGetAccountInfoResultEvent != null)
			{
				Delegate[] invocationList31 = this.OnGetAccountInfoResultEvent.GetInvocationList();
				foreach (Delegate delegate31 in invocationList31)
				{
					if (object.ReferenceEquals(delegate31.Target, instance))
					{
						OnGetAccountInfoResultEvent -= (PlayFabResultEvent<GetAccountInfoResult>)delegate31;
					}
				}
			}
			if (this.OnGetAllUsersCharactersRequestEvent != null)
			{
				Delegate[] invocationList32 = this.OnGetAllUsersCharactersRequestEvent.GetInvocationList();
				foreach (Delegate delegate32 in invocationList32)
				{
					if (object.ReferenceEquals(delegate32.Target, instance))
					{
						OnGetAllUsersCharactersRequestEvent -= (PlayFabRequestEvent<ListUsersCharactersRequest>)delegate32;
					}
				}
			}
			if (this.OnGetAllUsersCharactersResultEvent != null)
			{
				Delegate[] invocationList33 = this.OnGetAllUsersCharactersResultEvent.GetInvocationList();
				foreach (Delegate delegate33 in invocationList33)
				{
					if (object.ReferenceEquals(delegate33.Target, instance))
					{
						OnGetAllUsersCharactersResultEvent -= (PlayFabResultEvent<ListUsersCharactersResult>)delegate33;
					}
				}
			}
			if (this.OnGetCatalogItemsRequestEvent != null)
			{
				Delegate[] invocationList34 = this.OnGetCatalogItemsRequestEvent.GetInvocationList();
				foreach (Delegate delegate34 in invocationList34)
				{
					if (object.ReferenceEquals(delegate34.Target, instance))
					{
						OnGetCatalogItemsRequestEvent -= (PlayFabRequestEvent<GetCatalogItemsRequest>)delegate34;
					}
				}
			}
			if (this.OnGetCatalogItemsResultEvent != null)
			{
				Delegate[] invocationList35 = this.OnGetCatalogItemsResultEvent.GetInvocationList();
				foreach (Delegate delegate35 in invocationList35)
				{
					if (object.ReferenceEquals(delegate35.Target, instance))
					{
						OnGetCatalogItemsResultEvent -= (PlayFabResultEvent<GetCatalogItemsResult>)delegate35;
					}
				}
			}
			if (this.OnGetCharacterDataRequestEvent != null)
			{
				Delegate[] invocationList36 = this.OnGetCharacterDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate36 in invocationList36)
				{
					if (object.ReferenceEquals(delegate36.Target, instance))
					{
						OnGetCharacterDataRequestEvent -= (PlayFabRequestEvent<GetCharacterDataRequest>)delegate36;
					}
				}
			}
			if (this.OnGetCharacterDataResultEvent != null)
			{
				Delegate[] invocationList37 = this.OnGetCharacterDataResultEvent.GetInvocationList();
				foreach (Delegate delegate37 in invocationList37)
				{
					if (object.ReferenceEquals(delegate37.Target, instance))
					{
						OnGetCharacterDataResultEvent -= (PlayFabResultEvent<GetCharacterDataResult>)delegate37;
					}
				}
			}
			if (this.OnGetCharacterInventoryRequestEvent != null)
			{
				Delegate[] invocationList38 = this.OnGetCharacterInventoryRequestEvent.GetInvocationList();
				foreach (Delegate delegate38 in invocationList38)
				{
					if (object.ReferenceEquals(delegate38.Target, instance))
					{
						OnGetCharacterInventoryRequestEvent -= (PlayFabRequestEvent<GetCharacterInventoryRequest>)delegate38;
					}
				}
			}
			if (this.OnGetCharacterInventoryResultEvent != null)
			{
				Delegate[] invocationList39 = this.OnGetCharacterInventoryResultEvent.GetInvocationList();
				foreach (Delegate delegate39 in invocationList39)
				{
					if (object.ReferenceEquals(delegate39.Target, instance))
					{
						OnGetCharacterInventoryResultEvent -= (PlayFabResultEvent<GetCharacterInventoryResult>)delegate39;
					}
				}
			}
			if (this.OnGetCharacterLeaderboardRequestEvent != null)
			{
				Delegate[] invocationList40 = this.OnGetCharacterLeaderboardRequestEvent.GetInvocationList();
				foreach (Delegate delegate40 in invocationList40)
				{
					if (object.ReferenceEquals(delegate40.Target, instance))
					{
						OnGetCharacterLeaderboardRequestEvent -= (PlayFabRequestEvent<GetCharacterLeaderboardRequest>)delegate40;
					}
				}
			}
			if (this.OnGetCharacterLeaderboardResultEvent != null)
			{
				Delegate[] invocationList41 = this.OnGetCharacterLeaderboardResultEvent.GetInvocationList();
				foreach (Delegate delegate41 in invocationList41)
				{
					if (object.ReferenceEquals(delegate41.Target, instance))
					{
						OnGetCharacterLeaderboardResultEvent -= (PlayFabResultEvent<GetCharacterLeaderboardResult>)delegate41;
					}
				}
			}
			if (this.OnGetCharacterReadOnlyDataRequestEvent != null)
			{
				Delegate[] invocationList42 = this.OnGetCharacterReadOnlyDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate42 in invocationList42)
				{
					if (object.ReferenceEquals(delegate42.Target, instance))
					{
						OnGetCharacterReadOnlyDataRequestEvent -= (PlayFabRequestEvent<GetCharacterDataRequest>)delegate42;
					}
				}
			}
			if (this.OnGetCharacterReadOnlyDataResultEvent != null)
			{
				Delegate[] invocationList43 = this.OnGetCharacterReadOnlyDataResultEvent.GetInvocationList();
				foreach (Delegate delegate43 in invocationList43)
				{
					if (object.ReferenceEquals(delegate43.Target, instance))
					{
						OnGetCharacterReadOnlyDataResultEvent -= (PlayFabResultEvent<GetCharacterDataResult>)delegate43;
					}
				}
			}
			if (this.OnGetCharacterStatisticsRequestEvent != null)
			{
				Delegate[] invocationList44 = this.OnGetCharacterStatisticsRequestEvent.GetInvocationList();
				foreach (Delegate delegate44 in invocationList44)
				{
					if (object.ReferenceEquals(delegate44.Target, instance))
					{
						OnGetCharacterStatisticsRequestEvent -= (PlayFabRequestEvent<GetCharacterStatisticsRequest>)delegate44;
					}
				}
			}
			if (this.OnGetCharacterStatisticsResultEvent != null)
			{
				Delegate[] invocationList45 = this.OnGetCharacterStatisticsResultEvent.GetInvocationList();
				foreach (Delegate delegate45 in invocationList45)
				{
					if (object.ReferenceEquals(delegate45.Target, instance))
					{
						OnGetCharacterStatisticsResultEvent -= (PlayFabResultEvent<GetCharacterStatisticsResult>)delegate45;
					}
				}
			}
			if (this.OnGetContentDownloadUrlRequestEvent != null)
			{
				Delegate[] invocationList46 = this.OnGetContentDownloadUrlRequestEvent.GetInvocationList();
				foreach (Delegate delegate46 in invocationList46)
				{
					if (object.ReferenceEquals(delegate46.Target, instance))
					{
						OnGetContentDownloadUrlRequestEvent -= (PlayFabRequestEvent<GetContentDownloadUrlRequest>)delegate46;
					}
				}
			}
			if (this.OnGetContentDownloadUrlResultEvent != null)
			{
				Delegate[] invocationList47 = this.OnGetContentDownloadUrlResultEvent.GetInvocationList();
				foreach (Delegate delegate47 in invocationList47)
				{
					if (object.ReferenceEquals(delegate47.Target, instance))
					{
						OnGetContentDownloadUrlResultEvent -= (PlayFabResultEvent<GetContentDownloadUrlResult>)delegate47;
					}
				}
			}
			if (this.OnGetCurrentGamesRequestEvent != null)
			{
				Delegate[] invocationList48 = this.OnGetCurrentGamesRequestEvent.GetInvocationList();
				foreach (Delegate delegate48 in invocationList48)
				{
					if (object.ReferenceEquals(delegate48.Target, instance))
					{
						OnGetCurrentGamesRequestEvent -= (PlayFabRequestEvent<CurrentGamesRequest>)delegate48;
					}
				}
			}
			if (this.OnGetCurrentGamesResultEvent != null)
			{
				Delegate[] invocationList49 = this.OnGetCurrentGamesResultEvent.GetInvocationList();
				foreach (Delegate delegate49 in invocationList49)
				{
					if (object.ReferenceEquals(delegate49.Target, instance))
					{
						OnGetCurrentGamesResultEvent -= (PlayFabResultEvent<CurrentGamesResult>)delegate49;
					}
				}
			}
			if (this.OnGetFriendLeaderboardRequestEvent != null)
			{
				Delegate[] invocationList50 = this.OnGetFriendLeaderboardRequestEvent.GetInvocationList();
				foreach (Delegate delegate50 in invocationList50)
				{
					if (object.ReferenceEquals(delegate50.Target, instance))
					{
						OnGetFriendLeaderboardRequestEvent -= (PlayFabRequestEvent<GetFriendLeaderboardRequest>)delegate50;
					}
				}
			}
			if (this.OnGetFriendLeaderboardResultEvent != null)
			{
				Delegate[] invocationList51 = this.OnGetFriendLeaderboardResultEvent.GetInvocationList();
				foreach (Delegate delegate51 in invocationList51)
				{
					if (object.ReferenceEquals(delegate51.Target, instance))
					{
						OnGetFriendLeaderboardResultEvent -= (PlayFabResultEvent<GetLeaderboardResult>)delegate51;
					}
				}
			}
			if (this.OnGetFriendLeaderboardAroundPlayerRequestEvent != null)
			{
				Delegate[] invocationList52 = this.OnGetFriendLeaderboardAroundPlayerRequestEvent.GetInvocationList();
				foreach (Delegate delegate52 in invocationList52)
				{
					if (object.ReferenceEquals(delegate52.Target, instance))
					{
						OnGetFriendLeaderboardAroundPlayerRequestEvent -= (PlayFabRequestEvent<GetFriendLeaderboardAroundPlayerRequest>)delegate52;
					}
				}
			}
			if (this.OnGetFriendLeaderboardAroundPlayerResultEvent != null)
			{
				Delegate[] invocationList53 = this.OnGetFriendLeaderboardAroundPlayerResultEvent.GetInvocationList();
				foreach (Delegate delegate53 in invocationList53)
				{
					if (object.ReferenceEquals(delegate53.Target, instance))
					{
						OnGetFriendLeaderboardAroundPlayerResultEvent -= (PlayFabResultEvent<GetFriendLeaderboardAroundPlayerResult>)delegate53;
					}
				}
			}
			if (this.OnGetFriendsListRequestEvent != null)
			{
				Delegate[] invocationList54 = this.OnGetFriendsListRequestEvent.GetInvocationList();
				foreach (Delegate delegate54 in invocationList54)
				{
					if (object.ReferenceEquals(delegate54.Target, instance))
					{
						OnGetFriendsListRequestEvent -= (PlayFabRequestEvent<GetFriendsListRequest>)delegate54;
					}
				}
			}
			if (this.OnGetFriendsListResultEvent != null)
			{
				Delegate[] invocationList55 = this.OnGetFriendsListResultEvent.GetInvocationList();
				foreach (Delegate delegate55 in invocationList55)
				{
					if (object.ReferenceEquals(delegate55.Target, instance))
					{
						OnGetFriendsListResultEvent -= (PlayFabResultEvent<GetFriendsListResult>)delegate55;
					}
				}
			}
			if (this.OnGetGameServerRegionsRequestEvent != null)
			{
				Delegate[] invocationList56 = this.OnGetGameServerRegionsRequestEvent.GetInvocationList();
				foreach (Delegate delegate56 in invocationList56)
				{
					if (object.ReferenceEquals(delegate56.Target, instance))
					{
						OnGetGameServerRegionsRequestEvent -= (PlayFabRequestEvent<GameServerRegionsRequest>)delegate56;
					}
				}
			}
			if (this.OnGetGameServerRegionsResultEvent != null)
			{
				Delegate[] invocationList57 = this.OnGetGameServerRegionsResultEvent.GetInvocationList();
				foreach (Delegate delegate57 in invocationList57)
				{
					if (object.ReferenceEquals(delegate57.Target, instance))
					{
						OnGetGameServerRegionsResultEvent -= (PlayFabResultEvent<GameServerRegionsResult>)delegate57;
					}
				}
			}
			if (this.OnGetLeaderboardRequestEvent != null)
			{
				Delegate[] invocationList58 = this.OnGetLeaderboardRequestEvent.GetInvocationList();
				foreach (Delegate delegate58 in invocationList58)
				{
					if (object.ReferenceEquals(delegate58.Target, instance))
					{
						OnGetLeaderboardRequestEvent -= (PlayFabRequestEvent<GetLeaderboardRequest>)delegate58;
					}
				}
			}
			if (this.OnGetLeaderboardResultEvent != null)
			{
				Delegate[] invocationList59 = this.OnGetLeaderboardResultEvent.GetInvocationList();
				foreach (Delegate delegate59 in invocationList59)
				{
					if (object.ReferenceEquals(delegate59.Target, instance))
					{
						OnGetLeaderboardResultEvent -= (PlayFabResultEvent<GetLeaderboardResult>)delegate59;
					}
				}
			}
			if (this.OnGetLeaderboardAroundCharacterRequestEvent != null)
			{
				Delegate[] invocationList60 = this.OnGetLeaderboardAroundCharacterRequestEvent.GetInvocationList();
				foreach (Delegate delegate60 in invocationList60)
				{
					if (object.ReferenceEquals(delegate60.Target, instance))
					{
						OnGetLeaderboardAroundCharacterRequestEvent -= (PlayFabRequestEvent<GetLeaderboardAroundCharacterRequest>)delegate60;
					}
				}
			}
			if (this.OnGetLeaderboardAroundCharacterResultEvent != null)
			{
				Delegate[] invocationList61 = this.OnGetLeaderboardAroundCharacterResultEvent.GetInvocationList();
				foreach (Delegate delegate61 in invocationList61)
				{
					if (object.ReferenceEquals(delegate61.Target, instance))
					{
						OnGetLeaderboardAroundCharacterResultEvent -= (PlayFabResultEvent<GetLeaderboardAroundCharacterResult>)delegate61;
					}
				}
			}
			if (this.OnGetLeaderboardAroundPlayerRequestEvent != null)
			{
				Delegate[] invocationList62 = this.OnGetLeaderboardAroundPlayerRequestEvent.GetInvocationList();
				foreach (Delegate delegate62 in invocationList62)
				{
					if (object.ReferenceEquals(delegate62.Target, instance))
					{
						OnGetLeaderboardAroundPlayerRequestEvent -= (PlayFabRequestEvent<GetLeaderboardAroundPlayerRequest>)delegate62;
					}
				}
			}
			if (this.OnGetLeaderboardAroundPlayerResultEvent != null)
			{
				Delegate[] invocationList63 = this.OnGetLeaderboardAroundPlayerResultEvent.GetInvocationList();
				foreach (Delegate delegate63 in invocationList63)
				{
					if (object.ReferenceEquals(delegate63.Target, instance))
					{
						OnGetLeaderboardAroundPlayerResultEvent -= (PlayFabResultEvent<GetLeaderboardAroundPlayerResult>)delegate63;
					}
				}
			}
			if (this.OnGetLeaderboardForUserCharactersRequestEvent != null)
			{
				Delegate[] invocationList64 = this.OnGetLeaderboardForUserCharactersRequestEvent.GetInvocationList();
				foreach (Delegate delegate64 in invocationList64)
				{
					if (object.ReferenceEquals(delegate64.Target, instance))
					{
						OnGetLeaderboardForUserCharactersRequestEvent -= (PlayFabRequestEvent<GetLeaderboardForUsersCharactersRequest>)delegate64;
					}
				}
			}
			if (this.OnGetLeaderboardForUserCharactersResultEvent != null)
			{
				Delegate[] invocationList65 = this.OnGetLeaderboardForUserCharactersResultEvent.GetInvocationList();
				foreach (Delegate delegate65 in invocationList65)
				{
					if (object.ReferenceEquals(delegate65.Target, instance))
					{
						OnGetLeaderboardForUserCharactersResultEvent -= (PlayFabResultEvent<GetLeaderboardForUsersCharactersResult>)delegate65;
					}
				}
			}
			if (this.OnGetPaymentTokenRequestEvent != null)
			{
				Delegate[] invocationList66 = this.OnGetPaymentTokenRequestEvent.GetInvocationList();
				foreach (Delegate delegate66 in invocationList66)
				{
					if (object.ReferenceEquals(delegate66.Target, instance))
					{
						OnGetPaymentTokenRequestEvent -= (PlayFabRequestEvent<GetPaymentTokenRequest>)delegate66;
					}
				}
			}
			if (this.OnGetPaymentTokenResultEvent != null)
			{
				Delegate[] invocationList67 = this.OnGetPaymentTokenResultEvent.GetInvocationList();
				foreach (Delegate delegate67 in invocationList67)
				{
					if (object.ReferenceEquals(delegate67.Target, instance))
					{
						OnGetPaymentTokenResultEvent -= (PlayFabResultEvent<GetPaymentTokenResult>)delegate67;
					}
				}
			}
			if (this.OnGetPhotonAuthenticationTokenRequestEvent != null)
			{
				Delegate[] invocationList68 = this.OnGetPhotonAuthenticationTokenRequestEvent.GetInvocationList();
				foreach (Delegate delegate68 in invocationList68)
				{
					if (object.ReferenceEquals(delegate68.Target, instance))
					{
						OnGetPhotonAuthenticationTokenRequestEvent -= (PlayFabRequestEvent<GetPhotonAuthenticationTokenRequest>)delegate68;
					}
				}
			}
			if (this.OnGetPhotonAuthenticationTokenResultEvent != null)
			{
				Delegate[] invocationList69 = this.OnGetPhotonAuthenticationTokenResultEvent.GetInvocationList();
				foreach (Delegate delegate69 in invocationList69)
				{
					if (object.ReferenceEquals(delegate69.Target, instance))
					{
						OnGetPhotonAuthenticationTokenResultEvent -= (PlayFabResultEvent<GetPhotonAuthenticationTokenResult>)delegate69;
					}
				}
			}
			if (this.OnGetPlayerCombinedInfoRequestEvent != null)
			{
				Delegate[] invocationList70 = this.OnGetPlayerCombinedInfoRequestEvent.GetInvocationList();
				foreach (Delegate delegate70 in invocationList70)
				{
					if (object.ReferenceEquals(delegate70.Target, instance))
					{
						OnGetPlayerCombinedInfoRequestEvent -= (PlayFabRequestEvent<GetPlayerCombinedInfoRequest>)delegate70;
					}
				}
			}
			if (this.OnGetPlayerCombinedInfoResultEvent != null)
			{
				Delegate[] invocationList71 = this.OnGetPlayerCombinedInfoResultEvent.GetInvocationList();
				foreach (Delegate delegate71 in invocationList71)
				{
					if (object.ReferenceEquals(delegate71.Target, instance))
					{
						OnGetPlayerCombinedInfoResultEvent -= (PlayFabResultEvent<GetPlayerCombinedInfoResult>)delegate71;
					}
				}
			}
			if (this.OnGetPlayerProfileRequestEvent != null)
			{
				Delegate[] invocationList72 = this.OnGetPlayerProfileRequestEvent.GetInvocationList();
				foreach (Delegate delegate72 in invocationList72)
				{
					if (object.ReferenceEquals(delegate72.Target, instance))
					{
						OnGetPlayerProfileRequestEvent -= (PlayFabRequestEvent<GetPlayerProfileRequest>)delegate72;
					}
				}
			}
			if (this.OnGetPlayerProfileResultEvent != null)
			{
				Delegate[] invocationList73 = this.OnGetPlayerProfileResultEvent.GetInvocationList();
				foreach (Delegate delegate73 in invocationList73)
				{
					if (object.ReferenceEquals(delegate73.Target, instance))
					{
						OnGetPlayerProfileResultEvent -= (PlayFabResultEvent<GetPlayerProfileResult>)delegate73;
					}
				}
			}
			if (this.OnGetPlayerSegmentsRequestEvent != null)
			{
				Delegate[] invocationList74 = this.OnGetPlayerSegmentsRequestEvent.GetInvocationList();
				foreach (Delegate delegate74 in invocationList74)
				{
					if (object.ReferenceEquals(delegate74.Target, instance))
					{
						OnGetPlayerSegmentsRequestEvent -= (PlayFabRequestEvent<GetPlayerSegmentsRequest>)delegate74;
					}
				}
			}
			if (this.OnGetPlayerSegmentsResultEvent != null)
			{
				Delegate[] invocationList75 = this.OnGetPlayerSegmentsResultEvent.GetInvocationList();
				foreach (Delegate delegate75 in invocationList75)
				{
					if (object.ReferenceEquals(delegate75.Target, instance))
					{
						OnGetPlayerSegmentsResultEvent -= (PlayFabResultEvent<GetPlayerSegmentsResult>)delegate75;
					}
				}
			}
			if (this.OnGetPlayerStatisticsRequestEvent != null)
			{
				Delegate[] invocationList76 = this.OnGetPlayerStatisticsRequestEvent.GetInvocationList();
				foreach (Delegate delegate76 in invocationList76)
				{
					if (object.ReferenceEquals(delegate76.Target, instance))
					{
						OnGetPlayerStatisticsRequestEvent -= (PlayFabRequestEvent<GetPlayerStatisticsRequest>)delegate76;
					}
				}
			}
			if (this.OnGetPlayerStatisticsResultEvent != null)
			{
				Delegate[] invocationList77 = this.OnGetPlayerStatisticsResultEvent.GetInvocationList();
				foreach (Delegate delegate77 in invocationList77)
				{
					if (object.ReferenceEquals(delegate77.Target, instance))
					{
						OnGetPlayerStatisticsResultEvent -= (PlayFabResultEvent<GetPlayerStatisticsResult>)delegate77;
					}
				}
			}
			if (this.OnGetPlayerStatisticVersionsRequestEvent != null)
			{
				Delegate[] invocationList78 = this.OnGetPlayerStatisticVersionsRequestEvent.GetInvocationList();
				foreach (Delegate delegate78 in invocationList78)
				{
					if (object.ReferenceEquals(delegate78.Target, instance))
					{
						OnGetPlayerStatisticVersionsRequestEvent -= (PlayFabRequestEvent<GetPlayerStatisticVersionsRequest>)delegate78;
					}
				}
			}
			if (this.OnGetPlayerStatisticVersionsResultEvent != null)
			{
				Delegate[] invocationList79 = this.OnGetPlayerStatisticVersionsResultEvent.GetInvocationList();
				foreach (Delegate delegate79 in invocationList79)
				{
					if (object.ReferenceEquals(delegate79.Target, instance))
					{
						OnGetPlayerStatisticVersionsResultEvent -= (PlayFabResultEvent<GetPlayerStatisticVersionsResult>)delegate79;
					}
				}
			}
			if (this.OnGetPlayerTagsRequestEvent != null)
			{
				Delegate[] invocationList80 = this.OnGetPlayerTagsRequestEvent.GetInvocationList();
				foreach (Delegate delegate80 in invocationList80)
				{
					if (object.ReferenceEquals(delegate80.Target, instance))
					{
						OnGetPlayerTagsRequestEvent -= (PlayFabRequestEvent<GetPlayerTagsRequest>)delegate80;
					}
				}
			}
			if (this.OnGetPlayerTagsResultEvent != null)
			{
				Delegate[] invocationList81 = this.OnGetPlayerTagsResultEvent.GetInvocationList();
				foreach (Delegate delegate81 in invocationList81)
				{
					if (object.ReferenceEquals(delegate81.Target, instance))
					{
						OnGetPlayerTagsResultEvent -= (PlayFabResultEvent<GetPlayerTagsResult>)delegate81;
					}
				}
			}
			if (this.OnGetPlayerTradesRequestEvent != null)
			{
				Delegate[] invocationList82 = this.OnGetPlayerTradesRequestEvent.GetInvocationList();
				foreach (Delegate delegate82 in invocationList82)
				{
					if (object.ReferenceEquals(delegate82.Target, instance))
					{
						OnGetPlayerTradesRequestEvent -= (PlayFabRequestEvent<GetPlayerTradesRequest>)delegate82;
					}
				}
			}
			if (this.OnGetPlayerTradesResultEvent != null)
			{
				Delegate[] invocationList83 = this.OnGetPlayerTradesResultEvent.GetInvocationList();
				foreach (Delegate delegate83 in invocationList83)
				{
					if (object.ReferenceEquals(delegate83.Target, instance))
					{
						OnGetPlayerTradesResultEvent -= (PlayFabResultEvent<GetPlayerTradesResponse>)delegate83;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromFacebookIDsRequestEvent != null)
			{
				Delegate[] invocationList84 = this.OnGetPlayFabIDsFromFacebookIDsRequestEvent.GetInvocationList();
				foreach (Delegate delegate84 in invocationList84)
				{
					if (object.ReferenceEquals(delegate84.Target, instance))
					{
						OnGetPlayFabIDsFromFacebookIDsRequestEvent -= (PlayFabRequestEvent<GetPlayFabIDsFromFacebookIDsRequest>)delegate84;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromFacebookIDsResultEvent != null)
			{
				Delegate[] invocationList85 = this.OnGetPlayFabIDsFromFacebookIDsResultEvent.GetInvocationList();
				foreach (Delegate delegate85 in invocationList85)
				{
					if (object.ReferenceEquals(delegate85.Target, instance))
					{
						OnGetPlayFabIDsFromFacebookIDsResultEvent -= (PlayFabResultEvent<GetPlayFabIDsFromFacebookIDsResult>)delegate85;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromGameCenterIDsRequestEvent != null)
			{
				Delegate[] invocationList86 = this.OnGetPlayFabIDsFromGameCenterIDsRequestEvent.GetInvocationList();
				foreach (Delegate delegate86 in invocationList86)
				{
					if (object.ReferenceEquals(delegate86.Target, instance))
					{
						OnGetPlayFabIDsFromGameCenterIDsRequestEvent -= (PlayFabRequestEvent<GetPlayFabIDsFromGameCenterIDsRequest>)delegate86;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromGameCenterIDsResultEvent != null)
			{
				Delegate[] invocationList87 = this.OnGetPlayFabIDsFromGameCenterIDsResultEvent.GetInvocationList();
				foreach (Delegate delegate87 in invocationList87)
				{
					if (object.ReferenceEquals(delegate87.Target, instance))
					{
						OnGetPlayFabIDsFromGameCenterIDsResultEvent -= (PlayFabResultEvent<GetPlayFabIDsFromGameCenterIDsResult>)delegate87;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromGenericIDsRequestEvent != null)
			{
				Delegate[] invocationList88 = this.OnGetPlayFabIDsFromGenericIDsRequestEvent.GetInvocationList();
				foreach (Delegate delegate88 in invocationList88)
				{
					if (object.ReferenceEquals(delegate88.Target, instance))
					{
						OnGetPlayFabIDsFromGenericIDsRequestEvent -= (PlayFabRequestEvent<GetPlayFabIDsFromGenericIDsRequest>)delegate88;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromGenericIDsResultEvent != null)
			{
				Delegate[] invocationList89 = this.OnGetPlayFabIDsFromGenericIDsResultEvent.GetInvocationList();
				foreach (Delegate delegate89 in invocationList89)
				{
					if (object.ReferenceEquals(delegate89.Target, instance))
					{
						OnGetPlayFabIDsFromGenericIDsResultEvent -= (PlayFabResultEvent<GetPlayFabIDsFromGenericIDsResult>)delegate89;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromGoogleIDsRequestEvent != null)
			{
				Delegate[] invocationList90 = this.OnGetPlayFabIDsFromGoogleIDsRequestEvent.GetInvocationList();
				foreach (Delegate delegate90 in invocationList90)
				{
					if (object.ReferenceEquals(delegate90.Target, instance))
					{
						OnGetPlayFabIDsFromGoogleIDsRequestEvent -= (PlayFabRequestEvent<GetPlayFabIDsFromGoogleIDsRequest>)delegate90;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromGoogleIDsResultEvent != null)
			{
				Delegate[] invocationList91 = this.OnGetPlayFabIDsFromGoogleIDsResultEvent.GetInvocationList();
				foreach (Delegate delegate91 in invocationList91)
				{
					if (object.ReferenceEquals(delegate91.Target, instance))
					{
						OnGetPlayFabIDsFromGoogleIDsResultEvent -= (PlayFabResultEvent<GetPlayFabIDsFromGoogleIDsResult>)delegate91;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromKongregateIDsRequestEvent != null)
			{
				Delegate[] invocationList92 = this.OnGetPlayFabIDsFromKongregateIDsRequestEvent.GetInvocationList();
				foreach (Delegate delegate92 in invocationList92)
				{
					if (object.ReferenceEquals(delegate92.Target, instance))
					{
						OnGetPlayFabIDsFromKongregateIDsRequestEvent -= (PlayFabRequestEvent<GetPlayFabIDsFromKongregateIDsRequest>)delegate92;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromKongregateIDsResultEvent != null)
			{
				Delegate[] invocationList93 = this.OnGetPlayFabIDsFromKongregateIDsResultEvent.GetInvocationList();
				foreach (Delegate delegate93 in invocationList93)
				{
					if (object.ReferenceEquals(delegate93.Target, instance))
					{
						OnGetPlayFabIDsFromKongregateIDsResultEvent -= (PlayFabResultEvent<GetPlayFabIDsFromKongregateIDsResult>)delegate93;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromSteamIDsRequestEvent != null)
			{
				Delegate[] invocationList94 = this.OnGetPlayFabIDsFromSteamIDsRequestEvent.GetInvocationList();
				foreach (Delegate delegate94 in invocationList94)
				{
					if (object.ReferenceEquals(delegate94.Target, instance))
					{
						OnGetPlayFabIDsFromSteamIDsRequestEvent -= (PlayFabRequestEvent<GetPlayFabIDsFromSteamIDsRequest>)delegate94;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromSteamIDsResultEvent != null)
			{
				Delegate[] invocationList95 = this.OnGetPlayFabIDsFromSteamIDsResultEvent.GetInvocationList();
				foreach (Delegate delegate95 in invocationList95)
				{
					if (object.ReferenceEquals(delegate95.Target, instance))
					{
						OnGetPlayFabIDsFromSteamIDsResultEvent -= (PlayFabResultEvent<GetPlayFabIDsFromSteamIDsResult>)delegate95;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromTwitchIDsRequestEvent != null)
			{
				Delegate[] invocationList96 = this.OnGetPlayFabIDsFromTwitchIDsRequestEvent.GetInvocationList();
				foreach (Delegate delegate96 in invocationList96)
				{
					if (object.ReferenceEquals(delegate96.Target, instance))
					{
						OnGetPlayFabIDsFromTwitchIDsRequestEvent -= (PlayFabRequestEvent<GetPlayFabIDsFromTwitchIDsRequest>)delegate96;
					}
				}
			}
			if (this.OnGetPlayFabIDsFromTwitchIDsResultEvent != null)
			{
				Delegate[] invocationList97 = this.OnGetPlayFabIDsFromTwitchIDsResultEvent.GetInvocationList();
				foreach (Delegate delegate97 in invocationList97)
				{
					if (object.ReferenceEquals(delegate97.Target, instance))
					{
						OnGetPlayFabIDsFromTwitchIDsResultEvent -= (PlayFabResultEvent<GetPlayFabIDsFromTwitchIDsResult>)delegate97;
					}
				}
			}
			if (this.OnGetPublisherDataRequestEvent != null)
			{
				Delegate[] invocationList98 = this.OnGetPublisherDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate98 in invocationList98)
				{
					if (object.ReferenceEquals(delegate98.Target, instance))
					{
						OnGetPublisherDataRequestEvent -= (PlayFabRequestEvent<GetPublisherDataRequest>)delegate98;
					}
				}
			}
			if (this.OnGetPublisherDataResultEvent != null)
			{
				Delegate[] invocationList99 = this.OnGetPublisherDataResultEvent.GetInvocationList();
				foreach (Delegate delegate99 in invocationList99)
				{
					if (object.ReferenceEquals(delegate99.Target, instance))
					{
						OnGetPublisherDataResultEvent -= (PlayFabResultEvent<GetPublisherDataResult>)delegate99;
					}
				}
			}
			if (this.OnGetPurchaseRequestEvent != null)
			{
				Delegate[] invocationList100 = this.OnGetPurchaseRequestEvent.GetInvocationList();
				foreach (Delegate delegate100 in invocationList100)
				{
					if (object.ReferenceEquals(delegate100.Target, instance))
					{
						OnGetPurchaseRequestEvent -= (PlayFabRequestEvent<GetPurchaseRequest>)delegate100;
					}
				}
			}
			if (this.OnGetPurchaseResultEvent != null)
			{
				Delegate[] invocationList101 = this.OnGetPurchaseResultEvent.GetInvocationList();
				foreach (Delegate delegate101 in invocationList101)
				{
					if (object.ReferenceEquals(delegate101.Target, instance))
					{
						OnGetPurchaseResultEvent -= (PlayFabResultEvent<GetPurchaseResult>)delegate101;
					}
				}
			}
			if (this.OnGetSharedGroupDataRequestEvent != null)
			{
				Delegate[] invocationList102 = this.OnGetSharedGroupDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate102 in invocationList102)
				{
					if (object.ReferenceEquals(delegate102.Target, instance))
					{
						OnGetSharedGroupDataRequestEvent -= (PlayFabRequestEvent<GetSharedGroupDataRequest>)delegate102;
					}
				}
			}
			if (this.OnGetSharedGroupDataResultEvent != null)
			{
				Delegate[] invocationList103 = this.OnGetSharedGroupDataResultEvent.GetInvocationList();
				foreach (Delegate delegate103 in invocationList103)
				{
					if (object.ReferenceEquals(delegate103.Target, instance))
					{
						OnGetSharedGroupDataResultEvent -= (PlayFabResultEvent<GetSharedGroupDataResult>)delegate103;
					}
				}
			}
			if (this.OnGetStoreItemsRequestEvent != null)
			{
				Delegate[] invocationList104 = this.OnGetStoreItemsRequestEvent.GetInvocationList();
				foreach (Delegate delegate104 in invocationList104)
				{
					if (object.ReferenceEquals(delegate104.Target, instance))
					{
						OnGetStoreItemsRequestEvent -= (PlayFabRequestEvent<GetStoreItemsRequest>)delegate104;
					}
				}
			}
			if (this.OnGetStoreItemsResultEvent != null)
			{
				Delegate[] invocationList105 = this.OnGetStoreItemsResultEvent.GetInvocationList();
				foreach (Delegate delegate105 in invocationList105)
				{
					if (object.ReferenceEquals(delegate105.Target, instance))
					{
						OnGetStoreItemsResultEvent -= (PlayFabResultEvent<GetStoreItemsResult>)delegate105;
					}
				}
			}
			if (this.OnGetTimeRequestEvent != null)
			{
				Delegate[] invocationList106 = this.OnGetTimeRequestEvent.GetInvocationList();
				foreach (Delegate delegate106 in invocationList106)
				{
					if (object.ReferenceEquals(delegate106.Target, instance))
					{
						OnGetTimeRequestEvent -= (PlayFabRequestEvent<GetTimeRequest>)delegate106;
					}
				}
			}
			if (this.OnGetTimeResultEvent != null)
			{
				Delegate[] invocationList107 = this.OnGetTimeResultEvent.GetInvocationList();
				foreach (Delegate delegate107 in invocationList107)
				{
					if (object.ReferenceEquals(delegate107.Target, instance))
					{
						OnGetTimeResultEvent -= (PlayFabResultEvent<GetTimeResult>)delegate107;
					}
				}
			}
			if (this.OnGetTitleDataRequestEvent != null)
			{
				Delegate[] invocationList108 = this.OnGetTitleDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate108 in invocationList108)
				{
					if (object.ReferenceEquals(delegate108.Target, instance))
					{
						OnGetTitleDataRequestEvent -= (PlayFabRequestEvent<GetTitleDataRequest>)delegate108;
					}
				}
			}
			if (this.OnGetTitleDataResultEvent != null)
			{
				Delegate[] invocationList109 = this.OnGetTitleDataResultEvent.GetInvocationList();
				foreach (Delegate delegate109 in invocationList109)
				{
					if (object.ReferenceEquals(delegate109.Target, instance))
					{
						OnGetTitleDataResultEvent -= (PlayFabResultEvent<GetTitleDataResult>)delegate109;
					}
				}
			}
			if (this.OnGetTitleNewsRequestEvent != null)
			{
				Delegate[] invocationList110 = this.OnGetTitleNewsRequestEvent.GetInvocationList();
				foreach (Delegate delegate110 in invocationList110)
				{
					if (object.ReferenceEquals(delegate110.Target, instance))
					{
						OnGetTitleNewsRequestEvent -= (PlayFabRequestEvent<GetTitleNewsRequest>)delegate110;
					}
				}
			}
			if (this.OnGetTitleNewsResultEvent != null)
			{
				Delegate[] invocationList111 = this.OnGetTitleNewsResultEvent.GetInvocationList();
				foreach (Delegate delegate111 in invocationList111)
				{
					if (object.ReferenceEquals(delegate111.Target, instance))
					{
						OnGetTitleNewsResultEvent -= (PlayFabResultEvent<GetTitleNewsResult>)delegate111;
					}
				}
			}
			if (this.OnGetTitlePublicKeyRequestEvent != null)
			{
				Delegate[] invocationList112 = this.OnGetTitlePublicKeyRequestEvent.GetInvocationList();
				foreach (Delegate delegate112 in invocationList112)
				{
					if (object.ReferenceEquals(delegate112.Target, instance))
					{
						OnGetTitlePublicKeyRequestEvent -= (PlayFabRequestEvent<GetTitlePublicKeyRequest>)delegate112;
					}
				}
			}
			if (this.OnGetTitlePublicKeyResultEvent != null)
			{
				Delegate[] invocationList113 = this.OnGetTitlePublicKeyResultEvent.GetInvocationList();
				foreach (Delegate delegate113 in invocationList113)
				{
					if (object.ReferenceEquals(delegate113.Target, instance))
					{
						OnGetTitlePublicKeyResultEvent -= (PlayFabResultEvent<GetTitlePublicKeyResult>)delegate113;
					}
				}
			}
			if (this.OnGetTradeStatusRequestEvent != null)
			{
				Delegate[] invocationList114 = this.OnGetTradeStatusRequestEvent.GetInvocationList();
				foreach (Delegate delegate114 in invocationList114)
				{
					if (object.ReferenceEquals(delegate114.Target, instance))
					{
						OnGetTradeStatusRequestEvent -= (PlayFabRequestEvent<GetTradeStatusRequest>)delegate114;
					}
				}
			}
			if (this.OnGetTradeStatusResultEvent != null)
			{
				Delegate[] invocationList115 = this.OnGetTradeStatusResultEvent.GetInvocationList();
				foreach (Delegate delegate115 in invocationList115)
				{
					if (object.ReferenceEquals(delegate115.Target, instance))
					{
						OnGetTradeStatusResultEvent -= (PlayFabResultEvent<GetTradeStatusResponse>)delegate115;
					}
				}
			}
			if (this.OnGetUserDataRequestEvent != null)
			{
				Delegate[] invocationList116 = this.OnGetUserDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate116 in invocationList116)
				{
					if (object.ReferenceEquals(delegate116.Target, instance))
					{
						OnGetUserDataRequestEvent -= (PlayFabRequestEvent<GetUserDataRequest>)delegate116;
					}
				}
			}
			if (this.OnGetUserDataResultEvent != null)
			{
				Delegate[] invocationList117 = this.OnGetUserDataResultEvent.GetInvocationList();
				foreach (Delegate delegate117 in invocationList117)
				{
					if (object.ReferenceEquals(delegate117.Target, instance))
					{
						OnGetUserDataResultEvent -= (PlayFabResultEvent<GetUserDataResult>)delegate117;
					}
				}
			}
			if (this.OnGetUserInventoryRequestEvent != null)
			{
				Delegate[] invocationList118 = this.OnGetUserInventoryRequestEvent.GetInvocationList();
				foreach (Delegate delegate118 in invocationList118)
				{
					if (object.ReferenceEquals(delegate118.Target, instance))
					{
						OnGetUserInventoryRequestEvent -= (PlayFabRequestEvent<GetUserInventoryRequest>)delegate118;
					}
				}
			}
			if (this.OnGetUserInventoryResultEvent != null)
			{
				Delegate[] invocationList119 = this.OnGetUserInventoryResultEvent.GetInvocationList();
				foreach (Delegate delegate119 in invocationList119)
				{
					if (object.ReferenceEquals(delegate119.Target, instance))
					{
						OnGetUserInventoryResultEvent -= (PlayFabResultEvent<GetUserInventoryResult>)delegate119;
					}
				}
			}
			if (this.OnGetUserPublisherDataRequestEvent != null)
			{
				Delegate[] invocationList120 = this.OnGetUserPublisherDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate120 in invocationList120)
				{
					if (object.ReferenceEquals(delegate120.Target, instance))
					{
						OnGetUserPublisherDataRequestEvent -= (PlayFabRequestEvent<GetUserDataRequest>)delegate120;
					}
				}
			}
			if (this.OnGetUserPublisherDataResultEvent != null)
			{
				Delegate[] invocationList121 = this.OnGetUserPublisherDataResultEvent.GetInvocationList();
				foreach (Delegate delegate121 in invocationList121)
				{
					if (object.ReferenceEquals(delegate121.Target, instance))
					{
						OnGetUserPublisherDataResultEvent -= (PlayFabResultEvent<GetUserDataResult>)delegate121;
					}
				}
			}
			if (this.OnGetUserPublisherReadOnlyDataRequestEvent != null)
			{
				Delegate[] invocationList122 = this.OnGetUserPublisherReadOnlyDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate122 in invocationList122)
				{
					if (object.ReferenceEquals(delegate122.Target, instance))
					{
						OnGetUserPublisherReadOnlyDataRequestEvent -= (PlayFabRequestEvent<GetUserDataRequest>)delegate122;
					}
				}
			}
			if (this.OnGetUserPublisherReadOnlyDataResultEvent != null)
			{
				Delegate[] invocationList123 = this.OnGetUserPublisherReadOnlyDataResultEvent.GetInvocationList();
				foreach (Delegate delegate123 in invocationList123)
				{
					if (object.ReferenceEquals(delegate123.Target, instance))
					{
						OnGetUserPublisherReadOnlyDataResultEvent -= (PlayFabResultEvent<GetUserDataResult>)delegate123;
					}
				}
			}
			if (this.OnGetUserReadOnlyDataRequestEvent != null)
			{
				Delegate[] invocationList124 = this.OnGetUserReadOnlyDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate124 in invocationList124)
				{
					if (object.ReferenceEquals(delegate124.Target, instance))
					{
						OnGetUserReadOnlyDataRequestEvent -= (PlayFabRequestEvent<GetUserDataRequest>)delegate124;
					}
				}
			}
			if (this.OnGetUserReadOnlyDataResultEvent != null)
			{
				Delegate[] invocationList125 = this.OnGetUserReadOnlyDataResultEvent.GetInvocationList();
				foreach (Delegate delegate125 in invocationList125)
				{
					if (object.ReferenceEquals(delegate125.Target, instance))
					{
						OnGetUserReadOnlyDataResultEvent -= (PlayFabResultEvent<GetUserDataResult>)delegate125;
					}
				}
			}
			if (this.OnGetWindowsHelloChallengeRequestEvent != null)
			{
				Delegate[] invocationList126 = this.OnGetWindowsHelloChallengeRequestEvent.GetInvocationList();
				foreach (Delegate delegate126 in invocationList126)
				{
					if (object.ReferenceEquals(delegate126.Target, instance))
					{
						OnGetWindowsHelloChallengeRequestEvent -= (PlayFabRequestEvent<GetWindowsHelloChallengeRequest>)delegate126;
					}
				}
			}
			if (this.OnGetWindowsHelloChallengeResultEvent != null)
			{
				Delegate[] invocationList127 = this.OnGetWindowsHelloChallengeResultEvent.GetInvocationList();
				foreach (Delegate delegate127 in invocationList127)
				{
					if (object.ReferenceEquals(delegate127.Target, instance))
					{
						OnGetWindowsHelloChallengeResultEvent -= (PlayFabResultEvent<GetWindowsHelloChallengeResponse>)delegate127;
					}
				}
			}
			if (this.OnGrantCharacterToUserRequestEvent != null)
			{
				Delegate[] invocationList128 = this.OnGrantCharacterToUserRequestEvent.GetInvocationList();
				foreach (Delegate delegate128 in invocationList128)
				{
					if (object.ReferenceEquals(delegate128.Target, instance))
					{
						OnGrantCharacterToUserRequestEvent -= (PlayFabRequestEvent<GrantCharacterToUserRequest>)delegate128;
					}
				}
			}
			if (this.OnGrantCharacterToUserResultEvent != null)
			{
				Delegate[] invocationList129 = this.OnGrantCharacterToUserResultEvent.GetInvocationList();
				foreach (Delegate delegate129 in invocationList129)
				{
					if (object.ReferenceEquals(delegate129.Target, instance))
					{
						OnGrantCharacterToUserResultEvent -= (PlayFabResultEvent<GrantCharacterToUserResult>)delegate129;
					}
				}
			}
			if (this.OnLinkAndroidDeviceIDRequestEvent != null)
			{
				Delegate[] invocationList130 = this.OnLinkAndroidDeviceIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate130 in invocationList130)
				{
					if (object.ReferenceEquals(delegate130.Target, instance))
					{
						OnLinkAndroidDeviceIDRequestEvent -= (PlayFabRequestEvent<LinkAndroidDeviceIDRequest>)delegate130;
					}
				}
			}
			if (this.OnLinkAndroidDeviceIDResultEvent != null)
			{
				Delegate[] invocationList131 = this.OnLinkAndroidDeviceIDResultEvent.GetInvocationList();
				foreach (Delegate delegate131 in invocationList131)
				{
					if (object.ReferenceEquals(delegate131.Target, instance))
					{
						OnLinkAndroidDeviceIDResultEvent -= (PlayFabResultEvent<LinkAndroidDeviceIDResult>)delegate131;
					}
				}
			}
			if (this.OnLinkCustomIDRequestEvent != null)
			{
				Delegate[] invocationList132 = this.OnLinkCustomIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate132 in invocationList132)
				{
					if (object.ReferenceEquals(delegate132.Target, instance))
					{
						OnLinkCustomIDRequestEvent -= (PlayFabRequestEvent<LinkCustomIDRequest>)delegate132;
					}
				}
			}
			if (this.OnLinkCustomIDResultEvent != null)
			{
				Delegate[] invocationList133 = this.OnLinkCustomIDResultEvent.GetInvocationList();
				foreach (Delegate delegate133 in invocationList133)
				{
					if (object.ReferenceEquals(delegate133.Target, instance))
					{
						OnLinkCustomIDResultEvent -= (PlayFabResultEvent<LinkCustomIDResult>)delegate133;
					}
				}
			}
			if (this.OnLinkFacebookAccountRequestEvent != null)
			{
				Delegate[] invocationList134 = this.OnLinkFacebookAccountRequestEvent.GetInvocationList();
				foreach (Delegate delegate134 in invocationList134)
				{
					if (object.ReferenceEquals(delegate134.Target, instance))
					{
						OnLinkFacebookAccountRequestEvent -= (PlayFabRequestEvent<LinkFacebookAccountRequest>)delegate134;
					}
				}
			}
			if (this.OnLinkFacebookAccountResultEvent != null)
			{
				Delegate[] invocationList135 = this.OnLinkFacebookAccountResultEvent.GetInvocationList();
				foreach (Delegate delegate135 in invocationList135)
				{
					if (object.ReferenceEquals(delegate135.Target, instance))
					{
						OnLinkFacebookAccountResultEvent -= (PlayFabResultEvent<LinkFacebookAccountResult>)delegate135;
					}
				}
			}
			if (this.OnLinkGameCenterAccountRequestEvent != null)
			{
				Delegate[] invocationList136 = this.OnLinkGameCenterAccountRequestEvent.GetInvocationList();
				foreach (Delegate delegate136 in invocationList136)
				{
					if (object.ReferenceEquals(delegate136.Target, instance))
					{
						OnLinkGameCenterAccountRequestEvent -= (PlayFabRequestEvent<LinkGameCenterAccountRequest>)delegate136;
					}
				}
			}
			if (this.OnLinkGameCenterAccountResultEvent != null)
			{
				Delegate[] invocationList137 = this.OnLinkGameCenterAccountResultEvent.GetInvocationList();
				foreach (Delegate delegate137 in invocationList137)
				{
					if (object.ReferenceEquals(delegate137.Target, instance))
					{
						OnLinkGameCenterAccountResultEvent -= (PlayFabResultEvent<LinkGameCenterAccountResult>)delegate137;
					}
				}
			}
			if (this.OnLinkGoogleAccountRequestEvent != null)
			{
				Delegate[] invocationList138 = this.OnLinkGoogleAccountRequestEvent.GetInvocationList();
				foreach (Delegate delegate138 in invocationList138)
				{
					if (object.ReferenceEquals(delegate138.Target, instance))
					{
						OnLinkGoogleAccountRequestEvent -= (PlayFabRequestEvent<LinkGoogleAccountRequest>)delegate138;
					}
				}
			}
			if (this.OnLinkGoogleAccountResultEvent != null)
			{
				Delegate[] invocationList139 = this.OnLinkGoogleAccountResultEvent.GetInvocationList();
				foreach (Delegate delegate139 in invocationList139)
				{
					if (object.ReferenceEquals(delegate139.Target, instance))
					{
						OnLinkGoogleAccountResultEvent -= (PlayFabResultEvent<LinkGoogleAccountResult>)delegate139;
					}
				}
			}
			if (this.OnLinkIOSDeviceIDRequestEvent != null)
			{
				Delegate[] invocationList140 = this.OnLinkIOSDeviceIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate140 in invocationList140)
				{
					if (object.ReferenceEquals(delegate140.Target, instance))
					{
						OnLinkIOSDeviceIDRequestEvent -= (PlayFabRequestEvent<LinkIOSDeviceIDRequest>)delegate140;
					}
				}
			}
			if (this.OnLinkIOSDeviceIDResultEvent != null)
			{
				Delegate[] invocationList141 = this.OnLinkIOSDeviceIDResultEvent.GetInvocationList();
				foreach (Delegate delegate141 in invocationList141)
				{
					if (object.ReferenceEquals(delegate141.Target, instance))
					{
						OnLinkIOSDeviceIDResultEvent -= (PlayFabResultEvent<LinkIOSDeviceIDResult>)delegate141;
					}
				}
			}
			if (this.OnLinkKongregateRequestEvent != null)
			{
				Delegate[] invocationList142 = this.OnLinkKongregateRequestEvent.GetInvocationList();
				foreach (Delegate delegate142 in invocationList142)
				{
					if (object.ReferenceEquals(delegate142.Target, instance))
					{
						OnLinkKongregateRequestEvent -= (PlayFabRequestEvent<LinkKongregateAccountRequest>)delegate142;
					}
				}
			}
			if (this.OnLinkKongregateResultEvent != null)
			{
				Delegate[] invocationList143 = this.OnLinkKongregateResultEvent.GetInvocationList();
				foreach (Delegate delegate143 in invocationList143)
				{
					if (object.ReferenceEquals(delegate143.Target, instance))
					{
						OnLinkKongregateResultEvent -= (PlayFabResultEvent<LinkKongregateAccountResult>)delegate143;
					}
				}
			}
			if (this.OnLinkSteamAccountRequestEvent != null)
			{
				Delegate[] invocationList144 = this.OnLinkSteamAccountRequestEvent.GetInvocationList();
				foreach (Delegate delegate144 in invocationList144)
				{
					if (object.ReferenceEquals(delegate144.Target, instance))
					{
						OnLinkSteamAccountRequestEvent -= (PlayFabRequestEvent<LinkSteamAccountRequest>)delegate144;
					}
				}
			}
			if (this.OnLinkSteamAccountResultEvent != null)
			{
				Delegate[] invocationList145 = this.OnLinkSteamAccountResultEvent.GetInvocationList();
				foreach (Delegate delegate145 in invocationList145)
				{
					if (object.ReferenceEquals(delegate145.Target, instance))
					{
						OnLinkSteamAccountResultEvent -= (PlayFabResultEvent<LinkSteamAccountResult>)delegate145;
					}
				}
			}
			if (this.OnLinkTwitchRequestEvent != null)
			{
				Delegate[] invocationList146 = this.OnLinkTwitchRequestEvent.GetInvocationList();
				foreach (Delegate delegate146 in invocationList146)
				{
					if (object.ReferenceEquals(delegate146.Target, instance))
					{
						OnLinkTwitchRequestEvent -= (PlayFabRequestEvent<LinkTwitchAccountRequest>)delegate146;
					}
				}
			}
			if (this.OnLinkTwitchResultEvent != null)
			{
				Delegate[] invocationList147 = this.OnLinkTwitchResultEvent.GetInvocationList();
				foreach (Delegate delegate147 in invocationList147)
				{
					if (object.ReferenceEquals(delegate147.Target, instance))
					{
						OnLinkTwitchResultEvent -= (PlayFabResultEvent<LinkTwitchAccountResult>)delegate147;
					}
				}
			}
			if (this.OnLinkWindowsHelloRequestEvent != null)
			{
				Delegate[] invocationList148 = this.OnLinkWindowsHelloRequestEvent.GetInvocationList();
				foreach (Delegate delegate148 in invocationList148)
				{
					if (object.ReferenceEquals(delegate148.Target, instance))
					{
						OnLinkWindowsHelloRequestEvent -= (PlayFabRequestEvent<LinkWindowsHelloAccountRequest>)delegate148;
					}
				}
			}
			if (this.OnLinkWindowsHelloResultEvent != null)
			{
				Delegate[] invocationList149 = this.OnLinkWindowsHelloResultEvent.GetInvocationList();
				foreach (Delegate delegate149 in invocationList149)
				{
					if (object.ReferenceEquals(delegate149.Target, instance))
					{
						OnLinkWindowsHelloResultEvent -= (PlayFabResultEvent<LinkWindowsHelloAccountResponse>)delegate149;
					}
				}
			}
			if (this.OnLoginWithAndroidDeviceIDRequestEvent != null)
			{
				Delegate[] invocationList150 = this.OnLoginWithAndroidDeviceIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate150 in invocationList150)
				{
					if (object.ReferenceEquals(delegate150.Target, instance))
					{
						OnLoginWithAndroidDeviceIDRequestEvent -= (PlayFabRequestEvent<LoginWithAndroidDeviceIDRequest>)delegate150;
					}
				}
			}
			if (this.OnLoginWithCustomIDRequestEvent != null)
			{
				Delegate[] invocationList151 = this.OnLoginWithCustomIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate151 in invocationList151)
				{
					if (object.ReferenceEquals(delegate151.Target, instance))
					{
						OnLoginWithCustomIDRequestEvent -= (PlayFabRequestEvent<LoginWithCustomIDRequest>)delegate151;
					}
				}
			}
			if (this.OnLoginWithEmailAddressRequestEvent != null)
			{
				Delegate[] invocationList152 = this.OnLoginWithEmailAddressRequestEvent.GetInvocationList();
				foreach (Delegate delegate152 in invocationList152)
				{
					if (object.ReferenceEquals(delegate152.Target, instance))
					{
						OnLoginWithEmailAddressRequestEvent -= (PlayFabRequestEvent<LoginWithEmailAddressRequest>)delegate152;
					}
				}
			}
			if (this.OnLoginWithFacebookRequestEvent != null)
			{
				Delegate[] invocationList153 = this.OnLoginWithFacebookRequestEvent.GetInvocationList();
				foreach (Delegate delegate153 in invocationList153)
				{
					if (object.ReferenceEquals(delegate153.Target, instance))
					{
						OnLoginWithFacebookRequestEvent -= (PlayFabRequestEvent<LoginWithFacebookRequest>)delegate153;
					}
				}
			}
			if (this.OnLoginWithGameCenterRequestEvent != null)
			{
				Delegate[] invocationList154 = this.OnLoginWithGameCenterRequestEvent.GetInvocationList();
				foreach (Delegate delegate154 in invocationList154)
				{
					if (object.ReferenceEquals(delegate154.Target, instance))
					{
						OnLoginWithGameCenterRequestEvent -= (PlayFabRequestEvent<LoginWithGameCenterRequest>)delegate154;
					}
				}
			}
			if (this.OnLoginWithGoogleAccountRequestEvent != null)
			{
				Delegate[] invocationList155 = this.OnLoginWithGoogleAccountRequestEvent.GetInvocationList();
				foreach (Delegate delegate155 in invocationList155)
				{
					if (object.ReferenceEquals(delegate155.Target, instance))
					{
						OnLoginWithGoogleAccountRequestEvent -= (PlayFabRequestEvent<LoginWithGoogleAccountRequest>)delegate155;
					}
				}
			}
			if (this.OnLoginWithIOSDeviceIDRequestEvent != null)
			{
				Delegate[] invocationList156 = this.OnLoginWithIOSDeviceIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate156 in invocationList156)
				{
					if (object.ReferenceEquals(delegate156.Target, instance))
					{
						OnLoginWithIOSDeviceIDRequestEvent -= (PlayFabRequestEvent<LoginWithIOSDeviceIDRequest>)delegate156;
					}
				}
			}
			if (this.OnLoginWithKongregateRequestEvent != null)
			{
				Delegate[] invocationList157 = this.OnLoginWithKongregateRequestEvent.GetInvocationList();
				foreach (Delegate delegate157 in invocationList157)
				{
					if (object.ReferenceEquals(delegate157.Target, instance))
					{
						OnLoginWithKongregateRequestEvent -= (PlayFabRequestEvent<LoginWithKongregateRequest>)delegate157;
					}
				}
			}
			if (this.OnLoginWithPlayFabRequestEvent != null)
			{
				Delegate[] invocationList158 = this.OnLoginWithPlayFabRequestEvent.GetInvocationList();
				foreach (Delegate delegate158 in invocationList158)
				{
					if (object.ReferenceEquals(delegate158.Target, instance))
					{
						OnLoginWithPlayFabRequestEvent -= (PlayFabRequestEvent<LoginWithPlayFabRequest>)delegate158;
					}
				}
			}
			if (this.OnLoginWithSteamRequestEvent != null)
			{
				Delegate[] invocationList159 = this.OnLoginWithSteamRequestEvent.GetInvocationList();
				foreach (Delegate delegate159 in invocationList159)
				{
					if (object.ReferenceEquals(delegate159.Target, instance))
					{
						OnLoginWithSteamRequestEvent -= (PlayFabRequestEvent<LoginWithSteamRequest>)delegate159;
					}
				}
			}
			if (this.OnLoginWithTwitchRequestEvent != null)
			{
				Delegate[] invocationList160 = this.OnLoginWithTwitchRequestEvent.GetInvocationList();
				foreach (Delegate delegate160 in invocationList160)
				{
					if (object.ReferenceEquals(delegate160.Target, instance))
					{
						OnLoginWithTwitchRequestEvent -= (PlayFabRequestEvent<LoginWithTwitchRequest>)delegate160;
					}
				}
			}
			if (this.OnLoginWithWindowsHelloRequestEvent != null)
			{
				Delegate[] invocationList161 = this.OnLoginWithWindowsHelloRequestEvent.GetInvocationList();
				foreach (Delegate delegate161 in invocationList161)
				{
					if (object.ReferenceEquals(delegate161.Target, instance))
					{
						OnLoginWithWindowsHelloRequestEvent -= (PlayFabRequestEvent<LoginWithWindowsHelloRequest>)delegate161;
					}
				}
			}
			if (this.OnMatchmakeRequestEvent != null)
			{
				Delegate[] invocationList162 = this.OnMatchmakeRequestEvent.GetInvocationList();
				foreach (Delegate delegate162 in invocationList162)
				{
					if (object.ReferenceEquals(delegate162.Target, instance))
					{
						OnMatchmakeRequestEvent -= (PlayFabRequestEvent<MatchmakeRequest>)delegate162;
					}
				}
			}
			if (this.OnMatchmakeResultEvent != null)
			{
				Delegate[] invocationList163 = this.OnMatchmakeResultEvent.GetInvocationList();
				foreach (Delegate delegate163 in invocationList163)
				{
					if (object.ReferenceEquals(delegate163.Target, instance))
					{
						OnMatchmakeResultEvent -= (PlayFabResultEvent<MatchmakeResult>)delegate163;
					}
				}
			}
			if (this.OnOpenTradeRequestEvent != null)
			{
				Delegate[] invocationList164 = this.OnOpenTradeRequestEvent.GetInvocationList();
				foreach (Delegate delegate164 in invocationList164)
				{
					if (object.ReferenceEquals(delegate164.Target, instance))
					{
						OnOpenTradeRequestEvent -= (PlayFabRequestEvent<OpenTradeRequest>)delegate164;
					}
				}
			}
			if (this.OnOpenTradeResultEvent != null)
			{
				Delegate[] invocationList165 = this.OnOpenTradeResultEvent.GetInvocationList();
				foreach (Delegate delegate165 in invocationList165)
				{
					if (object.ReferenceEquals(delegate165.Target, instance))
					{
						OnOpenTradeResultEvent -= (PlayFabResultEvent<OpenTradeResponse>)delegate165;
					}
				}
			}
			if (this.OnPayForPurchaseRequestEvent != null)
			{
				Delegate[] invocationList166 = this.OnPayForPurchaseRequestEvent.GetInvocationList();
				foreach (Delegate delegate166 in invocationList166)
				{
					if (object.ReferenceEquals(delegate166.Target, instance))
					{
						OnPayForPurchaseRequestEvent -= (PlayFabRequestEvent<PayForPurchaseRequest>)delegate166;
					}
				}
			}
			if (this.OnPayForPurchaseResultEvent != null)
			{
				Delegate[] invocationList167 = this.OnPayForPurchaseResultEvent.GetInvocationList();
				foreach (Delegate delegate167 in invocationList167)
				{
					if (object.ReferenceEquals(delegate167.Target, instance))
					{
						OnPayForPurchaseResultEvent -= (PlayFabResultEvent<PayForPurchaseResult>)delegate167;
					}
				}
			}
			if (this.OnPurchaseItemRequestEvent != null)
			{
				Delegate[] invocationList168 = this.OnPurchaseItemRequestEvent.GetInvocationList();
				foreach (Delegate delegate168 in invocationList168)
				{
					if (object.ReferenceEquals(delegate168.Target, instance))
					{
						OnPurchaseItemRequestEvent -= (PlayFabRequestEvent<PurchaseItemRequest>)delegate168;
					}
				}
			}
			if (this.OnPurchaseItemResultEvent != null)
			{
				Delegate[] invocationList169 = this.OnPurchaseItemResultEvent.GetInvocationList();
				foreach (Delegate delegate169 in invocationList169)
				{
					if (object.ReferenceEquals(delegate169.Target, instance))
					{
						OnPurchaseItemResultEvent -= (PlayFabResultEvent<PurchaseItemResult>)delegate169;
					}
				}
			}
			if (this.OnRedeemCouponRequestEvent != null)
			{
				Delegate[] invocationList170 = this.OnRedeemCouponRequestEvent.GetInvocationList();
				foreach (Delegate delegate170 in invocationList170)
				{
					if (object.ReferenceEquals(delegate170.Target, instance))
					{
						OnRedeemCouponRequestEvent -= (PlayFabRequestEvent<RedeemCouponRequest>)delegate170;
					}
				}
			}
			if (this.OnRedeemCouponResultEvent != null)
			{
				Delegate[] invocationList171 = this.OnRedeemCouponResultEvent.GetInvocationList();
				foreach (Delegate delegate171 in invocationList171)
				{
					if (object.ReferenceEquals(delegate171.Target, instance))
					{
						OnRedeemCouponResultEvent -= (PlayFabResultEvent<RedeemCouponResult>)delegate171;
					}
				}
			}
			if (this.OnRegisterForIOSPushNotificationRequestEvent != null)
			{
				Delegate[] invocationList172 = this.OnRegisterForIOSPushNotificationRequestEvent.GetInvocationList();
				foreach (Delegate delegate172 in invocationList172)
				{
					if (object.ReferenceEquals(delegate172.Target, instance))
					{
						OnRegisterForIOSPushNotificationRequestEvent -= (PlayFabRequestEvent<RegisterForIOSPushNotificationRequest>)delegate172;
					}
				}
			}
			if (this.OnRegisterForIOSPushNotificationResultEvent != null)
			{
				Delegate[] invocationList173 = this.OnRegisterForIOSPushNotificationResultEvent.GetInvocationList();
				foreach (Delegate delegate173 in invocationList173)
				{
					if (object.ReferenceEquals(delegate173.Target, instance))
					{
						OnRegisterForIOSPushNotificationResultEvent -= (PlayFabResultEvent<RegisterForIOSPushNotificationResult>)delegate173;
					}
				}
			}
			if (this.OnRegisterPlayFabUserRequestEvent != null)
			{
				Delegate[] invocationList174 = this.OnRegisterPlayFabUserRequestEvent.GetInvocationList();
				foreach (Delegate delegate174 in invocationList174)
				{
					if (object.ReferenceEquals(delegate174.Target, instance))
					{
						OnRegisterPlayFabUserRequestEvent -= (PlayFabRequestEvent<RegisterPlayFabUserRequest>)delegate174;
					}
				}
			}
			if (this.OnRegisterPlayFabUserResultEvent != null)
			{
				Delegate[] invocationList175 = this.OnRegisterPlayFabUserResultEvent.GetInvocationList();
				foreach (Delegate delegate175 in invocationList175)
				{
					if (object.ReferenceEquals(delegate175.Target, instance))
					{
						OnRegisterPlayFabUserResultEvent -= (PlayFabResultEvent<RegisterPlayFabUserResult>)delegate175;
					}
				}
			}
			if (this.OnRegisterWithWindowsHelloRequestEvent != null)
			{
				Delegate[] invocationList176 = this.OnRegisterWithWindowsHelloRequestEvent.GetInvocationList();
				foreach (Delegate delegate176 in invocationList176)
				{
					if (object.ReferenceEquals(delegate176.Target, instance))
					{
						OnRegisterWithWindowsHelloRequestEvent -= (PlayFabRequestEvent<RegisterWithWindowsHelloRequest>)delegate176;
					}
				}
			}
			if (this.OnRemoveContactEmailRequestEvent != null)
			{
				Delegate[] invocationList177 = this.OnRemoveContactEmailRequestEvent.GetInvocationList();
				foreach (Delegate delegate177 in invocationList177)
				{
					if (object.ReferenceEquals(delegate177.Target, instance))
					{
						OnRemoveContactEmailRequestEvent -= (PlayFabRequestEvent<RemoveContactEmailRequest>)delegate177;
					}
				}
			}
			if (this.OnRemoveContactEmailResultEvent != null)
			{
				Delegate[] invocationList178 = this.OnRemoveContactEmailResultEvent.GetInvocationList();
				foreach (Delegate delegate178 in invocationList178)
				{
					if (object.ReferenceEquals(delegate178.Target, instance))
					{
						OnRemoveContactEmailResultEvent -= (PlayFabResultEvent<RemoveContactEmailResult>)delegate178;
					}
				}
			}
			if (this.OnRemoveFriendRequestEvent != null)
			{
				Delegate[] invocationList179 = this.OnRemoveFriendRequestEvent.GetInvocationList();
				foreach (Delegate delegate179 in invocationList179)
				{
					if (object.ReferenceEquals(delegate179.Target, instance))
					{
						OnRemoveFriendRequestEvent -= (PlayFabRequestEvent<RemoveFriendRequest>)delegate179;
					}
				}
			}
			if (this.OnRemoveFriendResultEvent != null)
			{
				Delegate[] invocationList180 = this.OnRemoveFriendResultEvent.GetInvocationList();
				foreach (Delegate delegate180 in invocationList180)
				{
					if (object.ReferenceEquals(delegate180.Target, instance))
					{
						OnRemoveFriendResultEvent -= (PlayFabResultEvent<RemoveFriendResult>)delegate180;
					}
				}
			}
			if (this.OnRemoveGenericIDRequestEvent != null)
			{
				Delegate[] invocationList181 = this.OnRemoveGenericIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate181 in invocationList181)
				{
					if (object.ReferenceEquals(delegate181.Target, instance))
					{
						OnRemoveGenericIDRequestEvent -= (PlayFabRequestEvent<RemoveGenericIDRequest>)delegate181;
					}
				}
			}
			if (this.OnRemoveGenericIDResultEvent != null)
			{
				Delegate[] invocationList182 = this.OnRemoveGenericIDResultEvent.GetInvocationList();
				foreach (Delegate delegate182 in invocationList182)
				{
					if (object.ReferenceEquals(delegate182.Target, instance))
					{
						OnRemoveGenericIDResultEvent -= (PlayFabResultEvent<RemoveGenericIDResult>)delegate182;
					}
				}
			}
			if (this.OnRemoveSharedGroupMembersRequestEvent != null)
			{
				Delegate[] invocationList183 = this.OnRemoveSharedGroupMembersRequestEvent.GetInvocationList();
				foreach (Delegate delegate183 in invocationList183)
				{
					if (object.ReferenceEquals(delegate183.Target, instance))
					{
						OnRemoveSharedGroupMembersRequestEvent -= (PlayFabRequestEvent<RemoveSharedGroupMembersRequest>)delegate183;
					}
				}
			}
			if (this.OnRemoveSharedGroupMembersResultEvent != null)
			{
				Delegate[] invocationList184 = this.OnRemoveSharedGroupMembersResultEvent.GetInvocationList();
				foreach (Delegate delegate184 in invocationList184)
				{
					if (object.ReferenceEquals(delegate184.Target, instance))
					{
						OnRemoveSharedGroupMembersResultEvent -= (PlayFabResultEvent<RemoveSharedGroupMembersResult>)delegate184;
					}
				}
			}
			if (this.OnReportDeviceInfoRequestEvent != null)
			{
				Delegate[] invocationList185 = this.OnReportDeviceInfoRequestEvent.GetInvocationList();
				foreach (Delegate delegate185 in invocationList185)
				{
					if (object.ReferenceEquals(delegate185.Target, instance))
					{
						OnReportDeviceInfoRequestEvent -= (PlayFabRequestEvent<DeviceInfoRequest>)delegate185;
					}
				}
			}
			if (this.OnReportDeviceInfoResultEvent != null)
			{
				Delegate[] invocationList186 = this.OnReportDeviceInfoResultEvent.GetInvocationList();
				foreach (Delegate delegate186 in invocationList186)
				{
					if (object.ReferenceEquals(delegate186.Target, instance))
					{
						OnReportDeviceInfoResultEvent -= (PlayFabResultEvent<EmptyResult>)delegate186;
					}
				}
			}
			if (this.OnReportPlayerRequestEvent != null)
			{
				Delegate[] invocationList187 = this.OnReportPlayerRequestEvent.GetInvocationList();
				foreach (Delegate delegate187 in invocationList187)
				{
					if (object.ReferenceEquals(delegate187.Target, instance))
					{
						OnReportPlayerRequestEvent -= (PlayFabRequestEvent<ReportPlayerClientRequest>)delegate187;
					}
				}
			}
			if (this.OnReportPlayerResultEvent != null)
			{
				Delegate[] invocationList188 = this.OnReportPlayerResultEvent.GetInvocationList();
				foreach (Delegate delegate188 in invocationList188)
				{
					if (object.ReferenceEquals(delegate188.Target, instance))
					{
						OnReportPlayerResultEvent -= (PlayFabResultEvent<ReportPlayerClientResult>)delegate188;
					}
				}
			}
			if (this.OnRestoreIOSPurchasesRequestEvent != null)
			{
				Delegate[] invocationList189 = this.OnRestoreIOSPurchasesRequestEvent.GetInvocationList();
				foreach (Delegate delegate189 in invocationList189)
				{
					if (object.ReferenceEquals(delegate189.Target, instance))
					{
						OnRestoreIOSPurchasesRequestEvent -= (PlayFabRequestEvent<RestoreIOSPurchasesRequest>)delegate189;
					}
				}
			}
			if (this.OnRestoreIOSPurchasesResultEvent != null)
			{
				Delegate[] invocationList190 = this.OnRestoreIOSPurchasesResultEvent.GetInvocationList();
				foreach (Delegate delegate190 in invocationList190)
				{
					if (object.ReferenceEquals(delegate190.Target, instance))
					{
						OnRestoreIOSPurchasesResultEvent -= (PlayFabResultEvent<RestoreIOSPurchasesResult>)delegate190;
					}
				}
			}
			if (this.OnSendAccountRecoveryEmailRequestEvent != null)
			{
				Delegate[] invocationList191 = this.OnSendAccountRecoveryEmailRequestEvent.GetInvocationList();
				foreach (Delegate delegate191 in invocationList191)
				{
					if (object.ReferenceEquals(delegate191.Target, instance))
					{
						OnSendAccountRecoveryEmailRequestEvent -= (PlayFabRequestEvent<SendAccountRecoveryEmailRequest>)delegate191;
					}
				}
			}
			if (this.OnSendAccountRecoveryEmailResultEvent != null)
			{
				Delegate[] invocationList192 = this.OnSendAccountRecoveryEmailResultEvent.GetInvocationList();
				foreach (Delegate delegate192 in invocationList192)
				{
					if (object.ReferenceEquals(delegate192.Target, instance))
					{
						OnSendAccountRecoveryEmailResultEvent -= (PlayFabResultEvent<SendAccountRecoveryEmailResult>)delegate192;
					}
				}
			}
			if (this.OnSetFriendTagsRequestEvent != null)
			{
				Delegate[] invocationList193 = this.OnSetFriendTagsRequestEvent.GetInvocationList();
				foreach (Delegate delegate193 in invocationList193)
				{
					if (object.ReferenceEquals(delegate193.Target, instance))
					{
						OnSetFriendTagsRequestEvent -= (PlayFabRequestEvent<SetFriendTagsRequest>)delegate193;
					}
				}
			}
			if (this.OnSetFriendTagsResultEvent != null)
			{
				Delegate[] invocationList194 = this.OnSetFriendTagsResultEvent.GetInvocationList();
				foreach (Delegate delegate194 in invocationList194)
				{
					if (object.ReferenceEquals(delegate194.Target, instance))
					{
						OnSetFriendTagsResultEvent -= (PlayFabResultEvent<SetFriendTagsResult>)delegate194;
					}
				}
			}
			if (this.OnSetPlayerSecretRequestEvent != null)
			{
				Delegate[] invocationList195 = this.OnSetPlayerSecretRequestEvent.GetInvocationList();
				foreach (Delegate delegate195 in invocationList195)
				{
					if (object.ReferenceEquals(delegate195.Target, instance))
					{
						OnSetPlayerSecretRequestEvent -= (PlayFabRequestEvent<SetPlayerSecretRequest>)delegate195;
					}
				}
			}
			if (this.OnSetPlayerSecretResultEvent != null)
			{
				Delegate[] invocationList196 = this.OnSetPlayerSecretResultEvent.GetInvocationList();
				foreach (Delegate delegate196 in invocationList196)
				{
					if (object.ReferenceEquals(delegate196.Target, instance))
					{
						OnSetPlayerSecretResultEvent -= (PlayFabResultEvent<SetPlayerSecretResult>)delegate196;
					}
				}
			}
			if (this.OnStartGameRequestEvent != null)
			{
				Delegate[] invocationList197 = this.OnStartGameRequestEvent.GetInvocationList();
				foreach (Delegate delegate197 in invocationList197)
				{
					if (object.ReferenceEquals(delegate197.Target, instance))
					{
						OnStartGameRequestEvent -= (PlayFabRequestEvent<StartGameRequest>)delegate197;
					}
				}
			}
			if (this.OnStartGameResultEvent != null)
			{
				Delegate[] invocationList198 = this.OnStartGameResultEvent.GetInvocationList();
				foreach (Delegate delegate198 in invocationList198)
				{
					if (object.ReferenceEquals(delegate198.Target, instance))
					{
						OnStartGameResultEvent -= (PlayFabResultEvent<StartGameResult>)delegate198;
					}
				}
			}
			if (this.OnStartPurchaseRequestEvent != null)
			{
				Delegate[] invocationList199 = this.OnStartPurchaseRequestEvent.GetInvocationList();
				foreach (Delegate delegate199 in invocationList199)
				{
					if (object.ReferenceEquals(delegate199.Target, instance))
					{
						OnStartPurchaseRequestEvent -= (PlayFabRequestEvent<StartPurchaseRequest>)delegate199;
					}
				}
			}
			if (this.OnStartPurchaseResultEvent != null)
			{
				Delegate[] invocationList200 = this.OnStartPurchaseResultEvent.GetInvocationList();
				foreach (Delegate delegate200 in invocationList200)
				{
					if (object.ReferenceEquals(delegate200.Target, instance))
					{
						OnStartPurchaseResultEvent -= (PlayFabResultEvent<StartPurchaseResult>)delegate200;
					}
				}
			}
			if (this.OnSubtractUserVirtualCurrencyRequestEvent != null)
			{
				Delegate[] invocationList201 = this.OnSubtractUserVirtualCurrencyRequestEvent.GetInvocationList();
				foreach (Delegate delegate201 in invocationList201)
				{
					if (object.ReferenceEquals(delegate201.Target, instance))
					{
						OnSubtractUserVirtualCurrencyRequestEvent -= (PlayFabRequestEvent<SubtractUserVirtualCurrencyRequest>)delegate201;
					}
				}
			}
			if (this.OnSubtractUserVirtualCurrencyResultEvent != null)
			{
				Delegate[] invocationList202 = this.OnSubtractUserVirtualCurrencyResultEvent.GetInvocationList();
				foreach (Delegate delegate202 in invocationList202)
				{
					if (object.ReferenceEquals(delegate202.Target, instance))
					{
						OnSubtractUserVirtualCurrencyResultEvent -= (PlayFabResultEvent<ModifyUserVirtualCurrencyResult>)delegate202;
					}
				}
			}
			if (this.OnUnlinkAndroidDeviceIDRequestEvent != null)
			{
				Delegate[] invocationList203 = this.OnUnlinkAndroidDeviceIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate203 in invocationList203)
				{
					if (object.ReferenceEquals(delegate203.Target, instance))
					{
						OnUnlinkAndroidDeviceIDRequestEvent -= (PlayFabRequestEvent<UnlinkAndroidDeviceIDRequest>)delegate203;
					}
				}
			}
			if (this.OnUnlinkAndroidDeviceIDResultEvent != null)
			{
				Delegate[] invocationList204 = this.OnUnlinkAndroidDeviceIDResultEvent.GetInvocationList();
				foreach (Delegate delegate204 in invocationList204)
				{
					if (object.ReferenceEquals(delegate204.Target, instance))
					{
						OnUnlinkAndroidDeviceIDResultEvent -= (PlayFabResultEvent<UnlinkAndroidDeviceIDResult>)delegate204;
					}
				}
			}
			if (this.OnUnlinkCustomIDRequestEvent != null)
			{
				Delegate[] invocationList205 = this.OnUnlinkCustomIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate205 in invocationList205)
				{
					if (object.ReferenceEquals(delegate205.Target, instance))
					{
						OnUnlinkCustomIDRequestEvent -= (PlayFabRequestEvent<UnlinkCustomIDRequest>)delegate205;
					}
				}
			}
			if (this.OnUnlinkCustomIDResultEvent != null)
			{
				Delegate[] invocationList206 = this.OnUnlinkCustomIDResultEvent.GetInvocationList();
				foreach (Delegate delegate206 in invocationList206)
				{
					if (object.ReferenceEquals(delegate206.Target, instance))
					{
						OnUnlinkCustomIDResultEvent -= (PlayFabResultEvent<UnlinkCustomIDResult>)delegate206;
					}
				}
			}
			if (this.OnUnlinkFacebookAccountRequestEvent != null)
			{
				Delegate[] invocationList207 = this.OnUnlinkFacebookAccountRequestEvent.GetInvocationList();
				foreach (Delegate delegate207 in invocationList207)
				{
					if (object.ReferenceEquals(delegate207.Target, instance))
					{
						OnUnlinkFacebookAccountRequestEvent -= (PlayFabRequestEvent<UnlinkFacebookAccountRequest>)delegate207;
					}
				}
			}
			if (this.OnUnlinkFacebookAccountResultEvent != null)
			{
				Delegate[] invocationList208 = this.OnUnlinkFacebookAccountResultEvent.GetInvocationList();
				foreach (Delegate delegate208 in invocationList208)
				{
					if (object.ReferenceEquals(delegate208.Target, instance))
					{
						OnUnlinkFacebookAccountResultEvent -= (PlayFabResultEvent<UnlinkFacebookAccountResult>)delegate208;
					}
				}
			}
			if (this.OnUnlinkGameCenterAccountRequestEvent != null)
			{
				Delegate[] invocationList209 = this.OnUnlinkGameCenterAccountRequestEvent.GetInvocationList();
				foreach (Delegate delegate209 in invocationList209)
				{
					if (object.ReferenceEquals(delegate209.Target, instance))
					{
						OnUnlinkGameCenterAccountRequestEvent -= (PlayFabRequestEvent<UnlinkGameCenterAccountRequest>)delegate209;
					}
				}
			}
			if (this.OnUnlinkGameCenterAccountResultEvent != null)
			{
				Delegate[] invocationList210 = this.OnUnlinkGameCenterAccountResultEvent.GetInvocationList();
				foreach (Delegate delegate210 in invocationList210)
				{
					if (object.ReferenceEquals(delegate210.Target, instance))
					{
						OnUnlinkGameCenterAccountResultEvent -= (PlayFabResultEvent<UnlinkGameCenterAccountResult>)delegate210;
					}
				}
			}
			if (this.OnUnlinkGoogleAccountRequestEvent != null)
			{
				Delegate[] invocationList211 = this.OnUnlinkGoogleAccountRequestEvent.GetInvocationList();
				foreach (Delegate delegate211 in invocationList211)
				{
					if (object.ReferenceEquals(delegate211.Target, instance))
					{
						OnUnlinkGoogleAccountRequestEvent -= (PlayFabRequestEvent<UnlinkGoogleAccountRequest>)delegate211;
					}
				}
			}
			if (this.OnUnlinkGoogleAccountResultEvent != null)
			{
				Delegate[] invocationList212 = this.OnUnlinkGoogleAccountResultEvent.GetInvocationList();
				foreach (Delegate delegate212 in invocationList212)
				{
					if (object.ReferenceEquals(delegate212.Target, instance))
					{
						OnUnlinkGoogleAccountResultEvent -= (PlayFabResultEvent<UnlinkGoogleAccountResult>)delegate212;
					}
				}
			}
			if (this.OnUnlinkIOSDeviceIDRequestEvent != null)
			{
				Delegate[] invocationList213 = this.OnUnlinkIOSDeviceIDRequestEvent.GetInvocationList();
				foreach (Delegate delegate213 in invocationList213)
				{
					if (object.ReferenceEquals(delegate213.Target, instance))
					{
						OnUnlinkIOSDeviceIDRequestEvent -= (PlayFabRequestEvent<UnlinkIOSDeviceIDRequest>)delegate213;
					}
				}
			}
			if (this.OnUnlinkIOSDeviceIDResultEvent != null)
			{
				Delegate[] invocationList214 = this.OnUnlinkIOSDeviceIDResultEvent.GetInvocationList();
				foreach (Delegate delegate214 in invocationList214)
				{
					if (object.ReferenceEquals(delegate214.Target, instance))
					{
						OnUnlinkIOSDeviceIDResultEvent -= (PlayFabResultEvent<UnlinkIOSDeviceIDResult>)delegate214;
					}
				}
			}
			if (this.OnUnlinkKongregateRequestEvent != null)
			{
				Delegate[] invocationList215 = this.OnUnlinkKongregateRequestEvent.GetInvocationList();
				foreach (Delegate delegate215 in invocationList215)
				{
					if (object.ReferenceEquals(delegate215.Target, instance))
					{
						OnUnlinkKongregateRequestEvent -= (PlayFabRequestEvent<UnlinkKongregateAccountRequest>)delegate215;
					}
				}
			}
			if (this.OnUnlinkKongregateResultEvent != null)
			{
				Delegate[] invocationList216 = this.OnUnlinkKongregateResultEvent.GetInvocationList();
				foreach (Delegate delegate216 in invocationList216)
				{
					if (object.ReferenceEquals(delegate216.Target, instance))
					{
						OnUnlinkKongregateResultEvent -= (PlayFabResultEvent<UnlinkKongregateAccountResult>)delegate216;
					}
				}
			}
			if (this.OnUnlinkSteamAccountRequestEvent != null)
			{
				Delegate[] invocationList217 = this.OnUnlinkSteamAccountRequestEvent.GetInvocationList();
				foreach (Delegate delegate217 in invocationList217)
				{
					if (object.ReferenceEquals(delegate217.Target, instance))
					{
						OnUnlinkSteamAccountRequestEvent -= (PlayFabRequestEvent<UnlinkSteamAccountRequest>)delegate217;
					}
				}
			}
			if (this.OnUnlinkSteamAccountResultEvent != null)
			{
				Delegate[] invocationList218 = this.OnUnlinkSteamAccountResultEvent.GetInvocationList();
				foreach (Delegate delegate218 in invocationList218)
				{
					if (object.ReferenceEquals(delegate218.Target, instance))
					{
						OnUnlinkSteamAccountResultEvent -= (PlayFabResultEvent<UnlinkSteamAccountResult>)delegate218;
					}
				}
			}
			if (this.OnUnlinkTwitchRequestEvent != null)
			{
				Delegate[] invocationList219 = this.OnUnlinkTwitchRequestEvent.GetInvocationList();
				foreach (Delegate delegate219 in invocationList219)
				{
					if (object.ReferenceEquals(delegate219.Target, instance))
					{
						OnUnlinkTwitchRequestEvent -= (PlayFabRequestEvent<UnlinkTwitchAccountRequest>)delegate219;
					}
				}
			}
			if (this.OnUnlinkTwitchResultEvent != null)
			{
				Delegate[] invocationList220 = this.OnUnlinkTwitchResultEvent.GetInvocationList();
				foreach (Delegate delegate220 in invocationList220)
				{
					if (object.ReferenceEquals(delegate220.Target, instance))
					{
						OnUnlinkTwitchResultEvent -= (PlayFabResultEvent<UnlinkTwitchAccountResult>)delegate220;
					}
				}
			}
			if (this.OnUnlinkWindowsHelloRequestEvent != null)
			{
				Delegate[] invocationList221 = this.OnUnlinkWindowsHelloRequestEvent.GetInvocationList();
				foreach (Delegate delegate221 in invocationList221)
				{
					if (object.ReferenceEquals(delegate221.Target, instance))
					{
						OnUnlinkWindowsHelloRequestEvent -= (PlayFabRequestEvent<UnlinkWindowsHelloAccountRequest>)delegate221;
					}
				}
			}
			if (this.OnUnlinkWindowsHelloResultEvent != null)
			{
				Delegate[] invocationList222 = this.OnUnlinkWindowsHelloResultEvent.GetInvocationList();
				foreach (Delegate delegate222 in invocationList222)
				{
					if (object.ReferenceEquals(delegate222.Target, instance))
					{
						OnUnlinkWindowsHelloResultEvent -= (PlayFabResultEvent<UnlinkWindowsHelloAccountResponse>)delegate222;
					}
				}
			}
			if (this.OnUnlockContainerInstanceRequestEvent != null)
			{
				Delegate[] invocationList223 = this.OnUnlockContainerInstanceRequestEvent.GetInvocationList();
				foreach (Delegate delegate223 in invocationList223)
				{
					if (object.ReferenceEquals(delegate223.Target, instance))
					{
						OnUnlockContainerInstanceRequestEvent -= (PlayFabRequestEvent<UnlockContainerInstanceRequest>)delegate223;
					}
				}
			}
			if (this.OnUnlockContainerInstanceResultEvent != null)
			{
				Delegate[] invocationList224 = this.OnUnlockContainerInstanceResultEvent.GetInvocationList();
				foreach (Delegate delegate224 in invocationList224)
				{
					if (object.ReferenceEquals(delegate224.Target, instance))
					{
						OnUnlockContainerInstanceResultEvent -= (PlayFabResultEvent<UnlockContainerItemResult>)delegate224;
					}
				}
			}
			if (this.OnUnlockContainerItemRequestEvent != null)
			{
				Delegate[] invocationList225 = this.OnUnlockContainerItemRequestEvent.GetInvocationList();
				foreach (Delegate delegate225 in invocationList225)
				{
					if (object.ReferenceEquals(delegate225.Target, instance))
					{
						OnUnlockContainerItemRequestEvent -= (PlayFabRequestEvent<UnlockContainerItemRequest>)delegate225;
					}
				}
			}
			if (this.OnUnlockContainerItemResultEvent != null)
			{
				Delegate[] invocationList226 = this.OnUnlockContainerItemResultEvent.GetInvocationList();
				foreach (Delegate delegate226 in invocationList226)
				{
					if (object.ReferenceEquals(delegate226.Target, instance))
					{
						OnUnlockContainerItemResultEvent -= (PlayFabResultEvent<UnlockContainerItemResult>)delegate226;
					}
				}
			}
			if (this.OnUpdateAvatarUrlRequestEvent != null)
			{
				Delegate[] invocationList227 = this.OnUpdateAvatarUrlRequestEvent.GetInvocationList();
				foreach (Delegate delegate227 in invocationList227)
				{
					if (object.ReferenceEquals(delegate227.Target, instance))
					{
						OnUpdateAvatarUrlRequestEvent -= (PlayFabRequestEvent<UpdateAvatarUrlRequest>)delegate227;
					}
				}
			}
			if (this.OnUpdateAvatarUrlResultEvent != null)
			{
				Delegate[] invocationList228 = this.OnUpdateAvatarUrlResultEvent.GetInvocationList();
				foreach (Delegate delegate228 in invocationList228)
				{
					if (object.ReferenceEquals(delegate228.Target, instance))
					{
						OnUpdateAvatarUrlResultEvent -= (PlayFabResultEvent<EmptyResult>)delegate228;
					}
				}
			}
			if (this.OnUpdateCharacterDataRequestEvent != null)
			{
				Delegate[] invocationList229 = this.OnUpdateCharacterDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate229 in invocationList229)
				{
					if (object.ReferenceEquals(delegate229.Target, instance))
					{
						OnUpdateCharacterDataRequestEvent -= (PlayFabRequestEvent<UpdateCharacterDataRequest>)delegate229;
					}
				}
			}
			if (this.OnUpdateCharacterDataResultEvent != null)
			{
				Delegate[] invocationList230 = this.OnUpdateCharacterDataResultEvent.GetInvocationList();
				foreach (Delegate delegate230 in invocationList230)
				{
					if (object.ReferenceEquals(delegate230.Target, instance))
					{
						OnUpdateCharacterDataResultEvent -= (PlayFabResultEvent<UpdateCharacterDataResult>)delegate230;
					}
				}
			}
			if (this.OnUpdateCharacterStatisticsRequestEvent != null)
			{
				Delegate[] invocationList231 = this.OnUpdateCharacterStatisticsRequestEvent.GetInvocationList();
				foreach (Delegate delegate231 in invocationList231)
				{
					if (object.ReferenceEquals(delegate231.Target, instance))
					{
						OnUpdateCharacterStatisticsRequestEvent -= (PlayFabRequestEvent<UpdateCharacterStatisticsRequest>)delegate231;
					}
				}
			}
			if (this.OnUpdateCharacterStatisticsResultEvent != null)
			{
				Delegate[] invocationList232 = this.OnUpdateCharacterStatisticsResultEvent.GetInvocationList();
				foreach (Delegate delegate232 in invocationList232)
				{
					if (object.ReferenceEquals(delegate232.Target, instance))
					{
						OnUpdateCharacterStatisticsResultEvent -= (PlayFabResultEvent<UpdateCharacterStatisticsResult>)delegate232;
					}
				}
			}
			if (this.OnUpdatePlayerStatisticsRequestEvent != null)
			{
				Delegate[] invocationList233 = this.OnUpdatePlayerStatisticsRequestEvent.GetInvocationList();
				foreach (Delegate delegate233 in invocationList233)
				{
					if (object.ReferenceEquals(delegate233.Target, instance))
					{
						OnUpdatePlayerStatisticsRequestEvent -= (PlayFabRequestEvent<UpdatePlayerStatisticsRequest>)delegate233;
					}
				}
			}
			if (this.OnUpdatePlayerStatisticsResultEvent != null)
			{
				Delegate[] invocationList234 = this.OnUpdatePlayerStatisticsResultEvent.GetInvocationList();
				foreach (Delegate delegate234 in invocationList234)
				{
					if (object.ReferenceEquals(delegate234.Target, instance))
					{
						OnUpdatePlayerStatisticsResultEvent -= (PlayFabResultEvent<UpdatePlayerStatisticsResult>)delegate234;
					}
				}
			}
			if (this.OnUpdateSharedGroupDataRequestEvent != null)
			{
				Delegate[] invocationList235 = this.OnUpdateSharedGroupDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate235 in invocationList235)
				{
					if (object.ReferenceEquals(delegate235.Target, instance))
					{
						OnUpdateSharedGroupDataRequestEvent -= (PlayFabRequestEvent<UpdateSharedGroupDataRequest>)delegate235;
					}
				}
			}
			if (this.OnUpdateSharedGroupDataResultEvent != null)
			{
				Delegate[] invocationList236 = this.OnUpdateSharedGroupDataResultEvent.GetInvocationList();
				foreach (Delegate delegate236 in invocationList236)
				{
					if (object.ReferenceEquals(delegate236.Target, instance))
					{
						OnUpdateSharedGroupDataResultEvent -= (PlayFabResultEvent<UpdateSharedGroupDataResult>)delegate236;
					}
				}
			}
			if (this.OnUpdateUserDataRequestEvent != null)
			{
				Delegate[] invocationList237 = this.OnUpdateUserDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate237 in invocationList237)
				{
					if (object.ReferenceEquals(delegate237.Target, instance))
					{
						OnUpdateUserDataRequestEvent -= (PlayFabRequestEvent<UpdateUserDataRequest>)delegate237;
					}
				}
			}
			if (this.OnUpdateUserDataResultEvent != null)
			{
				Delegate[] invocationList238 = this.OnUpdateUserDataResultEvent.GetInvocationList();
				foreach (Delegate delegate238 in invocationList238)
				{
					if (object.ReferenceEquals(delegate238.Target, instance))
					{
						OnUpdateUserDataResultEvent -= (PlayFabResultEvent<UpdateUserDataResult>)delegate238;
					}
				}
			}
			if (this.OnUpdateUserPublisherDataRequestEvent != null)
			{
				Delegate[] invocationList239 = this.OnUpdateUserPublisherDataRequestEvent.GetInvocationList();
				foreach (Delegate delegate239 in invocationList239)
				{
					if (object.ReferenceEquals(delegate239.Target, instance))
					{
						OnUpdateUserPublisherDataRequestEvent -= (PlayFabRequestEvent<UpdateUserDataRequest>)delegate239;
					}
				}
			}
			if (this.OnUpdateUserPublisherDataResultEvent != null)
			{
				Delegate[] invocationList240 = this.OnUpdateUserPublisherDataResultEvent.GetInvocationList();
				foreach (Delegate delegate240 in invocationList240)
				{
					if (object.ReferenceEquals(delegate240.Target, instance))
					{
						OnUpdateUserPublisherDataResultEvent -= (PlayFabResultEvent<UpdateUserDataResult>)delegate240;
					}
				}
			}
			if (this.OnUpdateUserTitleDisplayNameRequestEvent != null)
			{
				Delegate[] invocationList241 = this.OnUpdateUserTitleDisplayNameRequestEvent.GetInvocationList();
				foreach (Delegate delegate241 in invocationList241)
				{
					if (object.ReferenceEquals(delegate241.Target, instance))
					{
						OnUpdateUserTitleDisplayNameRequestEvent -= (PlayFabRequestEvent<UpdateUserTitleDisplayNameRequest>)delegate241;
					}
				}
			}
			if (this.OnUpdateUserTitleDisplayNameResultEvent != null)
			{
				Delegate[] invocationList242 = this.OnUpdateUserTitleDisplayNameResultEvent.GetInvocationList();
				foreach (Delegate delegate242 in invocationList242)
				{
					if (object.ReferenceEquals(delegate242.Target, instance))
					{
						OnUpdateUserTitleDisplayNameResultEvent -= (PlayFabResultEvent<UpdateUserTitleDisplayNameResult>)delegate242;
					}
				}
			}
			if (this.OnValidateAmazonIAPReceiptRequestEvent != null)
			{
				Delegate[] invocationList243 = this.OnValidateAmazonIAPReceiptRequestEvent.GetInvocationList();
				foreach (Delegate delegate243 in invocationList243)
				{
					if (object.ReferenceEquals(delegate243.Target, instance))
					{
						OnValidateAmazonIAPReceiptRequestEvent -= (PlayFabRequestEvent<ValidateAmazonReceiptRequest>)delegate243;
					}
				}
			}
			if (this.OnValidateAmazonIAPReceiptResultEvent != null)
			{
				Delegate[] invocationList244 = this.OnValidateAmazonIAPReceiptResultEvent.GetInvocationList();
				foreach (Delegate delegate244 in invocationList244)
				{
					if (object.ReferenceEquals(delegate244.Target, instance))
					{
						OnValidateAmazonIAPReceiptResultEvent -= (PlayFabResultEvent<ValidateAmazonReceiptResult>)delegate244;
					}
				}
			}
			if (this.OnValidateGooglePlayPurchaseRequestEvent != null)
			{
				Delegate[] invocationList245 = this.OnValidateGooglePlayPurchaseRequestEvent.GetInvocationList();
				foreach (Delegate delegate245 in invocationList245)
				{
					if (object.ReferenceEquals(delegate245.Target, instance))
					{
						OnValidateGooglePlayPurchaseRequestEvent -= (PlayFabRequestEvent<ValidateGooglePlayPurchaseRequest>)delegate245;
					}
				}
			}
			if (this.OnValidateGooglePlayPurchaseResultEvent != null)
			{
				Delegate[] invocationList246 = this.OnValidateGooglePlayPurchaseResultEvent.GetInvocationList();
				foreach (Delegate delegate246 in invocationList246)
				{
					if (object.ReferenceEquals(delegate246.Target, instance))
					{
						OnValidateGooglePlayPurchaseResultEvent -= (PlayFabResultEvent<ValidateGooglePlayPurchaseResult>)delegate246;
					}
				}
			}
			if (this.OnValidateIOSReceiptRequestEvent != null)
			{
				Delegate[] invocationList247 = this.OnValidateIOSReceiptRequestEvent.GetInvocationList();
				foreach (Delegate delegate247 in invocationList247)
				{
					if (object.ReferenceEquals(delegate247.Target, instance))
					{
						OnValidateIOSReceiptRequestEvent -= (PlayFabRequestEvent<ValidateIOSReceiptRequest>)delegate247;
					}
				}
			}
			if (this.OnValidateIOSReceiptResultEvent != null)
			{
				Delegate[] invocationList248 = this.OnValidateIOSReceiptResultEvent.GetInvocationList();
				foreach (Delegate delegate248 in invocationList248)
				{
					if (object.ReferenceEquals(delegate248.Target, instance))
					{
						OnValidateIOSReceiptResultEvent -= (PlayFabResultEvent<ValidateIOSReceiptResult>)delegate248;
					}
				}
			}
			if (this.OnValidateWindowsStoreReceiptRequestEvent != null)
			{
				Delegate[] invocationList249 = this.OnValidateWindowsStoreReceiptRequestEvent.GetInvocationList();
				foreach (Delegate delegate249 in invocationList249)
				{
					if (object.ReferenceEquals(delegate249.Target, instance))
					{
						OnValidateWindowsStoreReceiptRequestEvent -= (PlayFabRequestEvent<ValidateWindowsReceiptRequest>)delegate249;
					}
				}
			}
			if (this.OnValidateWindowsStoreReceiptResultEvent != null)
			{
				Delegate[] invocationList250 = this.OnValidateWindowsStoreReceiptResultEvent.GetInvocationList();
				foreach (Delegate delegate250 in invocationList250)
				{
					if (object.ReferenceEquals(delegate250.Target, instance))
					{
						OnValidateWindowsStoreReceiptResultEvent -= (PlayFabResultEvent<ValidateWindowsReceiptResult>)delegate250;
					}
				}
			}
			if (this.OnWriteCharacterEventRequestEvent != null)
			{
				Delegate[] invocationList251 = this.OnWriteCharacterEventRequestEvent.GetInvocationList();
				foreach (Delegate delegate251 in invocationList251)
				{
					if (object.ReferenceEquals(delegate251.Target, instance))
					{
						OnWriteCharacterEventRequestEvent -= (PlayFabRequestEvent<WriteClientCharacterEventRequest>)delegate251;
					}
				}
			}
			if (this.OnWriteCharacterEventResultEvent != null)
			{
				Delegate[] invocationList252 = this.OnWriteCharacterEventResultEvent.GetInvocationList();
				foreach (Delegate delegate252 in invocationList252)
				{
					if (object.ReferenceEquals(delegate252.Target, instance))
					{
						OnWriteCharacterEventResultEvent -= (PlayFabResultEvent<WriteEventResponse>)delegate252;
					}
				}
			}
			if (this.OnWritePlayerEventRequestEvent != null)
			{
				Delegate[] invocationList253 = this.OnWritePlayerEventRequestEvent.GetInvocationList();
				foreach (Delegate delegate253 in invocationList253)
				{
					if (object.ReferenceEquals(delegate253.Target, instance))
					{
						OnWritePlayerEventRequestEvent -= (PlayFabRequestEvent<WriteClientPlayerEventRequest>)delegate253;
					}
				}
			}
			if (this.OnWritePlayerEventResultEvent != null)
			{
				Delegate[] invocationList254 = this.OnWritePlayerEventResultEvent.GetInvocationList();
				foreach (Delegate delegate254 in invocationList254)
				{
					if (object.ReferenceEquals(delegate254.Target, instance))
					{
						OnWritePlayerEventResultEvent -= (PlayFabResultEvent<WriteEventResponse>)delegate254;
					}
				}
			}
			if (this.OnWriteTitleEventRequestEvent != null)
			{
				Delegate[] invocationList255 = this.OnWriteTitleEventRequestEvent.GetInvocationList();
				foreach (Delegate delegate255 in invocationList255)
				{
					if (object.ReferenceEquals(delegate255.Target, instance))
					{
						OnWriteTitleEventRequestEvent -= (PlayFabRequestEvent<WriteTitleEventRequest>)delegate255;
					}
				}
			}
			if (this.OnWriteTitleEventResultEvent == null)
			{
				return;
			}
			Delegate[] invocationList256 = this.OnWriteTitleEventResultEvent.GetInvocationList();
			foreach (Delegate delegate256 in invocationList256)
			{
				if (object.ReferenceEquals(delegate256.Target, instance))
				{
					OnWriteTitleEventResultEvent -= (PlayFabResultEvent<WriteEventResponse>)delegate256;
				}
			}
		}

		private void OnProcessingErrorEvent(PlayFabRequestCommon request, PlayFabError error)
		{
			if (_instance.OnGlobalErrorEvent != null)
			{
				_instance.OnGlobalErrorEvent(request, error);
			}
		}

		private void OnProcessingEvent(ApiProcessingEventArgs e)
		{
			if (e.EventType == ApiProcessingEventType.Pre)
			{
				Type type = e.Request.GetType();
				if (type == typeof(AcceptTradeRequest) && _instance.OnAcceptTradeRequestEvent != null)
				{
					_instance.OnAcceptTradeRequestEvent((AcceptTradeRequest)e.Request);
				}
				else if (type == typeof(AddFriendRequest) && _instance.OnAddFriendRequestEvent != null)
				{
					_instance.OnAddFriendRequestEvent((AddFriendRequest)e.Request);
				}
				else if (type == typeof(AddGenericIDRequest) && _instance.OnAddGenericIDRequestEvent != null)
				{
					_instance.OnAddGenericIDRequestEvent((AddGenericIDRequest)e.Request);
				}
				else if (type == typeof(AddOrUpdateContactEmailRequest) && _instance.OnAddOrUpdateContactEmailRequestEvent != null)
				{
					_instance.OnAddOrUpdateContactEmailRequestEvent((AddOrUpdateContactEmailRequest)e.Request);
				}
				else if (type == typeof(AddSharedGroupMembersRequest) && _instance.OnAddSharedGroupMembersRequestEvent != null)
				{
					_instance.OnAddSharedGroupMembersRequestEvent((AddSharedGroupMembersRequest)e.Request);
				}
				else if (type == typeof(AddUsernamePasswordRequest) && _instance.OnAddUsernamePasswordRequestEvent != null)
				{
					_instance.OnAddUsernamePasswordRequestEvent((AddUsernamePasswordRequest)e.Request);
				}
				else if (type == typeof(AddUserVirtualCurrencyRequest) && _instance.OnAddUserVirtualCurrencyRequestEvent != null)
				{
					_instance.OnAddUserVirtualCurrencyRequestEvent((AddUserVirtualCurrencyRequest)e.Request);
				}
				else if (type == typeof(AndroidDevicePushNotificationRegistrationRequest) && _instance.OnAndroidDevicePushNotificationRegistrationRequestEvent != null)
				{
					_instance.OnAndroidDevicePushNotificationRegistrationRequestEvent((AndroidDevicePushNotificationRegistrationRequest)e.Request);
				}
				else if (type == typeof(AttributeInstallRequest) && _instance.OnAttributeInstallRequestEvent != null)
				{
					_instance.OnAttributeInstallRequestEvent((AttributeInstallRequest)e.Request);
				}
				else if (type == typeof(CancelTradeRequest) && _instance.OnCancelTradeRequestEvent != null)
				{
					_instance.OnCancelTradeRequestEvent((CancelTradeRequest)e.Request);
				}
				else if (type == typeof(ConfirmPurchaseRequest) && _instance.OnConfirmPurchaseRequestEvent != null)
				{
					_instance.OnConfirmPurchaseRequestEvent((ConfirmPurchaseRequest)e.Request);
				}
				else if (type == typeof(ConsumeItemRequest) && _instance.OnConsumeItemRequestEvent != null)
				{
					_instance.OnConsumeItemRequestEvent((ConsumeItemRequest)e.Request);
				}
				else if (type == typeof(CreateSharedGroupRequest) && _instance.OnCreateSharedGroupRequestEvent != null)
				{
					_instance.OnCreateSharedGroupRequestEvent((CreateSharedGroupRequest)e.Request);
				}
				else if (type == typeof(ExecuteCloudScriptRequest) && _instance.OnExecuteCloudScriptRequestEvent != null)
				{
					_instance.OnExecuteCloudScriptRequestEvent((ExecuteCloudScriptRequest)e.Request);
				}
				else if (type == typeof(GetAccountInfoRequest) && _instance.OnGetAccountInfoRequestEvent != null)
				{
					_instance.OnGetAccountInfoRequestEvent((GetAccountInfoRequest)e.Request);
				}
				else if (type == typeof(ListUsersCharactersRequest) && _instance.OnGetAllUsersCharactersRequestEvent != null)
				{
					_instance.OnGetAllUsersCharactersRequestEvent((ListUsersCharactersRequest)e.Request);
				}
				else if (type == typeof(GetCatalogItemsRequest) && _instance.OnGetCatalogItemsRequestEvent != null)
				{
					_instance.OnGetCatalogItemsRequestEvent((GetCatalogItemsRequest)e.Request);
				}
				else if (type == typeof(GetCharacterDataRequest) && _instance.OnGetCharacterDataRequestEvent != null)
				{
					_instance.OnGetCharacterDataRequestEvent((GetCharacterDataRequest)e.Request);
				}
				else if (type == typeof(GetCharacterInventoryRequest) && _instance.OnGetCharacterInventoryRequestEvent != null)
				{
					_instance.OnGetCharacterInventoryRequestEvent((GetCharacterInventoryRequest)e.Request);
				}
				else if (type == typeof(GetCharacterLeaderboardRequest) && _instance.OnGetCharacterLeaderboardRequestEvent != null)
				{
					_instance.OnGetCharacterLeaderboardRequestEvent((GetCharacterLeaderboardRequest)e.Request);
				}
				else if (type == typeof(GetCharacterDataRequest) && _instance.OnGetCharacterReadOnlyDataRequestEvent != null)
				{
					_instance.OnGetCharacterReadOnlyDataRequestEvent((GetCharacterDataRequest)e.Request);
				}
				else if (type == typeof(GetCharacterStatisticsRequest) && _instance.OnGetCharacterStatisticsRequestEvent != null)
				{
					_instance.OnGetCharacterStatisticsRequestEvent((GetCharacterStatisticsRequest)e.Request);
				}
				else if (type == typeof(GetContentDownloadUrlRequest) && _instance.OnGetContentDownloadUrlRequestEvent != null)
				{
					_instance.OnGetContentDownloadUrlRequestEvent((GetContentDownloadUrlRequest)e.Request);
				}
				else if (type == typeof(CurrentGamesRequest) && _instance.OnGetCurrentGamesRequestEvent != null)
				{
					_instance.OnGetCurrentGamesRequestEvent((CurrentGamesRequest)e.Request);
				}
				else if (type == typeof(GetFriendLeaderboardRequest) && _instance.OnGetFriendLeaderboardRequestEvent != null)
				{
					_instance.OnGetFriendLeaderboardRequestEvent((GetFriendLeaderboardRequest)e.Request);
				}
				else if (type == typeof(GetFriendLeaderboardAroundPlayerRequest) && _instance.OnGetFriendLeaderboardAroundPlayerRequestEvent != null)
				{
					_instance.OnGetFriendLeaderboardAroundPlayerRequestEvent((GetFriendLeaderboardAroundPlayerRequest)e.Request);
				}
				else if (type == typeof(GetFriendsListRequest) && _instance.OnGetFriendsListRequestEvent != null)
				{
					_instance.OnGetFriendsListRequestEvent((GetFriendsListRequest)e.Request);
				}
				else if (type == typeof(GameServerRegionsRequest) && _instance.OnGetGameServerRegionsRequestEvent != null)
				{
					_instance.OnGetGameServerRegionsRequestEvent((GameServerRegionsRequest)e.Request);
				}
				else if (type == typeof(GetLeaderboardRequest) && _instance.OnGetLeaderboardRequestEvent != null)
				{
					_instance.OnGetLeaderboardRequestEvent((GetLeaderboardRequest)e.Request);
				}
				else if (type == typeof(GetLeaderboardAroundCharacterRequest) && _instance.OnGetLeaderboardAroundCharacterRequestEvent != null)
				{
					_instance.OnGetLeaderboardAroundCharacterRequestEvent((GetLeaderboardAroundCharacterRequest)e.Request);
				}
				else if (type == typeof(GetLeaderboardAroundPlayerRequest) && _instance.OnGetLeaderboardAroundPlayerRequestEvent != null)
				{
					_instance.OnGetLeaderboardAroundPlayerRequestEvent((GetLeaderboardAroundPlayerRequest)e.Request);
				}
				else if (type == typeof(GetLeaderboardForUsersCharactersRequest) && _instance.OnGetLeaderboardForUserCharactersRequestEvent != null)
				{
					_instance.OnGetLeaderboardForUserCharactersRequestEvent((GetLeaderboardForUsersCharactersRequest)e.Request);
				}
				else if (type == typeof(GetPaymentTokenRequest) && _instance.OnGetPaymentTokenRequestEvent != null)
				{
					_instance.OnGetPaymentTokenRequestEvent((GetPaymentTokenRequest)e.Request);
				}
				else if (type == typeof(GetPhotonAuthenticationTokenRequest) && _instance.OnGetPhotonAuthenticationTokenRequestEvent != null)
				{
					_instance.OnGetPhotonAuthenticationTokenRequestEvent((GetPhotonAuthenticationTokenRequest)e.Request);
				}
				else if (type == typeof(GetPlayerCombinedInfoRequest) && _instance.OnGetPlayerCombinedInfoRequestEvent != null)
				{
					_instance.OnGetPlayerCombinedInfoRequestEvent((GetPlayerCombinedInfoRequest)e.Request);
				}
				else if (type == typeof(GetPlayerProfileRequest) && _instance.OnGetPlayerProfileRequestEvent != null)
				{
					_instance.OnGetPlayerProfileRequestEvent((GetPlayerProfileRequest)e.Request);
				}
				else if (type == typeof(GetPlayerSegmentsRequest) && _instance.OnGetPlayerSegmentsRequestEvent != null)
				{
					_instance.OnGetPlayerSegmentsRequestEvent((GetPlayerSegmentsRequest)e.Request);
				}
				else if (type == typeof(GetPlayerStatisticsRequest) && _instance.OnGetPlayerStatisticsRequestEvent != null)
				{
					_instance.OnGetPlayerStatisticsRequestEvent((GetPlayerStatisticsRequest)e.Request);
				}
				else if (type == typeof(GetPlayerStatisticVersionsRequest) && _instance.OnGetPlayerStatisticVersionsRequestEvent != null)
				{
					_instance.OnGetPlayerStatisticVersionsRequestEvent((GetPlayerStatisticVersionsRequest)e.Request);
				}
				else if (type == typeof(GetPlayerTagsRequest) && _instance.OnGetPlayerTagsRequestEvent != null)
				{
					_instance.OnGetPlayerTagsRequestEvent((GetPlayerTagsRequest)e.Request);
				}
				else if (type == typeof(GetPlayerTradesRequest) && _instance.OnGetPlayerTradesRequestEvent != null)
				{
					_instance.OnGetPlayerTradesRequestEvent((GetPlayerTradesRequest)e.Request);
				}
				else if (type == typeof(GetPlayFabIDsFromFacebookIDsRequest) && _instance.OnGetPlayFabIDsFromFacebookIDsRequestEvent != null)
				{
					_instance.OnGetPlayFabIDsFromFacebookIDsRequestEvent((GetPlayFabIDsFromFacebookIDsRequest)e.Request);
				}
				else if (type == typeof(GetPlayFabIDsFromGameCenterIDsRequest) && _instance.OnGetPlayFabIDsFromGameCenterIDsRequestEvent != null)
				{
					_instance.OnGetPlayFabIDsFromGameCenterIDsRequestEvent((GetPlayFabIDsFromGameCenterIDsRequest)e.Request);
				}
				else if (type == typeof(GetPlayFabIDsFromGenericIDsRequest) && _instance.OnGetPlayFabIDsFromGenericIDsRequestEvent != null)
				{
					_instance.OnGetPlayFabIDsFromGenericIDsRequestEvent((GetPlayFabIDsFromGenericIDsRequest)e.Request);
				}
				else if (type == typeof(GetPlayFabIDsFromGoogleIDsRequest) && _instance.OnGetPlayFabIDsFromGoogleIDsRequestEvent != null)
				{
					_instance.OnGetPlayFabIDsFromGoogleIDsRequestEvent((GetPlayFabIDsFromGoogleIDsRequest)e.Request);
				}
				else if (type == typeof(GetPlayFabIDsFromKongregateIDsRequest) && _instance.OnGetPlayFabIDsFromKongregateIDsRequestEvent != null)
				{
					_instance.OnGetPlayFabIDsFromKongregateIDsRequestEvent((GetPlayFabIDsFromKongregateIDsRequest)e.Request);
				}
				else if (type == typeof(GetPlayFabIDsFromSteamIDsRequest) && _instance.OnGetPlayFabIDsFromSteamIDsRequestEvent != null)
				{
					_instance.OnGetPlayFabIDsFromSteamIDsRequestEvent((GetPlayFabIDsFromSteamIDsRequest)e.Request);
				}
				else if (type == typeof(GetPlayFabIDsFromTwitchIDsRequest) && _instance.OnGetPlayFabIDsFromTwitchIDsRequestEvent != null)
				{
					_instance.OnGetPlayFabIDsFromTwitchIDsRequestEvent((GetPlayFabIDsFromTwitchIDsRequest)e.Request);
				}
				else if (type == typeof(GetPublisherDataRequest) && _instance.OnGetPublisherDataRequestEvent != null)
				{
					_instance.OnGetPublisherDataRequestEvent((GetPublisherDataRequest)e.Request);
				}
				else if (type == typeof(GetPurchaseRequest) && _instance.OnGetPurchaseRequestEvent != null)
				{
					_instance.OnGetPurchaseRequestEvent((GetPurchaseRequest)e.Request);
				}
				else if (type == typeof(GetSharedGroupDataRequest) && _instance.OnGetSharedGroupDataRequestEvent != null)
				{
					_instance.OnGetSharedGroupDataRequestEvent((GetSharedGroupDataRequest)e.Request);
				}
				else if (type == typeof(GetStoreItemsRequest) && _instance.OnGetStoreItemsRequestEvent != null)
				{
					_instance.OnGetStoreItemsRequestEvent((GetStoreItemsRequest)e.Request);
				}
				else if (type == typeof(GetTimeRequest) && _instance.OnGetTimeRequestEvent != null)
				{
					_instance.OnGetTimeRequestEvent((GetTimeRequest)e.Request);
				}
				else if (type == typeof(GetTitleDataRequest) && _instance.OnGetTitleDataRequestEvent != null)
				{
					_instance.OnGetTitleDataRequestEvent((GetTitleDataRequest)e.Request);
				}
				else if (type == typeof(GetTitleNewsRequest) && _instance.OnGetTitleNewsRequestEvent != null)
				{
					_instance.OnGetTitleNewsRequestEvent((GetTitleNewsRequest)e.Request);
				}
				else if (type == typeof(GetTitlePublicKeyRequest) && _instance.OnGetTitlePublicKeyRequestEvent != null)
				{
					_instance.OnGetTitlePublicKeyRequestEvent((GetTitlePublicKeyRequest)e.Request);
				}
				else if (type == typeof(GetTradeStatusRequest) && _instance.OnGetTradeStatusRequestEvent != null)
				{
					_instance.OnGetTradeStatusRequestEvent((GetTradeStatusRequest)e.Request);
				}
				else if (type == typeof(GetUserDataRequest) && _instance.OnGetUserDataRequestEvent != null)
				{
					_instance.OnGetUserDataRequestEvent((GetUserDataRequest)e.Request);
				}
				else if (type == typeof(GetUserInventoryRequest) && _instance.OnGetUserInventoryRequestEvent != null)
				{
					_instance.OnGetUserInventoryRequestEvent((GetUserInventoryRequest)e.Request);
				}
				else if (type == typeof(GetUserDataRequest) && _instance.OnGetUserPublisherDataRequestEvent != null)
				{
					_instance.OnGetUserPublisherDataRequestEvent((GetUserDataRequest)e.Request);
				}
				else if (type == typeof(GetUserDataRequest) && _instance.OnGetUserPublisherReadOnlyDataRequestEvent != null)
				{
					_instance.OnGetUserPublisherReadOnlyDataRequestEvent((GetUserDataRequest)e.Request);
				}
				else if (type == typeof(GetUserDataRequest) && _instance.OnGetUserReadOnlyDataRequestEvent != null)
				{
					_instance.OnGetUserReadOnlyDataRequestEvent((GetUserDataRequest)e.Request);
				}
				else if (type == typeof(GetWindowsHelloChallengeRequest) && _instance.OnGetWindowsHelloChallengeRequestEvent != null)
				{
					_instance.OnGetWindowsHelloChallengeRequestEvent((GetWindowsHelloChallengeRequest)e.Request);
				}
				else if (type == typeof(GrantCharacterToUserRequest) && _instance.OnGrantCharacterToUserRequestEvent != null)
				{
					_instance.OnGrantCharacterToUserRequestEvent((GrantCharacterToUserRequest)e.Request);
				}
				else if (type == typeof(LinkAndroidDeviceIDRequest) && _instance.OnLinkAndroidDeviceIDRequestEvent != null)
				{
					_instance.OnLinkAndroidDeviceIDRequestEvent((LinkAndroidDeviceIDRequest)e.Request);
				}
				else if (type == typeof(LinkCustomIDRequest) && _instance.OnLinkCustomIDRequestEvent != null)
				{
					_instance.OnLinkCustomIDRequestEvent((LinkCustomIDRequest)e.Request);
				}
				else if (type == typeof(LinkFacebookAccountRequest) && _instance.OnLinkFacebookAccountRequestEvent != null)
				{
					_instance.OnLinkFacebookAccountRequestEvent((LinkFacebookAccountRequest)e.Request);
				}
				else if (type == typeof(LinkGameCenterAccountRequest) && _instance.OnLinkGameCenterAccountRequestEvent != null)
				{
					_instance.OnLinkGameCenterAccountRequestEvent((LinkGameCenterAccountRequest)e.Request);
				}
				else if (type == typeof(LinkGoogleAccountRequest) && _instance.OnLinkGoogleAccountRequestEvent != null)
				{
					_instance.OnLinkGoogleAccountRequestEvent((LinkGoogleAccountRequest)e.Request);
				}
				else if (type == typeof(LinkIOSDeviceIDRequest) && _instance.OnLinkIOSDeviceIDRequestEvent != null)
				{
					_instance.OnLinkIOSDeviceIDRequestEvent((LinkIOSDeviceIDRequest)e.Request);
				}
				else if (type == typeof(LinkKongregateAccountRequest) && _instance.OnLinkKongregateRequestEvent != null)
				{
					_instance.OnLinkKongregateRequestEvent((LinkKongregateAccountRequest)e.Request);
				}
				else if (type == typeof(LinkSteamAccountRequest) && _instance.OnLinkSteamAccountRequestEvent != null)
				{
					_instance.OnLinkSteamAccountRequestEvent((LinkSteamAccountRequest)e.Request);
				}
				else if (type == typeof(LinkTwitchAccountRequest) && _instance.OnLinkTwitchRequestEvent != null)
				{
					_instance.OnLinkTwitchRequestEvent((LinkTwitchAccountRequest)e.Request);
				}
				else if (type == typeof(LinkWindowsHelloAccountRequest) && _instance.OnLinkWindowsHelloRequestEvent != null)
				{
					_instance.OnLinkWindowsHelloRequestEvent((LinkWindowsHelloAccountRequest)e.Request);
				}
				else if (type == typeof(LoginWithAndroidDeviceIDRequest) && _instance.OnLoginWithAndroidDeviceIDRequestEvent != null)
				{
					_instance.OnLoginWithAndroidDeviceIDRequestEvent((LoginWithAndroidDeviceIDRequest)e.Request);
				}
				else if (type == typeof(LoginWithCustomIDRequest) && _instance.OnLoginWithCustomIDRequestEvent != null)
				{
					_instance.OnLoginWithCustomIDRequestEvent((LoginWithCustomIDRequest)e.Request);
				}
				else if (type == typeof(LoginWithEmailAddressRequest) && _instance.OnLoginWithEmailAddressRequestEvent != null)
				{
					_instance.OnLoginWithEmailAddressRequestEvent((LoginWithEmailAddressRequest)e.Request);
				}
				else if (type == typeof(LoginWithFacebookRequest) && _instance.OnLoginWithFacebookRequestEvent != null)
				{
					_instance.OnLoginWithFacebookRequestEvent((LoginWithFacebookRequest)e.Request);
				}
				else if (type == typeof(LoginWithGameCenterRequest) && _instance.OnLoginWithGameCenterRequestEvent != null)
				{
					_instance.OnLoginWithGameCenterRequestEvent((LoginWithGameCenterRequest)e.Request);
				}
				else if (type == typeof(LoginWithGoogleAccountRequest) && _instance.OnLoginWithGoogleAccountRequestEvent != null)
				{
					_instance.OnLoginWithGoogleAccountRequestEvent((LoginWithGoogleAccountRequest)e.Request);
				}
				else if (type == typeof(LoginWithIOSDeviceIDRequest) && _instance.OnLoginWithIOSDeviceIDRequestEvent != null)
				{
					_instance.OnLoginWithIOSDeviceIDRequestEvent((LoginWithIOSDeviceIDRequest)e.Request);
				}
				else if (type == typeof(LoginWithKongregateRequest) && _instance.OnLoginWithKongregateRequestEvent != null)
				{
					_instance.OnLoginWithKongregateRequestEvent((LoginWithKongregateRequest)e.Request);
				}
				else if (type == typeof(LoginWithPlayFabRequest) && _instance.OnLoginWithPlayFabRequestEvent != null)
				{
					_instance.OnLoginWithPlayFabRequestEvent((LoginWithPlayFabRequest)e.Request);
				}
				else if (type == typeof(LoginWithSteamRequest) && _instance.OnLoginWithSteamRequestEvent != null)
				{
					_instance.OnLoginWithSteamRequestEvent((LoginWithSteamRequest)e.Request);
				}
				else if (type == typeof(LoginWithTwitchRequest) && _instance.OnLoginWithTwitchRequestEvent != null)
				{
					_instance.OnLoginWithTwitchRequestEvent((LoginWithTwitchRequest)e.Request);
				}
				else if (type == typeof(LoginWithWindowsHelloRequest) && _instance.OnLoginWithWindowsHelloRequestEvent != null)
				{
					_instance.OnLoginWithWindowsHelloRequestEvent((LoginWithWindowsHelloRequest)e.Request);
				}
				else if (type == typeof(MatchmakeRequest) && _instance.OnMatchmakeRequestEvent != null)
				{
					_instance.OnMatchmakeRequestEvent((MatchmakeRequest)e.Request);
				}
				else if (type == typeof(OpenTradeRequest) && _instance.OnOpenTradeRequestEvent != null)
				{
					_instance.OnOpenTradeRequestEvent((OpenTradeRequest)e.Request);
				}
				else if (type == typeof(PayForPurchaseRequest) && _instance.OnPayForPurchaseRequestEvent != null)
				{
					_instance.OnPayForPurchaseRequestEvent((PayForPurchaseRequest)e.Request);
				}
				else if (type == typeof(PurchaseItemRequest) && _instance.OnPurchaseItemRequestEvent != null)
				{
					_instance.OnPurchaseItemRequestEvent((PurchaseItemRequest)e.Request);
				}
				else if (type == typeof(RedeemCouponRequest) && _instance.OnRedeemCouponRequestEvent != null)
				{
					_instance.OnRedeemCouponRequestEvent((RedeemCouponRequest)e.Request);
				}
				else if (type == typeof(RegisterForIOSPushNotificationRequest) && _instance.OnRegisterForIOSPushNotificationRequestEvent != null)
				{
					_instance.OnRegisterForIOSPushNotificationRequestEvent((RegisterForIOSPushNotificationRequest)e.Request);
				}
				else if (type == typeof(RegisterPlayFabUserRequest) && _instance.OnRegisterPlayFabUserRequestEvent != null)
				{
					_instance.OnRegisterPlayFabUserRequestEvent((RegisterPlayFabUserRequest)e.Request);
				}
				else if (type == typeof(RegisterWithWindowsHelloRequest) && _instance.OnRegisterWithWindowsHelloRequestEvent != null)
				{
					_instance.OnRegisterWithWindowsHelloRequestEvent((RegisterWithWindowsHelloRequest)e.Request);
				}
				else if (type == typeof(RemoveContactEmailRequest) && _instance.OnRemoveContactEmailRequestEvent != null)
				{
					_instance.OnRemoveContactEmailRequestEvent((RemoveContactEmailRequest)e.Request);
				}
				else if (type == typeof(RemoveFriendRequest) && _instance.OnRemoveFriendRequestEvent != null)
				{
					_instance.OnRemoveFriendRequestEvent((RemoveFriendRequest)e.Request);
				}
				else if (type == typeof(RemoveGenericIDRequest) && _instance.OnRemoveGenericIDRequestEvent != null)
				{
					_instance.OnRemoveGenericIDRequestEvent((RemoveGenericIDRequest)e.Request);
				}
				else if (type == typeof(RemoveSharedGroupMembersRequest) && _instance.OnRemoveSharedGroupMembersRequestEvent != null)
				{
					_instance.OnRemoveSharedGroupMembersRequestEvent((RemoveSharedGroupMembersRequest)e.Request);
				}
				else if (type == typeof(DeviceInfoRequest) && _instance.OnReportDeviceInfoRequestEvent != null)
				{
					_instance.OnReportDeviceInfoRequestEvent((DeviceInfoRequest)e.Request);
				}
				else if (type == typeof(ReportPlayerClientRequest) && _instance.OnReportPlayerRequestEvent != null)
				{
					_instance.OnReportPlayerRequestEvent((ReportPlayerClientRequest)e.Request);
				}
				else if (type == typeof(RestoreIOSPurchasesRequest) && _instance.OnRestoreIOSPurchasesRequestEvent != null)
				{
					_instance.OnRestoreIOSPurchasesRequestEvent((RestoreIOSPurchasesRequest)e.Request);
				}
				else if (type == typeof(SendAccountRecoveryEmailRequest) && _instance.OnSendAccountRecoveryEmailRequestEvent != null)
				{
					_instance.OnSendAccountRecoveryEmailRequestEvent((SendAccountRecoveryEmailRequest)e.Request);
				}
				else if (type == typeof(SetFriendTagsRequest) && _instance.OnSetFriendTagsRequestEvent != null)
				{
					_instance.OnSetFriendTagsRequestEvent((SetFriendTagsRequest)e.Request);
				}
				else if (type == typeof(SetPlayerSecretRequest) && _instance.OnSetPlayerSecretRequestEvent != null)
				{
					_instance.OnSetPlayerSecretRequestEvent((SetPlayerSecretRequest)e.Request);
				}
				else if (type == typeof(StartGameRequest) && _instance.OnStartGameRequestEvent != null)
				{
					_instance.OnStartGameRequestEvent((StartGameRequest)e.Request);
				}
				else if (type == typeof(StartPurchaseRequest) && _instance.OnStartPurchaseRequestEvent != null)
				{
					_instance.OnStartPurchaseRequestEvent((StartPurchaseRequest)e.Request);
				}
				else if (type == typeof(SubtractUserVirtualCurrencyRequest) && _instance.OnSubtractUserVirtualCurrencyRequestEvent != null)
				{
					_instance.OnSubtractUserVirtualCurrencyRequestEvent((SubtractUserVirtualCurrencyRequest)e.Request);
				}
				else if (type == typeof(UnlinkAndroidDeviceIDRequest) && _instance.OnUnlinkAndroidDeviceIDRequestEvent != null)
				{
					_instance.OnUnlinkAndroidDeviceIDRequestEvent((UnlinkAndroidDeviceIDRequest)e.Request);
				}
				else if (type == typeof(UnlinkCustomIDRequest) && _instance.OnUnlinkCustomIDRequestEvent != null)
				{
					_instance.OnUnlinkCustomIDRequestEvent((UnlinkCustomIDRequest)e.Request);
				}
				else if (type == typeof(UnlinkFacebookAccountRequest) && _instance.OnUnlinkFacebookAccountRequestEvent != null)
				{
					_instance.OnUnlinkFacebookAccountRequestEvent((UnlinkFacebookAccountRequest)e.Request);
				}
				else if (type == typeof(UnlinkGameCenterAccountRequest) && _instance.OnUnlinkGameCenterAccountRequestEvent != null)
				{
					_instance.OnUnlinkGameCenterAccountRequestEvent((UnlinkGameCenterAccountRequest)e.Request);
				}
				else if (type == typeof(UnlinkGoogleAccountRequest) && _instance.OnUnlinkGoogleAccountRequestEvent != null)
				{
					_instance.OnUnlinkGoogleAccountRequestEvent((UnlinkGoogleAccountRequest)e.Request);
				}
				else if (type == typeof(UnlinkIOSDeviceIDRequest) && _instance.OnUnlinkIOSDeviceIDRequestEvent != null)
				{
					_instance.OnUnlinkIOSDeviceIDRequestEvent((UnlinkIOSDeviceIDRequest)e.Request);
				}
				else if (type == typeof(UnlinkKongregateAccountRequest) && _instance.OnUnlinkKongregateRequestEvent != null)
				{
					_instance.OnUnlinkKongregateRequestEvent((UnlinkKongregateAccountRequest)e.Request);
				}
				else if (type == typeof(UnlinkSteamAccountRequest) && _instance.OnUnlinkSteamAccountRequestEvent != null)
				{
					_instance.OnUnlinkSteamAccountRequestEvent((UnlinkSteamAccountRequest)e.Request);
				}
				else if (type == typeof(UnlinkTwitchAccountRequest) && _instance.OnUnlinkTwitchRequestEvent != null)
				{
					_instance.OnUnlinkTwitchRequestEvent((UnlinkTwitchAccountRequest)e.Request);
				}
				else if (type == typeof(UnlinkWindowsHelloAccountRequest) && _instance.OnUnlinkWindowsHelloRequestEvent != null)
				{
					_instance.OnUnlinkWindowsHelloRequestEvent((UnlinkWindowsHelloAccountRequest)e.Request);
				}
				else if (type == typeof(UnlockContainerInstanceRequest) && _instance.OnUnlockContainerInstanceRequestEvent != null)
				{
					_instance.OnUnlockContainerInstanceRequestEvent((UnlockContainerInstanceRequest)e.Request);
				}
				else if (type == typeof(UnlockContainerItemRequest) && _instance.OnUnlockContainerItemRequestEvent != null)
				{
					_instance.OnUnlockContainerItemRequestEvent((UnlockContainerItemRequest)e.Request);
				}
				else if (type == typeof(UpdateAvatarUrlRequest) && _instance.OnUpdateAvatarUrlRequestEvent != null)
				{
					_instance.OnUpdateAvatarUrlRequestEvent((UpdateAvatarUrlRequest)e.Request);
				}
				else if (type == typeof(UpdateCharacterDataRequest) && _instance.OnUpdateCharacterDataRequestEvent != null)
				{
					_instance.OnUpdateCharacterDataRequestEvent((UpdateCharacterDataRequest)e.Request);
				}
				else if (type == typeof(UpdateCharacterStatisticsRequest) && _instance.OnUpdateCharacterStatisticsRequestEvent != null)
				{
					_instance.OnUpdateCharacterStatisticsRequestEvent((UpdateCharacterStatisticsRequest)e.Request);
				}
				else if (type == typeof(UpdatePlayerStatisticsRequest) && _instance.OnUpdatePlayerStatisticsRequestEvent != null)
				{
					_instance.OnUpdatePlayerStatisticsRequestEvent((UpdatePlayerStatisticsRequest)e.Request);
				}
				else if (type == typeof(UpdateSharedGroupDataRequest) && _instance.OnUpdateSharedGroupDataRequestEvent != null)
				{
					_instance.OnUpdateSharedGroupDataRequestEvent((UpdateSharedGroupDataRequest)e.Request);
				}
				else if (type == typeof(UpdateUserDataRequest) && _instance.OnUpdateUserDataRequestEvent != null)
				{
					_instance.OnUpdateUserDataRequestEvent((UpdateUserDataRequest)e.Request);
				}
				else if (type == typeof(UpdateUserDataRequest) && _instance.OnUpdateUserPublisherDataRequestEvent != null)
				{
					_instance.OnUpdateUserPublisherDataRequestEvent((UpdateUserDataRequest)e.Request);
				}
				else if (type == typeof(UpdateUserTitleDisplayNameRequest) && _instance.OnUpdateUserTitleDisplayNameRequestEvent != null)
				{
					_instance.OnUpdateUserTitleDisplayNameRequestEvent((UpdateUserTitleDisplayNameRequest)e.Request);
				}
				else if (type == typeof(ValidateAmazonReceiptRequest) && _instance.OnValidateAmazonIAPReceiptRequestEvent != null)
				{
					_instance.OnValidateAmazonIAPReceiptRequestEvent((ValidateAmazonReceiptRequest)e.Request);
				}
				else if (type == typeof(ValidateGooglePlayPurchaseRequest) && _instance.OnValidateGooglePlayPurchaseRequestEvent != null)
				{
					_instance.OnValidateGooglePlayPurchaseRequestEvent((ValidateGooglePlayPurchaseRequest)e.Request);
				}
				else if (type == typeof(ValidateIOSReceiptRequest) && _instance.OnValidateIOSReceiptRequestEvent != null)
				{
					_instance.OnValidateIOSReceiptRequestEvent((ValidateIOSReceiptRequest)e.Request);
				}
				else if (type == typeof(ValidateWindowsReceiptRequest) && _instance.OnValidateWindowsStoreReceiptRequestEvent != null)
				{
					_instance.OnValidateWindowsStoreReceiptRequestEvent((ValidateWindowsReceiptRequest)e.Request);
				}
				else if (type == typeof(WriteClientCharacterEventRequest) && _instance.OnWriteCharacterEventRequestEvent != null)
				{
					_instance.OnWriteCharacterEventRequestEvent((WriteClientCharacterEventRequest)e.Request);
				}
				else if (type == typeof(WriteClientPlayerEventRequest) && _instance.OnWritePlayerEventRequestEvent != null)
				{
					_instance.OnWritePlayerEventRequestEvent((WriteClientPlayerEventRequest)e.Request);
				}
				else if (type == typeof(WriteTitleEventRequest) && _instance.OnWriteTitleEventRequestEvent != null)
				{
					_instance.OnWriteTitleEventRequestEvent((WriteTitleEventRequest)e.Request);
				}
			}
			else
			{
				Type type2 = e.Result.GetType();
				if (type2 == typeof(LoginResult) && _instance.OnLoginResultEvent != null)
				{
					_instance.OnLoginResultEvent((LoginResult)e.Result);
				}
				else if (type2 == typeof(AcceptTradeResponse) && _instance.OnAcceptTradeResultEvent != null)
				{
					_instance.OnAcceptTradeResultEvent((AcceptTradeResponse)e.Result);
				}
				else if (type2 == typeof(AddFriendResult) && _instance.OnAddFriendResultEvent != null)
				{
					_instance.OnAddFriendResultEvent((AddFriendResult)e.Result);
				}
				else if (type2 == typeof(AddGenericIDResult) && _instance.OnAddGenericIDResultEvent != null)
				{
					_instance.OnAddGenericIDResultEvent((AddGenericIDResult)e.Result);
				}
				else if (type2 == typeof(AddOrUpdateContactEmailResult) && _instance.OnAddOrUpdateContactEmailResultEvent != null)
				{
					_instance.OnAddOrUpdateContactEmailResultEvent((AddOrUpdateContactEmailResult)e.Result);
				}
				else if (type2 == typeof(AddSharedGroupMembersResult) && _instance.OnAddSharedGroupMembersResultEvent != null)
				{
					_instance.OnAddSharedGroupMembersResultEvent((AddSharedGroupMembersResult)e.Result);
				}
				else if (type2 == typeof(AddUsernamePasswordResult) && _instance.OnAddUsernamePasswordResultEvent != null)
				{
					_instance.OnAddUsernamePasswordResultEvent((AddUsernamePasswordResult)e.Result);
				}
				else if (type2 == typeof(ModifyUserVirtualCurrencyResult) && _instance.OnAddUserVirtualCurrencyResultEvent != null)
				{
					_instance.OnAddUserVirtualCurrencyResultEvent((ModifyUserVirtualCurrencyResult)e.Result);
				}
				else if (type2 == typeof(AndroidDevicePushNotificationRegistrationResult) && _instance.OnAndroidDevicePushNotificationRegistrationResultEvent != null)
				{
					_instance.OnAndroidDevicePushNotificationRegistrationResultEvent((AndroidDevicePushNotificationRegistrationResult)e.Result);
				}
				else if (type2 == typeof(AttributeInstallResult) && _instance.OnAttributeInstallResultEvent != null)
				{
					_instance.OnAttributeInstallResultEvent((AttributeInstallResult)e.Result);
				}
				else if (type2 == typeof(CancelTradeResponse) && _instance.OnCancelTradeResultEvent != null)
				{
					_instance.OnCancelTradeResultEvent((CancelTradeResponse)e.Result);
				}
				else if (type2 == typeof(ConfirmPurchaseResult) && _instance.OnConfirmPurchaseResultEvent != null)
				{
					_instance.OnConfirmPurchaseResultEvent((ConfirmPurchaseResult)e.Result);
				}
				else if (type2 == typeof(ConsumeItemResult) && _instance.OnConsumeItemResultEvent != null)
				{
					_instance.OnConsumeItemResultEvent((ConsumeItemResult)e.Result);
				}
				else if (type2 == typeof(CreateSharedGroupResult) && _instance.OnCreateSharedGroupResultEvent != null)
				{
					_instance.OnCreateSharedGroupResultEvent((CreateSharedGroupResult)e.Result);
				}
				else if (type2 == typeof(ExecuteCloudScriptResult) && _instance.OnExecuteCloudScriptResultEvent != null)
				{
					_instance.OnExecuteCloudScriptResultEvent((ExecuteCloudScriptResult)e.Result);
				}
				else if (type2 == typeof(GetAccountInfoResult) && _instance.OnGetAccountInfoResultEvent != null)
				{
					_instance.OnGetAccountInfoResultEvent((GetAccountInfoResult)e.Result);
				}
				else if (type2 == typeof(ListUsersCharactersResult) && _instance.OnGetAllUsersCharactersResultEvent != null)
				{
					_instance.OnGetAllUsersCharactersResultEvent((ListUsersCharactersResult)e.Result);
				}
				else if (type2 == typeof(GetCatalogItemsResult) && _instance.OnGetCatalogItemsResultEvent != null)
				{
					_instance.OnGetCatalogItemsResultEvent((GetCatalogItemsResult)e.Result);
				}
				else if (type2 == typeof(GetCharacterDataResult) && _instance.OnGetCharacterDataResultEvent != null)
				{
					_instance.OnGetCharacterDataResultEvent((GetCharacterDataResult)e.Result);
				}
				else if (type2 == typeof(GetCharacterInventoryResult) && _instance.OnGetCharacterInventoryResultEvent != null)
				{
					_instance.OnGetCharacterInventoryResultEvent((GetCharacterInventoryResult)e.Result);
				}
				else if (type2 == typeof(GetCharacterLeaderboardResult) && _instance.OnGetCharacterLeaderboardResultEvent != null)
				{
					_instance.OnGetCharacterLeaderboardResultEvent((GetCharacterLeaderboardResult)e.Result);
				}
				else if (type2 == typeof(GetCharacterDataResult) && _instance.OnGetCharacterReadOnlyDataResultEvent != null)
				{
					_instance.OnGetCharacterReadOnlyDataResultEvent((GetCharacterDataResult)e.Result);
				}
				else if (type2 == typeof(GetCharacterStatisticsResult) && _instance.OnGetCharacterStatisticsResultEvent != null)
				{
					_instance.OnGetCharacterStatisticsResultEvent((GetCharacterStatisticsResult)e.Result);
				}
				else if (type2 == typeof(GetContentDownloadUrlResult) && _instance.OnGetContentDownloadUrlResultEvent != null)
				{
					_instance.OnGetContentDownloadUrlResultEvent((GetContentDownloadUrlResult)e.Result);
				}
				else if (type2 == typeof(CurrentGamesResult) && _instance.OnGetCurrentGamesResultEvent != null)
				{
					_instance.OnGetCurrentGamesResultEvent((CurrentGamesResult)e.Result);
				}
				else if (type2 == typeof(GetLeaderboardResult) && _instance.OnGetFriendLeaderboardResultEvent != null)
				{
					_instance.OnGetFriendLeaderboardResultEvent((GetLeaderboardResult)e.Result);
				}
				else if (type2 == typeof(GetFriendLeaderboardAroundPlayerResult) && _instance.OnGetFriendLeaderboardAroundPlayerResultEvent != null)
				{
					_instance.OnGetFriendLeaderboardAroundPlayerResultEvent((GetFriendLeaderboardAroundPlayerResult)e.Result);
				}
				else if (type2 == typeof(GetFriendsListResult) && _instance.OnGetFriendsListResultEvent != null)
				{
					_instance.OnGetFriendsListResultEvent((GetFriendsListResult)e.Result);
				}
				else if (type2 == typeof(GameServerRegionsResult) && _instance.OnGetGameServerRegionsResultEvent != null)
				{
					_instance.OnGetGameServerRegionsResultEvent((GameServerRegionsResult)e.Result);
				}
				else if (type2 == typeof(GetLeaderboardResult) && _instance.OnGetLeaderboardResultEvent != null)
				{
					_instance.OnGetLeaderboardResultEvent((GetLeaderboardResult)e.Result);
				}
				else if (type2 == typeof(GetLeaderboardAroundCharacterResult) && _instance.OnGetLeaderboardAroundCharacterResultEvent != null)
				{
					_instance.OnGetLeaderboardAroundCharacterResultEvent((GetLeaderboardAroundCharacterResult)e.Result);
				}
				else if (type2 == typeof(GetLeaderboardAroundPlayerResult) && _instance.OnGetLeaderboardAroundPlayerResultEvent != null)
				{
					_instance.OnGetLeaderboardAroundPlayerResultEvent((GetLeaderboardAroundPlayerResult)e.Result);
				}
				else if (type2 == typeof(GetLeaderboardForUsersCharactersResult) && _instance.OnGetLeaderboardForUserCharactersResultEvent != null)
				{
					_instance.OnGetLeaderboardForUserCharactersResultEvent((GetLeaderboardForUsersCharactersResult)e.Result);
				}
				else if (type2 == typeof(GetPaymentTokenResult) && _instance.OnGetPaymentTokenResultEvent != null)
				{
					_instance.OnGetPaymentTokenResultEvent((GetPaymentTokenResult)e.Result);
				}
				else if (type2 == typeof(GetPhotonAuthenticationTokenResult) && _instance.OnGetPhotonAuthenticationTokenResultEvent != null)
				{
					_instance.OnGetPhotonAuthenticationTokenResultEvent((GetPhotonAuthenticationTokenResult)e.Result);
				}
				else if (type2 == typeof(GetPlayerCombinedInfoResult) && _instance.OnGetPlayerCombinedInfoResultEvent != null)
				{
					_instance.OnGetPlayerCombinedInfoResultEvent((GetPlayerCombinedInfoResult)e.Result);
				}
				else if (type2 == typeof(GetPlayerProfileResult) && _instance.OnGetPlayerProfileResultEvent != null)
				{
					_instance.OnGetPlayerProfileResultEvent((GetPlayerProfileResult)e.Result);
				}
				else if (type2 == typeof(GetPlayerSegmentsResult) && _instance.OnGetPlayerSegmentsResultEvent != null)
				{
					_instance.OnGetPlayerSegmentsResultEvent((GetPlayerSegmentsResult)e.Result);
				}
				else if (type2 == typeof(GetPlayerStatisticsResult) && _instance.OnGetPlayerStatisticsResultEvent != null)
				{
					_instance.OnGetPlayerStatisticsResultEvent((GetPlayerStatisticsResult)e.Result);
				}
				else if (type2 == typeof(GetPlayerStatisticVersionsResult) && _instance.OnGetPlayerStatisticVersionsResultEvent != null)
				{
					_instance.OnGetPlayerStatisticVersionsResultEvent((GetPlayerStatisticVersionsResult)e.Result);
				}
				else if (type2 == typeof(GetPlayerTagsResult) && _instance.OnGetPlayerTagsResultEvent != null)
				{
					_instance.OnGetPlayerTagsResultEvent((GetPlayerTagsResult)e.Result);
				}
				else if (type2 == typeof(GetPlayerTradesResponse) && _instance.OnGetPlayerTradesResultEvent != null)
				{
					_instance.OnGetPlayerTradesResultEvent((GetPlayerTradesResponse)e.Result);
				}
				else if (type2 == typeof(GetPlayFabIDsFromFacebookIDsResult) && _instance.OnGetPlayFabIDsFromFacebookIDsResultEvent != null)
				{
					_instance.OnGetPlayFabIDsFromFacebookIDsResultEvent((GetPlayFabIDsFromFacebookIDsResult)e.Result);
				}
				else if (type2 == typeof(GetPlayFabIDsFromGameCenterIDsResult) && _instance.OnGetPlayFabIDsFromGameCenterIDsResultEvent != null)
				{
					_instance.OnGetPlayFabIDsFromGameCenterIDsResultEvent((GetPlayFabIDsFromGameCenterIDsResult)e.Result);
				}
				else if (type2 == typeof(GetPlayFabIDsFromGenericIDsResult) && _instance.OnGetPlayFabIDsFromGenericIDsResultEvent != null)
				{
					_instance.OnGetPlayFabIDsFromGenericIDsResultEvent((GetPlayFabIDsFromGenericIDsResult)e.Result);
				}
				else if (type2 == typeof(GetPlayFabIDsFromGoogleIDsResult) && _instance.OnGetPlayFabIDsFromGoogleIDsResultEvent != null)
				{
					_instance.OnGetPlayFabIDsFromGoogleIDsResultEvent((GetPlayFabIDsFromGoogleIDsResult)e.Result);
				}
				else if (type2 == typeof(GetPlayFabIDsFromKongregateIDsResult) && _instance.OnGetPlayFabIDsFromKongregateIDsResultEvent != null)
				{
					_instance.OnGetPlayFabIDsFromKongregateIDsResultEvent((GetPlayFabIDsFromKongregateIDsResult)e.Result);
				}
				else if (type2 == typeof(GetPlayFabIDsFromSteamIDsResult) && _instance.OnGetPlayFabIDsFromSteamIDsResultEvent != null)
				{
					_instance.OnGetPlayFabIDsFromSteamIDsResultEvent((GetPlayFabIDsFromSteamIDsResult)e.Result);
				}
				else if (type2 == typeof(GetPlayFabIDsFromTwitchIDsResult) && _instance.OnGetPlayFabIDsFromTwitchIDsResultEvent != null)
				{
					_instance.OnGetPlayFabIDsFromTwitchIDsResultEvent((GetPlayFabIDsFromTwitchIDsResult)e.Result);
				}
				else if (type2 == typeof(GetPublisherDataResult) && _instance.OnGetPublisherDataResultEvent != null)
				{
					_instance.OnGetPublisherDataResultEvent((GetPublisherDataResult)e.Result);
				}
				else if (type2 == typeof(GetPurchaseResult) && _instance.OnGetPurchaseResultEvent != null)
				{
					_instance.OnGetPurchaseResultEvent((GetPurchaseResult)e.Result);
				}
				else if (type2 == typeof(GetSharedGroupDataResult) && _instance.OnGetSharedGroupDataResultEvent != null)
				{
					_instance.OnGetSharedGroupDataResultEvent((GetSharedGroupDataResult)e.Result);
				}
				else if (type2 == typeof(GetStoreItemsResult) && _instance.OnGetStoreItemsResultEvent != null)
				{
					_instance.OnGetStoreItemsResultEvent((GetStoreItemsResult)e.Result);
				}
				else if (type2 == typeof(GetTimeResult) && _instance.OnGetTimeResultEvent != null)
				{
					_instance.OnGetTimeResultEvent((GetTimeResult)e.Result);
				}
				else if (type2 == typeof(GetTitleDataResult) && _instance.OnGetTitleDataResultEvent != null)
				{
					_instance.OnGetTitleDataResultEvent((GetTitleDataResult)e.Result);
				}
				else if (type2 == typeof(GetTitleNewsResult) && _instance.OnGetTitleNewsResultEvent != null)
				{
					_instance.OnGetTitleNewsResultEvent((GetTitleNewsResult)e.Result);
				}
				else if (type2 == typeof(GetTitlePublicKeyResult) && _instance.OnGetTitlePublicKeyResultEvent != null)
				{
					_instance.OnGetTitlePublicKeyResultEvent((GetTitlePublicKeyResult)e.Result);
				}
				else if (type2 == typeof(GetTradeStatusResponse) && _instance.OnGetTradeStatusResultEvent != null)
				{
					_instance.OnGetTradeStatusResultEvent((GetTradeStatusResponse)e.Result);
				}
				else if (type2 == typeof(GetUserDataResult) && _instance.OnGetUserDataResultEvent != null)
				{
					_instance.OnGetUserDataResultEvent((GetUserDataResult)e.Result);
				}
				else if (type2 == typeof(GetUserInventoryResult) && _instance.OnGetUserInventoryResultEvent != null)
				{
					_instance.OnGetUserInventoryResultEvent((GetUserInventoryResult)e.Result);
				}
				else if (type2 == typeof(GetUserDataResult) && _instance.OnGetUserPublisherDataResultEvent != null)
				{
					_instance.OnGetUserPublisherDataResultEvent((GetUserDataResult)e.Result);
				}
				else if (type2 == typeof(GetUserDataResult) && _instance.OnGetUserPublisherReadOnlyDataResultEvent != null)
				{
					_instance.OnGetUserPublisherReadOnlyDataResultEvent((GetUserDataResult)e.Result);
				}
				else if (type2 == typeof(GetUserDataResult) && _instance.OnGetUserReadOnlyDataResultEvent != null)
				{
					_instance.OnGetUserReadOnlyDataResultEvent((GetUserDataResult)e.Result);
				}
				else if (type2 == typeof(GetWindowsHelloChallengeResponse) && _instance.OnGetWindowsHelloChallengeResultEvent != null)
				{
					_instance.OnGetWindowsHelloChallengeResultEvent((GetWindowsHelloChallengeResponse)e.Result);
				}
				else if (type2 == typeof(GrantCharacterToUserResult) && _instance.OnGrantCharacterToUserResultEvent != null)
				{
					_instance.OnGrantCharacterToUserResultEvent((GrantCharacterToUserResult)e.Result);
				}
				else if (type2 == typeof(LinkAndroidDeviceIDResult) && _instance.OnLinkAndroidDeviceIDResultEvent != null)
				{
					_instance.OnLinkAndroidDeviceIDResultEvent((LinkAndroidDeviceIDResult)e.Result);
				}
				else if (type2 == typeof(LinkCustomIDResult) && _instance.OnLinkCustomIDResultEvent != null)
				{
					_instance.OnLinkCustomIDResultEvent((LinkCustomIDResult)e.Result);
				}
				else if (type2 == typeof(LinkFacebookAccountResult) && _instance.OnLinkFacebookAccountResultEvent != null)
				{
					_instance.OnLinkFacebookAccountResultEvent((LinkFacebookAccountResult)e.Result);
				}
				else if (type2 == typeof(LinkGameCenterAccountResult) && _instance.OnLinkGameCenterAccountResultEvent != null)
				{
					_instance.OnLinkGameCenterAccountResultEvent((LinkGameCenterAccountResult)e.Result);
				}
				else if (type2 == typeof(LinkGoogleAccountResult) && _instance.OnLinkGoogleAccountResultEvent != null)
				{
					_instance.OnLinkGoogleAccountResultEvent((LinkGoogleAccountResult)e.Result);
				}
				else if (type2 == typeof(LinkIOSDeviceIDResult) && _instance.OnLinkIOSDeviceIDResultEvent != null)
				{
					_instance.OnLinkIOSDeviceIDResultEvent((LinkIOSDeviceIDResult)e.Result);
				}
				else if (type2 == typeof(LinkKongregateAccountResult) && _instance.OnLinkKongregateResultEvent != null)
				{
					_instance.OnLinkKongregateResultEvent((LinkKongregateAccountResult)e.Result);
				}
				else if (type2 == typeof(LinkSteamAccountResult) && _instance.OnLinkSteamAccountResultEvent != null)
				{
					_instance.OnLinkSteamAccountResultEvent((LinkSteamAccountResult)e.Result);
				}
				else if (type2 == typeof(LinkTwitchAccountResult) && _instance.OnLinkTwitchResultEvent != null)
				{
					_instance.OnLinkTwitchResultEvent((LinkTwitchAccountResult)e.Result);
				}
				else if (type2 == typeof(LinkWindowsHelloAccountResponse) && _instance.OnLinkWindowsHelloResultEvent != null)
				{
					_instance.OnLinkWindowsHelloResultEvent((LinkWindowsHelloAccountResponse)e.Result);
				}
				else if (type2 == typeof(MatchmakeResult) && _instance.OnMatchmakeResultEvent != null)
				{
					_instance.OnMatchmakeResultEvent((MatchmakeResult)e.Result);
				}
				else if (type2 == typeof(OpenTradeResponse) && _instance.OnOpenTradeResultEvent != null)
				{
					_instance.OnOpenTradeResultEvent((OpenTradeResponse)e.Result);
				}
				else if (type2 == typeof(PayForPurchaseResult) && _instance.OnPayForPurchaseResultEvent != null)
				{
					_instance.OnPayForPurchaseResultEvent((PayForPurchaseResult)e.Result);
				}
				else if (type2 == typeof(PurchaseItemResult) && _instance.OnPurchaseItemResultEvent != null)
				{
					_instance.OnPurchaseItemResultEvent((PurchaseItemResult)e.Result);
				}
				else if (type2 == typeof(RedeemCouponResult) && _instance.OnRedeemCouponResultEvent != null)
				{
					_instance.OnRedeemCouponResultEvent((RedeemCouponResult)e.Result);
				}
				else if (type2 == typeof(RegisterForIOSPushNotificationResult) && _instance.OnRegisterForIOSPushNotificationResultEvent != null)
				{
					_instance.OnRegisterForIOSPushNotificationResultEvent((RegisterForIOSPushNotificationResult)e.Result);
				}
				else if (type2 == typeof(RegisterPlayFabUserResult) && _instance.OnRegisterPlayFabUserResultEvent != null)
				{
					_instance.OnRegisterPlayFabUserResultEvent((RegisterPlayFabUserResult)e.Result);
				}
				else if (type2 == typeof(RemoveContactEmailResult) && _instance.OnRemoveContactEmailResultEvent != null)
				{
					_instance.OnRemoveContactEmailResultEvent((RemoveContactEmailResult)e.Result);
				}
				else if (type2 == typeof(RemoveFriendResult) && _instance.OnRemoveFriendResultEvent != null)
				{
					_instance.OnRemoveFriendResultEvent((RemoveFriendResult)e.Result);
				}
				else if (type2 == typeof(RemoveGenericIDResult) && _instance.OnRemoveGenericIDResultEvent != null)
				{
					_instance.OnRemoveGenericIDResultEvent((RemoveGenericIDResult)e.Result);
				}
				else if (type2 == typeof(RemoveSharedGroupMembersResult) && _instance.OnRemoveSharedGroupMembersResultEvent != null)
				{
					_instance.OnRemoveSharedGroupMembersResultEvent((RemoveSharedGroupMembersResult)e.Result);
				}
				else if (type2 == typeof(EmptyResult) && _instance.OnReportDeviceInfoResultEvent != null)
				{
					_instance.OnReportDeviceInfoResultEvent((EmptyResult)e.Result);
				}
				else if (type2 == typeof(ReportPlayerClientResult) && _instance.OnReportPlayerResultEvent != null)
				{
					_instance.OnReportPlayerResultEvent((ReportPlayerClientResult)e.Result);
				}
				else if (type2 == typeof(RestoreIOSPurchasesResult) && _instance.OnRestoreIOSPurchasesResultEvent != null)
				{
					_instance.OnRestoreIOSPurchasesResultEvent((RestoreIOSPurchasesResult)e.Result);
				}
				else if (type2 == typeof(SendAccountRecoveryEmailResult) && _instance.OnSendAccountRecoveryEmailResultEvent != null)
				{
					_instance.OnSendAccountRecoveryEmailResultEvent((SendAccountRecoveryEmailResult)e.Result);
				}
				else if (type2 == typeof(SetFriendTagsResult) && _instance.OnSetFriendTagsResultEvent != null)
				{
					_instance.OnSetFriendTagsResultEvent((SetFriendTagsResult)e.Result);
				}
				else if (type2 == typeof(SetPlayerSecretResult) && _instance.OnSetPlayerSecretResultEvent != null)
				{
					_instance.OnSetPlayerSecretResultEvent((SetPlayerSecretResult)e.Result);
				}
				else if (type2 == typeof(StartGameResult) && _instance.OnStartGameResultEvent != null)
				{
					_instance.OnStartGameResultEvent((StartGameResult)e.Result);
				}
				else if (type2 == typeof(StartPurchaseResult) && _instance.OnStartPurchaseResultEvent != null)
				{
					_instance.OnStartPurchaseResultEvent((StartPurchaseResult)e.Result);
				}
				else if (type2 == typeof(ModifyUserVirtualCurrencyResult) && _instance.OnSubtractUserVirtualCurrencyResultEvent != null)
				{
					_instance.OnSubtractUserVirtualCurrencyResultEvent((ModifyUserVirtualCurrencyResult)e.Result);
				}
				else if (type2 == typeof(UnlinkAndroidDeviceIDResult) && _instance.OnUnlinkAndroidDeviceIDResultEvent != null)
				{
					_instance.OnUnlinkAndroidDeviceIDResultEvent((UnlinkAndroidDeviceIDResult)e.Result);
				}
				else if (type2 == typeof(UnlinkCustomIDResult) && _instance.OnUnlinkCustomIDResultEvent != null)
				{
					_instance.OnUnlinkCustomIDResultEvent((UnlinkCustomIDResult)e.Result);
				}
				else if (type2 == typeof(UnlinkFacebookAccountResult) && _instance.OnUnlinkFacebookAccountResultEvent != null)
				{
					_instance.OnUnlinkFacebookAccountResultEvent((UnlinkFacebookAccountResult)e.Result);
				}
				else if (type2 == typeof(UnlinkGameCenterAccountResult) && _instance.OnUnlinkGameCenterAccountResultEvent != null)
				{
					_instance.OnUnlinkGameCenterAccountResultEvent((UnlinkGameCenterAccountResult)e.Result);
				}
				else if (type2 == typeof(UnlinkGoogleAccountResult) && _instance.OnUnlinkGoogleAccountResultEvent != null)
				{
					_instance.OnUnlinkGoogleAccountResultEvent((UnlinkGoogleAccountResult)e.Result);
				}
				else if (type2 == typeof(UnlinkIOSDeviceIDResult) && _instance.OnUnlinkIOSDeviceIDResultEvent != null)
				{
					_instance.OnUnlinkIOSDeviceIDResultEvent((UnlinkIOSDeviceIDResult)e.Result);
				}
				else if (type2 == typeof(UnlinkKongregateAccountResult) && _instance.OnUnlinkKongregateResultEvent != null)
				{
					_instance.OnUnlinkKongregateResultEvent((UnlinkKongregateAccountResult)e.Result);
				}
				else if (type2 == typeof(UnlinkSteamAccountResult) && _instance.OnUnlinkSteamAccountResultEvent != null)
				{
					_instance.OnUnlinkSteamAccountResultEvent((UnlinkSteamAccountResult)e.Result);
				}
				else if (type2 == typeof(UnlinkTwitchAccountResult) && _instance.OnUnlinkTwitchResultEvent != null)
				{
					_instance.OnUnlinkTwitchResultEvent((UnlinkTwitchAccountResult)e.Result);
				}
				else if (type2 == typeof(UnlinkWindowsHelloAccountResponse) && _instance.OnUnlinkWindowsHelloResultEvent != null)
				{
					_instance.OnUnlinkWindowsHelloResultEvent((UnlinkWindowsHelloAccountResponse)e.Result);
				}
				else if (type2 == typeof(UnlockContainerItemResult) && _instance.OnUnlockContainerInstanceResultEvent != null)
				{
					_instance.OnUnlockContainerInstanceResultEvent((UnlockContainerItemResult)e.Result);
				}
				else if (type2 == typeof(UnlockContainerItemResult) && _instance.OnUnlockContainerItemResultEvent != null)
				{
					_instance.OnUnlockContainerItemResultEvent((UnlockContainerItemResult)e.Result);
				}
				else if (type2 == typeof(EmptyResult) && _instance.OnUpdateAvatarUrlResultEvent != null)
				{
					_instance.OnUpdateAvatarUrlResultEvent((EmptyResult)e.Result);
				}
				else if (type2 == typeof(UpdateCharacterDataResult) && _instance.OnUpdateCharacterDataResultEvent != null)
				{
					_instance.OnUpdateCharacterDataResultEvent((UpdateCharacterDataResult)e.Result);
				}
				else if (type2 == typeof(UpdateCharacterStatisticsResult) && _instance.OnUpdateCharacterStatisticsResultEvent != null)
				{
					_instance.OnUpdateCharacterStatisticsResultEvent((UpdateCharacterStatisticsResult)e.Result);
				}
				else if (type2 == typeof(UpdatePlayerStatisticsResult) && _instance.OnUpdatePlayerStatisticsResultEvent != null)
				{
					_instance.OnUpdatePlayerStatisticsResultEvent((UpdatePlayerStatisticsResult)e.Result);
				}
				else if (type2 == typeof(UpdateSharedGroupDataResult) && _instance.OnUpdateSharedGroupDataResultEvent != null)
				{
					_instance.OnUpdateSharedGroupDataResultEvent((UpdateSharedGroupDataResult)e.Result);
				}
				else if (type2 == typeof(UpdateUserDataResult) && _instance.OnUpdateUserDataResultEvent != null)
				{
					_instance.OnUpdateUserDataResultEvent((UpdateUserDataResult)e.Result);
				}
				else if (type2 == typeof(UpdateUserDataResult) && _instance.OnUpdateUserPublisherDataResultEvent != null)
				{
					_instance.OnUpdateUserPublisherDataResultEvent((UpdateUserDataResult)e.Result);
				}
				else if (type2 == typeof(UpdateUserTitleDisplayNameResult) && _instance.OnUpdateUserTitleDisplayNameResultEvent != null)
				{
					_instance.OnUpdateUserTitleDisplayNameResultEvent((UpdateUserTitleDisplayNameResult)e.Result);
				}
				else if (type2 == typeof(ValidateAmazonReceiptResult) && _instance.OnValidateAmazonIAPReceiptResultEvent != null)
				{
					_instance.OnValidateAmazonIAPReceiptResultEvent((ValidateAmazonReceiptResult)e.Result);
				}
				else if (type2 == typeof(ValidateGooglePlayPurchaseResult) && _instance.OnValidateGooglePlayPurchaseResultEvent != null)
				{
					_instance.OnValidateGooglePlayPurchaseResultEvent((ValidateGooglePlayPurchaseResult)e.Result);
				}
				else if (type2 == typeof(ValidateIOSReceiptResult) && _instance.OnValidateIOSReceiptResultEvent != null)
				{
					_instance.OnValidateIOSReceiptResultEvent((ValidateIOSReceiptResult)e.Result);
				}
				else if (type2 == typeof(ValidateWindowsReceiptResult) && _instance.OnValidateWindowsStoreReceiptResultEvent != null)
				{
					_instance.OnValidateWindowsStoreReceiptResultEvent((ValidateWindowsReceiptResult)e.Result);
				}
				else if (type2 == typeof(WriteEventResponse) && _instance.OnWriteCharacterEventResultEvent != null)
				{
					_instance.OnWriteCharacterEventResultEvent((WriteEventResponse)e.Result);
				}
				else if (type2 == typeof(WriteEventResponse) && _instance.OnWritePlayerEventResultEvent != null)
				{
					_instance.OnWritePlayerEventResultEvent((WriteEventResponse)e.Result);
				}
				else if (type2 == typeof(WriteEventResponse) && _instance.OnWriteTitleEventResultEvent != null)
				{
					_instance.OnWriteTitleEventResultEvent((WriteEventResponse)e.Result);
				}
			}
		}
	}
}