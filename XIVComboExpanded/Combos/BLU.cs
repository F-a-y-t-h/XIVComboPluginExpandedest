using Dalamud.Game.ClientState.Statuses;

using System.Security.Principal;

namespace XIVComboExpandedestPlugin.Combos
{
    internal static class BLU
    {
        public const byte JobID = 36;

        public const uint
            Addle = 7560,
            RoseOfDestruction = 23275,
            ShockStrike = 11429,
            JKick = 18325,
            Eruption = 11427,
            GlassDance = 11430,
            Surpanakha = 18323,
            Nightbloom = 23290,
            MoonFlute = 11415,
            Whistle = 18309,
            Tingle = 23265,
            TripleTrident = 23264,
            FinalSting = 11407,
            Bristle = 11393,
            MatraMagic = 23285,
            Chirp = 18301,
            PhantomFlurry = 23288,
            SongOfTorment = 11386;

        public static class Buffs
        {
            public const ushort
                MoonFlute = 1718,
                Bristle = 1716,
                WaningNocturne = 1727,
                PhantomFlurry = 2502,
                Tingle = 2492,
                Whistle = 2118,
                TankMimicry = 2124,
                DPSMimicry = 2125,
                BasicInstinct = 2498,
                Supana = 2130;
        }

        public static class Debuffs
        {
            public const ushort
                Slow = 9,
                Bind = 13,
                Stun = 142,
                SongOfTorment = 273,
                DeepFreeze = 1731,
                Offguard = 1717,
                Malodorous = 1715,
                Conked = 2115,
                Lightheaded = 2501;
        }

        public static class Levels
        {
            public const byte
                LucidDreaming = 24;
        }
    }

    internal class BluDoT : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.BluDoT;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BLU.Bristle)
            {
                if (!HasEffect(BLU.Buffs.Bristle))
                    return BLU.Bristle;
                return BLU.SongOfTorment;
            }

            if (actionID == BLU.RoseOfDestruction)
            {
                var dotdura = FindTargetEffect(BLU.Debuffs.SongOfTorment);

                if (dotdura == null || (dotdura != null && dotdura.RemainingTime <= 7))
                {
                    if (GetCooldown(BLU.RoseOfDestruction).IsCooldown)
                    {
                        if (!HasEffect(BLU.Buffs.Bristle) && lastComboMove != BLU.Bristle)
                            return BLU.Bristle;
                        return BLU.SongOfTorment;
                    }
                }

                return BLU.RoseOfDestruction;
            }

            return actionID;
        }
    }

    internal class BluTingle : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.BluTingle;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BLU.Tingle)
            {
                if (!HasEffect(BLU.Buffs.Whistle))
                    return BLU.Whistle;

                if (!HasEffect(BLU.Buffs.Tingle) && CurrentTarget is not null && lastComboMove != BLU.Tingle)
                    return BLU.Tingle;

                if (HasEffect(BLU.Buffs.Tingle))
                    return BLU.TripleTrident;

                return BLU.Whistle;
            }

            return actionID;
        }
    }

    internal class BluLimitBreak : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.BluLimitBreak;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BLU.FinalSting)
            {
                if (!HasEffect(BLU.Buffs.Whistle))
                    return BLU.Whistle;
                if (!HasEffect(BLU.Buffs.MoonFlute))
                    return BLU.MoonFlute;
                return BLU.FinalSting;
            }

            return actionID;
        }
    }

    internal class BluBurstCombo : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.BluBurstCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BLU.Nightbloom)
            {
                if (!GetCooldown(BLU.JKick).IsCooldown)
                    return BLU.JKick;
                if (!GetCooldown(BLU.Nightbloom).IsCooldown)
                    return BLU.Nightbloom;
                if (!GetCooldown(BLU.ShockStrike).IsCooldown)
                    return BLU.ShockStrike;
                if (!GetCooldown(BLU.GlassDance).IsCooldown)
                    return BLU.GlassDance;
                return BLU.Surpanakha;
            }

            return actionID;
        }
    }

    internal class BluTridentCombo : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.BluTridentCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BLU.Chirp)
            {
                if (HasEffect(BLU.Buffs.PhantomFlurry))
                    return OriginalHook(BLU.PhantomFlurry);

                if (HasEffect(BLU.Buffs.Supana))
                {
                    return BLU.Surpanakha;
                }

                if (GetCooldown(BLU.TripleTrident).CooldownRemaining < 5)
                {
                    if (!HasEffect(BLU.Buffs.Whistle) && lastComboMove != BLU.Whistle)
                        return BLU.Whistle;
// if (!HasEffect(BLU.Buffs.Tingle) && lastComboMove != BLU.Tingle)
//                        return BLU.Tingle;
                    if (!HasEffect(BLU.Buffs.MoonFlute) && lastComboMove != BLU.MoonFlute && !GetCooldown(BLU.Nightbloom).IsCooldown) // && !HasCondition(ConditionFlag.InCombat))
                        return BLU.MoonFlute;
                    if (!GetCooldown(BLU.JKick).IsCooldown)
                        return BLU.JKick;
                    if (!GetCooldown(BLU.TripleTrident).IsCooldown)
                        return BLU.TripleTrident;
                }

                if (!GetCooldown(BLU.JKick).IsCooldown)
                    return BLU.JKick;

                if (!GetCooldown(BLU.MatraMagic).IsCooldown)
                {
                    if (!HasEffect(BLU.Buffs.Bristle) && lastComboMove != BLU.Bristle)
                        return BLU.Bristle;

                    if (!GetCooldown(All.Swiftcast).IsCooldown)
                        return All.Swiftcast;

                    return BLU.MatraMagic;
                }

                if (!GetCooldown(BLU.Nightbloom).IsCooldown)
                    return BLU.Nightbloom;

                if (!GetCooldown(BLU.ShockStrike).IsCooldown)
                    return BLU.ShockStrike;
                if (!GetCooldown(BLU.GlassDance).IsCooldown)
                    return BLU.GlassDance;

                return BLU.Surpanakha;
            }

            return actionID;
        }
    }
}