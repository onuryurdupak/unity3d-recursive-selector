# Unity3D - Recursive Selector

#### DESCRIPTION:

Enables recursive selection of children components of an input type in Unity Editor.

#### USAGE:

- Download the asset from releases page and import into your project.

- In Unity Editor, go to the toolbar and click on `Window -> Recursive Selector` to open selector window.

- Go to the `Hierarchy` window, select game objects you want to make recursive selection over.

- Go to the `Recursive Selector`, type in type to search for and click on `Select`.

- Your `Hierarchy` selection should now only contain game objects that have your input component type.

---

Let's look at an example use case where this might come in handy.

Imagine your scene has following game objects:


![](/Readme/Example_Lobby_Representation.png)

You might want to select all `Text` components that contain the 'Empty Slot' text and modify their font color.

Your options include:

1-) Using the `Hierarchy` search bar with `Types` which will select all instances of `Text` component in the scene.

2-) Using a prefab for repeating elements; editing the prefab source, modifying all prefab clones. (This is a valid option, but you may want to avoid using prefabs for this. E.g: You may want to keep whole object state in the encapsulating scene.)

3-) Using the `Recursive Selector` as shown below:


- Select top level game objects from `Hierarchy` window.

![](/Readme/Hierarchy_Selection.png)

- Type in `UI.Text` (quick-select complete definition from `Filtered Results` foldout). Then click on `Select`.

![](/Readme/Recursive_Selector_Usage.png)

- Upon returning to the `Hierarchy` window, your selection should now only contain `UI.Text`-typed children of the previously selected items. You can now update the properties of all selected items from the `Inspector` window.

![](/Readme/Change.png)

(The game window images might appear blurry due to scaling down.)
