using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class ProvidedModificatorView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Color positiveColor;
        [SerializeField] private Color negativeColor;
        [SerializeField] private Color neutralColor;

        private void SetImageColor(Color color)
        {
            if (image == null)
            {
                Debug.Log("[LogicalModificatorView] Image is null!");
                return;
            }
            image.color = color;
        }
        
        public void SetPositive() => SetImageColor(positiveColor);
        public void SetNegative() => SetImageColor(negativeColor);
        public void SetNeutral() => SetImageColor(neutralColor);
        
    }
}