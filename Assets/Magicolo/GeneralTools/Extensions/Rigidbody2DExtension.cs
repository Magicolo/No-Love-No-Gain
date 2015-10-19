﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo
{
	public static class Rigidbody2DExtension
	{

		#region Velocity
		public static void SetVelocity(this Rigidbody2D rigidbody, Vector2 velocity, Axes axes = Axes.XY)
		{
			rigidbody.velocity = rigidbody.velocity.SetValues(velocity, axes);
		}

		public static void SetVelocity(this Rigidbody2D rigidbody, float velocity, Axes axes = Axes.XY)
		{
			rigidbody.SetVelocity(new Vector2(velocity, velocity), axes);
		}

		public static void Accelerate(this Rigidbody2D rigidbody, Vector2 acceleration, Axes axes = Axes.XY)
		{
			rigidbody.SetVelocity((rigidbody.velocity + acceleration * Time.fixedDeltaTime), axes);
		}

		public static void Accelerate(this Rigidbody2D rigidbody, float acceleration, Axes axes = Axes.XY)
		{
			rigidbody.Accelerate(new Vector2(acceleration, acceleration), axes);
		}

		public static void AccelerateTowards(this Rigidbody2D rigidbody, Vector2 targetAcceleration, float speed, float deltaTime, InterpolationModes interpolation = InterpolationModes.Quadratic, Axes axes = Axes.XY)
		{
			switch (interpolation)
			{
				case InterpolationModes.Quadratic:
					rigidbody.SetVelocity(rigidbody.velocity.Lerp(targetAcceleration, deltaTime * speed, axes), axes);
					break;
				case InterpolationModes.Linear:
					rigidbody.SetVelocity(rigidbody.velocity.LerpLinear(targetAcceleration, deltaTime * speed, axes), axes);
					break;
			}
		}

		public static void AccelerateTowards(this Rigidbody2D rigidbody, float targetAcceleration, float speed, float deltaTime, InterpolationModes interpolation = InterpolationModes.Quadratic, Axes axes = Axes.XY)
		{
			rigidbody.AccelerateTowards(new Vector2(targetAcceleration, targetAcceleration), speed, deltaTime, interpolation, axes);
		}

		public static void OscillateVelocity(this Rigidbody2D rigidbody, Vector2 frequency, Vector2 amplitude, Vector2 center, Axes axes = Axes.XY)
		{
			rigidbody.SetVelocity(rigidbody.velocity.Oscillate(frequency, amplitude, center, rigidbody.GetInstanceID() / 1000, axes), axes);
		}

		public static void OscillateVelocity(this Rigidbody2D rigidbody, Vector2 frequency, Vector2 amplitude, Axes axes = Axes.XY)
		{
			OscillateVelocity(rigidbody, frequency, amplitude, Vector2.zero, axes);
		}

		public static void OscillateVelocity(this Rigidbody2D rigidbody, float frequency, float amplitude, Axes axes = Axes.XY)
		{
			OscillateVelocity(rigidbody, new Vector2(frequency, frequency), new Vector2(amplitude, amplitude), Vector2.zero, axes);
		}

		public static void OscillateVelocity(this Rigidbody2D rigidbody, float frequency, float amplitude, float center, Axes axes = Axes.XY)
		{
			OscillateVelocity(rigidbody, new Vector2(frequency, frequency), new Vector2(amplitude, amplitude), new Vector2(center, center), axes);
		}
		#endregion

		#region Position
		public static void SetPosition(this Rigidbody2D rigidbody, Vector2 position, Axes axes = Axes.XY)
		{
			rigidbody.MovePosition(rigidbody.transform.position.SetValues((Vector3)position, axes));
		}

		public static void SetPosition(this Rigidbody2D rigidbody, float position, Axes axes = Axes.XY)
		{
			rigidbody.SetPosition(new Vector2(position, position), axes);
		}

		public static void Translate(this Rigidbody2D rigidbody, Vector2 translation, Axes axes = Axes.XY)
		{
			rigidbody.SetPosition(rigidbody.transform.position + (Vector3)translation * Time.fixedDeltaTime, axes);
		}

		public static void Translate(this Rigidbody2D rigidbody, float translation, Axes axes = Axes.XY)
		{
			rigidbody.Translate(new Vector2(translation, translation), axes);
		}

		public static void TranslateTowards(this Rigidbody2D rigidbody, Vector2 targetPosition, float speed, InterpolationModes interpolation, Axes axes = Axes.XY)
		{
			switch (interpolation)
			{
				case InterpolationModes.Quadratic:
					rigidbody.SetPosition(rigidbody.transform.position.Lerp((Vector3)targetPosition, Time.fixedDeltaTime * speed, axes), axes);
					break;
				case InterpolationModes.Linear:
					rigidbody.SetPosition(rigidbody.transform.position.LerpLinear((Vector3)targetPosition, Time.fixedDeltaTime * speed, axes), axes);
					break;
			}
		}

		public static void TranslateTowards(this Rigidbody2D rigidbody, Vector2 targetPosition, float speed, Axes axes = Axes.XY)
		{
			rigidbody.TranslateTowards(targetPosition, speed, InterpolationModes.Quadratic, axes);
		}

		public static void TranslateTowards(this Rigidbody2D rigidbody, float targetPosition, float speed, InterpolationModes interpolation, Axes axes = Axes.XY)
		{
			rigidbody.TranslateTowards(new Vector2(targetPosition, targetPosition), speed, interpolation, axes);
		}

		public static void TranslateTowards(this Rigidbody2D rigidbody, float targetPosition, float speed, Axes axes = Axes.XY)
		{
			rigidbody.TranslateTowards(new Vector2(targetPosition, targetPosition), speed, InterpolationModes.Quadratic, axes);
		}

		public static void OscillatePosition(this Rigidbody2D rigidbody, Vector2 frequency, Vector2 amplitude, Vector2 center, Axes axes = Axes.XY)
		{
			rigidbody.SetPosition(rigidbody.transform.position.Oscillate((Vector3)frequency, (Vector3)amplitude, (Vector3)center, rigidbody.transform.GetInstanceID() / 1000, axes), axes);
		}

		public static void OscillatePosition(this Rigidbody2D rigidbody, Vector2 frequency, Vector2 amplitude, Axes axes = Axes.XY)
		{
			rigidbody.OscillatePosition(frequency, amplitude, Vector2.zero, axes);
		}

		public static void OscillatePosition(this Rigidbody2D rigidbody, float frequency, float amplitude, float center, Axes axes = Axes.XY)
		{
			rigidbody.OscillatePosition(new Vector2(frequency, frequency), new Vector2(amplitude, amplitude), new Vector2(center, center), axes);
		}

		public static void OscillatePosition(this Rigidbody2D rigidbody, float frequency, float amplitude, Axes axes = Axes.XY)
		{
			rigidbody.OscillatePosition(new Vector2(frequency, frequency), new Vector2(amplitude, amplitude), Vector2.zero, axes);
		}
		#endregion

		#region Rotation
		public static void SetEulerAngle(this Rigidbody2D rigidbody, float angle)
		{
			rigidbody.MoveRotation(angle);
		}

		public static void Rotate(this Rigidbody2D rigidbody, float rotation)
		{
			rigidbody.SetEulerAngle(rigidbody.transform.eulerAngles.z + rotation * Time.fixedDeltaTime);
		}

		public static void RotateTowards(this Rigidbody2D rigidbody, float targetAngle, float speed, InterpolationModes interpolation)
		{
			switch (interpolation)
			{
				case InterpolationModes.Quadratic:
					rigidbody.SetEulerAngle(rigidbody.transform.eulerAngles.LerpAngles(new Vector3(targetAngle, targetAngle, targetAngle), Time.fixedDeltaTime * speed, Axes.Z).z);
					break;
				case InterpolationModes.Linear:
					rigidbody.SetEulerAngle(rigidbody.transform.eulerAngles.LerpAnglesLinear(new Vector3(targetAngle, targetAngle, targetAngle), Time.fixedDeltaTime * speed, Axes.Z).z);
					break;
			}
		}

		public static void RotateTowards(this Rigidbody2D rigidbody, float targetAngle, float speed)
		{
			rigidbody.RotateTowards(targetAngle, speed, InterpolationModes.Quadratic);
		}

		public static void OscillateEulerAngles(this Rigidbody2D rigidbody, float frequency, float amplitude, float center)
		{
			rigidbody.SetEulerAngle(rigidbody.transform.eulerAngles.Oscillate(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), rigidbody.GetInstanceID() / 1000, Axes.Z).z);
		}

		public static void OscillateEulerAngles(this Rigidbody2D rigidbody, float frequency, float amplitude)
		{
			rigidbody.OscillateEulerAngles(frequency, amplitude, 0);
		}
		#endregion
	}
}