# VRC_ChurroTweaks
An additional API for VRChat. This baby has ways to create and load in new custom events and more will be coming.

# Editing Policy
Fork from the release or dev branches. Release will have the current stable build but dev will have new experimental goodies. Add onto it from your hearts content from there.

# Hints and Tricks
When writing custom events you have to create both a CustomEventSpawn and CustomEvent. The CustomEventSpawn creates the CustomEvent in its Create(VrcEvent e) method. This is virtual, override it and make sure to give the CustomEvent the passed VrcEvent through SetEvent(). There is some other functionality but its pretty much spelled out in the names, email me if you have questions.
