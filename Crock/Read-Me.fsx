(*** hide ***)

#I "C:\Packages" 
#r @"FSharp.Data.1.1.10\lib\net40\FSharp.Data.dll"
#r "../libs/FSharpx.Collections.Experimental.dll"
#r "bin/Debug/Crock.dll"

open Crock.JsonSchema
open System.IO
open FSharp.Data
open FSharp.Data.Json

(**
#Crock
##(For Douglas Crockford, of course!)

A JSON schema handling system.

##A Single JSON Type Provider for all Types...

"Out of the box" the [JsonProvider](http://tpetricek.github.com/FSharp.Data/docs/JsonProvider.html) from the 
[FSharp.Data](http://tpetricek.github.com/FSharp.Data/index.html) library provides for strongly typing all JSON schemas 
known to the project at design time by loading them from a single file into one JSON type provider.
*)

type allJsonTypes = JsonProvider<"TwitterStreamSandBox.json", SampleList=true>

(**
Loading and parsing tweets with 2 different schemas, we see this works and the correctness of the generated types.

(Note the "altered" tweet has a "Text2" attribute.)
*)
let aTweet = (*[omit:("""""" {"in_reply_to_status_id_str":null,"text":...)]*)""" {"in_reply_to_status_id_str":null,"text":"\u5927\u91d1\u6255\u3063\u3066\u904a\u3070\u3057\u3066\u3082\u3089\u3046\u3002\u3082\u3046\u3053\u306e\u4e0a\u306a\u3044\u8d05\u6ca2\u3002\u3067\u3082\uff0c\u5b9f\u969b\u306b\u306f\u305d\u306e\u8d05\u6ca2\u306e\u672c\u8cea\u3092\u6e80\u55ab\u3067\u304d\u308b\u4eba\u306f\u9650\u3089\u308c\u3066\u308b\u3002\u305d\u3053\u306b\u76ee\u306b\u898b\u3048\u306a\u3044\u968e\u5c64\u304c\u3042\u308b\u3068\u304a\u3082\u3046\u3002","in_reply_to_user_id_str":null,"retweet_count":0,"geo":null,"source":"web","retweeted":false,"truncated":false,"id_str":"263290764686155776","entities":{"user_mentions":[],"hashtags":[],"urls":[]},"in_reply_to_user_id":null,"in_reply_to_status_id":null,"place":null,"coordinates":null,"in_reply_to_screen_name":null,"created_at":"Tue Oct 30 14:46:24 +0000 2012","user":{"notifications":null,"contributors_enabled":false,"time_zone":"Tokyo","profile_background_color":"FFFFFF","location":"Kodaira Tokyo Japan","profile_background_tile":false,"profile_image_url_https":"https:\/\/si0.twimg.com\/profile_images\/1172376796\/70768_100000537851636_3599485_q_normal.jpg","default_profile_image":false,"follow_request_sent":null,"profile_sidebar_fill_color":"17451B","description":"KS(Green62)\/WasedaUniv.(Schl Adv Sci\/Eng)\/SynBio\/ChronoBio\/iGEM2010-2012\/Travel\/Airplane\/ \u5bfa\u30fb\u5ead\u3081\u3050\u308a","favourites_count":17,"screen_name":"Merlin_wand","profile_sidebar_border_color":"000000","id_str":"94788486","verified":false,"lang":"ja","statuses_count":8641,"profile_use_background_image":true,"protected":false,"profile_image_url":"http:\/\/a0.twimg.com\/profile_images\/1172376796\/70768_100000537851636_3599485_q_normal.jpg","listed_count":31,"geo_enabled":true,"created_at":"Sat Dec 05 13:07:32 +0000 2009","profile_text_color":"000000","name":"Marin","profile_background_image_url":"http:\/\/a0.twimg.com\/profile_background_images\/612807391\/twitter_free1.br.jpg","friends_count":629,"url":null,"id":94788486,"is_translator":false,"default_profile":false,"following":null,"profile_background_image_url_https":"https:\/\/si0.twimg.com\/profile_background_images\/612807391\/twitter_free1.br.jpg","utc_offset":32400,"profile_link_color":"ADADAD","followers_count":426},"id":263290764686155776,"contributors":null,"favorited":false} """(*[/omit]*)
let anAlteredTweet = (*[omit:("""""" {"in_reply_to_status_id_str":null,"text2":...)]*)""" {"in_reply_to_status_id_str":null,"text2":"\u5927\u91d1\u6255\u3063\u3066\u904a\u3070\u3057\u3066\u3082\u3089\u3046\u3002\u3082\u3046\u3053\u306e\u4e0a\u306a\u3044\u8d05\u6ca2\u3002\u3067\u3082\uff0c\u5b9f\u969b\u306b\u306f\u305d\u306e\u8d05\u6ca2\u306e\u672c\u8cea\u3092\u6e80\u55ab\u3067\u304d\u308b\u4eba\u306f\u9650\u3089\u308c\u3066\u308b\u3002\u305d\u3053\u306b\u76ee\u306b\u898b\u3048\u306a\u3044\u968e\u5c64\u304c\u3042\u308b\u3068\u304a\u3082\u3046\u3002","in_reply_to_user_id_str":null,"retweet_count":0,"geo":null,"source":"web","retweeted":false,"truncated":false,"id_str":"263290764686155776","entities":{"user_mentions":[],"hashtags":[],"urls":[]},"in_reply_to_user_id":null,"in_reply_to_status_id":null,"place":null,"coordinates":null,"in_reply_to_screen_name":null,"created_at":"Tue Oct 30 14:46:24 +0000 2012","user":{"notifications":null,"contributors_enabled":false,"time_zone":"Tokyo","profile_background_color":"FFFFFF","location":"Kodaira Tokyo Japan","profile_background_tile":false,"profile_image_url_https":"https:\/\/si0.twimg.com\/profile_images\/1172376796\/70768_100000537851636_3599485_q_normal.jpg","default_profile_image":false,"follow_request_sent":null,"profile_sidebar_fill_color":"17451B","description":"KS(Green62)\/WasedaUniv.(Schl Adv Sci\/Eng)\/SynBio\/ChronoBio\/iGEM2010-2012\/Travel\/Airplane\/ \u5bfa\u30fb\u5ead\u3081\u3050\u308a","favourites_count":17,"screen_name":"Merlin_wand","profile_sidebar_border_color":"000000","id_str":"94788486","verified":false,"lang":"ja","statuses_count":8641,"profile_use_background_image":true,"protected":false,"profile_image_url":"http:\/\/a0.twimg.com\/profile_images\/1172376796\/70768_100000537851636_3599485_q_normal.jpg","listed_count":31,"geo_enabled":true,"created_at":"Sat Dec 05 13:07:32 +0000 2009","profile_text_color":"000000","name":"Marin","profile_background_image_url":"http:\/\/a0.twimg.com\/profile_background_images\/612807391\/twitter_free1.br.jpg","friends_count":629,"url":null,"id":94788486,"is_translator":false,"default_profile":false,"following":null,"profile_background_image_url_https":"https:\/\/si0.twimg.com\/profile_background_images\/612807391\/twitter_free1.br.jpg","utc_offset":32400,"profile_link_color":"ADADAD","followers_count":426},"id":263290764686155776,"contributors":null,"favorited":false} """(*[/omit]*)

let tweet = allJsonTypes.Parse(aTweet)
let altTweet = allJsonTypes.Parse(anAlteredTweet)

printfn "actual tweet: %s (retweeted %d times)\n:%s"
  tweet.User.Value.Name tweet.RetweetCount.Value tweet.Text.Value

printfn "altered schema tweet: %s (retweeted %d times)\n:%s"
  altTweet.User.Value.Name altTweet.RetweetCount.Value altTweet.Text2.Value

(**
We could go so far as to process World Bank data along with tweets employing a Type Provider instantiated from a single file of JSON messages.

In fact it seems we could load any combination of message schemas into the Type Provider, provided they are all wrapped as a JSON object. (It remains to be seen
whether there are other practical limitations processing a truly mixed message schema stream.)
*)


let worldBankData = (*[omit:("""""" {"indicator": { "id":)]*)""" {"indicator": { "id": "GC.DOD.TOTL.GD.ZS", "value": "Central government debt, total (% of GDP)" }, "country": { "id": "CZ", "value": "Czech Republic" }, "value": null, "decimal": "1", "date": "2012" } """(*[/omit]*)
let fromWorldBank = allJsonTypes.Parse(worldBankData)
printfn "%i " fromWorldBank.Date.Value

(**
##...but still leaves us unable to actually process multiple schemas

The first problem is we don't actually know the schema of the message we just parsed, making it difficult to write code to process parsed messages.
*)

try
    printfn "%s (retweeted %d times)\n:%s"
        altTweet.User.Value.Name altTweet.RetweetCount.Value altTweet.Text.Value
with
    | :? System.Collections.Generic.KeyNotFoundException as e -> printfn  "%s" e.Message 
    | e -> printfn  "Other error: %s" e.Message 

(**
##The Solution

To the rescue is Crock, a system for intelligently processing JSON message schemas. It is based on a representation of JSON schema nodes that allows for
comparison, updating, and future expansion with metadata:
*)

type JsonNode = { Key : string option; ValueTypeName : string; IsNullable : bool option }

(**
In this prototype JsonNodes are organized in an FSharpx.Collections.Experimental.EagerRoseTree. As input the JsonSchema takes a parsed message from
the FSharp.Data [Json Parser](http://tpetricek.github.com/FSharp.Data/docs/JsonValue.html).
*)

let x = Crock.JsonSchema.create (JsonValue.Parse(anAlteredTweet))

(**
The "pretty print" of the alternate tweet JsonSchema:
*)

Crock.JsonSchema.printPreOrder x

(**<pre>
Object isNullable: False
	contributors Null isNullable: True
	coordinates Null isNullable: True
	created_at String isNullable: unknown
	entities Object isNullable: False
		hashtags Array isNullable: False
		urls Array isNullable: False
		user_mentions Array isNullable: False
	favorited Boolean isNullable: False
	geo Null isNullable: True
	id Number isNullable: unknown
	id_str String isNullable: unknown
	in_reply_to_screen_name Null isNullable: True
	in_reply_to_status_id Null isNullable: True
	in_reply_to_status_id_str Null isNullable: True
	in_reply_to_user_id Null isNullable: True
	in_reply_to_user_id_str Null isNullable: True
	place Null isNullable: True
	retweet_count Number isNullable: unknown
	retweeted Boolean isNullable: False
	source String isNullable: unknown
	text2 String isNullable: unknown
	truncated Boolean isNullable: False
	user Object isNullable: False
		contributors_enabled Boolean isNullable: False
		created_at String isNullable: unknown
		default_profile Boolean isNullable: False
		default_profile_image Boolean isNullable: False
		description String isNullable: unknown
		favourites_count Number isNullable: unknown
		follow_request_sent Null isNullable: True
		followers_count Number isNullable: unknown
		following Null isNullable: True
		friends_count Number isNullable: unknown
		geo_enabled Boolean isNullable: True
		id Number isNullable: unknown
		id_str String isNullable: unknown
		is_translator Boolean isNullable: False
		lang String isNullable: unknown
		listed_count Number isNullable: unknown
		location String isNullable: unknown
		name String isNullable: unknown
		notifications Null isNullable: True
		profile_background_color String isNullable: unknown
		profile_background_image_url String isNullable: unknown
		profile_background_image_url_https String isNullable: unknown
		profile_background_tile Boolean isNullable: False
		profile_image_url String isNullable: unknown
		profile_image_url_https String isNullable: unknown
		profile_link_color String isNullable: unknown
		profile_sidebar_border_color String isNullable: unknown
		profile_sidebar_fill_color String isNullable: unknown
		profile_text_color String isNullable: unknown
		profile_use_background_image Boolean isNullable: True
		protected Boolean isNullable: False
		screen_name String isNullable: unknown
		statuses_count Number isNullable: unknown
		time_zone String isNullable: unknown
		url Null isNullable: True
		utc_offset Number isNullable: unknown
		verified Boolean isNullable: False
</pre>*)

(**
##Wrap-up

There's not much left to do, provided I'm going in the right direction. So I want to get some feedback at this point. Here's how I see the next steps:

1) Finish-up JsonSchema comparison and report on non-mathcing schemas using the "pretty print" format to show differences. (Including adapting
 the JsonSchema to have a custom GetHashCode().) 

2) Persist JsonSchema.

3) Handle a node which was previously "Null" discovering its actual type when a message comes through that is not null.

4) Generate JSON message from the persisted JsonSchema to load the Type Provider.

5) Not directly related to this project, but might be useful: the JsonNode structure can easily expand to accomodate metadata. For instance
the JSON type "string" may actually contain a date.

*)

(*** hide ***)

#load "FSharp.Formatting.1.0.15/literate/literate.fsx"
open FSharp.Literate
open System.IO

let source = __SOURCE_DIRECTORY__
let template = Path.Combine(source, "templates/template-file.html")

let script = Path.Combine(source, "Read-Me.fsx")
Literate.ProcessScriptFile(script, template)