# HoloLens-ElectricalMuscleStimulation

This repository provides you code to replicate the AR experiences in the paper 
``Adding Force Feedback to Mixed Reality Experiences and Games using Electrical Muscle Stimulation (CHI'18)``, which you can read [here](https://hpi.de/baudisch/projects/ems-ar-haptics.html).

## Structure of this repository

This repository has the four core folders:

1. HoloLens_Apps: the Unity3D apps that run in the HoloLens
2. EMS_Server: The Unity3D app that behaves as a server and accepts commands from the HoloLens apps, and generates EMS messages for an EMS device (more on the type of devices below).
3. Shared_Assets: Shared assets between both EMS_Server and running HoloLens Applications

Also:

* Documentation: notes, liability waiver, etc.


## Dependencies

### Software Dependencies

This will require:

1. [Unity 5.5.1f](https://unity3d.com/get-unity/download/archive) (other versions might work with modifications)
2. [Vuforia Plugin](https://www.vuforia.com/) for marker tracking (you can read [here](https://library.vuforia.com/articles/Training/getting-started-with-vuforia-in-unity-2017-2-beta.html) how to install Vuforia for Unity)
3. A working [HoloLens](https://developer.microsoft.com/en-us/windows/mixed-reality) installation with the HoloLens []toolchain](https://developer.microsoft.com/en-us/windows/mixed-reality/install_the_tools) (Windows 10, Visual Studio, etc)
4. A HoloLens Mixed Reality Headset [connected to your Wifi router](https://docs.microsoft.com/en-us/hololens/hololens-setup)

Optionally, we recommend:

* using our [AR marker generator](https://github.com/PedroLopes/AR-Marker-Generator) to quickly create a batch of markers (for hands, objects, etc) instead of manually creating every marker.

### Hardware Requirements

This project was built for a particular EMS device (Rehastim V1, which is off the market, sorry!). However, you can **easily** modify it to run on your custom EMS device (as long as it accepts USB commands) or using an open-source EMS controller such as [openEMSstim](https://github.com/PedroLopes/openEMSstim). If you are modifying the ``EMS_server`` to run with another EMS device, read this first.

## Running the examples

1. Clone this repository by running ``git clone https://github.com/PedroLopes/HoloLens-ElectricalMuscleStimulation.git`` (or you preferred visual tool)
2. Open the <name> project in Visual Studio.
3. Connect your HoloLens, connect also your HoloLens to a Wifi
4. Select to deploy this project to the HoloLens (verify that the app is launched and running)
5. Open the <name> project in your local machine (that will act as ``EMS_server``), connect this machine to the same Wifi.
6. Setup the communication between both HoloLens <> EMS_Server. To do this you have to configure the EMS_server to receive requests from the HoloLens. <give the IP address of the EMS_server to the HoloLens app?>. 
7. Deploy and Start both apps (once more double checked the IP addresses and Ports match).
8. Configure your EMS machine, calibrate it to work comfortably and Pain-free (see here for more details). 
9. ###Testing The EMS
9. Now, launch the HoloLens app, put your hand out in the pointing gesture (to make sure it is tracked, or attach a Hand_marker to it -- review your code to make sure you supplied the right marker to Vuforia). 
  11. Reach your hand out and touch a virtual object (e.g., we suggest you try the couch scene first). You should feel EMS (verify that the EMS_Server received the message). 
  12. Hooray! (or read the FAQ below)

### Support

This work was kindly supported by the Hasso Plattner Institute. 

![HPI](documentation/images/hpi.png)

### Code authors

The code was authored by **Sijing You** (Hololens Apps, EMS Unity Server) and **Pedro Lopes** (EMS protocol, Marker Generator). You can contact them via github.

The paper ``Adding Force Feedback to Mixed Reality Experiences and Games using Electrical Muscle Stimulation (CHI'18)``, which you can read [here](https://hpi.de/baudisch/projects/ems-ar-haptics.html) was authored by Pedro Lopes, Sijing You, Alexandra Ion and Patrick Baudisch. 

#FAQ

### 1. I don't have an EMS device! What to I do?

You can get or build an [openEMSstim](https://github.com/PedroLopes/openEMSstim) (a piece of hardware that allows you to easily control an off the shelf muscle stimulator). All the schematics and code are available. 

### 2. I don't feel EMS

1. Look at your EMS_Server, did it get the message from the HoloLens? If not
	1. re-check the IP addresses for both devices.
	2. Make sure they are on the same network
	3. Make sure they can ``ping`` each other (in other words, make sure this network is not aggressively blocking ports/messages)
	
2. If the message arrived at your EMS_Server but you did not feel anything: 
	1. Recheck your EMS machine by calibrating it again. 

### Liability

Please refer to the liability waiver (in documentation/liability_waiver.md).
