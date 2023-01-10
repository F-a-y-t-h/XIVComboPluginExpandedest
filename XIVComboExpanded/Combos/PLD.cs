using Dalamud.Game.ClientState.Statuses;

namespace XIVComboExpandedestPlugin.Combos
{
    internal static class PLD
    {
        public const byte ClassID = 1;
        public const byte JobID = 19;

        public const uint
            FastBlade = 9,
            RiotBlade = 15,
            ShieldBash = 16,
            FightOrFlight = 20,
            RageOfHalone = 21,
            GoringBlade = 3538,
            RoyalAuthority = 3539,
            LowBlow = 7540,
            TotalEclipse = 7381,
            Requiescat = 7383,
            HolySpirit = 7384,
            Prominence = 16457,
            HolyCircle = 16458,
            Confiteor = 16459,
            Atonement = 16460,
            BladeOfFaith = 25748,
            BladeOfTruth = 25749,
            BladeOfValor = 25750,
            SpiritsWithin = 29,
            Expiacion = 25747,
            CircleOfScorn = 23,
            ShieldLob = 24;

        public static class Buffs
        {
            public const ushort
                FightOrFlight = 76,
                Requiescat = 1368,
                SwordOath = 1902,
                DivineMight = 0;
        }

        public static class Debuffs
        {
            public const ushort Placeholder = 0;
        }

        public static class Levels
        {
            public const byte
                RiotBlade = 4,
                RageOfHalone = 26,
                Prominence = 40,
                CircleOfScorn = 50,
                GoringBlade = 54,
                RoyalAuthority = 60,
                HolyCircle = 72,
                Atonement = 76,
                Confiteor = 80,
                Expiacion = 86;
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

    internal class PaladinNoMercyFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.PaladinNotNoMercyToNotSonicBreak;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == PLD.FightOrFlight)
            {
                if (HasEffect(PLD.Buffs.FightOrFlight))
                {
                    if (IsActionOffCooldown(PLD.GoringBlade) && CanUseAction(PLD.GoringBlade))
                        return PLD.GoringBlade;
                }
            }

            return actionID;
        }
    }

    internal class PaladinAtonementFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.PaladinNotBurstStrikeToAtonement;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            return actionID == PLD.HolySpirit && ((!HasEffect(PLD.Buffs.DivineMight) && !HasEffect(PLD.Buffs.Requiescat)) || LocalPlayer?.CurrentMp < 1000) && HasEffect(PLD.Buffs.SwordOath) ? PLD.Atonement : actionID;
        }
    }

    internal class PaladinHolyCircleFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.PaladinNotFatedCircleFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if ((actionID == PLD.TotalEclipse || actionID == PLD.Prominence) && HasEffect(PLD.Buffs.Requiescat) && LocalPlayer?.CurrentMp >= 1000)
            {
                if (level >= PLD.Levels.Confiteor || OriginalHook(PLD.Confiteor) != PLD.Confiteor)
                    return OriginalHook(PLD.Confiteor);

                if (IsEnabled(CustomComboPreset.PaladinRequiescatComboSpirit))
                {
                    if (CanUseAction(PLD.HolySpirit) && !CanUseAction(OriginalHook(PLD.Confiteor)))
                        return (this.FilteredLastComboMove == PLD.Prominence || this.FilteredLastComboMove == PLD.TotalEclipse) && level >= PLD.Levels.HolyCircle ? PLD.HolyCircle : PLD.HolySpirit;
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
                        return IsEnabled(CustomComboPreset.PaladinNotFatedCircleOvercapFeature) && HasEffect(PLD.Buffs.DivineMight) && level >= PLD.Levels.HolyCircle ? PLD.HolyCircle : PLD.Prominence;
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
                if (level >= PLD.Levels.Confiteor || OriginalHook(PLD.Confiteor) != PLD.Confiteor)
                    return OriginalHook(PLD.Confiteor);

                if (IsEnabled(CustomComboPreset.PaladinRequiescatComboSpirit))
                {
                    if (CanUseAction(PLD.HolySpirit) && !CanUseAction(OriginalHook(PLD.Confiteor)))
                        return (this.FilteredLastComboMove == PLD.Prominence || this.FilteredLastComboMove == PLD.TotalEclipse) && level >= PLD.Levels.HolyCircle ? PLD.HolyCircle : PLD.HolySpirit;
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
            if (actionID == PLD.HolySpirit)
            {
                if ((this.FilteredLastComboMove == PLD.Prominence || this.FilteredLastComboMove == PLD.TotalEclipse) && level >= PLD.Levels.HolyCircle)
                    return PLD.HolyCircle;
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
}
