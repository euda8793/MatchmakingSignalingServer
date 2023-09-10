namespace MatchmakingSignalingServer.API;

public record ApiResponse(object data, List<string> validationErrors, List<string> serverErrors);
