using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class CharacterLiveIdle : State
{
	new public CharacterLive Layer { get { return ((CharacterLive)base.Layer); } }
}
