public class CombatResolver
{
    public enum Actor { Player, Monster }

    public struct Outcome
    {
        public Actor WhoActs;
        public int NewPlayerCooldown;
        public int NewMonsterCooldown;
    }

    public Outcome Resolve(int playerCooldown, int playerSpeed, int monsterCooldown, int monsterSpeed, int initialCooldown = 100)
    {
        int pc = playerCooldown;
        int mc = monsterCooldown;

        while (true)
        {
            if (pc <= 0)
                return new Outcome { WhoActs = Actor.Player, NewPlayerCooldown = initialCooldown + pc, NewMonsterCooldown = mc };
            if (mc <= 0)
                return new Outcome { WhoActs = Actor.Monster, NewPlayerCooldown = pc, NewMonsterCooldown = initialCooldown + mc };

            pc -= playerSpeed;
            mc -= monsterSpeed;
        }
    }
}
