using System;
using System.Collections.Generic;

namespace IA.RLearning
{
    #region QLearning.hpp
    //    public:
    //    int getAction(int state, int score, std::vector<int> availableActions);
    //    int getBestAction(int state, int score, std::vector<int> availableActions);
    //    void getActionReward(int state, int score, bool gameOver);
    //    void loadModel(PolicyAndActionValueFunction model);
    //    PolicyAndActionValueFunction compile();

    //    static void save(PolicyAndActionValueFunction policyAndActionValueFunction, PolicyAndActionValueFunction policyAndActionValueFunctionBoss);
    //    static std::vector<PolicyAndActionValueFunction> loadFromFile(std::string fileName);
    //    static std::string getModel(PolicyAndActionValueFunction policyAndActionValueFunction);

    #endregion

    #region QLearning.cpp
    //void QLearning::loadModel(PolicyAndActionValueFunction model) {
    //    pi = model.pi;
    //    q = model.q;
    //    for (auto it = q.begin(); it != q.end(); it++)
    //    {
    //        int state = it->first;
    //        int optimal_a;
    //        float bestValue = 0.0;
    //        for (int a = 0; a < numberOfActions; a++)
    //        {
    //            if (q[state][a] > bestValue)
    //            {
    //                optimal_a = a;
    //                bestValue = q[state][a];
    //            }
    //        }

    //        for (std::map<int, float>::iterator it = q[state].begin(); it != q[state].end(); ++it)
    //        {
    //            if (it->first == optimal_a)
    //            {
    //                b[state][it->first] = 1 - epsilon + epsilon / numberOfActions;
    //            }
    //            else
    //            {
    //                b[state][it->first] = epsilon / numberOfActions;
    //            }
    //        }
    //    }
    //}

    //int QLearning::getBestAction(int state, int score, std::vector<int> availableActions) {
    //    if (pi.find(state) != pi.end())
    //    {
    //        int bestAction = -1;
    //        float bestActionValue;
    //        for (std::map<int, float>::iterator it = q[state].begin(); it != q[state].end(); ++it)
    //        {
    //            if (std::find(availableActions.begin(), availableActions.end(), it->first) != availableActions.end())
    //            {
    //                if (bestAction == -1 || it->second > bestActionValue)
    //                {
    //                    bestActionValue = it->second;
    //                    bestAction = it->first;
    //                }
    //            }
    //        }
    //        if (bestAction != -1)
    //        {
    //            return bestAction;
    //        }
    //    }
    //    return getAction(state, score, availableActions);
    //}

    //int QLearning::getAction(int state, int score, std::vector<int> availableActions) {
    //    if (pi.find(state) == pi.end())
    //    {
    //        pi.insert(std::pair<int, std::map<int, float>>(state, { }));
    //        q.insert(std::pair<int, std::map<int, float>>(state, { }));
    //        b.insert(std::pair<int, std::map<int, float>>(state, { }));
    //        for (int a = 0; a < numberOfActions; a++)
    //        {
    //            pi[state].insert(std::pair<int, float>(a, 1.0 / numberOfActions));
    //            q[state].insert(std::pair<int, float>(a, 0.0));
    //            b[state].insert(std::pair<int, float>(a, 1.0 / numberOfActions));
    //        }
    //    }

    //    int optimal_a;
    //    float bestValue = 0.0;
    //    for (int a = 0; a < numberOfActions; a++)
    //    {
    //        if (q[state][a] > bestValue)
    //        {
    //            optimal_a = a;
    //            bestValue = q[state][a];
    //        }
    //    }

    //    for (std::map<int, float>::iterator it = q[state].begin(); it != q[state].end(); ++it)
    //    {
    //        if (it->first == optimal_a)
    //        {
    //            b[state][it->first] = 1 - epsilon + epsilon / numberOfActions;
    //        }
    //        else
    //        {
    //            b[state][it->first] = epsilon / numberOfActions;
    //        }
    //    }

    //    chosenAction = randomChoice(b[state], availableActions);
    //    scoreBeforeAction = score;
    //    stateBeforeAction = state;
    //    //debug
    //    if (saveChosenActions)
    //    {
    //        if (chosenActions.find(chosenAction) == chosenActions.end())
    //        {
    //            chosenActions.insert(std::pair<int, int>(chosenAction, 1));
    //        }
    //        else
    //        {
    //            chosenActions[chosenAction]++;
    //        }
    //    }
    //    return chosenAction;
    //}

