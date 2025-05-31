using Interface;
using JSON;
using UnityEngine;

public class PlayerSaver : IService
{
    private PlayerDataIO _saveHandler;
    private Player.Player _player;

    public void Initialize()
    {
        _saveHandler = ServicesLocator.ServiceLocator.Current.Get<PlayerDataIO>();
    }

    public void SetPlayer(Player.Player player)
    {
        _player = player;
    }

    public void Save()
    {
        _saveHandler.Save(_player.Data);
        Debug.Log("[PlayerSaver] Saved player data.");
    }

    public void Load()
    {
        var loadedData = _saveHandler.Load();
        _player.SetData(loadedData);
        Debug.Log("[PlayerSaver] Loaded player data.");
    }
}