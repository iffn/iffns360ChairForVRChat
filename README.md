# iffns360ChairForVRChat
A script that allows desktop players to turn 180° in either direction. Does not affect VR players.
Especially usefull in SaccFlightAndVehicles. Attach to the station and set the appropriate HeadXOffset value.
Note: Client sim currently unaffected
 
Requires VRChat SDK3 for Worlds, the compatible Unity version and UdonSharp 0.20.3 or higher.

### Integration without Submodules
To maintain compatability with other projects, please put everything into ```/Assets/iffnsStuff/iffns360ChairForVRChat``` 

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
