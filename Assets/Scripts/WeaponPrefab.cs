using System.Collections.Generic;
using UnityEngine;

public class WeaponPrefab : MonoBehaviour
{
	public Sprite WeaponSprite;

	public Sprite WeaponSpriteBroken;

	public Sprite WeaponSpriteUI;

	public Sprite WeaponSpriteBrokenUI;

	public Sprite WeaponExtraSprite;

	public Sprite WeaponExtraSpriteBroken;

	public Sprite WeaponExtraAnimationSprite;

	public Sprite WeaponExtraSpriteUI;

	public Sprite WeaponShapeSprite;

	public GameObject ImpactFX;

	public GameObject Projectile;

	public GameObject ProjectileLandingFX;

	public GameObject ShapeField;

	public List<PolygonCollider2D> Colliders;
}