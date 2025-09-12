# Release notes

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
