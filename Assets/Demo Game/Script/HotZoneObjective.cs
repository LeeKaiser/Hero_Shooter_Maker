using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using HeroShooterMaker.EventBus;
using HeroShooterMaker.MatchSystem;
using HeroShooterMaker.Character;

namespace HeroShooterMakerDemo
{
    public class HotZoneObjective : ObjectiveSystem
    {
        public int PointsPerTick = 1;
        public float TickRate = 1.0f;

        Dictionary<TeamManager, float> alreadyGivenPoint = new Dictionary<TeamManager, float>();

        void Update()
        {

            foreach (TeamManager team in alreadyGivenPoint.Keys.ToList())
            {
                alreadyGivenPoint[team] -= Time.deltaTime;
                if (alreadyGivenPoint[team] <= 0)
                {
                    alreadyGivenPoint.Remove(team);
                }
            }
        }
        void OnTriggerStay(Collider other)
        {
            //if it is player, get the team and give the team points
            CharCore player = other.gameObject.GetComponentInParent<CharCore>();
            if (player != null)
            {
                TeamManager team = player.PlayerAllegience;
                if (team == null)
                {
                    return;
                }
                if (!alreadyGivenPoint.ContainsKey(team))
                {
                    alreadyGivenPoint[team] = TickRate;
                    TeamGetPoint getpoint = new TeamGetPoint();
                    getpoint.TeamIdentity = team;
                    getpoint.pointQuantity = PointsPerTick;
                    EventBus<TeamGetPoint>.Invoke(getpoint);
                }
            }
        }
    }

}
