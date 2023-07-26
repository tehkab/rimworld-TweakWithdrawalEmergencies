using System;
using System.Collections.Generic;
using HarmonyLib;
using Verse;
using RimWorld;

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

    [HarmonyPatch(typeof(Alert_LifeThreateningHediff), "get_SickPawns")]
    static class Patch_Withdrawal
    {
        //TODO what type is hd really?
        private static bool hasLifeThreateningHediffNotOfType(Pawn p, Object hd)
        {
            foreach(Hediff h in p.health.hediffSet.hediffs)
            {
                if(h != null && h.def != hd && h.CurStage != null && h.CurStage.lifeThreatening && !h.FullyImmune())
                    return true;
            }

            return false;
        }

        private static List<Hediff> getDrugDependencyHediffsFor(Pawn p)
        {
            List<Hediff> ret = null;

            foreach(Hediff h in p.health.hediffSet.hediffs)
            {
                if(h.def == HediffDefOf.GeneticDrugNeed)
                {
                    if(ret == null)
                        ret = new List<Hediff>();
                    
                    ret.Add(h);
                }
            }

            return ret;
        }

        static void Postfix(ref List<Pawn> __result)
        {
            if (__result.Count <= 0)
                return;

            List<Pawn> ret_SickPawns = null;

            float threshold = Settings.colonistSeverityThreshold;

            foreach(Pawn p in __result)
            {
                List<Hediff> drugDependencies = getDrugDependencyHediffsFor(p);
                
                //no drug dependencies, ignore
                if(drugDependencies == null)
                {
                    continue;
                }

                //ignore if anything else life threatening
                if(hasLifeThreateningHediffNotOfType(p, HediffDefOf.GeneticDrugNeed))
                {
                    continue;
                }

                //set threshold
                if(p.IsPrisonerOfColony) //prisoner
                {
                    threshold = Settings.prisonerSeverityThreshold;
                }

                //test drug dependencies (could be multiple)
                bool over_threshold = false;
                foreach(Hediff h in drugDependencies)
                {
                    if(h.Severity > threshold)
                    {
                        over_threshold = true;
                    }
                }

                //ignore if no drug dependency hediff is over our set threshold
                if(over_threshold)
                {
                    continue;
                }
                
                //exclude this pawn from medical emergency alerts
                if(ret_SickPawns == null)
                {
                    ret_SickPawns = __result.ListFullCopy();
                }
                ret_SickPawns.Remove(p);
            }

            if(ret_SickPawns != null)
            {
                __result = ret_SickPawns;
            }
        }
    }
}