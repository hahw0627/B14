using System.Linq;
using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Common.Springs;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using UnityEngine;

namespace _10._Externals.HeroEditor4D.Common.Scripts.CharacterScripts
{
	/// <summary>
	/// Used to play animations.
	/// </summary>
	public class AnimationManager : MonoBehaviour
	{
		public Character4D Character;
		public Animator Animator;
		private static readonly int Action = Animator.StringToHash("Action");
		private static readonly int State = Animator.StringToHash("State");
		private static readonly int Slash1H1 = Animator.StringToHash("Slash1H");
		private static readonly int Slash2H1 = Animator.StringToHash("Slash2H");
		private static readonly int Jab1 = Animator.StringToHash("Jab");
		private static readonly int HeavySlash1H1 = Animator.StringToHash("HeavySlash1H");
		private static readonly int FastStab1 = Animator.StringToHash("FastStab");
		private static readonly int ShotBow1 = Animator.StringToHash("ShotBow");
		private static readonly int Evade1 = Animator.StringToHash("Evade");
		private static readonly int TwoHanded = Animator.StringToHash("TwoHanded");
		private static readonly int Hit1 = Animator.StringToHash("Hit");
		private static readonly int WeaponType1 = Animator.StringToHash("WeaponType");
		private static readonly int Fire1 = Animator.StringToHash("Fire");
		private static readonly int SecondaryShot1 = Animator.StringToHash("SecondaryShot");

		public bool IsAction
        {
            get => Animator.GetBool(Action);
            private set => Animator.SetBool(Action, value);
        }

		/// <summary>
		/// Set animation parameter State that controls transition.
		/// </summary>
		public void SetState(CharacterState state)
		{
			Animator.SetInteger(State, (int) state);
		}

	    /// <summary>
        /// Play Attack animation according to selected weapon.
        /// </summary>
		public void Attack()
		{
			switch (Character.WeaponType)
			{
				case WeaponType.Melee1H:
				case WeaponType.Melee2H:
					Slash1H();
					break;
				case WeaponType.Bow:
					ShotBow();
					break;
				case WeaponType.Paired:
				case WeaponType.Crossbow:
				case WeaponType.Firearm1H:
				case WeaponType.Firearm2H:
				case WeaponType.Throwable:
				default:
					//throw new NotImplementedException("This feature may be implemented in next updates.");
                    Slash1H();
                    break;
			}
		}

        /// <summary>
        /// Play Slash1H animation.
        /// </summary>
		public void Slash1H()
		{
			Animator.SetTrigger(Slash1H1);
            IsAction = true;
        }

	    /// <summary>
	    /// Play Slash2H animation.
	    /// </summary>
	    public void Slash2H()
	    {
	        Animator.SetTrigger(Slash2H1);
            IsAction = true;
		}

	    public void Slash(bool twoHanded)
	    {
	        Animator.SetTrigger(twoHanded ? "Slash2H" : "Slash1H");
            IsAction = true;
		}

        /// <summary>
        /// Play Jab animation.
        /// </summary>
        public void Jab()
        {
            Animator.SetTrigger(Jab1);
            IsAction = true;
		}

        /// <summary>
        /// Play Slash1H animation.
        /// </summary>
        public void HeavySlash1H()
        {
            Animator.SetTrigger(HeavySlash1H1);
            IsAction = true;
		}
        
	    /// <summary>
	    /// Play PowerStab animation.
	    /// </summary>
	    public void FastStab()
	    {
	        Animator.SetTrigger(FastStab1);
            IsAction = true;
		}

        /// <summary>
        /// Play Shot animation (bow).
        /// </summary>
		public void ShotBow()
		{
			Animator.SetTrigger(ShotBow1);
            IsAction = true;
		}

        /// <summary>
        /// Play Death animation.
        /// </summary>
	    public void Die()
	    {
	        SetState(CharacterState.Death);
	    }

        /// <summary>
        /// Play Hit animation. This will just scale character up and down.
        /// Hit will not break currently playing animation, for example you can Hit character while it's playing Attack animation.
        /// </summary>
	    public void Hit()
	    {
	        Animator.SetTrigger(Hit1);
        }

	    public void ShieldBlock()
	    {
	        SetState(CharacterState.ShieldBlock);
        }

	    public void WeaponBlock()
	    {
	        SetState(CharacterState.WeaponBlock);
	    }

	    public void Evade()
	    {
	        Animator.SetTrigger(Evade1);
        }

	    public void SetTwoHanded(bool twoHanded)
	    {
	        Animator.SetBool(TwoHanded, twoHanded);
        }

	    public void SetWeaponType(WeaponType weaponType)
	    {
	        Animator.SetInteger(WeaponType1, (int) weaponType);
	    }

        public void Fire()
        {
            Animator.SetTrigger(Fire1);
            IsAction = true;
        }

		public void SecondaryShot()
        {
            Animator.SetTrigger(SecondaryShot1);
            IsAction = true;
        }

        public void CrossbowShot()
        {
            Animator.SetTrigger(Fire1);
            IsAction = true;

            if (Character.Parts[0].CompositeWeapon == null) return;
            var loaded = Character.Front.CompositeWeapon.Single(i => i.name == "Side");
            var empty = Character.Front.CompositeWeapon.Single(i => i.name == "SideEmpty");

            foreach (var part in Character.Parts)
            {
	            part.PrimaryWeaponRenderer.sprite = part.PrimaryWeaponRenderer.sprite == loaded ? empty : loaded;
            }
        }

		/// <summary>
		/// Alternative way to Hit character (with a script).
		/// </summary>
		public void Spring()
	    {
            ScaleSpring.Begin(this, 1f, 1.1f, 40, 2);
        }
    }
}