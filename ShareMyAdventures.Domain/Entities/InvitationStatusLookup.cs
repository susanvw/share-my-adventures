﻿using ShareMyAdventures.Domain.SeedWork;

namespace ShareMyAdventures.Domain.Entities;

public sealed class InvitationStatusLookup : ValueObject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
