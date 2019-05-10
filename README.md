# Assets
## Mobile version
- AR Foundation  - [Unity License](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@1.0/license/LICENSE.html)
- ARKit - ARKit XR Plugin copyright © 2018 Unity Technologies ApS
Licensed under the Unity Companion License for Unity-dependent projects--see [Unity Companion License.](https://unity3d.com/legal/licenses/Unity_Companion_License?_ga=2.85255321.1440730770.1554121145-1667441578.1544005822)
Unless expressly provided otherwise, the Software under this license is made available strictly on an “AS IS” BASIS WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED. Please review the license for details on these and other terms and conditions.
- ARCore  -
 Component Name: Google ARCore SDK 1.5
 All rights reserved. Use of the Google ARCore SDK for Unity requires agreeing to and complying with the Google APIs Terms of Service accessed via  "https://developers.google.com/terms/" (Please be aware that you may need to register with Google to access certain APIs). If you cannot comply with Google APIs Terms of Service, do not use this SDK.
- TextMeshPro  - Unity License
- Vector Graphics  - Unity License

## HoloLens version
- Windows Mixed Reality - Unity License
- Mixed Reality Toolkit Unity - MIT License

#  Project setup
##  Requirements
- Unity 2018.3.6f1 or higher.
- Xcode Version 10.1 or higher (if the app needs to be built for iOS).
- Android SDK (if the app needs to be built for Android).
- HoloLens [Read this](https://docs.microsoft.com/en-us/windows/mixed-reality/install-the-tools/), Windows 10 computer with special software necessary

## Source Code
- master branch - mobile project version
- release/HoloLens branch  - Hololens project version

#  What is different between Hololens- and iPad-project-versions?
Mobile project-version (the iPad one) and HoloLens project are two different Unity-projects because MR (Mixed Reality from Hololens) does not support any third-party files and scripts, but only native. This counts even if scripts are only stored in the project and not used in the current scene.  
The scripts for Earth-rotating, satellites, camera movement, steps of satellites-displaying are common for both projects and do not rely on AR / MR technology

#  Troubleshootings (solved)

## Development

Issue: MR does not support scripts from ARKit/ARCore  
Solution: create two different Unity-projects for Hololens and mobile-version (iPad)

Issue: Hololens-app can’t installed on the device  
Solution: it is necessary to remember: the version of Hololens-firmware core and Visual Studio version core need to be compatible, so developer has to up- or downgrade the Visual Studio version on the build-computer 

## iOS
Issue: the Earth is not staying at the set place and is moving with iPad-movements  
Solution: that’s ARKit problem with glance surfaces, so another surface has to be used or user has to move around the chosen point to reach better ARKit Tracking

## HoloLens
Issue: very bad performance, slow movements, freezing  
Solution: optimizations of every app-element
- satellite-models have to be low-poly
- bake similar objects in a bigger one - satelliteы, trajectories (with MashBaker)
- optimize all scripts for positions-counting and lines of sight
- bake the lights, turn off real-time light

Issue: crash while using the app freezes the whole system and no other app could be started - „sleep-icon“ is displayed for a while  
Solution: that’s known and common issue about performance - solution see above - performance-optimization

Issue: sent satellites-models are too high-poly and aren’t suitable for AR-applications  
Solution: make it low-poly or use other schematic 3D models 

Issue: 3-5 seconds image-delay while streaming from Hololens to "Device portal" site - too big latency times for live-presentations  
Solution: stream via „Hololens App“ from MS Store - the delay is smaller (0,5-1 sec)


#  Development
Developed for [Bundeswehr University Munich](https://www.unibw.de/home-en) for Augmented Reality (AR) Demonstration during [Munich Satellite Navigation Summit](https://www.munich-satellite-navigation-summit.org/) by [App-Agentur](https://www.app-agentur.com/) of [Lindenvalley GmbH](https://www.lindenvalley.de/)
