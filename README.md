<div align="center">

# GAID

**Game for Attention and Intellectual Development**

A Unity mobile game that trains visual perception, short-term memory, and concentration in children with mild-to-moderate intellectual disabilities, using eye-tracking and gamified exercises.

[![Unity](https://img.shields.io/badge/Unity-2022.3_LTS-000000?logo=unity&logoColor=white)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![Firebase](https://img.shields.io/badge/Firebase-FFCA28?logo=firebase&logoColor=black)](https://firebase.google.com/)
[![SeeSo SDK](https://img.shields.io/badge/SeeSo-Eye_Tracking-4B8BBE)](https://visual.camp/)
[![Android](https://img.shields.io/badge/Android-3DDC84?logo=android&logoColor=white)](#)
[![iOS](https://img.shields.io/badge/iOS-000000?logo=apple&logoColor=white)](#)
[![ICCAS 2024](https://img.shields.io/badge/ICCAS_2024-2nd_Place-FFD700)](#awards)

</div>

---

## Overview

GAID targets children aged 5–12 with mild to moderate intellectual disabilities. Paper-based perception training tends to lose a child's attention before any of it sinks in, so we replaced the worksheet with a touch-driven mobile game that quietly measures gaze, accuracy, and time-on-task while the kid plays.

Built between March and August 2024 as a five-person university project. Won 2nd place at ICCAS 2024 (UK) and shown at EKC 2024.

## Training Modules

Five modules, three difficulty levels each. The taxonomy follows the standard visual-perception categories used in clinical assessments.

| Module | Folder | What It Trains |
| --- | --- | --- |
| Visual-Motor Coordination | `VM/` | Move a gaze-locked character toward a destination |
| Figure-Ground Perception | `FG/` | Find a target hidden inside a cluttered scene |
| Perceptual Constancy | `PC/` | Recognise the same object across scale / rotation changes |
| Position in Space | `SP/` | Read orientation and relative position |
| Spatial Relationships | `SR/` | Identify layouts between multiple objects |

## Metrics

Every session writes the following to a local JSON cache, then syncs to Firestore once the user is signed in.

| Metric | Source | Description |
| --- | --- | --- |
| Play Time | Session timer | Total interaction duration per stage |
| Accuracy | Stage attempts | Correct answers vs. total attempts |
| Concentration | SeeSo gaze tracker | On-target gaze ratio during play |
| Progress Score | Pre-game survey | Weighted score reflecting baseline severity |

## Architecture

```
        ┌─────────────────────────────┐
        │      Unity Client (C#)      │
        │   Scenes · UI · Gameplay    │
        └──────────────┬──────────────┘
                       │
         ┌─────────────┼──────────────┐
         │             │              │
         ▼             ▼              ▼
   ┌──────────┐  ┌──────────┐  ┌────────────┐
   │  SeeSo   │  │ Local DB │  │  Firebase  │
   │  (gaze)  │  │  (JSON)  │  │ Auth · FS  │
   └──────────┘  └──────────┘  │  Storage   │
                               └────────────┘
```

- **Unity (C#)** — gameplay, scene flow, and UI under `Scripts/`
- **Firebase Auth** — email sign-in via `Scripts/DB/SignInManager.cs`, `SignUpManager.cs`
- **Cloud Firestore** — user profile and progress through `FirebaseManager.cs`, `UserDataManager.cs`
- **Cloud Storage** — static art assets fetched by `AssetCheck.cs`
- **SeeSo SDK** — gaze calibration and live tracking in `TrackingManager.cs`, `CalibrationHandler.cs`
- **Local cache** — offline-safe JSON store in `LocalDataManager.cs`

## Repository Layout

```
GAID/
├── Scenes/              Unity scenes (auth, level select, survey, per-module)
├── Scripts/
│   ├── DB/              Firebase, auth, gaze tracking, local cache
│   ├── Common/          Shared UI + graph plotting
│   ├── FG/ PC/ SP/ SR/  Per-module gameplay logic
│   └── VisualMotor/     Gaze-driven movement
├── Prefabs/             Reusable UI prefabs (charts, indices, points)
├── Images/              Backgrounds and per-module sprites
├── FG/ PC/ SP/ SR/ VM/  Per-level scenes
└── GAID_poster.png      Conference poster
```

## Getting Started

**Requirements**

- Unity 2022.3 LTS (with Android and/or iOS build modules)
- A Firebase project with Authentication, Firestore, and Storage enabled
- A SeeSo SDK license key from [visual.camp](https://visual.camp/)

**Setup**

1. Clone the repository and open it from Unity Hub.
2. Drop your Firebase config into the project:
   - Android → `google-services.json`
   - iOS → `GoogleService-Info.plist`
3. Paste your SeeSo license key into the tracking configuration object referenced by `TrackingManager.cs`.
4. Open `Scenes/SignInScene.unity` and press Play, or build directly to a device.

## Awards

- **ICCAS 2024** (UK) — 2nd Place
- **EKC 2024** (UK) — Selected exhibition

## Team

| Member | Affiliation |
| --- | --- |
| Seonwoong Hwang | Pusan National University |
| Yui Cho | Inha University |
| Sojeong Kim | Kyungwoon University |
| Donggyu Na | Chungbuk National University |
| Mihye Kim *(Advisor)* | Chungbuk National University |

## Roadmap

- More modules and a finer difficulty curve
- Empirical validation with partner clinics
- Broader age range and support for adjacent cognitive profiles

---

## Poster

<p align="center">
  <img src="./GAID_poster.png" alt="GAID conference poster — ICCAS 2024" width="760">
</p>
