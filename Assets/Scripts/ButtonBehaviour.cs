using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
 {
     private TextMeshProUGUI _txt;
     private Color _baseColor;
     //private Button _btn;
     //private bool _interactableDelay;

     private void Start ()
     {
         _txt = GetComponentInChildren<TextMeshProUGUI>();
         _baseColor = _txt.color;
         //_btn = gameObject.GetComponent<Button> ();
         //_interactableDelay = _btn.interactable;
     }

     public void OnPointerEnter (PointerEventData eventData)
     {
         //Debug.Log("In OnPointerEnter");
         //_txt.color = Color.magenta;
         _txt.CrossFadeColor(Color.magenta, .2f, true, false);
         /*if (_btn.interactable) {
             _txt.color = _baseColor * _btn.colors.highlightedColor * _btn.colors.colorMultiplier;
         } else {
             _txt.color = _baseColor * _btn.colors.disabledColor * _btn.colors.colorMultiplier;
         }*/
     }
 
     public void OnPointerExit (PointerEventData eventData)
     {
         //Debug.Log("In OnPointerExit");
         //_txt.color = _baseColor;
         _txt.CrossFadeColor(_baseColor, .2f, true, false);
         /*if (_btn.interactable) {
             _txt.color = _baseColor * _btn.colors.normalColor * _btn.colors.colorMultiplier;
         } else {
             _txt.color = _baseColor * _btn.colors.disabledColor * _btn.colors.colorMultiplier;
         }*/
     }
 
     public void OnPointerDown (PointerEventData eventData)
     {
         /*if (_btn.interactable) {
             _txt.color = _baseColor * _btn.colors.pressedColor * _btn.colors.colorMultiplier;
         } else {
             _txt.color = _baseColor * _btn.colors.disabledColor * _btn.colors.colorMultiplier;
         }*/
     }
 
     public void OnPointerUp (PointerEventData eventData)
     {
         /*if (_btn.interactable) {
             _txt.color = _baseColor * _btn.colors.highlightedColor * _btn.colors.colorMultiplier;
         } else {
             _txt.color = _baseColor * _btn.colors.disabledColor * _btn.colors.colorMultiplier;
         }*/
     }
 }
