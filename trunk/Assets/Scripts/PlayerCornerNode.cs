﻿using UnityEngine;
using System.Collections;

public class PlayerCornerNode : NetworkNode {

	private int curFilesInRow = 0;
	public float cooldownTime;

	public Transform pointsDisplay;
	public float pointFlyingSpeed;
	public float pointRotationFactor = 50;
	public float pointScaleFactor = 3;

	override public void RecieveFile(File aFile, NetworkNode aFromNode)
    {
    }

    override public void HandleFile(File aFile, NetworkNode aFromNode)
    {
        if(aFile is Virus)
		{
			transform.parent.GetComponent<PlayerCorner>().RemoveScore(10);
			(aFile as Virus).DestroyJuicyVirus();
		}
		else if (aFile.DidPoint == false)
		{
			aFile.DidPoint = true;
			StartCoroutine(DoPointEffect(aFile));
		}
    }

	private IEnumerator DoPointEffect(File file)
	{
		curFilesInRow++;

		transform.parent.GetComponent<PlayerCorner>().AddScore(1);
		if(curFilesInRow > 3)
		{
			transform.parent.GetComponent<PlayerCorner>().AddScore(2);
			iTween.ShakePosition(Game.Instance.gameObject, new Vector3(0.15f,0.15f,0.15f), 1f);
		}
		if(curFilesInRow > 5)
		{
			transform.parent.GetComponent<PlayerCorner>().AddScore(2);
			iTween.ShakePosition(Game.Instance.gameObject, new Vector3(0.7f,0.7f,0.7f), 1f);
		}

		Hashtable ht = new Hashtable();
		ht.Add("position", pointsDisplay.position);
		ht.Add("time", pointFlyingSpeed);
		ht.Add("EaseType", "easeInQuad");
		iTween.MoveTo(file.gameObject, ht);
		iTween.ScaleTo(file.gameObject, file.transform.localScale*pointScaleFactor, pointFlyingSpeed);
		iTween.ColorTo(file.gameObject, new Color(1,1,1,0), pointFlyingSpeed/3);
		iTween.RotateBy(file.gameObject, new Vector3(0,0,Random.Range(-pointRotationFactor,pointRotationFactor)), 
		                									pointFlyingSpeed);

		yield return new WaitForSeconds(pointFlyingSpeed);
		file.DestroyJuicy(true, curFilesInRow); 

		yield return new WaitForSeconds(2f);
		curFilesInRow--;
	}
}