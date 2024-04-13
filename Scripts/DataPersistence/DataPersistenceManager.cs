using Patterns.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager :Singleton<DataPersistenceManager>
{
    [Header("Data Storage Config")]
    [SerializeField] private string _fileName;

    private GameData _gameData;
    private List<IDataPersistence> _dataPersistenceObjects;
    private FileDataHandler _fileDataHandler;

    protected override void Start()
    {
        base.Start();

        this._fileDataHandler = new FileDataHandler(Application.persistentDataPath, this._fileName);

        this._dataPersistenceObjects = this.FindAllDataPersistenceObjects();

        this.LoadGame();
    }

    protected override void OnEnable()
    {


        base.OnEnable();
    }

    public void NewGame()
    {
        this._gameData = new GameData();
    }

    public void LoadGame()
    {
        this._gameData = this._fileDataHandler.Load();

        if (this._gameData == null)
        {
            Debug.Log("No data was found.");
            this.NewGame();
        }

        foreach (IDataPersistence dataPersistence in this._dataPersistenceObjects)
        {
            dataPersistence.LoadGame(this._gameData);
        }

    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistence in this._dataPersistenceObjects)
        {
            dataPersistence.SaveGame(ref this._gameData);
        }

        this._fileDataHandler.Save(this._gameData);
    }

    private void OnApplicationQuit()
    {
        this.SaveGame();
    }


    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

}
