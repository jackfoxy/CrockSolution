namespace Crock

open FSharpx.Collections.Experimental
open FSharpx.Collections
open FSharp.Data.Json

type JsonNode = { Key : string option; ValueTypeName : string; IsNullable : bool option }
    with
    override x.ToString() = 
        let k =
            match x.Key with
            | None -> ""
            | Some k' -> k'
        let iN =
            match x.IsNullable with
            | None -> "unknown"
            | Some b -> (b.ToString())

        if (System.String.IsNullOrEmpty k)
        then sprintf "%s isNullable: %s" x.ValueTypeName iN
        else sprintf "%s %s isNullable: %s" k x.ValueTypeName iN

module JsonSchema =

    let create (value : JsonValue) = 
        let rec loop = function
            | key, JsonValue.Null -> EagerRoseTree.singleton { Key = key; ValueTypeName = "Null"; IsNullable = Some(true) }
            | key, JsonValue.Boolean b -> EagerRoseTree.singleton { Key = key; ValueTypeName = "Boolean"; IsNullable = Some(b) }
            | key, JsonValue.Number number -> EagerRoseTree.singleton { Key = key; ValueTypeName = "Number"; IsNullable = None }
            | key, JsonValue.Float number -> EagerRoseTree.singleton { Key = key; ValueTypeName = "Float"; IsNullable = None }
            | key, JsonValue.String t -> EagerRoseTree.singleton { Key = key; ValueTypeName = "String"; IsNullable = None }
            | key, JsonValue.Object properties -> 
                EagerRoseTree.create { Key = key; ValueTypeName = "Object"; IsNullable = Some(false) } 
                    (Map.fold (fun s k t -> s.Conj (loop (Some(k), t))) Vector.empty properties)
            | key, JsonValue.Array elements -> 
                EagerRoseTree.create { Key = key; ValueTypeName = "Array"; IsNullable = Some(false) } 
                    (Array.fold (fun s t -> s.Conj (loop (None, t))) Vector.empty elements )
  
        loop (None, value)

    let printPreOrder (jsonSchema: JsonNode EagerRoseTree) =
        let rec loop (x : JsonNode EagerRoseTree) p =
            printfn "%s%s" p (x.Root.ToString())
            Seq.iter (fun ( t : JsonNode EagerRoseTree) -> 
                        printfn "%s%s" (p + "\t") (t.Root.ToString())
                        for item in t.Children do
                         loop item (p + "\t\t")
                         ) x.Children

//            let rec loop2 i =
//                match Vector.tryNth i x.Children with
//                | Some hd -> 
//                    loop hd (p + "\t")
//                    loop2 (i + 1)
//                | None -> ()
            //loop2 0
            ()
        loop jsonSchema ""