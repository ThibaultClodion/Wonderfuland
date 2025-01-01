using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using Utils;

namespace SpellCreator
{


    //public enum SpellTargetSide
    //{
    //    ALLY,
    //    ENNEMY
    //}

    class Spell
    {
        public List<Effect> effects;
        public string iconPath;
        //public string name;
        
        // Ces 3 champs sont temporaire, l'UI les transformera en scopeZone
        public bool basicScope;
        [ReflectionLimit(0, 10)] public int scopeMin;
        [ReflectionLimit(0, 10)] public int scopeMax;
        public List<Vector2Int> scopeZone;

        public bool throughObstacles;
        public List<Cell.CellType> targets;
        public List<Cell.CellOccupation> targetOccupation;
        //public List<SpellTargetSide> targetSide; // if targetOccupation.contains CHARACTER_ON_IT 

        [ReflectionLimit(0, 50)] public int costValue; // ReflectionLimit doit dépendre du costStat
        public Stats.StatEnum costStat;
        [ReflectionLimit(0, 10)] public int cooldown;
        [ReflectionLimit(1, 5)] public int launchPerTurn;

        private int currentCooldown;
        private int launchedThisTurn = 0;

        public Spell()
        {
            effects = new List<Effect>();
            scopeZone = new List<Vector2Int>();
            targets = new List<Cell.CellType>();
            targetOccupation = new List<Cell.CellOccupation>();
            //targetSide = new List<SpellTargetSide>();
        }

        [OnDeserialized]
        private void JsonInit(StreamingContext context)
        {
            currentCooldown = cooldown;
        }

        public bool CanBeLaunched(Stats stats)
        {
            return stats.GetStatValue(costStat) >= costValue && launchedThisTurn < launchPerTurn
                && currentCooldown >= cooldown;
        }

        public bool CanBeLaunchedAt(Character launcher, Vector2Int position)
        {
            return CanBeLaunched(launcher.statsCurrent) && getAvailableLaunchPositions(launcher.position).Contains(position);
        }

        public List<Vector2Int> getAvailableLaunchPositions(Vector2Int launcherPosition)
        {
            Cell[][] map = MapManager.Instance.GetGrid();
            List<Vector2Int> filteringPositions = new List<Vector2Int>();

            // TMP
            targets = new List<Cell.CellType>
            {
                Cell.CellType.Ground
            };

            targetOccupation = new List<Cell.CellOccupation>
            {
                Cell.CellOccupation.FREE,
                Cell.CellOccupation.CHARACTER_ON_IT
            };

            //if (basicScope)
            //{
            scopeZone = BasicScopeToScopeZone(scopeMin, scopeMax);
            //}

            // Add all except out of map positions 
            foreach(Vector2Int zone in scopeZone)
            {
                int positionX = launcherPosition.x + zone.x;
                if (positionX < 0 || positionX >= map.Length)
                    continue;

                int positionY = launcherPosition.y + zone.y;
                if (positionY < 0 || positionY >= map[positionX].Length)
                    continue;

                filteringPositions.Add(new Vector2Int(positionX, positionY));
            }

            //Remove untouchable positions
            for(int i = 0; i < filteringPositions.Count; i++)
            {
                Vector2Int zone = filteringPositions[i];
                Cell.CellType currentType = map[zone.x][zone.y].GetCellType();
                
                if (currentType == Cell.CellType.Wall && !throughObstacles)
                {
                    // enlever positions derrieres murs
                }

                if (!targets.Contains(currentType))
                {
                    filteringPositions.Remove(zone);
                    i--;
                    continue;
                }

                // targetOccupation
                if (!targetOccupation.Contains(Cell.GetCellOccupation(zone)))
                {
                    filteringPositions.Remove(zone);
                    i--;
                    continue;
                }
            }

            return filteringPositions;
        }

        // Récupération de toutes les positions existantes dans la range scopeMin-scopeMax
        public static List<Vector2Int> BasicScopeToScopeZone(int scopeMin, int scopeMax)
        {
            List<Vector2Int> _scopeZone = new List<Vector2Int>();
            for (int x = -scopeMax; x <= scopeMax; x++)
            {
                for (int y = -scopeMax; y <= scopeMax; y++)
                {
                    if (Math.Abs(x) + Math.Abs(y) > scopeMax || Math.Abs(x) + Math.Abs(y) < scopeMin)
                        continue;

                    _scopeZone.Add(new Vector2Int(x, y));
                }
            }
            return _scopeZone;
        }

        public void Launch(Character launcher, Vector2Int position)
        {
            foreach (Effect effect in effects)
            {
                effect.Apply(launcher, position);
            }
            launchedThisTurn++;
            currentCooldown = 0;
        }

        public void NextTurn()
        {
            launchedThisTurn = 0;
            currentCooldown++;
        }
    }

    class Passif : Spell // directement sur le personnage en tant qu'effect ou liste d'effect
    {
        public Passif()
        {
            scopeMin = 0;
            scopeMax = 0;
            costValue = 0;
            costStat = Stats.StatEnum.PA;
            throughObstacles = false;
        }
    }
}
