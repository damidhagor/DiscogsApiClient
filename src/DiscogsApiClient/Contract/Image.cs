﻿namespace DiscogsApiClient.Contract;

public sealed record Image(
    ArtistImageType Type,
    string Uri,
    string ResourceUrl,
    string Uri150,
    int Width,
    int Height);
