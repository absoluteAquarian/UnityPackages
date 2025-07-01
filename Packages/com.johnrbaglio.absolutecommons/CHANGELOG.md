# v1.0.8/.9/.10
- Attempted to fix InterfaceDrawer

# v1.0.7
- Added InterfaceDrawer

# v1.0.6
- Fixed missing meta files issue

# v1.0.5
- Added ReadOnlyIfTrueAttribute, Optional

# v1.0.4
- Properly split editor and runtime code into different assemblies

# v1.0.3
- Updated the backend to support the Unity.InputSystem package, should it be active

# v1.0.2
- Fixed a bug that caused MappedInputDatabase to throw errors in the Console while in in the Editor

# v1.0.1
- Removed the dependency of using GameObjects for polling and using inputs from InputMapper
- Added new MappedInputDatabase scriptable object for use in registering controls for InputMapper
  - MappedInputDatabase instances do not have to be placed in a scene, but doing so is still supported