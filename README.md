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

## Running Spatial Understanding with Minions Scene ('Sid' scene)

1. Run the scene and wait for the spatial understanding walls to get loaded (walls will stop loading visually speaking).
2. Make sure you are in the **Game Window** of Unity before pressing the buttons mentioned below.
2. __Press the 'C' button__ for creating navmesh objects and attaching them to each of the walls.
3. __Press the 'B' button__ to bake the navmesh on the walls and the floor.
4. __Press the 'L' button__ to create navmesh links between the walls and the floor.
5. You are all set to use the scene now!
6. Click on any location in the scene using the __left mouse button__ and the hermit and spider will go to that point.
7. If the point is not reachable, they will go to their __nearest reachable points__.
7. The paths followed by the minions is shown in the scene using *line renderers*.
8. Remember, the __spider can walk on any surface__ but the __hermit can only walk on the floor__.
9. For the spider and the hermit, you can toggle between __follow player__ and __don't follow player__ modes by checking/unchecking the __follow player__ checkbox on the `Minion Follow Player Script` component of that particular object.
10. You can also shoot at the minions to kill them. See the *radial menu behaviour section* above. 

Here is an example.

![nmvWdBHHc9](https://user-images.githubusercontent.com/18630586/78309670-7c0e8e80-7500-11ea-92d0-34413b0e0b6a.gif)

## Boss Activation/Deactivation Cycle

1. Every enemy is in __Locomotion__ state at game start.
2. To make use of activation/deactivation behaviour, set the `isBoss` bool on `HealthManager` component of the respective enemy to `true` by checking the box.
3. Then, set the value of the `Resurrection Time` float on `HealthManager` component. This is the time in seconds for which the boss is deactivated.
4. While the boss is deactivated, you can prevent certain actions such as navigation and shooting at the player by making use of the `Activate` bool parameter on the `Animator` component of the boss. Just check if `GetComponent<Animator>.getBool("Activate")` is `true` and only then should you execute these tasks.
5. The boss will automatically go back to activated state after his `Resurrection Time` is complete and his health will be restored to `MAXHEALTH`. 
