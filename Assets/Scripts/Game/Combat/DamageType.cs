using System;

[Serializable]
public class DamageType {
    public bool God;
    public bool Heart;
    public bool Crabs;
    public bool Population;

    public bool MatchOneFrom(DamageType other)
    {
        return other.God 
            || (Heart && other.Heart)
            || (Crabs && other.Crabs)
            || (Population && other.Population);
    }
}

