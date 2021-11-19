using System;

[Serializable]
public class PlayerData : IComparable<PlayerData>
{
    private readonly int _score;
    private readonly string _name;
    private readonly Guid _guid;

    public PlayerData()
    {
        _score = -1;
        _name = "ZYM";
        _guid = Guid.Empty;
    }

    public PlayerData(int score, string name, Guid guid)
    {
        _score = score;
        _name = name;
        _guid = guid;
    }

    public int GetScore()
    {
        return _score;
    }

    public string GetName()
    {
        return _name;
    }

    public int CompareTo(PlayerData other)
    {
        return -_score.CompareTo(other._score);
    }

    public bool ExactEqual(PlayerData other)
    {
        return _guid.Equals(other._guid);
    }
}