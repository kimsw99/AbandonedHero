using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class DragAndDrop : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    Canvas canvas;
    RectTransform rect;
    PointerEventData pointerEventData;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData) // Ŭ�� �� �ߵ�
    {
        DOTween.Kill(rect); // ���� ������ ���

        rect.DORotate(Vector3.zero, 0.3f); // ���������� 
        pointerEventData = eventData;
    }

    public void OnDrag(PointerEventData eventData) 
    {
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor; //���콺 ��ǥ ������� �ϴ°�
    }

    public void StopDrag()
    {
        OnEndDrag(pointerEventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        pointerEventData = eventData;
    }
        /*
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (clickRock) return;

            DOTween.Kill(rect);

            isCheckPosition = true;
            rigid.velocity = Vector2.zero;

            PlayManager.instance.uiManager.battleUI.pickedCardInfo = this;

            RecentingPosition();
            StartCoroutine(CheckPosition());
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (clickRock) return;
            rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isCheckPosition = false;
            PlayManager.instance.uiManager.battleUI.pickedCardInfo = null;

            if (clickRock) return;

            #region ȭ�� �ٱ����� ������
            var tmpPosition = rect.anchoredPosition + (recentPosition[0] - recentPosition[1]);
            if (tmpPosition.x > 960) tmpPosition = new Vector2(960, tmpPosition.y);
            if (tmpPosition.x < -960) tmpPosition = new Vector2(-960, tmpPosition.y);
            if (tmpPosition.y > 540) tmpPosition = new Vector2(tmpPosition.x, 540);
            if (tmpPosition.y < -540) tmpPosition = new Vector2(tmpPosition.x, -540);

            tmpPosition = new Vector3(tmpPosition.x, tmpPosition.y, 0);

            #endregion

            rect.DOAnchorPos(tmpPosition, 1);
            rect.DORotate(new Vector3(0, 0, Random.Range(-45, 45)), 1f).OnComplete(() => {

                if (onTable && PlayManager.instance.uiManager.battleUI.myDeck.Count < 2) //ī�� �ߵ�
                {
                    CardReturn();
                    PlayManager.instance.uiManager.battleUI.SelectMyDeck(this);
                    PlayManager.instance.uiManager.battleUI.ActivateCard(this);
                }
            });

        }

        void RecentingPosition()
        {
            recentPosition[0] = rect.anchoredPosition;
            recentPosition[1] = rect.anchoredPosition;
        }

        IEnumerator CheckPosition()
        {
            if (!isCheckPosition) yield break;
            recentPosition[1] = recentPosition[0];
            recentPosition[0] = rect.anchoredPosition;

            yield return new WaitForSeconds(0.05f);

            StartCoroutine(CheckPosition());
        }
        */
    }
