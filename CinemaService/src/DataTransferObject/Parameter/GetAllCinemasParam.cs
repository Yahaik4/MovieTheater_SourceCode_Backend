﻿using Shared.Contracts.Interfaces;

namespace src.DataTransferObject.Parameter
{
    public class GetAllCinemasParam : IParam
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? City { get; set; }
        public string? Status { get; set; }
    }
}
