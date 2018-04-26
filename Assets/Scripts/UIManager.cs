using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Text Cities;
    public Text Kills;
    
    public void SetCities(int n)
    {
        Cities.text = n.ToString();
    }

    public void SetKills(int n)
    {
        Kills.text = n.ToString();
    }
}
