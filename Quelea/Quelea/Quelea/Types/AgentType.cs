﻿using System;
using Rhino.Geometry;
using RS = Quelea.Properties.Resources;

namespace Quelea
{
 public class AgentType : ParticleType, IAgent
  {
   public AgentType()
     : this(new Vector3d(-RS.velocityDefault, -RS.velocityDefault, -RS.velocityDefault),
            new Vector3d(RS.velocityDefault, RS.velocityDefault, RS.velocityDefault),
            Vector3d.Zero, RS.lifespanDefault, RS.massDefault, RS.bodySizeDefault, 
            RS.historyLenDefault, RS.maxSpeedDefault, RS.maxForceDefault, 
            RS.visionRadiusDefault, RS.visionAngleDefault)
    {
    }

    public AgentType(IAgent a)
     : this(a, a.MaxSpeed, a.MaxForce, a.VisionRadius, a.VisionAngle)
    {
    }

    public AgentType(IParticle p, double maxSpeed, double maxForce, double visionRadius, double visionAngle)
      : base(p.VelocityMin, p.VelocityMax, p.Acceleration, p.Lifespan, p.Mass, p.BodySize, p.HistoryLength)
    {
      SteerAcceleration = Vector3d.Zero;
      MaxSpeed = maxSpeed;
      MaxForce = maxForce;
      VisionRadius = visionRadius;
      VisionAngle = visionAngle;
      Lat = Util.Random.RandomDouble(0, RS.TWO_PI);
      Lon = Util.Random.RandomDouble(-RS.HALF_PI, RS.HALF_PI);
    }

    public AgentType(IAgent a, Point3d emittionPt, AbstractEnvironmentType environment)
      : base(a, emittionPt, environment)
    {
      SteerAcceleration = a.SteerAcceleration;
      MaxSpeed = a.MaxSpeed;
      MaxForce = a.MaxForce;
      VisionRadius = a.VisionRadius;
      VisionAngle = a.VisionAngle;
      Lat = Util.Random.RandomDouble(0, RS.TWO_PI);
      Lon = Util.Random.RandomDouble(-RS.HALF_PI, RS.HALF_PI);
    }

    public AgentType(Vector3d velocityMin, Vector3d velocityMax, Vector3d acceleration,
                        int lifespan, double mass, double bodySize,
                        int historyLength, double maxSpeed, double maxForce, double visionRadius, double visionAngle)
      : base(velocityMin, velocityMax, acceleration, lifespan, mass, bodySize, historyLength)
    {
      MaxSpeed = maxSpeed;
      MaxForce = maxForce;
      VisionRadius = visionRadius;
      VisionAngle = visionAngle;
      Lat = Util.Random.RandomDouble(0, 2 * Math.PI);
      Lon = Util.Random.RandomDouble(-Math.PI / 2, Math.PI / 2);
    }

    public Vector3d ApplySteeringForce(Vector3d force, double weightMultiplier, bool apply)
    {
      if (force.Equals(Vector3d.Zero))
      {
        return Vector3d.Zero;
      }
      // Reynold's steering formula: steer = desired - velocity
      force = force - Velocity;

      // Steering ability can be controlled by limiting the magnitude of the steering force.
      force = Util.Vector.Limit(force, MaxForce);
      force = force * weightMultiplier;
      if (apply)
      {
        SteerAcceleration += force;
      }
      return force;
    }

    public override void Run()
    {
      SteerAcceleration = Util.Vector.Limit(SteerAcceleration, MaxForce);
      Acceleration += SteerAcceleration;
      SteerAcceleration = Vector3d.Zero;
      base.Run();
    }

   public Vector3d SteerAcceleration { get; set; }
   public double MaxSpeed { get; set; }
   public double MaxForce { get; set; }
   public double VisionRadius { get; set; }
   public double VisionAngle { get; set; }
   public double Lon { get; set; }
   public double Lat { get; set; }
  }
}
