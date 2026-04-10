using Verse;

namespace TheFlesh
{
    public static class TheFleshTools
    {
        public static bool isInfectible(Pawn victim)
        {
            RaceProperties rProps = victim.RaceProps;
            return (victim.Spawned &&
                rProps.IsFlesh &&                
                !rProps.IsAnomalyEntity &&
                !rProps.Dryad &&
                !rProps.IsDrone &&
                !rProps.IsMechanoid &&
                !rProps.IsWorkMech
                );
        }
    }
}
