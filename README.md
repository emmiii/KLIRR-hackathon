# KLIRR - Krislogg

En skrivbordsapplikation för dokumentering under kris när internet inte finns tillgängligt. 
Användare använder applikationen lokalt på sin egen dator och dokumenterar viktig information, beslut etc. Loggar får automatiskt en tidsstämpel när de sparas.
Informationen kan delas via USB, exporteras (kopiera till vald plats -> USB-minnet) och sedan importeras (hämta från vald plats -> USB-minnet) till en annan Krislogg. Det går även att sätta en fil i en delad nätverkskatalog och låta flera enheter använda samma databas. 

## Funktionalitet

- **Logga** - Skapa loggar med nödvändig information, bland annat *Titel*, *Avdelning* och *Beslut* 
- **Import/Export** - Importera och exportera den lokala datan för att enkelt kunna dela information med andra.
- **Inställningar** - Välj var din data ska sparas. Antingen lokalt eller på gemensam nätverksyta för snabbare delning.

## Designbeslut

Hela applikationen utgår ifrån Logg-vyn. Tanken är att det viktigaste för användaren är att få en snabb överblick och det som är relevant att se är de senaste loggarna, vilket ligger överst som default. De fälten som är synliga (Tid, Titel, Avdelning, Beslut, Taggar) är sökbara för att snabbt kunna hitta den relevanta informationen. 

Vi har valt att inte ha någon form av signering på loggarna. Det finns ingen autentisering inom denna applikation och därmed heller inget säkert sätt att ge loggen en korrekt avsändare. Att ge användare möjlighet att signera sina loggar kan skapa problem med delvis sökande om personen skrivit fel namn/smeknamn/förnamn/efternamn. Det kan också utge en säkerhetsrisk då vem som helst kan skriva in ett namn och desinformation kan spridas med högre trovärdighet om det ser ut att komma från "rätt" person. 

Loggar har inga ID eller annan unik identifierare. Detta är inte möjligt då applikationen ska kunna användas helt lokalt och sedan kunna dela informationen (loggarna) med andra.  

Då vi har varken signering eller ID så är spårbarheten för loggarna mycket låg.

Loggen är ej redigerbar i användargränssnittet. Detta är ett medvetet val då loggar inte bör redigeras utan de ska ge en korrekt bild av den aktuella situationen. Inte heller radering av loggar är möjlig för användarna. Databasen som används är dock endast en .json fil och kan redigeras manuellt. 

Databasen som används är en .json fil som sparas lokalt på användarens dator. Default filsökväg är C:\krislogg.json men detta kan ändras under *Inställningar* i applikationen. Har man ett lokalt nätverk uppe är det möjlig att lägga sin .json fil i en delad mapp och dela den mellan flera enheter. (https://support.microsoft.com/sv-se/windows/fildelning-via-ett-n%C3%A4tverk-i-windows-b58704b2-f53a-4b82-7bc1-80f9994725bf)


## Systemkrav

- Windows 10/11
- .NET 8.0 Runtime

### Default inställningar

- Filsökväg för din data (om inget annat anges): `C:\krislogg.json`
- Filsökväg sparas i: `%LocalAppData%\KLIRR\settings.json`

## Usage

### Creating a Log Entry

1. Click **Logga** to open the log entry form
2. Fill in the required fields:
   - **Titel** - Brief title of the incident
   - **Avdelning** - Department involved
   - **Beskrivning** - Detailed description
   - **Beslut** - Decision or action taken
3. Click **Spara** to save

### Viewing Entries

- Double-click any entry in the list to view its details

### Import/Export

- **Exportera** - Save the current log to a JSON file
- **Importera** - Load entries from an external JSON file (duplicates are automatically skipped)

### Settings

Click **Inställningar** to change the log file storage location.

