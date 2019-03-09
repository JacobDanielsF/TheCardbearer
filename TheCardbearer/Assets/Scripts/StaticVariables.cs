using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticVariables
{
	private static int sceneEnemies;
	
	public static int SceneEnemies 
    {
        get 
        {
            return sceneEnemies;
        }
        set 
        {
            sceneEnemies = value;
        }
    }
}