    //void QLearning::getActionReward(int state, int score, bool gameOver) {
    //    int reward = score - scoreBeforeAction;
    //    int s_p = state;

    //    if (gameOver)
    //    {
    //        q[stateBeforeAction][chosenAction] += alpha * (reward + 0.0 - q[stateBeforeAction][chosenAction]);
    //    }
    //    else
    //    {
    //        if (pi.find(s_p) == pi.end())
    //        {
    //            pi.insert(std::pair<int, std::map<int, float>>(s_p, { }));
    //            q.insert(std::pair<int, std::map<int, float>>(s_p, { }));
    //            b.insert(std::pair<int, std::map<int, float>>(s_p, { }));
    //            for (int a = 0; a < numberOfActions; a++)
    //            {
    //                pi[s_p].insert(std::pair<int, float>(a, 1.0 / numberOfActions));
    //                q[s_p].insert(std::pair<int, float>(a, 0.0));
    //                b[s_p].insert(std::pair<int, float>(a, 1.0 / numberOfActions));
    //            }
    //        }
    //        float bestQ;
    //        for (std::map<int, float>::iterator it = q[s_p].begin(); it != q[s_p].end(); ++it)
    //        {
    //            if (it == q[s_p].begin())
    //            {
    //                bestQ = it->second;
    //            }
    //            else if (it->second > bestQ)
    //            {
    //                bestQ = it->second;
    //            }
    //        }
    //        q[stateBeforeAction][chosenAction] += alpha * (reward + gamma * bestQ - q[stateBeforeAction][chosenAction]);
    //    }
    //}

    //PolicyAndActionValueFunction QLearning::compile() {
    //    for (std::map<int, std::map<int, float>>::iterator it = q.begin(); it != q.end(); ++it)
    //    {
    //        int optimal_a;
    //        float bestValue = 0.0;
    //        for (std::map<int, float>::iterator it2 = it->second.begin(); it2 != it->second.end(); ++it2)
    //        {
    //            if (it2->second > bestValue)
    //            {
    //                optimal_a = it2->first;
    //                bestValue = it2->second;
    //            }
    //        }
    //        for (std::map<int, float>::iterator it2 = it->second.begin(); it2 != it->second.end(); ++it2)
    //        {
    //            if (it2->first == optimal_a)
    //            {
    //                pi[it->first][it2->first] = 1.0;
    //            }
    //            else
    //            {
    //                pi[it->first][it2->first] = 0.0;
    //            }
    //        }
    //    }
    //    return PolicyAndActionValueFunction{ pi = pi, q = q };
    //}



    //std::vector<PolicyAndActionValueFunction> QLearning::loadFromFile(std::string fileName) {
    //    std::ifstream MyReadFile("../ressources/policyAndActionValueFunction/" + fileName + ".model");
    //    std::string content;
    //    std::string line;
    //    while (getline(MyReadFile, line))
    //    {
    //        content += line;
    //    }

    //    MyReadFile.close();

    //    std::string delimiter = "/";
    //    size_t pos = 0;
    //    std::vector < std::string> models;
    //    while ((pos = content.find(delimiter)) != std::string::npos) {
    //        models.push_back(content.substr(0, pos));
    //        content.erase(0, pos + delimiter.length());
    //    }
    //    std::vector<PolicyAndActionValueFunction> result;
    //    result.push_back(PolicyAndActionValueFunction{ pi = stringToMap(models.at(0)), q = stringToMap(models.at(1)) });
    //    if (models.size() == 4 && models.at(2).length() != 0 && models.at(3).length() != 0)
    //    {
    //        result.push_back(PolicyAndActionValueFunction{ pi = stringToMap(models.at(2)), q = stringToMap(models.at(3)) });
    //    }
    //    return result;
    //}

    //std::string QLearning::getModel(PolicyAndActionValueFunction policyAndActionValueFunction) {
    //    return mapToString(policyAndActionValueFunction.pi) + "/"
    //        + mapToString(policyAndActionValueFunction.q);
    //}

