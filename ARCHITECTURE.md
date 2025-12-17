# Unity 2D Mobile RPG - Architecture Documentation

## System Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│                        Game Layer                            │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │  GameSetup   │  │ CombatManager│  │   CombatUI   │      │
│  │  (Helper)    │  │  (Control)   │  │  (Display)   │      │
│  └──────┬───────┘  └──────┬───────┘  └──────┬───────┘      │
└─────────┼──────────────────┼──────────────────┼─────────────┘
          │                  │                  │
          ▼                  ▼                  ▼
┌─────────────────────────────────────────────────────────────┐
│                      Entity Layer                            │
│  ┌──────────────┐  ┌──────────────┐                         │
│  │PlayerEntity  │  │ EnemyEntity  │                         │
│  │  (Player)    │  │   (AI)       │                         │
│  └──────┬───────┘  └──────┬───────┘                         │
│         │                 │                                  │
│         └────────┬────────┘                                  │
│                  ▼                                           │
│         ┌──────────────┐                                     │
│         │CombatEntity  │                                     │
│         │   (Base)     │                                     │
│         └──────┬───────┘                                     │
└────────────────┼─────────────────────────────────────────────┘
                 │
                 ▼
┌─────────────────────────────────────────────────────────────┐
│                    Data Layer (ScriptableObjects)            │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │CharacterClass│  │   Ability    │  │ScalingStats  │      │
│  │   (Base)     │  │   (Data)     │  │   (Data)     │      │
│  └──────┬───────┘  └──────────────┘  └──────────────┘      │
│         │                                                    │
│    ┌────┴────┬────────┐                                     │
│    ▼         ▼        ▼                                     │
│  ┌────┐  ┌────┐  ┌────┐                                    │
│  │Mage│  │Warr│  │Prie│                                    │
│  └────┘  └────┘  └────┘                                    │
└─────────────────────────────────────────────────────────────┘
```

## Core Components

### 1. Data Layer (ScriptableObjects)

**CharacterClass Family**
- `CharacterClass.cs` - Abstract base class for all character types
- `MageClass.cs` - Mage specialization (spell power focus)
- `WarriorClass.cs` - Warrior specialization (physical combat)
- `PriestClass.cs` - Priest specialization (healing/support)

**Ability System**
- `Ability.cs` - Defines spell/ability data (cooldown, damage, type)

**Stats**
- `ScalingStats.cs` - Exponential stat scaling system

### 2. Entity Layer (MonoBehaviours)

**Base Entity**
- `CombatEntity.cs` - Base class for all combat participants
  - Health management
  - Ability instance management
  - Damage/heal processing
  - Event system for combat actions

**Specialized Entities**
- `PlayerEntity.cs` - Player-specific behavior
  - Experience and leveling
  - Player input handling
  
- `EnemyEntity.cs` - Enemy-specific behavior
  - AI decision making
  - Intent system
  - Loot rewards

### 3. Combat System

**Combat Manager**
- `CombatManager.cs` - Central combat controller
  - Ability execution
  - Damage calculation
  - Fight-loot-repeat loop
  - Enemy spawning

**Ability Runtime**
- `AbilityInstance.cs` - Runtime state of abilities
  - Cooldown tracking
  - Cast progress
  - Interrupt handling

### 4. UI Layer

**Display Components**
- `CombatUI.cs` - Main UI coordinator
- `HealthBar.cs` - HP visualization
- `AbilityButton.cs` - Spell button with cooldown display
- `EnemyIntentDisplay.cs` - Enemy action telegraph

### 5. Core Systems

**Game Setup**
- `GameSetup.cs` - Quick scene initialization helper

## Data Flow

### Ability Cast Flow
```
1. Player taps AbilityButton
   ↓
2. AbilityButton calls CombatUI.OnAbilityButtonClicked()
   ↓
3. CombatUI calls PlayerEntity.CastAbility(index)
   ↓
4. PlayerEntity creates AbilityInstance and starts cast
   ↓
5. AbilityInstance updates cast timer
   ↓
6. When complete, AbilityInstance fires OnCastComplete event
   ↓
7. CombatEntity relays to CombatManager via OnAbilityCast event
   ↓
8. CombatManager applies ability effect to target
   ↓
9. Target entity updates health/stats
   ↓
10. UI updates via OnHealthChanged event
```

### Combat Loop
```
1. Player defeats Enemy
   ↓
2. EnemyEntity fires OnDeath event
   ↓
3. CombatManager receives event
   ↓
