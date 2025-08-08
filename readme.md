## About

This is a simple BepInEx plugin for Tunguska: The Visitation, made to provide some configurability
in modifying some survival mechanics found in the game.

<br>

This modification uses Harmony to inject scripts into functions via hooking,
and overrides functions when post and prefixes were unnable to achieve a desired result.

## Making Your Own Mods For Tunguska: The Visitation

#### Target Audience
This is meant for individuals who may have never modded a Unity Engine game before.

#### Things You'll Need
* The ability to read and understand code/documentation
* Rudimentary C# skills
* Rudimentary Visual Studio or other IDE experience
* A program to view Assembly .dll's
    - I use [dnSpy](https://github.com/dnSpy/dnSpy)
* [Visual Studio](https://visualstudio.microsoft.com/downloads/)

        - NOTE: I used Visual Study 2022.
* [BepInEx](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.23.3)

        - NOTE: I used and linked v5.4.23.3, but later versions should work fine.

#### Installing BepInEx
Install BepInEx in your game's root folder. If your using steam this can be done by,
right-clicking your game > selecting *Manage* > then *Browse local files*.

After installing BepInEx, run it once, now we should be all set when it comes time to test our mod.

#### Setting Up Our Project Environment
Start by creating a Visual Studio project, make it a class library, and I used 4.7.2 .NET Framework for the project settings. After creating your project, the first thing we'll need to do is add some references to it. We'll need to get some specific .dll files from both BepInEx and our game.

* **1.** We'll grab the .dll files from BepInEx, navigate to *Tunguska The VisitationBepInEx\core\\* folder where we'll need to add references to these .dll's
    - *0Harmony.dll*
    - *BepInEx.dll*

* **2.** Then we'll add references to our Unity/Game assemblies which can be found *Tunguska The Visitation\Tunguska_Data\Managed\\*
    - *Assembly-CSharp.dll*
    - *UnityEngine.dll*
    - *UnityEngine.CoreModule.dll*

Once we've added these assemblies as references we are good to start scripts to modify our mod, depending on the mod you are making and what it modifies you may need to add more references, but for most Harmony patches we should be good.

#### Using dnSpy
Now it's time to take a look at our *Assembly-CSharp.dll* with dnSpy. At this point there's not much I can tell you to do beyond looking at the code. Pick something you want to modify in the game and then start searching for it using key terms you think it might be identified with.

    - NOTE: Make sure to search not only by class and method names but also by parameters and attributes.

I will say *Tunguska: The Visitation* itself is far more readable than some other Unity Engine games such as *Wasteland 3*.

#### Creating Your Mod
With that you should be ready to create your mod, your environment should be all set, all thats left is to start modifying things. After writing your modifications just build your solution go to your project's debug folder and drop your solution .dll file into the *BepInEx\plugins\\* folder.

    - NOTE: When building/compiling your project it will also have all the reference .dll's 
    in your project's debug folder, you can ignore these .dll files.

### Final Notes
Here are some helpful resources for how BepInEx Plugins and Harmony works.
* [Harmony Documentation](https://harmony.pardeike.net/articles/patching.html)
* [BepInEx Documentation](https://docs.bepinex.dev/)

Goodluck, and remember to just keep experimenting.