using Dalamud.Game.ClientState.Statuses;

namespace XIVComboExpandedestPlugin.Combos
{
    internal static class PLD
    {
        public const byte ClassID = 1;
        public const byte JobID = 19;

        public const uint
            FightorFlight = 20,
            GoringBlade = 3538,
            HolySpirit = 7384,
            HolyCircle = 16458,
            Confiteor = 16459,
            BladeOfFaith = 25748,
            BladeOfTruth = 25749,
            BladeOfValor = 25750,
            Intervene = 16461,

            FastBlade = 9,
            RiotBlade = 15,
            ShieldBash = 16,
            NotNoMercy = 20,
            RageOfHalone = 21,
            NotSonicBreak = 3538,
            RoyalAuthority = 3539,
            LowBlow = 7540,
            TotalEclipse = 7381,
            Requiescat = 7383,
            NotBurstStrike = 7384,
            Prominence = 16457,
            NotFatedCircle = 16458,
            NotGnashingFangCombo = 16459,
            Atonement = 16460,
            NotGnashingFang = 25748,
            NotSavageClaw = 25749,
            NotWickedTalon = 25750,
            SpiritsWithin = 29,
            Expiacion = 25747,
            CircleOfScorn = 23,
            ShieldLob = 24;

        public static class Buffs
        {
            public const ushort
                BladeOfFaithReady = 3019,
                NotNoMercy = 76,
                Requiescat = 1368,
                SwordOath = 1902,
                DivineMight = 2673;
        }

        public static class Debuffs
        {
            public const ushort
                Placeholder = 0,
                GoringBlade = 725;
        }

        public static class Levels
        {
            public const byte
                NotSonicBreak = 54,
                NotFatedCircle = 72,
                NotGnashingFangCombo = 80,

                FightorFlight = 2,
                RiotBlade = 4,
                LowBlow = 12,
                SpiritsWithin = 30,
                CircleOfScorn = 50,
                RageOfHalone = 26,
                Prominence = 40,
                GoringBlade = 54,
                RoyalAuthority = 60,
                Requiescat = 68,
                HolyCircle = 72,
                Intervene = 74,
                Atonement = 76,
                Confiteor = 80,
                Expiacion = 86,
                BladeOfFaith = 90,
                BladeOfTruth = 90,
                BladeOfValor = 90;

        }
    }

    internal class PaladinLowBashFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.PaladinLowBashFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (IsEnabled(CustomComboPreset.AllTankInterruptFeature) && CanInterruptEnemy() && IsActionOffCooldown(All.Interject))
                return All.Interject;

