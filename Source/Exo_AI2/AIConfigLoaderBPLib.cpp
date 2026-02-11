#include "AIConfigLoaderBPLib.h"
#include "Misc/FileHelper.h"
#include "Misc/Paths.h"
#include "XmlFile.h"

// Lit un float depuis une balise XML
static bool ReadFloatTag(const FXmlNode* Parent, const FString& Tag, float& OutVal)
{
    if (!Parent) return false;
    const FXmlNode* Node = Parent->FindChildNode(Tag);
    if (!Node) return false;
    OutVal = FCString::Atof(*Node->GetContent());
    return true;
}

bool UAIConfigLoaderBPLib::LoadAIConfigFromXML(const FString& RelativePathFromProjectDir, FAIConfigData& OutConfig, FString& OutError)
{
    OutError = "";
    const FString FullPath = FPaths::Combine(FPaths::ProjectDir(), RelativePathFromProjectDir);

    FString XmlText;
    if (!FFileHelper::LoadFileToString(XmlText, *FullPath))
    {
        OutError = FString::Printf(TEXT("Impossible de lire le fichier: %s"), *FullPath);
        return false;
    }

    FXmlFile XmlFile(XmlText, EConstructMethod::ConstructFromBuffer);
    if (!XmlFile.IsValid())
    {
        OutError = "XML invalide (vérifier la syntaxe).";
        return false;
    }

    const FXmlNode* Root = XmlFile.GetRootNode(); // <AIConfig>
    if (!Root)
    {
        OutError = "Aucune racine <AIConfig>.";
        return false;
    }

    const FXmlNode* Perception = Root->FindChildNode("Perception");
    ReadFloatTag(Perception, "SightRadius", OutConfig.SightRadius);
    ReadFloatTag(Perception, "LoseSightRadius", OutConfig.LoseSightRadius);
    ReadFloatTag(Perception, "PeripheralVisionDegrees", OutConfig.PeripheralVisionDegrees);
    ReadFloatTag(Perception, "HearingRange", OutConfig.HearingRange);

    const FXmlNode* Behavior = Root->FindChildNode("Behavior");
    ReadFloatTag(Behavior, "SearchDurationSeconds", OutConfig.SearchDurationSeconds);

    const FXmlNode* Fuzzy = Root->FindChildNode("Fuzzy");
    ReadFloatTag(Fuzzy, "CloseMax", OutConfig.CloseMax);
    ReadFloatTag(Fuzzy, "FarMin", OutConfig.FarMin);
    ReadFloatTag(Fuzzy, "LowHP", OutConfig.LowHP);
    ReadFloatTag(Fuzzy, "HighHP", OutConfig.HighHP);


    return true;
}