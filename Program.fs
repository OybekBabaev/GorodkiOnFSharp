open System
open System.IO
open Validation

// uploading cities from text files located in the "citLists" folder
let cities = ResizeArray()
for letter in ['A' .. 'Z'] do
    let filePath = $"citLists\\cit{ letter }.txt"
    let citiesFromFile = File.ReadAllLines filePath
    cities.AddRange citiesFromFile

let trash = ResizeArray()
let disposeCity city = 
    trash.Add city
    cities.Remove city |> ignore


// single composed function for validating player input
let validateCity city =
    city
    |> doesMatchRegex @"^(\p{L}|[-]|\s)+$"
    |> Result.bind (isNotInList trash)
    |> Result.bind (isInList cities)
    |> Result.bind (doesStartWith (trash |> Seq.last |> Seq.last))


// game starters and helpers 
let cpuTurn (firstLetter: string option) =
    let gg = Random()
    match firstLetter with
    | Some letter ->
        let requiredCities = cities |> Seq.filter (fun c -> c.StartsWith letter) |> Seq.toList
        let cpuCity = requiredCities.Item (gg.Next requiredCities.Length - 1)
        printfn "%s" cpuCity
        disposeCity cpuCity
    | None ->
        let cpuCity = cities.Item (gg.Next cities.Count)
        printfn "%s" cpuCity
        disposeCity cpuCity

let printfnWarning message =
    Console.ForegroundColor <- ConsoleColor.Red
    printfn "%s" message
    Console.ForegroundColor <- ConsoleColor.Gray


// starting point
printfn "Hi there! What\'s your name?"
Console.ReadLine () |> printfn "\nWelcome, %s! Let\'s play Gorodki! I\'ll start:"

cpuTurn None
while true do
    printf "Enter your city: "
    let userCity = Console.ReadLine ()
    let userCity = userCity.Trim().ToUpper()
    let validationResult = validateCity userCity
    match validationResult with
    | Error e -> printfnWarning e
    | Ok city ->
        printfn "%s" city
        if cities.Count > 1 then
            System.Threading.Thread.Sleep 600
            printfn "-"
            cpuTurn <| Some (userCity[userCity.Length - 1].ToString())
        else
            printfn "So, no more cities left, which means the game is over. Bye!"
            System.Threading.Thread.Sleep 2000
            Environment.Exit 0