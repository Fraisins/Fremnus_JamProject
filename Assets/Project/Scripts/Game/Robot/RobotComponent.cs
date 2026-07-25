using UnityEngine;

namespace Project.Game.Robot
{
    public class RobotComponent : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer faceRenderer;

        public void SetFaceTexture(Sprite spriteTexture)
        {
            if (faceRenderer == null)
                return;
            faceRenderer.sprite = spriteTexture;
        }
    }
}