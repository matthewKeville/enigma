The Seattle Times pays a company called pzzl
to provide older NY Times Crosswords on there
website.

https://www.seattletimes.com/games-nytimes-crossword/

analyzing the network calls we see that the website uses an undocumented
API https://nytsyn.pzzl.com/nytsyn-crossword-mh/nytsyncrossword?date=240625
we can manipulate this api to get access to all of puzzl NY Times archives
by varying the date.

Experimentally we can get puzzles dating back to 2008 : 080625

The payload looks like this

```txt
ARCHIVE

240521

NY Times, Tue, Jun 25, 2024

Zachary David Levy / Joel Fagliano

15

15

35

43

ACED#PLIE#EASES
RULE#RIND#SAINT
CRIBNOTES#QANDA
###TOM#PEW##FUN
UGH#BOTTLEGOURD
GUESSSO##LADLES
GAPE##PASSTO###
#CARRIAGEHOUSE#
###GENZER##LETS
OHNEAT##VERSACE
MOBILEPHONE#THX
ETA##RAE#ZAG###
RAJAS#BABYPROOF
TIARA#STEM#IDLE
ARMED#THEE#NEED

Hit a serve past
Ballet dancer's bend
Lightens (up)
Word after golden or slide
Often-discarded part of a fruit
Canonized person
Cheat sheets
Post-panel sesh
Male cat
Big name in public opinion research
It might be poked
Cry of disgust that sounds like 24-Down
Fruit also known as calabash
"Yeah, I suppose"
Soup kitchen utensils
Stare open-mouthed
Target, as a wide receiver
Outbuilding for many a historic home
Millennial's successor, informally
Tennis do-overs
"That's pretty nifty!"
Fashion house whose logo features Medusa
Counterpart to a landline
Appreciative text
Schedule abbr.
Middle name for Alec Baldwin and Carly Jepsen
Go the other way
Indian royals
Make safer, in a way ... or what the starts of 17-, 27-, 38- and 52-Across might be?
Pageant topper
Often-discarded part of a fruit
Inactive
One-___ bandit
Biblical pronoun
Nonnegotiable thing

Airplane's path on a flight map, often
Junkyard dog
Roth of "Inglourious Basterds"
Red scare?
Sneak previews
Happening, in modern parlance
Bumbling
Old car make named for Henry Ford's son
Abbr. on a lawyer's business card
Highly rated, as a bond
Iniquitous
Stick it out
Array at a farmer's market
Noggins
Language in which "w" can be a vowel
Australian boot brand
Green dip, familiarly
Purifying filter acronym
November birthstone
One purring in Peru
Nonalcoholic beer brand
Composer Rachmaninoff
Ripen
Kind of motor used in robotics
Down-to-earth
Lead-in to mingle or mezzo
Ticket assignment
Cut quite a figure?
Something a prenatal ultrasound can determine
Mafia code of silence
Windbag's output
Classic video game with the catchphrase "He's on fire!"
Biological catalyst
Collect what's been sown
"Blue Ribbon" brewer
Toffee bar brand since 1928
Beam
"What ___ the odds?"
Down in the dumps
Wax producer
Shelley's "To a Skylark," for one
World Cup chant
Put quarters in, as a meter
```

---

It seems that the rules of the NYT puzzle require all non-blank spaces
that have TOP or LEFT border need to correspond to a question.

The question ordinals are laid out row by row. Where each non blank space 
not bordered by the above, get's assigned an ordinal clue.

Thus, given the geometry of the puzzle answer, we determine the (x,y) coordinate
of a given ordinal.

---

# Reproducing the puzzle

First, determine the ordinal positions in the puzzle based on the supplied
solution geometry.

Then step through the ordinals, when a ordinal has a corresponding border
type, we pop from the Clue list that clue and and assign to that ordinal.
We do this with both clue sets.

---

# Parsing the data

The data comes in a plain text format. To get the
data in the model it's necessary to translate this
into string literals (as ' are in clues). 
This is easily achieved with verbatim strings in C#

## Caveat

for verbatim strings quote sequences "" are interpreted, not literal
clues with "" will not work with verbatim strings

we can circumvent this by escaping the " with another quote "
so ""

---

```csharp
@"ARCHIVE"

@"240521"

@"NY Times, Tue, Jun 25, 2024"

@"Zachary David Levy / Joel Fagliano"

@"15"

@"15"

@"35"

@"43"

@"ACED#PLIE#EASES"
@"RULE#RIND#SAINT"
@"CRIBNOTES#QANDA"
@"###TOM#PEW##FUN"
@"UGH#BOTTLEGOURD"
@"GUESSSO##LADLES"
@"GAPE##PASSTO###"
@"#CARRIAGEHOUSE#"
@"###GENZER##LETS"
@"OHNEAT##VERSACE"
@"MOBILEPHONE#THX"
@"ETA##RAE#ZAG###"
@"RAJAS#BABYPROOF"
@"TIARA#STEM#IDLE"
@"ARMED#THEE#NEED"

@"Hit a serve past"
@"Ballet dancer's bend"
@"Lightens (up)"
@"Word after golden or slide"
@"Often-discarded part of a fruit"
@"Canonized person"
@"Cheat sheets"
@"Post-panel sesh"
@"Male cat"
@"Big name in public opinion research"
@"It might be poked"
@"Cry of disgust that sounds like 24-Down"
@"Fruit also known as calabash"
@""Yeah, I suppose""
@"Soup kitchen utensils"
@"Stare open-mouthed"
@"Target, as a wide receiver"
@"Outbuilding for many a historic home"
@"Millennial's successor, informally"
@"Tennis do-overs"
@""That's pretty nifty!""
@"Fashion house whose logo features Medusa"
@"Counterpart to a landline"
@"Appreciative text"
@"Schedule abbr."
@"Middle name for Alec Baldwin and Carly Jepsen"
@"Go the other way"
@"Indian royals"
@"Make safer, in a way ... or what the starts of 17-, 27-, 38- and 52-Across might be?"
@"Pageant topper"
@"Often-discarded part of a fruit"
@"Inactive"
@"One-___ bandit"
@"Biblical pronoun"
@"Nonnegotiable thing"

@"Airplane's path on a flight map, often"
@"Junkyard dog"
@"Roth of "Inglourious Basterds""
@"Red scare?"
@"Sneak previews"
@"Happening, in modern parlance"
@"Bumbling"
@"Old car make named for Henry Ford's son"
@"Abbr. on a lawyer's business card"
@"Highly rated, as a bond"
@"Iniquitous"
@"Stick it out"
@"Array at a farmer's market"
@"Noggins"
@"Language in which "w" can be a vowel"
@"Australian boot brand"
@"Green dip, familiarly"
@"Purifying filter acronym"
@"November birthstone"
@"One purring in Peru"
@"Nonalcoholic beer brand"
@"Composer Rachmaninoff"
@"Ripen"
@"Kind of motor used in robotics"
@"Down-to-earth"
@"Lead-in to mingle or mezzo"
@"Ticket assignment"
@"Cut quite a figure?"
@"Something a prenatal ultrasound can determine"
@"Mafia code of silence"
@"Windbag's output"
@"Classic video game with the catchphrase "He's on fire!""
@"Biological catalyst"
@"Collect what's been sown"
@""Blue Ribbon" brewer"
@"Toffee bar brand since 1928"
@"Beam"
@""What ___ the odds?""
@"Down in the dumps"
@"Wax producer"
@"Shelley's "To a Skylark," for one"
@"World Cup chant"
@"Put quarters in, as a meter"
```



