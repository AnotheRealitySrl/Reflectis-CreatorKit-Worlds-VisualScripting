# Release notes

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