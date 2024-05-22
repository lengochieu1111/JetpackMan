using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateWidget : BaseWidget
{
    [SerializeField] private bool _isEnoughEnergy;
    [SerializeField] private Image _skill_Image;
    [SerializeField] private Image _progressLevel_Image;
    [SerializeField] private Image _progressEnergy_Image;
    [SerializeField] private Text _coin_Text;
    [SerializeField] private Text _crystal_Text;
    [SerializeField] private Sprite _off_Skill, _on_Skill;
    [SerializeField] private Vector2[] _skillSpawnPosition = new Vector2[]
        { new Vector2( 4.9f, 6.5f ), new Vector2( 7.78f, 3.27f ) , new Vector2( 3.44f, -0.3f ) , 
            new Vector2( 2.42f, -6.15f ) , new Vector2( 5.33f, -3.79f ) , new Vector2( 7.73f, -0.57f ) };
    private Coroutine _skillCroutine;
    public Sprite Off_Skill => _off_Skill;
    public Sprite On_Skill => _on_Skill;
    public Image ProgressLevel_Image => _progressLevel_Image;
    public Image ProgressEnergy_Image => _progressEnergy_Image;
    public Image Skill_Image => _skill_Image;
    public Text Coin_Text
    {
        get { return this._coin_Text; }
        private set { this._coin_Text = value;}
    }
    public Text Crystal_Text
    {
        get { return this._crystal_Text; }
        private set { this._crystal_Text = value; }
    }
    public bool IsEnoughEnergy
    {
        get { return this._isEnoughEnergy; }
        private set 
        { 
            if (value)
            {
                this.Skill_Image.sprite = this.On_Skill;
            }
            else
            {
                this.Skill_Image.sprite = this.Off_Skill;
            }

            this._isEnoughEnergy = value; 
        }
    }

    public Vector2[] SkillSpawnPosition => _skillSpawnPosition;

    #region LoadComponents

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadSkill_Image();
        this.LoadCoin_Text();
        this.LoadCrystal_Text();
        this.LoadProgress_Level();
        this.LoadProgress_Energy();
    }

    private void LoadSkill_Image()
    {
        if (this._skill_Image != null) return;
        Transform skill = this.FindChildByName(transform, "Skill_Button");
        this._skill_Image = skill?.GetComponent<Image>();
    }
    
    private void LoadCoin_Text()
    {
        if (this._coin_Text != null) return;
        Transform coin = this.transform.Find("Coin")?.Find("Coin_Text");
        this.Coin_Text = coin?.GetComponent<Text>();
    }
    
    private void LoadCrystal_Text()
    {
        if (this._crystal_Text != null) return;

        Transform crystal = this.transform.Find("Crystal")?.Find("Crystal_Text");
        this.Crystal_Text = crystal?.GetComponent<Text>();
    }
    
    private void LoadProgress_Level()
    {
        if (this._progressLevel_Image != null) return;

        Transform progress_Level = this.FindChildByName(transform, "Progress_Level");
        this._progressLevel_Image = progress_Level?.GetComponent<Image>();
    }
    
    private void LoadProgress_Energy()
    {
        if (this._progressEnergy_Image != null) return;

        Transform progress_Level = this.FindChildByName(transform, "Progress_Energy");
        this._progressEnergy_Image = progress_Level?.GetComponent<Image>();
    }

    private Transform FindChildByName(Transform parrent, string childName)
    {
        Transform childObject = parrent.Find(childName);

        if (childObject != null)
            return childObject;
        else
        {
            foreach (Transform child in parrent)
            {
                childObject = this.FindChildByName(child, childName);

                if (childObject != null)
                    return childObject;
            }

            return null;
        }
    }

    #endregion

    protected override void SetupValues()
    {
        base.SetupValues();

        this._isEnoughEnergy = false;
    }

    /*
     * 
     */

    public void PressPauseButton()
    {
        GameManager.Instance.SetMatchState(MatchState.IsPaused);
    }
    
    public void PressSkillButton()
    {
        if (this.IsEnoughEnergy)
        {
            this.Skill();
        }
    }

    /*
     * 
     */

    public void UpdateCoin_Text(string text)
    {
        if (this.Coin_Text == null) return;
        this.Coin_Text.text = text;
    }
    
    public void UpdateCrystal_Text(string text)
    {
        if (this.Crystal_Text == null) return;
        this.Crystal_Text.text = text;
    }
    
    public void UpdateProgress_Level(float percen)
    {
        if (this.ProgressLevel_Image == null) return;
        this.ProgressLevel_Image.fillAmount = percen;
    }
    
    public void UpdateProgress_Energy(float percen)
    {
        if (this.ProgressEnergy_Image == null) return;
        this.ProgressEnergy_Image.fillAmount = percen;
    }

    /*
     * 
     */

    public void SetActive_SkillButton(bool isActive)
    {
        this.IsEnoughEnergy = isActive;
    }

    private void Skill()
    {
        this.IsEnoughEnergy = false;
        GameMode.Instance.HandlePlayerUseSkill();
        this._skillCroutine = StartCoroutine(this.Skill_Croutine());
    }

    private IEnumerator Skill_Croutine()
    {
        int counter = 0;

        while (counter < this.SkillSpawnPosition.Length)
        {
            Transform poolObject = BulletSpawner.Instance.Spawn(BulletSpawner.LightBullet_Purple, this.SkillSpawnPosition[counter], Quaternion.identity);
            poolObject.SetParent(CameraManager.Instance.LeftCornerOfCamera);
            poolObject.localPosition = this.SkillSpawnPosition[counter];
            poolObject.gameObject.SetActive(true);

            counter++;

            yield return new WaitForSeconds(0.1f);
        }

    }

}
