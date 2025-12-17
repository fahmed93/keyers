# Example Character Classes

Below are example configurations for the three character classes. These can be created in Unity by right-clicking in the Project window and selecting Create → RPG → Classes.

## Mage Class

```yaml
Class Name: Mage
Description: Masters of arcane magic, mages deal high spell damage but have lower health and defense.

Base Stats:
  Base Health: 100
  Base Damage: 15
  Base Defense: 5

Scaling Factors:
  Health Scale Factor: 1.08
  Damage Scale Factor: 1.06
  Defense Scale Factor: 1.04

Mage Specific:
  Spell Power Multiplier: 1.5
  Mana Cost Reduction: 0.9

Recommended Abilities:
  - Fireball (Damage)
  - Ice Blast (Damage)
  - Mana Shield (Defensive)
  - Counterspell (Interrupt)
```

**Stats at Level 10:**
- Health: 100 × (10^1.08) = 100 × 11.22 = 1,122
- Damage: 15 × (10^1.06) = 15 × 10.72 = 161
- Defense: 5 × (10^1.04) = 5 × 10.23 = 51

## Warrior Class

```yaml
Class Name: Warrior
Description: Heavily armored fighters with high health and physical damage.

Base Stats:
  Base Health: 150
  Base Damage: 20
  Base Defense: 10

Scaling Factors:
  Health Scale Factor: 1.12
  Damage Scale Factor: 1.05
  Defense Scale Factor: 1.06

Warrior Specific:
  Physical Damage Bonus: 1.3
  Armor Multiplier: 1.5

Recommended Abilities:
  - Mortal Strike (Damage)
  - Charge (Damage)
  - Shield Wall (Defensive)
  - Pummel (Interrupt)
```

**Stats at Level 10:**
- Health: 150 × (10^1.12) = 150 × 12.88 = 1,932
- Damage: 20 × (10^1.05) = 20 × 10.59 = 212
- Defense: 10 × (10^1.06) = 10 × 10.72 = 107

## Priest Class

```yaml
Class Name: Priest
Description: Divine healers with powerful support abilities and moderate combat power.

Base Stats:
  Base Health: 120
  Base Damage: 10
  Base Defense: 7

Scaling Factors:
  Health Scale Factor: 1.10
  Damage Scale Factor: 1.04
  Defense Scale Factor: 1.05

Priest Specific:
  Healing Power Multiplier: 1.4
  Shield Strength Bonus: 1.2

Recommended Abilities:
  - Smite (Damage)
  - Flash Heal (Heal)
  - Power Word: Shield (Defensive)
  - Silence (Interrupt)
```

**Stats at Level 10:**
- Health: 120 × (10^1.10) = 120 × 12.59 = 1,511
- Damage: 10 × (10^1.04) = 10 × 10.23 = 102
- Defense: 7 × (10^1.05) = 7 × 10.59 = 74

---

# Example Abilities

Below are example ability configurations for each class.

## Damage Abilities

### Fireball (Mage)
```yaml
Name: Fireball
Description: Hurl a ball of fire at the enemy
Type: Damage
Target: Enemy
Cooldown: 3 seconds
Cast Time: 1.5 seconds
Can Be Interrupted: Yes
Value: 30
Button Color: #FF6600 (Orange)
```

### Mortal Strike (Warrior)
```yaml
Name: Mortal Strike
Description: A powerful strike that deals heavy damage
Type: Damage
Target: Enemy
Cooldown: 4 seconds
Cast Time: 1.0 seconds
Can Be Interrupted: Yes
Value: 40
Button Color: #CC0000 (Red)
```

### Smite (Priest)
```yaml
Name: Smite
Description: Strike the enemy with holy power
Type: Damage
Target: Enemy
Cooldown: 3 seconds
Cast Time: 2.0 seconds
Can Be Interrupted: Yes
Value: 25
Button Color: #FFCC00 (Gold)
```

## Healing Abilities

### Flash Heal (Priest)
```yaml
Name: Flash Heal
Description: Quickly heal yourself
Type: Heal
Target: Self
Cooldown: 5 seconds
Cast Time: 1.5 seconds
Can Be Interrupted: Yes
Value: 50
Button Color: #00CC00 (Green)
```

### Regeneration (Warrior)
```yaml
Name: Second Wind
Description: Regenerate health over time
Type: Heal
Target: Self
Cooldown: 8 seconds
Cast Time: 1.0 seconds
Can Be Interrupted: No
Value: 40
Button Color: #66CC66 (Light Green)
```

