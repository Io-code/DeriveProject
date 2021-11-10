using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwimWen : MonoBehaviour
{
	public float minimalDistance, minimalSize;
	public float maximalDistance, maximalSize;
	public RectTransform[] wen;
	public Camera uiCamera;
	public float borderSize;
	private Vector2[] originalSize;
	public string[] animationState;

	private void Awake()
	{
		originalSize = new Vector2[2];
		originalSize[0] = wen[0].localScale;
		originalSize[1] = wen[1].localScale;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!wen[0].gameObject.active && !wen[1].gameObject.active) return;
		if (collision.tag == "Player")
		{
			wen[collision.GetComponent<Controller>().pc.playerNumber - 1].gameObject.SetActive(false);
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{	
		if (collision.tag == "Player")
		{
			StartCoroutine(PlayerDetection(collision.transform));
		}
	}

	private IEnumerator PlayerDetection(Transform pc)
	{
		string currentState = "";
		Vector3 lastPosition = Vector3.zero;
				//Afficher une loupe
		wen[pc.gameObject.GetComponent<Controller>().pc.playerNumber - 1].gameObject.SetActive(true);
		while (wen[pc.gameObject.GetComponent<Controller>().pc.playerNumber - 1].gameObject.active)
		{
				//La rotationner correctement
			Vector3 toPosition = pc.transform.position;
			Vector3 fromPosition = Camera.main.transform.position;
			fromPosition.z = 0f;
			Vector3 dir = (toPosition - fromPosition).normalized;
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			if (angle < 0) angle += 360;
			wen[pc.gameObject.GetComponent<Controller>().pc.playerNumber - 1].localEulerAngles = new Vector3(0, 0, angle);
			Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(pc.position);
			bool isOffScreen = targetPositionScreenPoint.x <= borderSize || targetPositionScreenPoint.x >= Screen.width - borderSize || targetPositionScreenPoint.y <= borderSize || targetPositionScreenPoint.y >= Screen.height - borderSize;

				//Placer correctement
			Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
			if (isOffScreen)
			{
				if (cappedTargetScreenPosition.x <= borderSize) cappedTargetScreenPosition.x = borderSize;
				if (cappedTargetScreenPosition.x >= Screen.width - borderSize) cappedTargetScreenPosition.x = Screen.width - borderSize;
				if (cappedTargetScreenPosition.y <= borderSize) cappedTargetScreenPosition.y = borderSize;
				if (cappedTargetScreenPosition.y >= Screen.height - borderSize) cappedTargetScreenPosition.y = Screen.height - borderSize;

				Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
				wen[pc.gameObject.GetComponent<Controller>().pc.playerNumber - 1].position = pointerWorldPosition;
				wen[pc.gameObject.GetComponent<Controller>().pc.playerNumber - 1].localPosition = new Vector3(wen[pc.gameObject.GetComponent<Controller>().pc.playerNumber - 1].localPosition.x, wen[pc.gameObject.GetComponent<Controller>().pc.playerNumber - 1].localPosition.y, 0f);
			}

				//Definir en taille entre les deux bornes en la convertissant en pourcentage
			float size = 0;
			size = Mathf.Clamp(Vector3.Distance(uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition), pc.position) - minimalDistance, 0, maximalDistance - minimalDistance);
			size /= (maximalDistance - minimalDistance);
			size = Mathf.Clamp(size, minimalSize, maximalSize) * -1 + minimalSize + maximalSize;

				//Modifier la taille
			wen[pc.GetComponent<Controller>().pc.playerNumber - 1].localScale = originalSize[pc.GetComponent<Controller>().pc.playerNumber - 1] * size;

				//Verifier s'il y a la nage
				//Changer l'animation en conséquence
			string newState;
			if (Vector3.Distance(pc.position, lastPosition) != 0) newState = animationState[pc.GetComponent<Controller>().pc.playerNumber - 1];
			else newState = animationState[pc.GetComponent<Controller>().pc.playerNumber + 1];
			lastPosition = pc.position;
			if (currentState != newState)
			{
				currentState = newState;
			}
			yield return new WaitForFixedUpdate();
		}
		wen[pc.GetComponent<Controller>().pc.playerNumber - 1].localScale = originalSize[pc.GetComponent<Controller>().pc.playerNumber - 1];
	}
	/*private void ChangeAnimationState(string newState)
	{
		if (currentState == newState) return;

		currentState = newState;
	}*/
}
