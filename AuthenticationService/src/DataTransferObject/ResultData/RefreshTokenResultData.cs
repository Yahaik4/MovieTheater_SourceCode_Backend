﻿namespace src.DataTransferObject.ResultData
{
    public class RefreshTokenResultData : BaseResultData
    {
        public RefreshTokenDataResult Data { get; set; } = null!;
    }

    public class RefreshTokenDataResult {
        public string AccessToken { get; set; }
    }
}
