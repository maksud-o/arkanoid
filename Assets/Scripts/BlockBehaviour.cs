using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class BlockBehaviour : MonoBehaviour
{
    public static event Action<int> OnBlockDestroy;

    [SerializeField] private string _ballTag;

    [Header("Block Settings")]
    [Tooltip("Can be null")][SerializeField] private List<Sprite> _multipleHitsSprites;
    [SerializeField] private int _scoreGiven = 1;

    private SpriteRenderer _spriteRenderer;
    private bool _isMultiHit = true;
    private int _hitStage = 0;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_multipleHitsSprites?.Count == 0)
        {
            _isMultiHit = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(_ballTag))
        {
            if (!_isMultiHit || !IsAlive())
            {
                gameObject.SetActive(false);
                OnBlockDestroy?.Invoke(_scoreGiven);
            }
        }
    }

    private bool IsAlive()
    {
        if (_hitStage < _multipleHitsSprites.Count)
        {
            _spriteRenderer.sprite = _multipleHitsSprites[_hitStage];
        }
        else
        {
            return false;
        }
        _hitStage++;
        return true;
    }
}