    //void QLearning::save(PolicyAndActionValueFunction policyAndActionValueFunction, PolicyAndActionValueFunction policyAndActionValueFunctionBoss) {
    //    DIR* dir;
    //    struct dirent*ent;
    //int it = -2;
    //if ((dir = opendir("../ressources/policyAndActionValueFunction")) != NULL)
    //{
    //    while ((ent = readdir(dir)) != NULL)
    //    {
    //        it++;
    //    }
    //    closedir(dir);
    //}
    //else
    //{
    //    std::cout << "directory not find" << std::endl;
    //    return;
    //}
    //std::ofstream outfile;
    //outfile.open("../ressources/policyAndActionValueFunction/model_" + std::to_string(it) + ".model");

    //std::vector < std::string> piLevel = mapToStrings(policyAndActionValueFunction.pi);
    //for (int i = 0; i < piLevel.size(); i++)
    //{
    //    outfile << piLevel.at(i);
    //}
    //outfile << "/";

    //std::vector < std::string> qLevel = mapToStrings(policyAndActionValueFunction.q);
    //for (int i = 0; i < qLevel.size(); i++)
    //{
    //    outfile << qLevel.at(i);
    //}
    //outfile << "/";

    //std::vector < std::string> piBoss = mapToStrings(policyAndActionValueFunctionBoss.pi);
    //for (int i = 0; i < piBoss.size(); i++)
    //{
    //    outfile << piBoss.at(i);
    //}
    //outfile << "/";

    //std::vector < std::string> qBoss = mapToStrings(policyAndActionValueFunctionBoss.q);
    //for (int i = 0; i < qBoss.size(); i++)
    //{
    //    outfile << qBoss.at(i);
    //}
    //outfile << std::endl;

    ////outfile << getModel(policyAndActionValueFunction) + "/";
    ////outfile << getModel(policyAndActionValueFunctionBoss) << std::endl;
    //outfile.close();
    //}
    #endregion

    using TypeToChoose = Dictionary<int, Dictionary<int, float>>;

    struct PolicyAndActionValueFunction
    {
        TypeToChoose pi;
        TypeToChoose q;
    };

    class QLearning
    {
        private const bool saveChosenActions = true;
        private Dictionary<int, int> chosenActions;
        private float alpha;
        private float epsilon;
        private float gamma;
        private TypeToChoose pi;
        private TypeToChoose q;
        private TypeToChoose b;
        private int numberOfActions = 7;
        private int scoreBeforeAction;
        private int stateBeforeAction;
        private int chosenAction;
        //private int actionIteration = 0;

        public QLearning(float alpha1, float epsilon1, float gamma1)
        {
            if (epsilon1 <= 0.0)
            {
                epsilon1 = 1.0f;
            }
            alpha = alpha1;
            epsilon = epsilon1;
            gamma = gamma1;
        }

        //private static string mapToString(TypeToChoose parameter)
        //{
        //    string result = string.Empty;
        //    int count = 0;
        //    foreach(var param in parameter)
        //    { 
        //        result += param.Key.ToString() + ":{";
        //        int count2 = 0;
        //        foreach (var param2 in param.Value)
        //        {
        //            //result += std::to_string(it2->first) + ":";
        //            std::stringstream floatBuffer;
        //            floatBuffer << std::fixed << std::setprecision(1) << it2->second;
        //            result += floatBuffer.str();
        //            if (count2 != it->second.size() - 1)
        //            {
        //                result += ",";
        //            }
        //            count2++;
        //        }
        //        result += "}";
        //        if (count != parameter.Count - 1)
        //        {
        //            result += ";";
        //        }
        //        count++;
        //    }
        //    return result;
        //}

        //private static List<string> mapToStrings(TypeToChoose parameter)
        //{
        //    std::vector < std::string> result;
        //    std::string val;
        //    int count = 0;
        //    for (std::map<int, std::map<int, float>>::iterator it = parameter.begin(); it != parameter.end(); ++it)
        //    {
        //        val += std::to_string(it->first) + ":{";
        //        int count2 = 0;
        //        for (std::map<int, float>::iterator it2 = it->second.begin(); it2 != it->second.end(); ++it2)
        //        {
        //            //val += std::to_string(it2->first) + ":";
        //            std::stringstream floatBuffer;
        //            floatBuffer << std::fixed << std::setprecision(1) << it2->second;
        //            val += floatBuffer.str();
        //            if (count2 != it->second.size() - 1)
        //            {
        //                val += ",";
        //            }
        //            count2++;
        //        }
        //        val += "}";
        //        if (count != parameter.size() - 1)
        //        {
        //            val += ";";
        //        }

