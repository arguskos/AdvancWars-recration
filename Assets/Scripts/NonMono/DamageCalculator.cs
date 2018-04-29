
public class DamageCalculator
{
    public static float OriginalFormula(float  baseDamage, float attackerHealth,float deffenseTerrain, float deffenderHealeth,float attckingCO= 100, float deffenceCO = 100)
    {
        return ((baseDamage * attckingCO) / 100f) * (attackerHealth / 10f) * ((200f - (deffenceCO + deffenseTerrain * deffenderHealeth)) / 100.0f);

    }
}