4. Player gains experience and gold
   ↓
5. Check for level up
   ↓
6. After delay, spawn new enemy
   ↓
7. Scale enemy level to player
   ↓
8. Reset combat state
   ↓
9. Loop continues...
```

## Design Patterns Used

### 1. Observer Pattern
- Events for health changes, ability casts, death
- Loose coupling between systems
- Easy to extend with new listeners

### 2. ScriptableObject Pattern
- Data-driven design
- Easy to modify without code changes
- Reusable ability and class definitions

### 3. Component Pattern
- MonoBehaviour components for game objects
- Composition over inheritance
- Flexible entity configuration

### 4. Singleton Pattern (Light)
- CombatManager uses simple singleton
- Single source of truth for combat state

## Scaling Formula

The core scaling formula is:
```
FinalStat = BaseStat × (Level ^ ScaleFactor)
```

**Example:**
- Base Health: 100
- Scale Factor: 1.1
- Level 1: 100 × (1 ^ 1.1) = 100
- Level 5: 100 × (5 ^ 1.1) = 100 × 6.2 = 620
- Level 10: 100 × (10 ^ 1.1) = 100 × 12.6 = 1,260
- Level 20: 100 × (20 ^ 1.1) = 100 × 29.0 = 2,900

This provides exponential growth that scales infinitely.

## Event System

### CombatEntity Events
```csharp
OnHealthChanged(CombatEntity entity, float newHealth)
OnDeath(CombatEntity entity)
OnAbilityCast(CombatEntity caster, AbilityInstance ability)
```

### AbilityInstance Events
```csharp
OnCooldownComplete(AbilityInstance instance)
OnCastComplete(AbilityInstance instance)
OnCastInterrupted(AbilityInstance instance)
```

## Mobile Optimization Considerations

1. **Touch Input**: Large, clear buttons for mobile screens
2. **Performance**: Minimal draw calls with 2D sprites
3. **UI Scaling**: Canvas scaler for multiple resolutions
4. **Cooldown Feedback**: Clear visual indicators
5. **Simple Controls**: Tap-only interaction

## Extension Points

### Adding New Features

**New Character Class:**
```csharp
[CreateAssetMenu(fileName = "Rogue", menuName = "RPG/Classes/Rogue")]
public class RogueClass : CharacterClass
{
    public float criticalChance = 0.25f;
    public float dodgeChance = 0.15f;
}
```

**New Ability Type:**
```csharp
public enum AbilityType
{
    Damage,
    Heal,
    Defensive,
    Interrupt,
    Buff,      // NEW
    Debuff     // NEW
}
```

**New Stats:**
```csharp
public class ScalingStats
{
    // Existing stats...
    
    public float baseCritRate = 5f;
    public float critRateScaleFactor = 1.02f;
    
    public float GetScaledCritRate(int level)
    {
        return baseCritRate * Mathf.Pow(level, critRateScaleFactor);
    }
}
```

## Testing Strategy

### Unit Testing (Recommended)
- Test stat scaling calculations
- Test ability cooldown logic
- Test damage calculations
- Test level-up experience requirements

### Integration Testing
- Test combat flow from button press to damage
- Test enemy AI decision making
- Test fight-loot-repeat loop
- Test UI updates on state changes

### Playtest Checklist
- [ ] Abilities cast correctly
- [ ] Cooldowns display and function properly
- [ ] Health bars update accurately
- [ ] Enemy AI makes reasonable decisions
- [ ] Experience and leveling works
- [ ] Stats scale appropriately
- [ ] Combat feels responsive
- [ ] UI is clear and readable

## Performance Considerations

### Memory
- ScriptableObjects are assets (loaded once)
- Ability instances are lightweight structs
- Minimal per-frame allocations

### CPU
- Simple AI (timer-based, not every frame)
- Event-driven updates (not polling)
- Single combat scene (no world loading)

### Build Size
- No external dependencies
- Small code footprint
- Asset-based configuration

## Future Architecture Improvements

1. **State Machine**: For complex combat states
2. **Object Pooling**: For visual effects and damage numbers
3. **Save System**: Player progress persistence
4. **Network Layer**: For leaderboards/multiplayer
5. **Audio Manager**: Centralized sound management
6. **Analytics**: Track player behavior and balance

## Conclusion

This architecture provides:
- ✅ Clean separation of concerns
- ✅ Easy to extend and modify
- ✅ Data-driven design
- ✅ Mobile-optimized
- ✅ Scalable for infinite progression
- ✅ Event-driven for loose coupling