            return IsActionOffCooldown(All.LowBlow) ? All.LowBlow : actionID;
        }
    }

    internal class PaladinNotNoMercyFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.PaladinNotNoMercyToNotSonicBreak;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == PLD.NotNoMercy)
            {
                if (HasEffect(PLD.Buffs.NotNoMercy))
                {
                    if (IsActionOffCooldown(PLD.NotSonicBreak) && CanUseAction(PLD.NotSonicBreak))
                        return PLD.NotSonicBreak;
                }
            }

            return actionID;
        }
    }

    /*internal class PaladinRoyalAuthorityAtonementFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.PaladinRoyalAuthorityAtonementFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            return HasEffect(PLD.Buffs.SwordOath) ? PLD.Atonement : actionID;
        }
    }

    internal class PaladinAtonementFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.PaladinNotBurstStrikeToAtonement;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            return actionID == PLD.NotBurstStrike && ((!HasEffect(PLD.Buffs.DivineMight) && !HasEffect(PLD.Buffs.Requiescat)) || LocalPlayer?.CurrentMp < 1000) && HasEffect(PLD.Buffs.SwordOath) && InMeleeRange() ? PLD.Atonement : actionID;
        }
    }*/

    internal class PaladinHolyCircleFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.PaladinNotFatedCircleFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if ((actionID == PLD.TotalEclipse || actionID == PLD.Prominence) && HasEffect(PLD.Buffs.Requiescat) && LocalPlayer?.CurrentMp >= 1000)
            {
                if (level >= PLD.Levels.NotGnashingFangCombo || OriginalHook(PLD.NotGnashingFangCombo) != PLD.NotGnashingFangCombo)
                    return OriginalHook(PLD.NotGnashingFangCombo);

                if (IsEnabled(CustomComboPreset.PaladinRequiescatComboSpirit))
                {
                    if (CanUseAction(PLD.NotBurstStrike) && !CanUseAction(OriginalHook(PLD.NotGnashingFangCombo)))
                        return (this.FilteredLastComboMove == PLD.Prominence || this.FilteredLastComboMove == PLD.TotalEclipse) && level >= PLD.Levels.NotFatedCircle ? PLD.NotFatedCircle : PLD.NotBurstStrike;
                }
            }

            return actionID;
        }
    }

    internal class PaladinRoyalAuthorityCombo : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.PaladinRoyalAuthorityCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == PLD.RoyalAuthority || actionID == PLD.RageOfHalone)
            {
                if (IsEnabled(CustomComboPreset.PaladinRoyalLobFeature))
                {
                    if (CanUseAction(PLD.ShieldLob) && !InMeleeRange())
                        return PLD.ShieldLob;
                }

                if (comboTime > 0)
                {
                    if (lastComboMove == PLD.FastBlade && level >= PLD.Levels.RiotBlade)
                        return PLD.RiotBlade;

                    if (lastComboMove == PLD.RiotBlade && level >= PLD.Levels.RageOfHalone)
                        return OriginalHook(PLD.RageOfHalone);
                }

                return PLD.FastBlade;
            }

            return actionID;
        }
    }

    internal class PaladinProminenceCombo : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.PaladinProminenceCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == (IsEnabled(CustomComboPreset.PaladinEvilProminenceCombo) ? PLD.TotalEclipse : PLD.Prominence))
            {
                if (comboTime > 0)
                {
                    if (lastComboMove == PLD.TotalEclipse && CanUseAction(PLD.Prominence) && LocalPlayer?.CurrentMp >= 1000)
                        return IsEnabled(CustomComboPreset.PaladinNotFatedCircleOvercapFeature) && HasEffect(PLD.Buffs.DivineMight) && level >= PLD.Levels.NotFatedCircle ? PLD.NotFatedCircle : PLD.Prominence;
                }

                return PLD.TotalEclipse;
            }

            return actionID;
        }
    }

    internal class PaladinRequiescatCombo : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.PaladinRequiescatCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == PLD.Requiescat && HasEffect(PLD.Buffs.Requiescat))
            {
                if (level >= PLD.Levels.NotGnashingFangCombo || OriginalHook(PLD.NotGnashingFangCombo) != PLD.NotGnashingFangCombo)
                    return OriginalHook(PLD.NotGnashingFangCombo);

                if (IsEnabled(CustomComboPreset.PaladinRequiescatComboSpirit))
                {
                    if (CanUseAction(PLD.NotBurstStrike) && !CanUseAction(OriginalHook(PLD.NotGnashingFangCombo)))
                        return (this.FilteredLastComboMove == PLD.Prominence || this.FilteredLastComboMove == PLD.TotalEclipse) && level >= PLD.Levels.NotFatedCircle ? PLD.NotFatedCircle : PLD.NotBurstStrike;
                }
            }

            return actionID;
        }
    }

    internal class PaladinHolySpiritToHolyCircleFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.PaladinHolySpiritToHolyCircleFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == PLD.NotBurstStrike)
            {
                if ((this.FilteredLastComboMove == PLD.Prominence || this.FilteredLastComboMove == PLD.TotalEclipse) && level >= PLD.Levels.NotFatedCircle)
                    return PLD.NotFatedCircle;
            }

            return actionID;
        }
    }

    internal class PaladinScornfulSpiritsFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.PaladinScornfulSpiritsFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (level < PLD.Levels.CircleOfScorn)
                return PLD.SpiritsWithin;

            var oppositeActionID = actionID == PLD.SpiritsWithin || actionID == PLD.Expiacion ? PLD.CircleOfScorn : PLD.SpiritsWithin;

            if (!IsActionOffCooldown(actionID) && IsActionOffCooldown(oppositeActionID))
                return OriginalHook(oppositeActionID);

            return actionID;
        }
    }

    internal class PaladinRoyalAuthorityCombo_v2 : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.PaladinRoyalAuthorityCombo_v2;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == PLD.RoyalAuthority)
            {
                if (HasEffect(PLD.Buffs.Requiescat))
                {
                    if (level >= PLD.Levels.NotGnashingFangCombo || OriginalHook(PLD.NotGnashingFangCombo) != PLD.NotGnashingFangCombo)
                    {
                        return OriginalHook(PLD.NotGnashingFangCombo);
                    }
                    else
                    {
                        return PLD.NotBurstStrike;
                    }
                }

                if (HasEffect(PLD.Buffs.SwordOath))
                    return PLD.Atonement;
                if (HasEffect(PLD.Buffs.DivineMight))
                    return PLD.NotBurstStrike;

                if (comboTime > 0)
                {
                    if (lastComboMove == PLD.FastBlade && level >= PLD.Levels.RiotBlade)
                        return PLD.RiotBlade;

                    if (lastComboMove == PLD.RiotBlade && level >= PLD.Levels.RageOfHalone)
                        return OriginalHook(PLD.RageOfHalone);
                }

                return PLD.FastBlade;
            }

            return actionID;
        }
    }

}
