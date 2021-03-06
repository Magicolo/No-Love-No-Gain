﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Magicolo
{
	public static class RigidbodyExtensions
	{
		#region Velocity
		public static void SetVelocity(this Rigidbody rigidbody, Vector3 velocity, Axes axes = Axes.XYZ)
		{
			rigidbody.velocity = rigidbody.velocity.SetValues(velocity, axes);
		}

		public static void SetVelocity(this Rigidbody rigidbody, float velocity, Axes axes = Axes.XYZ)
		{
			rigidbody.SetVelocity(new Vector3(velocity, velocity, velocity), axes);
		}

		public static void Accelerate(this Rigidbody rigidbody, Vector3 acceleration, Axes axes = Axes.XYZ)
		{
			rigidbody.SetVelocity(rigidbody.velocity + acceleration * Time.fixedDeltaTime, axes);
		}

		public static void Accelerate(this Rigidbody rigidbody, float acceleration, Axes axes = Axes.XYZ)
		{
			rigidbody.Accelerate(new Vector3(acceleration, acceleration, acceleration), axes);
		}

		public static void AccelerateTowards(this Rigidbody rigidbody, Vector3 targetAcceleration, float speed, InterpolationModes interpolation, Axes axes = Axes.XYZ)
		{
			switch (interpolation)
			{
				case InterpolationModes.Quadratic:
					rigidbody.SetVelocity(rigidbody.velocity.Lerp(targetAcceleration, Time.fixedDeltaTime * speed, axes), axes);
					break;
				case InterpolationModes.Linear:
					rigidbody.SetVelocity(rigidbody.velocity.LerpLinear(targetAcceleration, Time.fixedDeltaTime * speed, axes), axes);
					break;
			}
		}

		public static void AccelerateTowards(this Rigidbody rigidbody, Vector3 targetAcceleration, float speed, Axes axes = Axes.XYZ)
		{
			rigidbody.AccelerateTowards(targetAcceleration, speed, InterpolationModes.Quadratic, axes);
		}

		public static void AccelerateTowards(this Rigidbody rigidbody, float targetAcceleration, float speed, InterpolationModes interpolation, Axes axes = Axes.XYZ)
		{
			rigidbody.AccelerateTowards(new Vector3(targetAcceleration, targetAcceleration, targetAcceleration), speed, interpolation, axes);
		}

		public static void AccelerateTowards(this Rigidbody rigidbody, float targetAcceleration, float speed, Axes axes = Axes.XYZ)
		{
			rigidbody.AccelerateTowards(new Vector3(targetAcceleration, targetAcceleration, targetAcceleration), speed, InterpolationModes.Quadratic, axes);
		}

		public static void OscillateVelocity(this Rigidbody rigidbody, Vector3 frequency, Vector3 amplitude, Vector3 center, float time, Axes axes = Axes.XYZ)
		{
			rigidbody.SetVelocity(rigidbody.velocity.Oscillate(frequency, amplitude, center, time, rigidbody.GetInstanceID() / 1000, axes), axes);
		}

		public static void OscillateVelocity(this Rigidbody rigidbody, float frequency, float amplitude, float center, float time, Axes axes = Axes.XYZ)
		{
			OscillateVelocity(rigidbody, new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), time, axes);
		}
		#endregion

		#region Position
		public static void SetPosition(this Rigidbody rigidbody, Vector3 position, Axes axes = Axes.XYZ)
		{
			rigidbody.MovePosition(rigidbody.transform.position.SetValues(position, axes));
		}

		public static void SetPosition(this Rigidbody rigidbody, float position, Axes axes = Axes.XYZ)
		{
			rigidbody.SetPosition(new Vector3(position, position, position), axes);
		}

		public static void Translate(this Rigidbody rigidbody, Vector3 translation, Axes axes = Axes.XYZ)
		{
			rigidbody.SetPosition(rigidbody.transform.position + translation * Time.fixedDeltaTime, axes);
		}

		public static void Translate(this Rigidbody rigidbody, float translation, Axes axes = Axes.XYZ)
		{
			rigidbody.Translate(new Vector3(translation, translation, translation), axes);
		}

		public static void TranslateTowards(this Rigidbody rigidbody, Vector3 targetPosition, float speed, InterpolationModes interpolation, Axes axes = Axes.XYZ)
		{
			switch (interpolation)
			{
				case InterpolationModes.Quadratic:
					rigidbody.SetPosition(rigidbody.transform.position.Lerp(targetPosition, Time.fixedDeltaTime * speed, axes), axes);
					break;
				case InterpolationModes.Linear:
					rigidbody.SetPosition(rigidbody.transform.position.LerpLinear(targetPosition, Time.fixedDeltaTime * speed, axes), axes);
					break;
			}
		}

		public static void TranslateTowards(this Rigidbody rigidbody, Vector3 targetPosition, float speed, Axes axes = Axes.XYZ)
		{
			rigidbody.TranslateTowards(targetPosition, speed, InterpolationModes.Quadratic, axes);
		}

		public static void TranslateTowards(this Rigidbody rigidbody, float targetPosition, float speed, InterpolationModes interpolation, Axes axes = Axes.XYZ)
		{
			rigidbody.TranslateTowards(new Vector3(targetPosition, targetPosition, targetPosition), speed, interpolation, axes);
		}

		public static void TranslateTowards(this Rigidbody rigidbody, float targetPosition, float speed, Axes axes = Axes.XYZ)
		{
			rigidbody.TranslateTowards(new Vector3(targetPosition, targetPosition, targetPosition), speed, InterpolationModes.Quadratic, axes);
		}

		public static void OscillatePosition(this Rigidbody rigidbody, Vector3 frequency, Vector3 amplitude, Vector3 center, float time, Axes axes = Axes.XYZ)
		{
			rigidbody.SetPosition(rigidbody.transform.position.Oscillate(frequency, amplitude, center, time, rigidbody.GetInstanceID() / 1000, axes), axes);
		}

		public static void OscillatePosition(this Rigidbody rigidbody, float frequency, float amplitude, float center, float time, Axes axes = Axes.XYZ)
		{
			rigidbody.OscillatePosition(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), time, axes);
		}
		#endregion

		#region Rotation
		public static void SetEulerAngles(this Rigidbody rigidbody, Vector3 angles, Axes axes = Axes.XYZ)
		{
			rigidbody.MoveRotation(Quaternion.Euler(rigidbody.transform.eulerAngles.SetValues(angles, axes)));
		}

		public static void SetEulerAngles(this Rigidbody rigidbody, float angle, Axes axes = Axes.XYZ)
		{
			rigidbody.SetEulerAngles(new Vector3(angle, angle, angle), axes);
		}

		public static void Rotate(this Rigidbody rigidbody, Vector3 rotation, Axes axes = Axes.XYZ)
		{
			rigidbody.SetEulerAngles(rigidbody.transform.eulerAngles + rotation * Time.fixedDeltaTime, axes);
		}

		public static void Rotate(this Rigidbody rigidbody, float rotation, Axes axes = Axes.XYZ)
		{
			rigidbody.Rotate(new Vector3(rotation, rotation, rotation), axes);
		}

		public static void RotateTowards(this Rigidbody rigidbody, Vector3 targetAngles, float speed, InterpolationModes interpolation, Axes axes = Axes.XYZ)
		{
			switch (interpolation)
			{
				case InterpolationModes.Quadratic:
					rigidbody.SetEulerAngles(rigidbody.transform.eulerAngles.LerpAngles(targetAngles, Time.fixedDeltaTime * speed, axes), axes);
					break;
				case InterpolationModes.Linear:
					rigidbody.SetEulerAngles(rigidbody.transform.eulerAngles.LerpAnglesLinear(targetAngles, Time.fixedDeltaTime * speed, axes), axes);
					break;
			}
		}

		public static void RotateTowards(this Rigidbody rigidbody, Vector3 targetAngles, float speed, Axes axes = Axes.XYZ)
		{
			rigidbody.RotateTowards(targetAngles, speed, InterpolationModes.Quadratic, axes);
		}

		public static void RotateTowards(this Rigidbody rigidbody, float targetAngle, float speed, InterpolationModes interpolation, Axes axes = Axes.XYZ)
		{
			rigidbody.RotateTowards(new Vector3(targetAngle, targetAngle, targetAngle), speed, interpolation, axes);
		}

		public static void RotateTowards(this Rigidbody rigidbody, float targetAngle, float speed, Axes axes = Axes.XYZ)
		{
			rigidbody.RotateTowards(new Vector3(targetAngle, targetAngle, targetAngle), speed, InterpolationModes.Quadratic, axes);
		}

		public static void OscillateEulerAngles(this Rigidbody rigidbody, Vector3 frequency, Vector3 amplitude, Vector3 center, float time, Axes axes = Axes.XYZ)
		{
			rigidbody.SetEulerAngles(rigidbody.transform.eulerAngles.Oscillate(frequency, amplitude, center, time, rigidbody.GetInstanceID() / 1000, axes), axes);
		}

		public static void OscillateEulerAngles(this Rigidbody rigidbody, float frequency, float amplitude, float center, float time, Axes axes = Axes.XYZ)
		{
			rigidbody.OscillateEulerAngles(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), time, axes);
		}
		#endregion
	}
}

