using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Stom.UI
{
    [RequireComponent(typeof(Animator))]
    public class SimpleButton : UIBehaviour, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
    {
        [Serializable]
        public class ButtonClickedEvent : UnityEvent { }

        [SerializeField]
        private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

        private Animator animator;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }

        public ButtonClickedEvent onClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            Unpress();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            Press();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            m_OnClick.Invoke();
        }


        private void Press()
        {
            animator.SetBool("Press", true);

            if (!IsActive())
                return;
        }

        private void Unpress()
        {
            animator.SetBool("Press", false);

            if (!IsActive())
                return;
        }
    }
}
