# Mobile RPG - Unity 2D

A static combat mobile RPG built with Unity 2D featuring timing/strategy-focused gameplay.

## Overview

This is a **Unity 2D Mobile RPG** with static combat (no movement). The game focuses on timing and strategy with a simple fight-loot-repeat infinite scaling loop.

### Core Features

- **Tap-to-Cast Combat**: Mobile-friendly tap controls for spell casting
- **Interrupts**: Strategic interrupt mechanics to counter enemy abilities
- **Cooldown System**: Abilities have cooldowns for strategic timing
- **Defensive Abilities**: Protect yourself with shields and defensive buffs
- **Infinite Scaling**: Enemy levels scale with player progression
- **Class System**: Three distinct classes (Mage, Warrior, Priest)
- **Stat Scaling**: Stats scale exponentially with level using formula: `Stat = Base * (Level ^ Factor)`

## Project Structure

```
Assets/
├── Scenes/
│   └── MainScene.unity          # Main game scene
├── Scripts/
│   ├── Core/
│   │   └── ScalingStats.cs      # Stat scaling system
│   ├── ScriptableObjects/
│   │   ├── CharacterClass.cs    # Base class for character types
│   │   ├── MageClass.cs         # Mage class definition
│   │   ├── WarriorClass.cs      # Warrior class definition
│   │   ├── PriestClass.cs       # Priest class definition
│   │   └── Ability.cs           # Ability/spell definitions
│   ├── Combat/
│   │   ├── AbilityInstance.cs   # Runtime ability state
│   │   └── CombatManager.cs     # Combat flow controller
│   ├── Entities/
│   │   ├── CombatEntity.cs      # Base combat entity
│   │   ├── PlayerEntity.cs      # Player-specific logic
│   │   └── EnemyEntity.cs       # Enemy AI and behavior
│   └── UI/
│       ├── AbilityButton.cs     # Spell button UI
│       ├── HealthBar.cs         # HP bar display
│       ├── EnemyIntentDisplay.cs # Enemy intent indicator
│       └── CombatUI.cs          # Main UI manager
└── Resources/
    ├── Classes/                 # Character class ScriptableObjects
    └── Abilities/               # Ability ScriptableObjects
```

## Game Systems

### 1. Character Classes (ScriptableObjects)

Three distinct classes with unique attributes:

- **Mage**: High spell power, reduced mana costs
- **Warrior**: Increased physical damage and armor
- **Priest**: Enhanced healing and shield strength

Classes are implemented as ScriptableObjects for easy data-driven design.

### 2. Ability System

Abilities have the following properties:
- **Type**: Damage, Heal, Defensive, or Interrupt
- **Cooldown**: Time before ability can be used again
- **Cast Time**: Duration of casting before effect triggers
- **Interruptible**: Whether enemy can interrupt the cast
- **Value**: Damage/healing/shield amount

### 3. Combat Mechanics

- **Tap to Cast**: Tap ability buttons to cast spells
- **Cooldown Management**: Time abilities strategically
- **Interrupts**: Use interrupt abilities to cancel enemy casts
- **Defensive Play**: Use shields and defensive buffs to survive

### 4. Stat Scaling

Stats scale exponentially using the formula:
```
FinalStat = BaseStat * (Level ^ ScaleFactor)
```

- **Health Scale Factor**: Default 1.1
- **Damage Scale Factor**: Default 1.05
- **Defense Scale Factor**: Default 1.05

### 5. Fight-Loot-Repeat Loop

1. Defeat enemy in combat
2. Gain experience and gold
3. Player levels up when enough XP gained
4. New enemy spawns with scaled difficulty
5. Repeat infinitely

### 6. Enemy AI

Enemies use a simple AI system:
- **Intent System**: Shows upcoming action (Attack, Defend, Heal, Special)
- **Adaptive Behavior**: Changes tactics based on health
- **Automatic Actions**: Executes abilities on a timer

## UI Elements

### Player UI
- **Health Bar**: Shows current/max HP with color gradient
- **Ability Buttons**: Display icons, cooldowns, and cast progress
- **Cooldown Overlays**: Visual fill showing time remaining

### Enemy UI
- **Health Bar**: Enemy HP display
- **Intent Display**: Shows what the enemy will do next (Attack/Defend/Heal/Special)

## Getting Started

### Requirements
- Unity 2022.3.0f1 or later
- Unity 2D packages
- TextMeshPro (included)

### Setup Instructions

1. **Open Project**: Open the project in Unity Hub
2. **Open Main Scene**: Load `Assets/Scenes/MainScene.unity`
3. **Configure Scene**:
   - Add Player GameObject with `PlayerEntity` component
   - Add Enemy GameObject with `EnemyEntity` component
   - Add Canvas with `CombatUI` component
4. **Create Classes**: Right-click in Project → Create → RPG → Classes
5. **Create Abilities**: Right-click in Project → Create → RPG → Ability
6. **Assign References**: Link character classes and abilities in Inspector

### Creating a New Character Class

1. Right-click in Project → Create → RPG → Classes → (Mage/Warrior/Priest)
2. Configure base stats (health, damage, defense)
3. Set scaling factors (how stats grow with level)
4. Create abilities (see below)
5. Assign abilities to class

### Creating a New Ability

1. Right-click in Project → Create → RPG → Ability
2. Set ability name and description
3. Choose type (Damage/Heal/Defensive/Interrupt)
4. Set cooldown and cast time
5. Configure value (damage amount, heal amount, etc.)
6. Choose button color and icon (optional)

## Extending the System

### Adding New Class Types

1. Create new class inheriting from `CharacterClass`
2. Add class-specific properties
3. Create ScriptableObject menu attribute
4. Implement class-specific bonuses in combat

### Adding New Ability Types

1. Extend `AbilityType` enum in `Ability.cs`
2. Handle new type in `CombatManager.OnPlayerAbilityCast` and `OnEnemyAbilityCast`
3. Add visual feedback in UI if needed

### Customizing Stat Scaling

Modify `ScalingStats.cs` to change scaling formulas or add new stats:
```csharp
public float GetScaledStat(int level)
{
    return baseStat * Mathf.Pow(level, scaleFactor);
}
```

## Mobile Optimization

The game is designed for mobile with:
- Simple tap controls
- Large touch-friendly buttons
- Clear visual feedback
- Optimized 2D rendering
- Portrait or landscape orientation support

## Future Enhancements

Potential additions:
- Multiple enemy types
- Equipment system
- Skill trees
- Achievements
- Daily quests
- Leaderboards
- Sound effects and music
- Visual effects for abilities
- Combo system
- Boss fights

## License

This project is provided as-is for educational and commercial use.