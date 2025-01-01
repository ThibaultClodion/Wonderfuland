namespace IA.Fight
{
    class ModelParameters
    {
        // IDEA
        // Possibilité d'un système de random sur la probabilité de l'IA choisie  : 30% Individual, 20% Group, 50% Boss
        // ou en fonction de la vie du groupe et des coéquipiers / de l'état du combat
        // -- Ajoute de l'aléatoire aux combats (qui pour le moment n'existe pas)
        // ++ Peut amener à des dérives sympas -> Mob qui ne suit plus les ordres et fait n'importe quoi à cause d'une défaite assurée
        // ++ Boss qui tue / blesse un allié qui n'écoute pas les ordres 

        // Individual IA
        TargetFocus targetFocus;
        MindsetType mindsetType;

        public enum TargetFocus
        {
            CLOSER,
            LIFELESS,
            SPECIFIC
        }

        public enum MindsetType
        {
            COWARD,
            BRAVE,
            RAISONNABLE
        }

        // Group IA
        public enum GroupComportement
        {
            HELPER,
            INDIVIDUALIST

        }


        // Boss IA

        // Règles particulière -> 
    }
}
