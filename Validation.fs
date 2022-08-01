module Validation

open System.Text.RegularExpressions

let doesMatchRegex regex city =
    if Regex.IsMatch (city, regex) then Ok city
    else Error "City name contains invalid characters! Try again."

let isInList list city =
    if Seq.contains city list then Ok city
    else Error "This city doesn\'t exist (in our database at least). Try again."

let isNotInList list city =
    if not (Seq.contains city list) || Seq.isEmpty list then Ok city
    else Error "This city has already been used! Try again."

let doesStartWith (letter: char) (city: string) =
    if city.StartsWith letter then Ok city
    else Error "Your city starts with the wrong letter! Try again."