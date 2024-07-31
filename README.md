Tic Tac Toe implementation in Unity.
Using Unity version 2022.3.40f.

Supports Player vs Player and Player vs AI matches.

Intentionally not over-architected but still adhering to code standards set out in the task document.
Currently no usage of Dependency Injection or complex asset loading systems such as Addressables.

There are some places in code that could be optimized to reduce memory allocations and outside of Unity UI package code, it would be possible to reach zero allocations per frame after initialization.

Using MVP pattern where View is directly implemented in Unity UI.

Using simple state management, which could be expanded upon later.
