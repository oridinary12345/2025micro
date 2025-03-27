using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CFX_Demo : MonoBehaviour
{
	public bool orderedSpawns = true;

	public float step = 1f;

	public float range = 5f;

	private float order = -5f;

	public Material groundMat;

	public Material waterMat;

	public GameObject[] ParticleExamples;

	private Dictionary<string, float> ParticlesYOffsetD = new Dictionary<string, float>
	{
		{
			"CFX_ElectricGround",
			0.15f
		},
		{
			"CFX_ElectricityBall",
			1f
		},
		{
			"CFX_ElectricityBolt",
			1f
		},
		{
			"CFX_Explosion",
			2f
		},
		{
			"CFX_SmallExplosion",
			1.5f
		},
		{
			"CFX_SmokeExplosion",
			2.5f
		},
		{
			"CFX_Flame",
			1f
		},
		{
			"CFX_DoubleFlame",
			1f
		},
		{
			"CFX_Hit",
			1f
		},
		{
			"CFX_CircularLightWall",
			0.05f
		},
		{
			"CFX_LightWall",
			0.05f
		},
		{
			"CFX_Flash",
			2f
		},
		{
			"CFX_Poof",
			1.5f
		},
		{
			"CFX_Virus",
			1f
		},
		{
			"CFX_SmokePuffs",
			2f
		},
		{
			"CFX_Slash",
			1f
		},
		{
			"CFX_Splash",
			0.05f
		},
		{
			"CFX_Fountain",
			0.05f
		},
		{
			"CFX_Ripple",
			0.05f
		},
		{
			"CFX_Magic",
			2f
		},
		{
			"CFX_SoftStar",
			1f
		},
		{
			"CFX_SpikyAura_Sphere",
			1f
		},
		{
			"CFX_Firework",
			2.4f
		},
		{
			"CFX_GroundA",
			0.05f
		}
	};

	private int exampleIndex;

	private string randomSpawnsDelay = "0.5";

	private bool randomSpawns;

	private bool slowMo;

	private GameObject spawnParticle()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(ParticleExamples[exampleIndex]);
		gameObject.SetActive( true);
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			gameObject.transform.GetChild(i).gameObject.SetActive( true);
		}
		float y = 0f;
		foreach (KeyValuePair<string, float> item in ParticlesYOffsetD)
		{
			if (gameObject.name.StartsWith(item.Key))
			{
				y = item.Value;
				break;
			}
		}
		gameObject.transform.position = new Vector3(0f, y, 0f);
		return gameObject;
	}

	private void OnGUI()
	{
		GUILayout.BeginArea(new Rect(5f, 20f, Screen.width - 10, 30f));
		GUILayout.BeginHorizontal();
		GUILayout.Label("Effect", GUILayout.Width(50f));
		if (GUILayout.Button("<", GUILayout.Width(20f)))
		{
			prevParticle();
		}
		GUILayout.Label(ParticleExamples[exampleIndex].name, GUILayout.Width(190f));
		if (GUILayout.Button(">", GUILayout.Width(20f)))
		{
			nextParticle();
		}
		GUILayout.Label("Click on the ground to spawn selected particles");
		if (GUILayout.Button((!CFX_Demo_RotateCamera.rotating) ? "Rotate Camera" : "Pause Camera", GUILayout.Width(140f)))
		{
			CFX_Demo_RotateCamera.rotating = !CFX_Demo_RotateCamera.rotating;
		}
		if (GUILayout.Button((!randomSpawns) ? "Start UnityEngine.Random Spawns" : "Stop UnityEngine.Random Spawns", GUILayout.Width(140f)))
		{
			randomSpawns = !randomSpawns;
			if (randomSpawns)
			{
				StartCoroutine("RandomSpawnsCoroutine");
			}
			else
			{
				StopCoroutine("RandomSpawnsCoroutine");
			}
		}
		randomSpawnsDelay = GUILayout.TextField(randomSpawnsDelay, 10, GUILayout.Width(42f));
		randomSpawnsDelay = Regex.Replace(randomSpawnsDelay, "[^0-9.]", string.Empty);
		if (GUILayout.Button((!GetComponent<Renderer>().enabled) ? "Show Ground" : "Hide Ground", GUILayout.Width(90f)))
		{
			GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
		}
		if (GUILayout.Button((!slowMo) ? "Slow Motion" : "Normal Speed", GUILayout.Width(100f)))
		{
			slowMo = !slowMo;
			if (slowMo)
			{
				Time.timeScale = 0.33f;
			}
			else
			{
				Time.timeScale = 1f;
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	private IEnumerator RandomSpawnsCoroutine()
	{
		while (true)
		{
			GameObject particles = spawnParticle();
			if (orderedSpawns)
			{
				Transform transform = particles.transform;
				Vector3 position = base.transform.position;
				float x = order;
				Vector3 position2 = particles.transform.position;
				transform.position = position + new Vector3(x, position2.y, 0f);
				order -= step;
				if (order < 0f - range)
				{
					order = range;
				}
			}
			else
			{
				Transform transform2 = particles.transform;
				Vector3 a = base.transform.position + new Vector3(UnityEngine.Random.Range(0f - range, range), 0f, UnityEngine.Random.Range(0f - range, range));
				Vector3 position3 = particles.transform.position;
				transform2.position = a + new Vector3(0f, position3.y, 0f);
			}
			yield return new WaitForSeconds(float.Parse(randomSpawnsDelay));
		}
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
		{
			prevParticle();
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
		{
			nextParticle();
		}
	}

	private void prevParticle()
	{
		exampleIndex--;
		if (exampleIndex < 0)
		{
			exampleIndex = ParticleExamples.Length - 1;
		}
		if (ParticleExamples[exampleIndex].name.Contains("Splash") || ParticleExamples[exampleIndex].name == "CFX_Ripple" || ParticleExamples[exampleIndex].name == "CFX_Fountain")
		{
			GetComponent<Renderer>().material = waterMat;
		}
		else
		{
			GetComponent<Renderer>().material = groundMat;
		}
	}

	private void nextParticle()
	{
		exampleIndex++;
		if (exampleIndex >= ParticleExamples.Length)
		{
			exampleIndex = 0;
		}
		if (ParticleExamples[exampleIndex].name.Contains("Splash") || ParticleExamples[exampleIndex].name == "CFX_Ripple" || ParticleExamples[exampleIndex].name == "CFX_Fountain")
		{
			GetComponent<Renderer>().material = waterMat;
		}
		else
		{
			GetComponent<Renderer>().material = groundMat;
		}
	}
}