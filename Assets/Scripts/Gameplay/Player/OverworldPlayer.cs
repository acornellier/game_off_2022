using UnityEngine;

public class OverworldPlayer : MonoBehaviour, IPersistableData
{
    [SerializeField] TopDownPlayer _player;

    public void Load(PersistentData data)
    {
        if (data.playerPosition != null)
            _player.transform.position = data.playerPosition.ToVector3();
    }

    public void Save(PersistentData data)
    {
        data.playerPosition = new Vector3Json(_player.transform.position);
    }
}