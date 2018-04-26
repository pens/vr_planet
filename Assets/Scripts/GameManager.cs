using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public UIManager uiManager;
    public InputManager inputManager;
    public Transform rockets;
    public Transform cities;
    public Transform explosions;
    public GameObject earth;

    public float SPAWN_DIST;
    public float SPEED;
    public float SPAWN_RATE;

    Object rocketPrefab;
    Object explosionPrefab;
    Object trailPrefab;
    float delta;
    enum State
    {
        GameOver,
        Paused,
        Playing
    }
    State state;
    static int kills;
    static int citiesLeft;

    void Start()
    {
        rocketPrefab = Resources.Load("rocket");
        explosionPrefab = Resources.Load("explosion");
        trailPrefab = Resources.Load("Rocket Trail");
        inputManager.MenuClicked += pause;
        Restart();
    }

    void Update()
    {
        delta += Time.deltaTime;
        while (delta > SPAWN_RATE)
        {
            spawnRocket();
            delta -= SPAWN_RATE;
        }
    }

    void Restart()
    {
        foreach (Transform rocket in rockets)
        {
            Destroy(rocket.gameObject);
        }
        foreach (Transform explosion in explosions)
        {
            Destroy(explosion.gameObject);
        }
        foreach (Transform city in cities)
        {
            city.gameObject.SetActive(true);
        }
        earth.SetActive(true);
        kills = 0;
        uiManager.SetKills(kills);
        citiesLeft = cities.childCount;
        uiManager.SetCities(citiesLeft);
        state = State.Playing;
    }

    void pause(object sender, ClickedEventArgs e)
    {
        if (state == State.Playing)
        {
            state = State.Paused;
            Time.timeScale = 0;
        }
        else if (state == State.Paused)
        {
            state = State.Playing;
            Time.timeScale = 1;
        }
    }

    public void KillCity(GameObject city)
    {
        city.SetActive(false);
        uiManager.SetCities(--citiesLeft);
        if (citiesLeft == 0)
        {
            state = State.GameOver;
            //earth.SetActive(false);
        }
    }

    public void KillRocket(GameObject rocket, bool byPlayer)
    {
        explode(rocket);
        if (byPlayer)
            uiManager.SetKills(++kills);
        Destroy(rocket);
    }

    void spawnRocket()
    {
        var rocket = Instantiate(rocketPrefab) as GameObject;
        rocket.name = "rocket";
        rocket.transform.SetParent(rockets, false);
        rocket.transform.localPosition = Random.onUnitSphere * SPAWN_DIST;

        var nearest = cities.GetChild(0);
        float min = Vector3.Distance(rocket.transform.position, cities.GetChild(0).position);
        for (var i = 0; i < cities.childCount; ++i)
        {
            var dist = Vector3.Distance(rocket.transform.position, cities.GetChild(i).position);
            if (dist < min)
            {
                nearest = cities.GetChild(i);
                min = dist;
            }
        }

        rocket.transform.LookAt(nearest);
        rocket.GetComponent<Rocket>().gameManager = this;
        rocket.GetComponent<Rocket>().target = nearest.position;
        var trail = Instantiate(trailPrefab) as GameObject;
        trail.name = "trail";
        trail.transform.SetParent(rocket.transform, false);
    }

    void explode(GameObject target)
    {
        var expl = Instantiate(explosionPrefab) as GameObject;
        expl.name = "explosion";
        expl.transform.SetParent(explosions, false);
        expl.transform.position = target.transform.position;
        expl.transform.rotation = target.transform.rotation;
        StartCoroutine(grow(expl));
    }

    //TODO fix for when match ends
    IEnumerator grow(GameObject expl)
    {
        for (var i = 0.0f; i < 3.0f; i += Time.deltaTime)
        {
            for (var j = 0; j < expl.transform.childCount; ++j)
            {
                Color color = expl.transform.GetChild(j).GetComponent<Renderer>().material.color;
                expl.transform.GetChild(j).GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, 1.0f - i / 3.0f);
            }
            expl.transform.localScale = Vector3.one * .05f * i;
            yield return null;
        }
        Destroy(expl);
    }
}
