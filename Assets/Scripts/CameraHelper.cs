using UnityEngine;

static class CameraHelper {
    public static float HalfWidth {
        get {
            return Camera.main.orthographicSize / Screen.height * Screen.width;
        }
    }

    public static float Width {
        get {
            return 2*HalfWidth;
        }
    }

    public static float HalfHeight {
        get {
            return Camera.main.orthographicSize;
        }
    }

    public static float Height {
        get {
            return 2 * HalfHeight;
        }
    }

    public static Rect Rect {
        get {
            return Camera.main.rect;
        }
    }
}
