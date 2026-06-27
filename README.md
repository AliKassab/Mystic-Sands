# Mystic Sands

A 3D narrative adventure built in **Unity 6**, set in a desert world where the player explores an open environment, talks to characters through a branching dialogue system, and survives a timed, procedurally generated maze. The project is fully **bilingual (Arabic + English)** with proper right‑to‑left text shaping.


---

## Highlights

- **Procedural maze generation** — randomized mazes built with a recursive‑backtracker algorithm, rendered into Unity geometry at runtime, wrapped in a timed escape loop with progressively shorter rounds.
- **Branching dialogue** — conversation trees with trigger zones, resumable state, and input handling, integrated through a node‑based dialogue editor.
- **Full Arabic localization** — custom RTL text shaping for UI, TextMeshPro, and in‑world 3D text, with runtime language switching.
- **Character & camera controllers** — Rigidbody‑based third‑person movement with smooth orientation, plus a clamped mouse‑look camera.
- **Interactive systems** — drag‑and‑drop inventory and object placement, a letter‑based word puzzle, and a live minimap.
- **Clean menu architecture** — pause, settings, instructions, and main‑menu flows implemented with the **Command pattern** (`ICommand`) for decoupled, extensible UI actions.
- **Cloud persistence** — player data saved via Unity Gaming Services Cloud Save.

---

## Tech Stack

| Area | Tools |
|------|-------|
| Engine | Unity `6000.3.9f1` (Unity 6) |
| Language | C# |
| Rendering | Built‑in Render Pipeline + Post‑Processing |
| AI / Navigation | Unity AI Navigation (NavMesh) |
| Camera | Cinemachine |
| Backend | Unity Gaming Services — Cloud Save |
| Other | TextMeshPro, Timeline, custom Arabic text shaping, GLTF import |

---

## Architecture Highlights

- **Command pattern** decouples UI intent from execution — each menu action (`StartGameCommand`, `PauseCommand`, `SettingsCommand`, …) implements a shared `ICommand` interface, so new actions drop in without touching menu wiring.
- **Algorithm‑driven content** — the maze is generated independently of its presentation: `MazeGenerator` produces a wall‑state grid, and `MazeRenderer` translates it into scene geometry, keeping generation testable and reusable.
- **Localization as a layer** — Arabic shaping is applied through dedicated fixers (UI / TMP / 3D) rather than baked into content, so the same scenes run in either language.

```
Assets/_Scripts/
├── Gameplay/
│   ├── Camera/        # mouse-look + camera positioning
│   └── Mechanics/     # maze generation/rendering, movement, game loop
├── UiScripts/
│   ├── UI Options/    # Command-pattern menu actions
│   └── Misc_/         # minimap, persistent elements, hover/light effects
└── Arabic/            # RTL text-shaping support
```

---
---

## Gameplay


- Explore the open desert world and interact with characters.
- Solve puzzles and manage items through drag‑and‑drop placement.
- Enter the maze and escape before the timer runs out — each round gets tighter.

---
