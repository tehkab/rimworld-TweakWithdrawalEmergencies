using UnityEngine;
using Verse;

namespace TweakWithdrawalEmergencies
{
    public class Settings : ModSettings
    {
        //defaults
        public const float DEFAULT_THRESHOLD_COLONISTS = 6.0f; //6.0 is the default 30 days
        public const float DEFAULT_THRESHOLD_PRISONERS = 11.0f; //11.0 is at 55 of 60 days
        public const float MIN_THRESHOLD = 6.0f; //30 days, comatose
        public const float MAX_THRESHOLD = 12.0f; //60 days, dead

        //0.2 per day
        public static float colonistSeverityThreshold = DEFAULT_THRESHOLD_COLONISTS; 
        public static float prisonerSeverityThreshold = DEFAULT_THRESHOLD_PRISONERS; 

        public override void ExposeData()
        {
            Scribe_Values.Look(ref colonistSeverityThreshold, "colonistSeverityThreshold", DEFAULT_THRESHOLD_COLONISTS);
            Scribe_Values.Look(ref prisonerSeverityThreshold, "prisonerSeverityThreshold", DEFAULT_THRESHOLD_PRISONERS);
            base.ExposeData();
        }
    }

    public class TweakWithdrawalEmergenciesMod : Mod
    {
        public Settings settings;

        public TweakWithdrawalEmergenciesMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<Settings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            int hours,days;
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            
            listingStandard.Label(TranslatorFormattedStringExtensions.Translate("OptionKabbyTWEDescription"));

            if(Settings.colonistSeverityThreshold < Settings.MAX_THRESHOLD)
            {
                hours = (int)(Settings.colonistSeverityThreshold * 120.0f); 
                days = hours / 24;
                hours %= 24;            
                
                listingStandard.Label(TranslatorFormattedStringExtensions.Translate("OptionKabbyTWEColonistThreshold", days, hours));
            }
            else
            {
                listingStandard.Label(TranslatorFormattedStringExtensions.Translate("OptionKabbyTWEColonistThresholdNever"));
            }
            Settings.colonistSeverityThreshold = listingStandard.Slider(Settings.colonistSeverityThreshold, Settings.MIN_THRESHOLD, Settings.MAX_THRESHOLD);

            if(Settings.prisonerSeverityThreshold < Settings.MAX_THRESHOLD)
            {
                hours = (int)(Settings.prisonerSeverityThreshold * 120.0f); 
                days = hours / 24;
                hours %= 24;            
                
                listingStandard.Label(TranslatorFormattedStringExtensions.Translate("OptionKabbyTWEPrisonerThreshold", days, hours));
            }
            else
            {
                listingStandard.Label(TranslatorFormattedStringExtensions.Translate("OptionKabbyTWEPrisonerThresholdNever"));
            }
            Settings.prisonerSeverityThreshold = listingStandard.Slider(Settings.prisonerSeverityThreshold, Settings.MIN_THRESHOLD, Settings.MAX_THRESHOLD);

            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return Translator.Translate("TweakWithdrawalEmergencies");
        }
    }
}