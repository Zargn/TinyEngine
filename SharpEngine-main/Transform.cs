using System;
using System.Numerics;
using System.Runtime.InteropServices;
using static OpenGL.Gl;

namespace SharpEngine {
	public class Transform {
		public Vector CurrentScale { get; private set; }
		public Vector Position { get; private set; }
		public Vector Rotation { get; private set; }
		public Matrix Matrix => Matrix.Translation(Position) * Matrix.Rotation(Rotation) * Matrix.Scale(CurrentScale);


		public Transform()
		{
			this.CurrentScale = new Vector(1, 1, 1);
		}

		public void Scale(Vector multiplier)
		{
			CurrentScale *= multiplier.x;
		}

		public void Move(Vector direction)
		{
			this.Position += direction;
		}

		public void Rotate(Vector angle)
		{
			// this.transform *= Matrix.RotateZ(rotation);
			var rotation = this.Rotation;
			rotation += angle;
			this.Rotation = rotation;
		}
	}
}