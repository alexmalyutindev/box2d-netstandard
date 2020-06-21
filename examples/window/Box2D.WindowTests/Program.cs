﻿/*
    Window Simulation Copyright © Ben Ukhanov 2020
*/

using System;
using System.Threading;
using Box2D.NetStandard.Collision;
using Box2D.NetStandard.Common;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.World;
using Box2D.NetStandard.Dynamics.World.Callbacks;
using Box2D.Window;
using TestWorlds;

namespace Box2D.WindowTests
{
    public static class Program
    {
        private static readonly World world;

        static Program()
        {
            //world = CreateWorld();
            world = RubeGoldberg.CreateWorld();
        }

        private static void Main()
        {
            var windowThread = new Thread(new ThreadStart(() =>
            {
                var game = new SimulationWindow("Physics Simulation", 800, 600);
                game.UpdateFrame += OnUpdateFrame;
                game.Disposed += OnDisposed;
                game.SetView(new CameraView());

                var physicsDrawer = new DrawPhysics(game);
                physicsDrawer.AppendFlags(DrawFlags.Aabb);
                physicsDrawer.AppendFlags(DrawFlags.Shape);

                world.SetDebugDraw(physicsDrawer);

                //CreateBodies();

                game.VSync = OpenTK.VSyncMode.Adaptive;
                game.Run(60.0, 0.0);
            }));

            windowThread.Start();
        }

        private static void OnUpdateFrame(object sender, EventArgs eventArgs)
        {
            // Prepare for simulation. Typically we use a time step of 1/60 of a
            // second (60Hz) and 10 iterations. This provides a high quality simulation
            // in most game scenarios.
            const float TimeStep = 1.0f / 60.0f;
            const int VelocityIterations = 8;
            const int PositionIterations = 3;

            // Instruct the world to perform a single step of simulation. It is
            // generally best to keep the time step and iterations fixed.
            world?.Step(TimeStep, VelocityIterations, PositionIterations);
            world?.DrawDebugData();
        }

        private static void OnDisposed(object sender, EventArgs eventArgs)
        {
            world?.SetDebugDraw(null);
            //world?.Dispose();
        }

        // private static void CreateBodies()
        // {
        //     AddBox(new Vec2(0.0f, 10.0f), new Vec2(5.0f, 5.0f));
        //     AddBox(new Vec2(0.0f, 20.0f), new Vec2(5.0f, 5.0f));
        //
        //     var box = AddBox(new Vec2(2.5f, 25.0f), new Vec2(5.0f, 5.0f));
        //     box.SetLinearVelocity(new Vec2(5, 0));
        //     box.SetAngularVelocity(5);
        //
        //     AddBox(new Vec2(-2.5f, 25.0f), new Vec2(5.0f, 5.0f));
        //     AddStaticBox(new Vec2(0.0f, -10.0f), new Vec2(50.0f, 10.0f));
        // }
        //
        // private static Body AddBox(Vec2 position, Vec2 size)
        // {
        //     var bodyDefinition = new BodyDef();
        //     bodyDefinition.position.Set(position.X, position.Y);
        //
        //     var boxDefinition = new PolygonDef();
        //     boxDefinition.SetAsBox(size.X, size.Y);
        //     boxDefinition.Density = 1.0f;
        //     boxDefinition.Friction = 0.3f;
        //     boxDefinition.Restitution = 0.2f;
        //
        //     var body = world?.CreateBody(bodyDefinition);
        //     body.CreateFixture(boxDefinition);
        //     body.SetMassFromShapes();
        //
        //     return body;
        // }
        //
        // private static void AddStaticBox(Vec2 position, Vec2 size)
        // {
        //     var bodyDefinition = new BodyDef();
        //     bodyDefinition.Position.Set(position.X, position.Y);
        //
        //     var boxDefinition = new PolygonDef();
        //     boxDefinition.SetAsBox(size.X, size.Y);
        //     boxDefinition.Density = 0.0f;
        //
        //     var body = world?.CreateBody(bodyDefinition);
        //     body.CreateFixture(boxDefinition);
        //     body.SetMassFromShapes();
        // }
        //
        // private static World CreateWorld()
        // {
        //     var aABB = new AABB();
        //     aABB.LowerBound.Set(-100.0f, -100.0f);
        //     aABB.UpperBound.Set(100.0f, 100.0f);
        //
        //     return new World(aABB, gravity: new Vec2(0.0f, -9.8f), doSleep: true);
        // }
    }
}