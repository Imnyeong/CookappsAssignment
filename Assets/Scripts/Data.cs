#region Unit
public enum UnitType
{
    Character,
    Enemy
}
public enum UnitState
{
    Idle,
    Move,
    Attack,
    Death
}
public enum Elemental
{
    Fire,
    Water,
    Grass
}
public enum Profession
{
    Tanker,
    Warrior,
    Supporter
}
public enum SkillType
{
    Attack,
    Heal,
    Special
}
#endregion

#region Stage
public enum StageState
{ 
    Ready,
    Play,
    Clear,
    Defeat
}
#endregion