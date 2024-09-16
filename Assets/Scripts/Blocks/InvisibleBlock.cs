namespace Arkanoid.Blocks
{
    public class InvisibleBlock : Block
    {
        #region Unity lifecycle

        private new void Awake()
        {
            base.Awake();
            SpriteRenderer.enabled = false;
        }

        private new void OnCollisionEnter2D()
        {
            if (!SpriteRenderer.enabled)
            {
                SpriteRenderer.enabled = true;
                return;
            }
            base.OnCollisionEnter2D();
        }

        #endregion
    }
}