namespace DiscogsApiClient.Contract.Release;

/// <summary>
/// Represents a video.
/// </summary>
/// <param name="Uri">The video url</param>
/// <param name="Title">Video title</param>
/// <param name="Description">Video description</param>
/// <param name="DurationInSeconds">Video duration in seconds</param>
/// <param name="IsEmbedded"></param>
public sealed record Video(
    [property:JsonPropertyName("uri")]
    string Uri,
    [property:JsonPropertyName("title")]
    string Title,
    [property:JsonPropertyName("description")]
    string Description,
    [property:JsonPropertyName("duration")]
    int DurationInSeconds,
    [property:JsonPropertyName("embed")]
    bool IsEmbedded);


/*
"videos":[
    {
        "uri":"https://www.youtube.com/watch?v=QD1j8c3GBAo",
        "title":"HAMMERFALL - GLORY TO THE BRAVE: 20TH ANNIVERSARY LTD. BOXSET EDITION unboxing",
        "description":"All rights reserved!\n\u24c5 + \u24b8 2017\nN u c l e a r    B l a s t   G m b H\n\nB U Y     T H E    M U S I C!\nR E S P E C T    T H E   A R T I S T S! \n\nBUY THE BOXSET HERE: http://www.nuclearblast.de/en/products/tontraeger/cd/2cd-digi-dvd/hammerfall-glory-to-th",
        "duration":121,
        "embed":true
    },
    {
        "uri":"https://www.youtube.com/watch?v=YeySMqUqeWw",
        "title":"HAMMERFALL - Glory to the Brave [Full Album 1997] + B\u00f6nus tracks",
        "description":"0:00:00 - 01.The Dragon Lies Bleeding\n0:04:22 - 02.The Metal Age\n0:08:51 - 03.HammeFall\n0:13:38 - 04.I Believe\n0:18:32 - 05.Child Of The Damned (Warlord cover)\n0:22:15 - 06.Steel Meets Steel\n0:26:17 - 07.Stone Cold\n*\n0:32:00 - 09.Glory To The Brave\n0:39:2",
        "duration":3273,
        "embed":true
    }
],
*/