        //        if (val.length() >= 63000)
        //        {
        //            result.push_back(val);
        //            val = "";
        //        }
        //        count++;
        //    }
        //    result.push_back(val);
        //    return result;
        //}

        //private static TypeToChoose stringToMap(string parameter)
        //{
        //    uint pos = 0;
        //    List<string> states;
        //    while ((pos = parameter.find(";")) != std::string::npos) {
        //        states.Add(parameter.substr(0, pos));
        //        parameter.erase(0, pos + 1);
        //    }

        //    TypeToChoose result;
        //    for (int i = 0; i < states.Count; i++)
        //    {
        //        pos = states.at(i).find(":");
        //        int state = stoi(states.at(i).substr(0, pos));
        //        result.Add(std::pair<int, std::map<int, float>>(state, { }));
        //    int it = 0;
        //    while ((pos = states.at(i).find(",")) != std::string::npos) {
        //        std::string actionContent = states.at(i).substr(0, pos);
        //        if (it == 0)
        //        {
        //            size_t newPos = actionContent.find("{") + 1;
        //            actionContent = actionContent.substr(newPos, actionContent.length());
        //        }
        //        states.at(i).erase(0, pos + 1);
        //        pos = states.at(i).find(":");
        //        if (pos == std::string::npos) {
        //            pos = 0;
        //        }
        //            else
        //        {
        //            pos += 1;
        //        }
        //        //int action = stoi(actionContent.substr(0, pos));
        //        int action = it;
        //        float proba = stof(actionContent.substr(pos, actionContent.length()));
        //        //float proba = stof(actionContent.substr(pos + 1, actionContent.length()));
        //        result[state].insert(std::pair<int, float>(action, proba));
        //        it++;
        //    }
        //    std::string actionContent = states.at(i).substr(0, states.at(i).length());
        //    pos = states.at(i).find(":");
        //    if (pos == std::string::npos) {
        //        pos = 0;
        //    }
        //        else
        //    {
        //        pos += 1;
        //    }
        //    //int action = stoi(actionContent.substr(0, pos));
        //    int action = it;
        //    float proba = stof(actionContent.substr(pos, actionContent.length()));
        //    //float proba = stof(actionContent.substr(pos + 1, actionContent.length()));
        //    result[state].insert(std::pair<int, float>(action, proba));

        //    return result;
        //}

        private int randomChoice(Dictionary<int, float> probalities, List<int> availableActions)
        {
            Dictionary<int, float> new_probabilites = new Dictionary<int, float>();
            for (int a = 0; a < numberOfActions; a++)
            {
                bool isAvailable = false;
                for (int j = 0; j < availableActions.Count; j++)
                {
                    if (a == availableActions[j])
                    {
                        new_probabilites.Add(a, probalities[a]);
                        isAvailable = true;
                        break;
                    }
                }
                if (!isAvailable)
                {
                    new_probabilites.Add(a, -1.0f);
                }
            }
            return randomChoice(new_probabilites);
        }

        private int randomChoice(Dictionary<int, float> probalities)
        {
            Random rand = new Random();
            const int RAND_MAX = 32767;
            float rd = (rand.Next(RAND_MAX) / (RAND_MAX));
            float probality = 0.0f;
            foreach (var proba in probalities)
            {
                if (proba.Value > 0)
                {
                    probality += proba.Value;
                    if (probality >= rd)
                    {
                        return proba.Key;
                    }
                }
            }

            if (probality > 1)
            {
                throw new Exception("error in QLearning random choice return");
            }

            int actionToDo = rand.Next(probalities.Count);
            while (probalities[actionToDo] == -1.0)
            {
                actionToDo = rand.Next(probalities.Count);
            }
            return actionToDo;
        }
    }
}
