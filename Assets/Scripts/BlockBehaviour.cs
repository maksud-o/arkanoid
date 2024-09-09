using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BlockBehaviour : MonoBehaviour
{
    [SerializeField] private string _ballTag;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(_ballTag))
        {
            gameObject.SetActive(false);
        }
    }
}
