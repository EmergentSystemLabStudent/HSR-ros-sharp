# HSR-ros-sharp

The HSR-ros-sharp project bridges the Toyota Human Support Robot (HSR) with the Microsoft HoloLens, through [Rosbridge](http://wiki.ros.org/rosbridge_suite) and [ROS#](https://github.com/siemens/ros-sharp), to visualize the Emergent Reality (ER).

**Content:**
*   [Project Description](#project-description)
    *   [Fork History](#fork-history)
    *   [Contribution Policy](#contribution-policy)
*   [Project Documentation](#project-documentation)
    *   [Original Documentation](#original-documentation)
    *   [Getting Started](#getting-started)
        *   [System Requirements](#system-requirements)
        *   [Unity Configuration](#unity-configuration)
        *   [Deployment Procedure](#deployment-procedure)
    *   [Custom Visualizations](#custom-visualizations)
        *   [Spatial Concepts](#spatial-concepts)
        *   [Navigation Goal](#navigation-goal)
        *   [Path Plan](#path-plan)
        *   [Reference Frame](#reference-frame)
    *   [Setup History](#setup-history)
        *   [Original Requirements](#original-requirements)
        *   [Original Configuration](#original-configuration)

## Project Description

This project heavily relies on ROS# developed and maintained by Siemens. More specifically, it is a fork of `tarukosu/ros-sharp` ([https://github.com/tarukosu/ros-sharp](https://github.com/tarukosu/ros-sharp)), which itself is a [UWP](https://docs.microsoft.com/en-us/windows/uwp/get-started/universal-application-platform-guide)-enabled fork of the original `siemens/ros-sharp` ([https://github.com/siemens/ros-sharp](https://github.com/siemens/ros-sharp)). This allows to use ROS# with the Microsoft HoloLens to visualize in real time the robot's perception and cognition.

### Fork History

Below is a brief summary of the ROS# project history and current status.

*   Siemens developed ROS# to interface ROS (Ubuntu) and C# (Windows/Unity) via Rosbridge. Initially, there was no support for Microsoft.
*   The project was dormant until `MartinBischoff` did a complete refactoring around June 2018 with new ROS messages and types, but there was still no support for UWP.
*   Just **before** that refactoring, `tarukosu` forked ROS# to enable a preliminary support for UWP in [https://github.com/tarukosu/ros-sharp](https://github.com/tarukosu/ros-sharp). This HSR-ros-sharp project is a fork of that fork, and was deployed during World Robot Summit 2018.
*   Just **after** that refactoring, `dwhit` forked the refactored ROS# to enable UWP support too in [https://github.com/dwhit/ros-sharp](https://github.com/dwhit/ros-sharp). This latest fork is up-to-date and maintained with `MartinBischoff`'s upstream repository. More importantly, it has been officially recognized by him as the version to use with HoloLens, and added to the official documentation (as he will not add UWP support in the upstream repository): [https://github.com/siemens/ros-sharp#platform-support](https://github.com/siemens/ros-sharp#platform-support).
*   More detailed information can be found in the following discussions:
    *   [https://github.com/siemens/ros-sharp/issues/33](https://github.com/siemens/ros-sharp/issues/33)
    *   [https://github.com/siemens/ros-sharp/issues/59](https://github.com/siemens/ros-sharp/issues/59)

### Contribution Policy

*   HSR-ros-sharp is a child project of the HSR project ([https://gitlab.com/emlab/HSR](https://gitlab.com/emlab/HSR)) and, unless stated otherwise, the contribution guidelines of the parent HSR project apply: [https://gitlab.com/emlab/HSR/blob/devel/CONTRIBUTING.md](https://gitlab.com/emlab/HSR/blob/devel/CONTRIBUTING.md).
*   The branches and tags used to customize this fork should include the prefix `hsr-`. The default branch is `hsr-devel`. All forked branches from the upstream repository should be left untouched.
*   Please report any issue or future work by using the GitLab issue tracker: [https://gitlab.com/emlab/HSR-ros-sharp/issues](https://gitlab.com/emlab/HSR-ros-sharp/issues).

## Project Documentation

The sections below provide various documentation about the HSR-ros-sharp project and, in particular, information regarding the shared interface between the Toyota HSR robot and the Microsoft HoloLens (that is, the expected ROS outputs and ROS# inputs).

### Original Documentation

Original documentation can be found at the links below.

*   Readme: [https://github.com/siemens/ros-sharp](https://github.com/siemens/ros-sharp).
*   Releases: [https://github.com/siemens/ros-sharp/releases](https://github.com/siemens/ros-sharp/releases).
*   Wiki: [https://github.com/siemens/ros-sharp/wiki](https://github.com/siemens/ros-sharp/wiki).

### Getting Started

Follow the step-by-step guide below to get started with the HSR-ros-sharp development on Unity.

#### System Requirements

*To be added.*

#### Unity Configuration

The initial Unity configuration of the HSR-ros-sharp project should be performed as follows.

1.   Clone this HSR-ros-sharp repository and open the included Unity project `./Unity3D/` with Unity.
2.   Import the [`HoloToolkit-Unity-2017.4.0.0.unitypackage`](https://github.com/Microsoft/MixedRealityToolkit-Unity/releases/tag/2017.4.0.0) Unity package.
3.   Disable (or delete) the following DLL files:
     *   `Assets/HoloToolkit/Utilities/Scripts/GLTF/Plugins/JsonNet/Newtonsoft.Json.dll`
     *   `Assets/HoloToolkit/Utilities/Scripts/GLTF/Plugins/JsonNet/WSA/Newtonsoft.Json.dll`
4.   Choose the scene `Assets/Scene/WRS/WRS_main.unity`.
5.   From the "Mixed Reality Toolkit > Configure" tab:
     *   Select "Apply Mixed Reality Project Settings", then press the "Apply" button without changing the default settings.
     *   Select "Apply UWP Capability Settings", check "Internet Client Server" and "Private Network Client Server", then press the "Apply" button.
6.   Launch the simulator by pressing the "Play" button in the main Unity window.

#### Deployment Procedure

*To be added.*

### Custom Visualizations

Find below information about the custom visualizations of the HSR-ros-sharp project that can be rendered in Mixed Reality (MR) using the Microsoft HoloLens.

#### Spatial Concepts

*   [`visualization_msgs/MarkerArray`](http://docs.ros.org/kinetic/api/visualization_msgs/html/msg/MarkerArray.html): `/em/draw_position/array` ← Array of [Rviz](http://wiki.ros.org/rviz) markers.
    *   [`visualization_msgs/Marker`](http://docs.ros.org/kinetic/api/visualization_msgs/html/msg/Marker.html) ← Visual representation of a Spatial Concept.
        *   `int32 type = 2` ← Marker of type `SPHERE`.
        *   `geometry_msgs/Pose pose` ← Position (`x`, `y`, `z`) and rotation (`x`, `y`, `z`, `w`) of the Spatial Concept.
        *   `std_msgs/ColorRGBA color` ← Shape color.
        *   `geometry_msgs/Vector3 scale` ← Distribution (`scale.x`, `scale.y`) of the Spatial Concept.
    *   [`visualization_msgs/Marker`](http://docs.ros.org/kinetic/api/visualization_msgs/html/msg/Marker.html) ← Name attached to a Spatial Concept.
        *   `int32 type = 9` ← Marker of type `TEXT_VIEW_FACING`.
        *   `geometry_msgs/Pose pose` ← Position (`x`, `y`, `z`) of the Spatial Concept.
        *   `std_msgs/ColorRGBA color` ← Text color.
        *   `string text` ← Name and probability attached to the Spatial Concept.

**Note:** Two kind of markers are required to simultaneously display the shape and names of each Spatial Concept. Although all the markers are randomly arranged inside a single array, there is no need to sort it as the included position in each marker is enough for correctly associate the shape and text markers on display.

#### Navigation Goal

*   [`move_base_msgs/MoveBaseActionGoal`](http://docs.ros.org/kinetic/api/move_base_msgs/html/msg/MoveBaseActionGoal.html): `/move_base/move/goal` ← Robot's navigation goal.

#### Path Plan

*   [`nav_msgs/Path`](http://docs.ros.org/kinetic/api/nav_msgs/html/msg/Path.html): `base_local_path_ref` ← Robot's navigation trajectory.

#### Reference Frame

*   [`geometry_msgs/PoseStamped.msg`](http://docs.ros.org/kinetic/api/geometry_msgs/html/msg/PoseStamped.html): `/global_pose` ← Robot's position on the map.
*   [`tf2_msgs/TFMessage`](http://docs.ros.org/kinetic/api/tf2_msgs/html/msg/TFMessage.html): `/tf` ← Robot's reference frame tree.

**Note:** Currently, the HoloLens coordinate system has to match the HSR coordinate system on the map. In other words, the initial position the HoloLens should be the origin of the HSR map. Future work should tackle this limitation by including an online recalibration procedure using, for example, AR markers or point cloud matching.

### Setup History

**(For reference only, see [Getting Started](#getting-started) for current setup and configuration.)**

The original Unity configuration of the HSR-ros-sharp project was performed as described in this section. The step-by-step guide below should only be seen as a memo/reference would a similar initialization be required in the future.

#### Original Requirements

*   **Unity**: LTS Release 2017.4.6f1 (June 22th, 2018), [https://unity3d.com/unity/qa/lts-releases](https://unity3d.com/unity/qa/lts-releases).
*   **MixedRealityToolkit-Unity**: HoloToolkit-Unity-2017.4.0.0.unitypackage (May 31th, 2018), [https://github.com/Microsoft/MixedRealityToolkit-Unity/releases/tag/2017.4.0.0](https://github.com/Microsoft/MixedRealityToolkit-Unity/releases/tag/2017.4.0.0).
*   **ROS#**: RosSharp-tarukosu-cb56e38.unitypackage (July 5th, 2018), [https://share.hsr.io/remote.php/webdav/share_tmc_emlab/data/HSR-ros-sharp/RosSharp-tarukosu-cb56e38.unitypackage](https://share.hsr.io/remote.php/webdav/share_tmc_emlab/data/HSR-ros-sharp/RosSharp-tarukosu-cb56e38.unitypackage).
*   **Visual Studio**: Visual Studio Community 2017 15.7.5+ (July 10th, 2018), [https://visualstudio.microsoft.com/vs/community/](https://visualstudio.microsoft.com/vs/community/).
    *   Workloads:
        *   "Universal Windows Platform development" (incl. "Windows 10 SDK (10.0.17134.0)")
        *   "Game development with Unity"
    *   Individual components:
        *   "MSBuild"

#### Original Configuration

1.   Create a new Unity project.
2.   Import the [`HoloToolkit-Unity-2017.4.0.0.unitypackage`](https://github.com/Microsoft/MixedRealityToolkit-Unity/releases/tag/2017.4.0.0) Unity package.
3.   Disable (or delete) the following DLL files:
     *   `Assets/HoloToolkit/Utilities/Scripts/GLTF/Plugins/JsonNet/Newtonsoft.Json.dll`
     *   `Assets/HoloToolkit/Utilities/Scripts/GLTF/Plugins/JsonNet/WSA/Newtonsoft.Json.dll`
3.   Import the `RosSharp-tarukosu-cb56e38.unitypackage` Unity package.
4.   Add `Assets/RvizOnHoloLens/Scenes/ViewTF.unity` to the "Build Settings" in the Unity GUI.
5.   Build the solution with "Mixed Reality Toolkit > Build Window > Build All" in the Unity GUI.
6.   Edit the `ROSConnector` parameters in `Assets/RvizOnHoloLens/Scenes/ViewTF.unity` with the "Inspector" panel as needed.
7.   Launch the simulator by pressing the "Play" button in the main Unity window.
