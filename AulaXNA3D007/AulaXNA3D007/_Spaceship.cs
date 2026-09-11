using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AulaXNA3D007
{
    public class _Spaceship
    {
        Game game;
        Vector3 position, scale, rotation;
        float speedRot;
        Matrix world;
        Model model;
        Matrix[] modelTransforms;

        public _Spaceship(Game game)
        {
            this.game = game;

            this.position = Vector3.Zero;
            this.scale = Vector3.One * 0.1f;
            this.rotation = Vector3.Zero;
            this.speedRot = 100;

            this.SetupMatrixWorld();

            this.model = this.game.Content.Load<Model>(@"Models\spaceship");
            this.modelTransforms = new Matrix[this.model.Bones.Count];
            this.model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            
            //foreach (ModelMesh mesh in this.model.Meshes) // uso único mais eficiente
            //    foreach (BasicEffect effect in mesh.Effects)
            //        effect.EnableDefaultLighting();
        }

        public void Update(GameTime gt)
        {
            this.rotation.Y += this.speedRot * (float)gt.ElapsedGameTime.TotalSeconds;

            this.SetupMatrixWorld();
        }

        public void Draw(_Camera camera)
        {
            foreach (ModelMesh mesh in this.model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting(); // didático
                    //effect.World = this.world * mesh.ParentBone.Transform; //modelos simples
                    effect.World = modelTransforms[mesh.ParentBone.Index] * this.world; //modelos complexos
                    effect.View = camera.GetView();
                    effect.Projection = camera.GetProjection();
                }
                mesh.Draw();
            }
        }

        private void SetupMatrixWorld()
        {
            this.world = Matrix.Identity;
            this.world *= Matrix.CreateScale(this.scale);
            this.world *= Matrix.CreateRotationX(MathHelper.ToRadians(this.rotation.X));
            this.world *= Matrix.CreateRotationY(MathHelper.ToRadians(this.rotation.Y));
            this.world *= Matrix.CreateRotationZ(MathHelper.ToRadians(this.rotation.Z));
            this.world *= Matrix.CreateTranslation(this.position);
        }
    }
}