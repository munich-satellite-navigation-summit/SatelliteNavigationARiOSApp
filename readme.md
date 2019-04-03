# Assets:
## Mobile version:
- AR Foundation  - Unity License (https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@1.0/license/LICENSE.html)
- ARKit - ARKit XR Plugin copyright © 2018 Unity Technologies ApS
Licensed under the Unity Companion License for Unity-dependent projects--see Unity Companion License.  https://unity3d.com/legal/licenses/Unity_Companion_License?_ga=2.85255321.1440730770.1554121145-1667441578.1544005822
Unless expressly provided otherwise, the Software under this license is made available strictly on an “AS IS” BASIS WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED. Please review the license for details on these and other terms and conditions.
- ARCore  -
 Component Name: Google ARCore SDK 1.5
 All rights reserved. Use of the Google ARCore SDK for Unity requires agreeing to and complying with the Google APIs Terms of Service accessed via:  "https://developers.google.com/terms/" (Please be aware that you may need to register with Google to access certain APIs). If you cannot comply with Google APIs Terms of Service, do not use this SDK.
- TextMeshPro  - Unity License
- Vector Graphics  - Unity License

## HoloLens version:
- Windows Mixed Reality - Unity License
- Mixed Reality Toolkit Unity - MIT License

#  Project setup:
# #  Requirements
    - Unity 2018.3.6f1 or higher.
    - Xcode Version 10.1 or higher (when developing for iOS).
    - Android SDK (when developing for Android).
    - HoloLens [Read this](https://docs.microsoft.com/en-us/windows/mixed-reality/install-the-tools)

#  What is different?
Mobile version project and HoloLens project are two different projects because MR (Mixed Reality) does not support any files, scripts, even if they are not used in the current scene/project.
We use the same scripts to rotating the Earth, satellites, camera movement, steps of showing satellites, everything that does not depend on AR / MR technology
