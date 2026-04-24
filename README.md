# 2D Unity Prototype (Unity 6) — System Overview

This project is a small-scale 2D RPG-style prototype inspired by classic Pokémon games. It demonstrates a clean, modular architecture using Unity’s component-based design, ScriptableObjects, and an event-driven system.

Below is a breakdown of all major systems and scripts implemented in the project.

---

# Player System

## **PlayerController2D**

Handles player movement using Rigidbody2D.

### Features:

* 8-directional movement (WASD input)
* Normalized diagonal movement
* Uses `rb.linearVelocity` (Unity 6 compliant)
* Stores last movement direction for animation

### Responsibilities:

* Reads input (`Input.GetAxisRaw`)
* Applies movement in `FixedUpdate`
* Provides direction data for animation

---

## **Player Animation System**

Uses an Animator with a **2D Freeform Directional Blend Tree**.

### Parameters:

* `moveX` (float)
* `moveY` (float)
* `isMoving` (bool)

### Features:

* Smooth blending across 8 directions
* Sprite flipping for right-side movement
* Last direction tracking for idle

---

# Enemy System

## **EnemyData (ScriptableObject)**

Defines configurable enemy parameters.

### Fields:

* `moveSpeed`
* `chaseMultiplier`

### Purpose:

* Decouples data from behavior
* Allows easy tuning and reuse across enemies

---

## **EnemyBehaviour**

Controls enemy AI using a simple state machine.

### States:

* `Patrol`
* `Chase`
* `ReturnToPatrol`
* `GameOver`

### Features:

* Patrols between predefined points
* Detects player via external trigger system
* Chases player at increased speed
* Returns to nearest patrol point after disengaging
* Stops movement on Game Over

### Movement:

* Uses Rigidbody2D (`linearVelocity`)
* Direction-based movement (no NavMesh)

---

## **EnemyTriggerArea**

Handles player detection using a trigger collider.

### Features:

* Fires events when player enters/exits area
* Decoupled from enemy logic

### Events:

* `OnPlayerEnter`
* `OnPlayerExit`

Enemies subscribe to these events to switch states.

---

# Collectible System

## **Collectible**

Attached to collectible objects.

### Features:

* Detects player via `OnTriggerEnter2D`
* Fires event when collected
* Disables itself (object pooling compatible)

### Event:

* `GameEvents.TriggerCollectiblePicked(int value)`

---

## **CollectibleSpawner**

Manages spawning using object pooling.

### Features:

* Pre-instantiated object pool
* Maintains constant number of active collectibles
* Random spawn within defined rectangular area
* Prevents overlap using distance checks
* Uses Gizmos for visual debugging

### Behavior:

* Spawns initial collectibles at start
* Listens to collectible events
* Respawns new collectibles when one is picked

---

# Event System

## **GameEvents (Static Class)**

Centralized event hub used across systems.

### Events:

* `OnGameOver`
* `OnCollectiblePicked`

### Purpose:

* Decouples systems
* Enables communication between gameplay, UI, and audio

---

# UI System

## **UIManager**

Handles all UI-related functionality.

### Features:

* Controls Game Over panel using CanvasGroup
* Updates score UI
* Handles scene restart
* Pauses game on Game Over

### CanvasGroup Usage:

* Uses `alpha`, `interactable`, and `blocksRaycasts`
* Avoids enabling/disabling GameObjects

---

# Audio System

## **AudioManager**

Centralized audio controller using a simple pooled system.

### Features:

* Singleton pattern
* Separate handling for:

  * Background music
  * Sound effects (SFX)
* SFX pooling to prevent audio cutoff
* Pitch randomization for variation
* Per-sound and global volume control

### Event Integration:

* Subscribes to:

  * `OnGameOver`
  * `OnCollectiblePicked`

### Behavior:

* Stops music on Game Over
* Plays appropriate sound effects
* Supports UI button sounds

---

# 📷 Camera System

## **CameraShake**

Adds screen shake effect on Game Over.

### Features:

* Event-driven (`OnGameOver`)
* Uses random positional offsets
* Uses `Time.unscaledDeltaTime` to work during pause

---

# ⏱️ Game State Control

## Time Management

### Behavior:

* `Time.timeScale = 0` on Game Over
* `Time.timeScale = 1` on restart

### Effects:

* Pauses gameplay systems
* Keeps UI and audio functional
* Enhances game feel during failure state

---

# Architecture Highlights

### ✔ Event-Driven Design

* Systems communicate via `GameEvents`
* Reduces tight coupling

### ✔ ScriptableObject Usage

* Clean separation of data and logic

### ✔ Object Pooling

* Efficient collectible management
* Avoids runtime instantiation overhead

### ✔ Modular Systems

* Each system handles a single responsibility
* Easy to extend and maintain

---

# In Short :

This project demonstrates a complete gameplay loop with:

* Player movement and animation
* Enemy AI with state machine
* Collectible spawning and scoring
* UI management with CanvasGroup
* Audio feedback system
* Camera effects
* Global game state control

The architecture prioritizes **simplicity, scalability, and clean separation of concerns**, making it a solid foundation for expanding into a full Game system in future.

---


