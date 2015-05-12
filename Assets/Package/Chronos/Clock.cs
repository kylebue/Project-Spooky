using System;
using UnityEngine;

namespace Chronos
{
	/// <summary>
	/// Determines how a clock combines its time scale with that of others.
	/// </summary>
	public enum ClockBlend
	{
		/// <summary>
		/// The clock's time scale is multiplied with that of others.
		/// </summary>
		Multiplicative = 0,

		/// <summary>
		/// The clock's time scale is added to that of others.
		/// </summary>
		Additive = 1
	}

	/// <summary>
	/// An abstract base component that provides common timing functionality to all types of clocks. 
	/// </summary>
	public abstract class Clock : MonoBehaviour
	{
		protected virtual void Start()
		{
			startTime = Time.unscaledTime;
			lastTimeScale = timeScale;

			OnChangedTimeScale(timeScale);
		}

		protected virtual void Update()
		{
			if (isLerping)
			{
				_localTimeScale = Mathf.Lerp(lerpFrom, lerpTo, (unscaledTime - lerpStart) / (lerpEnd - lerpStart));

				if (unscaledTime >= lerpEnd)
				{
					isLerping = false;
				}
			}

			float timeScale = this.timeScale;

			if (lastTimeScale != timeScale)
			{
				OnChangedTimeScale(timeScale);
			}

			unscaledTime += Time.unscaledDeltaTime;
			deltaTime = Time.unscaledDeltaTime * timeScale;
			fixedDeltaTime = Time.fixedDeltaTime * timeScale;
			time += deltaTime;

			lastTimeScale = timeScale;
		}

		#region Fields

		protected bool requestedCacheParent = true;
		protected float lastTimeScale;
		protected bool isLerping;
		protected float lerpStart;
		protected float lerpEnd;
		protected float lerpFrom;
		protected float lerpTo;

		#endregion

		#region Properties

		[SerializeField]
		private float _localTimeScale = 1;
		/// <summary>
		/// The scale at which the time is passing for the clock. This can be used for slow motion, acceleration, pause or even rewind effects. 
		/// </summary>
		public float localTimeScale
		{
			get { return _localTimeScale; }
			set { _localTimeScale = value; isLerping = false; }
		}

		/// <summary>
		/// The computed time scale of the clock. This value takes into consideration all of the clock's parameters (parent, paused, etc.) and multiplies their effect accordingly. 
		/// </summary>
		public float timeScale
		{
			get
			{
				if (paused)
				{
					return 0;
				}

				if (parent == null)
				{
					return localTimeScale;
				}
				else
				{
					if (parentBlend == ClockBlend.Multiplicative)
					{
						return parent.timeScale * localTimeScale;
					}
					else // if (combinationMode == ClockCombinationMode.Additive)
					{
						return parent.timeScale + localTimeScale;
					}
				}
			}
		}

		/// <summary>
		/// The time in seconds since the creation of the clock, affected by the time scale. Returns the same value if called multiple times in a single frame.
		/// Unlike Time.time, this value will not return Time.fixedTime when called inside MonoBehaviour's FixedUpdate. 
		/// </summary>
		public float time { get; protected set; }

		/// <summary>
		/// The time in seconds since the creation of the clock, regardless of the time scale. Returns the same value if called multiple times in a single frame. 
		/// </summary>
		public float unscaledTime { get; protected set; }

		/// <summary>
		/// The time in seconds it took to complete the last frame, multiplied by the time scale. Returns the same value if called multiple times in a single frame. 
		/// Unlike Time.deltaTime, this value will not return Time.fixedDeltaTime when called inside MonoBehaviour's FixedUpdate. 
		/// </summary>
		public float deltaTime { get; protected set; }

		/// <summary>
		/// The interval in seconds at which physics and other fixed frame rate updates, multiplied by the time scale.
		/// </summary>
		public float fixedDeltaTime { get; protected set; }

		/// <summary>
		/// The unscaled time in seconds between the start of the game and the creation of the clock. 
		/// </summary>
		public float startTime { get; protected set; }

		[SerializeField]
		private bool _paused;
		/// <summary>
		/// Determines whether the clock is paused. This toggle is especially useful if you want to pause a clock without having to worry about storing its previous time scale to restore it afterwards. 
		/// </summary>
		public bool paused
		{
			get { return _paused; }
			set { _paused = value; }
		}

		[SerializeField]
		private string _parentKey;
		/// <summary>
		/// The key of the parent global clock, or null for none. 
		/// </summary>
		public string parentKey
		{
			get { return _parentKey; }
			set { _parentKey = value; requestedCacheParent = true; }
		}

		protected GlobalClock _parent;
		/// <summary>
		/// The parent global clock. The parent clock will multiply its time scale with all of its children, allowing for cascading time effects.
		/// </summary>
		public GlobalClock parent
		{
			get
			{
				if (requestedCacheParent)
				{
					_parent = FindParent();
					requestedCacheParent = false;
				}

				return _parent;
			}
		}

		[SerializeField]
		private ClockBlend _parentBlend = ClockBlend.Multiplicative;
		/// <summary>
		/// Determines how the clock combines its time scale with that of its parent.
		/// </summary>
		public ClockBlend parentBlend
		{
			get { return _parentBlend; }
			set { _parentBlend = value; }
		}

		/// <summary>
		/// Indicates the state of the clock. 
		/// </summary>
		public TimeState state
		{
			get
			{
				return Timekeeper.GetTimeState(timeScale);
			}
		}

		#endregion

		protected virtual GlobalClock FindParent()
		{
			GlobalClock oldParent = _parent as GlobalClock;

			if (oldParent != null)
			{
				oldParent.Unregister(this);
			}

			if (string.IsNullOrEmpty(parentKey))
			{
				return null;
			}

			if (!Timekeeper.instance.HasClock(parentKey))
			{
				throw new ChronosException(string.Format("Missing parent clock: '{0}'.", parentKey));
			}

			GlobalClock parent = Timekeeper.instance.Clock(parentKey);

			parent.Register(this);

			return parent;
		}

		/// <summary>
		/// Changes the local time scale smoothly over the given duration in seconds.
		/// </summary>
		public void LerpTimeScale(float timeScale, float duration, bool steady = false)
		{
			if (duration < 0) throw new ArgumentException("Duration must be positive.", "duration");

			if (duration == 0)
			{
				localTimeScale = timeScale;
				isLerping = false;
				return;
			}

			float modifier = 1;

			if (steady)
			{
				modifier = Mathf.Abs(localTimeScale - timeScale);
			}

			if (modifier != 0)
			{
				lerpFrom = localTimeScale;
				lerpStart = unscaledTime;

				lerpTo = timeScale;
				lerpEnd = lerpStart + (duration * modifier);

				isLerping = true;
			}
		}

		internal virtual void OnChangedTimeScale(float newTimeScale)
		{

		}
	}
}
