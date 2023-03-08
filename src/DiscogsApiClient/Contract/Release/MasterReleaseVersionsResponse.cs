namespace DiscogsApiClient.Contract.Release;

/// <summary>
/// Returns paginated release versions for a master release
/// </summary>
/// <param name="Pagination">Pagination information</param>
/// <param name="Filters">Available and applied filters for the query</param>
/// <param name="FilterFacets">Filter facets for the query</param>
/// <param name="ReleaseVersions">Release versions</param>
public sealed record MasterReleaseVersionsResponse(
    [property:JsonPropertyName("pagination")]
    Pagination Pagination,
    [property:JsonPropertyName("filters")]
    MasterReleaseVersionFilters Filters,
    [property:JsonPropertyName("filter_facets")]
    List<MasterReleaseVersionFilterFacet> FilterFacets,
    [property:JsonPropertyName("versions")]
    List<MasterReleaseVersion> ReleaseVersions);


/**
{
   "pagination":{
      "page":1,
      "pages":2,
      "per_page":50,
      "items":63,
      "urls":{
         "last":"https://api.discogs.com/masters/156551/versions?page=2&per_page=50",
         "next":"https://api.discogs.com/masters/156551/versions?page=2&per_page=50"
      }
   },
   "filters":{
      "applied":{
         
      },
      "available":{
         "format":{
            "Album":63,
            "CD":35,
            "Reissue":26,
            "Cassette":14,
            "LP":14,
            "Vinyl":14,
            "Limited Edition":13,
            "Unofficial Release":9,
            "Deluxe Edition":8,
            "Enhanced":6,
            "Remastered":6,
            "Stereo":6,
            "All Media":3,
            "DVD":3,
            "DVD-Video":3,
            "Numbered":3,
            "Repress":3,
            "Box Set":2,
            "NTSC":2,
            "CD-ROM":1,
            "PAL":1,
            "Picture Disc":1,
            "Promo":1,
            "Shape":1,
            "Test Pressing":1,
            "White Label":1
         },
         "label":{
            "Nuclear Blast":43,
            "Studio Fredman":14,
            "Nuclear Blast Gmbh":13,
            "The Mastering Factory":12,
            "Technicolor":8,
            "Nuclear Blast Records":7,
            "Vic Records":5,
            "Nuclear Blast Tontr\u00e4ger Produktions- Und Vertriebs Gmbh":4,
            "Battle Hymns Music":3,
            "Angel'S Of Hell Records":2,
            "Bod Berlin Optical Disc":2,
            "Cornucopia Entertainment":2,
            "Dadc":2,
            "Metal Records":2,
            "Moon Records":2,
            "Not On Label (Hammerfall)":2,
            "Reloaded":2,
            "Wizard":2,
            "Wizard Ltd.":2,
            "\u041c\u0443\u0437\u044b\u043a\u0430\u043b\u044c\u043d\u0430\u044f \u041a\u043e\u043b\u043b\u0435\u043a\u0446\u0438\u044f":2,
            "\u041e\u043e\u043e \"\u0421\u043f\u044e\u0440\u043a\"":2,
            "Back On Black":1,
            "Bona Records":1,
            "Cd+":1,
            "Cd+ Ind\u00fastria Da Amaz\u00f4nia Ltda":1,
            "Chaos Reigns":1,
            "Cnr Music":1,
            "Denwa Com\u00e9rcio E Importa\u00e7\u00e3o Ltda.":1,
            "Docdata Germany":1,
            "Eruditus":1,
            "Firepower Records":1,
            "First Town Records":1,
            "Hammer M\u00fczik":1,
            "Heavy Best":1,
            "Irond":1,
            "Laser Company Records":1,
            "Laser Disc Argentina":1,
            "Lumi\u00e8re":1,
            "Metalizer":1,
            "Morbid Noizz Productions":1,
            "Nems Enterprises":1,
            "Nuclear Blast America":1,
            "Nuclear Blast America, Inc.":1,
            "Nuclear Blast Picture-Disc Series":1,
            "Optical Disc De France":1,
            "Phoenix Music":1,
            "Prophecies Publishing":1,
            "Pt. Indo Semar Sakti":1,
            "Pure Metal":1,
            "Record Industry":1,
            "Rock Brigade Records":1,
            "Rocris Disc":1,
            "Shinigami Records":1,
            "Sony Music Marketing Inc.":1,
            "Sound City Records":1,
            "Twister Com\u00e9rcio De Discos E Fitas Ltda.":1,
            "Unknown (Sp)":1,
            "Victor":1,
            "Victor Entertainment, Inc.":1,
            "Wagram Music S.A.":1,
            "Ward Records":1,
            "Ward Records, Inc.":1,
            "Witch Of The East":1,
            "Zph Log":1,
            "\u041f\u0434\u0432 Records":1,
            "\u0420\u0430\u043e":1
         },
         "country":{
            "Germany":24,
            "Russia":8,
            "US":4,
            "Brazil":3,
            "Europe":3,
            "Belarus":2,
            "Bulgaria":2,
            "Colombia":2,
            "Japan":2,
            "USA & Europe":2,
            "Ukraine":2,
            "Argentina":1,
            "Czech Republic":1,
            "France":1,
            "Indonesia":1,
            "Malaysia":1,
            "Mexico":1,
            "Poland":1,
            "Romania":1,
            "Turkey":1
         },
         "released":{
            "1997":21,
            "1998":3,
            "1999":1,
            "2000":1,
            "2001":2,
            "2002":1,
            "2003":3,
            "2005":4,
            "2006":1,
            "2010":3,
            "2011":2,
            "2013":4,
            "2017":4,
            "2018":1,
            "2019":1
         }
      }
   },
   "filter_facets":[
      {
         "title":"Format",
         "id":"format",
         "values":[
            {
               "title":"Album",
               "value":"Album",
               "count":63
            },
            {
               "title":"CD",
               "value":"CD",
               "count":35
            },
            {
               "title":"Reissue",
               "value":"Reissue",
               "count":26
            },
            {
               "title":"Cassette",
               "value":"Cassette",
               "count":14
            },
            {
               "title":"LP",
               "value":"LP",
               "count":14
            },
            {
               "title":"Vinyl",
               "value":"Vinyl",
               "count":14
            },
            {
               "title":"Limited Edition",
               "value":"Limited+Edition",
               "count":13
            },
            {
               "title":"Unofficial Release",
               "value":"Unofficial+Release",
               "count":9
            },
            {
               "title":"Deluxe Edition",
               "value":"Deluxe+Edition",
               "count":8
            },
            {
               "title":"Enhanced",
               "value":"Enhanced",
               "count":6
            },
            {
               "title":"Remastered",
               "value":"Remastered",
               "count":6
            },
            {
               "title":"Stereo",
               "value":"Stereo",
               "count":6
            },
            {
               "title":"All Media",
               "value":"All+Media",
               "count":3
            },
            {
               "title":"DVD",
               "value":"DVD",
               "count":3
            },
            {
               "title":"DVD-Video",
               "value":"DVD-Video",
               "count":3
            },
            {
               "title":"Numbered",
               "value":"Numbered",
               "count":3
            },
            {
               "title":"Repress",
               "value":"Repress",
               "count":3
            },
            {
               "title":"Box Set",
               "value":"Box+Set",
               "count":2
            },
            {
               "title":"NTSC",
               "value":"NTSC",
               "count":2
            },
            {
               "title":"CD-ROM",
               "value":"CD-ROM",
               "count":1
            },
            {
               "title":"PAL",
               "value":"PAL",
               "count":1
            },
            {
               "title":"Picture Disc",
               "value":"Picture+Disc",
               "count":1
            },
            {
               "title":"Promo",
               "value":"Promo",
               "count":1
            },
            {
               "title":"Shape",
               "value":"Shape",
               "count":1
            },
            {
               "title":"Test Pressing",
               "value":"Test+Pressing",
               "count":1
            },
            {
               "title":"White Label",
               "value":"White+Label",
               "count":1
            }
         ],
         "allows_multiple_values":true
      },
      {
         "title":"Label",
         "id":"label",
         "values":[
            {
               "title":"Nuclear Blast",
               "value":"Nuclear+Blast",
               "count":43
            },
            {
               "title":"Studio Fredman",
               "value":"Studio+Fredman",
               "count":14
            },
            {
               "title":"Nuclear Blast Gmbh",
               "value":"Nuclear+Blast+Gmbh",
               "count":13
            },
            {
               "title":"The Mastering Factory",
               "value":"The+Mastering+Factory",
               "count":12
            },
            {
               "title":"Technicolor",
               "value":"Technicolor",
               "count":8
            },
            {
               "title":"Nuclear Blast Records",
               "value":"Nuclear+Blast+Records",
               "count":7
            },
            {
               "title":"Vic Records",
               "value":"Vic+Records",
               "count":5
            },
            {
               "title":"Nuclear Blast Tontr\u00e4ger Produktions- Und Vertriebs Gmbh",
               "value":"Nuclear+Blast+Tontr%C3%A4ger+Produktions-+Und+Vertriebs+Gmbh",
               "count":4
            },
            {
               "title":"Battle Hymns Music",
               "value":"Battle+Hymns+Music",
               "count":3
            },
            {
               "title":"Angel'S Of Hell Records",
               "value":"Angel%27S+Of+Hell+Records",
               "count":2
            },
            {
               "title":"Bod Berlin Optical Disc",
               "value":"Bod+Berlin+Optical+Disc",
               "count":2
            },
            {
               "title":"Cornucopia Entertainment",
               "value":"Cornucopia+Entertainment",
               "count":2
            },
            {
               "title":"Dadc",
               "value":"Dadc",
               "count":2
            },
            {
               "title":"Metal Records",
               "value":"Metal+Records",
               "count":2
            },
            {
               "title":"Moon Records",
               "value":"Moon+Records",
               "count":2
            },
            {
               "title":"Not On Label (Hammerfall)",
               "value":"Not+On+Label+%28Hammerfall%29",
               "count":2
            },
            {
               "title":"Reloaded",
               "value":"Reloaded",
               "count":2
            },
            {
               "title":"Wizard",
               "value":"Wizard",
               "count":2
            },
            {
               "title":"Wizard Ltd.",
               "value":"Wizard+Ltd.",
               "count":2
            },
            {
               "title":"\u041c\u0443\u0437\u044b\u043a\u0430\u043b\u044c\u043d\u0430\u044f \u041a\u043e\u043b\u043b\u0435\u043a\u0446\u0438\u044f",
               "value":"%D0%9C%D1%83%D0%B7%D1%8B%D0%BA%D0%B0%D0%BB%D1%8C%D0%BD%D0%B0%D1%8F+%D0%9A%D0%BE%D0%BB%D0%BB%D0%B5%D0%BA%D1%86%D0%B8%D1%8F",
               "count":2
            },
            {
               "title":"\u041e\u043e\u043e \"\u0421\u043f\u044e\u0440\u043a\"",
               "value":"%D0%9E%D0%BE%D0%BE+%22%D0%A1%D0%BF%D1%8E%D1%80%D0%BA%22",
               "count":2
            },
            {
               "title":"Back On Black",
               "value":"Back+On+Black",
               "count":1
            },
            {
               "title":"Bona Records",
               "value":"Bona+Records",
               "count":1
            },
            {
               "title":"Cd+",
               "value":"Cd%2B",
               "count":1
            },
            {
               "title":"Cd+ Ind\u00fastria Da Amaz\u00f4nia Ltda",
               "value":"Cd%2B+Ind%C3%BAstria+Da+Amaz%C3%B4nia+Ltda",
               "count":1
            },
            {
               "title":"Chaos Reigns",
               "value":"Chaos+Reigns",
               "count":1
            },
            {
               "title":"Cnr Music",
               "value":"Cnr+Music",
               "count":1
            },
            {
               "title":"Denwa Com\u00e9rcio E Importa\u00e7\u00e3o Ltda.",
               "value":"Denwa+Com%C3%A9rcio+E+Importa%C3%A7%C3%A3o+Ltda.",
               "count":1
            },
            {
               "title":"Docdata Germany",
               "value":"Docdata+Germany",
               "count":1
            },
            {
               "title":"Eruditus",
               "value":"Eruditus",
               "count":1
            },
            {
               "title":"Firepower Records",
               "value":"Firepower+Records",
               "count":1
            },
            {
               "title":"First Town Records",
               "value":"First+Town+Records",
               "count":1
            },
            {
               "title":"Hammer M\u00fczik",
               "value":"Hammer+M%C3%BCzik",
               "count":1
            },
            {
               "title":"Heavy Best",
               "value":"Heavy+Best",
               "count":1
            },
            {
               "title":"Irond",
               "value":"Irond",
               "count":1
            },
            {
               "title":"Laser Company Records",
               "value":"Laser+Company+Records",
               "count":1
            },
            {
               "title":"Laser Disc Argentina",
               "value":"Laser+Disc+Argentina",
               "count":1
            },
            {
               "title":"Lumi\u00e8re",
               "value":"Lumi%C3%A8re",
               "count":1
            },
            {
               "title":"Metalizer",
               "value":"Metalizer",
               "count":1
            },
            {
               "title":"Morbid Noizz Productions",
               "value":"Morbid+Noizz+Productions",
               "count":1
            },
            {
               "title":"Nems Enterprises",
               "value":"Nems+Enterprises",
               "count":1
            },
            {
               "title":"Nuclear Blast America",
               "value":"Nuclear+Blast+America",
               "count":1
            },
            {
               "title":"Nuclear Blast America, Inc.",
               "value":"Nuclear+Blast+America%2C+Inc.",
               "count":1
            },
            {
               "title":"Nuclear Blast Picture-Disc Series",
               "value":"Nuclear+Blast+Picture-Disc+Series",
               "count":1
            },
            {
               "title":"Optical Disc De France",
               "value":"Optical+Disc+De+France",
               "count":1
            },
            {
               "title":"Phoenix Music",
               "value":"Phoenix+Music",
               "count":1
            },
            {
               "title":"Prophecies Publishing",
               "value":"Prophecies+Publishing",
               "count":1
            },
            {
               "title":"Pt. Indo Semar Sakti",
               "value":"Pt.+Indo+Semar+Sakti",
               "count":1
            },
            {
               "title":"Pure Metal",
               "value":"Pure+Metal",
               "count":1
            },
            {
               "title":"Record Industry",
               "value":"Record+Industry",
               "count":1
            },
            {
               "title":"Rock Brigade Records",
               "value":"Rock+Brigade+Records",
               "count":1
            },
            {
               "title":"Rocris Disc",
               "value":"Rocris+Disc",
               "count":1
            },
            {
               "title":"Shinigami Records",
               "value":"Shinigami+Records",
               "count":1
            },
            {
               "title":"Sony Music Marketing Inc.",
               "value":"Sony+Music+Marketing+Inc.",
               "count":1
            },
            {
               "title":"Sound City Records",
               "value":"Sound+City+Records",
               "count":1
            },
            {
               "title":"Twister Com\u00e9rcio De Discos E Fitas Ltda.",
               "value":"Twister+Com%C3%A9rcio+De+Discos+E+Fitas+Ltda.",
               "count":1
            },
            {
               "title":"Unknown (Sp)",
               "value":"Unknown+%28Sp%29",
               "count":1
            },
            {
               "title":"Victor",
               "value":"Victor",
               "count":1
            },
            {
               "title":"Victor Entertainment, Inc.",
               "value":"Victor+Entertainment%2C+Inc.",
               "count":1
            },
            {
               "title":"Wagram Music S.A.",
               "value":"Wagram+Music+S.A.",
               "count":1
            },
            {
               "title":"Ward Records",
               "value":"Ward+Records",
               "count":1
            },
            {
               "title":"Ward Records, Inc.",
               "value":"Ward+Records%2C+Inc.",
               "count":1
            },
            {
               "title":"Witch Of The East",
               "value":"Witch+Of+The+East",
               "count":1
            },
            {
               "title":"Zph Log",
               "value":"Zph+Log",
               "count":1
            },
            {
               "title":"\u041f\u0434\u0432 Records",
               "value":"%D0%9F%D0%B4%D0%B2+Records",
               "count":1
            },
            {
               "title":"\u0420\u0430\u043e",
               "value":"%D0%A0%D0%B0%D0%BE",
               "count":1
            }
         ],
         "allows_multiple_values":true
      },
      {
         "title":"Country",
         "id":"country",
         "values":[
            {
               "title":"Germany",
               "value":"Germany",
               "count":24
            },
            {
               "title":"Russia",
               "value":"Russia",
               "count":8
            },
            {
               "title":"US",
               "value":"US",
               "count":4
            },
            {
               "title":"Brazil",
               "value":"Brazil",
               "count":3
            },
            {
               "title":"Europe",
               "value":"Europe",
               "count":3
            },
            {
               "title":"Belarus",
               "value":"Belarus",
               "count":2
            },
            {
               "title":"Bulgaria",
               "value":"Bulgaria",
               "count":2
            },
            {
               "title":"Colombia",
               "value":"Colombia",
               "count":2
            },
            {
               "title":"Japan",
               "value":"Japan",
               "count":2
            },
            {
               "title":"USA & Europe",
               "value":"USA+%26+Europe",
               "count":2
            },
            {
               "title":"Ukraine",
               "value":"Ukraine",
               "count":2
            },
            {
               "title":"Argentina",
               "value":"Argentina",
               "count":1
            },
            {
               "title":"Czech Republic",
               "value":"Czech+Republic",
               "count":1
            },
            {
               "title":"France",
               "value":"France",
               "count":1
            },
            {
               "title":"Indonesia",
               "value":"Indonesia",
               "count":1
            },
            {
               "title":"Malaysia",
               "value":"Malaysia",
               "count":1
            },
            {
               "title":"Mexico",
               "value":"Mexico",
               "count":1
            },
            {
               "title":"Poland",
               "value":"Poland",
               "count":1
            },
            {
               "title":"Romania",
               "value":"Romania",
               "count":1
            },
            {
               "title":"Turkey",
               "value":"Turkey",
               "count":1
            }
         ],
         "allows_multiple_values":true
      },
      {
         "title":"Released",
         "id":"released",
         "values":[
            {
               "title":"1997",
               "value":"1997",
               "count":21
            },
            {
               "title":"1998",
               "value":"1998",
               "count":3
            },
            {
               "title":"1999",
               "value":"1999",
               "count":1
            },
            {
               "title":"2000",
               "value":"2000",
               "count":1
            },
            {
               "title":"2001",
               "value":"2001",
               "count":2
            },
            {
               "title":"2002",
               "value":"2002",
               "count":1
            },
            {
               "title":"2003",
               "value":"2003",
               "count":3
            },
            {
               "title":"2005",
               "value":"2005",
               "count":4
            },
            {
               "title":"2006",
               "value":"2006",
               "count":1
            },
            {
               "title":"2010",
               "value":"2010",
               "count":3
            },
            {
               "title":"2011",
               "value":"2011",
               "count":2
            },
            {
               "title":"2013",
               "value":"2013",
               "count":4
            },
            {
               "title":"2017",
               "value":"2017",
               "count":4
            },
            {
               "title":"2018",
               "value":"2018",
               "count":1
            },
            {
               "title":"2019",
               "value":"2019",
               "count":1
            }
         ],
         "allows_multiple_values":true
      }
   ],
   "versions":[
      {
         "id":9636672,
         "label":"First Town Records",
         "country":"Russia",
         "title":"Glory To The Brave",
         "major_formats":[
            "CD"
         ],
         "format":"Album, Unofficial Release",
         "catno":"FTCD-5547",
         "released":"1997",
         "status":"Accepted",
         "resource_url":"https://api.discogs.com/releases/9636672",
         "thumb":"https://i.discogs.com/RNyxG6DsRjvkUHC4bBG1ArbrDEIKq67eWvkZvGjff9E/rs:fit/g:sm/q:40/h:150/w:150/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTk2MzY2/NzItMTQ4Mzk5Mzc3/OS0zODgxLmpwZWc.jpeg",
         "stats":{
            "community":{
               "in_wantlist":14,
               "in_collection":5
            },
            "user":{
               "in_wantlist":0,
               "in_collection":0
            }
         }
      },
      {
         "id":8747344,
         "label":"Wizard (4)",
         "country":"Bulgaria",
         "title":"Glory To The Brave",
         "major_formats":[
            "Cassette"
         ],
         "format":"Album",
         "catno":"WNB 200.157, WNB 200.158",
         "released":"1997",
         "status":"Accepted",
         "resource_url":"https://api.discogs.com/releases/8747344",
         "thumb":"https://i.discogs.com/IVlMUx14HEwYsblky5jphTeGD6RPSmp3xwq7qexqpu0/rs:fit/g:sm/q:40/h:150/w:150/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTg3NDcz/NDQtMTQ2ODMwNzA2/NC02NTg2LmpwZWc.jpeg",
         "stats":{
            "community":{
               "in_wantlist":17,
               "in_collection":1
            },
            "user":{
               "in_wantlist":0,
               "in_collection":0
            }
         }
      },
      {
         "id":6992693,
         "label":"Wizard (4)",
         "country":"Bulgaria",
         "title":"Glory To The Brave",
         "major_formats":[
            "Cassette"
         ],
         "format":"Album",
         "catno":"WNB 200.157, WNB 200.158",
         "released":"1997",
         "status":"Accepted",
         "resource_url":"https://api.discogs.com/releases/6992693",
         "thumb":"https://i.discogs.com/6i2_J1SRIxmCfTKD1InkOBngNOEfJE-0W6cpx36TccY/rs:fit/g:sm/q:40/h:150/w:150/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTY5OTI2/OTMtMTQ3ODYxNzY3/Ni00MTEyLmpwZWc.jpeg",
         "stats":{
            "community":{
               "in_wantlist":17,
               "in_collection":6
            },
            "user":{
               "in_wantlist":0,
               "in_collection":0
            }
         }
      },
      {
         "id":4649289,
         "label":"Rocris Disc",
         "country":"Romania",
         "title":"Glory To The Brave",
         "major_formats":[
            "Cassette"
         ],
         "format":"Album",
         "catno":"ROC 023-01",
         "released":"1997",
         "status":"Accepted",
         "resource_url":"https://api.discogs.com/releases/4649289",
         "thumb":"https://i.discogs.com/ExQH1x5pKcaUicbZ-flJMtkKzyHmDyR1z-O3vHnZyck/rs:fit/g:sm/q:40/h:150/w:150/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTQ2NDky/ODktMTM3MTA0NDg4/My0zMDg0LmpwZWc.jpeg",
         "stats":{
            "community":{
               "in_wantlist":14,
               "in_collection":7
            },
            "user":{
               "in_wantlist":0,
               "in_collection":0
            }
         }
      },
      {
         "id":12981949,
         "label":"Nuclear Blast",
         "country":"Germany",
         "title":"Glory To The Brave",
         "major_formats":[
            "CD"
         ],
         "format":"Album",
         "catno":"NB 265-2, 27361 62652",
         "released":"1997",
         "status":"Accepted",
         "resource_url":"https://api.discogs.com/releases/12981949",
         "thumb":"https://i.discogs.com/D92H1rPq8AKSBzLKoYjwglQP_nikj3uX_NDyi5ieHII/rs:fit/g:sm/q:40/h:150/w:150/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTEyOTgx/OTQ5LTE1NDU4NDgz/NzYtNDYzNS5qcGVn.jpeg",
         "stats":{
            "community":{
               "in_wantlist":29,
               "in_collection":280
            },
            "user":{
               "in_wantlist":0,
               "in_collection":0
            }
         }
   ]
}
*/
