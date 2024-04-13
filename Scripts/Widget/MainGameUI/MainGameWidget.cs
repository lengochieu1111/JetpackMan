using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameWidget : RyoMonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _background, _menu_1, _menu_2;
    [SerializeField] private float _startGameTime = 1f;
    private Coroutine _startGameCoroutine;
    public Animator Animator => _animator;
    public Transform Background => _background;
    public Transform Menu_1 => _menu_1;
    public Transform Menu_2 => _menu_2;
    public float StartGameTime => _startGameTime;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadAnimator();

        this.LoadBackground();
        this.LoadMenuOne();
        this.LoadMenuTwo();
    }

    private void LoadAnimator()
    {
        if (this._animator != null) return;
        this._animator = GetComponent<Animator>();
    }

    private void LoadBackground()
    {
        if (this._background != null) return;
        this._background = this.transform.Find("Background");
    }
    private void LoadMenuOne()
    {
        if (this._menu_1 != null) return;
        this._menu_1 = this.transform.Find("Menu_1");
    }
    private void LoadMenuTwo()
    {
        if (this._menu_2 != null) return;
        this._menu_2 = this.transform.Find("Menu_2");
    }

    public void RequestStartGame()
    {
        this.Animator.SetTrigger("isStartGame");
    }

    public void StartGame()
    {
        this.LoadSceneMatch();
    }

    private IEnumerator StartGameCourountine()
    {
        Vector3 currentPos_background = this.Background.position;
        Vector3 startGamePos_background = Vector3.zero;
        float timeCounter = 0;

        while (timeCounter < this.StartGameTime)
        {
            timeCounter += Time.deltaTime;

            this.Background.position = Vector3.Lerp(currentPos_background, startGamePos_background, timeCounter / this.StartGameTime);

            yield return null;
        }

        this.LoadSceneMatch();
    }
    
    private IEnumerator StartGameCourountine_Menu_Down()
    {
        Vector3 currentPos_menu_1 = this.Menu_1.position;
        Vector3 currentPos_menu_2 = this.Menu_2.position;

        Vector3 startGamePos_menu_1 = currentPos_menu_1;
        startGamePos_menu_1.y -= 20;

        Vector3 startGamePos_menu_2 = currentPos_menu_2;
        startGamePos_menu_2.x += 20;

        float timeCounter = 0;

        while (timeCounter < 2)
        {
            timeCounter += Time.deltaTime;

            this.Menu_1.position = Vector3.Lerp(currentPos_menu_1, startGamePos_menu_1, timeCounter / 2);
            this.Menu_2.position = Vector3.Lerp(currentPos_menu_2, startGamePos_menu_2, timeCounter / 2);

            yield return null;
        }

        StartCoroutine(this.StartGameCourountine_Menu_Up());

    }

    private IEnumerator StartGameCourountine_Menu_Up()
    {
        Vector3 currentPos_menu_1 = this.Menu_1.position;
        Vector3 currentPos_menu_2 = this.Menu_2.position;

        Vector3 startGamePos_menu_1 = Vector3.zero;
        startGamePos_menu_1.y = 200;
        Vector3 startGamePos_menu_2 = Vector3.zero;
        startGamePos_menu_2.x = -525;

        float timeCounter = 0;

        while (timeCounter < 10)
        {
            timeCounter += Time.deltaTime;

            this.Menu_1.position = Vector3.Lerp(currentPos_menu_1, startGamePos_menu_1, timeCounter / 10);
            this.Menu_2.position = Vector3.Lerp(currentPos_menu_2, startGamePos_menu_2, timeCounter / 10);

            yield return null;
        }

    }

    private void LoadSceneMatch()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene % sceneCount, LoadSceneMode.Single);
    }

}
