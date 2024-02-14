#region 유닛 정보 관련
public enum UnitType
{
    Character,
    Enemy
}
public enum UnitState
{
    Idle,
    Move,
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

#region 스테이지 정보 관련
public enum StageState
{ 
    Ready,
    Play,
    Clear
}
#endregion