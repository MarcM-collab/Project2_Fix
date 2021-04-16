//ENTITY MANAGER
//public static Entity GetNextAlliedExecutor(Team team)
//{
//    var entitiesActive = GetLivingNonExhaustedAllies(team);

//    if (entitiesActive.Length == 0)
//    {
//        return null;
//    }

//    else if (entitiesActive.Length == 1)
//        return entitiesActive[0];

//    else
//    {
//        var nextExecutor = entitiesActive[0];
//        var maxHeight = nextExecutor.gameObject.transform.position.y;

//        for (int i = 1; i < entitiesActive.Length; i++)
//        {
//            var currentHeight = entitiesActive[i].gameObject.transform.position.y;
//            if (maxHeight < currentHeight)
//            {
//                nextExecutor = entitiesActive[i];
//                maxHeight = currentHeight;
//            }
//        }
//        return nextExecutor;
//    }
//}
//public static Entity GetRandomEnemy(Entity entity)
//{
//    var livingEnemies = GetLivingEnemies(entity);
//    return livingEnemies[Random.Range(0, livingEnemies.Length)];
//}
////Cambiar despres de implementar el health system
//public static Entity GetMoreHealthMissingAllied(Entity entity)
//{
//    var livingAllies = GetLivingAllies(entity);
//    var maxEntity = livingAllies[0];
//    var maxHealthMissing = maxEntity.MaxHP - maxEntity.HP;

//    for (int i = 1; i < livingAllies.Length; i++)
//    {
//        var currentEntity = livingAllies[i];
//        var currentHealthMissing = currentEntity.MaxHP - currentEntity.HP;
//        if (maxHealthMissing < currentHealthMissing)
//        {
//            maxEntity = currentEntity;
//            maxHealthMissing = currentHealthMissing;
//        }
//    }
//    return maxEntity;
//}
//public static void RemoveTeamExhaustion(Team team)
//{
//    var livingAllies = GetLivingAllies(team);

//    foreach (Entity entity in livingAllies)
//    {
//        entity.Exhausted = false;
//        entity.ShowNonExhausted();
//    }
//}
//public static void AddTeamExhaustion(Team team)
//{
//    var livingAllies = GetLivingAllies(team);

//    foreach (Entity entity in livingAllies)
//    {
//        entity.Exhausted = true;
//        entity.ShowExhausted();
//    }
//}
//public static bool IsEntityInList(Entity[] list, Entity entitySearched)
//{
//    foreach (Entity entity in list)
//    {
//        if (entity == entitySearched)
//            return true;
//    }
//    return false;
//}
//public static bool AllMaxHealth(Entity entity)
//{
//    var livingAllies = GetLivingAllies(entity);

//    foreach (Entity ent in livingAllies)
//    {
//        if (ent.HP != ent.MaxHP)
//            return false;
//    }
//    return true;
//}
//public static int TeamScore(Team team)
//{
//    Entity[] teamEntities = Entities.Where(x => x.Team == team).ToArray();
//    int score = 0;

//    foreach (Entity entity in teamEntities)
//    {
//        score += (int)entity.HP;
//    }
//    return score;
//}
//::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    //private static Color ActiveColor = Color.white;
    //private static Color ExhaustedColor = new Color(0.3f, 0.3f, 0.3f);
    //private static Color DeadColor = new Color(0.3f, 0.0f, 0.0f);

    //public static void ShowActive(SpriteRenderer spriteRenderer)
    //{
    //    spriteRenderer.color = ActiveColor;
    //}
    //public static void ShowExhausted(SpriteRenderer spriteRenderer)
    //{
    //    spriteRenderer.color = ExhaustedColor;
    //}
    //public static void ShowDead(SpriteRenderer spriteRenderer)
    //{
    //    spriteRenderer.color = DeadColor;
    //}