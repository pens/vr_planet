
using UnityEngine;

public class Earth : MonoBehaviour {
    public InputManager inputManager;

    bool isGrabbing;
    Vector3 prevDist;

    private void Start()
    {
        inputManager.Grabbed += grab;
        inputManager.Ungrabbed += ungrab;
    }

    void Update () {
        if (isGrabbing)
        {
            Vector3 dist = inputManager.RightPos() - inputManager.LeftPos();
            transform.rotation = Quaternion.FromToRotation(prevDist, dist) * transform.rotation;
            prevDist = dist;
        }
    }
    void grab(object sender, ClickedEventArgs e)
    {
        isGrabbing = true;
        prevDist = inputManager.RightPos() - inputManager.LeftPos();
    }

    void ungrab(object sender, ClickedEventArgs e)
    {
        isGrabbing = false;
    }
}
