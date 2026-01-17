# RaycastDebugger
A simple-to-use, lightweight runtime physics visualization utility for Unity.

It mirrors Unity’s built-in ```Physics.*Cast``` and ```Physics.Overlap*``` APIs and renders their results clearly in-scene for debugging.

Usage is as simple as adding a **single line** to your code.

The entire Utility is a **single script** ```RaycastDebugger.cs```, to be easy to add to you projects and extremely lightweight.

- Intended for use with **Unity 2019 LTS+**
- Not officially tested across all Unity versions
- Community feedback and PRs are welcome
<br><br><br>

<iframe width="560" height="315" src="https://youtu.be/FFBhl0hzOjc" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>

## Supported Physics Queries
### Casts
    • Raycast
    • BoxCast
    • SphereCast
    • CapsuleCast
### Overlaps
    • OverlapBox
    • OverlapSphere
    • OverlapCapsule
All examples are provided for each use case.
<br><br><br>


## Example Usage
```
void FixedUpdate()
{
    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 5f))
        RaycastDebugger.DebugRaycast(transform.position, transform.forward, hit.distance, Color.red);
    else
        RaycastDebugger.DebugRaycast(transform.position, transform.forward, 5f);
}
```
<br><br>

## Calls
Call repeatedly through ```Update``` or ```FixedUpdate``` to continuously visualize your queries.
<br><br><br>

## Color Convention
The debugger follows a consistent, intention-based color scheme:
| Color | Meaning |
|------|---------|
| White | No hit |
| Red | Hit |
| Green | Overlap |

You may override colors per call.
<br><br><br>


## Optional Parameters
Helper methods support optional parameters to visualize more clearly:

    • Collider collider = null for optional visualization
    • Color color = default

Default Color Behavior

    • If the color parameter is not set, it defaults to ```Color.white```.
    • Explicit colors **always override** the convention.
<br>

### Example usage with defaults:
```
RaycastDebugger.DebugRaycast(origin, dir, length);
```
```
// Example usage with explicit color:
RaycastDebugger.DebugRaycast(origin, dir, hit.distance, Color.red);
```
<br><br>
### Design Goals
    • Simple to use
    • Minimal allocations
    • Familiar Unity API feel
    • Clear visual feedback

<br><br>
### Shipping and Final Builds
It is not recommended to ship this script with your final build.
The script will not compile (safely) if not running in the Unity Editor, preventing unnecessary runtime overhead.

### Status
✅ All common Physics casts & overlaps covered

✅ Easy to use — add a single line to your code
<br><br>


## Contributing

### Contributions are welcome!

If you’d like to improve or extend this utility, that would be great!!

Please keep changes lightweight and aligned with the existing API style.

---
I have been trying really to break into the industry and this is just one step ahead.
Wish me luck :)
