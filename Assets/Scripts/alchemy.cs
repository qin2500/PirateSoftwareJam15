using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

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

    public AlchemyUpgrade (Element element1, Element? element2)
    {
        this.elements = new HashSet<Element> { element1 };

        this.elements.Add(element1);
        if (element2.HasValue) this.elements.Add(element2.Value);

        this.effect = new Effect ();
    }

    public static AlchemyUpgrade from(Element element1, Element? element2)
    {
        AlchemyUpgrade upgrade = new AlchemyUpgrade(element1, element2);
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

    public bool isCombination()
    {
        return this.elements.Count > 1;
    }

    public Element getFirstElement()
    {
        return this.elements.First();
    }

    public Element? getOtherElement(Element firstElement)
    {
        return this.elements.Where(element => element != firstElement).FirstOrDefault(null);
    }
}

public enum Element
{
    Fire,
    Water,
    Grass
}

public static class EffectFactory
{
    public static Effect FIRE_EFFECT = new PlayerEffect("Boosted attack speed", player => player.potionCooldown -= 2);
    public static Effect WATER_EFFECT = new PotionEffect("Boosted aoe", potion => potion.radius *= (float) 1.2);
    public static Effect GRASS_EFFECT = new PotionEffect("Boosted knockback", potion => potion.knockbackForce *= (float)1.2);
    public static Effect FIRE_FIRE_EFFECT = new PotionEffect("Burn Damage", potion => potion.burnEnemies = true);
    public static Effect WATER_WATER_EFFECT = new PotionEffect("Bounce off enemies", potion => potion.chainBounce = true);
    public static Effect GRASS_GRASS_EFFECT = new PotionEffect("Enemies drop healing items on hit", potion => potion.healOnHit = true);
    public static Effect FIRE_WATER_EFFECT = new PotionEffect("Smoke cloud on enemy death", potion => potion.smokeOnDeath = true);
    public static Effect FIRE_GRASS_EFFECT = new PotionEffect("Enemy explodes on death", potion => potion.explodeOnDeath = true);
    public static Effect WATER_GRASS_EFFECT = new PotionEffect("Enemy slows on hit", potion => potion.slowFrames = 5);

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

    public PotionEffect(string name, Action<LightBulletController> effectFunction)
    {
        this.name = name;
        this.effectFunction = effectFunction;
    }       
}

public class Pentagram
{
    private HashSet<AlchemyUpgrade> upgrades;

    public Pentagram()
    {
        upgrades = new HashSet<AlchemyUpgrade>();
    }
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

    public void applyEffects(LightBulletController potion)
    {
        foreach (var upgrade in upgrades)
        {
            Debug.Log("Applying Effect");
            upgrade.applyEffect(potion);
        }
    }

    public int getNumCombinations()
    {
        return upgrades.Where(upgrade => upgrade.isCombination()).Count();
    }

    public List<AlchemyUpgrade> getUpgrades()
    {
        return upgrades.ToList();
    }
}

public static class AlchemyConstants
{

}