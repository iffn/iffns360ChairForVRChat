# iffns360ChairForVRChat
A script that allows desktop players to turn 180° in either direction. Does not affect VR players.

Especially usefull in vehicles. This works by rotation the player

A modified version has been added to SaccFlightAndVehicles:

https://github.com/Sacchan-VRC/SaccFlightAndVehicles/blob/603cc8afcd7c82b8f41f444d395cd5e8644aaef8/Scripts/SaccVehicleSeat.cs#L355

### How to use:
- Requires VRChat SDK3 for Worlds, the compatible Unity version and UdonSharp.
- Attach the iffns360Chair.cs script to the station and set the appropriate HeadXOffset value.
- Note: It is recommended to set the Player Enter Location of the attached station to a sepparate object, since the chair will otherwise also rotate.

### Integration without Submodules
To maintain compatability with other projects, it is recommended to put everything into ```/Assets/iffnsStuff/iffns360ChairForVRChat```

This is done automatically when importing the Unity package.

### Git Submodule integration
Add this submodule with the following git command (Assuming the root of your Git project is the Unity project folder)
```
git submodule add https://github.com/iffn/iffns360ChairForVRChat.git Assets/iffnsStuff/iffns360ChairForVRChat
```

If you have manually added it, use this one first. (I recommend to close the Unity project first)
```
git rm Assets/iffnsStuff/iffns360ChairForVRChat -r
```

![image](https://user-images.githubusercontent.com/18383974/176571999-c1894a3e-4877-447b-8fa9-91ea06a9c440.png)
