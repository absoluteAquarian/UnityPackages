# v1.0.2
- Fixed a bug that caused MappedInputDatabase to throw errors in the Console while in in the Editor

# v1.0.1
- Removed the dependency of using GameObjects for polling and using inputs from InputMapper
- Added new MappedInputDatabase scriptable object for use in registering controls for InputMapper
  - MappedInputDatabase instances do not have to be placed in a scene, but doing so is still supported