﻿using HugsLib;
using Verse;
using RimWorld;
using Xnope.Defs;
using System.Linq;

namespace Xnope
{
    [StaticConstructorOnStartup]
    public class XnopeCoreMod : ModBase
    {
        public override string ModIdentifier { get { return "XnopeCore"; } }

        //protected override bool HarmonyAutoPatch { get { return false; } }

        static XnopeCoreMod()
        {

        }


        public override void DefsLoaded()
        {
            InjectSpawnCategories();
        }


        private static void InjectSpawnCategories()
        {
            foreach (var injector in DefDatabase<SpawnCategoryInjectorDef>.AllDefs)
            {
                foreach (var targetBS in injector.injectToBackstories)
                {
                    foreach (var bs in (from b in BackstoryDatabase.allBackstories.Values
                                        where b.Title.Equals(targetBS)
                                            && !b.spawnCategories.Contains(injector.newCategory)
                                        select b))
                    {
                        if (injector.ignoreAdulthoods && bs.slot == BackstorySlot.Adulthood) continue;
                        if (injector.ignoreChildhoods && bs.slot == BackstorySlot.Childhood) continue;

                        bs.spawnCategories.Add(injector.newCategory);
                        if (Prefs.DevMode)
                            Log.Message("[XnopeCore] Added spawn category \'" + injector.newCategory + "\' to backstory \'" + bs.Title + "\'");
                    }
                }

                foreach (var targetCat in injector.injectToCategories)
                {
                    foreach (var bs in (from b in BackstoryDatabase.allBackstories.Values
                                        where b.spawnCategories.Contains(targetCat)
                                            && !b.identifier.StartsWith("XnopeBS")
                                            && !b.spawnCategories.Contains(injector.newCategory)
                                        select b))
                    {
                        if (injector.ignoreAdulthoods && bs.slot == BackstorySlot.Adulthood) continue;
                        if (injector.ignoreChildhoods && bs.slot == BackstorySlot.Childhood) continue;

                        bs.spawnCategories.Add(injector.newCategory);
                        if (Prefs.DevMode)
                            Log.Message("[XnopeCore] Added spawn category \'" + injector.newCategory + "\' to backstory \'" + bs.Title + "\'");
                    }
                }
            }
        }
    }
}
