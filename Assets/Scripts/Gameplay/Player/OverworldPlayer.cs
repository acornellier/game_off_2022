using System;
using UnityEngine;

public class OverworldPlayer : MonoBehaviour, IPersistableData
{
    [SerializeField] TopDownPlayer _player;
    [SerializeField] Interactors _interactors;

    void Awake()
    {
        _player.onInteract += Interact;
    }

    void Interact()
    {
        GetInteractor()?.Interact();
    }

    PlayerInteractor GetInteractor()
    {
        if (_player.facingDirection.x >= 0)
        {
            if (_player.facingDirection.y >= 0)
                return _player.facingDirection.x > _player.facingDirection.y
                    ? _interactors.right
                    : _interactors.up;

            return _player.facingDirection.x > -_player.facingDirection.y
                ? _interactors.right
                : _interactors.down;
        }

        if (_player.facingDirection.y >= 0)
            return _player.facingDirection.x < -_player.facingDirection.y
                ? _interactors.left
                : _interactors.up;

        return _player.facingDirection.x < _player.facingDirection.y
            ? _interactors.left
            : _interactors.down;
    }

    public void Load(PersistentData data)
    {
        if (data.playerPosition != null)
            _player.transform.position = data.playerPosition.ToVector3();
    }

    public void Save(PersistentData data)
    {
        data.playerPosition = new Vector3Json(_player.transform.position);
    }

    [Serializable]
    class Interactors
    {
        public PlayerInteractor up;
        public PlayerInteractor right;
        public PlayerInteractor down;
        public PlayerInteractor left;
    }
}