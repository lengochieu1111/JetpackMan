using MVCS.Architecture.BaseCharacter;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : RyoMonoBehaviour
{
    [SerializeField] private BaseCharacter _character;
    [SerializeField] private float time = 2f;

    public BaseCharacter Character => _character;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadCharacter();
    }

    private void LoadCharacter()
    {
        if (this._character != null) return;
        this._character = this.GetComponentInChildren<BaseCharacter>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this._character.transform.localPosition = new Vector2(-25, this._character.transform.localScale.y);
        this._character.Controller.SendRunRequest(true);
        StartCoroutine(EndGame_Coroutine());
    }

    private IEnumerator EndGame_Coroutine()
    {
        float counter = 0;
        
        while (counter < this.time)
        {
            counter += Time.deltaTime;
            Vector3 tartget = this.transform.localPosition;
            tartget.x = 0;
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, tartget, counter / this.time);

            yield return null;
        }
        this._character.Controller.SendRunRequest(false);

        yield return new WaitForSeconds(2F);

        int sceneTotal = SceneManager.sceneCountInBuildSettings;
        int sceneNext = SceneManager.GetActiveScene().buildIndex + 1;

        SceneManager.LoadScene(sceneNext % sceneTotal, LoadSceneMode.Single);

    }

}
