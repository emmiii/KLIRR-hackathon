# KLIRR - Krislogg

En skrivbordsapplikation för dokumentering under kris när internet inte finns tillgängligt. 
Användare använder applikationen lokalt på sin egen dator och dokumenterar viktig information, utfall etc. Loggar får automatiskt en tidsstämpel samt datornamnet när de sparas.
Informationen kan delas via USB, exporteras (kopiera till vald plats -> USB-minnet) och sedan importeras (hämta från vald plats -> USB-minnet) till en annan Krislogg. Det går även att sätta en fil i en delad nätverkskatalog som databas och låta flera enheter använda samma. 

## Funktionalitet

- **Ny logg** - Skapa loggar med nödvändig information, bland annat *Titel*, *Avdelning* och *Utfall*.
- **Import/Export** - Importera och exportera den lokala datan för att enkelt kunna dela information med andra.
- **Inställningar** - Välj var din data ska sparas, antingen lokalt, på portabel hårddisk eller på gemensam nätverksyta för snabbare delning. Härifrån kan du även hitta errorloggen om du behöver felsöka applikationen. 

## Beslut

Hela applikationen utgår ifrån en vy där samtliga loggar i databasen är synliga. Tanken är att det viktigaste för användaren är att få en snabb överblick och det som är relevant att se är de senaste loggarna, vilket ligger överst. De fälten som är synliga (Tid, Titel, Avdelning, Utfall, Taggar) är sökbara för att snabbt kunna hitta den relevanta informationen. 

Applikationen använder en ljusblå, vit och ljusgrå med rundade kanter och mjukt typsnitt för att ge ett lugnade intryck, vilket kan vara en fördel i kris eller likande stressade situationer. 

*Titel*, *Avdelning* och *Beskrivning* är obligatoriska fält för en logg. 
*Tid*, *Titel*, *Avdelning*, *Utfall* och *Taggar* är sökbara och gör att användaren snabbt kan se t.ex. vad som hänt för en specifik avdelning eller vad som hände under en given dag.  
*Exportera* kopierar över den aktuella databasen till den angivna sökvägen i dialogruten. 
*Importera* låter användaren välja en fil och lägger till loggar från den valda filen som inte redan finns till den befintliga databasen.

Vi har valt att inte ha någon form av verifierad signering på loggarna. Det finns ingen autentisering inom denna applikation och därmed heller inget säkert sätt att ge loggen en korrekt avsändare. Att ge användare möjlighet att signera sina loggar kan skapa problem med delvis sökande om personen skrivit fel namn/smeknamn/förnamn/efternamn. Det kan också utge en säkerhetsrisk då vem som helst kan skriva in ett namn och desinformation kan spridas med högre trovärdighet om det ser ut att komma från "rätt" person. Istället noteras  datornamn för att ge någon form av spårbarhet för loggarna.

Loggar har inga ID eller annan unik identifierare. Detta är inte möjligt då applikationen ska kunna användas helt lokalt och sedan kunna dela informationen (loggarna) med andra.  

Loggen är ej redigerbar i användargränssnittet. Detta är ett medvetet val då loggar inte bör redigeras utan de ska ge en korrekt bild av den aktuella situationen. Inte heller radering av loggar är möjlig för användarna. Databasen som används är dock endast en .json fil och kan redigeras manuellt. 

Databasen som används är en .json fil som sparas lokalt på användarens dator. Default filsökväg är C:\krislogg.json men detta kan ändras under *Inställningar* i applikationen. Har man ett lokalt nätverk uppe är det möjlig att lägga sin .json fil i en delad mapp och dela den mellan flera enheter. Filsökvägen för databasen som används sparas i %LocalAppData%\KLIRR\settings.json. (https://support.microsoft.com/sv-se/windows/fildelning-via-ett-n%C3%A4tverk-i-windows-b58704b2-f53a-4b82-7bc1-80f9994725bf)

Filsökväg för errorloggen, för eventuell felsökning hittas via %LocalAppData%\KLIRR\app.log

## Systemkrav

- Windows 10/11
- .NET 8.0 Runtime
