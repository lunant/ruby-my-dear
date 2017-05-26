using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MotionController {
    /*
     * Interfaces with animatables
     */
    static Animatable2 _prefab;

    public static void Animate(LineParams2 lp, MotionParams mp) {
        var p = CreatePlane(lp.scale, lp.rotation);
        p.localScale = lp.scale;
        p.position = lp.position;
        // must come after position
        Debug.Log(lp.level);
        p.level = lp.level;
        p.velocity = mp.velocity;
        p.color = lp.color;
    }

    static Animatable2 CreatePlane(Vector2 localScale, float entryAngle) {
        return GameObject.Instantiate<Animatable2>(prefab, localScale, Quaternion.Euler(0, 0, entryAngle));
    }

    static Animatable2 prefab {
        get {
            if (_prefab == null) {
                _prefab = Resources.Load<Animatable2>("Animatable2");
            }
            return _prefab;
        }
    }
}


