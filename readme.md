#Crock
##(For Douglas Crockford, of course!)

A JSON schema handling system.

##A Single JSON Type Provider for all Types...

"Out of the box" the [JsonProvider](http://tpetricek.github.com/FSharp.Data/docs/JsonProvider.html) from the [FSharp.Data](http://tpetricek.github.com/FSharp.Data/index.html) library provides for strongly typing all JSON schemas known to the project at design time by loading them from a single file into one JSON type provider.

type allJsonTypes = JsonProvider<"TwitterStreamSandBox.json", SampleList=true>

Loading and parsing tweets with 2 different schemas, we see this works and the correctness of the generated types.

(Note the "altered" tweet has a "Text2" attribute.)

    let aTweet = """ {"in_reply_to_status_id_str":null,"text":"\u5927\u91d1...

    let tweet = allJsonTypes.Parse(aTweet)
    let altTweet = allJsonTypes.Parse(anAlteredTweet)

    printfn "actual tweet: %s (retweeted %d times)\n:%s"
      tweet.User.Value.Name tweet.RetweetCount.Value tweet.Text.Value

    printfn "altered schema tweet: %s (retweeted %d times)\n:%s"
      altTweet.User.Value.Name altTweet.RetweetCount.Value altTweet.Text2.Value

We could go so far as to process World Bank data along with tweets employing a Type Provider instantiated from a single file of JSON messages.

In fact it seems we could load any combination of message schemas into the Type Provider, provided they are all wrapped as a JSON object. (It remains to be seen whether there are other practical limitations processing a truly mixed message schema stream.)


    let worldBankData = (*[omit:("""""" {"indicator": { "id":)]*)""" {"indicator": { "id": "GC.DOD.TOTL.GD.ZS", "value": "Central government debt, total (% of GDP)" }, "country": { "id": "CZ", "value": "Czech Republic" }, "value": null, "decimal": "1", "date": "2012" } """(*[/omit]*)
    let fromWorldBank = allJsonTypes.Parse(worldBankData)
    printfn "%i " fromWorldBank.Date.Value

##...but still leaves us unable to actually process multiple schemas

The first problem is we don't actually know the schema of the message we just parsed, making it difficult to write code to process parsed messages.

    try
        printfn "%s (retweeted %d times)\n:%s"
            altTweet.User.Value.Name altTweet.RetweetCount.Value altTweet.Text.Value
    with
        | :? System.Collections.Generic.KeyNotFoundException as e -> printfn  "%s" e.Message 
        | e -> printfn  "Other error: %s" e.Message 

##The Solution

To the rescue is Crock, a system for intelligently processing JSON message schemas. It is based on a representation of JSON schema nodes that allows for comparison, updating, and future expansion with metadata:

    type JsonNode = { Key : string option; ValueTypeName : string; IsNullable : bool option }

In this prototype JsonNodes are organized in an FSharpx.Collections.Experimental.EagerRoseTree. As input the JsonSchema takes a parsed message from the FSharp.Data [Json Parser](http://tpetricek.github.com/FSharp.Data/docs/JsonValue.html).

    let x = Crock.JsonSchema.create (JsonValue.Parse(anAlteredTweet))

The "pretty print" of the alternate tweet JsonSchema:

    Crock.JsonSchema.printPreOrder x

<pre>
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
</pre>

##Wrap-up

There's not much left to do, provided I'm going in the right direction. So I want to get some feedback at this point. Here's how I see the next steps:

1) Finish-up JsonSchema comparison and report on non-mathcing schemas using the "pretty print" format to show differences. (Including adapting the JsonSchema to have a custom GetHashCode().) 

2) Persist JsonSchema.

3) Handle a node which was previously "Null" discovering its actual type when a message comes through that is not null.

4) Generate JSON message from the persisted JsonSchema to load the Type Provider.

5) Not directly related to this project, but might be useful: the JsonNode structure can easily expand to accomodate metadata. For instance the JSON type "string" may actually contain a date.