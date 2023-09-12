using FluentAssertions.Extensions;

namespace MatchmakingSignalingServer.Domain.Test;

public class GameSessionTests
{
    const string VALID_GAME_SESSION_NAME = "Test New Game";
    const string VALID_PLAYER_HOST_NAME = "Hosting Player";
    const string VALID_PLAYER_NAME = "Client Player";
    const string VALID_NONEXISTENT_PLAYER_NAME = "Non Existant Player";

    IceCandidate ValidIceCandidate => IceCandidate.With("abc", "4", "medianame");

    PlayerName ValidPlayerHostName => PlayerName.Create(VALID_PLAYER_HOST_NAME);
    
    PlayerName ValidPlayerName => PlayerName.Create(VALID_PLAYER_NAME);

    PlayerName ValidPlayerNameButDoesntExist => PlayerName.Create(VALID_NONEXISTENT_PLAYER_NAME);

    GameSessionName ValidGameSessionName => GameSessionName.Create(VALID_GAME_SESSION_NAME); 

    GameSession ValidNewGameSession => new GameSession(ValidGameSessionName, ValidPlayerHostName);

    GameSession ValidGameSessionWithClientAndPlayer
    {
        get
        {
            var gameSession = ValidNewGameSession;
            gameSession.AddPlayerToGameSession(ValidPlayerName);
            return gameSession;
        }
    }
    GameSession ValidGameSessionWithClientAndConnectedPlayer
    {
        get
        {
            var gameSession = ValidNewGameSession;
            gameSession.AddPlayerToGameSession(ValidPlayerName);
            gameSession.UpdatePlayerToConnected(ValidPlayerName);
            return gameSession;
        }
    }

