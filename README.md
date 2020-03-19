# CRASHAsteroids

## Bullet Settings

1. Any GameObject can be a bullet, but the GameObject must have `Collider` and `RigidBody` components attached to it.  
2. Bullet prefabs must be present in the `Resources/Projectiles` folder. 
3. Make sure you add the `HitEnemy.cs` script as a component to every bullet prefab.
4. Damage incurred by each bullet upon hitting enemy can be set up in the `bulletDamage` public variable of `HitEnemy.cs`.

## Enemy Settings

1. Any GameObject can be an enemy, but the GameObject must have `Collider` and `RigidBody` components attached to it.
2. Make sure you add the `HealthManager.cs` script as a component to every enemy prefab.
3. Each enemy initially has health equal to 100. This value can be changed in the  `health` public variable of `HealthManager.cs` script.

## Bullet Hitting Enemy Behaviour

1. Each bullet prefab by default has a `bulletDamage` value of 10.
2. Upon hitting the enemy, the enemy changes color for a brief period and its health decreases.
3. Since the health default value for an enemy is 100, you need 10 bullets to kill the enemy.
4. When the health of an enemy becomes 0 (or <0), the enemy disappears.
5. Every bullet hitting an enemy also disappears.

Here is an example.  

![ShootingEnemy](https://user-images.githubusercontent.com/18630586/75085486-c3a40280-54de-11ea-93ab-08e858cbcf7e.gif)

## Bullet hitting Non-Enemy Behaviour

1. The bullet simply disappears.

## Shooting Settings

1. *Right Mouse Click or Air Tap*: Shoot the bullet in the direction of camera gaze pointer.
2. *Left Mouse Click or Air Tap*: Reload the magazine of bullets.

## Radial Menu Behaviour

1. The radial menu is activated only when the __right palm__ is facing the player.
2. The radial menu appears on the __left side__ of the __right palm__.
3. The user has an arsenal of __8 types of bullets__ (each with its own damage and speed characteristics, this needs to be updated). 
4. The user can choose a bullet type by pressing it like a button using his/her __left hand__.
5. Upon pressing a button, the user gets *audio feedback* and the *button is highlighted* to indicate selection.
6. Each bullet type has its own __shot sound__ and __hit sound__ (these will be 3D sounds, this needs to be updated).

Here is an example.

![RadialMenu](https://user-images.githubusercontent.com/18630586/77114670-27d8ba00-69ea-11ea-9a54-e0e6f3990172.gif)
