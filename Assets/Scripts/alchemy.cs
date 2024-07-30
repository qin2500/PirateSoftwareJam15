using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

public class AlchemyUpgrade
{
    private HashSet<Element> elements;
    private Effect effect;

    public Effect Effect { get { return effect; } } 
    
    public override string ToString()
    {
        return elements.ToString();
    }

    public override int GetHashCode()
    {
        return elements.GetHashCode();
    }

    public static AlchemyUpgrade from(Element element1, Element? element2)
    {
        AlchemyUpgrade upgrade = new AlchemyUpgrade();
        upgrade.elements = new HashSet<Element> { element1};

        if (element2.HasValue)
        {
            upgrade.elements.Add(element2.Value);
        }

        return upgrade;
    }

    public override bool Equals(object obj)
    {
        AlchemyUpgrade otherElement = obj as AlchemyUpgrade;
        if (otherElement != null)
        {
            if (otherElement.elements.Equals(this.elements)) return true;
        }

        return false;

    }

    public void applyEffect(LightBulletController potion)
    {
        if (effect is PlayerEffect)
        {
            PlayerEffect playerEffect = (PlayerEffect)effect;

            playerEffect.tryApply(GlobalReferences.PLAYER);
        } else
        {
            PotionEffect potionEffect = (PotionEffect)effect;

            potionEffect.effectFunction(potion);
        }
    }
}

public enum Element
{
    Fire,
    Water,
    Grass
}

public static class AlchemyUpgradeFactory
{

}

public static class EffectFactory
{
    public static Effect from(Element element1, Element? element2)
    {
        //TODO: Fill in

        return new Effect();
    }
}

public enum EffectType
{
    PlayerEffect,
    PotionEffect
}

public class Effect
{
}

public class PlayerEffect: Effect
{
    public string name;
    public bool isApplied = false;
    public Action<Player> effectFunction;

    public PlayerEffect(string name, Action<Player> effectFunction)
    {
        this.name = name;  
        this.effectFunction = effectFunction;
    }

    public override string ToString()
    {
        return name;
    }

    public bool tryApply(Player player)
    {
        if (isApplied)
        {
            return false;
        }

        effectFunction(player);

        return true;
    }
}

public class PotionEffect:Effect
{
        public string name;
        public Action<LightBulletController> effectFunction;
}

public class Pentagram
{
    private HashSet<AlchemyUpgrade> upgrades;


    public void addUpgrade(AlchemyUpgrade upgrade)
    {
       upgrades.Add(upgrade);
    }

    public void combineUpgrades(Element element1, Element element2)
    {
        upgrades.Remove(AlchemyUpgrade.from(element1, element2));
    }

    public override string ToString()
    {
        return upgrades.ToString();
    }

    public void applyEffects()
    {
        foreach (var upgrade in upgrades)
        {
            applyEffects();
        }
    }
}