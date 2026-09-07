# Release notes

## v2.4.1

### Fixed
- PanForcer: the Mobile output of its platform switch was left unconnected when the port was added in 2.4.0, so the PanOnSetup auto-pan never ran on Mobile. Mobile now enters the same branch as WebGL. Before 2.4.0 a mobile Android build fell into the old UNITY_ANDROID fallback and destroyed the PanForcer at scene setup, which killed its ForcePan/ForceDepan entry points too, while iOS took the WebGL branch — Mobile now behaves the same on both. PanManager needs no equivalent change: its switch wires only the VR output, to Destroy, so an unconnected Mobile output already means "keep the pan manager", exactly like WebGL

## v2.4.0

### Added
- Added Mobile control output to CheckPlatformUnit ("Reflectis Platform: Switch"), taken when the experience runs on the Mobile platform
- Added "Creator Kit update routines/v2026.4.x -> v2026.5.0" editor window: scans every graph asset, prefab and scene under Assets/ for CheckPlatformUnit occurrences, lists them with per-row selection and focus (opens the graph on the node), and connects the Mobile output to the same destination as the WebGL one for all selected nodes. Nodes whose Mobile port was already connected (same or different destination) are reported as clean and not preselected

### Fixed
- PanAndHideEverything and ExitPanAndShowEverything now gate on the runtime platform: WebGL and Mobile run the existing hide/show chain, VR goes straight to the graph output. In VR the pan itself was already a no-op (CharacterControllerSystem.GoToInteractState/GoToSetMovementState are only overridden by the flat-screen controller), but the three hide nodes ran anyway, so selecting a chatbot or a quiz hid every avatar. Consumers that route VR around the macros themselves are unaffected — this covers the ones that do not, creator-authored graphs in particular
- CheckPlatformUnit: the fallback taken when the switch resolves no platform now mirrors PlatformSystem.Init and reads the build profile's REFLECTIS_* defines. It previously keyed off UNITY_ANDROID and returned the VR output, which is also the mobile player's define

## v2.3.0

### Added
- Added AddPickableToInventoryNode to add items to the inventories
- Added SetAlphaInventoryNode to change alpha for the inventory canvas
- Added SpawnSpawnableObjectNode to spawn object locally and via network, both prefabs and scene objects
- Added SpawnFeedbackCheckNode to spawn a feedback check in webGL
- Added TaskUIShowIntroNode to show the introduction for the taskSystem

## v2.2.0

### Added
- Added SetCameraModeNode to handle camera behaviour
- Added control manager end node
- Added on control manager end node
- Added EndInformativeItem node
- Added StartInformativeItem node
- Added OnControlTaskStart node
- Added HoverInformativeItem node
- Added GetStringFromKey node for localization purposes
- Added get player head node
- Added get player hand node
- Added change language node
- Added field to quizInstanceNode to show question label
- Added animated screen canvas to show hints with control manager
- Added cinemachine

### Fixed
- Fixed reload scene node

## v2.1.1

### Fixed

- Fixed change scene node priority

### Removed

- Removed tenant bool on change scene node

## v2.1.1

### Fixed

- Fixed missing references in `ChatBotSelectUnit` and `ChatBotDeselectUnit`.

## v2.1.0

### Added

- Added save data units and improved create custom GameObject.
- Added node to check if a scene is available to change.
- Added `GetLocalizationData` node and descriptor.
- Added language change event unit.
- Added languageChanged descriptor and changed its namespace.
- Added create leaderboard record unit.
- Added force release node for manipulables.
- Added `OnTutorialCloseEvent` unit.
- Added flag to select tenant envs in change scene node.

### Fixed

- Fixed load default env node.

## v2.0.0

### Changed

- Revised nodes to match the new structure experiences/sessions.

### Added

- Added `GetCurrentNetworkTime` node to get network time.

## v1.2.0

### Added

- Added new visual scripting nodes to activate fade from black and fade to black routines.

### Fixed

- Fixed shard closing logic.

## v1.1.0

### Added

- Added implementation of `VisualScriptingInteractable` to make it accessible from visual scripting nodes.

### Fixed

- Fixed missing items in collection of visual scripting custom types, fixed retrieval of custom types.
- Fixed occurrences of `GenericInteractable` in tooltips into `VisualScriptingInteractable`.
- Fixed `OnSelectedVisualScriptingInteractableChange` event unit substituting
  the occurrences of `IVisualScriptingInteractable` interface into `VisualScriptingInteractable` class.
- Fix unselection of a destroyed visual scripting interactable in `VisualScriptingInteractable`.

## v1.0.0

- Initial release.
