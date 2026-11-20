# Reactor Control Room Simulator (Proyecto Integrador)

Interactive nuclear power plant control-room simulator built in Unity for my integrative project at UDLA (Software Engineering). The operator has 30 seconds to take a running reactor to a safe shutdown by following the correct procedure while the plant keeps reacting in real time.

## How it works

- **Real-time plant model** (`ReactorSystem.cs`): water level, water temperature and electric output (MWe) evolve every frame. Low water heats the core faster; output follows temperature until the turbines stop. States: `Running → Cooling → Shutdown`, or `EmergencyStopped`.
- **Ordered shutdown procedure** (`ShutdownSequence.cs`): stop turbines → vent → exchange water → reactor off, plus an emergency protocol. Each step is validated against a 30-second timer: actions done out of order are rejected, counted as errors and explained in the operator log.
- **Hold-to-confirm controls** (`HoldToConfirm.cs`): critical buttons must be held for a moment before they fire, a common HCI safeguard against accidental clicks.
- **Live UI binding** (`UIBinder.cs`): gauges and an operator log stay in sync with the simulation.

## Stack

Unity 2022.3 LTS · C# · uGUI · TextMesh Pro

## Open the project

1. Install **Unity 2022.3.62f3** (or any 2022.3 LTS) from Unity Hub.
2. *Add project from disk* and select this folder. Unity rebuilds `Library/` on first open.
3. Open `Assets/Main.unity` and press Play.

## Author

**José Ignacio Pumagualli** · [GitHub](https://github.com/JoseIgnacio-Puma08) · pumaguallipro@gmail.com
