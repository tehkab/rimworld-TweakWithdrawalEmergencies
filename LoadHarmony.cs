using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using HarmonyLib;

namespace TweakWithdrawalEmergencies
{
    [StaticConstructorOnStartup]
    static class LoadHarmony
    {
        static LoadHarmony()
        {
            Harmony harmony = new Harmony("Kabby.Tweaks.WithdrawalEmergencies");
            harmony.PatchAll();
        }
    }
}