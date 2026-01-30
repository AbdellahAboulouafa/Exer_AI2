#pragma once
#include "CoreMinimal.h"
#include "AIConfigTypes.generated.h"

USTRUCT(BlueprintType)
struct FAIConfigData
{
    GENERATED_BODY()

    UPROPERTY(BlueprintReadWrite) float SightRadius = 1500.f;
    UPROPERTY(BlueprintReadWrite) float LoseSightRadius = 1800.f;
    UPROPERTY(BlueprintReadWrite) float PeripheralVisionDegrees = 70.f;
    UPROPERTY(BlueprintReadWrite) float HearingRange = 1200.f;

    UPROPERTY(BlueprintReadWrite) float SearchDurationSeconds = 4.f;
};