## Defensive Abilities

### Mana Shield (Mage)
```yaml
Name: Mana Shield
Description: Absorb damage with magical energy
Type: Defensive
Target: Self
Cooldown: 10 seconds
Cast Time: 0.5 seconds
Can Be Interrupted: No
Value: 20 (defense bonus)
Duration: 5 seconds
Button Color: #0099FF (Blue)
```

### Shield Wall (Warrior)
```yaml
Name: Shield Wall
Description: Raise your shield for massive protection
Type: Defensive
Target: Self
Cooldown: 12 seconds
Cast Time: 0.5 seconds
Can Be Interrupted: No
Value: 30 (defense bonus)
Duration: 6 seconds
Button Color: #0066CC (Dark Blue)
```

### Power Word: Shield (Priest)
```yaml
Name: Power Word: Shield
Description: Shield yourself from harm
Type: Defensive
Target: Self
Cooldown: 10 seconds
Cast Time: 1.0 seconds
Can Be Interrupted: Yes
Value: 25 (defense bonus)
Duration: 5 seconds
Button Color: #9999FF (Light Blue)
```

## Interrupt Abilities

### Counterspell (Mage)
```yaml
Name: Counterspell
Description: Interrupt enemy spellcasting
Type: Interrupt
Target: Enemy
Cooldown: 6 seconds
Cast Time: 0.3 seconds
Can Be Interrupted: No
Value: 0
Button Color: #CC00CC (Purple)
```

### Pummel (Warrior)
```yaml
Name: Pummel
Description: Bash the enemy to interrupt their cast
Type: Interrupt
Target: Enemy
Cooldown: 5 seconds
Cast Time: 0.5 seconds
Can Be Interrupted: No
Value: 5 (bonus damage)
Button Color: #FFFF00 (Yellow)
```

### Silence (Priest)
```yaml
Name: Silence
Description: Silence the enemy's magic
Type: Interrupt
Target: Enemy
Cooldown: 7 seconds
Cast Time: 0.5 seconds
Can Be Interrupted: No
Value: 0
Button Color: #996699 (Dark Purple)
```

---

# Enemy Configurations

## Basic Enemy (Level 1)
```yaml
Class: Warrior (modified)
Base Health: 120
Base Damage: 12
Base Defense: 6
AI Think Time: 3 seconds

Abilities:
  - Basic Attack (Damage, cooldown 2s)
  - Defend (Defensive, cooldown 8s)

Loot:
  Experience Reward: 50
  Gold Reward: 10
```

## Elite Enemy (Boss variant)
```yaml
Class: Warrior (enhanced)
Base Health: 200
Base Damage: 18
Base Defense: 12
AI Think Time: 2 seconds

Abilities:
  - Heavy Strike (Damage, cooldown 3s)
  - Heal (Heal, cooldown 10s)
  - Shield Up (Defensive, cooldown 12s)
  - Interrupt (Interrupt, cooldown 6s)

Loot:
  Experience Reward: 150
  Gold Reward: 50
```

---

# Balancing Guidelines

## Ability Balance
- **Damage abilities**: 20-40 base value, 2-4s cooldown
- **Heal abilities**: 40-60 base value, 5-8s cooldown
- **Defensive abilities**: 15-30 defense bonus, 8-12s cooldown, 4-6s duration
- **Interrupt abilities**: Low/no damage, 5-7s cooldown, 0.3-0.5s cast

## Cast Time Balance
- Fast casts: 0.3-0.8 seconds (interrupts, quick actions)
- Normal casts: 1.0-2.0 seconds (most abilities)
- Slow casts: 2.5-3.5 seconds (powerful effects, should be rare)

## Stat Scaling Balance
- Health factor: 1.08-1.12 (more = tankier)
- Damage factor: 1.04-1.06 (more = higher DPS)
- Defense factor: 1.04-1.06 (more = better survivability)

## Level Scaling
- Enemy level offset: 0-2 for balanced difficulty
- Offset 0: Equal level fights
- Offset 1: Slightly challenging
- Offset 2: Hard mode
- Offset 3+: Very difficult

## Combat Flow
- Average combat length: 30-60 seconds
- Player should use 5-10 abilities per fight
- Player should need to heal 1-2 times per fight
- Interrupts should feel impactful but not mandatory
