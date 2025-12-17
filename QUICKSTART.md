# Unity 2D Mobile RPG - Quick Start Guide

## Quick Setup (3 Steps)

### Step 1: Create Character Classes

1. In Unity Project window, navigate to `Assets/Resources/Classes`
2. Right-click → Create → RPG → Classes → Mage
3. Name it "MageClass" and configure:
   - Base Health: 100
   - Base Damage: 15
   - Base Defense: 5
   - Health Scale Factor: 1.1
   - Spell Power Multiplier: 1.5

4. Right-click → Create → RPG → Classes → Warrior  
5. Name it "WarriorClass" and configure:
   - Base Health: 150
   - Base Damage: 20
   - Base Defense: 10
   - Physical Damage Bonus: 1.3
   - Armor Multiplier: 1.5

6. Right-click → Create → RPG → Classes → Priest
7. Name it "PriestClass" and configure:
   - Base Health: 120
   - Base Damage: 10
   - Base Defense: 7
   - Healing Power Multiplier: 1.4

### Step 2: Create Abilities

In `Assets/Resources/Abilities`, create these abilities:

**Fireball** (Damage ability)
- Right-click → Create → RPG → Ability
- Name: "Fireball"
- Type: Damage
- Target: Enemy
- Cooldown: 3
- Cast Time: 1.5
- Value: 30
- Can Be Interrupted: Yes
- Button Color: Orange/Red

**Heal** (Heal ability)
- Name: "Heal"
- Type: Heal
- Target: Self
- Cooldown: 5
- Cast Time: 2
- Value: 40
- Can Be Interrupted: Yes
- Button Color: Green

**Shield** (Defensive ability)
- Name: "Shield"
- Type: Defensive
- Target: Self
- Cooldown: 8
- Cast Time: 1
- Value: 15 (defense bonus)
- Duration: 5
- Can Be Interrupted: No
- Button Color: Blue

**Interrupt** (Interrupt ability)
- Name: "Kick"
- Type: Interrupt
- Target: Enemy
- Cooldown: 6
- Cast Time: 0.5
- Value: 0
- Can Be Interrupted: No
- Button Color: Yellow

### Step 3: Set Up the Scene

1. Open `Assets/Scenes/MainScene.unity`
2. Create a new GameObject named "GameSetup"
3. Add the `GameSetup` component to it
4. Assign:
   - Player Class: MageClass (or your choice)
   - Player Start Level: 1
   - Enemy Class: WarriorClass (or your choice)
   - Enemy Level Offset: 0
   - Auto Create Entities: ✓

5. Assign abilities to your character classes:
   - Select MageClass asset
   - Set Abilities array size to 4
   - Drag: Fireball, Heal, Shield, Interrupt

6. Press Play!

## Creating UI (Optional)

For a complete UI:

1. Create Canvas (Right-click Hierarchy → UI → Canvas)
2. Set Canvas Scaler to "Scale With Screen Size" (Reference: 1920x1080)
3. Add these UI elements:

**Player Health Bar:**
- Create Panel (UI → Panel)
- Add Image for fill (set fill method to Horizontal)
- Add TextMeshPro for HP text
- Add `HealthBar` component

**Enemy Health Bar:**
- Same as above

**Ability Buttons:**
- Create Button (UI → Button - TextMeshPro)
- Add Images for: Icon, Cooldown Overlay, Cast Bar
- Add `AbilityButton` component
- Save as Prefab

**Enemy Intent:**
- Create Panel
- Add Image for intent icon
- Add TextMeshPro for text
- Add `EnemyIntentDisplay` component

4. Add `CombatUI` component to Canvas
5. Assign all references in inspector

## Testing

Play the scene and you should see:
- Player and Enemy automatically created
- Debug logs showing stats
- Abilities ready to cast (if UI is set up)
- Enemy AI acting automatically

## Troubleshooting

**"NullReferenceException" errors:**
- Make sure character classes are assigned
- Ensure abilities array is not empty
- Check all UI references are linked

**Abilities not working:**
- Verify abilities are assigned to character class
- Check cooldown/cast time values
- Ensure CombatManager exists in scene

**No damage happening:**
- Check ability values are > 0
- Verify CombatManager is set up correctly
- Ensure player/enemy references are assigned

## Next Steps

1. Create more abilities with different effects
2. Design custom UI layout
3. Add visual effects (particles, animations)
4. Implement sound effects
5. Create multiple enemy types
6. Add loot and rewards system
7. Build for mobile device

## Tips

- Keep ability cooldowns balanced (3-10 seconds)
- Make cast times shorter for better feel (0.5-2 seconds)
- Scale enemy levels slowly (offset 0-2)
- Use color coding for ability types
- Test on actual mobile device for touch responsiveness
