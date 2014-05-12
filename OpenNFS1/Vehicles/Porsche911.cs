﻿using System;
using System.Collections.Generic;
using System.Text;
using NeedForSpeed.Physics;
using Microsoft.Xna.Framework;
using NeedForSpeed.Parsers;
using NfsEngine;
using NeedForSpeed.Dashboards;
using Microsoft.Xna.Framework.Graphics;

namespace NeedForSpeed.Vehicles
{
    class Porsche911 : Vehicle
    {
        private CarModel _model;

        public Porsche911()
            : base(1480, "ToyotaSupra")
        {

            _model = CarModelCache.GetModel(@"SIMDATA\CARFAMS\p911.CFM");

            _wheels[0] = new VehicleWheel(this, new Vector3(-9f, 0f, 15.9f), _model.TyreTexture, 6.6f);
            _wheels[1] = new VehicleWheel(this, new Vector3(9f, 0f, 15.9f), _model.TyreTexture, 6.6f);
            _wheels[2] = new VehicleWheel(this, new Vector3(-10f, 0f, -12.0f), _model.TyreTexture, 6.7f);
            _wheels[3] = new VehicleWheel(this, new Vector3(10f, 0f, -12f), _model.TyreTexture, 6.7f);

            _bodyRideHeight = 0f;
        }


				public override void Render()
				{
					RenderShadow();
					_effect.View = Engine.Instance.Camera.View;
					_effect.Projection = Engine.Instance.Camera.Projection;
					_effect.CurrentTechnique.Passes[0].Apply();
					_effect.World = Matrix.CreateScale(0.09f) * _renderMatrix;
					_model.Render(_effect, VehicleController.Brake > 0);
					base.Render();
				}

        internal override BaseDashboard Dashboard
        {
            get { return new P911Dashboard(this); }
        }

        public override void InitializeForDriving()
        {
            base.InitializeForDriving();
            List<float> power = new List<float>(new float[] { 0.2f, 0.3f, 0.4f, 0.7f, 0.8f, 1.0f, 1.0f, 0.8f });
            List<float> ratios = new List<float>(new float[] { 3.827f, 2.360f, 1.685f, 1.312f, 1.000f, 0.793f });

            BaseGearbox gearbox = BaseGearbox.Create(GameConfig.ManualGearbox, ratios, 0.4f);
            _motor = new Motor(power, 6, 6.6f, gearbox);
            _motor.Gearbox.GearChangeStarted += new EventHandler(Gearbox_GearChanged);
            _traction = (_motor.GetPowerAtRpmForGear(_motor.RedlineRpm, 2) * 30) - 30;
        }

        public override string Name
        {
            get
            {
                return "1993 Porsche 911 Turbo";
            }
        }

        public override string Description
        {
            get
            {
                return "Max power: 355hp\r\nEngine: 3.6l Flat 6\r\nWeight: 1350kg";
            }
        }
    }
}