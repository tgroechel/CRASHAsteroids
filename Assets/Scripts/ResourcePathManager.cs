﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crash;

public class ResourcePathManager : Singleton<ResourcePathManager>
{
    public static string JunkMaterialPath = "junk";
    public static string navMeshLink = "NM_Link";
    public static string navMesh = "NavMesh";
    public static string projectilesFolder = "Projectiles/Projectiles/";
    public static string projectilesThumbnailsFolder = "Projectile Thumbnails/";
    public static string materialsFolder = "Materials/";
    public static string bulletHighlightMaterial = "SelectedBulletHighlight";
    public static string bulletDeHighlightMaterial = "SelectedBulletDeHighlight";
    public static string bullet1 = "vfx_Projectile_BlackHole01_Orange";
    public static string prefabsFolder = "Prefabs/";
    public static string radialMenuButton = "RadialMenuButton";
    //public static List<string> bullets = new List<string>() { "vfx_Projectile_BlackHole01_Orange", "vfx_Projectile_ElectricBall01_Blue", "vfx_Projectile_Fireball03_Orange", "vfx_Projectile_IceSpike01_Blue", "vfx_Projectile_Orb02_Green", "vfx_Projectile_Firefrost01_Purple", "vfx_Projectile_Trail05_Pink", "vfx_Projectile_WaterBall01_Blue" };
    public static List<string> bullets = new List<string>() { "vfx_Projectile_BlackHole01_Orange", "vfx_Projectile_ElectricBall01_Blue","vfx_Projectile_IceSpike01_Blue"};
    // public static string bullet1 = "vfx_Projectile_BlackHole01_Orange_Mobile Variant";
    public static string blackhole = "Projectiles/Projectiles/vfx_Projectile_BlackHole01_Purple";
    public static string bossProjectile = "Projectiles/Projectiles/vfx_Projectile_BlackHole01_Orange_Simple";

    public static string bossLaser = "Lasers/Laser beam 3";
    public static string aimLaser = "Lasers/AimLaser";
    public static string minion = "Spider_Mini";
    public static string boss = "Spider_Hvy";
    public static string kuri = "Kuri";


    public static string bossGunFire = "Projectiles/vfx_Muzzle_Orb05_Orange";
}
