#pragma once
#include "Kismet/BlueprintFunctionLibrary.h"
#include "AIConfigTypes.h"
#include "AIConfigLoaderBPLib.generated.h"

UCLASS()
class UAIConfigLoaderBPLib : public UBlueprintFunctionLibrary
{
    GENERATED_BODY()
public:
    UFUNCTION(BlueprintCallable, Category = "AI|Config")
        static bool LoadAIConfigFromXML(const FString& RelativePathFromProjectDir, FAIConfigData& OutConfig, FString& OutError);
};