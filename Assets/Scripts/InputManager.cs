using UnityEngine;

public class InputManager : MonoBehaviour {
    public SteamVR_TrackedController left;
    public SteamVR_TrackedController right;
    public GameObject earth;
    
    int grips;

    public event ClickedEventHandler MenuClicked;
    public event ClickedEventHandler Grabbed;
    public event ClickedEventHandler Ungrabbed;

    public Vector3 RightPos()
    {
        return right.gameObject.transform.position;
    }

    public Vector3 LeftPos()
    {
        return left.gameObject.transform.position;
    }

    void Start()
    {
        left.MenuButtonClicked += menu;
        right.MenuButtonClicked += menu;

        left.Gripped += grab;
        left.Ungripped += ungrab;
        right.Gripped += grab;
        right.Ungripped += ungrab;
    }

    void menu(object sender, ClickedEventArgs e)
    {
        if (MenuClicked != null)
            MenuClicked(sender, e);
    }

    void grab(object sender, ClickedEventArgs e)
    {
        grips++;
        if (grips == 2)
            if (Grabbed != null)
                Grabbed(sender, e);
    }

    void ungrab(object sender, ClickedEventArgs e)
    {
        if (grips == 2)
            if (Ungrabbed != null)
                Ungrabbed(sender, e);
        grips--;
    }
}
