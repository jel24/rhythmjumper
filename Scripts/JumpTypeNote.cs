using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTypeNote : MonoBehaviour {

	private JumpType jumpType;

	public void SetJumpType(JumpType j){
		jumpType = j;
	}

	public JumpType GetJumpType(){
		return jumpType;
	}


}
