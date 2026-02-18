using System;
using System.Globalization;
using System.IO;

static float AskFloat(string label, float defaultValue)
{
    Console.Write($"{label} (défaut: {defaultValue}) : ");
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input)) return defaultValue;
    if (float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out var v)) return v;
    Console.WriteLine("Valeur invalide, défaut utilisé.");
    return defaultValue;
}

static int AskInt(string label, int defaultValue)
{
    Console.Write($"{label} (défaut: {defaultValue}) : ");
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input)) return defaultValue;
    if (int.TryParse(input, out var v)) return v;
    Console.WriteLine("Valeur invalide, défaut utilisé.");
    return defaultValue;
}

static string BuildXml(
    float sightRadius, float loseSightRadius, float fovDeg, float hearingRange,
    int searchSeconds,
    float closeMax, float farMin, float lowHp, float highHp,
    (int atk, int def, int ret) nearHigh,
    (int atk, int def, int ret) nearLow,
    (int atk, int def, int ret) farHigh,
    (int atk, int def, int ret) farLow
)
{
    string F(float x) => x.ToString(CultureInfo.InvariantCulture);

    return $@"<AIConfig>
  <Perception>
    <SightRadius>{F(sightRadius)}</SightRadius>
    <LoseSightRadius>{F(loseSightRadius)}</LoseSightRadius>
    <PeripheralVisionDegrees>{F(fovDeg)}</PeripheralVisionDegrees>
    <HearingRange>{F(hearingRange)}</HearingRange>
  </Perception>

  <Search>
    <SearchDurationSeconds>{searchSeconds}</SearchDurationSeconds>
  </Search>

  <Fuzzy>
    <CloseMax>{F(closeMax)}</CloseMax>
    <FarMin>{F(farMin)}</FarMin>
    <LowHP>{F(lowHp)}</LowHP>
    <HighHP>{F(highHp)}</HighHP>
  </Fuzzy>

  <MiniMax>
    <State distance=""Near"" hp=""High"" attack=""{nearHigh.atk}"" defend=""{nearHigh.def}"" retreat=""{nearHigh.ret}"" />
    <State distance=""Near"" hp=""Low""  attack=""{nearLow.atk}"" defend=""{nearLow.def}"" retreat=""{nearLow.ret}"" />
    <State distance=""Far""  hp=""High"" attack=""{farHigh.atk}"" defend=""{farHigh.def}"" retreat=""{farHigh.ret}"" />
    <State distance=""Far""  hp=""Low""  attack=""{farLow.atk}"" defend=""{farLow.def}"" retreat=""{farLow.ret}"" />
  </MiniMax>
</AIConfig>";
}

Console.WriteLine("=== AIConfig.xml Generator (TP Final UE) ===");
Console.Write("Chemin de sortie complet (ex: .../VotreProjetUE/Content/AI/Data/AIConfig.xml) : ");
var outPath = Console.ReadLine();
if (string.IsNullOrWhiteSpace(outPath))
{
    Console.WriteLine("Chemin requis. Fin.");
    return;
}

// Perception
float sightRadius = AskFloat("SightRadius", 1500);
float loseSightRadius = AskFloat("LoseSightRadius", 1800);
float fovDeg = AskFloat("PeripheralVisionDegrees", 70);
float hearingRange = AskFloat("HearingRange", 1200);

// Search
int searchSeconds = AskInt("SearchDurationSeconds", 4);

// Fuzzy
float closeMax = AskFloat("CloseMax", 300);
float farMin = AskFloat("FarMin", 1200);
float lowHp = AskFloat("LowHP", 20);
float highHp = AskFloat("HighHP", 70);

// MiniMax
Console.WriteLine("--- MiniMax scores (Attack/Defend/Retreat) ---");

(int, int, int) AskScores(string label, int a, int d, int r)
{
    Console.WriteLine(label);
    int atk = AskInt("  Attack", a);
    int def = AskInt("  Defend", d);
    int ret = AskInt("  Retreat", r);
    return (atk, def, ret);
}

var nearHigh = AskScores("State Near/High", 10, 5, -5);
var nearLow = AskScores("State Near/Low", -10, 5, 8);
var farHigh = AskScores("State Far/High", 3, 6, 0);
var farLow = AskScores("State Far/Low", -8, 4, 10);

var xml = BuildXml(
    sightRadius, loseSightRadius, fovDeg, hearingRange,
    searchSeconds,
    closeMax, farMin, lowHp, highHp,
    nearHigh, nearLow, farHigh, farLow
);

Directory.CreateDirectory(Path.GetDirectoryName(outPath)!);
File.WriteAllText(outPath, xml);

Console.WriteLine("✅ AIConfig.xml généré : " + outPath);
Console.WriteLine("Astuce : côté Unreal, vous pouvez ajouter une touche (ex: R) pour recharger le XML sans relancer.");
