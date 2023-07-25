#!/bin/bash
rm -rf mod-build
mkdir -p mod-build/Assemblies
mkdir -p mod-build/About
FrameworkPathOverride="/usr/lib/mono/4.7.2-api/" dotnet build TweakWithdrawalEmergencies.csproj /property:Configuration=Release
cp bin/Release/TweakWithdrawalEmergencies.dll mod-build/Assemblies
cp About/* mod-build/About