    [Fact]
    public void AddPlayerToGameSession_PlayerAlreadyExistsInGame_ThrowInvalidOperation()
    {
        var sut = ValidNewGameSession;

        Action testAction = () => sut.AddPlayerToGameSession(ValidPlayerHostName);

        testAction.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AddPlayerToGameSession_PlayerDoesntExistInGame_PlayerIsInClientsAndSignalingStepsCreated()
    {
        var sut = ValidNewGameSession;

        sut.AddPlayerToGameSession(ValidPlayerName);

        sut.Clients.Should().HaveCount(2);
        sut.Steps.Should().HaveCount(1);
    }

    [Fact]
    public void IsPlayerHost_PlayerIs_ReturnsTrue()
    {
        var sut = ValidNewGameSession;

        var result = sut.IsPlayerHost(ValidPlayerHostName);

        result.Should().BeTrue();
    }

    [Fact]
    public void IsPlayerHost_PlayerIsNot_ReturnsFalse()
    {
        var sut = ValidGameSessionWithClientAndPlayer;

        var result = sut.IsPlayerHost(ValidPlayerName);

        result.Should().BeFalse();
    }

    [Fact]
    public void RemovePlayer_PlayerDoesntExist_ThrowsInvalidOperation()
    {
        var sut = ValidGameSessionWithClientAndPlayer;

        Action testAction = () => sut.RemovePlayer(ValidPlayerNameButDoesntExist);

        testAction.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void RemovePlayer_PlayerExistsAsClient_SignalingStepsRelatedToPlayerAreGoneClientsMissingPlayer()
    {
        var sut = ValidGameSessionWithClientAndPlayer;

        sut.RemovePlayer(ValidPlayerName);

        sut.Clients.Should().HaveCount(1);
        sut.Steps.Should().HaveCount(0);
    }

    [Fact]
    public void RemovePlayer_PlayerExistsAsHost_ThrowInvalidOperation()
    {
        var sut = ValidGameSessionWithClientAndPlayer;

        Action testAction = () => sut.RemovePlayer(ValidPlayerNameButDoesntExist);

        testAction.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetSignalingStepToPlayer_PlayerDoesntExist_ThrowInvalidOperation()
    {
        var sut = ValidGameSessionWithClientAndPlayer;

        Action testAction = () => sut.GetSignalingStepToPlayer(ValidPlayerNameButDoesntExist);

        testAction.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetSignalingStepToPlayer_PlayerExists_SignalingStepTargetingPlayerReturned()
    {
        var sut = ValidGameSessionWithClientAndPlayer;

        var result = sut.GetSignalingStepToPlayer(ValidPlayerName);

        result.Target.Should().Be(ValidPlayerName);
        result.Source.Should().Be(ValidPlayerHostName);
        result.InformationType.Should().Be(InformationType.WAITING_FOR_INFORMATION);
    }

    [Fact]
    public void GetSignalingStepsForHost_AtLeastOnePlayerConnected_SignalingStepsTargetingHostReturned()
    {
        var sut = ValidGameSessionWithClientAndPlayer;
        sut.Steps.Clear();
        sut.Steps.Add(new SignalingStep(ValidPlayerName, ValidPlayerHostName));

        var result = sut.GetSignalingStepsForHost();

        result.Should().HaveCount(1);

        var firstStep = result.First();
        firstStep.Target.Should().Be(ValidPlayerHostName);
        firstStep.Source.Should().Be(ValidPlayerName);
        firstStep.InformationType.Should().Be(InformationType.WAITING_FOR_INFORMATION);
    }

    [Fact]
    public void UpdatePlayerConnected_PlayerDoesntExist_ThrowInvalidOperationException()
    {
        var sut = ValidGameSessionWithClientAndPlayer;

        Action testAction = () => sut.UpdatePlayerToConnected(ValidPlayerNameButDoesntExist);

        testAction.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void UpdatePlayerToConnected_PlayerExists_SetInfoTypeToConnectedAndClearStepsForPlayer()
    {
        var sut = ValidGameSessionWithClientAndPlayer;

        sut.UpdatePlayerToConnected(ValidPlayerName);

        sut.Clients.Skip(1).First().ConnectionState.Should().Be(ConnectionState.CONNECTED);
        sut.Steps.Should().HaveCount(0);
    }

    [Fact]
    public void ReconnectPlayer_PlayerIsHost_ThrowArgumentException()
    {
        var sut = ValidGameSessionWithClientAndPlayer;

        Action testAction = () => sut.ReconnectPlayer(ValidPlayerHostName);

        testAction.Should().Throw<ArgumentException>();
    }
    
    [Fact]
    public void ReconnectPlayer_PlayerIsClient_SignalResetAndPlayerConnectionStateConnecting()
    {
        var sut = ValidGameSessionWithClientAndConnectedPlayer;

        sut.ReconnectPlayer(ValidPlayerName);

        sut.Clients.Skip(1).First().ConnectionState.Should().Be(ConnectionState.CONNECTING);
        sut.Steps.Should().HaveCount(1);
    }

    [Fact]
    public void UpdatePlayerExpirationTime_PlayerIsClient_Expiration5MinsLater()
    {
        var sut = ValidGameSessionWithClientAndConnectedPlayer;

        sut.UpdatePlayerExpirationTime(ValidPlayerName);

        sut.Clients.Skip(1).First().Expiration.Should().BeAtLeast(4.Minutes()).After(DateTime.UtcNow);
    }

    [Fact]
    public void ReverseStepAndUpdate_PlayerClientWasTarget_HostIsTargetAndNetworkInfoUpdated()
    {
        var sut = ValidGameSessionWithClientAndConnectedPlayer;
        sut.Steps.Clear();
        sut.Steps.Add(new SignalingStep(ValidPlayerHostName, ValidPlayerName));

        sut.SetHostAsTarget(ValidPlayerName, InformationType.ICE_CANDIDATE, ValidIceCandidate, null);

        sut.Steps.First().IceCandidate.Should().Be(ValidIceCandidate);
        sut.Steps.First().Target.Should().Be(ValidPlayerHostName);
        sut.Steps.First().Source.Should().Be(ValidPlayerName);
    }

    [Fact]
    public void ReverseStepAndUpdate_HostWasTarget_PlayerIsTargetAndNetworkInfoUpdated()
    {
        var sut = ValidGameSessionWithClientAndConnectedPlayer;
        sut.Steps.Clear();
        sut.Steps.Add(new SignalingStep(ValidPlayerName, ValidPlayerHostName));

        sut.SetConnectedPlayerAsTarget(ValidPlayerName, InformationType.ICE_CANDIDATE, ValidIceCandidate, null);

        sut.Steps.First().IceCandidate.Should().Be(ValidIceCandidate);
        sut.Steps.First().Target.Should().Be(ValidPlayerName);
        sut.Steps.First().Source.Should().Be(ValidPlayerHostName);
    }
}
