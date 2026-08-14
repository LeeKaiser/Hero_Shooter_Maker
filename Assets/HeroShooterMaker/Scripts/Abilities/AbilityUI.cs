using UnityEngine;

/*
Ability UI
abstract parent for User Interface for the ability
*/
namespace HeroShooterMaker.Abilities
{
    public abstract class AbilityUI : MonoBehaviour
    {
        public Vector2 positionChangeOnOverlap;

        public Ability AbilityReference;
        public abstract void Initialize();

        public abstract void UpdateUI();

        public void ShiftIfOverlapping(RectTransform otherUI)
        {
            if (otherUI.gameObject == this.gameObject)
            {
                return;
            }

            RectTransform thisTransform = GetComponent<RectTransform>();
            Vector3[] thisCorners = new Vector3[4];
            Vector3[] otherCorners = new Vector3[4];

            thisTransform.GetWorldCorners(thisCorners);
            otherUI.GetWorldCorners(otherCorners);

            Rect thisRect = new Rect(thisCorners[0], thisCorners[2] - thisCorners[0]);
            Rect otherRect = new Rect(otherCorners[0], otherCorners[2] - otherCorners[0]);

            if (thisRect.Overlaps(otherRect))
            {
                //translate elsewhere
                thisTransform.anchoredPosition += positionChangeOnOverlap;
            }
        }
    }
    
}
