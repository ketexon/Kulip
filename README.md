# Kulip

Kulip is a general purpose Unity library meant to speed up development by providing common features.

# Features

## "Static" Scriptable Objects `StaticSO<T>`

What I term a "static" ScriptableObject is a ScriptableObject whose goal is to be a global variable.

This is useful to allow **serializing a reference to a value, not a component, to prevent coupling**. It is also somewhat of a replacement for `static` variables or singletons, however, it does not actually have static lifetime. In addition, `StaticSO` allows for reactive programming using a `System.Action<T> ValueChangedEvent`. Comes with a few built-in types (`StaticFloatSO`, `StaticIntSO`, `StaticObjectSO`, `StaticUnityObjectSO`) and can be derived to add more. Note: the generic base class, as far as I can tell, cannot be instanced as a ScriptableObject, but could probably be used for a `SerializedField`.

![Unity_zMnMmGSZ4I](https://github.com/ketexon/Kulip/assets/29184562/11371263-f12e-4521-91c6-25c9dcdb8789)

## Customizable Time ScriptableObjects

Time that is stored and manipulated isolated within a static ScriptableObject. This allows you to not mess with, for example, `Physics.timeScale` if you want to scale your time or pause your game. In addition, it allows multiple `Time` instances at once. For an added bonus, it does not require an `Update` loop, since all calculations are done with respect to the global `Time.time`. *This might not work well with the physics engine.*

![Unity_Ka2vkIN0Lu](https://github.com/ketexon/Kulip/assets/29184562/c597dcb9-5c99-4147-ab20-009dfe6cc945)

## Easing Functions

Easing functions ported from https://easings.net/ [(GitHub)](https://github.com/ai/easings.net). See the sample in `/Samples/Easing` for a demo!

![Unity_XYF1AmSIKf](https://github.com/ketexon/Kulip/assets/29184562/4b4937df-3328-425b-a863-892f9968912a)

# Getting Started

[Installation instructions](/wiki/Installation) can be found on the wiki.
